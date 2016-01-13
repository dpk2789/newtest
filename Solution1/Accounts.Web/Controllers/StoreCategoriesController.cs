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
    public class StoreCategoriesController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        private List<StoreCategory> GetListOfNodes()
        {
            List<StoreCategory> sourceCategories = _dbContext.StoreCategories.ToList();
            List<StoreCategory> categories = new List<StoreCategory>();
            foreach (StoreCategory sourceCategory in sourceCategories)
            {
                StoreCategory c = new StoreCategory();
                c.Id = sourceCategory.Id;
                c.CategoryName = sourceCategory.CategoryName;
                if (sourceCategory.ParentCategoryId != null)
                {
                    c.Parent = new StoreCategory();
                    c.Parent.Id = (int)sourceCategory.ParentCategoryId;
                }
                categories.Add(c);
            }
            return categories;
        }
         private string EnumerateNodes(StoreCategory parent)
        {
            // Init an empty string
            string content = String.Empty;

            // Add <li> category name
            content += "<li class=\"treenode\">";
            content += parent.CategoryName;
            content += String.Format("<a href=\"/StoreCategories/Edit/{0}\" class=\"btn btn-primary btn-xs treenodeeditbutton\">Edit</a>", parent.Id);
            content += String.Format("<a href=\"/StoreCategories/Delete/{0}\" class=\"btn btn-danger btn-xs treenodedeletebutton\">Delete</a>", parent.Id);

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
                    StoreCategory child = parent.Children[i];
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

            IList<StoreCategory> listOfNodes = GetListOfNodes();
            IList<StoreCategory> topLevelCategories = TreeHelper.ConvertToForest(listOfNodes);

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
            StoreCategory storeCategory = _dbContext.StoreCategories.Find(id);
            if (storeCategory == null)
            {
                return HttpNotFound();
            }
            return View(storeCategory);
        }

        public ActionResult Create()
        {
            ViewBag.ParentCategorySelectList = new SelectList(_dbContext.StoreCategories, "Id", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParentCategoryId,CategoryName")] StoreCategory storeCategory)
        {
            if (ModelState.IsValid)
            {
                _dbContext.StoreCategories.Add(storeCategory);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(storeCategory);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreCategory storeCategory = _dbContext.StoreCategories.Find(id);
            ViewBag.ParentCategorySelectList = new SelectList(_dbContext.StoreCategories, "Id", "CategoryName");
            if (storeCategory == null)
            {
                return HttpNotFound();
            }
            return View(storeCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParentCategoryId,CategoryName")] StoreCategory storeCategory)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(storeCategory).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storeCategory);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreCategory storeCategory = _dbContext.StoreCategories.Find(id);
            if (storeCategory == null)
            {
                return HttpNotFound();
            }
            return View(storeCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StoreCategory storeCategory = _dbContext.StoreCategories.Find(id);
            _dbContext.StoreCategories.Remove(storeCategory);
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
