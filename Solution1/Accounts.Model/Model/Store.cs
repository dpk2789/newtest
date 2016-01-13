using Accounts.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Model
{
    public class Store : AuditableEntity<Guid>
    {
        public string Name { get; set; }
        public Guid WareHouseId { get; set; }
        public virtual WareHouse WareHouse { get; set; }
       // public virtual List<PurchaseItems> PurchaseItems { get; set; }
        public virtual List<StoreItems> StoreItems { get; set; }
        public virtual List<PurchaseItems> PurchaseItems { get; set; }
    }

}
