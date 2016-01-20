using Accounts.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Model
{
    public class UnitRelations : AuditableEntity<Guid>
    {
        public Guid? BigUnitId { get; set; }
        public Guid? SmallUnitId { get; set; }
        public virtual Unit Unit { get; set; }
        public decimal RelationNumber { get; set; }

    }
}
