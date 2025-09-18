using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
namespace Prosol.Core.Model
{
  public class Prosol_ServiceValuation
    {
        public ObjectId _id { get; set; }
        public string ServiceValuationcode { get; set; }
        public string ServiceValuationname { get; set; }
        public bool Islive { get; set; }
    }
}
