using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;


using System.ComponentModel.DataAnnotations;


namespace ProsolOnline.Models
{
    public class UserCreateModel
    {
        public string _id { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string[] Plantcode { get; set; }
        //  public string[] Usertype { get; set; }
        public string[] Modules { get; set; }
        public string Islive { get; set; }
        public DateTime? Createdon { get; set; }
        [Required]
        public string Userid { get; set; }
        public string Departmentname { get; set; }
        public string ImageId { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public string FileData { get; set; }
        public DateTime? Lastlogin { get; set; }
        public List<TargetExtn> Roles { get; set; }


    }
    public class TargetExtn
    {
        public string Name { get; set; }       
        public string TargetId { get; set; }

    }
}