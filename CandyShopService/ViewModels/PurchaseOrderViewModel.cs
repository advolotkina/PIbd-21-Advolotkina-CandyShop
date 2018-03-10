namespace CandyShopService.ViewModels
{
    public class PurchaseOrderViewModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string CustomerFIO { get; set; }

        public int CandyId { get; set; }

        public string CandyName { get; set; }

        public int? ConfectionerId { get; set; }

        public string ConfectionerName { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public string Status { get; set; }

        public string DateCreate { get; set; }

        public string DateImplement { get; set; }
    }
}
