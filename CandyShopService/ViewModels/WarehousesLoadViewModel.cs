using System;
using System.Collections.Generic;

namespace CandyShopService.ViewModels
{
    public class WarehousesLoadViewModel
    {
        public string WarehouseName { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<Tuple<string, int>> Ingredients { get; set; }
    }
}
