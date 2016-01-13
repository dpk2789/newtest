using Accounts.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Model
{
    public class StoreItems : AuditableEntity<Guid>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ExtendedPrice { get; set; }
        public decimal? BalanceQuantity { get; set; }
        public Guid UnitId { get; set; }
        public virtual Unit Unit { get; set; }      
        public Guid StoreId { get; set; }
        public virtual Store Store { get; set; }
        public Guid? PurchaseBillId { get; set; }
        public virtual IList<IssueItems> IssueItems { get; set; }
      
    }
}
