using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Prosol.Core.Model
{
    public class Prosol_Departments
    {
        public ObjectId _id { get; set; }
        public string Departmentname { get; set; }
        public bool Islive { get; set; }
    }
}