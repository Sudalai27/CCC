using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_equipbom
    {
        public ObjectId _id { get; set; }

     //   public string equipid { get; set; }

      //  public string funloc { get; set; }

        public string Itemcode { get; set; }

        public string Shortdesc { get; set; }


        public string Longdesc { get; set; }
        public string partqnt { get; set; }

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

        public string ManufSerialNo { get; set; }

        public string ConID { get; set; }
        public string ABCindic { get; set; }
        public string Materialtype { get; set; }

    }
}
