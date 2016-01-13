using Accounts.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Model
{
    public class Unit : AuditableEntity<Guid>
    {
        public string Name { get; set; }
      
    }
}
