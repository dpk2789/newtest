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
using AutoMapper;

namespace Accounts.Web.Controllers
{
    public class PurchaseBillsController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ActionResult Index()
        {
            // ViewBag.Suppliers = _dbContext.Suppliers.ToList();
            ViewBag.SupplierId = new SelectList(_dbContext.Suppliers, "Id", "AccountName");
            var purchaseBills = _dbContext.PurchaseBills.ToList();
            var viewModel = Mapper.Map<IEnumerable<PurchaseBillViewModel>>(purchaseBills);
            return View(viewModel);

        }

        public ActionResult DisplaySearchResults(Guid? supplierId, DateTime? todate, DateTime? fromdate)
        {
            if (supplierId != null)
            {
                if (todate != null && fromdate != null)
                {
                    var purchaseBills = _dbContext.PurchaseBills.Where(s => s.SupplierId == supplierId);
                    var toFromList = purchaseBills.Where(d1 => d1.BillDate >= todate && d1.BillDate <= fromdate);
                    var viewModel = Mapper.Map<IEnumerable<PurchaseBillViewModel>>(toFromList);
                    return PartialView("_PurchaseBills", viewModel);

                }
                else
                {
                    var purchaseBills = _dbContext.PurchaseBills.Where(s => s.SupplierId == supplierId);
                    var viewModel = Mapper.Map<IEnumerable<PurchaseBillViewModel>>(purchaseBills);
                    return PartialView("_PurchaseBills", viewModel);
                }
            }
            else
            {
                var toFromList = _dbContext.PurchaseBills.Where(d1 => d1.BillDate >= todate && d1.BillDate <= fromdate);
                var viewModel = Mapper.Map<IEnumerable<PurchaseBillViewModel>>(toFromList);
                return PartialView("_PurchaseBills", viewModel);

            }
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseBill purchaseBill = _dbContext.PurchaseBills.Find(id);
            if (purchaseBill == null)
            {
                return HttpNotFound();
            }
            return View(purchaseBill);
        }

        public ActionResult Create()
        {
            double? automaticNumbering;
            PurchaseBill purchaseBill = new PurchaseBill();
            ViewBag.SupplierId = new SelectList(_dbContext.Suppliers, "Id", "AccountName");
            var automaticInvoiceForm = _dbContext.AutomaticInvoiceForm.Where(a => a.Type == "Purchase").FirstOrDefault();
            if (automaticInvoiceForm != null)
            {
                if (automaticInvoiceForm.AutomaticPurchaseInvoice == true)
                {
                    double? Count = _dbContext.PurchaseBills.Count();
                    if (Count != 0)
                    {
                        automaticNumbering = automaticInvoiceForm.Numbering + Count + 1;
                        purchaseBill.BillInvoice = automaticInvoiceForm.Prefix + automaticNumbering.ToString() + automaticInvoiceForm.Suffix;
                    }
                }

            }
            else
            {

            }
            return View(purchaseBill);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BillInvoice,BillDate,SupplierId,SupplierInvoice")] PurchaseBill purchaseBill)
        {
            if (ModelState.IsValid)
            {
                purchaseBill.Id = Guid.NewGuid();
                _dbContext.PurchaseBills.Add(purchaseBill);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SupplierId = new SelectList(_dbContext.Suppliers, "Id", "AccountName", purchaseBill.SupplierId);
            return View(purchaseBill);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseBill purchaseBill = _dbContext.PurchaseBills.Find(id);
            if (purchaseBill == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierId = new SelectList(_dbContext.Suppliers, "Id", "AccountName", purchaseBill.SupplierId);
            return View(purchaseBill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BillInvoice,BillDate,SupplierId,SupplierInvoice,Freight,Packaging,OtherExpenses")] PurchaseBill purchaseBill)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(purchaseBill).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SupplierId = new SelectList(_dbContext.Suppliers, "Id", "AccountName", purchaseBill.SupplierId);
            return View(purchaseBill);
        }


        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseBill purchaseBill = _dbContext.PurchaseBills.Find(id);
            if (purchaseBill == null)
            {
                return HttpNotFound();
            }
            return View(purchaseBill);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PurchaseBill purchaseBill = _dbContext.PurchaseBills.Find(id);
            _dbContext.PurchaseBills.Remove(purchaseBill);
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
