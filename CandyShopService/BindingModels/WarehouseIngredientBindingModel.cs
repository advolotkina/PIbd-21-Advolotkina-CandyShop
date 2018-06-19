using System.Runtime.Serialization;
namespace CandyShopService.BindingModels
{
    [DataContract]
    public class WarehouseIngredientBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int WarehouseId { get; set; }

        [DataMember]
        public int IngredientId { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
