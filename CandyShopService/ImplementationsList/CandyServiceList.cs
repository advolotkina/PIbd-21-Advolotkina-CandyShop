using CandyShopModel;
using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;

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
            List<CandyViewModel> result = new List<CandyViewModel>();
            for (int i = 0; i < source.Candies.Count; ++i)
            {
                
                List<CandyIngredientViewModel> candyIngredients = new List<CandyIngredientViewModel>();
                for (int j = 0; j < source.CandyIngredients.Count; ++j)
                {
                    if (source.CandyIngredients[j].CandyId == source.Candies[i].Id)
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
                        candyIngredients.Add(new CandyIngredientViewModel
                        {
                            Id = source.CandyIngredients[j].Id,
                            CandyId = source.CandyIngredients[j].CandyId,
                            IngredientId = source.CandyIngredients[j].IngredientId,
                            IngredientName = ingredientName,
                            Count = source.CandyIngredients[j].Count
                        });
                    }
                }
                result.Add(new CandyViewModel
                {
                    Id = source.Candies[i].Id,
                    CandyName = source.Candies[i].CandyName,
                    Price = source.Candies[i].Price,
                    CandyIngredients = candyIngredients
                });
            }
            return result;
        }

        public CandyViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Candies.Count; ++i)
            {
                
                List<CandyIngredientViewModel> candyIngredients = new List<CandyIngredientViewModel>();
                for (int j = 0; j < source.CandyIngredients.Count; ++j)
                {
                    if (source.CandyIngredients[j].CandyId == source.Candies[i].Id)
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
                        candyIngredients.Add(new CandyIngredientViewModel
                        {
                            Id = source.CandyIngredients[j].Id,
                            CandyId = source.CandyIngredients[j].CandyId,
                            IngredientId = source.CandyIngredients[j].IngredientId,
                            IngredientName = ingredientName,
                            Count = source.CandyIngredients[j].Count
                        });
                    }
                }
                if (source.Candies[i].Id == id)
                {
                    return new CandyViewModel
                    {
                        Id = source.Candies[i].Id,
                        CandyName = source.Candies[i].CandyName,
                        Price = source.Candies[i].Price,
                        CandyIngredients = candyIngredients
                    };
                }
            }

            throw new Exception("Элемент не найден");
        }

        public void AddElement(CandyBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Candies.Count; ++i)
            {
                if (source.Candies[i].Id > maxId)
                {
                    maxId = source.Candies[i].Id;
                }
                if (source.Candies[i].CandyName == model.CandyName)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            source.Candies.Add(new Candy
            {
                Id = maxId + 1,
                CandyName = model.CandyName,
                Price = model.Price
            });
            
            int maxPCId = 0;
            for (int i = 0; i < source.CandyIngredients.Count; ++i)
            {
                if (source.CandyIngredients[i].Id > maxPCId)
                {
                    maxPCId = source.CandyIngredients[i].Id;
                }
            }
            
            for (int i = 0; i < model.CandyIngredients.Count; ++i)
            {
                for (int j = 1; j < model.CandyIngredients.Count; ++j)
                {
                    if(model.CandyIngredients[i].IngredientId ==
                        model.CandyIngredients[j].IngredientId)
                    {
                        model.CandyIngredients[i].Count +=
                            model.CandyIngredients[j].Count;
                        model.CandyIngredients.RemoveAt(j--);
                    }
                }
            }
            
            for (int i = 0; i < model.CandyIngredients.Count; ++i)
            {
                source.CandyIngredients.Add(new CandyIngredient
                {
                    Id = ++maxPCId,
                    CandyId = maxId + 1,
                    IngredientId = model.CandyIngredients[i].IngredientId,
                    Count = model.CandyIngredients[i].Count
                });
            }
        }

        public void UpdElement(CandyBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Candies.Count; ++i)
            {
                if (source.Candies[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Candies[i].CandyName == model.CandyName && 
                    source.Candies[i].Id != model.Id)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Candies[index].CandyName = model.CandyName;
            source.Candies[index].Price = model.Price;
            int maxPCId = 0;
            for (int i = 0; i < source.CandyIngredients.Count; ++i)
            {
                if (source.CandyIngredients[i].Id > maxPCId)
                {
                    maxPCId = source.CandyIngredients[i].Id;
                }
            }
            
            for (int i = 0; i < source.CandyIngredients.Count; ++i)
            {
                if (source.CandyIngredients[i].CandyId == model.Id)
                {
                    bool flag = true;
                    for (int j = 0; j < model.CandyIngredients.Count; ++j)
                    {
                        
                        if (source.CandyIngredients[i].Id == model.CandyIngredients[j].Id)
                        {
                            source.CandyIngredients[i].Count = model.CandyIngredients[j].Count;
                            flag = false;
                            break;
                        }
                    }
                    
                    if(flag)
                    {
                        source.CandyIngredients.RemoveAt(i--);
                    }
                }
            }
            
            for(int i = 0; i < model.CandyIngredients.Count; ++i)
            {
                if(model.CandyIngredients[i].Id == 0)
                {
                    
                    for (int j = 0; j < source.CandyIngredients.Count; ++j)
                    {
                        if (source.CandyIngredients[j].CandyId == model.Id &&
                            source.CandyIngredients[j].IngredientId == model.CandyIngredients[i].IngredientId)
                        {
                            source.CandyIngredients[j].Count += model.CandyIngredients[i].Count;
                            model.CandyIngredients[i].Id = source.CandyIngredients[j].Id;
                            break;
                        }
                    }
                    
                    if (model.CandyIngredients[i].Id == 0)
                    {
                        source.CandyIngredients.Add(new CandyIngredient
                        {
                            Id = ++maxPCId,
                            CandyId = model.Id,
                            IngredientId = model.CandyIngredients[i].IngredientId,
                            Count = model.CandyIngredients[i].Count
                        });
                    }
                }
            }
        }

        public void DelElement(int id)
        {
            
            for (int i = 0; i < source.CandyIngredients.Count; ++i)
            {
                if (source.CandyIngredients[i].CandyId == id)
                {
                    source.CandyIngredients.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Candies.Count; ++i)
            {
                if (source.Candies[i].Id == id)
                {
                    source.Candies.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
