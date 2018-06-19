using System.Runtime.Serialization;
namespace CandyShopService.BindingModels
{
    [DataContract]
    public class CandyIngredientBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CandyId { get; set; }

        [DataMember]
        public int IngredientId { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
