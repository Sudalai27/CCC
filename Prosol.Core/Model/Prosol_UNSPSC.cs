using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_UNSPSC
    {

        public ObjectId _id { get; set; }

        public string Noun { get; set; }
        public string Modifier { get; set; }

        public string value { get; set; }
        public string Segment { get; set; }
        public string SegmentTitle { get; set; }
        public string Family { get; set; }
        public string FamilyTitle { get; set; }
        public string Class { get; set; }
        public string ClassTitle { get; set; }
        public string Commodity { get; set; }
        public string CommodityTitle { get; set; }
        public string Version { get; set; }
    }
}
