using Accounts.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Accounts.Web.ViewModel
{
    public class StoreItemsViewModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ExtendedPrice { get; set; }
        public decimal? FORPrice { get; set; }
        public Guid ItemId { get; set; }
        public Guid UnitId { get; set; }
        public virtual Unit Unit { get; set; }
        public Guid StoreId { get; set; }
        public virtual Store Store { get; set; }
        public Guid? PurchaseBillId { get; set; }      
        public decimal? ItemExtentedPriceTotal { get; set; }
        public decimal? BalanceQuantity { get; set; }
        public string BillInvoice { get; set; }
        public string SupplierInvoice { get; set; }
    }
}