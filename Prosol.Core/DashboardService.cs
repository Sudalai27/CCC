using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Prosol.Core
{
    public class DashboardService : IDashboard
    {
        private readonly IRepository<Prosol_Vendor> _VendorRepository;
        private readonly IRepository<Prosol_Datamaster> _DatamasterRepository;
        private readonly IRepository<Prosol_NounModifiers> _nounModifierRepository;
        private readonly IRepository<Prosol_Users> _usersRepository;
        private readonly IRepository<Prosol_ERPInfo> _erpRepository;
        private readonly IRepository<Prosol_Plants> _PlanttRepository;
        private readonly IRepository<Prosol_Request> _ItemRequestRepository;

        public DashboardService(IRepository<Prosol_Vendor> vendorRepository,
            IRepository<Prosol_Datamaster> datamasterRepository,
            IRepository<Prosol_NounModifiers> nounModifierRepository,
            IRepository<Prosol_Users> usersRepository,
            IRepository<Prosol_ERPInfo> erpRepository,
            IRepository<Prosol_Plants> PlantRepository,
            IRepository<Prosol_Request> itemRequestRepository)
        {

            this._VendorRepository = vendorRepository;
            this._DatamasterRepository = datamasterRepository;
            this._nounModifierRepository = nounModifierRepository;
            this._usersRepository = usersRepository;
            this._erpRepository = erpRepository;
            this._PlanttRepository = PlantRepository;
            this._ItemRequestRepository = itemRequestRepository;
        }
        public List<Prosol_Dashboard> BindTotalItem(string[] plants)
        {
            List<Prosol_Dashboard> LstDash = new List<Prosol_Dashboard>();

            if (plants == null)
            {
                var PltQry = Query.EQ("Islive", true);
                var plantLst = _PlanttRepository.FindAll(PltQry).ToList();
                plants = new string[plantLst.Count];
                int i = 0;
                foreach (Prosol_Plants mdl in plantLst)
                {
                    plants[i] = mdl.Plantcode;
                    i++;
                }

            }
            foreach (string pnt in plants)
            {
                Prosol_Dashboard mdl = new Prosol_Dashboard();
                string[] flds = { "Itemcode", "Plant", "Plant_" };
                var fields = Fields.Include(flds).Exclude("_id");
                var query = Query.EQ("Plant", pnt);
                var plantList = _erpRepository.FindAll(fields, query).ToList();


                string[] flds1 = { "Itemcode", "UpdatedOn" };
                var fields1 = Fields.Include(flds1).Exclude("_id");

                //total release
                // var query1 = Query.And(Query.Or(Query.EQ("Duplicates", ""), Query.EQ("Duplicates",BsonNull.Value)), Query.EQ("ItemStatus", 6));
                var query1 = Query.EQ("ItemStatus", 6);

                var dataList = _DatamasterRepository.FindAll(fields1, query1);

                var TotCreated = (from data in dataList join plant in plantList on data.Itemcode equals plant.Itemcode select data).ToList();
                if (TotCreated.Count > 0)
                {
                    mdl.DataList = TotCreated;
                    mdl.TotCreated = TotCreated.Count;
                }
                //total duplicate
                var query2 = Query.And(Query.And(Query.NE("Duplicates", ""), Query.NE("Duplicates", BsonNull.Value)), Query.EQ("ItemStatus", 6));

                var dataList1 = _DatamasterRepository.FindAll(fields1, query2);

                var totduplicate = (from data in dataList1 join plant in plantList on data.Itemcode equals plant.Itemcode select data).ToList();
                if (totduplicate.Count > 0)
                {
                    mdl.TotDuplicate = totduplicate.Count;
                }
                mdl.PlantCode = pnt;
                var PltQry = Query.EQ("Plantcode", pnt);
                var plantName = _PlanttRepository.FindOne(PltQry);
                mdl.PlantName = plantName.Plantname;

                //total catalogue
                var query3 = Query.And(Query.GTE("ItemStatus", 0), Query.LT("ItemStatus", 2));
                var dataList2 = _DatamasterRepository.FindAll(fields1, query3);
                var totCatalogue = (from data in dataList2 join plant in plantList on data.Itemcode equals plant.Itemcode select data).ToList();
                if (totCatalogue.Count > 0)
                {
                    mdl.TotCatalogue = totCatalogue.Count;
                }

                //total Review
                var query4 = Query.And(Query.GTE("ItemStatus", 2), Query.LT("ItemStatus", 4));
                var dataList3 = _DatamasterRepository.FindAll(fields1, query4);
                var totReview = (from data in dataList3 join plant in plantList on data.Itemcode equals plant.Itemcode select data).ToList();
                if (totReview.Count > 0)
                {
                    mdl.TotReview = totReview.Count;
                }

                //total release
                var query5 = Query.And(Query.GTE("ItemStatus", 4), Query.LT("ItemStatus", 6));
                var dataList4 = _DatamasterRepository.FindAll(fields1, query5);
                var TotRelease = (from data in dataList4 join plant in plantList on data.Itemcode equals plant.Itemcode select data).ToList();
                if (TotRelease.Count > 0)
                {
                    mdl.TotRelease = TotRelease.Count;
                }


                ////total R code
                //var Rcode = Query.EQ("Maincode", "R");

                //var RcodeList = _DatamasterRepository.FindAll(fields1, Rcode);
                //var TotRequest = (from data in RcodeList join plant in plantList on data.Itemcode equals plant.Itemcode select data).ToList();
                //if (TotRequest.Count > 0)
                //{
                //    mdl.TotRequest = TotRequest.Count;
                //}
                ////total D code
                //var Dcode = Query.EQ("Maincode", "D");

                //var DcodeList = _DatamasterRepository.FindAll(fields1, Dcode);
                //var TotApprove = (from data in DcodeList join plant in plantList on data.Itemcode equals plant.Itemcode select data).ToList();
                //if (TotApprove.Count > 0)
                //{
                //    mdl.TotApprove = TotApprove.Count;
                //}
                ////total Deactivated codes

                //var query8 = Query.EQ("ItemStatus", -1);

                //var dataList7 = _DatamasterRepository.FindAll(fields1, query8);
                //var TotReject = (from data in dataList7 join plant in plantList on data.Itemcode equals plant.Itemcode select data).ToList();
                //if (TotReject.Count > 0)
                //{
                //    mdl.TotReject = TotReject.Count;
                //}



                LstDash.Add(mdl);
            }
            //total vendor
            string[] fld = { "Code", "UpdatedOn" };
            var feids = Fields.Include(fld).Exclude("_id");
            var query9 = Query.EQ("Enabled", true);
            var dataList8 = _VendorRepository.FindAll(feids, query9).ToList();
            if (dataList8.Count > 0)
            {
                LstDash[0].TotVendors = dataList8.Count;
                LstDash[0].VendorDataList = dataList8;
            }

            //total noun modifier
            string[] ffdld = { "Noun", "Modifier", "UpdatedOn" };
            var fedids = Fields.Include(ffdld).Exclude("_id");
            var dataList9 = _nounModifierRepository.FindAll(fedids).ToList();
            if (dataList9.Count > 0)
            {
                LstDash[0].TotNounModifiers = dataList9.Count;
                LstDash[0].NMDataList = dataList9;
            }
            return LstDash;
        }
        public List<Prosol_Dashboard> BindReleaser(string[] plants, List<TargetExn> UserRole, string userId)
        {
            List<Prosol_Dashboard> LstDash = new List<Prosol_Dashboard>();
            int flg = 0;
            if (plants == null)
            {
                var PltQry = Query.EQ("Islive", true);
                var plantLst = _PlanttRepository.FindAll(PltQry).ToList();
                plants = new string[plantLst.Count];
                int i = 0;
                foreach (Prosol_Plants mdl in plantLst)
                {
                    plants[i] = mdl.Plantcode;
                    i++;
                }
                flg = 1;
            }
            foreach (TargetExn dl in UserRole)
            {
                if (dl.Name == "Releaser")
                {
                    flg = 1;

                }

            }
            if (flg == 1)
            {
                foreach (string pnt in plants)
                {
                    Prosol_Dashboard mdl = new Prosol_Dashboard();
                    string[] flds = { "Itemcode", "Plant", "Plant_" };
                    var fields = Fields.Include(flds).Exclude("_id");
                    var query = Query.EQ("Plant", pnt);
                    var plantList = _erpRepository.FindAll(fields, query).ToList();

                    mdl.PlantCode = pnt;
                    var PltQry = Query.EQ("Plantcode", pnt);
                    var plntName = _PlanttRepository.FindOne(PltQry);
                    mdl.PlantName = plntName.Plantname;


                    var usrLst = new List<Prosol_Users>();
                    if (UserRole[0].Name == "SuperAdmin")
                    {
                        var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Releaser"));
                        usrLst = _usersRepository.FindAll(UsrQry).ToList();
                    }
                    else
                    {
                        var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Releaser"), Query.EQ("Userid", userId));
                        usrLst = _usersRepository.FindAll(UsrQry).ToList();

                    }

                    // var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Releaser"));
                    // var usrLst = _usersRepository.FindAll(UsrQry).ToList();


                    string[] flds1 = { "Itemcode", "UpdatedOn", "Release", "Review", "Catalogue", "ItemStatus", "Rework" };
                    var fields1 = Fields.Include(flds1).Exclude("_id");

                    //  var query1 = Query.Or(Query.EQ("Duplicates", ""), Query.EQ("Duplicates", BsonNull.Value));
                    var dataList = _DatamasterRepository.FindAll(fields1).ToList();
                    List<Prosol_showDetail> resLst = new List<Prosol_showDetail>();

                    foreach (Prosol_Users usr in usrLst)
                    {
                        Prosol_showDetail shwmdl = new Prosol_showDetail();
                        shwmdl.Username = usr.UserName;

                        var TotCreated = (from data in dataList join plant in plantList on data.Itemcode equals plant.Itemcode where data.Release != null && data.Release.UserId == usr.Userid select data).ToList();
                        if (TotCreated.Count > 0)
                        {
                            var p = (from i in TotCreated where i.ItemStatus >= 4 && i.ItemStatus < 6 select i).ToList();
                            shwmdl.Pending = p.Count;

                            var c = (from i in TotCreated where i.ItemStatus == 6 select i).ToList();
                            shwmdl.Completed = c.Count;

                            // var r = (from i in TotCreated where i.ItemStatus >= 4 && i.ItemStatus < 6 && i.Rework != null select i).ToList();
                            shwmdl.Rework = 0;

                            resLst.Add(shwmdl);

                            mdl.showdetail = resLst;


                        }

                    }

                    LstDash.Add(mdl);
                }
            }
            return LstDash;

        }
        public List<Prosol_Dashboard> BindReview(string[] plants, List<TargetExn> UserRole, string userId)
        {
            List<Prosol_Dashboard> LstDash = new List<Prosol_Dashboard>();
            int flg = 0;
            if (plants == null)
            {
                var PltQry = Query.EQ("Islive", true);
                var plantLst = _PlanttRepository.FindAll(PltQry).ToList();
                plants = new string[plantLst.Count];
                int i = 0;
                foreach (Prosol_Plants mdl in plantLst)
                {
                    plants[i] = mdl.Plantcode;
                    i++;
                }
                flg = 1;
            }
            foreach (TargetExn dl in UserRole)
            {
                if (dl.Name == "Reviewer")
                {
                    flg = 1;

                }

            }
            if (flg == 1)
            {
                foreach (string pnt in plants)
                {
                    Prosol_Dashboard mdl = new Prosol_Dashboard();
                    string[] flds = { "Itemcode", "Plant", "Plant_" };
                    var fields = Fields.Include(flds).Exclude("_id");
                    var query = Query.EQ("Plant", pnt);
                    var plantList = _erpRepository.FindAll(fields, query).ToList();

                    mdl.PlantCode = pnt;
                    var PltQry = Query.EQ("Plantcode", pnt);
                    var plntName = _PlanttRepository.FindOne(PltQry);
                    mdl.PlantName = plntName.Plantname;


                    var usrLst = new List<Prosol_Users>();
                    if (UserRole[0].Name == "SuperAdmin")
                    {
                        var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Reviewer"));
                        usrLst = _usersRepository.FindAll(UsrQry).ToList();
                    }
                    else
                    {
                        var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Reviewer"), Query.EQ("Userid", userId));
                        usrLst = _usersRepository.FindAll(UsrQry).ToList();

                    }

                    // var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Reviewer"));
                    //var usrLst = _usersRepository.FindAll(UsrQry).ToList();


                    string[] flds1 = { "Itemcode", "UpdatedOn", "Release", "Review", "Catalogue", "ItemStatus", "Rework" };
                    var fields1 = Fields.Include(flds1).Exclude("_id");

                    //var query1 = Query.EQ("Duplicates", "");
                    var dataList = _DatamasterRepository.FindAll(fields1).ToList();
                    List<Prosol_showDetail> resLst = new List<Prosol_showDetail>();

                    foreach (Prosol_Users usr in usrLst)
                    {
                        Prosol_showDetail shwmdl = new Prosol_showDetail();
                        shwmdl.Username = usr.UserName;

                        var TotCreated = (from data in dataList join plant in plantList on data.Itemcode equals plant.Itemcode where data.Review != null && data.Review.UserId == usr.Userid select data).ToList();
                        if (TotCreated.Count > 0)
                        {
                            var p = (from i in TotCreated where i.ItemStatus >= 2 && i.ItemStatus < 4 && i.Rework == null select i).ToList();
                            shwmdl.Pending = p.Count;

                            var c = (from i in TotCreated where i.ItemStatus >= 4 select i).ToList();
                            shwmdl.Completed = c.Count;

                            var r = (from i in TotCreated where i.ItemStatus >= 2 && i.ItemStatus < 4 && i.Rework != null select i).ToList();
                            shwmdl.Rework = r.Count;

                            resLst.Add(shwmdl);

                            mdl.showdetail = resLst;


                        }

                    }

                    LstDash.Add(mdl);
                }
            }
            return LstDash;

        }
        public List<Prosol_Dashboard> BindCatalogue(string[] plants, List<TargetExn> UserRole, string userId)
        {
            List<Prosol_Dashboard> LstDash = new List<Prosol_Dashboard>();
            int flg = 0;
            if (plants == null && UserRole[0].Name == "SuperAdmin")
            {
                var PltQry = Query.EQ("Islive", true);
                var plantLst = _PlanttRepository.FindAll(PltQry).ToList();
                plants = new string[plantLst.Count];
                int i = 0;
                foreach (Prosol_Plants mdl in plantLst)
                {
                    plants[i] = mdl.Plantcode;
                    i++;
                }
                flg = 1;
            }
            foreach (TargetExn dl in UserRole)
            {
                if (dl.Name == "Cataloguer")
                {
                    flg = 1;

                }

            }
            if (flg == 1)
            {
                foreach (string pnt in plants)
                {
                    Prosol_Dashboard mdl = new Prosol_Dashboard();
                    string[] flds = { "Itemcode", "Plant", "Plant_" };
                    var fields = Fields.Include(flds).Exclude("_id");
                    var query = Query.EQ("Plant", pnt);
                    var plantList = _erpRepository.FindAll(fields, query).ToList();

                    mdl.PlantCode = pnt;
                    var PltQry = Query.EQ("Plantcode", pnt);
                    var plntName = _PlanttRepository.FindOne(PltQry);
                    mdl.PlantName = plntName.Plantname;

                    var usrLst = new List<Prosol_Users>();
                    if (UserRole[0].Name == "SuperAdmin")
                    {
                        var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Cataloguer"));
                        usrLst = _usersRepository.FindAll(UsrQry).ToList();
                    }
                    else
                    {
                        var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Cataloguer"), Query.EQ("Userid", userId));
                        usrLst = _usersRepository.FindAll(UsrQry).ToList();

                    }




                    string[] flds1 = { "Itemcode", "UpdatedOn", "Release", "Review", "Catalogue", "ItemStatus", "Rework", "Reworkcat" };
                    var fields1 = Fields.Include(flds1).Exclude("_id");

                    // var query1 = Query.Or(Query.EQ("Duplicates", ""), Query.EQ("Duplicates", BsonNull.Value));
                    var dataList = _DatamasterRepository.FindAll(fields1).ToList();
                    List<Prosol_showDetail> resLst = new List<Prosol_showDetail>();

                    foreach (Prosol_Users usr in usrLst)
                    {
                        Prosol_showDetail shwmdl = new Prosol_showDetail();
                        shwmdl.Username = usr.UserName;

                        var TotCreated = (from data in dataList join plant in plantList on data.Itemcode equals plant.Itemcode where data.Catalogue != null && data.Catalogue.UserId == usr.Userid select data).ToList();
                        if (TotCreated.Count > 0)
                        {
                            var p = (from i in TotCreated where i.ItemStatus >= 0 && i.ItemStatus < 2 && i.Reworkcat == null select i).ToList();
                            shwmdl.Pending = p.Count;

                            var c = (from i in TotCreated where i.ItemStatus >= 2 select i).ToList();
                            shwmdl.Completed = c.Count;

                            var r = (from i in TotCreated where i.ItemStatus >= 0 && i.ItemStatus < 2 && i.Reworkcat != null select i).ToList();
                            shwmdl.Rework = r.Count;

                            resLst.Add(shwmdl);

                            mdl.showdetail = resLst;


                        }

                    }

                    LstDash.Add(mdl);
                }
            }
            return LstDash;

        }
        //public List<Prosol_Dashboard> BindApprove(string[] plants, List<TargetExn> UserRole, string userId)
        //{

        //    List<Prosol_Dashboard> LstDash = new List<Prosol_Dashboard>();
        //    int flg = 0;
        //    if (plants == null && UserRole[0].Name == "SuperAdmin")
        //    {
        //        var PltQry = Query.EQ("Islive", true);
        //        var plantLst = _PlanttRepository.FindAll(PltQry).ToList();
        //        plants = new string[plantLst.Count];
        //        int i = 0;
        //        foreach (Prosol_Plants mdl in plantLst)
        //        {
        //            plants[i] = mdl.Plantcode;
        //            i++;
        //        }
        //        flg = 1;
        //    }
        //    foreach (TargetExn dl in UserRole)
        //    {
        //        if (dl.Name == "Approver")
        //        {
        //            flg = 1;

        //        }

        //    }
        //    if (flg == 1)
        //    {
        //        foreach (string pnt in plants)
        //        {
        //            Prosol_Dashboard mdl = new Prosol_Dashboard();
        //            string[] flds = { "Itemcode", "Plant", "Plant_" };
        //            var fields = Fields.Include(flds).Exclude("_id");
        //            var query = Query.EQ("Plant", pnt);
        //            var plantList = _erpRepository.FindAll(fields, query).ToList();

        //            mdl.PlantCode = pnt;
        //            var PltQry = Query.EQ("Plantcode", pnt);
        //            var plntName = _PlanttRepository.FindOne(PltQry);
        //            mdl.PlantName = plntName.Plantname;
        //            var usrLst = new List<Prosol_Users>();
        //            if (UserRole[0].Name == "SuperAdmin")
        //            {
        //                var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Approver"));
        //                usrLst = _usersRepository.FindAll(UsrQry).ToList();
        //            }
        //            else
        //            {
        //                var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Approver"), Query.EQ("Userid", userId));
        //                usrLst = _usersRepository.FindAll(UsrQry).ToList();

        //            }
        //            string[] flds1 = { "itemId", "plant", "requester", "approver", "itemStatus", "cataloguer" };
        //            var fields1 = Fields.Include(flds1).Exclude("_id");

        //            var query1 = Query.NE("itemStatus", "deleted");
        //            var dataList = _ItemRequestRepository.FindAll(fields1).ToList();
        //            List<Prosol_showDetail> resLst = new List<Prosol_showDetail>();

        //            foreach (Prosol_Users usr in usrLst)
        //            {
        //                Prosol_showDetail shwmdl = new Prosol_showDetail();
        //                shwmdl.Username = usr.UserName;

        //                var TotCreated = (from data in dataList where data.approver != null && data.approver == usr.Userid && data.plant == pnt select data).ToList();
        //                if (TotCreated.Count > 0)
        //                {
        //                    var p = (from i in TotCreated where i.itemStatus == "pending" select i).ToList();
        //                    shwmdl.Pending = p.Count;

        //                    var c = (from i in TotCreated where i.itemStatus == "approved" select i).ToList();
        //                    shwmdl.Completed = c.Count;

        //                    var r = (from i in TotCreated where i.itemStatus == "rejected" select i).ToList();
        //                    shwmdl.Rework = r.Count;

        //                    resLst.Add(shwmdl);

        //                    mdl.showdetail = resLst;


        //                }

        //            }

        //            LstDash.Add(mdl);
        //        }
        //    }
        //    return LstDash;

        //}
        //public List<Prosol_Dashboard> BindRequest(string[] plants, List<TargetExn> UserRole, string userId)
        //{
        //    List<Prosol_Dashboard> LstDash = new List<Prosol_Dashboard>();

        //    int flg = 0;
        //    if (plants == null && UserRole[0].Name == "SuperAdmin")
        //    {
        //        var PltQry = Query.EQ("Islive", true);
        //        var plantLst = _PlanttRepository.FindAll(PltQry).ToList();
        //        plants = new string[plantLst.Count];
        //        int i = 0;
        //        foreach (Prosol_Plants mdl in plantLst)
        //        {
        //            plants[i] = mdl.Plantcode;
        //            i++;
        //        }
        //        flg = 1;
        //    }
        //    foreach (TargetExn dl in UserRole)
        //    {
        //        if (dl.Name == "Requester")
        //            flg = 1;

        //    }
        //    if (flg == 1)
        //    {
        //        foreach (string pnt in plants)
        //        {
        //            Prosol_Dashboard mdl = new Prosol_Dashboard();
        //            string[] flds = { "Itemcode", "Plant", "Plant_" };
        //            var fields = Fields.Include(flds).Exclude("_id");
        //            var query = Query.EQ("Plant", pnt);
        //            var plantList = _erpRepository.FindAll(fields, query).ToList();

        //            mdl.PlantCode = pnt;
        //            var PltQry = Query.EQ("Plantcode", pnt);
        //            var plntName = _PlanttRepository.FindOne(PltQry);
        //            mdl.PlantName = plntName.Plantname;


        //            var usrLst = new List<Prosol_Users>();
        //            if (UserRole[0].Name == "SuperAdmin")
        //            {
        //                var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Requester"));
        //                usrLst = _usersRepository.FindAll(UsrQry).ToList();
        //            }
        //            else
        //            {
        //                var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Requester"), Query.EQ("Userid", userId));
        //                usrLst = _usersRepository.FindAll(UsrQry).ToList();

        //            }

        //            // var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Requester"));
        //            // var usrLst = _usersRepository.FindAll(UsrQry).ToList();


        //            string[] flds1 = { "itemId", "plant", "requester", "approver", "itemStatus", "cataloguer" };
        //            var fields1 = Fields.Include(flds1).Exclude("_id");

        //            var query1 = Query.NE("itemStatus", "deleted");
        //            var dataList = _ItemRequestRepository.FindAll(fields1).ToList();
        //            List<Prosol_showDetail> resLst = new List<Prosol_showDetail>();

        //            foreach (Prosol_Users usr in usrLst)
        //            {
        //                Prosol_showDetail shwmdl = new Prosol_showDetail();
        //                shwmdl.Username = usr.UserName;

        //                var TotCreated = (from data in dataList where data.requester != null && data.requester == usr.Userid && data.plant == pnt select data).ToList();
        //                if (TotCreated.Count > 0)
        //                {
        //                    var p = (from i in TotCreated where i.itemStatus == "pending" select i).ToList();
        //                    shwmdl.Pending = p.Count;

        //                    var c = (from i in TotCreated where i.itemStatus == "approved" select i).ToList();
        //                    shwmdl.Completed = c.Count;

        //                    var r = (from i in TotCreated where i.itemStatus == "rejected" select i).ToList();
        //                    shwmdl.Rework = r.Count;

        //                    resLst.Add(shwmdl);

        //                    mdl.showdetail = resLst;


        //                }

        //            }

        //            LstDash.Add(mdl);
        //        }
        //    }
        //    return LstDash;

        //}



        //target
        public List<Prosol_Dashboard> bindReviewTarget(string[] plants, string userId)
        {
            List<Prosol_Dashboard> LstDash = new List<Prosol_Dashboard>();
            foreach (string pnt in plants)
            {
                Prosol_Dashboard mdl = new Prosol_Dashboard();
                string[] flds = { "Itemcode", "Plant", "Plant_" };
                var fields = Fields.Include(flds).Exclude("_id");
                var query = Query.EQ("Plant", pnt);
                var plantList = _erpRepository.FindAll(fields, query).ToList();

                mdl.PlantCode = pnt;
                var PltQry = Query.EQ("Plantcode", pnt);
                var plntName = _PlanttRepository.FindOne(PltQry);
                mdl.PlantName = plntName.Plantname;

                //  var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Reviewer"), Query.EQ("Roles.TargetId", userId));
                //  var usrLst = _usersRepository.FindAll(UsrQry).ToList();

                var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Reviewer"));
                var usrLst = _usersRepository.FindAll(UsrQry).ToList();
                var proUserList = new List<Prosol_Users>();
                foreach (Prosol_Users pro in usrLst)
                {
                    foreach (TargetExn xtn in pro.Roles)
                    {
                        if (xtn.Name == "Reviewer" && xtn.TargetId == userId)
                        {
                            proUserList.Add(pro);
                        }
                    }
                }

                string[] flds1 = { "Itemcode", "UpdatedOn", "Release", "Review", "Catalogue", "ItemStatus", "Rework" };
                var fields1 = Fields.Include(flds1).Exclude("_id");

                // var query1 = Query.Or(Query.EQ("Duplicates", ""), Query.EQ("Duplicates", BsonNull.Value));
                var dataList = _DatamasterRepository.FindAll(fields1).ToList();
                List<Prosol_showDetail> resLst = new List<Prosol_showDetail>();

                foreach (Prosol_Users usr in proUserList)
                {
                    Prosol_showDetail shwmdl = new Prosol_showDetail();
                    shwmdl.Username = usr.UserName;

                    var TotCreated = (from data in dataList join plant in plantList on data.Itemcode equals plant.Itemcode where data.Review != null && data.Review.UserId == usr.Userid select data).ToList();
                    if (TotCreated.Count > 0)
                    {
                        var p = (from i in TotCreated where i.ItemStatus >= 2 && i.ItemStatus < 4 && i.Rework == null select i).ToList();
                        shwmdl.Pending = p.Count;

                        var c = (from i in TotCreated where i.ItemStatus >= 4 select i).ToList();
                        shwmdl.Completed = c.Count;

                        var r = (from i in TotCreated where i.ItemStatus >= 2 && i.ItemStatus < 4 && i.Rework != null select i).ToList();
                        shwmdl.Rework = r.Count;

                        resLst.Add(shwmdl);

                        mdl.showdetail = resLst;


                    }

                }

                LstDash.Add(mdl);

            }

            return LstDash;
        }
        public List<Prosol_Dashboard> bindCatalogueTarget(string[] plants, string userId)
        {
            List<Prosol_Dashboard> LstDash = new List<Prosol_Dashboard>();
            foreach (string pnt in plants)
            {
                Prosol_Dashboard mdl = new Prosol_Dashboard();
                string[] flds = { "Itemcode", "Plant", "Plant_" };
                var fields = Fields.Include(flds).Exclude("_id");
                var query = Query.EQ("Plant", pnt);
                var plantList = _erpRepository.FindAll(fields, query).ToList();

                mdl.PlantCode = pnt;
                var PltQry = Query.EQ("Plantcode", pnt);
                var plntName = _PlanttRepository.FindOne(PltQry);
                mdl.PlantName = plntName.Plantname;

                var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Cataloguer"));
                var usrLst = _usersRepository.FindAll(UsrQry).ToList();
                var proUserList = new List<Prosol_Users>();
                foreach (Prosol_Users pro in usrLst)
                {
                    foreach (TargetExn xtn in pro.Roles)
                    {
                        if (xtn.Name == "Cataloguer" && xtn.TargetId == userId)
                        {
                            proUserList.Add(pro);
                        }
                    }
                }
                string[] flds1 = { "Itemcode", "UpdatedOn", "Release", "Review", "Catalogue", "ItemStatus", "Rework", "Reworkcat" };
                var fields1 = Fields.Include(flds1).Exclude("_id");

                //  var query1 = Query.Or(Query.EQ("Duplicates", ""), Query.EQ("Duplicates", BsonNull.Value));
                var dataList = _DatamasterRepository.FindAll(fields1).ToList();
                List<Prosol_showDetail> resLst = new List<Prosol_showDetail>();

                foreach (Prosol_Users usr in proUserList)
                {
                    Prosol_showDetail shwmdl = new Prosol_showDetail();
                    shwmdl.Username = usr.UserName;

                    var TotCreated = (from data in dataList join plant in plantList on data.Itemcode equals plant.Itemcode where data.Catalogue != null && data.Catalogue.UserId == usr.Userid select data).ToList();
                    if (TotCreated.Count > 0)
                    {
                        var p = (from i in TotCreated where i.ItemStatus >= 0 && i.ItemStatus < 2 && i.Reworkcat == null select i).ToList();
                        shwmdl.Pending = p.Count;

                        var c = (from i in TotCreated where i.ItemStatus >= 2 select i).ToList();
                        shwmdl.Completed = c.Count;

                        var r = (from i in TotCreated where i.ItemStatus >= 0 && i.ItemStatus < 2 && i.Reworkcat != null select i).ToList();
                        shwmdl.Rework = r.Count;

                        resLst.Add(shwmdl);

                        mdl.showdetail = resLst;


                    }

                }

                LstDash.Add(mdl);

            }

            return LstDash;
        }
        //public List<Prosol_Dashboard> bindApproveTarget(string[] plants, string userId)
        //{
        //    List<Prosol_Dashboard> LstDash = new List<Prosol_Dashboard>();
        //    foreach (string pnt in plants)
        //    {
        //        Prosol_Dashboard mdl = new Prosol_Dashboard();
        //        string[] flds = { "Itemcode", "Plant", "Plant_" };
        //        var fields = Fields.Include(flds).Exclude("_id");
        //        var query = Query.EQ("Plant", pnt);
        //        var plantList = _erpRepository.FindAll(fields, query).ToList();

        //        mdl.PlantCode = pnt;
        //        var PltQry = Query.EQ("Plantcode", pnt);
        //        var plntName = _PlanttRepository.FindOne(PltQry);
        //        mdl.PlantName = plntName.Plantname;

        //       // var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Approver"), Query.EQ("Roles.TargetId", userId));
        //       // var usrLst = _usersRepository.FindAll(UsrQry).ToList();

        //        var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Approver"));
        //        var usrLst = _usersRepository.FindAll(UsrQry).ToList();
        //        var proUserList = new List<Prosol_Users>();
        //        foreach (Prosol_Users pro in usrLst)
        //        {
        //            foreach (TargetExn xtn in pro.Roles)
        //            {
        //                if (xtn.Name == "Approver" && xtn.TargetId == userId)
        //                {
        //                    proUserList.Add(pro);
        //                }
        //            }
        //        }


        //        string[] flds1 = { "itemId", "plant", "requester", "approver", "itemStatus", "cataloguer" };
        //        var fields1 = Fields.Include(flds1).Exclude("_id");

        //        var query1 = Query.NE("itemStatus", "deleted");
        //        var dataList = _ItemRequestRepository.FindAll(fields1).ToList();
        //        List<Prosol_showDetail> resLst = new List<Prosol_showDetail>();

        //        foreach (Prosol_Users usr in proUserList)
        //        {
        //            Prosol_showDetail shwmdl = new Prosol_showDetail();
        //            shwmdl.Username = usr.UserName;

        //            var TotCreated = (from data in dataList where data.approver != null && data.approver == usr.Userid && data.plant == pnt select data).ToList();
        //            if (TotCreated.Count > 0)
        //            {
        //                var p = (from i in TotCreated where i.itemStatus == "pending" select i).ToList();
        //                shwmdl.Pending = p.Count;

        //                var c = (from i in TotCreated where i.itemStatus == "approved" select i).ToList();
        //                shwmdl.Completed = c.Count;

        //                var r = (from i in TotCreated where i.itemStatus == "rejected" select i).ToList();
        //                shwmdl.Rework = r.Count;

        //                resLst.Add(shwmdl);

        //                mdl.showdetail = resLst;


        //            }

        //        }

        //        LstDash.Add(mdl);

        //    }

        //    return LstDash;
        //}
        //public List<Prosol_Dashboard> bindRequestTarget(string[] plants, string userId)
        //{
        //    List<Prosol_Dashboard> LstDash = new List<Prosol_Dashboard>();
        //    foreach (string pnt in plants)
        //    {
        //        Prosol_Dashboard mdl = new Prosol_Dashboard();
        //        string[] flds = { "Itemcode", "Plant", "Plant_" };
        //        var fields = Fields.Include(flds).Exclude("_id");
        //        var query = Query.EQ("Plant", pnt);
        //        var plantList = _erpRepository.FindAll(fields, query).ToList();

        //        mdl.PlantCode = pnt;
        //        var PltQry = Query.EQ("Plantcode", pnt);
        //        var plntName = _PlanttRepository.FindOne(PltQry);
        //        mdl.PlantName = plntName.Plantname;

        //        // var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Requester"), Query.EQ("Roles.TargetId", userId));
        //        //  var usrLst = _usersRepository.FindAll(UsrQry).ToList();
        //        var UsrQry = Query.And(Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Roles.Name", "Requester"));
        //        var usrLst = _usersRepository.FindAll(UsrQry).ToList();
        //        var proUserList = new List<Prosol_Users>();
        //        foreach (Prosol_Users pro in usrLst)
        //        {
        //            foreach (TargetExn xtn in pro.Roles)
        //            {
        //                if (xtn.Name == "Requester" && xtn.TargetId == userId)
        //                {
        //                    proUserList.Add(pro);
        //                }
        //            }
        //        }

        //        string[] flds1 = { "itemId", "plant", "requester", "approver", "itemStatus", "cataloguer" };
        //        var fields1 = Fields.Include(flds1).Exclude("_id");

        //        var query1 = Query.NE("itemStatus", "deleted");
        //        var dataList = _ItemRequestRepository.FindAll(fields1).ToList();
        //        List<Prosol_showDetail> resLst = new List<Prosol_showDetail>();

        //        foreach (Prosol_Users usr in proUserList)
        //        {
        //            Prosol_showDetail shwmdl = new Prosol_showDetail();
        //            shwmdl.Username = usr.UserName;

        //            var TotCreated = (from data in dataList where data.approver != null && data.requester == usr.Userid && data.plant == pnt select data).ToList();
        //            if (TotCreated.Count > 0)
        //            {
        //                var p = (from i in TotCreated where i.itemStatus == "pending" select i).ToList();
        //                shwmdl.Pending = p.Count;

        //                var c = (from i in TotCreated where i.itemStatus == "approved" select i).ToList();
        //                shwmdl.Completed = c.Count;

        //                var r = (from i in TotCreated where i.itemStatus == "rejected" select i).ToList();
        //                shwmdl.Rework = r.Count;

        //                resLst.Add(shwmdl);

        //                mdl.showdetail = resLst;


        //            }

        //        }

        //        LstDash.Add(mdl);

        //    }

        //    return LstDash;
        //}

        // Item History
        public List<Prosol_ActionHistory> BindItemHistory(string[] plants)
        {
            List<Prosol_ActionHistory> LstDash = new List<Prosol_ActionHistory>();

            if (plants == null)
            {
                var PltQry = Query.EQ("Islive", true);
                var plantLst = _PlanttRepository.FindAll(PltQry).ToList();
                plants = new string[plantLst.Count];
                int i = 0;
                foreach (Prosol_Plants mdl in plantLst)
                {
                    plants[i] = mdl.Plantcode;
                    i++;
                }

            }
            foreach (string pnt in plants)
            {
                Prosol_ActionHistory mdl = new Prosol_ActionHistory();
                string[] flds = { "Itemcode", "Plant", "Plant_" };
                var fields = Fields.Include(flds).Exclude("_id");
                var query = Query.EQ("Plant", pnt);
                var plantList = _erpRepository.FindAll(fields, query).ToList();

                var PltQry = Query.EQ("Plantcode", pnt);
                var plantName = _PlanttRepository.FindOne(PltQry);
                mdl.Plant = plantName.Plantname;

                string[] flds1 = { "Itemcode", "UpdatedOn", "Catalogue", "Review", "Release", "ItemStatus" };
                var fields1 = Fields.Include(flds1).Exclude("_id");


                var date = DateTime.SpecifyKind(DateTime.Now.AddDays(-60), DateTimeKind.Utc);

                // date = DateTime.Parse(date, new CultureInfo("en-US", true));
                // date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

                //total catalogue
                //var QryAll =  Query.LT("UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now.AddDays(-60), DateTimeKind.Utc)));
                // var query3 = Query.LT("ItemStatus", 2);
                var dataList2 = _DatamasterRepository.FindAll(fields1);
                var totCatalogue = (from data in dataList2 join plant in plantList on data.Itemcode equals plant.Itemcode where data.Catalogue != null && data.Catalogue.UpdatedOn >= date && data.ItemStatus > 1 select data).ToList();
                if (totCatalogue.Count > 0)
                {
                    foreach (Prosol_Datamaster pdm in totCatalogue)
                    {
                        mdl = new Prosol_ActionHistory();
                        mdl.Datetime = DateTime.Parse(pdm.Catalogue.UpdatedOn.ToString());
                        //  mdl.Datetime = DateTime.Parse(pdm.Catalogue.UpdatedOn.ToString(), new CultureInfo("en-US", true));
                        mdl.ActionPerformed = "Item code : " + pdm.Itemcode + " , has been catalogued by " + pdm.Catalogue.Name;
                        LstDash.Add(mdl);
                    }

                }

                //total Review
                // var query4 = Query.And(Query.GTE("ItemStatus", 2), Query.LT("ItemStatus", 4));
                var dataList3 = _DatamasterRepository.FindAll(fields1);
                var totReview = (from data in dataList3 join plant in plantList on data.Itemcode equals plant.Itemcode where data.Review != null && data.Review.UpdatedOn >= date && data.ItemStatus > 3 select data).ToList();
                if (totReview.Count > 0)
                {
                    foreach (Prosol_Datamaster pdm in totReview)
                    {
                        mdl = new Prosol_ActionHistory();
                        mdl.Datetime = DateTime.Parse(pdm.Review.UpdatedOn.ToString());
                        mdl.ActionPerformed = "Item code : " + pdm.Itemcode + " , has been reviewed by " + pdm.Review.Name;
                        LstDash.Add(mdl);
                    }
                }

                //total release
                // var query5 = Query.And(Query.GTE("ItemStatus", 4), Query.LT("ItemStatus", 6));
                var dataList4 = _DatamasterRepository.FindAll(fields1);
                var TotRelease = (from data in dataList4 join plant in plantList on data.Itemcode equals plant.Itemcode where data.Release != null && data.Release.UpdatedOn >= date && data.ItemStatus == 6 select data).ToList();
                if (TotRelease.Count > 0)
                {
                    foreach (Prosol_Datamaster pdm in TotRelease)
                    {
                        mdl = new Prosol_ActionHistory();
                        mdl.Datetime = DateTime.Parse(pdm.Release.UpdatedOn.ToString());
                        mdl.ActionPerformed = "Item code : " + pdm.Itemcode + " , has been released by " + pdm.Release.Name;
                        LstDash.Add(mdl);
                    }
                }


                //total request pending

                var usrLst = _usersRepository.FindAll().ToList();

                // string[] fds = { "itemId", "Plant","requestedOn", };
                // var filds = Fields.Include(fds).Exclude("_id");

                var query6 = Query.EQ("plant", pnt);
                var dataList5 = _ItemRequestRepository.FindAll(query6).ToList();
                if (dataList5.Count > 0)
                {
                    foreach (Prosol_Request pdm in dataList5)
                    {
                        mdl = new Prosol_ActionHistory();
                        var uName = (from usr in usrLst where usr.Userid == pdm.requester select new { Name = usr.UserName }).ToList();

                        mdl.Datetime = DateTime.Parse(pdm.requestedOn.ToString());
                        mdl.ActionPerformed = "Item code : " + pdm.itemId + " , has been requested by " + uName[0].Name;
                        LstDash.Add(mdl);
                    }
                }

                //total request approved
                var query7 = Query.And(Query.EQ("itemStatus", "approved"), Query.EQ("plant", pnt));
                var dataList6 = _ItemRequestRepository.FindAll(query7).ToList();
                if (dataList6.Count > 0)
                {
                    foreach (Prosol_Request pdm in dataList6)
                    {
                        mdl = new Prosol_ActionHistory();
                        var uName = (from usr in usrLst where usr.Userid == pdm.approver select new { Name = usr.UserName }).ToList();

                        mdl.Datetime = DateTime.Parse(pdm.approvedOn.ToString());
                        mdl.ActionPerformed = "Item code : " + pdm.itemId + " , has been approved by " + uName[0].Name;
                        LstDash.Add(mdl);
                    }
                }
                //total request rejected
                var query8 = Query.And(Query.EQ("itemStatus", "rejected"), Query.EQ("plant", pnt));
                var dataList7 = _ItemRequestRepository.FindAll(query8).ToList();
                if (dataList7.Count > 0)
                {
                    foreach (Prosol_Request pdm in dataList7)
                    {
                        mdl = new Prosol_ActionHistory();
                        var uName = (from usr in usrLst where usr.Userid == pdm.requester select new { Name = usr.UserName }).ToList();

                        mdl.Datetime = DateTime.Parse(pdm.requestedOn.ToString());
                        mdl.ActionPerformed = "Item code : " + pdm.itemId + " , has been rejected by " + uName[0].Name;
                        LstDash.Add(mdl);
                    }
                }


            }
            //  if (LstDash.Count > 0)
            var newLst = LstDash.OrderByDescending(item => item.Datetime).Take(1000).ToList();
            return newLst;
        }



        ////ItemHistoryGO
        //wrote by saikat chowdhury 07/01/2019


        public List<Prosol_Request> BindRequest(DateTime dte, DateTime dte1, string Uid)
        {
            dte = DateTime.SpecifyKind(dte, DateTimeKind.Utc);
            dte1 = DateTime.SpecifyKind(dte1, DateTimeKind.Utc);
            var QreAll = Query.And(Query.Or(Query.EQ("Requester", Uid), Query.EQ("approver", Uid)), Query.GTE("requestedOn", BsonDateTime.Create(dte)), Query.LT("requestedOn", BsonDateTime.Create(dte1.AddDays(1))));
            var dataList = _ItemRequestRepository.FindAll(QreAll).ToList();
            return dataList;
        }


        public List<Prosol_ActionHistory> BindItemHistory2(DateTime dte, DateTime dte1)
        {
            List<Prosol_ActionHistory> LstDash = new List<Prosol_ActionHistory>();
            dte = DateTime.SpecifyKind(dte, DateTimeKind.Utc);
            dte1 = DateTime.SpecifyKind(dte1, DateTimeKind.Utc);
            for (; dte <= dte1; dte = dte.AddDays(1))
            {
                int cunt = 0;
                var QryAll = Query.And(Query.GTE("UpdatedOn", BsonDateTime.Create(dte)), Query.LT("UpdatedOn", BsonDateTime.Create(dte.AddDays(1))));
                var dataList = _DatamasterRepository.FindAll(QryAll).ToList();
                if (dataList.Count > 0)
                {
                    var mdl = new Prosol_ActionHistory();
                    mdl.CreatedOn = DateTime.Parse(dte.ToString());

                    var p = (from i in dataList where i.ItemStatus >1 select i).ToList();
                    mdl.Completed = p.Count;

                    var p1 = (from i in dataList where i.ItemStatus == -1 select i).ToList();
                    mdl.Clarification = p1.Count;

                    var p2 = (from i in dataList where i.ItemStatus < 2 && i.ItemStatus != -1 select i).ToList();
                    mdl.Pending = p2.Count;


                    mdl.total = dataList.Count;


                    LstDash.Add(mdl);


                }

            }
            
            //////LstDash = LstDash.OrderBy(x => x.CreatedOn);
            return LstDash;
        }

        public List<list_details> BindOverAll(DateTime dte, DateTime dte1)
        {
            dte = DateTime.SpecifyKind(dte, DateTimeKind.Utc);
            dte1 = DateTime.SpecifyKind(dte1, DateTimeKind.Utc);

            var qry_f_cat_req = Query.And(Query.NE("Catalogue", BsonNull.Value), Query.GTE("UpdatedOn", BsonDateTime.Create(dte)), Query.LT("UpdatedOn", BsonDateTime.Create(dte1.AddDays(1))));
            var catcount1 = _DatamasterRepository.FindAll(qry_f_cat_req).ToList();

            var cat_tot = from r in catcount1
                          where (r.Catalogue != null || (r.RejectedBy != null && r.RejectedBy.UserId == r.Catalogue.UserId))
                          group r by r.Catalogue.Name into g
                          select new
                          {
                              Name = g.Key,
                              Req = g.Count(),
                              Completed = (from a in g where a.ItemStatus > 1 select a).Count(),
                              Pending = (from a in g where a.ItemStatus < 2 && a.ItemStatus != -1 select a).Count(),
                              Clarification = (from a in g where a.RejectedBy != null && a.RejectedBy.UserId != a.Catalogue.UserId select a).Count()

                          };
            List<list_details> ld = new List<list_details>();
            var user_res = _usersRepository.FindAll();
            foreach (var ur in user_res)
            {
                list_details ld1 = new list_details();
                if (cat_tot.Any(cus => cus.Name == ur.UserName))
                {
                    if (cat_tot.Any(cus => cus.Name == ur.UserName))
                    {
                        // if (ur.UserName != "Clarification")
                        // {
                        ld1.name = ur.UserName;
                        ld1.cat_comp = cat_tot.Where(x => x.Name == ur.UserName).SingleOrDefault().Completed;
                        ld1.cat_pen = cat_tot.Where(x => x.Name == ur.UserName).SingleOrDefault().Pending;
                        ld1.cat_clf = cat_tot.Where(x => x.Name == ur.UserName).SingleOrDefault().Clarification;
                        ld1.cat_req = cat_tot.Where(x => x.Name == ur.UserName).SingleOrDefault().Req;

                        ld.Add(ld1);
                        //}
                    }
                    else
                    {
                        ld1.name = ur.UserName;
                        ld1.cat_pen = 0;
                        ld1.cat_comp = 0;
                        ld1.cat_clf = 0;
                        ld1.cat_req = 0;
                        ld.Add(ld1);
                    }
                }

            }

            return ld;

        }


        //Dashboard

        public List<Prosol_Plants> getRepPurg()
        {
            var purgLst = _PlanttRepository.FindAll().ToList();
            return purgLst;
        }
        public List<Prosol_Datamaster> getReqItems()
        {
            var itmsLst = _DatamasterRepository.FindAll().ToList();
            return itmsLst;
        }

        public List<Prosol_Datamaster> getMasterItems()
        {
            //var query = Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", -1), Query.EQ("ItemStatus", 0)/*, Query.EQ("ItemStatus", 0)*/);
            var query = Query.And(Query.GTE("ItemStatus", -1),Query.Matches("Itemcode", BsonRegularExpression.Create(new Regex("AD".TrimStart().TrimEnd(), RegexOptions.IgnoreCase))), Query.NE("Materialcode", BsonNull.Value));
            var itmsLst = _DatamasterRepository.FindAll(query).ToList();
            return itmsLst;
        }
        public List<Prosol_Datamaster> getSKUItems()
        {
            var query = Query.And(Query.Matches("Itemcode", BsonRegularExpression.Create(new Regex("AD".TrimStart().TrimEnd(), RegexOptions.IgnoreCase))), Query.NE("Materialcode", BsonNull.Value)); 
            var itmsLst = _DatamasterRepository.FindAll(query).ToList();
            return itmsLst;
        }
        public List<Prosol_Request> getRlsdReqItems()
        {
            var query = Query.EQ("ItemStatus", 6);
            var itmsLst = _ItemRequestRepository.FindAll(query).ToList();
            return itmsLst;
        }
        public List<Prosol_Datamaster> getRlsdMasItems()
        {
            var query = Query.And(Query.EQ("ItemStatus", 6), Query.Not(Query.Matches("Itemcode", new BsonRegularExpression("^CODA"))), Query.Not(Query.Matches("Itemcode", new BsonRegularExpression("^CS"))), Query.Not(Query.Matches("Itemcode", new BsonRegularExpression("^SAM"))), Query.Not(Query.Matches("Itemcode", new BsonRegularExpression("^DEM"))));
            var itmsLst = _DatamasterRepository.FindAll(query).ToList();
            return itmsLst;
        }
        public List<Prosol_Datamaster> getPendItems()
        {
            var query = Query.EQ("ItemStatus", 0);
            var itmsLst = _DatamasterRepository.FindAll(query).ToList();
            return itmsLst;
        }
        public List<Prosol_Request> getNewItems()
        {
            var query = Query.EQ("requestStatus", "Plant Extension");
            var itmsLst = _ItemRequestRepository.FindAll(query).ToList();
            return itmsLst;
        }
        public List<Prosol_Request> getModItems()
        {
            var query = Query.Or(Query.EQ("requestStatus", "Block"), Query.EQ("requestStatus", "Unblock"));
            var itmsLst = _ItemRequestRepository.FindAll(query).ToList();
            return itmsLst;
        }


        public class list_details
        {
            public string name { get; set; }


            public int cat_comp { get; set; }

            public int cat_pen { get; set; }

            public int cat_clf { get; set; }

            public int cat_req { get; set; }
        }





    }
}
