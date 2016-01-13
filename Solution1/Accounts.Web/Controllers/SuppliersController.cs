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
using System.IO;
using ImageResizer;
using System.Data.Entity.Infrastructure;
using Accounts.Web.Helpers;
using AutoMapper;

namespace Accounts.Web.Controllers
{
    public class SuppliersController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        public ActionResult Index()
        {            
            return View(_dbContext.Suppliers.ToList().OrderBy(x=>x.AccountName));
        }


        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = _dbContext.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SupplierViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //  Product product = Mapper.Map<Product>(viewModel);
                    Supplier supplier = AutoMapper.Mapper.Map<Supplier>(viewModel);
                    if (viewModel.MainImageNameFile != null)
                    {
                        string fName = "";
                        HttpPostedFileBase file = viewModel.MainImageNameFile;
                        fName = viewModel.MainImageNameFile.FileName;
                        string ImageNameWithOutExtention = System.IO.Path.GetFileNameWithoutExtension(fName);
                        string extension = Path.GetExtension(fName);

                        var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Customers", Server.MapPath(@"\")));
                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");
                        var fileName1 = Path.GetFileName(file.FileName);
                        bool isExists = System.IO.Directory.Exists(pathString);
                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);
                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(path);


                        var versions = new Dictionary<string, string>();
                        var imagePath = string.Format("{0}\\{1}", pathString, ImageNameWithOutExtention);

                        versions.Add("_small", "maxwidth=100&maxheight=100&format=jpg");
                        versions.Add("_medium", "maxwidth=500&maxheight=500&format=jpg");
                        versions.Add("_large", "maxwidth=900&maxheight=900&format=jpg");
                        foreach (var suffix in versions.Keys)
                        {
                            file.InputStream.Seek(0, SeekOrigin.Begin);
                            ImageBuilder.Current.Build(
                                new ImageJob(
                                    file.InputStream,
                                   imagePath + suffix,
                                    new Instructions(versions[suffix]),
                                    false,
                                    true));
                        }

                        supplier.MainImageName = ImageNameWithOutExtention;
                        supplier.ImageExtention = extension;
                    }
                    //customer.CompanyId = CompanyCookie.CompId;
                    supplier.Id = Guid.NewGuid();
                    _dbContext.Suppliers.Add(supplier);
                    _dbContext.SaveChanges();

                    TempData["MessageToClientSuccess"] = "SuccessFully Saved";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException dbex)
                {
                    // var error = new ModelStateException(dbex);
                    // Log4NetHelper.Log(String.Format("Cannot Create Deparment {0} ", viewModel.Id), LogLevel.ERROR, "Department", viewModel.Id, User.Identity.Name, dbex);
                    var msg = new ModelStateException(dbex);
                    TempData["MessageToClientError"] = msg;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    //viewModel.getAllGenderList = viewModel.getGenderList();
                    //viewModel.getAllCustomerTypeList = viewModel.getCustomerTypeList();
                    return View(viewModel);
                }
            }

            return View(viewModel);
        }


        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = _dbContext.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            SupplierViewModel viewModel = AutoMapper.Mapper.Map<SupplierViewModel>(supplier);            
            List<PurchaseBillViewModel> purchaseBillViewModelList = new List<PurchaseBillViewModel>();
            var purchaseBills = _dbContext.PurchaseBills.ToList();
            foreach (var bill in purchaseBills)
            {
                PurchaseBillViewModel purchaseBillViewModel = new PurchaseBillViewModel();
                purchaseBillViewModel.Id = bill.Id;
                purchaseBillViewModel.BillInvoice = bill.BillInvoice;
                purchaseBillViewModel.BillTotal = bill.BillTotal;
                purchaseBillViewModel.BillDate = bill.BillDate;
                purchaseBillViewModel.SupplierInvoice = bill.SupplierInvoice;
                purchaseBillViewModelList.Add(purchaseBillViewModel);
            }
            IEnumerable<PurchaseBillViewModel> en = purchaseBillViewModelList;
            viewModel.PurchaseBillViewModel = en;
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SupplierViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Supplier supplier = Mapper.Map<Supplier>(viewModel);
                if (viewModel.MainImageNameFile != null)
                {
                    string fName = "";
                    HttpPostedFileBase file = viewModel.MainImageNameFile;
                    fName = viewModel.MainImageNameFile.FileName;
                    string ImageNameWithOutExtention = System.IO.Path.GetFileNameWithoutExtension(fName);
                    string extension = Path.GetExtension(fName);

                    var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Customers", Server.MapPath(@"\")));
                    string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");
                    var fileName1 = Path.GetFileName(file.FileName);
                    bool isExists = System.IO.Directory.Exists(pathString);
                    if (!isExists)
                        System.IO.Directory.CreateDirectory(pathString);
                    var path = string.Format("{0}\\{1}", pathString, file.FileName);
                    file.SaveAs(path);


                    var versions = new Dictionary<string, string>();
                    var imagePath = string.Format("{0}\\{1}", pathString, ImageNameWithOutExtention);

                    versions.Add("_small", "maxwidth=100&maxheight=100&format=jpg");
                    versions.Add("_medium", "maxwidth=500&maxheight=500&format=jpg");
                    versions.Add("_large", "maxwidth=900&maxheight=900&format=jpg");
                    foreach (var suffix in versions.Keys)
                    {
                        file.InputStream.Seek(0, SeekOrigin.Begin);
                        ImageBuilder.Current.Build(
                            new ImageJob(
                                file.InputStream,
                               imagePath + suffix,
                                new Instructions(versions[suffix]),
                                false,
                                true));
                    }

                    supplier.MainImageName = ImageNameWithOutExtention;
                    supplier.ImageExtention = extension;
                }

                _dbContext.Entry(supplier).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }


        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = _dbContext.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Supplier supplier = _dbContext.Suppliers.Find(id);
            _dbContext.Suppliers.Remove(supplier);
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
