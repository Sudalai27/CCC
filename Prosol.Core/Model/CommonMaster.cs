using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class CommonMaster
    {
        public List<Prosol_Business> Businesses { get; set; }
        public List<Prosol_MajorClass> MajorClasses { get; set; }
        public List<Prosol_MinorClass> MinorClasses { get; set; }

        public List<Prosol_Identifier> Identifiers { get; set; }
        public List<Prosol_Region> Regions { get; set; }
        public List<Prosol_City> Cities { get; set; }
        public List<Prosol_Area> Areas { get; set; }

        public List<Prosol_SubArea> SubAreas { get; set; }

        public List<Prosol_Location> Locations { get; set; }

        public List<Prosol_EquipmentClass> EquipmentClasses { get; set; }

        public List<Prosol_EquipmentType> EquipmentTypes { get; set; }
    }
}
