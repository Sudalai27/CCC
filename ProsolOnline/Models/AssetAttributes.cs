using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AssetAttributes
    {

        public string UniqueId { get; set; }
        public string Noun { get; set; }
        public string Modifier { get; set; }
        public string Characteristic { get; set; }
        public string exNoun { get; set; }
        public string exModifier { get; set; }
        public string Value { get; set; }
        public string UOM { get; set; }
        public int Squence { get; set; }
        public int ShortSquence { get; set; }
        public string Source { get; set; }
        public string SourceUrl { get; set; }
        public bool Approve { get; set; }
        public string Abbrevated { get; set; }

        public string[] Uom { get; set; }
        public string UomMandatory { get; set; }
        public List<CatAsset_AttributeList> Characterisitics { get; set; }
        public List<CatAsset_AttributeList> exCharacterisitics { get; set; }


    }
    public class CatAsset_AttributeList
    {
        public string Characteristic { get; set; }
        public string Value { get; set; }
        public string UOM { get; set; }
        public int Squence { get; set; }
        public int ShortSquence { get; set; }
        public string Source { get; set; }
        public string SourceUrl { get; set; }
        public bool Approve { get; set; }
        public string Abbrevated { get; set; }

        public string[] Uom { get; set; }
        public string UomMandatory { get; set; }


    }
}