using Accounts.Model.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Context.Configuration
{
    public class CompoundItemIngredientConfiguration : EntityTypeConfiguration<CompoundItemIngredient>
    {
        public CompoundItemIngredientConfiguration()
        {
            this.ToTable("tbl_CompoundItemIngredient");

        }
    }
}
