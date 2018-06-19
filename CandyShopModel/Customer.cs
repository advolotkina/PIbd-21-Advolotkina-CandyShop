using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CandyShopModel
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string CustomerFIO { get; set; }

        [ForeignKey("CustomerId")]
        public virtual List<PurchaseOrder> Orders { get; set; }
    }
}
