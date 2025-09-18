using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Bulkrequest_load
    {
        public string plant { get; set; }
        public string storage_Location { get; set; }
        public string group { get; set; }
        public string subGroup { get; set; }
        public string source { get; set; }
        public string Materialtype { get; set; }
        public string Industrysector { get; set; }
        public string MaterialStrategicGroup { get; set; }
        public string UnitPrice { get; set; }


        public List<table_bulk> table_blk { get; set; }




    }

    public class table_bulk
    {
        public string Itemno { get; set; }
        public string Shortdesc { get; set; }
        public string Longdesc { get; set; }
    }
}
