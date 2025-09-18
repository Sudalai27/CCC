using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class LogicsModel
    {

        public string _id { get; set; }
        [Required]
        public string Noun { get; set; }
        [Required]
        public string Modifier { get; set; }

        public string Generalterm { get; set; }
        public string Partno { get; set; }
        public string Refno { get; set; }     
        public List<ValuesList> Attributes { get; set; }
        public string Manufacturer { get; set; }
    }
}