using PagedList;
using project_masterdetails_crud.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace project_masterdetails_crud.Models.Controllers
{
    [Authorize]
    public class DressesController : Controller
    {
        private readonly StyleweekDbContext db = new StyleweekDbContext();
        // GET: Dresses
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public PartialViewResult DressDetails(int pg = 1)
        {
            var data = db.Dresses
                       .Include(x=>x.Stocks)
                       .Include(x => x.DressModel)
                       .Include(x => x.Brand)
                       .OrderBy(x => x.DressId)
                       .ToPagedList(pg, 5);
            return PartialView("_DressDetails", data);
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult CreateForm()
        {
            DressInputModel model = new DressInputModel();
            model.Stocks.Add(new Stock());
            ViewBag.DressModels = db.DressModels.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return PartialView("_CreateForm", model);
        }
        [HttpPost]
        public ActionResult Create(DressInputModel model, string act = "")
        {
            if (act == "add")
            {
                model.Stocks.Add(new Stock());
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                model.Stocks.RemoveAt(index);
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (act == "insert")
            {
                if (ModelState.IsValid)
                {
                    var dress = new Dress
                    {
                        BrandId = model.BrandId,
                        DressModelId = model.DressModelId,
                        Name = model.Name,
                        FirstIntroduceOn = model.FirstIntroduceOn,
                        OnSale = model.OnSale
                    };
                    //For Image
                    string ext = Path.GetExtension(model.Picture.FileName);
                    string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Path.Combine(Server.MapPath("~/Images"), f);
                    model.Picture.SaveAs(savePath);
                    dress.Picture = f;

                    db.Dresses.Add(dress);
                    db.SaveChanges();

                    //Stocks
                    foreach (var s in model.Stocks)
                    {
                        db.Database.ExecuteSqlCommand($"spInsertStock {(int)s.Size},{s.Price},{(int)s.Quantity},{dress.DressId}");
                    }

                    DressInputModel newModel = new DressInputModel()
                    {
                        Name = "",
                        FirstIntroduceOn = DateTime.Today
                    };
                    newModel.Stocks.Add(new Stock());

                    ViewBag.DressModels = db.DressModels.ToList();
                    ViewBag.Brands = db.Brands.ToList();
                    foreach (var e in ModelState.Values)
                    {
                        e.Value = null;
                    }
                    return View("_CreateForm", newModel);
                }
            }
            ViewBag.DressModels = db.DressModels.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return View("_CreateForm", model);
        }
        public ActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public ActionResult EditForm(int id)
        {
            var data = db.Dresses.FirstOrDefault(x => x.DressId == id);
            if (data == null) return new HttpNotFoundResult();
            db.Entry(data).Collection(x => x.Stocks).Load();
            DressEditModel model = new DressEditModel
            {
                DressId = id,
                BrandId = data.BrandId,
                DressModelId = data.DressModelId,
                Name = data.Name,
                FirstIntroduceOn = data.FirstIntroduceOn,
                OnSale = data.OnSale,
                Stocks = data.Stocks.ToList()
            };
            ViewBag.DressModels = db.DressModels.ToList();
            ViewBag.Brands = db.Brands.ToList();
            ViewBag.CurrentPic = data.Picture;
            return PartialView("_EditForm", model);
        }
        [HttpPost]
        public ActionResult Edit(DressEditModel model, string act = "")
        {
            if (act == "add")
            {
                model.Stocks.Add(new Stock());
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                model.Stocks.RemoveAt(index);
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (act == "update")
            {
                if (ModelState.IsValid)
                {
                    var dress = db.Dresses.FirstOrDefault(x => x.DressId == model.DressId);
                    if (dress == null) { return new HttpNotFoundResult(); }
                    dress.Name = model.Name;
                    dress.FirstIntroduceOn = model.FirstIntroduceOn;
                    dress.OnSale = model.OnSale;
                    dress.BrandId = model.BrandId;
                    dress.DressModelId = model.DressModelId;
                    if (model.Picture != null)
                    {
                        string ext = Path.GetExtension(model.Picture.FileName);
                        string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                        string savePath = Path.Combine(Server.MapPath("~/Images"), f);
                        model.Picture.SaveAs(savePath);
                        dress.Picture = f;
                    }
                    else
                    {

                    }

                    db.SaveChanges();                   
                    db.Database.ExecuteSqlCommand($"EXEC DeleteStockByDressId {dress.DressId}");
                    foreach (var s in model.Stocks)
                    {
                        db.Database.ExecuteSqlCommand($"EXEC spInsertStock {(int)s.Size}, {s.Price}, {s.Quantity}, {dress.DressId}");
                    }
                    return RedirectToAction("Index");
                }
            }
            ViewBag.DressModels = db.DressModels.ToList();
            ViewBag.Brands = db.Brands.ToList();
            ViewBag.CurrentPic = db.Dresses.FirstOrDefault(x => x.DressId == model.DressId)?.Picture;
            return View("_EditForm", model);
        }
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                var drs = db.Dresses .FirstOrDefault(x => x.DressId == id);
                var dressDetails = db.Dresses.FirstOrDefault(x => x.DressId == id);

                db.Dresses.Remove(dressDetails);
                db.Dresses.Remove(drs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }
}