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
using Newtonsoft.Json;

namespace Accounts.Web.Controllers
{
    public class ItemsController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        public ActionResult Index()
        {
            var items = _dbContext.ItemCategoryies;
            return View(items.ToList());
        }


        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _dbContext.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }


        public ActionResult Create()
        {
            ViewBag.ItemCategoryId = new SelectList(_dbContext.ItemCategoryies, "Id", "CategoryName");
            ViewBag.UnitId = new SelectList(_dbContext.Units, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,Name,UnitPrice,ItemCategoryId,UnitId")] Item item)
        {
            if (ModelState.IsValid)
            {
                item.Id = Guid.NewGuid();
                _dbContext.Items.Add(item);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemCategoryId = new SelectList(_dbContext.ItemCategoryies, "Id", "CategoryName", item.ItemCategoryId);
            ViewBag.UnitId = new SelectList(_dbContext.Units, "Id", "Name");
            return View(item);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _dbContext.Items.Find(id);

            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemCategoryId = new SelectList(_dbContext.ItemCategoryies, "Id", "CategoryName", item.ItemCategoryId);
            ViewBag.UnitId = new SelectList(_dbContext.Units, "Id", "Name", item.UnitId);
            ViewBag.IngridentUnitId = new SelectList(_dbContext.Units, "Id", "Name", item.UnitId);
            ViewBag.IngridentId = new SelectList(_dbContext.Items, "Id", "Name");
            return View(item);
        }


        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult Edit(string data, string code, Guid id)
        {
            if (ModelState.IsValid)
            {
                var deserialiseList = JsonConvert.DeserializeObject<List<CompoundItemIngredient>>(data);
                Item item = _dbContext.Items.Find(id);
                var retriveCompoundItemIngredients = _dbContext.CompoundItemIngredients.Where(i => i.ItemId == id).ToList();
                foreach (var retriveIngrident in retriveCompoundItemIngredients)
                {
                    _dbContext.CompoundItemIngredients.Remove(retriveIngrident);
                    _dbContext.SaveChanges();
                }
                foreach (var ingrident in deserialiseList)
                {
                    CompoundItemIngredient compoundItemIngredient = new CompoundItemIngredient();
                    compoundItemIngredient.Id = Guid.NewGuid();
                    compoundItemIngredient.IngridentId = ingrident.IngridentId;
                    compoundItemIngredient.UnitQuantity = ingrident.UnitQuantity;
                    compoundItemIngredient.IngridentUnitId = ingrident.IngridentUnitId;
                    compoundItemIngredient.IngridentName = ingrident.IngridentName;
                    compoundItemIngredient.ItemId = ingrident.ItemId;
                    _dbContext.CompoundItemIngredients.Add(compoundItemIngredient);
                    _dbContext.SaveChanges();
                }
                _dbContext.Entry(item).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return Json(new { success = true });
            }
            ViewBag.ItemCategoryId = new SelectList(_dbContext.ItemCategoryies, "Id", "CategoryName");
            ViewBag.UnitId = new SelectList(_dbContext.Units, "Id", "Name");
            return View();
        }


        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _dbContext.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Item item = _dbContext.Items.Find(id);
            _dbContext.Items.Remove(item);
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

        public JsonResult GetItemsForAutocomplete(string term)
        {
            Item[] matchingItems = String.IsNullOrWhiteSpace(term)
         ? null
         : _dbContext.Items.Where(ii => ii.Code.Contains(term) || ii.Name.Contains(term)).ToArray();

            return Json(matchingItems.Select(m => new
            {
                id = m.Code,
                value = m.Code,
                label = String.Format("{0}: {1}", m.Code, m.Name),
                Name = m.Name,
                UnitPrice = m.UnitPrice,
                UnitId = m.UnitId
            }), JsonRequestBehavior.AllowGet);


        }
    }
}
