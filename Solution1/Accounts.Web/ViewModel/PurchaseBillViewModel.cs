using Accounts.Model.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accounts.Web.ViewModel
{
    public class PurchaseBillViewModel
    {
        public Guid? Id { get; set; }
        public string BillInvoice { get; set; }
        public string SupplierInvoice { get; set; }
        public DateTime? BillDate { get; set; }
        public decimal? TaxWithExtendedPrice { get; set; }
        public decimal? BillTotal { get; set; }
        public decimal? ItemTotal { get; set; }
        public virtual List<PurchaseItems> PurchaseItems { get; set; }
        public Guid SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}