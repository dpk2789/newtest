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
    public class TaxConfiguration : EntityTypeConfiguration<Tax>
    {
        public TaxConfiguration()
        {
            this.ToTable("tbl_Tax");
            Property(ii => ii.Name).HasMaxLength(80).IsRequired()
           .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("AK_Tax_Name") { IsUnique = true }));

        }
    }
}
