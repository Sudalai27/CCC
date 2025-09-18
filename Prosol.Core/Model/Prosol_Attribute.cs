using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
namespace Prosol.Core.Model
{
    public class Prosol_Attribute
    {
        public ObjectId _id { get; set; }

        public string Attribute { get; set; }
        public string[] ValueList { get; set; }
        public string[] UOMList { get; set; }
        public int Validation { get; set; }
    }
}
