namespace CandyShopService.ViewModels
{
    public class WarehouseIngredientViewModel
    {
        public int Id { get; set; }

        public int WarehouseId { get; set; }

        public int IngredientId { get; set; }

        public string IngredientName { get; set; }

        public int Count { get; set; }
    }
}
