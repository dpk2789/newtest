using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accounts.Web.ViewModel
{
    public class CompoundItemIngredientViewModel
    {
        public Guid CompoundItemId { get; set; }
        public Guid ItemId { get; set; }
        public decimal UnitQuantity { get; set; }
    }
}