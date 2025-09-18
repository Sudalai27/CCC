using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class SequenceModel
    {

        public string _id { get; set; }
        [Required]
        public int CatId { get; set; }

        [Required]
      
        public string Category { get; set; }
              
        [Required]
        public string Description { get; set; }
        [Required]
        public string Seq { get; set; }
        [Required]
        public string Required { get; set; }
        [Required]
        public string Separator { get; set; }
        public string ShortLength { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}