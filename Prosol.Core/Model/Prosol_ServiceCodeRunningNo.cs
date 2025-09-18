using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Prosol.Core.Model
{
    public class Prosol_ServiceCodeRunningNo
    {
        public ObjectId _id { get; set; }
        public string Maincode { get; set; }
        public string Subcode { get; set; }
        public string Subsubcode { get; set; }
        public int Runningno { get; set; }

    }
}
