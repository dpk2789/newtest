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
    public class PurchaseTaxesController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ActionResult Index(Guid? purchaseBillId)
        {
            decimal? taxTotal = 0;
            var purchaseTaxes = _dbContext.PurchaseTaxes.Where(pi => pi.PurchaseBillId == purchaseBillId);
            foreach (var item in purchaseTaxes)
            {               
                decimal? percent = 0;
                percent = item.Percent;
                taxTotal = taxTotal + percent;
            }
            ViewBag.TaxTotal = taxTotal;
            return PartialView("_Index", purchaseTaxes.ToList());
        }
      
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseTaxes purchaseTaxes = _dbContext.PurchaseTaxes.Find(id);
            if (purchaseTaxes == null)
            {
                return HttpNotFound();
            }
            return View(purchaseTaxes);
        }

        public ActionResult Create(Guid purchaseBillId)
        {
            PurchaseTaxes purchasetax = new PurchaseTaxes();
           // ViewBag.PurchaseBillId = new SelectList(_dbContext.PurchaseBills, "Id", "BillInvoice");
            purchasetax.PurchaseBillId = purchaseBillId;
            return PartialView("_Create", purchasetax);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TaxCode,Name,Percent,PurchaseBillId")] PurchaseTaxes purchaseTaxes)
        {
            if (ModelState.IsValid)
            {
                purchaseTaxes.Id = Guid.NewGuid();
                _dbContext.PurchaseTaxes.Add(purchaseTaxes);
                _dbContext.SaveChanges();
                return Json(new { success = true });
            }

            ViewBag.PurchaseBillId = new SelectList(_dbContext.PurchaseBills, "Id", "BillInvoice", purchaseTaxes.PurchaseBillId);
            return View(purchaseTaxes);
        }
     
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseTaxes purchaseTaxes = _dbContext.PurchaseTaxes.Find(id);
            if (purchaseTaxes == null)
            {
                return HttpNotFound();
            }
            ViewBag.PurchaseBillId = new SelectList(_dbContext.PurchaseBills, "Id", "BillInvoice", purchaseTaxes.PurchaseBillId);
            return PartialView("_Edit", purchaseTaxes);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TaxCode,Name,Percent,PurchaseBillId")] PurchaseTaxes purchaseTaxes)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(purchaseTaxes).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return Json(new { success = true });
            }
            ViewBag.PurchaseBillId = new SelectList(_dbContext.PurchaseBills, "Id", "BillInvoice", purchaseTaxes.PurchaseBillId);
            return View(purchaseTaxes);
        }
     
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseTaxes purchaseTaxes = _dbContext.PurchaseTaxes.Find(id);
            if (purchaseTaxes == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete", purchaseTaxes);
        }
     
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PurchaseTaxes purchaseTaxes = _dbContext.PurchaseTaxes.Find(id);
            try
            {
                _dbContext.PurchaseTaxes.Remove(purchaseTaxes);
                _dbContext.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

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
