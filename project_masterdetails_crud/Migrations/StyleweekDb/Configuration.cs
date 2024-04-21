namespace project_masterdetails_crud.Migrations.StyleweekDb
{
    using project_masterdetails_crud.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<project_masterdetails_crud.Models.StyleweekDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\StyleweekDb";
        }

        protected override void Seed(project_masterdetails_crud.Models.StyleweekDbContext db)
        {
            db.Brands.AddRange(new Brand[]
           {
                new Brand{BrandName="Allen Solly"},
                new Brand{BrandName="Berrylush"}
           });
            db.DressModels.AddRange(new DressModel[]
            {
                new DressModel{ModelName="Tutu dress"},
                new DressModel{ModelName="Shift dress"}
            });
            db.SaveChanges();
            Dress d = new Dress
            {
                Name = "Salwar Kameez",
                DressModelId = 1,
                BrandId = 1,
                FirstIntroduceOn = new DateTime(2024, 1, 1),
                OnSale = true,
                Picture = "03.png"
            };
            d.Stocks.Add(new Stock { Size = Size.I34, Quantity = 10, Price = 900 });
            d.Stocks.Add(new Stock { Size = Size.I36, Quantity = 10, Price = 950 });
            db.Dresses.Add(d);
            db.SaveChanges();
        }
    }
}
