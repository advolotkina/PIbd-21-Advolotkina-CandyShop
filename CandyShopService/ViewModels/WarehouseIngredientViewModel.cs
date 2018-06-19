using System.Runtime.Serialization;

namespace CandyShopService.ViewModels
{
    [DataContract]
    public class WarehouseIngredientViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int WarehouseId { get; set; }

        [DataMember]
        public int IngredientId { get; set; }

        [DataMember]
        public string IngredientName { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
