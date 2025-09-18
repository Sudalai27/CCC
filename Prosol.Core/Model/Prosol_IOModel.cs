using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
   public class Prosol_IOModel
    {
        public ObjectId _id { get; set; }
        public string Materialcode { get; set; }
        public string FunctLocation { get; set; }

        public string TechIdentNo { get; set; }
        public DateTime? INtime { get; set; }
        public DateTime? OUTtime { get; set; }
        public DateTime? Createdon { get; set; }
    }
}
