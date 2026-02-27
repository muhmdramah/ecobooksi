using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobooksi.Models.Models
{
    [Table("Companies")]
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [DisplayNameAttribute("Company Name")]
        public string CompanyName { get; set; }

        [DisplayNameAttribute("Street Address")]
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        [DisplayNameAttribute("Postal Code")]
        public string PostalCode { get; set; }


        [DisplayNameAttribute("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
