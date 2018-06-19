using System.Runtime.Serialization;
namespace CandyShopService.BindingModels
{
    [DataContract]
    public class ConfectionerBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string ConfectionerFIO { get; set; }
    }
}
