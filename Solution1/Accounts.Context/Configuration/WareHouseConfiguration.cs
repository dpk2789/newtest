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
    public class WareHouseConfiguration : EntityTypeConfiguration<WareHouse>
    {
        public WareHouseConfiguration()
        {
            this.ToTable("tbl_WareHouse");
            Property(ii => ii.Name).HasMaxLength(80).IsRequired()
           .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("AK_WareHouse_Name") { IsUnique = true }));

        }
    }
}
