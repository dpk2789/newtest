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
    public class UnitRelationsController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        public ActionResult Index()
        {
            var units = _dbContext.UnitRelations.ToList();
            foreach (var unit in units)
            {
                var bigUnitId = _dbContext.Units.Where(x => x.Id == unit.BigUnitId).FirstOrDefault();
                UnitRelations unitRelations = new UnitRelations();
                ViewBag.BigUnitName = bigUnitId.Name;
            }

            return View(units);
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitRelations unitRelations = _dbContext.UnitRelations.Find(id);
            if (unitRelations == null)
            {
                return HttpNotFound();
            }
            return View(unitRelations);
        }

        public ActionResult Create()
        {
            var units = new SelectList(_dbContext.Units, "Id", "Name");
            ViewBag.BigUnitId = units;
            ViewBag.SmallUnitId = units;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BigUnitId,SmallUnitId,RelationNumber")] UnitRelations unitRelations)
        {
            if (ModelState.IsValid)
            {
                unitRelations.Id = Guid.NewGuid();
                _dbContext.UnitRelations.Add(unitRelations);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unitRelations);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitRelations unitRelations = _dbContext.UnitRelations.Find(id);
            if (unitRelations == null)
            {
                return HttpNotFound();
            }
            return View(unitRelations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BigUnitId,SmallUnitId,RelationNumber,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,IsActive")] UnitRelations unitRelations)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(unitRelations).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unitRelations);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitRelations unitRelations = _dbContext.UnitRelations.Find(id);
            if (unitRelations == null)
            {
                return HttpNotFound();
            }
            return View(unitRelations);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UnitRelations unitRelations = _dbContext.UnitRelations.Find(id);
            _dbContext.UnitRelations.Remove(unitRelations);
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
