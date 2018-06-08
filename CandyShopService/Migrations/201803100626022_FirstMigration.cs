namespace CandyShopService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Candies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CandyName = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CandyIngredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CandyId = c.Int(nullable: false),
                        IngredientId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candies", t => t.CandyId, cascadeDelete: true)
                .ForeignKey("dbo.Ingredients", t => t.IngredientId, cascadeDelete: true)
                .Index(t => t.CandyId)
                .Index(t => t.IngredientId);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IngredientName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WarehouseIngredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WarehouseId = c.Int(nullable: false),
                        IngredientId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ingredients", t => t.IngredientId, cascadeDelete: true)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId, cascadeDelete: true)
                .Index(t => t.WarehouseId)
                .Index(t => t.IngredientId);
            
            CreateTable(
                "dbo.Warehouses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WarehouseName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PurchaseOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        CandyId = c.Int(nullable: false),
                        ConfectionerId = c.Int(),
                        Count = c.Int(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateImplement = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candies", t => t.CandyId, cascadeDelete: true)
                .ForeignKey("dbo.Confectioners", t => t.ConfectionerId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.CandyId)
                .Index(t => t.ConfectionerId);
            
            CreateTable(
                "dbo.Confectioners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConfectionerFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseOrders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.PurchaseOrders", "ConfectionerId", "dbo.Confectioners");
            DropForeignKey("dbo.PurchaseOrders", "CandyId", "dbo.Candies");
            DropForeignKey("dbo.WarehouseIngredients", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.WarehouseIngredients", "IngredientId", "dbo.Ingredients");
            DropForeignKey("dbo.CandyIngredients", "IngredientId", "dbo.Ingredients");
            DropForeignKey("dbo.CandyIngredients", "CandyId", "dbo.Candies");
            DropIndex("dbo.PurchaseOrders", new[] { "ConfectionerId" });
            DropIndex("dbo.PurchaseOrders", new[] { "CandyId" });
            DropIndex("dbo.PurchaseOrders", new[] { "CustomerId" });
            DropIndex("dbo.WarehouseIngredients", new[] { "IngredientId" });
            DropIndex("dbo.WarehouseIngredients", new[] { "WarehouseId" });
            DropIndex("dbo.CandyIngredients", new[] { "IngredientId" });
            DropIndex("dbo.CandyIngredients", new[] { "CandyId" });
            DropTable("dbo.Customers");
            DropTable("dbo.Confectioners");
            DropTable("dbo.PurchaseOrders");
            DropTable("dbo.Warehouses");
            DropTable("dbo.WarehouseIngredients");
            DropTable("dbo.Ingredients");
            DropTable("dbo.CandyIngredients");
            DropTable("dbo.Candies");
        }
    }
}
