using Excel;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace Prosol.Core
{
    public partial class ServiceMasterService : IServiceMaster
    {
        private readonly IRepository<Prosol_ServiceCategory> _ServiceMastersercatRepository;
        private readonly IRepository<Prosol_ServiceGroup> _ServiceMastergrpRepository;
      //  private readonly IRepository<Prosol_ServiceActivity> _ServiceMasteractRepository;
        private readonly IRepository<Prosol_ServiceUom> _ServiceMasterUomRepository;
        private readonly IRepository<Prosol_ServiceValuation> _ServiceMasterValRepository;
        private readonly IRepository<Prosol_SMainCode> _ServiceMasterMainRepository;
        private readonly IRepository<Prosol_SSubCode> _ServiceMasterSubRepository;
        private readonly IRepository<Prosol_SSubSubCode> _ServiceMasterSubSubRepository;
     //   private readonly IRepository<Prosol_SMainSubs> _ServiceMasterMNRepository;
        private readonly IRepository<Prosol_MSAttribute> _ServiceMasterMSAttRepository;

        private readonly IRepository<Prosol_UNSPSC> _unspscRepository;
        private readonly IRepository<Prosol_CodeLogic> _CodeLogicRepository;



        private readonly IRepository<Prosol_RequestServiceRunning> _ServiceMaster_reqservicerun_Repository;
        private readonly IRepository<Prosol_Users> _ItemRequestService_Users_Repository;
        private readonly IRepository<Prosol_ItemIdRunningReqser> _ItemIdRunning_Reqser_Repository;
        private readonly IRepository<Prosol_RequestService> _ItemRequest_Service_Repository;
        private readonly IRepository<Prosol_ServiceCharacteristicValue> _ServiceMasterCharacteriosticvalue_abbrRepository;
        //  private readonly IRepository<workshop> _one;
        //  private readonly IRepository<innoent_workshop> _two;
        private readonly IRepository<Prosol_Plants> _bulkreqplant_repository;
        private readonly IRepository<Prosol_UOMMODEL> _UommodelRepository;
        private readonly IRepository<Prosol_Attribute> _attributeRepository;
        private readonly IRepository<Prosol_Logic> _attrivalueRepository;
        private readonly IRepository<Prosol_ServiceDefaultAttr> _DefaultAttrRepository;
        private readonly IRepository<Prosol_Abbrevate> _AbbrRepository;
        private readonly IRepository<Prosol_Attribute> _AttriRepository;
        private readonly IRepository<Prosol_MSAttribute> _MSAttrRepository;
      




        public ServiceMasterService(IRepository<Prosol_ServiceCategory> ServiceMastersercatRepository,
            IRepository<Prosol_ServiceCharacteristicValue> ServiceMasterCharacteriosticvalue_abbrRepository,
                                    IRepository<Prosol_ServiceGroup> ServiceMastergrpRepository,
                                 //   IRepository<Prosol_ServiceActivity> ServiceMasteractRepository,
                                    IRepository<Prosol_ServiceUom> ServiceMasterUomRepository,
                                    IRepository<Prosol_ServiceValuation> ServiceMasterValRepository,
                                    IRepository<Prosol_SMainCode> ServiceMasterMainRepository,
                                    IRepository<Prosol_SSubCode> ServiceMasterSubRepository,
                                    IRepository<Prosol_SSubSubCode> ServiceMasterSubSubRepository,
                                  //  IRepository<Prosol_SMainSubs> ServiceMasterMNRepository,
                                    IRepository<Prosol_MSAttribute> ServiceMasterMSAttRepository,
                                    IRepository<Prosol_RequestServiceRunning> ServiceMaster_reqservicerun_Repository,
                                    IRepository<Prosol_Users> ItemRequestService_Users_Repository,
                                    IRepository<Prosol_ItemIdRunningReqser> ItemIdRunning_Reqser_Repository,
                                    IRepository<Prosol_RequestService> ItemRequest_Service_Repository,

                                    //  IRepository<workshop> One,
                                    //  IRepository<innoent_workshop> Two
                                      IRepository<Prosol_Plants> bulkreqplant_repository,
                                      IRepository<Prosol_UOMMODEL> UommodelRepository,
                                      IRepository<Prosol_Attribute> attributeRepository,
                                      IRepository<Prosol_Logic> attrivalueRepository,
                                      IRepository<Prosol_ServiceDefaultAttr> DefaultAttrRepository,
                                      IRepository<Prosol_Abbrevate> AbbrRepository,
                                      IRepository<Prosol_Attribute> AttriRepository,
                                      IRepository<Prosol_MSAttribute> MSAttrRepository,
                                       IRepository<Prosol_CodeLogic> codelogicRepository,
                                       IRepository<Prosol_UNSPSC> unspscRepository

                                    )
        {
            this._ServiceMastersercatRepository = ServiceMastersercatRepository;
            this._ServiceMasterCharacteriosticvalue_abbrRepository = ServiceMasterCharacteriosticvalue_abbrRepository;
            this._ServiceMastergrpRepository = ServiceMastergrpRepository;
           // this._ServiceMasteractRepository = ServiceMasteractRepository;
            this._ServiceMasterUomRepository = ServiceMasterUomRepository;
            this._ServiceMasterValRepository = ServiceMasterValRepository;
            this._ServiceMasterMainRepository = ServiceMasterMainRepository;
            this._ServiceMasterSubRepository = ServiceMasterSubRepository;
            this._ServiceMasterSubSubRepository = ServiceMasterSubSubRepository;
           // this._ServiceMasterMNRepository = ServiceMasterMNRepository;
            this._ServiceMasterMSAttRepository = ServiceMasterMSAttRepository;
            this._ServiceMaster_reqservicerun_Repository = ServiceMaster_reqservicerun_Repository;
            this._ItemRequestService_Users_Repository = ItemRequestService_Users_Repository;
            this._ItemIdRunning_Reqser_Repository = ItemIdRunning_Reqser_Repository;
            this._ItemRequest_Service_Repository = ItemRequest_Service_Repository;

            //  this._one = One;
            //  this._two = Two;
            this._bulkreqplant_repository = bulkreqplant_repository;
            this._UommodelRepository = UommodelRepository;
            this._attributeRepository = attributeRepository;
            this._attrivalueRepository = attrivalueRepository;
            this._DefaultAttrRepository = DefaultAttrRepository;
            this._AbbrRepository = AbbrRepository;
            this._AttriRepository = AttriRepository;
            this._MSAttrRepository = MSAttrRepository;
            this._unspscRepository = unspscRepository;
            this._CodeLogicRepository = codelogicRepository;

        }

        public List<Prosol_Users> getCleanser()
        {
            ///
            var query = Query.And(Query.EQ("Roles.Name", "Cataloguer"), Query.EQ("Islive", "Active"));
            ///
           // var query = Query.EQ("Roles.Name", "Cataloguer");
           // string[] arr = { "Cataloguer" };
           // var query = Query.In("Usertype", new BsonArray(arr));
            var res = _ItemRequestService_Users_Repository.FindAll(query).ToList();
            return res;
        }
 
        //category
        public bool InsertDatasercat(Prosol_ServiceCategory data,int update)
        {
            bool res = false;
            if (update == 1)
            {
                res = _ServiceMastersercatRepository.Add(data);
                return res;
            }
            else
            {
           // bool res = false;
            data.Islive = true;
            var query = Query.Or(Query.EQ("SeviceCategorycode", data.SeviceCategorycode), Query.EQ("SeviceCategoryname", data.SeviceCategoryname));
            var vn = _ServiceMastersercatRepository.FindAll(query).ToList();
            if (vn.Count == 0)
            {
                res = _ServiceMastersercatRepository.Add(data);
            }
            return res;
        }
        }
        public bool insert_Rerequest(List<Prosol_RequestService> request)
        {
            var f_res = false;
            var qry = Query.EQ("ItemId", request[0].ItemId);
            var objReq = _ItemRequest_Service_Repository.FindOne(qry);
            var qry1 = Query.EQ("Userid", objReq.requester.UserId);
            var app= _ItemRequestService_Users_Repository.FindOne(qry1);
            if (objReq.RequestStatus == "Clarification" || objReq.ServiceStatus == "Clarification")
            {
                objReq.RequestStatus = "Cleanse";
                objReq.ServiceStatus = "Cleanse";
                objReq.Reject_reason = request[0].Reject_reason;
                //if (app.Roles[0].Name == "Requester")
                //{
                //    objReq.approver = app.Roles[0].TargetId;
                //}
               
                f_res = _ItemRequest_Service_Repository.Add(objReq);

                //var qry1 = Query.EQ("Itemcode", request[0].ItemId);
                //var objCat = _DatamasterRepository.FindOne(qry1);
                //objCat.ItemStatus = 0;
                //objCat.Legacy = request[0].Legacy;
                //objCat.Remarks = request[0].Reject_reason;
                //_DatamasterRepository.Add(objCat);
                //return f_res;
            }
            else
            {
                objReq.RequestStatus = "pending";
                objReq.Reject_reason = request[0].Reject_reason;
                objReq.PlantCode = request[0].PlantCode;
                objReq.ServiceGroupCode = request[0].ServiceGroupCode;
                objReq.ServiceGroupName = request[0].ServiceGroupName;
                objReq.Legacy = request[0].Legacy;
                objReq.ShortDesc = request[0].ShortDesc;
               
                //objReq.UnitPrice = request[0].UnitPrice;

                f_res = _ItemRequest_Service_Repository.Add(objReq);
                //return f_res;
            }


          


            return f_res;
        }
        public IEnumerable<Prosol_ServiceCategory> showall_Categoryuser()
        {


            //**** Dont delete it, it was done for balu sir   ****//

            //var from = _one.FindAll().ToList();

            //foreach (workshop ws in from)
            //{
            //    innoent_workshop iw = new innoent_workshop();
            //    iw._id = ws._id;
            //    iw._class = ws._class;
            //    iw.userId = ws.userId;
            //    iw.targetAudience = ws.targetAudience;
            //    iw.howToConnect = ws.howToConnect;
            //    iw.drawing = ws.drawing;
            //    iw.drawingDescription = ws.drawingDescription;
            //    iw.model = ws.model;
            //    iw.modelDescription = ws.modelDescription;
            //    iw.protoType = ws.protoType;
            //    iw.howItWorkes = ws.howItWorkes;
            //    iw.parts =ws.parts.Select(c => c.ToString()).ToArray();
            //    iw.product_evaluate_actions = ws.product_evaluate_actions;
            //    iw.solutionId = ws.solutionId;
            //    iw.what_is_the_product = ws.what_is_the_product;
            //    iw.how_does_it_work = ws.how_does_it_work;
            //    iw.how_would_you_make_it = ws.how_would_you_make_it;
            //    iw.what_should_it_look_like = ws.what_should_it_look_like;
            //    iw.lastChangeTime = ws.lastChangeTime;
            //    iw.lastChanegeBy = ws.lastChanegeBy;
            //    iw.createdBy = ws.createdBy;
            //    iw.versionNo = ws.versionNo;
            //    iw.pros = ws.pros;
            //    iw.cons = ws.cons;
            //    iw.steps = ws.steps;
            //    iw.advantage = ws.advantage;
            //    iw.self_evaluate_data_collection = ws.self_evaluate_data_collection;
            //    iw.self_evaluate_use_of_knowledge = ws.self_evaluate_use_of_knowledge;
            //    iw.myName = ws.myName;
            //    iw.companyName = ws.companyName;
            //    iw.companyLogo = ws.companyLogo;
            //    iw.myBusiness = ws.myBusiness;
            //    iw.coreStaff = ws.coreStaff;
            //    iw.ppu = ws.ppu;
            //    iw.rrp = ws.rrp;
            //    iw.status = ws.status;
            //    iw.nextSteps = ws.nextSteps;
            //    iw.self_evalute_business_plan = ws.self_evalute_business_plan;
            //    iw.self_evalute_env_sustainability = ws.self_evalute_env_sustainability;
            //    iw.self_evalute_sustainability_company = ws.self_evalute_sustainability_company;
            //    iw.contact = ws.contact;
            //    iw.costOfProduction = ws.costOfProduction;
            //    iw.tagList = ws.tagList;
            //    iw.createdDate = ws.createdDate;
            //    iw.needName = ws.needName;
            //    iw.needImageUrl = ws.needImageUrl;
            //    iw.needId = ws.needId;
            //    iw.solutionName = ws.solutionName;
            //    iw.solutionDefinition = ws.solutionDefinition;
            //    iw.needDiscription = ws.needDiscription;
            //    iw.collection = ws.collection;
            //    iw.solutionDrawing = ws.solutionDrawing;
            //    iw.solution_evaluate_novelty = ws.solution_evaluate_novelty;
            //    iw.solution_evaluate_originality = ws.solution_evaluate_originality;
            //    iw.solution_evaluate_needSolutionRelation = ws.solution_evaluate_needSolutionRelation;
            //    iw.portfolioName = ws.portfolioName;
            //    iw.target_audience_product = ws.target_audience_product;
            //    iw.target_audience_features = ws.target_audience_features;
            //    iw.target_audience_other_groups = ws.target_audience_other_groups;
            //    iw.demographics_localarea = ws.demographics_localarea;
            //    iw.demographics_nationally = ws.demographics_nationally;
            //    iw.demographics_internationally = ws.demographics_internationally;
            //    iw.connecting_target_audience = ws.connecting_target_audience;
            //    iw.businessModel = ws.businessModel;
            //    iw.brands_interesting_brand = ws.brands_interesting_brand;
            //    iw.brands_find_brand = ws.brands_find_brand;
            //    iw.competitors_localarea = ws.competitors_localarea;
            //    iw.competitors_nationally = ws.competitors_nationally;
            //    iw.competitors_internationally = ws.competitors_internationally;
            //    iw.competitors_compare_your_product = ws.competitors_compare_your_product;
            //    iw.competitors_google_patent = ws.competitors_google_patent;
            //    iw.core_staff_company_stucture = ws.core_staff_company_stucture;
            //    iw.core_staff_why = ws.core_staff_why;
            //    var res = _two.Add(iw);
            //}

            // dont delete





            //string[] Userfield = { "SeviceCategorycode", "SeviceCategoryname", "Islive", "_id" };
            //var fields = Fields.Include(Userfield);
            var sort = SortBy.Descending("UpdatedOn");
            var shwusr = _ServiceMastersercatRepository.FindAll(sort).ToList();
            return shwusr;
        }
        public bool Dservicecategory(string id, bool sts)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("Islive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _ServiceMastersercatRepository.Update(query, Updae, flg);
            return res;
        }
        public virtual bool DelServicecode(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _ServiceMastersercatRepository.Delete(query);
            return res;

        }

        //servicegroup
        public bool InsertData(Prosol_ServiceGroup data,int update)
        {

            bool res = false;
            if (update == 1)
            {
                res = _ServiceMastergrpRepository.Add(data);
                return res;
            }
            else
            {
                // bool res = false;
                data.Islive = true;
                var query = Query.And(Query.EQ("SeviceCategorycode", data.SeviceCategorycode), Query.EQ("ServiceGroupcode", data.ServiceGroupcode));
                var vn = _ServiceMastergrpRepository.FindAll(query).ToList();
                if (vn.Count == 0)
                {
                    var query1 = Query.And(Query.EQ("SeviceCategorycode", data.SeviceCategorycode), Query.EQ("ServiceGroupname", data.ServiceGroupname));
                    var vn1 = _ServiceMastergrpRepository.FindAll(query1).ToList();
                    if (vn1.Count == 0)
                    {
                        res = _ServiceMastergrpRepository.Add(data);
                    }
                }
                return res;

            }
        }

        public IEnumerable<Prosol_ServiceGroup> showall_groupuser()
        {
            //string[] Userfield = { "SeviceCategorycode", "SeviceCategoryname", "Islive", "_id" };
            //var fields = Fields.Include(Userfield);
            var shwusr1 = _ServiceMastergrpRepository.FindAll().ToList();
            return shwusr1;
        }


        public bool Dservicegroup(string id, bool sts)
        {

            // var query = Query.And(Query.EQ("SeviceCategoryname", SeviceCategoryname), Query.EQ("ServiceGroupcode", ServiceGroupcode));
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("Islive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _ServiceMastergrpRepository.Update(query, Updae, flg);
            return res;
        }
        public virtual bool DelGroupcode(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _ServiceMastergrpRepository.Delete(query);
            return res;

        }

        //serviceactivity
        //public bool InsertDataAct(Prosol_ServiceActivity data)
        //{
        //    bool res = false;
        //    if (data._id != null && data._id.ToString() != "undefined")
        //    {
        //        res = _ServiceMasteractRepository.Add(data);
        //        return res;
        //    }
        //    else
        //    {

        //        //bool res = false;
        //        data.Islive = true;
        //        var query = Query.Or(Query.EQ("ServiceActivitycode", data.ServiceActivitycode), Query.EQ("ServiceActivityname", data.ServiceActivityname));
        //        var vn = _ServiceMasteractRepository.FindAll(query).ToList();
        //        if (vn.Count == 0)
        //        {
        //            res = _ServiceMasteractRepository.Add(data);
        //        }
        //        return res;

        //    }
        //}

        public IEnumerable<Prosol_ServiceGroup> getgroup(string SeviceCategorycode)
        {
            //string[] strfield = { "SeviceCategorycode", "SeviceCategoryname" };
            //var fields = Fields.Include(strfield).Exclude("_id");
            var query = Query.And(Query.EQ("Islive", true), Query.EQ("SeviceCategorycode", SeviceCategorycode));
            var vn = _ServiceMastergrpRepository.FindAll(query).ToList();
            return vn;
        }
        //public IEnumerable<Prosol_ServiceActivity> showall_Activityuser()
        //{

        //    var Activity = _ServiceMasteractRepository.FindAll().ToList();
        //    return Activity;
        //}


        //public bool Dserviceactivity(string id, bool sts)
        //{

        //    var query = Query.EQ("_id", new ObjectId(id));
        //    var Updae = Update.Set("Islive", sts);
        //    var flg = UpdateFlags.Upsert;
        //    var res = _ServiceMasteractRepository.Update(query, Updae, flg);
        //    return res;
        //}
        //uom
        public bool InsertDataUom(Prosol_ServiceUom data, int update)
        {
            bool res = false;
            if (update == 1)
            {
                res = _ServiceMasterUomRepository.Add(data);
                return res;
            }
            else
            {
                // bool res = false;
                data.Islive = true;
                var query = Query.Or(Query.EQ("ServiceUomcode", data.ServiceUomcode), Query.EQ("ServiceUomname", data.ServiceUomname));
                var vn = _ServiceMasterUomRepository.FindAll(query).ToList();
                if (vn.Count == 0)
                {
                    res = _ServiceMasterUomRepository.Add(data);
                }
                return res;
            }
        }
        public IEnumerable<Prosol_ServiceUom> showall_Uomuser()
        {
            //string[] Userfield = { "SeviceCategorycode", "SeviceCategoryname", "Islive", "_id" };
            //var fields = Fields.Include(Userfield);
            var shwusr = _ServiceMasterUomRepository.FindAll().ToList();
            return shwusr;
        }
        public bool DserviceUom(string id, bool sts)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("Islive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _ServiceMasterUomRepository.Update(query, Updae, flg);
            return res;

        }
        public bool DelUOMcode(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _ServiceMasterUomRepository.Delete(query);
            return res;
        }

        //valuation
        public bool InsertDataValuation(Prosol_ServiceValuation data,int update)
        {
            bool res = false;
            if (update == 1)
            {
                res = _ServiceMasterValRepository.Add(data);
                return res;
            }
            else
            {
                //  bool res = false;
                data.Islive = true;
                var query = Query.Or(Query.EQ("ServiceValuationcode", data.ServiceValuationcode), Query.EQ("ServiceValuationname", data.ServiceValuationname));
                var vn = _ServiceMasterValRepository.FindAll(query).ToList();
                if (vn.Count == 0)
                {
                    res = _ServiceMasterValRepository.Add(data);
                }
                return res;
            }
        }
        public IEnumerable<Prosol_ServiceValuation> showall_Valuationuser()
        {
            //string[] Userfield = { "SeviceCategorycode", "SeviceCategoryname", "Islive", "_id" };
            //var fields = Fields.Include(Userfield);
            var shwusr = _ServiceMasterValRepository.FindAll().ToList();
            return shwusr;
        }
        public bool Dservicevaluation(string id, bool sts)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("Islive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _ServiceMasterValRepository.Update(query, Updae, flg);
            return res;

        }
        public bool DelValuationcode(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _ServiceMasterValRepository.Delete(query);
            return res;
        }


        //coading
        //maincode
        public bool InsertDataMainCode(Prosol_SMainCode data,int update)
        {
            bool res = false;
            if (update == 1)
            {
                res = _ServiceMasterMainRepository.Add(data);
                return res;
            }
            else
            {
                // bool res = false;
                data.Islive = true;
                var query = Query.Or(Query.EQ("MainCode", data.MainCode), Query.EQ("MainDiscription", data.MainDiscription));
                var vn = _ServiceMasterMainRepository.FindAll(query).ToList();
                if (vn.Count == 0)
                {
                    res = _ServiceMasterMainRepository.Add(data);
                }
                return res;
            }
        }
        public IEnumerable<Prosol_SMainCode> showall_MainCode(string catagory)
        {
            var query = Query.EQ("SeviceCategorycode", catagory);
            var shwusr = _ServiceMasterMainRepository.FindAll(query).ToList();
            return shwusr;
        }
        public IEnumerable<Prosol_SMainCode> showall_MainCode()
        {
          //  var query = Query.EQ("SeviceCategorycode", catagory);
            var shwusr = _ServiceMasterMainRepository.FindAll().ToList();
            return shwusr;
        }
        public bool DMainCode(string id, bool sts)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("Islive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _ServiceMasterMainRepository.Update(query, Updae, flg);
            return res;

        }
        public bool DelMaincode(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _ServiceMasterMainRepository.Delete(query);
            return res;
        }
        //subcode
        public bool InsertDataSubCode(Prosol_SSubCode data, int update)
        {
            bool res = false;
            if (update == 1)
            {
                res = _ServiceMasterSubRepository.Add(data);
                return res;
            }
            else
            {
                //  bool res = false;
                data.Islive = true;
                var query = Query.And(Query.EQ("MainCode", data.MainCode), Query.EQ("SubDiscription", data.SubDiscription));
                var vn = _ServiceMasterSubRepository.FindAll(query).ToList();
                if (vn.Count == 0)
                {
                    var query1 = Query.And(Query.EQ("MainCode", data.MainCode), Query.EQ("SubCode", data.SubCode));
                    var vn1 = _ServiceMastergrpRepository.FindAll(query1).ToList();
                    if (vn1.Count == 0)
                    {
                        res = _ServiceMasterSubRepository.Add(data);
                    }
                }
                return res;
                // bool res = false;
                //data.Islive = true;
                // res = _ServiceMasterSubRepository.Add(data);
                //return res;
            }
        }
        public IEnumerable<Prosol_SSubCode> showall_SubCodeUser()
        {
            
            var shwusr1 = _ServiceMasterSubRepository.FindAll().ToList();
            return shwusr1;
        }
        public bool DSubCode(string id, bool sts)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("Islive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _ServiceMasterSubRepository.Update(query, Updae, flg);
            return res;

        }
        public bool DelSubcode(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _ServiceMasterSubRepository.Delete(query);
            return res;
        }
        //subsub
        public bool InsertDataSubSub(Prosol_SSubSubCode data,int update)
        {
            bool res = false;
            if (update == 1)
            {
                res = _ServiceMasterSubSubRepository.Add(data);
                return res;
            }
            else
            {
                //bool res = false;
                data.Islive = true;
                var query = Query.And(Query.EQ("MainCode", data.MainCode), Query.EQ("SubCode", data.SubCode), Query.EQ("SubSubCode", data.SubSubCode));
                var vn = _ServiceMasterSubSubRepository.FindAll(query).ToList();
                if (vn.Count == 0)
                {
                    var query1 = Query.And(Query.EQ("MainCode", data.MainCode), Query.EQ("SubCode", data.SubCode), Query.EQ("SubSubDiscription", data.SubSubDiscription));
                    var vn1 = _ServiceMasterSubSubRepository.FindAll(query).ToList();
                    if (vn1.Count == 0)
                        res = _ServiceMasterSubSubRepository.Add(data);
                }
                return res;
            }
        }
        public bool Insertvalabbr(Prosol_ServiceCharacteristicValue pcv,int update)
        {
            bool res = false;
            if (update == 1)
            {
                res = _ServiceMasterCharacteriosticvalue_abbrRepository.Add(pcv);
                return res;
            }
            else
            {
              
                pcv.Islive = true;
                var query = Query.Or(Query.EQ("CharacteristicValue", pcv.CharacteristicValue), Query.EQ("ValueAbbreviation", pcv.ValueAbbreviation));
                var vn = _ServiceMasterCharacteriosticvalue_abbrRepository.FindAll(query).ToList();
                if (vn.Count == 0)
                {

                    res = _ServiceMasterCharacteriosticvalue_abbrRepository.Add(pcv);
                }
                return res;
            }
        }
        public IEnumerable<Prosol_ServiceCharacteristicValue> ListValAbbr()
        {
            var valabbr = _ServiceMasterCharacteriosticvalue_abbrRepository.FindAll().ToList();
            return valabbr;
        }

        public bool Delvalabbrcode(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _ServiceMasterCharacteriosticvalue_abbrRepository.Delete(query);
            return res;

        }
        public bool DValAbbr(string id, bool sts)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("Islive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _ServiceMasterCharacteriosticvalue_abbrRepository.Update(query, Updae, flg);
            return res;

        }

        //public IEnumerable<Prosol_SSubCode> getSubList(string MainCode)
        //{
        //    string[] strfield = { "MainCode", "MainDiscription", "SubCode", "SubDiscription", "Islive" };
        //    var fields = Fields.Include(strfield).Exclude("_id");
        //    var query = Query.And(Query.EQ("Islive", true), Query.EQ("MainCode", MainCode));
        //    var vn = _ServiceMasterSubRepository.FindAll(fields,query).ToList();
        //    return vn;
        //}
        //public IEnumerable<Prosol_SSubCode> getSubList(string MainCode)
        //{
        //    //string[] strfield = { "MainCode", "MainDiscription", "Islive" };
        //    // var fields = Fields.Include(strfield).Exclude("_id");
        //    var query = Query.And(Query.EQ("Islive", true), Query.EQ("MainCode", MainCode));
        //    //var query = Query.EQ("MainCode", MainCode);
        //    var vn = _ServiceMasterSubRepository.FindAll(query).ToList();
        //    return vn;
        //}
        public virtual IEnumerable<Prosol_SSubCode> getSubList(string MainCode)
        {
            var query = Query.EQ("MainCode", MainCode);
            var grpList = _ServiceMasterSubRepository.FindAll(query);
            return grpList;
        }
        public virtual IEnumerable<Prosol_ServiceGroup> getSubList1(string MainCode)
        {
            var query = Query.EQ("SeviceCategorycode", MainCode);
            var grpList = _ServiceMastergrpRepository.FindAll(query);
            return grpList;
        }
        //public virtual IEnumerable<Prosol_SSubCode> subcode1(string MainCode)
        //{
        //    var query = Query.EQ("groupCode", MainCode);
        //    var grpList = _ServiceMasterSubRepository.FindAll(query);
        //    return grpList;
        //}
        public IEnumerable<Prosol_SSubSubCode> showall_SubSubUser()
        {

            var Activity = _ServiceMasterSubSubRepository.FindAll().ToList();
            return Activity;
        }
        public bool DSubSub(string id, bool sts)
        {

            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("Islive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _ServiceMasterSubSubRepository.Update(query, Updae, flg);
            return res;
        }
       public bool DelSubSubcode(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _ServiceMasterSubSubRepository.Delete(query);
            return res;
        }
        //mainsub
        //public bool addMS(Prosol_SMainSubs data)
        //{

        //    bool res = false;
        //   // data.Islive = true;
        //    var query = Query.And(Query.EQ("MainCode", data.MainCode), Query.EQ("SubCode", data.SubCode));
        //    var vn = _ServiceMasterMNRepository.FindAll(query).ToList();
        //    if (vn.Count == 0)
        //    {
        //        res = _ServiceMasterMNRepository.Add(data);
        //    }
        //    return true;


        //    //bool res = false;
        //    ////data.Islive = true;
        //    // res = _ServiceMasterMNRepository.Add(data);
        //    //return res;
        //}
        //attributes
        public bool attribute(List<Prosol_MSAttribute> data)        {            var DicObj = Query.And(Query.EQ("Noun", data[0].Noun), Query.EQ("Modifier", data[0].Modifier));            var result = _ServiceMasterMSAttRepository.FindAll(DicObj).ToList();

            foreach (Prosol_MSAttribute pms in result)            {                var quryfordel = Query.EQ("_id", pms._id);
                var res1 = _ServiceMasterMSAttRepository.Delete(quryfordel);            }            int res = _ServiceMasterMSAttRepository.Add(data);            return true;        }
        //getattribute
        //public IEnumerable<Prosol_MSAttribute> getAttribute(string Activity)
        //{

        //    var query =  Query.EQ("Activity", Activity);
        //    var vn = _ServiceMasterMSAttRepository.FindAll(query).ToList();
        //    return vn;
        //}
        public IEnumerable<Prosol_ServiceDefaultAttr> getAttribute()
        {

           // var query = Query.And(Query.EQ("MainCode", MainCode), Query.EQ("SubCode", SubCode));
            var vn1 = _DefaultAttrRepository.FindAll().ToList();
            return vn1;
        }
        public List<Prosol_MSAttribute> getAttributeUniqueList()
        {
            var sort = SortBy.Ascending("Attributes");
            var AttributesList = _ServiceMasterMSAttRepository.FindAll(sort).ToList();
            return AttributesList;
        }
        //attributelist
        public IEnumerable<Prosol_Attribute> GetAttributes()
        {
            var res = _attributeRepository.FindAll();
            return res;
        }
      
        
        //attributeeeeeee

        public virtual IEnumerable<Prosol_Abbrevate> GetAbbrList()
        {

            var abbrList = _AbbrRepository.FindAll();
            return abbrList;
        }
        public Prosol_Attribute GetAttributeDetail(string Name)
        {
            var query = Query.EQ("Attribute", Name);
            var res = _AttriRepository.FindOne(query);
            return res;
        }

        public Prosol_MSAttribute GetCharacteristicvalues(string Name, string Activity)
        {
            var queryyy = Query.And(Query.EQ("Activity", Activity), Query.EQ("Attributes", Name));
            var result = _MSAttrRepository.FindOne(queryyy);
            return result;

        }
        public virtual IEnumerable<Prosol_Abbrevate> GetAbbrList(string srchtxt)
        {
            var query = Query.Or(Query.Matches("Value", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Abbrevated", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Equivalent", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("LikelyWords", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))));
            var AbbrList = _AbbrRepository.FindAll(query);
            return AbbrList;
        }

        //requestservice
        public IEnumerable<Prosol_Users> get_approvercodename()
        {
            var query = Query.And(Query.EQ("Usertype", "Approver"), Query.EQ("Islive", "Active"));
            var approver = _ItemRequestService_Users_Repository.FindAll(query).ToList();
            return approver;
        }
        //public IEnumerable<Prosol_Users> getcleansername(string sr)
        //{
        //    var query = Query.And(Query.EQ("Roles.Cataloguer", sr), Query.EQ("Islive", "Active"));
        //    var approver = _ItemRequestService_Users_Repository.FindAll(query).ToList();
        //    return approver;
        //}


        //runningnum
        public int getlast_request_R_no1(string str)
        {
            var sort = SortBy.Ascending("RunningNo");
            //  string[] order = { "RunningNo" };
            var query = Query.EQ("Sn", str);
            var result = _ServiceMaster_reqservicerun_Repository.FindAll(query, sort).ToList();
            if (result.Count > 0)
            {
                int r_no = (result[result.Count - 1].RunningNo) + 1;

                Prosol_RequestServiceRunning R_R = new Prosol_RequestServiceRunning();
                R_R.Sn = str;
                R_R.RunningNo = r_no;
                bool RES = _ServiceMaster_reqservicerun_Repository.Add(R_R);
                return r_no;
            }
            else
            {
                Prosol_RequestServiceRunning R_R = new Prosol_RequestServiceRunning();
                R_R.Sn = str;
                R_R.RunningNo = 1;
                bool RES = _ServiceMaster_reqservicerun_Repository.Add(R_R);
                return 1;
            }

        }

        public bool newRequestService(Prosol_RequestService req_model)
        {
            //try
            //{
            //    //  var query = Query.And(Query.EQ("Userid", req_model.approver), Query.EQ("Userid", req_model.approver));
            //    var query = Query.And(Query.EQ("Userid", req_model.approver), Query.EQ("Islive", "Active"));
            //    var user_deteails = _ItemRequestService_Users_Repository.FindAll(query).ToList();
            //    string email_to = user_deteails[0].EmailId;

            //    var query1 = Query.And(Query.EQ("Userid", req_model.requester), Query.EQ("Islive", "Active"));
            //    var From_user = _ItemRequestService_Users_Repository.FindAll(query1).ToList();

            //    string from = From_user[0].EmailId;
            //    using (MailMessage mail = new MailMessage(from, email_to))
            //    {

            //        string to_mail = email_to;
            //        // string userid = Data[0].Userid;

            //        email email1 = new email();

            //        email1.email_to = to_mail;
            //        email1.email_from = "codasol.madras@gmail.com";
            //        email1.subject = "New Item Request";
            //        email1.body = "You have received an item request for an item code " + req_model.ServiceCategory + "-" + req_model.ServiceGroup + "-001 from " + From_user[0].UserName;
            //        email1.IsBodyHtml = true;
            //        email1.host = "smtp.gmail.com";
            //        email1.enablessl = true;
            //        email1.UseDefaultCredentials = true;
            //        email1.Port = 587;
            //        email1.password = "codasolwestmambalam";

            //        emailservice es = new emailservice();
            //        bool val = es.sendmail(email1);
            //    }

            //    bool result = _ItemRequest_Service_Repository.Add(req_model);
            //    //  smtp.Send(msg);
            //    return result;
            //}
            //catch (Exception e)
            //{
                return false;
           // }
        }

        public IEnumerable<Prosol_Users> get_approvercode_name_using_approverid1(string app)
        {
            var query = Query.And(Query.EQ("Userid", app), Query.EQ("Islive", "Active"));
            var approver = _ItemRequestService_Users_Repository.FindAll(query).ToList();
            return approver;
        }
        //public IEnumerable<Prosol_Users> get_approvercode_name_using_approverid2(string app)
        //{
        //    var query = Query.And(Query.EQ("UserName", app), Query.EQ("Islive", "Active"));
        //    var approver = _ItemRequestService_Users_Repository.FindAll(query).ToList();
        //    return approver;
        //}
        public bool insert_multiplerequest1(List<Prosol_RequestService> request, string reqid)
        {

            var date = DateTime.Now;
            date = date.Date;
            var date1 = date.AddDays(1);
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            date1 = DateTime.SpecifyKind(date1, DateTimeKind.Utc);

            var quereryyyy = Query.And(Query.GTE("requester.UpdatedOn", BsonDateTime.Create(date)), Query.LT("requester.UpdatedOn", BsonDateTime.Create(date1)));


            int i = _ItemRequest_Service_Repository.FindAll(quereryyyy).ToList().Count() + 1;
            foreach (Prosol_RequestService pr1 in request)
            {
                pr1.RequestId = reqid;
                //  pr1.RequestedOn = DateTime.Now;
                pr1.ServiceStatus = "pending";
                pr1.RequestStatus = "pending";
                //   pr1.ItemId = reqid + "-00001";
                ///////////////
                if (pr1.ItemId == null || pr1.ItemId == "")
                {
                    // pr1.ItemId = reqid.Substring(0, 6) + "-" + 1;
                    if (i.ToString().Length == 1)
                    {
                        pr1.ItemId = reqid.Substring(0, 8) + "-" + "00000" + i;
                        // pr1.ItemId = reqid + "-" + "00000" + i;
                    }
                    if (i.ToString().Length == 2)
                    {
                        pr1.ItemId = reqid.Substring(0, 8) + "-" + "0000" + i;
                    }
                    if (i.ToString().Length == 3)
                    {
                        pr1.ItemId = reqid.Substring(0, 8) + "-" + "000" + i;
                    }
                    if (i.ToString().Length == 4)
                    {
                        pr1.ItemId = reqid.Substring(0, 8) + "-" + "00" + i;
                    }
                    if (i.ToString().Length == 5)
                    {
                        pr1.ItemId = reqid.Substring(0, 8) + "-" + "0" + i;
                    }
                    if (i.ToString().Length == 6)
                    {
                        pr1.ItemId = reqid.Substring(0, 8) + "-" + i;
                    }
                    i++;
                }
                else
                {
                    var query = Query.EQ("ItemId", pr1.ItemId);
                    var result = _ItemRequest_Service_Repository.FindOne(query);
                    pr1._id = result._id;

                }
                //var sort = SortBy.Ascending("RunningNo");
                //var query = Query.And(Query.EQ("Servicecategory", pr1.ServiceCategoryCode), Query.EQ("Servicegroup", pr1.ServiceGroupCode));
                //var result = _ItemIdRunning_Reqser_Repository.FindAll(query, sort).ToList();

                //if (result.Count > 0)
                //{
                //    pr1.ItemId = pr1.ServiceCategoryCode + "-" + pr1.ServiceGroupCode + "-" + ((result[result.Count - 1].RunningNo) + 1).ToString();
                //    Prosol_ItemIdRunningReqser i_r = new Prosol_ItemIdRunningReqser();
                //    i_r.Servicecategory = pr1.ServiceCategoryCode;
                //    i_r.Servicegroup = pr1.ServiceGroupCode;
                //    i_r.RunningNo = result[result.Count - 1].RunningNo + 1;
                //    bool res1 = _ItemIdRunning_Reqser_Repository.Add(i_r);
                //}
                //else
                //{
                //    pr1.ItemId = pr1.ServiceCategoryCode + "-" + pr1.ServiceGroupCode + "-300001";

                //    Prosol_ItemIdRunningReqser i_r = new Prosol_ItemIdRunningReqser();
                //    i_r.Servicecategory = pr1.ServiceCategoryCode;
                //    i_r.Servicegroup = pr1.ServiceGroupCode;
                //    i_r.RunningNo = 300001;
                //    bool res1 = _ItemIdRunning_Reqser_Repository.Add(i_r);
                //}

                ///////////////////////


            }
            int f_res = _ItemRequest_Service_Repository.Add(request);

            if (f_res > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //bulkload for servicemaster
        public List<BulkRequestService_Load> loaddata1(HttpPostedFileBase file)
        {
            // int cunt = 0;
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
            // List<Prosol_Import> loaddata = new List<Prosol_Import>();
            if (file.FileName.EndsWith(".xls"))
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (file.FileName.EndsWith(".xlsx"))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            reader.IsFirstRowAsColumnNames = true;
            var res = reader.AsDataSet();

            DataTable dt = res.Tables[0];
            reader.Close();
            string dkfl = dt.Columns[0].ToString();

            List<BulkRequestService_Load> brl = new List<BulkRequestService_Load>();
            var unspscVersion = _CodeLogicRepository.FindOne();
            var query = Query.And(Query.EQ("Noun", "Service"), Query.EQ("Modifier", "Service"), Query.EQ("Version", unspscVersion.unspsc_Version));
            var LstUnspsc = _unspscRepository.FindAll(query).ToList();
            if (dt.Columns[0].ToString() == "PlantName" && dt.Columns[1].ToString() == "ServiceCategoryCode" && dt.Columns[2].ToString() == "ServiceGroupCode" && dt.Columns[3].ToString() == "UomName" && dt.Columns[4].ToString() == "Legacy" && dt.Columns[5].ToString() == "Cleanser Name")
            {
                foreach (DataRow row in dt.Rows)
                {
                    BulkRequestService_Load brl1 = new BulkRequestService_Load();
                    brl1.PlantName = row["PlantName"].ToString();
                    brl1.ServiceCategoryCode = row["ServiceCategoryCode"].ToString();
                    var CategoryName = LstUnspsc.Where(u => u.Segment == brl1.ServiceCategoryCode).ToList();
                    brl1.ServiceCategoryName = CategoryName[0].SegmentTitle.ToString();
                    brl1.ServiceGroupCode = row["ServiceGroupCode"].ToString();
                    var GroupName = LstUnspsc.Where(u => u.Family == brl1.ServiceGroupCode).ToList();
                    brl1.ServiceGroupName = GroupName[0].FamilyTitle.ToString();
                    brl1.UomName = row["UomName"].ToString();
                    brl1.Legacy = row["Legacy"].ToString();
                    brl1.Cleanser = row["Cleanser Name"].ToString();

                    //List<Prosol_Datamaster> pdm = getpossibledup_bulk(" " + row["Source"].ToString());
                    //if (pdm.Count > 0)
                    //{
                    //    List<table_bulk> blktabl = new List<table_bulk>();
                    //    foreach (Prosol_Datamaster pd in pdm)
                    //    {
                    //        table_bulk blktabl1 = new table_bulk();
                    //        blktabl1.Itemno = pd.Itemcode;
                    //        blktabl1.Shortdesc = pd.Shortdesc;
                    //        blktabl1.Longdesc = pd.Longdesc;
                    //        blktabl.Add(blktabl1);
                    //    }
                    //    brl1.table_blk = blktabl != null ? blktabl : null;
                    //}
                    brl.Add(brl1);
                }


                return brl;
                //return statement of valid data;
            }
            else
            {
                // List<Bulkrequest_load> brl = new List<Bulkrequest_load>();
                return brl;
                // return statement for invalid column names;
            }

        }


        public IEnumerable<Prosol_Plants> getplantCode_Name()
        {
            var query = Query.EQ("Islive", true);
            var plantdetails = _bulkreqplant_repository.FindAll(query).ToList();
            return plantdetails;
        }

        public IEnumerable<Prosol_ServiceCategory> getcat()
         {
            var sort = SortBy.Ascending("SeviceCategoryname");
            string[] incl = { "SeviceCategorycode", "SeviceCategoryname" };
            var query = Fields.Include(incl);
            var groupcodes = _ServiceMastersercatRepository.FindAll(query, sort).ToList();
            return groupcodes;
        }
        public IEnumerable<Prosol_ServiceGroup> getgroupCode_Name()
        {
            var groupcodes = _ServiceMastergrpRepository.FindAll().ToList();
            return groupcodes;
        }
        public IEnumerable<Prosol_UOMMODEL> getsuom_Name()
        {
            var groupcodes = _UommodelRepository.FindAll().ToList();
            return groupcodes;
        }
        public string getusernameforrequest(string id)
        {
            var qry = Query.EQ("Userid", id);
            var res = _ItemRequestService_Users_Repository.FindOne(qry);

            return res != null ? res.UserName : null;
        }
        public string getuserIDforrequest(string name)
        {
            var qry = Query.EQ("UserName", name);
            var res = _ItemRequestService_Users_Repository.FindOne(qry);

            return res != null ? res.Userid : null;
        }
        public IEnumerable<Prosol_UOMMODEL> getuomlist()
        {

            //var query = Query.EQ("UOMNAME", UOMNAME);
            var vn = _UommodelRepository.FindAll().ToList();
            return vn;
        }

        //public IEnumerable<Prosol_Logic> GetValues(string Attribute)
        //{
        //    //var Qry = Query.EQ("AttributeName1", Attribute);
        //    var Qry = Query.Or(Query.EQ("AttributeName1", Attribute), Query.EQ("AttributeName2", Attribute), Query.EQ("AttributeName3", Attribute), Query.EQ("AttributeName4", Attribute));
        //    var AttributeList1 = _attrivalueRepository.FindAll(Qry).ToList();
        //    return AttributeList1;
        //}
        public List<string> GetValues(string Noun, string Modifier, string Attribute)
        {
            var Lst1 = new List<string>();

            var Qry = Query.EQ("Attribute", Attribute);
            var AttributeList = _attributeRepository.FindOne(Qry);

            if (AttributeList != null && AttributeList.ValueList != null)
            {
                var Lst = new List<ObjectId>();
                string[] strArr = { "Value" };
                var fields = Fields.Include(strArr).Exclude("_id");
                foreach (string str in AttributeList.ValueList)
                {
                    Lst.Add(new ObjectId(str));
                }
                var query = Query.In("_id", new BsonArray(Lst));
                var arrResult = _AbbrRepository.FindAll(fields, query);
                foreach (Prosol_Abbrevate mdl in arrResult)
                {
                    Lst1.Add(mdl.Value);

                }
            }
            if (Attribute != null)
            {

                string[] strArr = { "Values" };
                var fields = Fields.Include(strArr).Exclude("_id");

                var query = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.EQ("Attributes", Attribute));
                var Lstobj = _MSAttrRepository.FindAll(fields, query).ToList();

                if (Lstobj != null && Lstobj.Count > 0 && Lstobj[0].Values != null)
                {

                    var Lst = new List<ObjectId>();
                    foreach (string str in Lstobj[0].Values)
                    {
                        Lst.Add(new ObjectId(str));
                    }
                    query = Query.In("_id", new BsonArray(Lst));
                    var arrResult = _AbbrRepository.FindAll(query);
                    foreach (Prosol_Abbrevate mdl in arrResult)
                    {
                        Lst1.Add(mdl.Value);

                    }
                }
            }

            return Lst1;
        }
        public bool InsertDefaultAttr(Prosol_ServiceDefaultAttr data)
        {
          //  bool res = false;
            var data1 = _DefaultAttrRepository.Add(data);
            return data1;
        }
        public IEnumerable<Prosol_ServiceDefaultAttr> ShwDefaultAttr()
        {
        
            var shwusr = _DefaultAttrRepository.FindAll().ToList();
            return shwusr;
        }
        public bool DeleteDefAttr(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _DefaultAttrRepository.Delete(query);
            return res;
        }

        //SERIVICE MAPPING

        public List<Prosol_RequestService> frstmat()
        {
            string[] str = { "ServiceCode", "MainCode", "SubCode", "ShortDesc", "LongDesc", "UomCode","parent", "Legacy" };
            var fields = Fields.Include(str);
            var qry1 = Query.EQ("ServiceStatus", "Completed");

            var Result = _ItemRequest_Service_Repository.FindAll(fields, qry1).ToList();
            return Result;

        }


        public List<Prosol_RequestService> searching1(string term)
        {
            string[] str = { "ServiceCode", "MainCode", "SubCode", "ShortDesc", "LongDesc", "UomCode", "parent", "Legacy" };
            var fields = Fields.Include(str);
            var qry1 = Query.And(Query.Or(Query.Matches("ServiceCode", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                Query.Matches("ShortDesc", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                Query.Matches("LongDesc", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase)))),
                Query.EQ("ServiceStatus", "Completed"));

            var Result = _ItemRequest_Service_Repository.FindAll(fields, qry1).ToList();
            return Result;
        }
        public List<Prosol_RequestService> searching(string term)
        {
            string[] str = { "ServiceCode", "MainCode", "SubCode", "ShortDesc", "LongDesc", "UomCode", "Legacy" };
            var fields = Fields.Include(str);
            var qry1 = Query.And(Query.Or(Query.Matches("ServiceCode", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                Query.Matches("ShortDesc", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),
                Query.Matches("LongDesc", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase)))),
                  Query.EQ("ServiceStatus", "Completed"), Query.NE("parent", "Yes"));
            var Result = _ItemRequest_Service_Repository.FindAll(fields, qry1).ToList();
            return Result;
        }
        public List<Prosol_RequestService> srchchild(string term)
        {
            var qry = Query.EQ("ServiceCode", term);
            var arrResult2 = _ItemRequest_Service_Repository.FindAll(qry).ToList();
            return arrResult2;
        }
        public int InsertChildData(string[] data,string item2)
        {
            var query = Query.EQ("ServiceCode", item2);
            var vn = _ItemRequest_Service_Repository.FindOne(query);
          
            if(vn.Child == null)
            {
                vn.parent = "Yes";
                vn.Child = data;
                foreach (string a in data)
                {
                    var query1 = Query.EQ("ServiceCode", a);
                    var vn1 = _ItemRequest_Service_Repository.FindOne(query1);
                    if (vn1.Child == null)
                    {
                        vn1.Child = new string[] { item2 };

                    }
                    else
                    {
                        var temp = vn1.Child.ToList();
                        temp.Add(item2);
                        vn1.Child = temp.ToArray();
                    }
                    _ItemRequest_Service_Repository.Add(vn1);
                }
               var f_res = _ItemRequest_Service_Repository.Add(vn);
                return data.Length;

            }
            else
            {
                return 0;
                
            }
     
        }
        public IEnumerable<Prosol_RequestService> get_itemsToApprove(string userid)
        {
            var sortt = SortBy.Descending("Requestedon");
            var query = Query.And(Query.EQ("approver", userid), Query.EQ("RequestStatus", "pending"));
            var approver = _ItemRequest_Service_Repository.FindAll(query, sortt).ToList();

            var query1 = Query.And(Query.Or(Query.EQ("Roles", "Requester"), Query.EQ("Roles", "SuperAdmin"), Query.EQ("Islive", "Active")));
            var user = _ItemRequestService_Users_Repository.FindAll(query1).ToList();
            if (user != null && approver != null)
            {
                foreach (Prosol_Users pr in user)
                {
                    foreach (Prosol_RequestService pp in approver)
                    {
                        if (pp.requester.UserId == pr.Userid)
                        {
                            pp.requester.Name = pr.UserName;
                        }
                    }
                }
                return approver;
            }
            else
            {
                return approver;
            }
         

        }
        public IEnumerable<Prosol_RequestService> getsingle_requested_record(string abcsony)
        {
            var query = Query.EQ("ItemId", abcsony);
            var singlerow = _ItemRequest_Service_Repository.FindAll(query).ToList();
            var query1 = Query.And(Query.Or(Query.EQ("Roles", "Requester"), Query.EQ("Roles", "SuperAdmin"), Query.EQ("Islive", "Active")));
            var user = _ItemRequestService_Users_Repository.FindAll(query1).ToList();
            if (singlerow[0].Cleanser != null)
            {
                var qr = Query.And(Query.EQ("Userid", singlerow[0].Cleanser));
                var clnsr = _ItemRequestService_Users_Repository.FindAll(qr).ToList();
                foreach (Prosol_Users pr1 in user)
                {

                    foreach (Prosol_RequestService pp in singlerow)
                    {
                        if (pp.Cleanser == pr1.Userid)
                        {
                            pp.Cleanser = pr1.UserName;
                        }
                    }
                }
            }
            if (user != null && singlerow != null)
            {
                foreach (Prosol_Users pr1 in user)
                {
                    foreach (Prosol_RequestService pp in singlerow)
                    {
                        if (pp.requester.UserId == pr1.Userid)
                        {
                            pp.requester.Name = pr1.UserName;
                        }
                    }

                    foreach (Prosol_RequestService pp in singlerow)
                    {
                        if (pp.approver == pr1.Userid)
                        {
                            pp.approver = pr1.UserName;
                        }
                    }
                }
            }
            //string reqs = DateTime.Parse(singlerow[0].requester.Name.ToString()).ToString("dd/MM/yyyy");
            string str = DateTime.Parse(singlerow[0].UpdatedOn.ToString()).ToString("dd/MM/yyyy");//.ToLongDateString();
                                                                                                  //                                                                                      // string str1 = str.Substring(0, 6) + " " + str.Substring(str.Length - 4);
                                                                                                  //if (singlerow.Count > 0)
                                                                                                  //   // singlerow[0].approver = str;
                                                                                                  //singlerow[0].requester.Name = reqs;
                                                                                                  //Prosol_RequestService pr = new Prosol_RequestService();

            //var query2 = Query.EQ("Plantcode", singlerow[0].PlantName);
            //var plant2 = _bulkreqplant_repository.FindAll(query2).ToList();
            //if (plant2.Count > 0)
            //    pr.PlantName = plant2[0].Plantname;
            //pr.approver = singlerow[].approver;
            // pr.requester.Name = reqs;
            //singlerow.Add(pr);
          

            return singlerow;

        }
        public IEnumerable<Prosol_RequestService> getappApproved_Records(string userid)
        {
            var sortt = SortBy.Descending("Approvedon");
            var today = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            var frmdate = DateTime.SpecifyKind(today.AddDays(-15), DateTimeKind.Utc);
            var query = Query.And(Query.EQ("approver", userid), Query.LTE("Approvedon", BsonDateTime.Create(today)), Query.GTE("Approvedon", BsonDateTime.Create(frmdate)), Query.NE("ServiceStatus", "pending"), Query.NE("ServiceStatus", "Clarification"), Query.NE("ServiceStatus", "Rejected"), Query.Or(Query.EQ("RequestStatus", "Service approved"), Query.EQ("ServiceStatus", "Cleansed"), Query.EQ("ServiceStatus", "QC"), Query.EQ("ServiceStatus", "Review"), Query.EQ("ServiceStatus", "Release"), Query.EQ("ServiceStatus", "Completed")));
            var approver = _ItemRequest_Service_Repository.FindAll(query, sortt).ToList();
            return approver;

        }
        public IEnumerable<Prosol_RequestService> getreqApproved_Records(string userid)
        {
            var sortt = SortBy.Descending("Approvedon");
            var today = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            var frmdate = DateTime.SpecifyKind(today.AddDays(-15), DateTimeKind.Utc);
            var query = Query.And(Query.EQ("requester.UserId", userid), Query.LTE("Approvedon", BsonDateTime.Create(today)), Query.GTE("Approvedon", BsonDateTime.Create(frmdate)), Query.NE("ServiceStatus", "pending"), Query.NE("ServiceStatus", "Clarification"), Query.NE("ServiceStatus", "Rejected"), Query.Or(Query.EQ("RequestStatus", "Service approved"), Query.EQ("ServiceStatus", "Cleansed"), Query.EQ("ServiceStatus", "QC"), Query.EQ("ServiceStatus", "Review"), Query.EQ("ServiceStatus", "Release"), Query.EQ("ServiceStatus", "Completed")) );
            var approver = _ItemRequest_Service_Repository.FindAll(query, sortt).ToList();
            return approver;

        }
        public IEnumerable<Prosol_RequestService> getRejected_Records(string userid)
        {
            var sortt = SortBy.Descending("Rejectedon");
            var query = Query.And(Query.EQ("approver", userid), Query.EQ("RequestStatus", "Service rejected"));
            var approver = _ItemRequest_Service_Repository.FindAll(query, sortt).ToList();
            return approver;
        }
        public IEnumerable<Prosol_RequestService> getreqRejected_Records(string userid)
        {
            var sortt = SortBy.Descending("Rejectedon");
            var query = Query.And(Query.EQ("requester.UserId", userid), Query.EQ("RequestStatus", "Service rejected"));
            var approver = _ItemRequest_Service_Repository.FindAll(query, sortt).ToList();
            return approver;
        }
        public IEnumerable<Prosol_RequestService> getClarification_Records(string userid)
        {
            var sortt = SortBy.Descending("Clarification_On");
            var query = Query.And(Query.EQ("requester.UserId", userid), Query.EQ("ServiceStatus", "Clarification"));
            var approver = _ItemRequest_Service_Repository.FindAll(query, sortt).ToList();
            return approver;
        }
        public Prosol_Users getcatname(string Userid)
        {
            var query = Query.EQ("Userid", Userid);

            var theresult = _ItemRequestService_Users_Repository.FindOne(query);

            return theresult;
        }
        public bool submit_servcapproval(Prosol_RequestService pro_req, Prosol_UpdatedBy pub, string catname)
        {
            var query = Query.EQ("ItemId", pro_req.ItemId);

            if (pro_req.RequestStatus == "Service approved")
            {
                var Updte = Update.Set("RequestStatus", pro_req.RequestStatus).Set("Approvedon", DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)).Set("Cleanser", pro_req.Cleanser).Set("ServiceStatus", "Cleanse");
                var flg = UpdateFlags.Upsert;

                var theresult = _ItemRequest_Service_Repository.Update(query, Updte, flg);

                return theresult;
            }
            else
            {
                var Updte = Update.Set("RequestStatus", pro_req.RequestStatus).Set("Rejectedon", DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)).Set("Reject_reason", pro_req.Reject_reason);
                var flg = UpdateFlags.Upsert;

                var theresult = _ItemRequest_Service_Repository.Update(query, Updte, flg);
                return theresult;
            }
        }

        public IEnumerable<Prosol_MSAttribute> getAttribute(string Noun, string Modifier)
        {

            var query = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier));
            var vn = _ServiceMasterMSAttRepository.FindAll(query).ToList();
            return vn;
        }
        public virtual string[] AutoSearchNoun(string term)
        {

            var query = Query.Matches("Noun", BsonRegularExpression.Create(new Regex(string.Format("^{0}", term.TrimStart().TrimEnd()), RegexOptions.IgnoreCase)));
            var arrResult = _ServiceMasterMSAttRepository.AutoSearch(query, "Noun");
            return arrResult;


        }
        public virtual Prosol_MSAttribute GetNounDetail(string Noun)
        {
            var query = Query.EQ("Noun", Noun);
            var Nm = _ServiceMasterMSAttRepository.FindOne(query);
            return Nm;
        }
        public virtual IEnumerable<Prosol_MSAttribute> GetModifierList(string Noun)
        {
            var sort = SortBy.Ascending("Modifier");
            var query = Query.EQ("Noun", Noun);
            var arrResult = _ServiceMasterMSAttRepository.FindAll(query, sort).ToList();
            return arrResult;

        }

        public bool deleteRequest(string ItemId)
        {
            var query = Query.EQ("ItemId", ItemId);
            var Updte = Update.Set("RequestStatus", "Deleted");
            var flg = UpdateFlags.Upsert;
            var theResult = _ItemRequest_Service_Repository.Update(query, Updte, flg);
            return theResult;

        }



    }
}
    

    