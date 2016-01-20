using Accounts.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Model
{
    public class Item : AuditableEntity<Guid>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual ItemCategory ItemCategory { get; set; }
        public int ItemCategoryId { get; set; }
        public virtual Unit Unit { get; set; }
        public Guid UnitId { get; set; }
        public string TaxType { get; set; }
        public string ItemType { get; set; }
        public virtual IList<CompoundItemIngredient> CompoundItemIngredients { get; set; }
    }
}
