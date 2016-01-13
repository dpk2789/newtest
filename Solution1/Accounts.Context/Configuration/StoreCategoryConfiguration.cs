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
    public class StoreCategoryConfiguration : EntityTypeConfiguration<StoreCategory>
    {
        public StoreCategoryConfiguration()
        {
            this.ToTable("tbl_StoreCategory");
            Property(ii => ii.CategoryName).HasMaxLength(80).IsRequired()
           .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("AK_StoreCategory_CategoryName") { IsUnique = true }));

        }
    }
}
