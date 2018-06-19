using System.Runtime.Serialization;
namespace CandyShopService.BindingModels
{
    [DataContract]
    public class PurchaseOrderBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public int CandyId { get; set; }

        [DataMember]
        public int? ConfectionerId { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public decimal Sum { get; set; }
    }
}
