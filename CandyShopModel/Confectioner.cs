using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CandyShopModel
{
    public class Confectioner
    {
        public int Id { get; set; }

        [Required]
        public string ConfectionerFIO { get; set; }

        [ForeignKey("ConfectionerId")]
        public virtual List<PurchaseOrder> Orders { get; set; }
    }
}
