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
using TreeUtility;

namespace Accounts.Web.Controllers
{
    public class TaxCategoriesController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        private List<TaxCategory> GetListOfNodes()
        {
            List<TaxCategory> sourceCategories = _dbContext.TaxCategories.ToList();
            List<TaxCategory> categories = new List<TaxCategory>();
            foreach (TaxCategory sourceCategory in sourceCategories)
            {
                TaxCategory c = new TaxCategory();
                c.Id = sourceCategory.Id;
                c.CategoryName = sourceCategory.CategoryName;
                if (sourceCategory.ParentCategoryId != null)
                {
                    c.Parent = new TaxCategory();
                    c.Parent.Id = (int)sourceCategory.ParentCategoryId;
                }
                categories.Add(c);
            }
            return categories;
        }
        private string EnumerateNodes(TaxCategory parent)
        {
            // Init an empty string
            string content = String.Empty;

            // Add <li> category name
            content += "<li class=\"treenode\">";
            content += parent.CategoryName;
            content += String.Format("<a href=\"/TaxCategories/Edit/{0}\" class=\"btn btn-primary btn-xs treenodeeditbutton\">Edit</a>", parent.Id);
            content += String.Format("<a href=\"/TaxCategories/Delete/{0}\" class=\"btn btn-danger btn-xs treenodedeletebutton\">Delete</a>", parent.Id);

            // If there are no children, end the </li>
            if (parent.Children.Count == 0)
                content += "</li>";
            else   // If there are children, start a <ul>
                content += "<ul>";

            // Loop one past the number of children
            int numberOfChildren = parent.Children.Count;
            for (int i = 0; i <= numberOfChildren; i++)
            {
                // If this iteration's index points to a child,
                // call this function recursively
                if (numberOfChildren > 0 && i < numberOfChildren)
                {
                    TaxCategory child = parent.Children[i];
                    content += EnumerateNodes(child);
                }

                // If this iteration's index points past the children, end the </ul>
                if (numberOfChildren > 0 && i == numberOfChildren)
                    content += "</ul>";
            }

            // Return the content
            return content;
        }
        public ActionResult Index()
        {
            // Start the outermost list
            string fullString = "<ul>";

            IList<TaxCategory> listOfNodes = GetListOfNodes();
            IList<TaxCategory> topLevelCategories = TreeHelper.ConvertToForest(listOfNodes);

            foreach (var category in topLevelCategories)
                fullString += EnumerateNodes(category);

            // End the outermost list
            fullString += "</ul>";
            return View((object)fullString);
        }
      
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxCategory taxCategory = _dbContext.TaxCategories.Find(id);
            if (taxCategory == null)
            {
                return HttpNotFound();
            }
            return View(taxCategory);
        }
    
        public ActionResult Create()
        {
            ViewBag.ParentCategorySelectList = new SelectList(_dbContext.TaxCategories, "Id", "CategoryName");
            return View();
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParentCategoryId,CategoryName")] TaxCategory taxCategory)
        {
            if (ModelState.IsValid)
            {
                _dbContext.TaxCategories.Add(taxCategory);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentCategorySelectList = new SelectList(_dbContext.TaxCategories, "Id", "CategoryName");
            return View(taxCategory);
        }

     
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxCategory taxCategory = _dbContext.TaxCategories.Find(id);
            if (taxCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentCategorySelectList = new SelectList(_dbContext.TaxCategories, "Id", "CategoryName");
            return View(taxCategory);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParentCategoryId,CategoryName")] TaxCategory taxCategory)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(taxCategory).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentCategorySelectList = new SelectList(_dbContext.TaxCategories, "Id", "CategoryName");
            return View(taxCategory);
        }

       
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxCategory taxCategory = _dbContext.TaxCategories.Find(id);
            if (taxCategory == null)
            {
                return HttpNotFound();
            }
            return View(taxCategory);
        }

     
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaxCategory taxCategory = _dbContext.TaxCategories.Find(id);
            _dbContext.TaxCategories.Remove(taxCategory);
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
