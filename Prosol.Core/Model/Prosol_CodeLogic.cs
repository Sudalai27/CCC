using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
  public class Prosol_CodeLogic
    {
        public ObjectId _id { get; set; }
        public string CODELOGIC { get; set; }
        public string unspsc_Version { get; set; }
    }
}
