using System.Collections.Generic;

namespace CandyShopService.BindingModels
{
    public class CandyBindingModel
    {
        public int Id { get; set; }

        public string CandyName { get; set; }

        public decimal Price { get; set; }

        public List<CandyIngredientBindingModel> CandyIngredients { get; set; }
    }
}
