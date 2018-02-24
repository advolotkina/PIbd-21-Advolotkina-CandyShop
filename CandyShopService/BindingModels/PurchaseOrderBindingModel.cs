namespace CandyShopService.BindingModels
{
    public class PurchaseOrderBindingModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int CandyId { get; set; }

        public int? ConfectionerId { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }
    }
}
