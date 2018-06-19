using System.Runtime.Serialization;

namespace CandyShopService.ViewModels
{
    [DataContract]
    public class PurchaseOrderViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public string CustomerFIO { get; set; }

        [DataMember]
        public int CandyId { get; set; }

        [DataMember]
        public string CandyName { get; set; }

        [DataMember]
        public int? ConfectionerId { get; set; }

        [DataMember]
        public string ConfectionerName { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public decimal Sum { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string DateCreate { get; set; }

        [DataMember]
        public string DateImplement { get; set; }
    }
}
