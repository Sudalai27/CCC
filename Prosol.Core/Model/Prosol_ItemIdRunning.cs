using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_ItemIdRunning
    {
        public ObjectId _id { get; set; }
        public string Group { get; set; }
        public string Subgroup { get; set; }
        public int RunningNo { get; set; }
    }
}
