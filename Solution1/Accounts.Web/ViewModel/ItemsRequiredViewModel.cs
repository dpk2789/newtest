using Accounts.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accounts.Web.ViewModel
{
    public class ItemsRequiredViewModel
    {
        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }
        public Guid IngridentId { get; set; }
        public string IngridentName { get; set; }
        public string UnitName { get; set; }
        public decimal? RequiredQuantity { get; set; }
        public Guid IngridentUnitId { get; set; }
        public virtual Unit Unit { get; set; }
    }
}