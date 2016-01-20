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
    public class IssueItemsController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        public ActionResult Index(Guid storeItemsId, string itemname)
        {
            var issueItems = _dbContext.IssueItems.Where(s => s.StoreItems.Name == itemname && s.StoreItemsId == storeItemsId);
            List<IssueItemsViewModel> vmList = new List<IssueItemsViewModel>();
            foreach (var item in issueItems)
            {
                IssueItemsViewModel vm = new IssueItemsViewModel();
                vm.IssueInvoice = item.IssueInvoice;
                vm.ItemName = itemname;
                vm.DepartmentName = item.DepartmentName;
                vm.EmployeeName = item.EmployeeName;
                vm.IssuedQuantity = item.IssuedQuantity;
                vm.InwardQuantity = item.InwardQuantity;
                vm.IssueDate = item.IssueDate;
                vm.RemainingItem = item.RemainingItem;
                vm.IssueType = item.IssueType;
                vm.Remark = item.Remark;
                vmList.Add(vm);
            }
            return PartialView("_Index", vmList);
        }
        public ActionResult CurrentStock()
        {
            ViewBag.ItemId = new SelectList(_dbContext.Items, "Id", "Name");
            ViewBag.CategoryId = new SelectList(_dbContext.ItemCategoryies, "Id", "CategoryName");
            ViewBag.StoreId = new SelectList(_dbContext.Store, "Id", "Name");
            List<StockBookViewModel> stockBookViewModelList = new List<ViewModel.StockBookViewModel>();

            var issueItems = _dbContext.IssueItems.ToList();
            var storeItems = _dbContext.StoreItems.OrderBy(x => x.ItemAddedDate);

            // var viewModel = Mapper.Map<IEnumerable<StockBookViewModel>>(storeItems);
            foreach (var storeItem in storeItems)
            {
                StockBookViewModel stockBookViewModelNew = new StockBookViewModel();
                stockBookViewModelNew.ItemAddedDate = storeItem.ItemAddedDate;
                stockBookViewModelNew.Name = storeItem.Name;
                stockBookViewModelNew.Quantity = storeItem.Quantity;
                stockBookViewModelNew.UnitPrice = storeItem.UnitPrice;
                stockBookViewModelNew.ExtendedPrice = storeItem.ExtendedPrice;
                stockBookViewModelNew.BalanceQuantity = storeItem.Quantity;
                stockBookViewModelNew.Type = storeItem.Type;
                stockBookViewModelList.Add(stockBookViewModelNew);
                var issue = issueItems.Where(x => x.StoreItemsId == storeItem.Id);
                if (issue.Count() == 0)
                {
                    //StockBookViewModel stockBookViewModel = new StockBookViewModel();
                    //stockBookViewModel.ItemAddedDate = storeItem.ItemAddedDate;
                    //stockBookViewModel.Name = storeItem.Name;
                    //stockBookViewModel.Quantity = storeItem.Quantity;
                    //stockBookViewModel.UnitPrice = storeItem.UnitPrice;
                    //stockBookViewModel.ExtendedPrice = storeItem.ExtendedPrice;
                    //stockBookViewModelList.Add(stockBookViewModel);
                }
                else
                {
                    foreach (var issueItem in issue)
                    {
                        StockBookViewModel stockBookViewModel = new StockBookViewModel();
                        stockBookViewModel.ItemAddedDate = issueItem.IssueDate;
                        stockBookViewModel.Name = storeItem.Name;
                        stockBookViewModel.IssuedQuantity = issueItem.IssuedQuantity;
                        stockBookViewModel.BalanceQuantity = issueItem.RemainingItem;
                        stockBookViewModel.Type = issueItem.IssueType;
                        stockBookViewModelList.Add(stockBookViewModel);
                    }

                }
            }

            return View(stockBookViewModelList);
        }
        public ActionResult DisplaySearchResults(Guid? StoreId, int? CategoryId, Guid? ItemId, DateTime? todate, DateTime? fromdate)
        {
            List<StockBookViewModel> stockBookViewModelList = new List<ViewModel.StockBookViewModel>();

            if (StoreId != null)
            {
                if (CategoryId != null)
                {
                    var items = _dbContext.Items.Where(x => x.ItemCategoryId == CategoryId);
                    var viewModel = Mapper.Map<IEnumerable<ItemViewModel>>(items);
                    var issueItems = _dbContext.IssueItems.ToList();
                    if (todate != null && fromdate != null)
                    {
                        foreach (var item in viewModel)
                        {
                            var storeItems = _dbContext.StoreItems.Where(s => s.Code == item.Code && s.StoreId == StoreId);
                            var toFromList = storeItems.Where(d1 => d1.ItemAddedDate >= todate && d1.ItemAddedDate <= fromdate);
                            if (storeItems.Count() != 0)
                            {
                                foreach (var storeItem in toFromList)
                                {
                                    StockBookViewModel stockBookViewModelNew = new StockBookViewModel();
                                    stockBookViewModelNew.ItemAddedDate = storeItem.ItemAddedDate;
                                    stockBookViewModelNew.Name = storeItem.Name;
                                    stockBookViewModelNew.Quantity = storeItem.Quantity;
                                    stockBookViewModelNew.UnitPrice = storeItem.UnitPrice;
                                    stockBookViewModelNew.ExtendedPrice = storeItem.ExtendedPrice;
                                    stockBookViewModelNew.BalanceQuantity = storeItem.Quantity;
                                    stockBookViewModelNew.Type = storeItem.Type;
                                    stockBookViewModelList.Add(stockBookViewModelNew);
                                    var issue = issueItems.Where(x => x.StoreItemsId == storeItem.Id);
                                    if (issue.Count() == 0)
                                    {
                                        //StockBookViewModel stockBookViewModel = new StockBookViewModel();
                                        //stockBookViewModel.ItemAddedDate = storeItem.ItemAddedDate;
                                        //stockBookViewModel.Name = storeItem.Name;
                                        //stockBookViewModel.Quantity = storeItem.Quantity;
                                        //stockBookViewModel.UnitPrice = storeItem.UnitPrice;
                                        //stockBookViewModel.ExtendedPrice = storeItem.ExtendedPrice;
                                        //stockBookViewModelList.Add(stockBookViewModel);
                                    }
                                    else
                                    {
                                        foreach (var issueItem in issue)
                                        {
                                            StockBookViewModel stockBookViewModel = new StockBookViewModel();
                                            stockBookViewModel.ItemAddedDate = issueItem.IssueDate;
                                            stockBookViewModel.Name = storeItem.Name;
                                            stockBookViewModel.IssuedQuantity = issueItem.IssuedQuantity;
                                            stockBookViewModel.BalanceQuantity = issueItem.RemainingItem;
                                            stockBookViewModel.Type = issueItem.IssueType;
                                            stockBookViewModelList.Add(stockBookViewModel);
                                        }

                                    }
                                }
                            }

                        }
                    }
                    else
                    {

                        foreach (var item in viewModel)
                        {
                            var storeItems = _dbContext.StoreItems.Where(s => s.Code == item.Code && s.StoreId == StoreId);
                            if (storeItems.Count() != 0)
                            {
                                foreach (var storeItem in storeItems)
                                {
                                    StockBookViewModel stockBookViewModelNew = new StockBookViewModel();
                                    stockBookViewModelNew.ItemAddedDate = storeItem.ItemAddedDate;
                                    stockBookViewModelNew.Name = storeItem.Name;
                                    stockBookViewModelNew.Quantity = storeItem.Quantity;
                                    stockBookViewModelNew.UnitPrice = storeItem.UnitPrice;
                                    stockBookViewModelNew.ExtendedPrice = storeItem.ExtendedPrice;
                                    stockBookViewModelNew.BalanceQuantity = storeItem.Quantity;
                                    stockBookViewModelNew.Type = storeItem.Type;
                                    stockBookViewModelList.Add(stockBookViewModelNew);
                                    var issue = issueItems.Where(x => x.StoreItemsId == storeItem.Id);
                                    if (issue.Count() == 0)
                                    {
                                        //StockBookViewModel stockBookViewModel = new StockBookViewModel();
                                        //stockBookViewModel.ItemAddedDate = storeItem.ItemAddedDate;
                                        //stockBookViewModel.Name = storeItem.Name;
                                        //stockBookViewModel.Quantity = storeItem.Quantity;
                                        //stockBookViewModel.UnitPrice = storeItem.UnitPrice;
                                        //stockBookViewModel.ExtendedPrice = storeItem.ExtendedPrice;
                                        //stockBookViewModelList.Add(stockBookViewModel);
                                    }
                                    else
                                    {
                                        foreach (var issueItem in issue)
                                        {
                                            StockBookViewModel stockBookViewModel = new StockBookViewModel();
                                            stockBookViewModel.ItemAddedDate = issueItem.IssueDate;
                                            stockBookViewModel.Name = storeItem.Name;
                                            stockBookViewModel.IssuedQuantity = issueItem.IssuedQuantity;
                                            stockBookViewModel.BalanceQuantity = issueItem.RemainingItem;
                                            stockBookViewModel.Type = issueItem.IssueType;
                                            stockBookViewModelList.Add(stockBookViewModel);
                                        }

                                    }
                                }
                            }

                        }
                    }


                }
                else
                {
                    var issueItems = _dbContext.IssueItems.ToList();
                    var storeItems = _dbContext.StoreItems.Where(s => s.StoreId == StoreId);
                    if (todate != null && fromdate != null)
                    {
                        var toFromList = storeItems.Where(d1 => d1.ItemAddedDate >= todate && d1.ItemAddedDate <= fromdate);
                        foreach (var storeItem in toFromList)
                        {
                            StockBookViewModel stockBookViewModelNew = new StockBookViewModel();
                            stockBookViewModelNew.ItemAddedDate = storeItem.ItemAddedDate;
                            stockBookViewModelNew.Name = storeItem.Name;
                            stockBookViewModelNew.Quantity = storeItem.Quantity;
                            stockBookViewModelNew.UnitPrice = storeItem.UnitPrice;
                            stockBookViewModelNew.ExtendedPrice = storeItem.ExtendedPrice;
                            stockBookViewModelNew.BalanceQuantity = storeItem.Quantity;
                            stockBookViewModelNew.Type = storeItem.Type;
                            stockBookViewModelList.Add(stockBookViewModelNew);
                            var issue = issueItems.Where(x => x.StoreItemsId == storeItem.Id);
                            if (issue.Count() == 0)
                            {
                                //StockBookViewModel stockBookViewModel = new StockBookViewModel();
                                //stockBookViewModel.ItemAddedDate = storeItem.ItemAddedDate;
                                //stockBookViewModel.Name = storeItem.Name;
                                //stockBookViewModel.Quantity = storeItem.Quantity;
                                //stockBookViewModel.UnitPrice = storeItem.UnitPrice;
                                //stockBookViewModel.ExtendedPrice = storeItem.ExtendedPrice;
                                //stockBookViewModelList.Add(stockBookViewModel);
                            }
                            else
                            {
                                foreach (var issueItem in issue)
                                {
                                    StockBookViewModel stockBookViewModel = new StockBookViewModel();
                                    stockBookViewModel.ItemAddedDate = issueItem.IssueDate;
                                    stockBookViewModel.Name = storeItem.Name;
                                    stockBookViewModel.IssuedQuantity = issueItem.IssuedQuantity;
                                    stockBookViewModel.BalanceQuantity = issueItem.RemainingItem;
                                    stockBookViewModel.Type = issueItem.IssueType;
                                    stockBookViewModelList.Add(stockBookViewModel);
                                }

                            }
                        }
                    }
                    else
                    {
                        foreach (var storeItem in storeItems)
                        {
                            StockBookViewModel stockBookViewModelNew = new StockBookViewModel();
                            stockBookViewModelNew.ItemAddedDate = storeItem.ItemAddedDate;
                            stockBookViewModelNew.Name = storeItem.Name;
                            stockBookViewModelNew.Quantity = storeItem.Quantity;
                            stockBookViewModelNew.UnitPrice = storeItem.UnitPrice;
                            stockBookViewModelNew.ExtendedPrice = storeItem.ExtendedPrice;
                            stockBookViewModelNew.BalanceQuantity = storeItem.Quantity;
                            stockBookViewModelNew.Type = storeItem.Type;
                            stockBookViewModelList.Add(stockBookViewModelNew);
                            var issue = issueItems.Where(x => x.StoreItemsId == storeItem.Id);
                            if (issue.Count() == 0)
                            {
                                //StockBookViewModel stockBookViewModel = new StockBookViewModel();
                                //stockBookViewModel.ItemAddedDate = storeItem.ItemAddedDate;
                                //stockBookViewModel.Name = storeItem.Name;
                                //stockBookViewModel.Quantity = storeItem.Quantity;
                                //stockBookViewModel.UnitPrice = storeItem.UnitPrice;
                                //stockBookViewModel.ExtendedPrice = storeItem.ExtendedPrice;
                                //stockBookViewModelList.Add(stockBookViewModel);
                            }
                            else
                            {
                                foreach (var issueItem in issue)
                                {
                                    StockBookViewModel stockBookViewModel = new StockBookViewModel();
                                    stockBookViewModel.ItemAddedDate = issueItem.IssueDate;
                                    stockBookViewModel.Name = storeItem.Name;
                                    stockBookViewModel.IssuedQuantity = issueItem.IssuedQuantity;
                                    stockBookViewModel.BalanceQuantity = issueItem.RemainingItem;
                                    stockBookViewModel.Type = issueItem.IssueType;
                                    stockBookViewModelList.Add(stockBookViewModel);
                                }

                            }
                        }
                    }

                    // var viewModel = Mapper.Map<IEnumerable<StockBookViewModel>>(storeItems);
                }
            }


            return PartialView("_CurrentStock", stockBookViewModelList);
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueItems issueItems = _dbContext.IssueItems.Find(id);
            if (issueItems == null)
            {
                return HttpNotFound();
            }
            return View(issueItems);
        }

        public ActionResult Create(Guid storeItemsId)
        {
            StoreItems storeItems = _dbContext.StoreItems.Find(storeItemsId);
            IssueItems issueItems = new IssueItems();
            IssueItemsViewModel viewModel = Mapper.Map<IssueItemsViewModel>(issueItems);
            ViewBag.IssueType = new SelectList(viewModel.getIssueTypeList(), "Value", "Text", "1");
            ViewBag.ItemName = storeItems.Name;
            viewModel.StoreItemsId = storeItemsId;
            viewModel.RemainingItem = storeItems.BalanceQuantity;

            return PartialView("_Create", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IssueInvoice,IssueDate,DepartmentName,EmployeeName,Remark,IssuedQuantity,RemainingItem,StoreItemsId,IssueType")] IssueItemsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                IssueItems issueItems = Mapper.Map<IssueItems>(viewModel);
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
            }

            ViewBag.IssueType = new SelectList(viewModel.getIssueTypeList(), "Value", "Text", "1");
            return View(viewModel);
        }

        public ActionResult Inward(Guid storeItemsId)
        {
            StoreItems storeItems = _dbContext.StoreItems.Find(storeItemsId);
            IssueItems issueItems = new IssueItems();
            IssueItemsViewModel viewModel = Mapper.Map<IssueItemsViewModel>(issueItems);
            ViewBag.IssueType = new SelectList(viewModel.getIssueTypeList(), "Value", "Text", "2");
            ViewBag.ItemName = storeItems.Name;
            viewModel.StoreItemsId = storeItemsId;
            viewModel.RemainingItem = storeItems.BalanceQuantity;
            return PartialView("_Inward", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inward([Bind(Include = "Id,IssueInvoice,IssueDate,DepartmentName,EmployeeName,Remark,InwardQuantity,RemainingItem,StoreItemsId,IssueType")] IssueItemsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                IssueItems issueItems = Mapper.Map<IssueItems>(viewModel);
                issueItems.IssueType = "Return";
                StoreItems storeItems = _dbContext.StoreItems.Find(issueItems.StoreItemsId);
                decimal? remaningItems = storeItems.BalanceQuantity + issueItems.InwardQuantity;
                issueItems.RemainingItem = remaningItems;
                issueItems.Id = Guid.NewGuid();

                // updating store items balance items
                storeItems.BalanceQuantity = remaningItems;
                _dbContext.Entry(storeItems).State = EntityState.Modified;

                _dbContext.IssueItems.Add(issueItems);
                _dbContext.SaveChanges();
                return Json(new { success = true });
            }

            ViewBag.IssueType = new SelectList(viewModel.getIssueTypeList(), "Value", "Text", "1");
            return View(viewModel);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueItems issueItems = _dbContext.IssueItems.Find(id);
            if (issueItems == null)
            {
                return HttpNotFound();
            }
            ViewBag.StoreItemsId = new SelectList(_dbContext.StoreItems, "Id", "Code", issueItems.StoreItemsId);
            return View(issueItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IssueInvoice,IssueDate,DepartmentName,EmployeeName,Remark,IssuedQuantity,RemainingItem,StoreItemsId")] IssueItems issueItems)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(issueItems).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StoreItemsId = new SelectList(_dbContext.StoreItems, "Id", "Code", issueItems.StoreItemsId);
            return View(issueItems);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueItems issueItems = _dbContext.IssueItems.Find(id);
            if (issueItems == null)
            {
                return HttpNotFound();
            }
            return View(issueItems);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            IssueItems issueItems = _dbContext.IssueItems.Find(id);
            _dbContext.IssueItems.Remove(issueItems);
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
