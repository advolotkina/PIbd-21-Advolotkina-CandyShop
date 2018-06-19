using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CandyShopService.ViewModels
{
    [DataContract]
    public class WarehouseViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string WarehouseName { get; set; }

        [DataMember]
        public List<WarehouseIngredientViewModel> WarehouseIngredients { get; set; }
    }
}
