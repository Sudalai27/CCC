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
    public partial class ItemApproveService : IItemApprove
    {
        private readonly IRepository<Prosol_Request> _ItemApprove_Request_Repository;
        private readonly IRepository<Prosol_Plants> _ItemApprove_Plant_Repository;
        private readonly IRepository<Prosol_Master> _ItemRequestSLRepository;
        private readonly IRepository<Prosol_GroupCodes> _ItemApprove_GCRepository;
        private readonly IRepository<Prosol_SubGroupCodes> _ItemApprove_SGCRepository;
        private readonly IRepository<Prosol_Users> _ItemApprove_Users_Repository;
        private readonly IRepository<Prosol_Datamaster> _ItemApprove_Datamaster_Repository;
        private readonly IRepository<Prosol_ERPInfo> _erpRepository;
        private readonly INounModifier _NounModifiService;
        public ItemApproveService(IRepository<Prosol_Request> ItemApprove_Request_Repository,
            IRepository<Prosol_Plants> ItemApprove_Plant_Repository,
           IRepository<Prosol_Master> ItemRequestSLRepository,
            IRepository<Prosol_GroupCodes> ItemApprove_GCRepository,
            IRepository<Prosol_SubGroupCodes> ItemApprove_SGCRepository,
            IRepository<Prosol_Users> ItemApprove_Users_Repository,
            IRepository<Prosol_Datamaster> ItemApprove_Datamaster_Repository,
            IRepository<Prosol_ERPInfo> erpRepository,
             INounModifier NMservice)
        {
            this._ItemApprove_Request_Repository = ItemApprove_Request_Repository;
            this._ItemApprove_Plant_Repository = ItemApprove_Plant_Repository;
            this._ItemRequestSLRepository = ItemRequestSLRepository;
            this._ItemApprove_GCRepository = ItemApprove_GCRepository;
            this._ItemApprove_SGCRepository = ItemApprove_SGCRepository;
            this._ItemApprove_Users_Repository = ItemApprove_Users_Repository;
            this._ItemApprove_Datamaster_Repository = ItemApprove_Datamaster_Repository;
            this._erpRepository = erpRepository;
            this._NounModifiService = NMservice;
        }

        public bool submit_approval(Prosol_Request pro_req, Prosol_UpdatedBy pub)
        {
            var query = Query.EQ("itemId", pro_req.itemId);

            if (pro_req.itemStatus == "approved")
            {
                var Updte = Update.Set("itemStatus", pro_req.itemStatus).Set("approvedOn", DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)).Set("cataloguer", pro_req.cataloguer);
                var flg = UpdateFlags.Upsert;

                var theresult = _ItemApprove_Request_Repository.Update(query, Updte, flg);

                Prosol_Datamaster pdm = new Prosol_Datamaster();
                pdm.Itemcode = pro_req.itemId;
                pdm.Legacy = pro_req.source;

                pdm.CreatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                pdm.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                pdm.ItemStatus = 0;
                pdm.Catalogue = pub;

                var nmRes = scrubNM(pro_req.source);
                pdm.Noun = nmRes[0];
                pdm.Modifier = nmRes[1];
                // pdm.Attachment = pro_req.attachment;
                var Qry4exRcd = Query.EQ("Itemcode", pdm.Itemcode);

                var exRcd = _ItemApprove_Datamaster_Repository.FindOne(Qry4exRcd);
                if (exRcd != null)
                {
                    pdm._id = exRcd._id;
                    theresult = _ItemApprove_Datamaster_Repository.Add(pdm);

                    var eprMdl = _erpRepository.FindOne(Qry4exRcd);
                    if (eprMdl != null)
                    {
                        eprMdl.Itemcode = pro_req.itemId;

                        string[] spltplant = pro_req.plant.Split('/');
                        eprMdl.Plant_ = spltplant[0].TrimEnd();
                        eprMdl.Plant = spltplant[1].TrimStart();

                        string[] spltSL = pro_req.storage_Location.Split('/');
                        eprMdl.StorageLocation_ = spltSL[0].TrimEnd();
                        eprMdl.StorageLocation = spltSL[1].TrimStart();

                        string[] spltMT = pro_req.Materialtype.Split('/');
                        eprMdl.Materialtype_ = spltMT[0].TrimEnd();
                        eprMdl.Materialtype = spltMT[1].TrimStart();

                        string[] spltIS = pro_req.Industrysector.Split('/');
                        eprMdl.Industrysector_ = spltIS[0].TrimEnd();
                        eprMdl.Industrysector = spltIS[1].TrimStart();

                        string[] spltMG = pro_req.Industrysector.Split('/');
                        eprMdl.MaterialStrategicGroup_ = spltMG[0].TrimEnd();
                        eprMdl.MaterialStrategicGroup = spltMG[1].TrimStart();

                        eprMdl.StandardPrice_ = pro_req.UnitPrice;


                        _erpRepository.Add(eprMdl);
                    }


                }
                else
                {
                    theresult = _ItemApprove_Datamaster_Repository.Add(pdm);
                    var eprMdl = new Prosol_ERPInfo();
                    eprMdl.Itemcode = pro_req.itemId;

                    string[] spltplant = pro_req.plant.Split('/');
                    eprMdl.Plant_ = spltplant[0].TrimEnd();
                    eprMdl.Plant = spltplant[1].TrimStart();

                    string[] spltSL = pro_req.storage_Location.Split('/');
                    eprMdl.StorageLocation_ = spltSL[0].TrimEnd();
                    eprMdl.StorageLocation = spltSL[1].TrimStart();

                    string[] spltMT = pro_req.Materialtype.Split('/');
                    eprMdl.Materialtype_ = spltMT[0].TrimEnd();
                    eprMdl.Materialtype = spltMT[1].TrimStart();

                    string[] spltIS = pro_req.Industrysector.Split('/');
                    eprMdl.Industrysector_ = spltIS[0].TrimEnd();
                    eprMdl.Industrysector = spltIS[1].TrimStart();

                    string[] spltMG = pro_req.MaterialStrategicGroup.Split('/');
                    eprMdl.MaterialStrategicGroup_ = spltMG[0].TrimEnd();
                    eprMdl.MaterialStrategicGroup = spltMG[1].TrimStart();

                    eprMdl.StandardPrice_ = pro_req.UnitPrice;


                    _erpRepository.Add(eprMdl);

                }



              
             
                return theresult;
            }
            else
            {
                var Updte = Update.Set("itemStatus", pro_req.itemStatus).Set("rejectedOn", DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)).Set("reason_rejection", pro_req.reason_rejection);
                var flg = UpdateFlags.Upsert;

                var theresult = _ItemApprove_Request_Repository.Update(query, Updte, flg);
                return theresult;
            }
        }
        private string[] scrubNM(string Legacy)
        {

            string[] strArr = new string[2];
            var NounList = _NounModifiService.GetNMinfo("");

            string[] ArrNoun = NounList.Select(p => p.Noun).Distinct().ToArray();
            // string[] ArrNounSys = NounList.Select(p => p.Similaritems).Distinct().ToArray();
            // string[] ArrNMAbb = NounList.Select(p => p.Nounabv).Distinct().ToArray();

            var scrubLst = _NounModifiService.GetScrubNMinfo();
            // string[] ScrubNoun = scrubLst.Select(p => p.Scrubnoun).Distinct().ToArray();


            Legacy = Legacy.Replace(';', ' ');
            Legacy = Legacy.Replace(':', ' ');
            Legacy = Legacy.Replace(',', ' ');
            // Legacy = Legacy.Replace('.', ' ');
            Legacy = Legacy.Replace("  ", " ");
            string[] ArrLegacy = Legacy.Split(' ');
            var tempLst = new List<Prosol_NounModifiers>();
            var HighpriorityLst = new List<Prosol_NounModifiers>();
            int flg = 0;
            foreach (string str in ArrLegacy)
            {
                // int g = 0;
                if (!string.IsNullOrEmpty(str))
                {

                    var NounandAbbr = NounList.Where(x => (x.Noun.StartsWith(str)) || (x.Nounabv != null && x.Nounabv.StartsWith(str))).ToList();
                    if (NounandAbbr != null && NounandAbbr.Count > 0)
                    {
                        foreach (string Mstr in ArrLegacy)
                        {
                            if (Mstr != "")
                            {
                                var ModifierandAbbr = NounList.Where(x => x.Noun == NounandAbbr[0].Noun && (x.Modifier.StartsWith(str) || x.Nounabv.Contains(" " + Mstr))).ToList();

                                if (ModifierandAbbr != null && ModifierandAbbr.Count > 0)
                                {
                                    strArr[0] = ModifierandAbbr[0].Noun;
                                    strArr[1] = ModifierandAbbr[0].Modifier;
                                    flg = 1;
                                    break;

                                }
                            }
                        }
                        if (flg == 1)
                            break;
                    }


                    if (flg == 0)
                    {
                        var nmScrub = scrubLst.Where(x => x.Scrubnoun != null && x.Scrubnoun.StartsWith(str)).ToList();
                        if (nmScrub != null && nmScrub.Count > 0)
                        {

                            foreach (string Mstr in ArrLegacy)
                            {
                                if (Mstr != "")
                                {
                                    var objLst1 = scrubLst.Where(x => x.ScrubModifier != null && x.Scrubnoun == nmScrub[0].Scrubnoun && x.ScrubModifier.Contains(Mstr)).ToList();
                                    if (objLst1 != null && objLst1.Count > 0)
                                    {
                                        strArr[0] = objLst1[0].Noun;
                                        strArr[1] = objLst1[0].Modifier;
                                        flg = 1;
                                        break;
                                    }
                                }

                            }
                            if (flg == 1)
                                break;
                        }
                    }
                    if (flg == 0)
                    {
                        var SimNM = NounList.Where(x => x.ModifierEqu != null && x.ModifierEqu.Contains(str)).ToList();
                        if (SimNM != null && SimNM.Count > 0 && str.Length > 2)
                        {
                            foreach (string Mstr in ArrLegacy)
                            {
                                if (Mstr != "")
                                {
                                    var objLst1 = NounList.Where(x => x.ModifierEqu != null && x.Noun == SimNM[0].Noun && x.ModifierEqu.Contains(Mstr)).ToList();
                                    if (objLst1 != null && objLst1.Count > 0)
                                    {
                                        strArr[0] = objLst1[0].Noun;
                                        strArr[1] = objLst1[0].Modifier;
                                        flg = 1;
                                        break;
                                    }
                                }
                            }
                            if (flg == 1)
                                break;
                        }
                    }
                    if (flg == 0)
                    {
                        var SimNM = NounList.Where(x => x.NounEqu != null && x.NounEqu.Contains(str)).ToList();
                        if (SimNM != null && SimNM.Count > 0 && str.Length > 2)
                        {
                            foreach (string Mstr in ArrLegacy)
                            {
                                if (Mstr != "")
                                {
                                    var objLst1 = NounList.Where(x => x.Modifier != null && x.Noun == SimNM[0].Noun && x.Modifier.Contains(Mstr)).ToList();
                                    if (objLst1 != null && objLst1.Count > 0)
                                    {
                                        strArr[0] = objLst1[0].Noun;
                                        strArr[1] = objLst1[0].Modifier;
                                        flg = 1;
                                        break;
                                    }
                                }
                            }
                            if (flg == 0)
                            {
                                strArr[1] = "NO MODIFIER";
                                flg = 1;
                            }
                            break;
                        }
                    }
                    //Noun contains
                    if (flg == 0)
                    {
                        if (ArrNoun.Contains(str.TrimEnd('.'), StringComparer.CurrentCultureIgnoreCase))
                        {
                            strArr[0] = str.ToUpper().TrimEnd('.');

                            var ModifierList = _NounModifiService.GetModifierList(str.ToUpper());

                            string[] ArrModifier = ModifierList.Select(p => p.Modifier).ToArray();

                            //  int flg = 0;
                            foreach (string Mstr in ArrLegacy)
                            {
                                if (Mstr != "")
                                {
                                    if (ArrModifier.Contains(Mstr, StringComparer.CurrentCultureIgnoreCase))
                                    {
                                        flg = 1;
                                        strArr[1] = Mstr.ToUpper();


                                    }
                                }
                            }
                            if (flg == 0)
                                strArr[1] = "NO MODIFIER";
                            break;


                        }
                    }
                }
            }
            return strArr;

        }
        public IEnumerable<Prosol_Users> get_cataloguer_emailid(string userid)
        {
            var query = Query.And(Query.EQ("Userid", userid), Query.EQ("Islive", "Active"));
            var cataloguer = _ItemApprove_Users_Repository.FindAll(query).ToList();
            return cataloguer;
        }


        public IEnumerable<Prosol_Request> get_itemsToApprove(string userid)
        {
            var sortt = SortBy.Descending("requestedOn");
            var query = Query.And(Query.EQ("approver", userid), Query.EQ("itemStatus", "pending"));
            var approver = _ItemApprove_Request_Repository.FindAll(query, sortt).ToList();

            var query1 = Query.And(Query.Or(Query.EQ("Roles", "Requester"), Query.EQ("Roles", "SuperAdmin"), Query.EQ("Islive", "Active")));
            var user = _ItemApprove_Users_Repository.FindAll(query1).ToList();
            if (user != null && approver != null)
            {
                foreach (Prosol_Users pr in user)
                {
                    foreach (Prosol_Request pp in approver)
                    {
                        if (pp.requester == pr.Userid)
                        {
                            pp.requester = pr.UserName;
                        }
                    }
                }
                return approver;
            }
            else
            {
                return approver;
            }
            // var query =Query.And(Query.EQ("approver",userid), Query.EQ("itemStatus", "pending"));
            // var approver = _ItemApprove_Request_Repository.FindAll(query).ToList();
            //// approver[0].approver = "lijoio";
            //// Prosol_Request
            // return approver;

        }
        public IEnumerable<Prosol_Users> get_req(string usrname)
        {
            var query = Query.And(Query.EQ("UserName", usrname), Query.EQ("Islive", "Active"));
            var cataloguer = _ItemApprove_Users_Repository.FindAll(query).ToList();
            return cataloguer;
        }
        public IEnumerable<Prosol_Request> getApproved_Records(string userid)
        {
            var sortt = SortBy.Descending("approvedOn");
            var query = Query.And(Query.EQ("approver", userid), Query.EQ("itemStatus", "approved"));
            var approver = _ItemApprove_Request_Repository.FindAll(query, sortt).ToList();
            return approver;

        }

        public IEnumerable<Prosol_Request> getRejected_Records(string userid)
        {
            var sortt = SortBy.Descending("rejectedOn");
            var query = Query.And(Query.EQ("approver", userid), Query.EQ("itemStatus", "rejected"));
            var approver = _ItemApprove_Request_Repository.FindAll(query, sortt).ToList();
            return approver;
        }
        public IEnumerable<Prosol_Request> getsingle_requested_record(string abcsony)
        {
            var query = Query.EQ("itemId", abcsony);
            var singlerow = _ItemApprove_Request_Repository.FindAll(query).ToList();

            string str = DateTime.Parse(singlerow[0].requestedOn.ToString()).ToString("dd/MM/yyyy");//.ToLongDateString();
                                                                                                    // string str1 = str.Substring(0, 6) + " " + str.Substring(str.Length - 4);
            if (singlerow.Count > 0)
                singlerow[0].approver = str;
            Prosol_Request pr = new Prosol_Request();

            var query1 = Query.And(Query.EQ("Userid", singlerow[0].requester), Query.EQ("Islive", "Active"));
            var requester1 = _ItemApprove_Users_Repository.FindAll(query1).ToList();
            if (requester1.Count > 0)
                pr.requester = requester1[0].UserName;

            var query2 = Query.EQ("Plantcode", singlerow[0].plant);
            var plant2 = _ItemApprove_Plant_Repository.FindAll(query2).ToList();
            if (plant2.Count > 0)
                pr.plant = plant2[0].Plantname;

            var query3 = Query.And(Query.EQ("Plantcode", singlerow[0].plant), Query.EQ("Code", singlerow[0].storage_Location), Query.EQ("Label", "Storage location"));
            var sl2 = _ItemRequestSLRepository.FindAll(query3).ToList();
            if (sl2.Count > 0)
                pr.storage_Location = sl2[0].Title;

            if (singlerow[0].group != null)
            {
                var query4 = Query.EQ("code", singlerow[0].group);
                var group2 = _ItemApprove_GCRepository.FindAll(query4).ToList();
                if (group2.Count > 0)
                    pr.group = group2[0].title;
            }
            if (singlerow[0].subGroup != null)
            {
                var query5 = Query.And(Query.EQ("groupCode", singlerow[0].group), Query.EQ("code", singlerow[0].subGroup));
                var sg2 = _ItemApprove_SGCRepository.FindAll(query5).ToList();
                if (sg2.Count > 0)
                    pr.subGroup = sg2[0].title;
            }
            if (singlerow[0].Materialtype != null)
            {
                var query80 = Query.And(Query.EQ("Code", singlerow[0].Materialtype), Query.EQ("Label", "Material type"));
                var mt = _ItemRequestSLRepository.FindAll(query80).ToList();
                if (mt.Count > 0)
                    pr.Materialtype = mt[0].Title;
            }
            if (singlerow[0].Industrysector != null)
            {
                var query80 =Query.And( Query.EQ("Code", singlerow[0].Industrysector), Query.EQ("Label", "Industry sector"));
                var mt = _ItemRequestSLRepository.FindAll(query80).ToList();
                if (mt.Count > 0)
                    pr.Industrysector = mt[0].Title;
            }
            if (singlerow[0].MaterialStrategicGroup != null)
            {
                var query80 = Query.And(Query.EQ("Code", singlerow[0].MaterialStrategicGroup), Query.EQ("Label","Material Strategic Group"));
                var mt = _ItemRequestSLRepository.FindAll(query80).ToList();
                if (mt.Count > 0)
                    pr.MaterialStrategicGroup = mt[0].Title;
            }
            if (singlerow[0].UnitPrice != null)
            {
                var query80 = Query.EQ("Code", singlerow[0].UnitPrice);
                var mt = _ItemRequestSLRepository.FindAll(query80).ToList();
                if (mt.Count > 0)
                    pr.UnitPrice = mt[0].Title;
            }
            if (singlerow[0].cataloguer != null)
            {
                var query6 = Query.And(Query.EQ("Userid", singlerow[0].cataloguer), Query.EQ("Islive", "Active"));
                var cataloguer1 = _ItemApprove_Users_Repository.FindAll(query6).ToList();
                if (cataloguer1.Count > 0)
                    pr.cataloguer = cataloguer1[0].UserName;
            }

            singlerow.Add(pr);


            return singlerow;
            //var requester = _ItemApprove_Users_Repository.FindAll(query1).ToList();
            //return singlerow.ToBsonDocument;
        }


        public IEnumerable<Prosol_Users> getcataloguernames_id()
        {
            var query = Query.And(Query.EQ("Usertype", "Cataloguer"), Query.EQ("Islive", "Active"));
            var cataloguer = _ItemApprove_Users_Repository.FindAll(query).ToList();
            return cataloguer;
        }

   

    }
}
