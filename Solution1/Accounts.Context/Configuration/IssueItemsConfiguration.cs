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
    public class IssueItemsConfiguration : EntityTypeConfiguration<IssueItems>
    {
        public IssueItemsConfiguration()
        {
            this.ToTable("tbl_IssueItems");

            Property(ii => ii.IssueInvoice).HasMaxLength(80).IsRequired()
            .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("AK_IssueItem_BillInvoice,2") { IsUnique = false }));

            Property(ii => ii.IssueType).HasMaxLength(80).IsRequired()
           .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("AK_IssueItem_BillInvoice,1") { IsUnique = false }));
        }
    }
}
