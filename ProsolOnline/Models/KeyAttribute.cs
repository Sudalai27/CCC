using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class KeyAttribute
    {
        //public string _id { get; set; }
        public string Characteristic { get; set; }
        public string Value { get; set; }
        public string UOM { get; set; }
        public int Squence { get; set; }
        public int ShortSquence { get; set; }
        public bool check { get; set; }


        /////////////
        //public ObjectId _id { get; set; }

        public string Noun { get; set; }
        public string Modifier { get; set; }
        //[Required]
        //[StringLength(50, MinimumLength = 2)]
        //[Display(Name = "Characteristic")]
      //  public string Characteristic { get; set; }
        public string Abbrivation { get; set; }

        // [Required]       
      //  public int Squence { get; set; }
       // public int ShortSquence { get; set; }
        public string Mandatory { get; set; }
        public string Definition { get; set; }
        public string[] Values { get; set; }
        public DateTime? UpdatedOn { get; set; }


    }
}