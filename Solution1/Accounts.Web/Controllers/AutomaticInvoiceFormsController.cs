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
    public class AutomaticInvoiceFormsController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(_dbContext.AutomaticInvoiceForm.ToList());
        }


        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AutomaticInvoiceForm automaticInvoiceForm = _dbContext.AutomaticInvoiceForm.Find(id);
            if (automaticInvoiceForm == null)
            {
                return HttpNotFound();
            }
            return View(automaticInvoiceForm);
        }


        //public ActionResult Create(string type)
        //{
        //    AutomaticInvoiceForm automaticPurchaseInvoiceForm = new AutomaticInvoiceForm();
        //    automaticPurchaseInvoiceForm.Type = type;
        //    return View(automaticPurchaseInvoiceForm);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Type,Prefix,Suffix,Numbering")] AutomaticInvoiceForm automaticInvoiceForm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        automaticInvoiceForm.Id = Guid.NewGuid();
        //        automaticInvoiceForm.Type = "Purchase";
        //        _dbContext.AutomaticInvoiceForm.Add(automaticInvoiceForm);
        //        _dbContext.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(automaticInvoiceForm);
        //}

        public ActionResult Edit(string type)
        {
            AutomaticInvoiceForm automaticInvoiceForm = _dbContext.AutomaticInvoiceForm.Where(a => a.Type == type).FirstOrDefault();
            AutomaticInvoiceFormViewModel viewModel = Mapper.Map<AutomaticInvoiceFormViewModel>(automaticInvoiceForm);
            if (automaticInvoiceForm.AutomaticPurchaseInvoice == true)
            {
                ViewBag.AutomaticPurchaseInvoice = new SelectList(viewModel.getAutomaticPurchaseInvoice(), "Value", "Text", "1");
            }
            else
            {
                ViewBag.AutomaticPurchaseInvoice = new SelectList(viewModel.getAutomaticPurchaseInvoice(), "Value", "Text", "2");
            }
           
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AutomaticInvoiceFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //AutomaticInvoiceForm automaticInvoiceForm = Mapper.Map<AutomaticInvoiceForm>(viewModel);
                AutomaticInvoiceForm automaticInvoiceForm = _dbContext.AutomaticInvoiceForm.Where(a => a.Type == viewModel.Type).FirstOrDefault();
                automaticInvoiceForm.Numbering = viewModel.Numbering;
                automaticInvoiceForm.Prefix = viewModel.Prefix;
                automaticInvoiceForm.Suffix = viewModel.Suffix;
                if (viewModel.AutomaticPurchaseInvoice == "1")
                {
                    automaticInvoiceForm.AutomaticPurchaseInvoice = true;
                }
                else
                {
                    automaticInvoiceForm.AutomaticPurchaseInvoice = false;
                }

                _dbContext.Entry(automaticInvoiceForm).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(viewModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AutomaticInvoiceForm automaticInvoiceForm = _dbContext.AutomaticInvoiceForm.Find(id);
            if (automaticInvoiceForm == null)
            {
                return HttpNotFound();
            }
            return View(automaticInvoiceForm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AutomaticInvoiceForm automaticInvoiceForm = _dbContext.AutomaticInvoiceForm.Find(id);
            _dbContext.AutomaticInvoiceForm.Remove(automaticInvoiceForm);
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
