using CandyShopModel;
using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CandyShopService.ImplementationsList
{
    public class IngredientServiceList : IIngredientService
    {
        private DataListSingleton source;

        public IngredientServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<IngredientViewModel> GetList()
        {
            List<IngredientViewModel> result = source.Ingredients
                .Select(rec => new IngredientViewModel
                {
                    Id = rec.Id,
                    IngredientName = rec.IngredientName
                })
                .ToList();
            return result;
        }

        public IngredientViewModel GetElement(int id)
        {
            Ingredient element = source.Ingredients.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new IngredientViewModel
                {
                    Id = element.Id,
                    IngredientName = element.IngredientName
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(IngredientBindingModel model)
        {
            Ingredient element = source.Ingredients.FirstOrDefault(rec => rec.IngredientName == model.IngredientName);
            if (element != null)
            {
                throw new Exception("Уже есть ингредиент с таким названием");
            }
            int maxId = source.Ingredients.Count > 0 ? source.Ingredients.Max(rec => rec.Id) : 0;
            source.Ingredients.Add(new Ingredient
            {
                Id = maxId + 1,
                IngredientName = model.IngredientName
            });
        }

        public void UpdElement(IngredientBindingModel model)
        {
            Ingredient element = source.Ingredients.FirstOrDefault(rec =>
                                        rec.IngredientName == model.IngredientName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть ингредиент с таким названием");
            }
            element = source.Ingredients.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.IngredientName = model.IngredientName;
        }

        public void DelElement(int id)
        {
            Ingredient element = source.Ingredients.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Ingredients.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
