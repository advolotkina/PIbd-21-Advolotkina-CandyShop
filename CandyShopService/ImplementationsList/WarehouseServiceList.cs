using CandyShopModel;
using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;

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
            List<WarehouseViewModel> result = new List<WarehouseViewModel>();
            for (int i = 0; i < source.Warehouses.Count; ++i)
            {
                
                List<WarehouseIngredientViewModel> WarehouseIngredients = new List<WarehouseIngredientViewModel>();
                for (int j = 0; j < source.WarehouseIngredients.Count; ++j)
                {
                    if (source.WarehouseIngredients[j].WarehouseId == source.Warehouses[i].Id)
                    {
                        string ingredientName = string.Empty;
                        for (int k = 0; k < source.Ingredients.Count; ++k)
                        {
                            if (source.WarehouseIngredients[j].IngredientId == source.Ingredients[k].Id)
                            {
                                ingredientName = source.Ingredients[k].IngredientName;
                                break;
                            }
                        }
                        WarehouseIngredients.Add(new WarehouseIngredientViewModel
                        {
                            Id = source.WarehouseIngredients[j].Id,
                            WarehouseId = source.WarehouseIngredients[j].WarehouseId,
                            IngredientId = source.WarehouseIngredients[j].IngredientId,
                            IngredientName = ingredientName,
                            Count = source.WarehouseIngredients[j].Count
                        });
                    }
                }
                result.Add(new WarehouseViewModel
                {
                    Id = source.Warehouses[i].Id,
                    WarehouseName = source.Warehouses[i].WarehouseName,
                    WarehouseIngredients = WarehouseIngredients
                });
            }
            return result;
        }

        public WarehouseViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Warehouses.Count; ++i)
            {
             
                List<WarehouseIngredientViewModel> WarehouseIngredients = new List<WarehouseIngredientViewModel>();
                for (int j = 0; j < source.WarehouseIngredients.Count; ++j)
                {
                    if (source.WarehouseIngredients[j].WarehouseId == source.Warehouses[i].Id)
                    {
                        string ingredientName = string.Empty;
                        for (int k = 0; k < source.Ingredients.Count; ++k)
                        {
                            if (source.CandyIngredients[j].IngredientId == source.Ingredients[k].Id)
                            {
                                ingredientName = source.Ingredients[k].IngredientName;
                                break;
                            }
                        }
                        WarehouseIngredients.Add(new WarehouseIngredientViewModel
                        {
                            Id = source.WarehouseIngredients[j].Id,
                            WarehouseId = source.WarehouseIngredients[j].WarehouseId,
                            IngredientId = source.WarehouseIngredients[j].IngredientId,
                            IngredientName = ingredientName,
                            Count = source.WarehouseIngredients[j].Count
                        });
                    }
                }
                if (source.Warehouses[i].Id == id)
                {
                    return new WarehouseViewModel
                    {
                        Id = source.Warehouses[i].Id,
                        WarehouseName = source.Warehouses[i].WarehouseName,
                        WarehouseIngredients = WarehouseIngredients
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(WarehouseBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Warehouses.Count; ++i)
            {
                if (source.Warehouses[i].Id > maxId)
                {
                    maxId = source.Warehouses[i].Id;
                }
                if (source.Warehouses[i].WarehouseName == model.WarehouseName)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            source.Warehouses.Add(new Warehouse
            {
                Id = maxId + 1,
                WarehouseName = model.WarehouseName
            });
        }

        public void UpdElement(WarehouseBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Warehouses.Count; ++i)
            {
                if (source.Warehouses[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Warehouses[i].WarehouseName == model.WarehouseName && 
                    source.Warehouses[i].Id != model.Id)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Warehouses[index].WarehouseName = model.WarehouseName;
        }

        public void DelElement(int id)
        {
          
            for (int i = 0; i < source.WarehouseIngredients.Count; ++i)
            {
                if (source.WarehouseIngredients[i].WarehouseId == id)
                {
                    source.WarehouseIngredients.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Warehouses.Count; ++i)
            {
                if (source.Warehouses[i].Id == id)
                {
                    source.Warehouses.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
