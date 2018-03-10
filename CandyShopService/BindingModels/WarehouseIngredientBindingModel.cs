namespace CandyShopService.BindingModels
{
    public class WarehouseIngredientBindingModel
    {
        public int Id { get; set; }

        public int WarehouseId { get; set; }

        public int IngredientId { get; set; }

        public int Count { get; set; }
    }
}
