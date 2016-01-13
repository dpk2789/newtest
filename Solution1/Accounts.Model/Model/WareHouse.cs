using Accounts.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Model
{
    public class WareHouse : AuditableEntity<Guid>
    {
        public string Name { get; set; }
        public string PlotNumber { get; set; }
        public string StreetName { get; set; }
        public string LandMark { get; set; }
        public string Colony { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string AddressBlock
        {
            get
            {
                string addressBlock = string.Format("{0}<br/>{1}, {2}, {3} ,{4} </br> {5} ,{6} ,{7}", Name, PlotNumber, StreetName, LandMark, Colony, City, State, ZipCode).Trim();
                return addressBlock == "<br/>," ? string.Empty : addressBlock;
            }
        }

    
        //public virtual List<PurchaseItems> PurchaseItems { get; set; }
        public virtual List<Store> Stores { get; set; }


    }
}
