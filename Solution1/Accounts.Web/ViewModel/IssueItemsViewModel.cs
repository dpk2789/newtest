using Accounts.Model.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accounts.Web.ViewModel
{
    public class IssueItemsViewModel
    {
        public string IssueInvoice { get; set; }
        public string InwardInvoice { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        [Required]
        public DateTime? IssueDate { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeName { get; set; }
        public string Remark { get; set; }
        public decimal? IssuedQuantity { get; set; }
        public decimal? InwardQuantity { get; set; }
        public decimal? RemainingItem { get; set; }
        public Guid StoreItemsId { get; set; }
        public virtual StoreItems StoreItems { get; set; }
        public string IssueType { get; set; }
        public List<SelectListItem> IssueTypeList { get; set; }
        public List<SelectListItem> getIssueTypeList()
        {
            List<SelectListItem> myList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value="1",Text="Issue"},
                 new SelectListItem{ Value="2",Text="Inward"},
              
             };
            myList = data.ToList();
            return myList;
        }

    }
}