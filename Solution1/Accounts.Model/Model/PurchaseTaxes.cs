using Accounts.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Model
{
    public class PurchaseTaxes : AuditableEntity<Guid>
    {
        public string TaxCode { get; set; }
        public string Name { get; set; }
        public decimal? Percent { get; set; }
        public Guid PurchaseBillId { get; set; }
        public virtual PurchaseBill PurchaseBill { get; set; }
    }
}
