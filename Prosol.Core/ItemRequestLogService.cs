using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Web.Mvc;
using MongoDB.Bson.IO;

using MongoDB.Driver.Builders;
using MongoDB.Bson;
using System.Text.RegularExpressions;

using Prosol.Core.Interface;
using Prosol.Core.Model;
using MongoDB.Driver;

namespace Prosol.Core
{
    public class ItemRequestLogService : IItemRequestLog
    {
        private readonly IRepository<Prosol_Request> _ItemRequestLog_Request_Repository;
        private readonly IRepository<Prosol_Plants> _ItemRequestLog_Plant_Repository;
        private readonly IRepository<Prosol_Master> _ItemRequestLog_SLRepository;
        private readonly IRepository<Prosol_GroupCodes> _ItemRequestLog_GCRepository;
        private readonly IRepository<Prosol_SubGroupCodes> _ItemRequestLog_SGCRepository;
        private readonly IRepository<Prosol_Users> _ItemRequestLog_Users_Repository;
        private readonly IRepository<Prosol_Datamaster> _ItemRequestLog_Datamaster_Repository;

        public ItemRequestLogService(IRepository<Prosol_Request> ItemRequestLog_Request_Repository,
            IRepository<Prosol_Plants> ItemRequestLog_Plant_Repository,
            IRepository<Prosol_Master> ItemRequestLog_SLRepository,
            IRepository<Prosol_GroupCodes> ItemRequestLog_GCRepository,
            IRepository<Prosol_SubGroupCodes> ItemRequestLog_SGCRepository,
            IRepository<Prosol_Users> ItemRequestLog_Users_Repository,
            IRepository<Prosol_Datamaster> ItemRequestLog_Datamaster_Repository
            )
        {
            this._ItemRequestLog_Request_Repository = ItemRequestLog_Request_Repository;
            this._ItemRequestLog_Plant_Repository = ItemRequestLog_Plant_Repository;
            this._ItemRequestLog_SLRepository = ItemRequestLog_SLRepository;
            this._ItemRequestLog_GCRepository = ItemRequestLog_GCRepository;
            this._ItemRequestLog_SGCRepository = ItemRequestLog_SGCRepository;
            this._ItemRequestLog_Users_Repository = ItemRequestLog_Users_Repository;
            this._ItemRequestLog_Datamaster_Repository = ItemRequestLog_Datamaster_Repository;
        }



        public IEnumerable<Prosol_Request> get_itemsToApprove(string userid)
        {
            var sortt = SortBy.Descending("requestedOn");
            var query = Query.And(Query.EQ("requester", userid), Query.EQ("itemStatus", "pending"));
            var approver = _ItemRequestLog_Request_Repository.FindAll(query, sortt).ToList();
            return approver;
        }

        public IEnumerable<Prosol_Request> getApproved_Records(string userid)
        {
            var sortt = SortBy.Descending("approvedOn");
            var query = Query.And(Query.EQ("requester", userid), Query.EQ("itemStatus", "approved"));
            var approver = _ItemRequestLog_Request_Repository.FindAll(query, sortt).ToList();
            return approver;

        }

        public IEnumerable<Prosol_Request> getRejected_Records(string userid)
        {
            var sortt = SortBy.Descending("rejectedOn");
            var query = Query.And(Query.EQ("requester", userid), Query.EQ("itemStatus", "rejected"));
            var approver = _ItemRequestLog_Request_Repository.FindAll(query, sortt).ToList();
            return approver;
        }
        public IEnumerable<Prosol_Request> getClarification_Records(string userid)
        {
            var sortt = SortBy.Descending("rejectedOn");
            var query = Query.And(Query.EQ("requester", userid), Query.EQ("itemStatus", "clarification"));
            var approver = _ItemRequestLog_Request_Repository.FindAll(query, sortt).ToList();
            return approver;
        }

