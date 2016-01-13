using Accounts.Model.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accounts.Web.ViewModel
{
    public class SupplierViewModel
    {
        public Guid? Id { get; set; }
        public string AccountName { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string FathersName { get; set; }
        public string CustomerType { get; set; }


        public string OfficePlotNumber { get; set; }
        public string OfficeStreetName { get; set; }
        public string OfficeLandMark { get; set; }
        public string OfficeColony { get; set; }
        public string OfficeCity { get; set; }
        public string OfficeState { get; set; }
        public string OfficeZipCode { get; set; }

        public string ShippingPlotNumber { get; set; }
        public string ShippingStreetName { get; set; }
        public string ShippingLandMark { get; set; }
        public string ShippingColony { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingZipCode { get; set; }


        public string Email { get; set; }
        public string Mobile { get; set; }
        public string EmpergencyContact { get; set; }



        public string MainImageName { get; set; }
        public string ImageExtention { get; set; }

        public HttpPostedFileBase MainImageNameFile { get; set; }
        public string SalesTaxNumber { get; set; }
        public string PANNumber { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public string AddressBlock
        {
            get
            {
                string addressBlock = string.Format("{0}<br/>{1}, {2}, {3} ,{4} </br> {5} ,{6} ,{7}", FullName, OfficePlotNumber, OfficeStreetName, OfficeLandMark, OfficeColony, OfficeCity, OfficeState, OfficeZipCode).Trim();
                return addressBlock == "<br/>," ? string.Empty : addressBlock;
            }
        }
        public bool? Status { get; set; }

        public virtual IEnumerable<PurchaseBillViewModel> PurchaseBillViewModel { get; set; } 
    }
}