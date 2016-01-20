using Accounts.Model.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Context.Configuration
{
    public class UnitRelationsConfiguration : EntityTypeConfiguration<UnitRelations>
    {
        public UnitRelationsConfiguration()
        {
            this.ToTable("tbl_UnitRelations");
            HasRequired(c => c.Unit).WithMany(d => d.UnitRelations).HasForeignKey(d => new { d.SmallUnitId });
        }
    }
}