        public IEnumerable<Prosol_Request> getsingle_requested_record(string abcsony)
        {
            var query = Query.EQ("itemId", abcsony);
            var singlerow = _ItemRequestLog_Request_Repository.FindAll(query).ToList();

            Prosol_Request pr = new Prosol_Request();

            var query1 = Query.EQ("Userid", singlerow[0].approver);
            var requester1 = _ItemRequestLog_Users_Repository.FindAll(query1).ToList();
            pr.requester = requester1[0].UserName;

          //  string str = singlerow[0].requestedOn.ToLongDateString();
            string str = DateTime.Parse(singlerow[0].requestedOn.ToString()).ToString("dd/MM/yyyy");
            //  string str1 = str.Substring(0, 6) + " " + str.Substring(str.Length - 4);
            singlerow[0].approver = str;
           

            var query2 = Query.EQ("Plantcode", singlerow[0].plant);
            var plant2 = _ItemRequestLog_Plant_Repository.FindAll(query2).ToList();
            pr.plant = plant2[0].Plantname;

            //var query3 = Query.And(Query.EQ("Plantcode", singlerow[0].plant), Query.EQ("SL_Code", singlerow[0].storage_Location));
            //var sl2 = _ItemRequestLog_SLRepository.FindAll(query3).ToList();
            //pr.storage_Location = sl2[0].SL_Title;

            var query3 = Query.And(Query.EQ("Plantcode", singlerow[0].plant), Query.EQ("Code", singlerow[0].storage_Location));
            var sl2 = _ItemRequestLog_SLRepository.FindAll(query3).ToList();
            pr.storage_Location = sl2[0].Title;



            //var query4 = Query.EQ("code", singlerow[0].group);
            //var group2 = _ItemRequestLog_GCRepository.FindAll(query4).ToList();
            //pr.group = group2[0].title;

            //var query5 = Query.And(Query.EQ("groupCode", singlerow[0].group), Query.EQ("code", singlerow[0].subGroup));
            //var sg2 = _ItemRequestLog_SGCRepository.FindAll(query5).ToList();
            //if(sg2.Count>0)
            //pr.subGroup = sg2[0].title;


            if (singlerow[0].group != null)
            {
                var query4 = Query.EQ("code", singlerow[0].group);
                var group2 = _ItemRequestLog_GCRepository.FindAll(query4).ToList();
                 if (group2.Count > 0)
                pr.group = group2[0].title;
             
              //  pr.group = group2[0].title;
            }
            if (singlerow[0].subGroup != null)
            {
                var query5 = Query.And(Query.EQ("groupCode", singlerow[0].group), Query.EQ("code", singlerow[0].subGroup));
                var sg2 = _ItemRequestLog_SGCRepository.FindAll(query5).ToList();
                if (sg2.Count > 0)
                    pr.subGroup = sg2[0].title;
            }
            if (singlerow[0].Materialtype != null)
            {
                var query80 = Query.EQ("Code", singlerow[0].Materialtype);
                var mt = _ItemRequestLog_SLRepository.FindAll(query80).ToList();
                if (mt.Count > 0)
                    pr.Materialtype = mt[0].Title;
            }
            if (singlerow[0].Industrysector != null)
            {
                var query80 = Query.EQ("Code", singlerow[0].Industrysector);
                var mt = _ItemRequestLog_SLRepository.FindAll(query80).ToList();
                if (mt.Count > 0)
                    pr.Industrysector = mt[0].Title;
            }
            if (singlerow[0].MaterialStrategicGroup != null)
            {
                var query80 = Query.EQ("Code", singlerow[0].MaterialStrategicGroup);
                var mt = _ItemRequestLog_SLRepository.FindAll(query80).ToList();
                if (mt.Count > 0)
                    pr.MaterialStrategicGroup = mt[0].Title;
            }
            if (singlerow[0].UnitPrice != null)
            {
                var query80 = Query.EQ("Code", singlerow[0].UnitPrice);
                var mt = _ItemRequestLog_SLRepository.FindAll(query80).ToList();
                if (mt.Count > 0)
                    pr.UnitPrice = mt[0].Title;
            }


            if (singlerow[0].cataloguer != null)
            {
                var query6 = Query.EQ("Userid", singlerow[0].cataloguer);
                var cataloguer1 = _ItemRequestLog_Users_Repository.FindAll(query6).ToList();
                pr.cataloguer = cataloguer1[0].UserName;
            }

            singlerow.Add(pr);

            return singlerow;
        }



    }
}
