using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
   public class Prosol_NMAttributeRelationship
    {
        public ObjectId _id { get; set; }
        public string Noun { get; set; }
        public string Modifier { get; set; }
        public List<Prosol_KeyAttribute> Characteristics { get; set; }
        //public List<AttributeList> Characteristics { get; set; }
        // public string characteristic { get; set; }
      //  public string Value { get; set; }
       // public string Uom { get; set; }
        public string KeyAttribute { get; set; }
        public string KeyValue { get; set; }





    }
}
