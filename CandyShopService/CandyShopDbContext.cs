using CandyShopModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace CandyShopService
{
    //[Table("CandyShopDatabase")]
    public class CandyShopDbContext: DbContext
    {
        public CandyShopDbContext() : base("CandyShopDatabase")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Ingredient> Ingredients { get; set; }

        public virtual DbSet<Confectioner> Confectioners { get; set; }

        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        public virtual DbSet<Candy> Candies { get; set; }

        public virtual DbSet<CandyIngredient> CandyIngredients { get; set; }

        public virtual DbSet<Warehouse> Warehouses { get; set; }

        public virtual DbSet<WarehouseIngredient> WarehouseIngredients { get; set; }
    }
}
