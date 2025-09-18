using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.WebApi
{
   public class ApiModel
    {

        //User
        public Int32 UserId { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string UserPswd { get; set; }
        public string Ustatus { get; set; }

        //Stock

        //public Int32 Id { get; set; }
        //public Int32 UId { get; set; }
        //public string Sapcode { get; set; }
        //public string Oldcode { get; set; }
        //public string Storagebin { get; set; }
        //public string Sysbal { get; set; }
        //public string Pysbal { get; set; }
        //public string MaterialDesc { get; set; }
        //public string Bun { get; set; }
        //public string StockRemark { get; set; }
        //public string Datagap { get; set; }
        //public string Additioninfo { get; set; }
        //public string CreatedOn { get; set; }
        //public string status { get; set; }
        //public string Bup { get; set; }



        public Int32 Id { set; get; }
        public Int16 UId { set; get; }
        public string Sapcode { set; get; }
        public string Storagebin { set; get; }
        public string Sysbal { set; get; }
        public string Pysbal { set; get; }
        public string MaterialDesc { set; get; }
        public string BinUpdation { set; get; }
        public string StockRemark { set; get; }
        public string DataCollection { set; get; }
        public string Additioninfo { set; get; }
        public string CreatedOn { set; get; }
        public string status { set; get; }
        public string Mode { set; get; }

        public DateTime? QtyasonDate { set; get; }

        public DateTime? GRDate { set; get; }
        public string PysicalObservation { set; get; }
        public string PysicalObservationQty { set; get; }
        public string ExpiredQty { set; get; }
        public DateTime? ExpiredDate { set; get; }
        public string SelfLife { set; get; }
        public string Model { set; get; }
        public string Make { set; get; }
        public string UOM { set; get; }

         public string Appimage1 { set; get; }
         public string Appimage2 { set; get; }
    }
}
