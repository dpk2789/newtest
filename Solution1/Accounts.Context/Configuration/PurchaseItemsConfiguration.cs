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
    public class PurchaseItemsConfiguration : EntityTypeConfiguration<PurchaseItems>
    {
        public PurchaseItemsConfiguration()
        {
            this.ToTable("tbl_PurchaseItems");
            Property(p => p.ExtendedPrice).HasPrecision(18, 2).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }
    }
}
