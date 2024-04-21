using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace project_masterdetails_crud.Models
{
    public enum Size {I34=34, I36 = 36, I38 = 38, I40 = 40, I42 = 42, I44 = 44, U4=4,U6=6,U8=8,U10=10,U12=12,U14=14,U16=16,U18=18 }
    public class DressModel
    {
        public int DressModelId { get; set; }
        [Required,StringLength(50),Display(Name = "Model Name")]
        public string ModelName { get; set; }
        //nev
        public virtual ICollection<Dress> Dresses { get; set; } = new List<Dress>();
    }
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        //nev
        public virtual ICollection<Dress> Dresses { get; set; } = new List<Dress>();
    }
    public class Dress
    {
        public int DressId { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "First Introduce"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FirstIntroduceOn { get; set; }
        public bool OnSale { get; set; }
        public string Picture { get; set; }
        //fk
        [ForeignKey("DressModel")]
        public int DressModelId { get; set; }
        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        //nev
        public virtual DressModel DressModel { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
    public class Stock
    {
        public int StockId { get; set; }
        public Size Size { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [Required, ForeignKey("Dress")]
        public int DressId { get; set; }
        //
        public virtual Dress Dress { get; set; }
    }
    public class StyleweekDbContext : DbContext
    {

        public DbSet<Brand> Brands { get; set; }
        public DbSet<DressModel> DressModels { get; set; }
        public DbSet<Dress> Dresses { get; set; }
        public DbSet<Stock> Stocks { get; set; }
    }
}