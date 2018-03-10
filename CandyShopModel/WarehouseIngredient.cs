namespace CandyShopModel
{
    public class WarehouseIngredient
    {
        public int Id { get; set; }

        public int WarehouseId { get; set; }

        public int IngredientId { get; set; }

        public int Count { get; set; }

        public virtual Warehouse Warehouse { get; set; }

        public virtual Ingredient Ingredient { get; set; }
    }
}
