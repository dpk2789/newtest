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
using AutoMapper;
using Accounts.Web.ViewModel;

namespace Accounts.Web.Controllers
{
    public class PurchaseItemsController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        public ActionResult Index(Guid? purchaseBillId, DateTime? puchaseBillDate)
        {
            decimal? itemTotal = 0;
            decimal? quantityTotal = 0;

            var purchaseItems = _dbContext.PurchaseItems.Where(pi => pi.PurchaseBillId == purchaseBillId).OrderBy(pi => pi.CreatedDate);
            var viewModel = Mapper.Map<IEnumerable<PurchaseItemsViewModel>>(purchaseItems);
            foreach (var item in viewModel)
            {
                decimal? extendedPrice = 0;
                decimal? quantity = 0;
                extendedPrice = item.ExtendedPrice;
                quantity = item.Quantity;
                itemTotal = extendedPrice + itemTotal;
                quantityTotal = quantityTotal + quantity;
            }
            ViewBag.QuantityTotal = quantityTotal;
            ViewBag.ItemExtentedPriceTotal = itemTotal;
            ViewBag.purchaseBillId = purchaseBillId;
            ViewBag.purchaseBillDate = puchaseBillDate;
            return PartialView("_Index", viewModel.ToList());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseItems purchaseItems = _dbContext.PurchaseItems.Find(id);
            if (purchaseItems == null)
            {
                return HttpNotFound();
            }
            return View(purchaseItems);
        }

        public ActionResult Create(Guid purchaseBillId, DateTime puchaseBillDate)
        {
            PurchaseItemsViewModel viewModel = new PurchaseItemsViewModel();
            ViewBag.WareHouseId = new SelectList(_dbContext.WareHouses, "Id", "Name");
            ViewBag.PurchaseBillId = new SelectList(_dbContext.PurchaseBills, "Id", "BillInvoice");
            ViewBag.UnitId = new SelectList(_dbContext.Units, "Id", "Name");
            ViewBag.StoreId = new SelectList(_dbContext.Store, "Id", "Name");
            viewModel.PurchaseBillId = purchaseBillId;
            viewModel.PurchaseBillDate = puchaseBillDate;
            return PartialView("_Create", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,Name,Quantity,UnitPrice,UnitId,PurchaseBillId,StoreId,PurchaseBillDate")] PurchaseItemsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                StoreItems storeItems = new StoreItems();
                PurchaseItems purchaseItems = Mapper.Map<PurchaseItems>(viewModel);
                Guid purchaseItemid = Guid.NewGuid();
                purchaseItems.Id = purchaseItemid;
                storeItems.Id = Guid.NewGuid();
                storeItems.Code = viewModel.Code;
                storeItems.Name = viewModel.Name;
                storeItems.PurchaseBillId = viewModel.PurchaseBillId;
                storeItems.Quantity = viewModel.Quantity;
                storeItems.UnitPrice = viewModel.UnitPrice;
                storeItems.ExtendedPrice = viewModel.ExtendedPrice;
                storeItems.StoreId = viewModel.StoreId;
                storeItems.Unit = viewModel.Unit;
                storeItems.UnitId = viewModel.UnitId;
                storeItems.BalanceQuantity = viewModel.Quantity;
                storeItems.ItemAddedDate = viewModel.PurchaseBillDate;
                storeItems.PurchaseItemsId = purchaseItemid;
                storeItems.Type = "Inward";

                _dbContext.StoreItems.Add(storeItems);
                _dbContext.PurchaseItems.Add(purchaseItems);
                _dbContext.SaveChanges();
                return Json(new { success = true });
            }

            ViewBag.PurchaseBillId = new SelectList(_dbContext.PurchaseBills, "Id", "BillInvoice", viewModel.PurchaseBillId);
            ViewBag.UnitId = new SelectList(_dbContext.Units, "Id", "Name", viewModel.UnitId);
            ViewBag.StoreId = new SelectList(_dbContext.Store, "Id", "Name");
            return View(viewModel);
        }

        public ActionResult Edit(Guid? id, DateTime puchaseBillDate)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseItems purchaseItems = _dbContext.PurchaseItems.Find(id);
            if (purchaseItems == null)
            {
                return HttpNotFound();
            }
            PurchaseItemsViewModel viewModel = Mapper.Map<PurchaseItemsViewModel>(purchaseItems);
            ViewBag.PurchaseBillId = new SelectList(_dbContext.PurchaseBills, "Id", "BillInvoice", purchaseItems.PurchaseBillId);
            ViewBag.UnitId = new SelectList(_dbContext.Units, "Id", "Name", purchaseItems.UnitId);
            ViewBag.StoreId = new SelectList(_dbContext.Store, "Id", "Name", purchaseItems.StoreId);
            viewModel.PurchaseBillDate = puchaseBillDate;
            return PartialView("_Edit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Name,Quantity,UnitPrice,UnitId,WareHouseId,PurchaseBillId,StoreId,PurchaseBillDate")] PurchaseItemsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                PurchaseItems purchaseItems = Mapper.Map<PurchaseItems>(viewModel);
               // StoreItems storeItems = _dbContext.StoreItems.Where(si => si.PurchaseBillId == purchaseItems.PurchaseBillId && si.StoreId == purchaseItems.StoreId).FirstOrDefault();
                StoreItems storeItems = _dbContext.StoreItems.Where(si=>si.PurchaseItemsId==viewModel.Id).FirstOrDefault();
                storeItems.Code = viewModel.Code;
                storeItems.Name = viewModel.Name;
                storeItems.Quantity = viewModel.Quantity;
                storeItems.UnitPrice = viewModel.UnitPrice;
                storeItems.ExtendedPrice = viewModel.ExtendedPrice;
                storeItems.BalanceQuantity = viewModel.Quantity;
                storeItems.ItemAddedDate = viewModel.PurchaseBillDate;

                _dbContext.Entry(purchaseItems).State = EntityState.Modified;
                _dbContext.Entry(storeItems).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return Json(new { success = true });
            }
            ViewBag.PurchaseBillId = new SelectList(_dbContext.PurchaseBills, "Id", "BillInvoice", viewModel.PurchaseBillId);
            ViewBag.UnitId = new SelectList(_dbContext.Units, "Id", "Name", viewModel.UnitId);
            ViewBag.WareHouseId = new SelectList(_dbContext.WareHouses, "Id", "Name");
            return View(viewModel);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseItems purchaseItems = _dbContext.PurchaseItems.Find(id);
            if (purchaseItems == null)
            {
                return HttpNotFound();
            }

            return PartialView("_Delete", purchaseItems);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PurchaseItems purchaseItems = _dbContext.PurchaseItems.Find(id);
            StoreItems storeItems = _dbContext.StoreItems.Where(si => si.PurchaseItemsId == id).FirstOrDefault();
            try
            {
                _dbContext.PurchaseItems.Remove(purchaseItems);
                _dbContext.StoreItems.Remove(storeItems);
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
