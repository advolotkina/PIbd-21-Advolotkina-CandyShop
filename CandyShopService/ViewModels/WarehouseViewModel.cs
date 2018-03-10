using System.Collections.Generic;

namespace CandyShopService.ViewModels
{
    public class WarehouseViewModel
    {
        public int Id { get; set; }

        public string WarehouseName { get; set; }

        public List<WarehouseIngredientViewModel> WarehouseIngredients { get; set; }
    }
}
