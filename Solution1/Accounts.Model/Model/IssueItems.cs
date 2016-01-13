using Accounts.Model.Common;
using Accounts.Model.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Model
{
    public class IssueItems : AuditableEntity<Guid>
    {       
        public string IssueType { get; set; }
        public DateTime? IssueDate { get; set; }
        public string IssueInvoice { get; set; }
       // public string InwardInvoice { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeName { get; set; }
        public string Remark { get; set; }
        public decimal? IssuedQuantity { get; set; }
        public decimal? InwardQuantity { get; set; }
        public decimal? RemainingItem { get; set; }
        public Guid StoreItemsId { get; set; }
        public virtual StoreItems StoreItems { get; set; }

    }
}
