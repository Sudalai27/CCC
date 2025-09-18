using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class UOMModel
    {

        public string _id { get; set; }

        [Required]
        // [StringLength(50, MinimumLength = 2)]
        public string UOMname { get; set; }
        //  [Required]
        //[StringLength(10, MinimumLength = 2)]
        //[Display(Name = "Noun Abv")]
        [Required]
        public string Unitname { get; set; }

       // public string Attribute { get; set; }

        public DateTime? UpdatedOn { get; set; }

       
    }


}