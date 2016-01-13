using Accounts.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Model
{
    public class PurchaseBill : AuditableEntity<Guid>
    {
        public string BillInvoice { get; set; }
        public string SupplierInvoice { get; set; }
        public DateTime? BillDate { get; set; }
        public decimal? TaxWithExtendedPrice { get; set; }
        public decimal? Freight { get; set; }
        public decimal? Packaging { get; set; }
        public decimal? OtherExpenses { get; set; }
        public decimal? BillTotal { get; set; }
        public virtual List<PurchaseItems> PurchaseItems { get; set; }
        public virtual List<PurchaseTaxes> PurchaseTaxes { get; set; } 
        public Guid SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
     
    }
}
