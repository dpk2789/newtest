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
    public class StoreConfiguration : EntityTypeConfiguration<Store>
    {
        public StoreConfiguration()
        {
            this.ToTable("tbl_Store");
            Property(ii => ii.Name).HasMaxLength(80).IsRequired()
           .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("AK_Store_Name") { IsUnique = true }));

        }
    }
}
