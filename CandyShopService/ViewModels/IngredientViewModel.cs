using System.Runtime.Serialization;

namespace CandyShopService.ViewModels
{
    [DataContract]
    public class IngredientViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string IngredientName { get; set; }
    }
}
