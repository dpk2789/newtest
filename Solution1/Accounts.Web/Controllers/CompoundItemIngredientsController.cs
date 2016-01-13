using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Accounts.Context;
using Accounts.Model.Model;
using Accounts.Web.ViewModel;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace Accounts.Web.Controllers
{
    public class CompoundItemIngredientsController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(_dbContext.CompoundItemIngredients.ToList());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompoundItemIngredient compoundItemIngredient = _dbContext.CompoundItemIngredients.Find(id);
            if (compoundItemIngredient == null)
            {
                return HttpNotFound();
            }
            return View(compoundItemIngredient);
        }

        public ActionResult Create()
        {
            ViewBag.CompoundItemId = new SelectList(_dbContext.Items, "Id", "Name");
            ViewBag.ItemId = new SelectList(_dbContext.Items, "Id", "Name");
            return View();
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult Create(string data)
        {
            var deserialiseList = JsonConvert.DeserializeObject<List<CompoundItemIngredient>>(data);
            foreach (var item in deserialiseList)
            {
                CompoundItemIngredient compoundItemIngredient = new CompoundItemIngredient();
                compoundItemIngredient.Id = Guid.NewGuid();
               // compoundItemIngredient.CompoundItemId = item.CompoundItemId;
                compoundItemIngredient.ItemId = item.ItemId;
                _dbContext.CompoundItemIngredients.Add(compoundItemIngredient);
                _dbContext.SaveChanges();
            }

            ViewBag.CompoundItemId = new SelectList(_dbContext.Items, "Id", "Name");
            ViewBag.ItemId = new SelectList(_dbContext.Items, "Id", "Name");
            return Json(new { success = true });
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompoundItemIngredient compoundItemIngredient = _dbContext.CompoundItemIngredients.Find(id);
            if (compoundItemIngredient == null)
            {
                return HttpNotFound();
            }
            return View(compoundItemIngredient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompoundItemId,ItemId,UnitQuantity")] CompoundItemIngredient compoundItemIngredient)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(compoundItemIngredient).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(compoundItemIngredient);
        }


        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompoundItemIngredient compoundItemIngredient = _dbContext.CompoundItemIngredients.Find(id);
            if (compoundItemIngredient == null)
            {
                return HttpNotFound();
            }
            return View(compoundItemIngredient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CompoundItemIngredient compoundItemIngredient = _dbContext.CompoundItemIngredients.Find(id);
            _dbContext.CompoundItemIngredients.Remove(compoundItemIngredient);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
