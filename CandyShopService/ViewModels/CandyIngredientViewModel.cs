using System.Runtime.Serialization;

namespace CandyShopService.ViewModels
{
    [DataContract]
    public class CandyIngredientViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CandyId { get; set; }

        [DataMember]
        public int IngredientId { get; set; }

        [DataMember]
        public string IngredientName { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
