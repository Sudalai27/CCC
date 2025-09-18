using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class VendorModel
    {
       
        public string _id { get; set; }
        
        public string Code { get; set; }
        public string ShortDescName { get; set; }
        [Required]
        public string Name { get; set; }
      
        public string Name2 { get; set; }
        
        public string Name3 { get; set; }
       
        public string Name4 { get; set; }
        
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }

        public string City { get; set; }
        public string Region { get; set; }
        public string Postal { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Acquiredby { get; set; }
        public string AcquiredCompanyName { get; set; }
        public bool Enabled { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}