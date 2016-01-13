using Accounts.Model.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Context.Configuration
{
    public class StoreItemsConfiguration : EntityTypeConfiguration<StoreItems>
    {
        public StoreItemsConfiguration()
        {
            this.ToTable("tbl_StoreItems");
            Property(p => p.ExtendedPrice).HasPrecision(18, 2).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }
    }
}
