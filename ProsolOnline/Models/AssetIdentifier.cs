using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AssetIdentifier
    {
        public string id { get; set; }
        public string fClass_Id { get; set; }
        public string IdentifierCode { get; set; }
        public string Identifier { get; set; }

        public string Business_Id { get; set; }
        public string Major_Id { get; set; }

        public bool IsActive { get; set; }
    }
}