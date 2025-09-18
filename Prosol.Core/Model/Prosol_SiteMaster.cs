using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
namespace Prosol.Core.Model
{
    public class Prosol_SiteMaster
    {
        public ObjectId _id { get; set; }
        public string Label { get; set; }
        public string SiteId { get; set; }
        public string Cluster { get; set; }
        public string HighLevelLocation { get; set; }
        public bool Islive { get; set; }
    }
}