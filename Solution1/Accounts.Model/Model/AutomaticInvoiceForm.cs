using Accounts.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Model
{
    public class AutomaticInvoiceForm : AuditableEntity<int>
    {
        public string Type { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public double? Numbering { get; set; }
        public bool? AutomaticPurchaseInvoice { get; set; }
    }
}
