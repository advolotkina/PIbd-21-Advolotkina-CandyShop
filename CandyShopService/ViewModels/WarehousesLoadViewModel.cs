using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CandyShopService.ViewModels
{
    [DataContract]
    public class WarehousesLoadViewModel
    {
        [DataMember]
        public string WarehouseName { get; set; }

        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public IEnumerable<Tuple<string, int>> Ingredients { get; set; }
    }
}
