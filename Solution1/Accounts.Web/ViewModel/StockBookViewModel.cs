using Accounts.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accounts.Web.ViewModel
{
    public class StockBookViewModel
    {
        public DateTime? ItemAddedDate { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ExtendedPrice { get; set; }
        public decimal? BalanceQuantity { get; set; }
        public decimal? IssuedQuantity { get; set; }
        public decimal? InwardQuantity { get; set; }
        public decimal? RemainingItem { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeName { get; set; }
        public string Remark { get; set; }
        public Guid StoreItemsId { get; set; }
        public Guid UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        public Guid StoreId { get; set; }
        public virtual Store Store { get; set; }
        public Guid? PurchaseBillId { get; set; }
        public Guid? PurchaseItemsId { get; set; }
    }
}