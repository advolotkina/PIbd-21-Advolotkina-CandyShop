namespace CandyShopService.ViewModels
{
    public class CandyIngredientViewModel
    {
        public int Id { get; set; }

        public int CandyId { get; set; }

        public int IngredientId { get; set; }

        public string IngredientName { get; set; }

        public int Count { get; set; }
    }
}
