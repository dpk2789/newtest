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
    public class StoresController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        public ActionResult Index()
        {
            var store = _dbContext.Store.Include(s => s.WareHouse);
            return View(store.ToList());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = _dbContext.Store.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        public ActionResult Create()
        {
            ViewBag.WareHouseId = new SelectList(_dbContext.WareHouses, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,WareHouseId")] Store store)
        {
            if (ModelState.IsValid)
            {
                store.Id = Guid.NewGuid();
                _dbContext.Store.Add(store);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WareHouseId = new SelectList(_dbContext.WareHouses, "Id", "Name", store.WareHouseId);
            return View(store);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = _dbContext.Store.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            ViewBag.WareHouseId = new SelectList(_dbContext.WareHouses, "Id", "Name", store.WareHouseId);
            return View(store);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,WareHouseId")] Store store)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(store).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WareHouseId = new SelectList(_dbContext.WareHouses, "Id", "Name", store.WareHouseId);
            return View(store);
        }
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = _dbContext.Store.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Store store = _dbContext.Store.Find(id);
            _dbContext.Store.Remove(store);
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
