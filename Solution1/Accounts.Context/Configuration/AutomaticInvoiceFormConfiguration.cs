using Accounts.Model.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Context.Configuration
{
    public class AutomaticInvoiceFormConfiguration : EntityTypeConfiguration<AutomaticInvoiceForm>
    {
        public AutomaticInvoiceFormConfiguration()
        {
            this.ToTable("tbl_AutomaticInvoiceForm");
          
        }
    }
}
