using CandyShopModel;
using System.Collections.Generic;

namespace CandyShopService
{
    class DataListSingleton
    {
        private static DataListSingleton instance;

        public List<Customer> Customers { get; set; }

        public List<Ingredient> Ingredients { get; set; }

        public List<Confectioner> Confectioners { get; set; }

        public List<PurchaseOrder> PurchaseOrders { get; set; }

        public List<Candy> Candies { get; set; }

        public List<CandyIngredient> CandyIngredients { get; set; }

        public List<Warehouse> Warehouses { get; set; }

        public List<WarehouseIngredient> WarehouseIngredients { get; set; }

        private DataListSingleton()
        {
            Customers = new List<Customer>();
            Ingredients = new List<Ingredient>();
            Confectioners = new List<Confectioner>();
            PurchaseOrders = new List<PurchaseOrder>();
            Candies = new List<Candy>();
            CandyIngredients = new List<CandyIngredient>();
            Warehouses = new List<Warehouse>();
            WarehouseIngredients = new List<WarehouseIngredient>();
        }

        public static DataListSingleton GetInstance()
        {
            if(instance == null)
            {
                instance = new DataListSingleton();
            }

            return instance;
        }
    }
}
