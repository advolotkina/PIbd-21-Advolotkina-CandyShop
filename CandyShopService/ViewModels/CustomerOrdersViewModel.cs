using System.Runtime.Serialization;
namespace CandyShopService.ViewModels
{
    [DataContract]
    public class CustomerOrdersViewModel
    {
        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string DateCreate { get; set; }

        [DataMember]
        public string CandyName { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public decimal Sum { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}
