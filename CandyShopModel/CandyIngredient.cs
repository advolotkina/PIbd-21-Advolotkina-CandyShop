namespace CandyShopModel
{
    public class CandyIngredient
    {
        public int Id { get; set; }

        public int CandyId { get; set; }

        public int IngredientId { get; set; }

        public int Count { get; set; }

        public virtual Candy Candy { get; set; }

        public virtual Ingredient Ingredient { get; set; }
    }
}
