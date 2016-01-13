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

namespace Accounts.Web.Controllers
{
    public class TaxesController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        public ActionResult Index()
        {
            var taxies = _dbContext.Taxies.Include(t => t.TaxCategory);
            return View(taxies.ToList());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax tax = _dbContext.Taxies.Find(id);
            if (tax == null)
            {
                return HttpNotFound();
            }
            return View(tax);
        }
        public ActionResult Create()
        {
            ViewBag.TaxCategoryId = new SelectList(_dbContext.TaxCategories, "Id", "CategoryName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TaxCode,Name,Percent,TaxCategoryId")] Tax tax)
        {
            if (ModelState.IsValid)
            {
                tax.Id = Guid.NewGuid();
                _dbContext.Taxies.Add(tax);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TaxCategoryId = new SelectList(_dbContext.TaxCategories, "Id", "CategoryName", tax.TaxCategoryId);
            return View(tax);
        }


        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax tax = _dbContext.Taxies.Find(id);
            if (tax == null)
            {
                return HttpNotFound();
            }
            ViewBag.TaxCategoryId = new SelectList(_dbContext.TaxCategories, "Id", "CategoryName", tax.TaxCategoryId);
            return View(tax);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TaxCode,Name,Percent,TaxCategoryId")] Tax tax)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(tax).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TaxCategoryId = new SelectList(_dbContext.TaxCategories, "Id", "CategoryName", tax.TaxCategoryId);
            return View(tax);
        }


        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax tax = _dbContext.Taxies.Find(id);
            if (tax == null)
            {
                return HttpNotFound();
            }
            return View(tax);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Tax tax = _dbContext.Taxies.Find(id);
            _dbContext.Taxies.Remove(tax);
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

        public JsonResult GetTaxesForAutocomplete(string term)
        {
            Tax[] matchingItems = String.IsNullOrWhiteSpace(term)
                ? null
                : _dbContext.Taxies.Where(ii => ii.TaxCode.Contains(term) || ii.Name.Contains(term)).ToArray();

            return Json(matchingItems.Select(m => new
            {
                id = m.TaxCode,
                value = m.TaxCode,
                label = String.Format("{0}: {1}", m.TaxCode, m.Name),
                Name = m.Name,
                Percent = m.Percent
            }), JsonRequestBehavior.AllowGet);
        }
    }
}
