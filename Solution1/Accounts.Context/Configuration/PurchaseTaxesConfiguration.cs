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
    public class PurchaseTaxesConfiguration : EntityTypeConfiguration<PurchaseTaxes>
    {
        public PurchaseTaxesConfiguration()
        {
            this.ToTable("tbl_PurchaseTaxes");
        }
    }
}
