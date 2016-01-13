using Accounts.Model.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Context.Configuration
{
    public class PurchaseBillConfiguration : EntityTypeConfiguration<PurchaseBill>
    {
        public PurchaseBillConfiguration()
        {
            this.ToTable("tbl_PurchasBill");
            Property(ii => ii.BillInvoice).HasMaxLength(80).IsRequired()
           .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("AK_PurchaseBill_BillInvoice") { IsUnique = true }));

            Property(wo => wo.BillTotal).HasPrecision(18, 2).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(wo => wo.TaxWithExtendedPrice).HasPrecision(18, 2).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

        }
    }
}
