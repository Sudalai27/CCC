using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_AssetAttributes
    {
        public ObjectId _id { get; set; }
        public string UniqueId { get; set; }
        public string Noun { get; set; }
        public string exNoun { get; set; }
        public string Modifier { get; set; }
        public string exModifier { get; set; }
        public List<Asset_AttributeList> Characterisitics { get; set; }
        public List<Asset_AttributeList> exCharacterisitics { get; set; }
      
       

    }
    public class Asset_AttributeList
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
