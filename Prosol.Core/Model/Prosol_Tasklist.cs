using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Prosol.Core.Model
{
    public class Prosol_Tasklist
    {
        public ObjectId _id { get; set; }
        public string Equipment { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public List<Tasklist_operationSequence> TLOS { get; set; }
    }
}
