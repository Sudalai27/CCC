using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class assetCommon
    {
        public List<AssetBusiness> Businesses { get; set; }
       public List<AssetMajorClass> MajorClasses { get; set; }
       public List<AssetMinorClass> MinorClasses { get; set; }

        public List<AssetIdentifier> Identifiers { get; set; }
        public List<AssetRegion> Regions { get; set; }
        public List<AssetCity> Cities { get; set; }
        public List<AssetArea> Areas { get; set; }

        public List<AssetsubArea> SubAreas { get; set; }

        public List<AssetLocation> Locations { get; set; }

        public List<AssetEquipClass> EquipmentClasses { get; set; }

        public List<AssetEquipType> EquipmentTypes { get; set; }
    }
}