namespace CandyShopService.BindingModels
{
    public class CandyIngredientBindingModel
    {
        public int Id { get; set; }

        public int CandyId { get; set; }

        public int IngredientId { get; set; }

        public int Count { get; set; }
    }
}
