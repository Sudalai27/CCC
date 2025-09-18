using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
 public class Prosol_RequestServiceRunning
    {
        public ObjectId _id { get; set; }
        public string Sn { get; set; }
        public int RunningNo { get; set; }
    }
}
