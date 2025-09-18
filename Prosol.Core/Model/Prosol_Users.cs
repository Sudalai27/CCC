using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;





namespace Prosol.Core.Model
{
    public class Prosol_Users
    {
        public ObjectId _id { get; set; }
        //[Required] 
        //[StringLength(10, MinimumLength = 2)]
        //[Display(Name = "Noun Abv")]

        //[Required(ErrorMessage = "Enter Name")]
        public string FirstName { get; set; }

        public string LastName { get; set; }
        //[Required(ErrorMessage = "The email address is required")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailId { get; set; }

        //[Required(ErrorMessage = "Phone Number is required")]
        //[StringLength(10, MinimumLength = 2)]
        public string Mobile { get; set; }

        //[Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "Password required")]
        //[StringLength(10, MinimumLength = 6)]
        public string Password { get; set; }

       

        //[Required(ErrorMessage = "Select Plant")]
        public string[] Plantcode { get; set; }

        //[Required(ErrorMessage = "Select Department")]
        //public string Department { get; set; }

        //[Required(ErrorMessage = "Select User role")]
      //  public string[] Usertype { get; set; }

        //[Required(ErrorMessage = "Select Status")]
        public string Islive { get; set; }
        public DateTime? Createdon { get; set; }        

        //[StringLength(50, MinimumLength = 1)]
        public string Userid { get; set; }

        public string Departmentname { get; set; }
        public string ImageId { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public string FileData { get; set; }
        public DateTime? Lastlogin { get; set; }
        public List<TargetExn> Roles { get; set; }
        public string[] Modules { get; set; }
    }
    public class TargetExn
    {
        public string Name { get; set; }
        public string TargetId { get; set; }

    }
}