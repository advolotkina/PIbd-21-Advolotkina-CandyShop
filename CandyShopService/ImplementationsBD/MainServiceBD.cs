using CandyShopModel;
using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace CandyShopService.ImplementationsBD
{
    public class MainServiceBD: IMainService
    {
        private CandyShopDbContext context;

        public MainServiceBD(CandyShopDbContext context)
        {
            this.context = context;
        }

        public void CreatePurchaseOrder(PurchaseOrderBindingModel model)
        {
            var order = new PurchaseOrder
            {
                CustomerId = model.CustomerId,
                CandyId = model.CandyId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = PurchaseOrderStatus.Принят
            };
            context.PurchaseOrders.Add(order);
            context.SaveChanges();

            var customer = context.Customers.FirstOrDefault(x => x.Id == model.CustomerId);
            SendEmail(customer.Mail, "Оповещение по заказам",
            string.Format("Заказ №{0} от {1} создан успешно", order.Id,
            order.DateCreate.ToShortDateString()));
        }

        public void FinishPurchaseOrder(int id)
        {
            PurchaseOrder element = context.PurchaseOrders.Include(rec => rec.Customer).FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = PurchaseOrderStatus.Готов;
            context.SaveChanges();
            SendEmail(element.Customer.Mail, "Оповещение по заказам",
            string.Format("Заказ №{0} от {1} передан на оплату", element.Id,
                element.DateCreate.ToShortDateString()));
        }

        public List<PurchaseOrderViewModel> GetList()
        {
            List<PurchaseOrderViewModel> result = context.PurchaseOrders
                .Select(rec => new PurchaseOrderViewModel
                {
                    Id = rec.Id,
                    CustomerId = rec.CustomerId,
                    CandyId = rec.CandyId,
                    ConfectionerId = rec.ConfectionerId,
                    DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateCreate),
                    DateImplement = rec.DateImplement == null ? "" :
                                        SqlFunctions.DateName("dd", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("mm", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("yyyy", rec.DateImplement.Value),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    CustomerFIO = rec.Customer.CustomerFIO,
                    CandyName = rec.Candy.CandyName,
                    ConfectionerName = rec.Confectioner.ConfectionerFIO
                })
                .ToList();
            return result;
        }

        public void PayPurchaseOrder(int id)
        {
            PurchaseOrder element = context.PurchaseOrders.Include(rec => rec.Customer).FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = PurchaseOrderStatus.Оплачен;
            context.SaveChanges();
            SendEmail(element.Customer.Mail, "Оповещение по заказам",
                string.Format("Заказ №{0} от {1} оплачен успешно", element.Id, element.DateCreate.ToShortDateString()));
        }

        public void PutIngredientOnStock(WarehouseIngredientBindingModel model)
        {
            WarehouseIngredient element = context.WarehouseIngredients
                                                .FirstOrDefault(rec => rec.WarehouseId == model.WarehouseId &&
                                                                    rec.IngredientId == model.IngredientId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                context.WarehouseIngredients.Add(new WarehouseIngredient
                {
                    WarehouseId = model.WarehouseId,
                    IngredientId = model.IngredientId,
                    Count = model.Count
                });
            }
            context.SaveChanges();
        }

        public void TakePurchaseOrderInWork(PurchaseOrderBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    PurchaseOrder element = context.PurchaseOrders.Include(rec => rec.Customer).FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    var candyIngredients = context.CandyIngredients
                                                .Include(rec => rec.Ingredient)
                                                .Where(rec => rec.CandyId == element.CandyId);
                    
                    foreach (var candyIngredient in candyIngredients)
                    {
                        int countOnStocks = candyIngredient.Count * element.Count;
                        var stockComponents = context.WarehouseIngredients
                                                    .Where(rec => rec.IngredientId == candyIngredient.IngredientId);
                        foreach (var warehouseIngredient in stockComponents)
                        {
                            
                            if (warehouseIngredient.Count >= countOnStocks)
                            {
                                warehouseIngredient.Count -= countOnStocks;
                                countOnStocks = 0;
                                context.SaveChanges();
                                break;
                            }
                            else
                            {
                                countOnStocks -= warehouseIngredient.Count;
                                warehouseIngredient.Count = 0;
                                context.SaveChanges();
                            }
                        }
                        if (countOnStocks > 0)
                        {
                            throw new Exception("Не достаточно компонента " +
                                candyIngredient.Ingredient.IngredientName + " требуется " +
                                candyIngredient.Count + ", не хватает " + countOnStocks);
                        }
                    }
                    element.ConfectionerId = model.ConfectionerId;
                    element.DateImplement = DateTime.Now;
                    element.Status = PurchaseOrderStatus.Выполняется;
                    context.SaveChanges();
                    SendEmail(element.Customer.Mail, "Оповещение по заказам",
                        string.Format("Заказ №{0} от {1} передеан в работу", element.Id, element.DateCreate.ToShortDateString()));
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private void SendEmail(string mailAddress, string subject, string text)
        {
            MailMessage objMailMessage = new MailMessage();
            SmtpClient objSmtpClient = null;

            try
            {
                objMailMessage.From = new MailAddress(ConfigurationManager.AppSettings["MailLogin"]);
                objMailMessage.To.Add(new MailAddress(mailAddress));
                objMailMessage.Subject = subject;
                objMailMessage.Body = text;
                objMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                objSmtpClient = new SmtpClient("smtp.gmail.com", 587);
                objSmtpClient.UseDefaultCredentials = false;
                objSmtpClient.EnableSsl = true;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailLogin"],
                    ConfigurationManager.AppSettings["MailPassword"]);

                objSmtpClient.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objMailMessage = null;
                objSmtpClient = null;
            }
        }
    }
}
