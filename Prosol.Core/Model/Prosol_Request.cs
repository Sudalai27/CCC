using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Prosol.Core.Model
{  
    
    
    public class Prosol_Request
    {
     

        public ObjectId _id { get; set; }
        public string requestId { get; set; }
        public string itemId { get; set; }
        public string source { get; set; }
        public string plant { get; set; }
        public string storage_Location { get; set; }
        public string group { get; set; }
        public string subGroup { get; set; }
        public string requester { get; set; }
        //[BsonElement("requestedOn")]
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? requestedOn { get; set; }
        public string approver { get; set; }
        public string itemStatus { get; set; }
        public string requestStatus { get; set; }
        public DateTime? approvedOn { get; set; }

      
        public DateTime? rejectedOn { get; set; }
       // [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public string cataloguer { get; set; }

        public string attachment { get; set; }
        public string reason_rejection { get; set; }
        public string Type { get; set; }
        public string Materialtype { get; set; }
        public string Industrysector { get; set; }
        public string MaterialStrategicGroup { get; set; }
        public string UnitPrice { get; set; }
    }
}
