using Accounts.Context.Configuration;
using Accounts.Model.Common;
using Accounts.Model.Model;
using Accounts.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Accounts.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
    
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ItemCategory> ItemCategoryies { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<WareHouse> WareHouses { get; set; }
        public DbSet<PurchaseBill> PurchaseBills { get; set; }
        public DbSet<PurchaseItems> PurchaseItems { get; set; }
        public DbSet<TaxCategory> TaxCategories { get; set; }
        public DbSet<Tax> Taxies { get; set; }
        public DbSet<PurchaseTaxes> PurchaseTaxes { get; set; }
        public DbSet<StoreCategory> StoreCategories { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<StoreItems> StoreItems { get; set; }
        public DbSet<IssueItems> IssueItems { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<AutomaticInvoiceForm> AutomaticInvoiceForm { get; set; }
        public DbSet<CompoundItemIngredient> CompoundItemIngredients { get; set; }
        public DbSet<UnitRelations> UnitRelations { get; set; }
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SupplierConfiguration());
            modelBuilder.Configurations.Add(new ItemCategoryConfiguration());
            modelBuilder.Configurations.Add(new UnitConfiguration());
            modelBuilder.Configurations.Add(new ItemConfiguration());
            modelBuilder.Configurations.Add(new WareHouseConfiguration());
            modelBuilder.Configurations.Add(new PurchaseBillConfiguration());
            modelBuilder.Configurations.Add(new PurchaseItemsConfiguration());
            modelBuilder.Configurations.Add(new TaxCategoriesConfiguration());
            modelBuilder.Configurations.Add(new TaxConfiguration());
            modelBuilder.Configurations.Add(new PurchaseTaxesConfiguration());
            modelBuilder.Configurations.Add(new StoreCategoryConfiguration());
            modelBuilder.Configurations.Add(new StoreConfiguration());
            modelBuilder.Configurations.Add(new StoreItemsConfiguration());
            modelBuilder.Configurations.Add(new IssueItemsConfiguration());
            modelBuilder.Configurations.Add(new SettingsConfiguration());
            modelBuilder.Configurations.Add(new AutomaticInvoiceFormConfiguration());
            modelBuilder.Configurations.Add(new CompoundItemIngredientConfiguration());
            modelBuilder.Configurations.Add(new UnitRelationsConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity
                    && (x.State == System.Data.Entity.EntityState.Added || x.State == System.Data.Entity.EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                IAuditableEntity entity = entry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    string identityName = Thread.CurrentPrincipal.Identity.Name;
                    DateTime now = DateTime.Now;

                    if (entry.State == System.Data.Entity.EntityState.Added)
                    {
                        entity.CreatedBy = identityName;
                        entity.CreatedDate = now;
                    }
                    else
                    {
                        base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }

                    entity.UpdatedBy = identityName;
                    entity.UpdatedDate = now;
                }
            }

            return base.SaveChanges();
        }
    }
}
