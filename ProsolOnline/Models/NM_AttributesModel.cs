using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace ProsolOnline.Models
{
    public class NM_AttributesModel
    {
        public string _id { get; set; }


        // [StringLength(50, MinimumLength = 2)]
   
        public string Noun { get; set; }
      
        public string Modifier { get; set; }
        //[Required]
        //[StringLength(50, MinimumLength = 2)]
        //[Display(Name = "Characteristic")]
        [Required]
        public string Characteristic { get; set; }
        public string Abbrivation { get; set; }

        // [Required]       
        public int Squence { get; set; }
        public int ShortSquence { get; set; }
        public string Mandatory { get; set; }
        public string Definition { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string[] Values { get; set; }
        public string[] Uom { get; set; }
        public string UomMandatory { get; set; }
        public string Validation { get; set; }
        public int Remove { get; set; }

        public string Source { get; set; }

        public string SourceUrl { get; set; }
        public string ClassificationId { get; set; }
        public string HierarchyPath { get; set; }
        public string PDesc { get; set; }
        public string ClassLevel { get; set; }
    }
}