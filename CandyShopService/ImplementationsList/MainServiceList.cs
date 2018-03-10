using CandyShopModel;
using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CandyShopService.ImplementationsList
{
    public class MainServiceList : IMainService
    {
        private DataListSingleton source;

        public MainServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<PurchaseOrderViewModel> GetList()
        {
            List<PurchaseOrderViewModel> result = source.PurchaseOrders
                .Select(rec => new PurchaseOrderViewModel
                {
                    Id = rec.Id,
                    CustomerId = rec.CustomerId,
                    CandyId = rec.CandyId,
                    ConfectionerId = rec.ConfectionerId,
                    DateCreate = rec.DateCreate.ToLongDateString(),
                    DateImplement = rec.DateImplement?.ToLongDateString(),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    CustomerFIO = source.Customers
                                    .FirstOrDefault(recC => recC.Id == rec.CustomerId)?.CustomerFIO,
                    CandyName = source.Candies
                                    .FirstOrDefault(recP => recP.Id == rec.CandyId)?.CandyName,
                    ConfectionerName = source.Confectioners
                                    .FirstOrDefault(recI => recI.Id == rec.ConfectionerId)?.ConfectionerFIO
                })
                .ToList();
            return result;
        }

        public void CreatePurchaseOrder(PurchaseOrderBindingModel model)
        {
            int maxId = source.PurchaseOrders.Count > 0 ? source.PurchaseOrders.Max(rec => rec.Id) : 0;
            source.PurchaseOrders.Add(new PurchaseOrder
            {
                Id = maxId + 1,
                CustomerId = model.CustomerId,
                CandyId = model.CandyId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = PurchaseOrderStatus.Принят
            });
        }

        public void TakePurchaseOrderInWork(PurchaseOrderBindingModel model)
        {
            PurchaseOrder element = source.PurchaseOrders.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }

            var candyIngredients = source.CandyIngredients.Where(rec => rec.CandyId == element.CandyId);
            foreach (var candyIngredient in candyIngredients)
            {
                int countOnWarehouses = source.WarehouseIngredients
                                            .Where(rec => rec.IngredientId == candyIngredient.IngredientId)
                                            .Sum(rec => rec.Count);
                if (countOnWarehouses < candyIngredient.Count * element.Count)
                {
                    var ingredientName = source.Ingredients
                                    .FirstOrDefault(rec => rec.Id == candyIngredient.IngredientId);
                    throw new Exception("Недостаточно ингредиентов " + ingredientName?.IngredientName +
                        " требуется " + candyIngredient.Count + ", в наличии " + countOnWarehouses);
                }
            }

            foreach (var candyIngredient in candyIngredients)
            {
                int countOnWarehouses = candyIngredient.Count * element.Count;
                var warehouseIngredients = source.WarehouseIngredients
                                            .Where(rec => rec.IngredientId == candyIngredient.IngredientId);
                foreach (var warehouseIngredient in warehouseIngredients)
                {

                    if (warehouseIngredient.Count >= countOnWarehouses)
                    {
                        warehouseIngredient.Count -= countOnWarehouses;
                        break;
                    }
                    else
                    {
                        countOnWarehouses -= warehouseIngredient.Count;
                        warehouseIngredient.Count = 0;
                    }
                }
            }
            element.ConfectionerId = model.ConfectionerId;
            element.DateImplement = DateTime.Now;
            element.Status = PurchaseOrderStatus.Выполняется;
        }

        public void FinishPurchaseOrder(int id)
        {
            PurchaseOrder element = source.PurchaseOrders.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = PurchaseOrderStatus.Готов;
        }

        public void PayPurchaseOrder(int id)
        {
            PurchaseOrder element = source.PurchaseOrders.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = PurchaseOrderStatus.Оплачен;
        }

        public void PutIngredientOnStock(WarehouseIngredientBindingModel model)
        {
            WarehouseIngredient element = source.WarehouseIngredients
                                                .FirstOrDefault(rec => rec.WarehouseId == model.WarehouseId &&
                                                                    rec.IngredientId == model.IngredientId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                int maxId = source.WarehouseIngredients.Count > 0 ? source.WarehouseIngredients.Max(rec => rec.Id) : 0;
                source.WarehouseIngredients.Add(new WarehouseIngredient
                {
                    Id = ++maxId,
                    WarehouseId = model.WarehouseId,
                    IngredientId = model.IngredientId,
                    Count = model.Count
                });
            }
        }
    }
}
