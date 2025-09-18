using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;



namespace Prosol.Core.Model
{
    public class Prosol_Access
    {
        public ObjectId _id { get; set; }

        public  string Userid { get; set; }
        public string Pages { get; set; }

        public DateTime? Createdon { get; set; }

        public string Status { get; set; }
    }
} 