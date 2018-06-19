using System.Runtime.Serialization;
namespace CandyShopService.ViewModels
{
    [DataContract]
    public class CustomerViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string CustomerFIO { get; set; }
    }
}
