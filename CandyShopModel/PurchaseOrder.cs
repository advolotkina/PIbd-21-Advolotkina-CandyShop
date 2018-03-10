using System;

namespace CandyShopModel
{
    public class PurchaseOrder
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int CandyId { get; set; }

        public int? ConfectionerId { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public PurchaseOrderStatus Status { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime? DateImplement { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Candy Candy { get; set; }

        public virtual Confectioner Confectioner { get; set; }
    }
}
