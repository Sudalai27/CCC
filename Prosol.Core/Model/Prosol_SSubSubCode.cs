using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
   public class Prosol_SSubSubCode
    {
        public ObjectId _id { get; set; }
        public string MainCode { get; set; }
        public string MainDiscription { get; set; }
        public string SubCode { get; set; }
        public string SubDiscription { get; set; }
        public string SubSubCode { get; set; }
        public string SubSubDiscription { get; set; }
        public bool Islive { get; set; }
    }
}
