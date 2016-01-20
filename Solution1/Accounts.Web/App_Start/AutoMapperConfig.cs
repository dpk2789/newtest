
using Accounts.Model.Model;
using Accounts.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Accounts.Web
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            AutoMapper.Mapper.CreateMap<Supplier, SupplierViewModel>();
            AutoMapper.Mapper.CreateMap<SupplierViewModel, Supplier>();
            AutoMapper.Mapper.CreateMap<PurchaseItems, PurchaseItemsViewModel>();
            AutoMapper.Mapper.CreateMap<PurchaseItemsViewModel, PurchaseItems>();
            AutoMapper.Mapper.CreateMap<StoreItems, StoreItemsViewModel>();
            AutoMapper.Mapper.CreateMap<StoreItemsViewModel, StoreItems>();
            AutoMapper.Mapper.CreateMap<IssueItems, IssueItemsViewModel>();
            AutoMapper.Mapper.CreateMap<IssueItemsViewModel, IssueItems>();
            AutoMapper.Mapper.CreateMap<PurchaseBill, PurchaseBillViewModel>();
            AutoMapper.Mapper.CreateMap<PurchaseBillViewModel, PurchaseBill>();
            AutoMapper.Mapper.CreateMap<AutomaticInvoiceForm, AutomaticInvoiceFormViewModel>();
            AutoMapper.Mapper.CreateMap<AutomaticInvoiceFormViewModel, AutomaticInvoiceForm>();
            AutoMapper.Mapper.CreateMap<StoreItems, StockBookViewModel>();
            AutoMapper.Mapper.CreateMap<StockBookViewModel, StoreItems>();
            AutoMapper.Mapper.CreateMap<Item, ItemViewModel>();
            AutoMapper.Mapper.CreateMap<ItemViewModel, Item>();
        }
    }
}