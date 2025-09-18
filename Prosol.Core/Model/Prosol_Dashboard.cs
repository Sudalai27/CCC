using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Prosol.Core.Model
{
    public class Prosol_Dashboard
    {
        public ObjectId _id { get; set; }
        public string PlantName { get; set; }
        public string PlantCode { get; set; }

        public int TotCreated { get; set; }
        public int TotRelease { get; set; }
        public int TotReview { get; set; }
        public int TotCatalogue { get; set; }
        public int TotRework { get; set; }

        public int TotRequest { get; set; }
        public int TotApprove { get; set; }
        public int TotReject { get; set; }

        public int TotDuplicate { get; set; }
        public int TotVendors { get; set; }
        public int TotNounModifiers { get; set; }
        public int TotUsers { get; set; }

        public List<Prosol_Datamaster> DataList { get; set; }
        public List<Prosol_Vendor> VendorDataList { get; set; }
        public List<Prosol_NounModifiers> NMDataList { get; set; }
        public List<Prosol_showDetail> showdetail { get; set; }
        public string Cluster { get; set; }
        public string Location { get; set; }
        public string ReportGroup { get; set; }
        public int AssetCount { get; set; }
        public int AssetCompleted { get; set; }
        public int NewAsset { get; set; }
        public int Furniture { get; set; }
        public int AssetPending { get; set; }
        public string Remarks { get; set; }
    }
    public class Prosol_showDetail
    {
      
        public string Username { get; set; }
        public int Pending { get; set; }
        public int Completed { get; set; }
        public int Rework { get; set; }


      
    }
}
