using CandyShopModel;
using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CandyShopService.ImplementationsBD
{
    public class CandyServiceBD : ICandyService
    {
        private CandyShopDbContext context;

        public CandyServiceBD(CandyShopDbContext context)
        {
            this.context = context;
        }

        public void AddElement(CandyBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Candy element = context.Candies.FirstOrDefault(rec => rec.CandyName == model.CandyName);
                    if (element != null)
                    {
                        throw new Exception("Уже есть сладость с таким названием");
                    }
                    element = new Candy
                    {
                        CandyName = model.CandyName,
                        Price = model.Price
                    };
                    context.Candies.Add(element);
                    context.SaveChanges();
                    var groupIngredients = model.CandyIngredients
                                                .GroupBy(rec => rec.IngredientId)
                                                .Select(rec => new
                                                {
                                                    IngredientId = rec.Key,
                                                    Count = rec.Sum(r => r.Count)
                                                });
                    
                    foreach (var groupIngredient in groupIngredients)
                    {
                        context.CandyIngredients.Add(new CandyIngredient
                        {
                            CandyId = element.Id,
                            IngredientId = groupIngredient.IngredientId,
                            Count = groupIngredient.Count
                        });
                        context.SaveChanges();
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

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Candy element = context.Candies.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        context.CandyIngredients.RemoveRange(
                                            context.CandyIngredients.Where(rec => rec.CandyId == id));
                        context.Candies.Remove(element);
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

        public CandyViewModel GetElement(int id)
        {
            Candy element = context.Candies.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new CandyViewModel
                {
                    Id = element.Id,
                    CandyName = element.CandyName,
                    Price = element.Price,
                    CandyIngredients = context.CandyIngredients
                            .Where(recPC => recPC.CandyId == element.Id)
                            .Select(recPC => new CandyIngredientViewModel
                            {
                                Id = recPC.Id,
                                CandyId = recPC.CandyId,
                                IngredientId = recPC.IngredientId,
                                IngredientName = recPC.Ingredient.IngredientName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public List<CandyViewModel> GetList()
        {
            List<CandyViewModel> result = context.Candies
                .Select(rec => new CandyViewModel
                {
                    Id = rec.Id,
                    CandyName = rec.CandyName,
                    Price = rec.Price,
                    CandyIngredients = context.CandyIngredients
                            .Where(recPC => recPC.CandyId == rec.Id)
                            .Select(recPC => new CandyIngredientViewModel
                            {
                                Id = recPC.Id,
                                CandyId = recPC.CandyId,
                                IngredientId = recPC.IngredientId,
                                IngredientName = recPC.Ingredient.IngredientName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public void UpdElement(CandyBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Candy element = context.Candies.FirstOrDefault(rec =>
                                        rec.CandyName == model.CandyName && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть сладость с таким названием");
                    }
                    element = context.Candies.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.CandyName = model.CandyName;
                    element.Price = model.Price;
                    context.SaveChanges();

                    
                    var ingrIds = model.CandyIngredients.Select(rec => rec.IngredientId).Distinct();
                    var updateIngredients = context.CandyIngredients
                                                    .Where(rec => rec.CandyId == model.Id &&
                                                        ingrIds.Contains(rec.IngredientId));
                    foreach (var updateIngredient in updateIngredients)
                    {
                        updateIngredient.Count = model.CandyIngredients
                                                        .FirstOrDefault(rec => rec.Id == updateIngredient.Id).Count;
                    }
                    context.SaveChanges();
                    context.CandyIngredients.RemoveRange(
                                        context.CandyIngredients.Where(rec => rec.CandyId == model.Id &&
                                                                            !ingrIds.Contains(rec.IngredientId)));
                    context.SaveChanges();
                    
                    var groupIngredients = model.CandyIngredients
                                                .Where(rec => rec.Id == 0)
                                                .GroupBy(rec => rec.IngredientId)
                                                .Select(rec => new
                                                {
                                                    IngredientId = rec.Key,
                                                    Count = rec.Sum(r => r.Count)
                                                });
                    foreach (var groupIngredient in groupIngredients)
                    {
                        CandyIngredient elementPC = context.CandyIngredients
                                                .FirstOrDefault(rec => rec.CandyId == model.Id &&
                                                                rec.IngredientId == groupIngredient.IngredientId);
                        if (elementPC != null)
                        {
                            elementPC.Count += groupIngredient.Count;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.CandyIngredients.Add(new CandyIngredient
                            {
                                CandyId = model.Id,
                                IngredientId = groupIngredient.IngredientId,
                                Count = groupIngredient.Count
                            });
                            context.SaveChanges();
                        }
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
    }
}
