using CandyShopModel;
using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CandyShopService.ImplementationsList
{
    public class WarehouseServiceList : IWarehouseService
    {
        private DataListSingleton source;

        public WarehouseServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<WarehouseViewModel> GetList()
        {
            List<WarehouseViewModel> result = source.Warehouses
                .Select(rec => new WarehouseViewModel
                {
                    Id = rec.Id,
                    WarehouseName = rec.WarehouseName,
                    WarehouseIngredients = source.WarehouseIngredients
                            .Where(recPC => recPC.WarehouseId == rec.Id)
                            .Select(recPC => new WarehouseIngredientViewModel
                            {
                                Id = recPC.Id,
                                WarehouseId = recPC.WarehouseId,
                                IngredientId = recPC.IngredientId,
                                IngredientName = source.Ingredients
                                    .FirstOrDefault(recC => recC.Id == recPC.IngredientId)?.IngredientName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public WarehouseViewModel GetElement(int id)
        {
            Warehouse element = source.Warehouses.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new WarehouseViewModel
                {
                    Id = element.Id,
                    WarehouseName = element.WarehouseName,
                    WarehouseIngredients = source.WarehouseIngredients
                            .Where(recPC => recPC.WarehouseId == element.Id)
                            .Select(recPC => new WarehouseIngredientViewModel
                            {
                                Id = recPC.Id,
                                WarehouseId = recPC.WarehouseId,
                                IngredientId = recPC.IngredientId,
                                IngredientName = source.Ingredients
                                    .FirstOrDefault(recC => recC.Id == recPC.IngredientId)?.IngredientName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(WarehouseBindingModel model)
        {
            Warehouse element = source.Warehouses.FirstOrDefault(rec => rec.WarehouseName == model.WarehouseName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            int maxId = source.Warehouses.Count > 0 ? source.Warehouses.Max(rec => rec.Id) : 0;
            source.Warehouses.Add(new Warehouse
            {
                Id = maxId + 1,
                WarehouseName = model.WarehouseName
            });
        }

        public void UpdElement(WarehouseBindingModel model)
        {
            Warehouse element = source.Warehouses.FirstOrDefault(rec =>
                                        rec.WarehouseName == model.WarehouseName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = source.Warehouses.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.WarehouseName = model.WarehouseName;
        }

        public void DelElement(int id)
        {
            Warehouse element = source.Warehouses.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.WarehouseIngredients.RemoveAll(rec => rec.WarehouseId == id);
                source.Warehouses.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
