using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
   public class Prosol_Idealtime
    {
        public ObjectId _id { get; set; }
        public string userid { get; set; }
        public DateTime? Logintime { get; set; }
        public DateTime? Logouttime { get; set; }
    }
}
