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
    public class StoreItemsController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        public ActionResult Index()
        {
            var storeItems = _dbContext.StoreItems.Include(s => s.Store).Include(s => s.Unit);
            return View(storeItems.ToList());
        }
      
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreItems storeItems = _dbContext.StoreItems.Find(id);
            if (storeItems == null)
            {
                return HttpNotFound();
            }
            return View(storeItems);
        }

        public ActionResult Create(Guid storeid)
        {
            //ViewBag.StoreId = new SelectList(_dbContext.Store, "Id", "Name");
            ViewBag.UnitId = new SelectList(_dbContext.Units, "Id", "Name");
            StoreItems storeItems = new StoreItems();
            storeItems.StoreId = storeid;
            return PartialView("_Create", storeItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,Name,Quantity,UnitPrice,UnitId,StoreId,PurchaseBillId")] StoreItems storeItems)
        {
            if (ModelState.IsValid)
            {
                storeItems.BalanceQuantity = storeItems.Quantity;
                storeItems.Id = Guid.NewGuid();
                _dbContext.StoreItems.Add(storeItems);
                _dbContext.SaveChanges();
                return Json(new { success = true });
            }

            // ViewBag.StoreId = new SelectList(_dbContext.Store, "Id", "Name", storeItems.StoreId);
            ViewBag.UnitId = new SelectList(_dbContext.Units, "Id", "Name", storeItems.UnitId);
            return View(storeItems);
        }

        public ActionResult StoreItems(Guid? storeid)
        {
            decimal? unitPrice = 0;
            var storeitems = _dbContext.StoreItems.Where(pi => pi.StoreId == storeid).OrderBy(pi => pi.CreatedDate);
            var viewModel = Mapper.Map<IEnumerable<StoreItemsViewModel>>(storeitems);
            foreach (var item in viewModel)
            {
                decimal? totaltax = 0;
                unitPrice = item.UnitPrice;
                var purchaseBill = _dbContext.PurchaseBills.Find(item.PurchaseBillId);
                if (purchaseBill == null)
                {
                    item.BillInvoice = null;
                    item.SupplierInvoice = null;
                }
                else
                {
                    item.BillInvoice = purchaseBill.BillInvoice;
                    item.SupplierInvoice = purchaseBill.SupplierInvoice;
                }

                var taxes = _dbContext.PurchaseTaxes.Where(pi => pi.PurchaseBillId == item.PurchaseBillId);
                foreach (var itemtax in taxes)
                {
                    totaltax = itemtax.Percent + totaltax;
                }
                decimal? taxamount = (unitPrice * totaltax) / 100;
                //  decimal? FORPerUnit = FORtotal / item.Quantity;
                item.FORPrice = taxamount + unitPrice;
            }
            ViewBag.StoreId = storeid;
            return PartialView("_storeItems", viewModel.ToList());
        }
        public ActionResult IssueItems()
        {
            ViewBag.StoreId = new SelectList(_dbContext.Store, "Id", "Name");
            return View();
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreItems storeItems = _dbContext.StoreItems.Find(id);
            if (storeItems == null)
            {
                return HttpNotFound();
            }
            ViewBag.StoreId = new SelectList(_dbContext.Store, "Id", "Name", storeItems.StoreId);
            ViewBag.UnitId = new SelectList(_dbContext.Units, "Id", "Name", storeItems.UnitId);
            return View(storeItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Name,Quantity,UnitPrice,ExtendedPrice,UnitId,StoreId,PurchaseBillId")] StoreItems storeItems)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(storeItems).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StoreId = new SelectList(_dbContext.Store, "Id", "Name", storeItems.StoreId);
            ViewBag.UnitId = new SelectList(_dbContext.Units, "Id", "Name", storeItems.UnitId);
            return View(storeItems);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreItems storeItems = _dbContext.StoreItems.Find(id);
            if (storeItems == null)
            {
                return HttpNotFound();
            }
            return View(storeItems);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            StoreItems storeItems = _dbContext.StoreItems.Find(id);
            _dbContext.StoreItems.Remove(storeItems);
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
