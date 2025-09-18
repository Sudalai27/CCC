using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class prosol_FARdashboard
    {

        public ObjectId _id { get; set; }     
        public string Business { get; set; }
        public string Category { get; set; }
        public string Assets { get; set; }
        public List<HierarchyCol> HierarchyCols { get; set; }
        public string Estimated { get; set; }
        public string plannedComplete { get; set; }
        public string ActualComplete { get; set; }
        public string PlannedPerc { get; set; }
        public string ActualPerc { get; set; }      
        public DateTime UpdatedOn { get; set; }
        public List<Logs> History { get; set; }
    }
    public class HierarchyCol
    {
        public string colName { get; set; }
        public string Value { get; set; }
    }
    public class Logs
    {
        public string Estimated { get; set; }
        public string plannedComplete { get; set; }
        public string ActualComplete { get; set; }
        public string PlannedPerc { get; set; }
        public string ActualPerc { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}