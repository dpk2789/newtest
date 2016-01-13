using Accounts.Model.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accounts.Web.ViewModel
{
    public class ItemViewModel
    {
        [Required(ErrorMessage = "You must enter an Item Code.")]
        [StringLength(15, ErrorMessage = "The Item Code must be 20 characters or shorter.")]
        [Display(Name = "Item Code")]
        public string Code { get; set; }

        [Required(ErrorMessage = "You must enter a Name.")]
        [StringLength(80, ErrorMessage = "The Name must be 80 characters or shorter.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
        public virtual ItemCategory ItemCategory { get; set; }

        [Display(Name = "Category")]
        public int ItemCategoryId { get; set; }
        public virtual Unit Unit { get; set; }
        public Guid UnitId { get; set; }
        public string TaxType { get; set; }
        public string ItemType { get; set; }
        public List<SelectListItem> getItemTypeList()
        {
            List<SelectListItem> myList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value="1",Text="Simple"},
                 new SelectListItem{ Value="2",Text="Compound"},
              
             };
            myList = data.ToList();
            return myList;
        }
    }
}