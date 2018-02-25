using CandyShopModel;
using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CandyShopService.ImplementationsList
{
    public class CandyServiceList : ICandyService
    {
        private DataListSingleton source;

        public CandyServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<CandyViewModel> GetList()
        {
            List<CandyViewModel> result = source.Candies
                .Select(rec => new CandyViewModel
                {
                    Id = rec.Id,
                    CandyName = rec.CandyName,
                    Price = rec.Price,
                    CandyIngredients = source.CandyIngredients
                            .Where(recPC => recPC.CandyId == rec.Id)
                            .Select(recPC => new CandyIngredientViewModel
                            {
                                Id = recPC.Id,
                                CandyId = recPC.CandyId,
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

        public CandyViewModel GetElement(int id)
        {
            Candy element = source.Candies.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new CandyViewModel
                {
                    Id = element.Id,
                    CandyName = element.CandyName,
                    Price = element.Price,
                    CandyIngredients = source.CandyIngredients
                            .Where(recCC => recCC.CandyId == element.Id)
                            .Select(recCC => new CandyIngredientViewModel
                            {
                                Id = recCC.Id,
                                CandyId = recCC.CandyId,
                                IngredientId = recCC.IngredientId,
                                IngredientName = source.Ingredients
                                        .FirstOrDefault(recC => recC.Id == recCC.IngredientId)?.IngredientName,
                                Count = recCC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(CandyBindingModel model)
        {
            Candy element = source.Candies.FirstOrDefault(rec => rec.CandyName == model.CandyName);
            if (element != null)
            {
                throw new Exception("Уже есть сладость с таким названием");
            }
            int maxId = source.Candies.Count > 0 ? source.Candies.Max(rec => rec.Id) : 0;
            source.Candies.Add(new Candy
            {
                Id = maxId + 1,
                CandyName = model.CandyName,
                Price = model.Price
            });

            int maxCCId = source.CandyIngredients.Count > 0 ?
                                    source.CandyIngredients.Max(rec => rec.Id) : 0;

            var groupIngredients = model.CandyIngredients
                                        .GroupBy(rec => rec.IngredientId)
                                        .Select(rec => new
                                        {
                                            IngredientId = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });

            foreach (var groupIngredient in groupIngredients)
            {
                source.CandyIngredients.Add(new CandyIngredient
                {
                    Id = ++maxCCId,
                    CandyId = maxId + 1,
                    IngredientId = groupIngredient.IngredientId,
                    Count = groupIngredient.Count
                });
            }
        }

        public void UpdElement(CandyBindingModel model)
        {
            Candy element = source.Candies.FirstOrDefault(rec =>
                                        rec.CandyName == model.CandyName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть сладость с таким названием");
            }
            element = source.Candies.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.CandyName = model.CandyName;
            element.Price = model.Price;

            int maxPCId = source.CandyIngredients.Count > 0 ? source.CandyIngredients.Max(rec => rec.Id) : 0;

            var compIds = model.CandyIngredients.Select(rec => rec.IngredientId).Distinct();
            var updateIngredients = source.CandyIngredients
                                            .Where(rec => rec.CandyId == model.Id &&
                                           compIds.Contains(rec.IngredientId));
            foreach (var updateIngredient in updateIngredients)
            {
                updateIngredient.Count = model.CandyIngredients
                                                .FirstOrDefault(rec => rec.Id == updateIngredient.Id).Count;
            }
            source.CandyIngredients.RemoveAll(rec => rec.CandyId == model.Id &&
                                       !compIds.Contains(rec.IngredientId));

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
                CandyIngredient elementCC = source.CandyIngredients
                                        .FirstOrDefault(rec => rec.CandyId == model.Id &&
                                                        rec.IngredientId == groupIngredient.IngredientId);
                if (elementCC != null)
                {
                    elementCC.Count += groupIngredient.Count;
                }
                else
                {
                    source.CandyIngredients.Add(new CandyIngredient
                    {
                        Id = ++maxPCId,
                        CandyId = model.Id,
                        IngredientId = groupIngredient.IngredientId,
                        Count = groupIngredient.Count
                    });
                }
            }
        }

        public void DelElement(int id)
        {

            Candy element = source.Candies.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.CandyIngredients.RemoveAll(rec => rec.CandyId == id);
                source.Candies.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
