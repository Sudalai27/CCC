using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
  public class Prosol_SubSubGroupCode
    {
        public ObjectId _id { get; set; }
        public string MainGroupCode { get; set; }
        public string MainGroupTitle { get; set; }
        public string SubGroupCode { get; set; }
        public string SubGroupTitle { get; set; }
        public string SubSubGroupCode { get; set; }
        public string SubSubGroupTitle { get; set; }
    }
}
