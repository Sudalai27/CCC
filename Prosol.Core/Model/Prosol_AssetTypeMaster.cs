using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
namespace Prosol.Core.Model
{
    public class Prosol_AssetTypeMaster
    {
        public ObjectId _id { get; set; }
        public string Label { get; set; }
        public string AssetType { get; set; }
        public string ClassificationHierarchyDesc { get; set; }
        public string FailureCode { get; set; }
        public bool Islive { get; set; }
    }
}