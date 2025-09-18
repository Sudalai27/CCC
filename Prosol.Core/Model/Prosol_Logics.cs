using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_Logics
    {

        public ObjectId _id { get; set; }
       
        public string Noun { get; set; }
      
        public string Modifier { get; set; }

        public string Generalterm { get; set; }
        public string Partno { get; set; }
        public string Refno { get; set; }
   
        public List<ValueList> Attributes { get; set; }
        public string Manufacturer { get; set; }
    }
}
