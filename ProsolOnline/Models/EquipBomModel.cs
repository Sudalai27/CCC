using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class EquipBomModel
    {
        public string _id { get; set; }

        public string Itemcode { get; set; }

        public string Shortdesc { get; set; }
        public string Longdesc { get; set; }
        public Int32 partqnt { get; set; }

        public string itemcat { get; set; }

        public string FunctLocation { get; set; }

        public string FunctDesc { get; set; }

        public string SupFunctLoc { get; set; }

        public string Objecttype { get; set; }

        public string TechIdentNo { get; set; }
        public string EquipDesc { get; set; }

        public string Manufacturer { get; set; }

        public string ManufCon { get; set; }

        public string Modelno { get; set; }
        public string ABCindic { get; set; }
        public string ManufSerialNo { get; set; }

      
       
    }
}