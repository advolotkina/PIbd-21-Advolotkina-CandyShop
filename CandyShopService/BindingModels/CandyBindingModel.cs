using System.Runtime.Serialization;
using System.Collections.Generic;

namespace CandyShopService.BindingModels
{
    [DataContract]
    public class CandyBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string CandyName { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public List<CandyIngredientBindingModel> CandyIngredients { get; set; }
    }
}
