using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
namespace Prosol.Core.Model
{
   public class Prosol_SubGroupCodes
    {
        public ObjectId _id { get; set; }
        public string groupCode { get; set; }
        public string groupTitle { get; set; }
        public string code { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }
}
