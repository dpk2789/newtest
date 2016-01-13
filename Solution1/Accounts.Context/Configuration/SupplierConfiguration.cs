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
    public class SupplierConfiguration : EntityTypeConfiguration<Supplier>
    {
        public SupplierConfiguration()
        {
            this.ToTable("tbl_Supplier");           
            Property(ii => ii.AccountName).HasMaxLength(80).IsRequired()
           .HasColumnAnnotation("Index",new IndexAnnotation(new IndexAttribute("AK_Supplier_AccountName") { IsUnique = true }));

        }
    }
}
