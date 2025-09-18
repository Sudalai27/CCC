using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_Charateristics
    {

        public ObjectId _id { get; set; }      
       
        public string Noun { get; set; }
        public string Modifier { get; set; }
        //[Required]
        //[StringLength(50, MinimumLength = 2)]
        //[Display(Name = "Characteristic")]
        public string Characteristic { get; set; }
        public string Abbrivation { get; set; }

        // [Required]       
        public int Squence { get; set; }
        public int ShortSquence { get; set; }
        public string Mandatory { get; set; }
        public string Definition { get; set; }
        public string[] Values { get; set; }
        public string[] Uom { get; set; }
        public string UomMandatory { get; set; }
        public string ClassificationId { get; set; }
        public string HierarchyPath { get; set; }
        public string PDesc { get; set; }
        public string ClassLevel { get; set; }
        
        public DateTime? UpdatedOn { get; set; }
    }
}
