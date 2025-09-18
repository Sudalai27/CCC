using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_UOM
    {

        public ObjectId _id { get; set; }    
      
        public string UOMname { get; set; }       
        public string Unitname { get; set; }
       // public string Attribute { get; set; }
        public DateTime? UpdatedOn { get; set; }


    }
}
