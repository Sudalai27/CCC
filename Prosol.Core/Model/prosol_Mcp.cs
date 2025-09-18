using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class prosol_Mcp
    {
        public ObjectId _id { get; set; }
        public string decipline { get; set; }
        public string Drivefunction { get; set; }
        public string equipment { get; set; }
        public string decipline_desc { get; set; }
        public string Drivefunction_desc { get; set; }
        public string equipment_desc { get; set; }
        public string file { get; set; }
        public string[] functionlocation { get; set; }
        public string mcpcode { get; set; }
    }
}
