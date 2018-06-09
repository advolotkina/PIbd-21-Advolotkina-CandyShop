using CandyShopModel;
using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CandyShopService.ImplementationsBD
{
    public class WarehouseServiceBD: IWarehouseService
    {
        private CandyShopDbContext context;

        public WarehouseServiceBD(CandyShopDbContext context)
        {
            this.context = context;
        }

        public void AddElement(WarehouseBindingModel model)
        {
            Warehouse element = context.Warehouses.FirstOrDefault(rec => rec.WarehouseName == model.WarehouseName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            context.Warehouses.Add(new Warehouse
            {
                WarehouseName = model.WarehouseName
            });
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Warehouse element = context.Warehouses.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        context.WarehouseIngredients.RemoveRange(
                                            context.WarehouseIngredients.Where(rec => rec.WarehouseId == id));
                        context.Warehouses.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public WarehouseViewModel GetElement(int id)
        {
            Warehouse element = context.Warehouses.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new WarehouseViewModel
                {
                    Id = element.Id,
                    WarehouseName = element.WarehouseName,
                    WarehouseIngredients = context.WarehouseIngredients
                            .Where(recPC => recPC.WarehouseId == element.Id)
                            .Select(recPC => new WarehouseIngredientViewModel
                            {
                                Id = recPC.Id,
                                WarehouseId = recPC.WarehouseId,
                                IngredientId = recPC.IngredientId,
                                IngredientName = recPC.Ingredient.IngredientName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public List<WarehouseViewModel> GetList()
        {
            List<WarehouseViewModel> result = context.Warehouses
                .Select(rec => new WarehouseViewModel
                {
                    Id = rec.Id,
                    WarehouseName = rec.WarehouseName,
                    WarehouseIngredients = context.WarehouseIngredients
                            .Where(recPC => recPC.WarehouseId == rec.Id)
                            .Select(recPC => new WarehouseIngredientViewModel
                            {
                                Id = recPC.Id,
                                WarehouseId = recPC.WarehouseId,
                                IngredientId = recPC.IngredientId,
                                IngredientName = recPC.Ingredient.IngredientName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public void UpdElement(WarehouseBindingModel model)
        {
            Warehouse element = context.Warehouses.FirstOrDefault(rec =>
                                        rec.WarehouseName == model.WarehouseName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = context.Warehouses.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.WarehouseName = model.WarehouseName;
            context.SaveChanges();
        }
    }
}
