using CandyShopModel;
using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;

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
            List<PurchaseOrderViewModel> result = new List<PurchaseOrderViewModel>();
            for (int i = 0; i < source.PurchaseOrders.Count; ++i)
            {
                string clientFIO = string.Empty;
                for (int j = 0; j < source.Customers.Count; ++j)
                {
                    if(source.Customers[j].Id == source.PurchaseOrders[i].CustomerId)
                    {
                        clientFIO = source.Customers[j].CustomerFIO;
                        break;
                    }
                }
                string productName = string.Empty;
                for (int j = 0; j < source.Candies.Count; ++j)
                {
                    if (source.Candies[j].Id == source.PurchaseOrders[i].CandyId)
                    {
                        productName = source.Candies[j].CandyName;
                        break;
                    }
                }
                string implementerFIO = string.Empty;
                if(source.PurchaseOrders[i].ConfectionerId.HasValue)
                {
                    for (int j = 0; j < source.Confectioners.Count; ++j)
                    {
                        if (source.Confectioners[j].Id == source.PurchaseOrders[i].ConfectionerId.Value)
                        {
                            implementerFIO = source.Confectioners[j].ConfectionerFIO;
                            break;
                        }
                    }
                }
                result.Add(new PurchaseOrderViewModel
                {
                    Id = source.PurchaseOrders[i].Id,
                    CustomerId = source.PurchaseOrders[i].CustomerId,
                    CustomerFIO = clientFIO,
                    CandyId = source.PurchaseOrders[i].CandyId,
                    CandyName = productName,
                    ConfectionerId = source.PurchaseOrders[i].ConfectionerId,
                    ConfectionerName = implementerFIO,
                    Count = source.PurchaseOrders[i].Count,
                    Sum = source.PurchaseOrders[i].Sum,
                    DateCreate = source.PurchaseOrders[i].DateCreate.ToLongDateString(),
                    DateImplement = source.PurchaseOrders[i].DateImplement?.ToLongDateString(),
                    Status = source.PurchaseOrders[i].Status.ToString()
                });
            }
            return result;
        }

        public void CreatePurchaseOrder(PurchaseOrderBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.PurchaseOrders.Count; ++i)
            {
                if (source.PurchaseOrders[i].Id > maxId)
                {
                    maxId = source.Customers[i].Id;
                }
            }
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
            int index = -1;
            for (int i = 0; i < source.PurchaseOrders.Count; ++i)
            {
                if (source.PurchaseOrders[i].Id == model.Id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            
            for(int i = 0; i < source.CandyIngredients.Count; ++i)
            {
                if(source.CandyIngredients[i].CandyId == source.PurchaseOrders[index].CandyId)
                {
                    int countOnStocks = 0;
                    for(int j = 0; j < source.WarehouseIngredients.Count; ++j)
                    {
                        if(source.WarehouseIngredients[j].IngredientId == source.CandyIngredients[i].IngredientId)
                        {
                            countOnStocks += source.WarehouseIngredients[j].Count;
                        }
                    }
                    if(countOnStocks < source.CandyIngredients[i].Count * source.PurchaseOrders[index].Count)
                    {
                        for (int j = 0; j < source.Ingredients.Count; ++j)
                        {
                            if (source.Ingredients[j].Id == source.CandyIngredients[i].IngredientId)
                            {
                                throw new Exception("Не достаточно компонента " + source.Ingredients[j].IngredientName + 
                                    " требуется " + source.CandyIngredients[i].Count + ", в наличии " + countOnStocks);
                            }
                        }
                    }
                }
            }
            
            for (int i = 0; i < source.CandyIngredients.Count; ++i)
            {
                if (source.CandyIngredients[i].CandyId == source.PurchaseOrders[index].CandyId)
                {
                    int countOnStocks = source.CandyIngredients[i].Count * source.PurchaseOrders[index].Count;
                    for (int j = 0; j < source.WarehouseIngredients.Count; ++j)
                    {
                        if (source.WarehouseIngredients[j].IngredientId == source.CandyIngredients[i].IngredientId)
                        {
                            
                            if (source.WarehouseIngredients[j].Count >= countOnStocks)
                            {
                                source.WarehouseIngredients[j].Count -= countOnStocks;
                                break;
                            }
                            else
                            {
                                countOnStocks -= source.WarehouseIngredients[j].Count;
                                source.WarehouseIngredients[j].Count = 0;
                            }
                        }
                    }
                }
            }
            source.PurchaseOrders[index].ConfectionerId = model.ConfectionerId;
            source.PurchaseOrders[index].DateImplement = DateTime.Now;
            source.PurchaseOrders[index].Status = PurchaseOrderStatus.Выполняется;
        }

        public void FinishPurchaseOrder(int id)
        {
            int index = -1;
            for (int i = 0; i < source.PurchaseOrders.Count; ++i)
            {
                if (source.Customers[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.PurchaseOrders[index].Status = PurchaseOrderStatus.Готов;
        }

        public void PayPurchaseOrder(int id)
        {
            int index = -1;
            for (int i = 0; i < source.PurchaseOrders.Count; ++i)
            {
                if (source.Customers[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.PurchaseOrders[index].Status = PurchaseOrderStatus.Оплачен;
        }

        public void PutIngredientOnStock(WarehouseIngredientBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.WarehouseIngredients.Count; ++i)
            {
                if(source.WarehouseIngredients[i].WarehouseId == model.WarehouseId && 
                    source.WarehouseIngredients[i].IngredientId == model.IngredientId)
                {
                    source.WarehouseIngredients[i].Count += model.Count;
                    return;
                }
                if (source.WarehouseIngredients[i].Id > maxId)
                {
                    maxId = source.WarehouseIngredients[i].Id;
                }
            }
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
