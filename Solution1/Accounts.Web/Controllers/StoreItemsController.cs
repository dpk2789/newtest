﻿using System;
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
        public ActionResult Create([Bind(Include = "Id,Code,Name,Quantity,UnitPrice,UnitId,StoreId,PurchaseBillId,ItemAddedDate")] StoreItems storeItems)
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

        public ActionResult OtherItemsReq(string ItemName, decimal? IssuedQuantity)
        {
            List<ItemsRequiredViewModel> itemRequiredViewModelList = new List<ItemsRequiredViewModel>();
            if (IssuedQuantity == null)
            {
                return PartialView("_OtherItemsReq", itemRequiredViewModelList);
            }
            else
            {

                var itemDetail = _dbContext.Items.Where(x => x.Name == ItemName).FirstOrDefault();
                var ingredientsRequired = _dbContext.CompoundItemIngredients.Where(x => x.ItemId == itemDetail.Id);
                var storeItems = _dbContext.StoreItems.ToList();
                var units = _dbContext.Units.ToList();
                if (ingredientsRequired.Count() == 0)
                {

                }
                else
                {
                    foreach (var item in ingredientsRequired)
                    {
                        ItemsRequiredViewModel itemRequiredViewModel = new ItemsRequiredViewModel();
                        string ingridientName = item.IngridentName;
                        decimal ingridientQuantity = item.UnitQuantity;
                        var unit = units.Where(u => u.Id == item.IngridentUnitId).FirstOrDefault();
                        itemRequiredViewModel.UnitName = unit.Name;
                        itemRequiredViewModel.IngridentName = item.IngridentName;
                        itemRequiredViewModel.IngridentId = item.IngridentId;
                        var storeItem = storeItems.Where(s => s.Name == ingridientName).FirstOrDefault();
                        // StoreItemsViewModel viewModel = Mapper.Map<StoreItemsViewModel>(storeItem);
                        if (storeItem.BalanceQuantity > 0)
                        {
                            // decimal issueQuantity= Convert.ToDecimal(IssuedQuantity);
                            decimal? requiredQuantity = IssuedQuantity * ingridientQuantity;
                            itemRequiredViewModel.RequiredQuantity = requiredQuantity;
                        }

                        itemRequiredViewModelList.Add(itemRequiredViewModel);
                    }
                }

                return PartialView("_OtherItemsReq", itemRequiredViewModelList);
            }

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


        [HttpPost]
        public ActionResult IssueIngridients(string ItemName, decimal? IssuedQuantity)
        {
            IssueItems issueItems = new IssueItems();
            issueItems.IssueType = "Issue";
            StoreItems storeItems = _dbContext.StoreItems.Find(issueItems.StoreItemsId);
            decimal? remaningItems = storeItems.BalanceQuantity - issueItems.IssuedQuantity;
            issueItems.RemainingItem = remaningItems;
            issueItems.Id = Guid.NewGuid();

            // updating store items balance items
            storeItems.BalanceQuantity = remaningItems;
            _dbContext.Entry(storeItems).State = EntityState.Modified;

            _dbContext.IssueItems.Add(issueItems);
            _dbContext.SaveChanges();
            return Json(new { success = true });


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
