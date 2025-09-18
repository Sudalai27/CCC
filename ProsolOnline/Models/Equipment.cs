using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class Equipment
    {

        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Modelno { get; set; }
        public string Tagno { get; set; }
        public string Serialno { get; set; }
        public int ENS { get; set; }
        public int EMS { get; set; }
        public string Additionalinfo { get; set; }
        public string SuperiorEquipment { get; set; }

       
    }
}