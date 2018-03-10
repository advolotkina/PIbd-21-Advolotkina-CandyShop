using System.Collections.Generic;

namespace CandyShopService.ViewModels
{
    public class CandyViewModel
    {
        public int Id { get; set; }

        public string CandyName { get; set; }

        public decimal Price { get; set; }

        public List<CandyIngredientViewModel> CandyIngredients { get; set; }
    }
}
