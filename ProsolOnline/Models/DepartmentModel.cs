using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace ProsolOnline.Models
{
    public class DepartmentModel
    {
        public string _id { get; set; }
        public string Departmentname { get; set; }
        public bool Islive { get; set; }
    }
}