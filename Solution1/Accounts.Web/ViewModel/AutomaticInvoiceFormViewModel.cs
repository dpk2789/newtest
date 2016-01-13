using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accounts.Web.ViewModel
{
    public class AutomaticInvoiceFormViewModel
    {
        public int? Id { get; set; }
        public string Type { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public double Numbering { get; set; }
        public string AutomaticPurchaseInvoice { get; set; }
        public List<SelectListItem> AutomaticPurchaseInvoiceList { get; set; }
        public List<SelectListItem> getAutomaticPurchaseInvoice()
        {
            List<SelectListItem> myList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value="1",Text="True"},
                 new SelectListItem{ Value="2",Text="False"},
              
             };
            myList = data.ToList();
            return myList;
        }
    }
}