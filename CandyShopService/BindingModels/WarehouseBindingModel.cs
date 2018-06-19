using System.Runtime.Serialization;
namespace CandyShopService.BindingModels
{
    [DataContract]
    public class WarehouseBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string WarehouseName { get; set; }
    }
}
