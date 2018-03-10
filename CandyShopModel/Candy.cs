using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CandyShopModel
{
    public class Candy
    {
        public int Id { get; set; }

        [Required]
        public string CandyName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [ForeignKey("CandyId")]
        public virtual List<PurchaseOrder> Orders { get; set; }

        [ForeignKey("CandyId")]
        public virtual List<CandyIngredient> CandyIngredients { get; set; }
    }
}
