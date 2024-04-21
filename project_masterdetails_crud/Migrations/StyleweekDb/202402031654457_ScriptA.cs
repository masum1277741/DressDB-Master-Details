namespace project_masterdetails_crud.Migrations.StyleweekDb
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScriptA : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        BrandId = c.Int(nullable: false, identity: true),
                        BrandName = c.String(),
                    })
                .PrimaryKey(t => t.BrandId);
            
            CreateTable(
                "dbo.Dresses",
                c => new
                    {
                        DressId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        FirstIntroduceOn = c.DateTime(nullable: false, storeType: "date"),
                        OnSale = c.Boolean(nullable: false),
                        Picture = c.String(),
                        DressModelId = c.Int(nullable: false),
                        BrandId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DressId)
                .ForeignKey("dbo.Brands", t => t.BrandId, cascadeDelete: true)
                .ForeignKey("dbo.DressModels", t => t.DressModelId, cascadeDelete: true)
                .Index(t => t.DressModelId)
                .Index(t => t.BrandId);
            
            CreateTable(
                "dbo.DressModels",
                c => new
                    {
                        DressModelId = c.Int(nullable: false, identity: true),
                        ModelName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.DressModelId);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        StockId = c.Int(nullable: false, identity: true),
                        Size = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        DressId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StockId)
                .ForeignKey("dbo.Dresses", t => t.DressId, cascadeDelete: true)
                .Index(t => t.DressId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stocks", "DressId", "dbo.Dresses");
            DropForeignKey("dbo.Dresses", "DressModelId", "dbo.DressModels");
            DropForeignKey("dbo.Dresses", "BrandId", "dbo.Brands");
            DropIndex("dbo.Stocks", new[] { "DressId" });
            DropIndex("dbo.Dresses", new[] { "BrandId" });
            DropIndex("dbo.Dresses", new[] { "DressModelId" });
            DropTable("dbo.Stocks");
            DropTable("dbo.DressModels");
            DropTable("dbo.Dresses");
            DropTable("dbo.Brands");
        }
    }
}
