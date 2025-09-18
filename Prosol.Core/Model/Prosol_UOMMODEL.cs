using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
   public class Prosol_UOMMODEL
    {
        public ObjectId _id { get; set; }
        public string UOMNAME { get; set; }
        public string UOMDESC { get; set; }
        public DateTime? UpdatedOn { get; set; }

       
    }
}
