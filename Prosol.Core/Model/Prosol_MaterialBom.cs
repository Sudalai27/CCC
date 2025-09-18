using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
  public  class Prosol_MaterialBom
    {
        public ObjectId _id { get; set; }

        public string HeaderBID { get; set; }

        public string Noun { get; set; }

        public string Modifier { get; set; }       

        public string Manufacturer { get; set; }

        public string Partno { get; set; }

        public string Itemcode { get; set; }
        public string Shortdesc { get; set; }
        public string partqnt { get; set; }

        public string itemcat { get; set; }
        public string Longdesc { get; set; }
    }
}
