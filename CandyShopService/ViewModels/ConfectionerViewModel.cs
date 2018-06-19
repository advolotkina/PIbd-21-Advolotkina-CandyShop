using System.Runtime.Serialization;

namespace CandyShopService.ViewModels
{
    [DataContract]
    public class ConfectionerViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string ConfectionerFIO { get; set; }
    }
}
