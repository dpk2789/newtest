using Accounts.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Model
{
    public class Settings : AuditableEntity<Guid>
    {
        public bool? AutomaticPurchaseInvoice { get; set; }

    }
}
