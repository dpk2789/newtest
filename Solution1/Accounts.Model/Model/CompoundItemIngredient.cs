using Accounts.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Model
{
    public class CompoundItemIngredient : AuditableEntity<Guid>
    {
        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }
        public Guid IngridentId { get; set; }
        public string IngridentName { get; set; }      
        public decimal UnitQuantity { get; set; }
        public Guid IngridentUnitId { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
