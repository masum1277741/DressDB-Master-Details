using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace project_masterdetails_crud.Models.Controllers
{
    public class DressModelsController : Controller
    {
        private StyleweekDbContext db = new StyleweekDbContext();
        public ActionResult Index()
        {
            return View(db.DressModels.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DressModel dressModel = db.DressModels.Find(id);
            if (dressModel == null)
            {
                return HttpNotFound();
            }
            return View(dressModel);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DressModelId,ModelName")] DressModel dressModel)
        {
            if (ModelState.IsValid)
            {
                db.DressModels.Add(dressModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dressModel);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DressModel dressModel = db.DressModels.Find(id);
            if (dressModel == null)
            {
                return HttpNotFound();
            }
            return View(dressModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DressModelId,ModelName")] DressModel dressModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dressModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dressModel);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DressModel dressModel = db.DressModels.Find(id);
            if (dressModel == null)
            {
                return HttpNotFound();
            }
            return View(dressModel);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DressModel dressModel = db.DressModels.Find(id);
            db.DressModels.Remove(dressModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
