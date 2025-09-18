using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Prosol.Core.ServiceReference2;
using System.ServiceModel;
using System.Data;

namespace Prosol.Core
{

    public partial class ServiceCreationService : IServiceCreation
    {
      
      
        private readonly IRepository<Prosol_MSAttribute> _ServiceCreateMainSubATTRIRepository;
        private readonly IRepository<Prosol_SSubSubCode> _ServiceCreateSubSubRepository;
        private readonly IRepository<Prosol_RequestService> _ServiceCreateREQSERRepository;
        private readonly IRepository<Prosol_ServiceGroup> _Servicegroup;
        private readonly IRepository<Prosol_ServiceCodeRunningNo> _Service_rno;
        private readonly IRepository<Prosol_Users> _ServiceReviewer;
        private readonly IRepository<Prosol_ServiceCharacteristicValue> _abvRepository;
        private readonly IRepository<Prosol_Abbrevate> _abbreivateRepository;
        private readonly IRepository<Prosol_Attribute> _attributeRepository;
        private readonly IRepository<Prosol_MSAttribute> _CharacteristicRepository;
        private readonly IRepository<Prosol_Servicecodelogic> _codelogicrepository;
        private readonly IRepository<Prosol_UNSPSC> _unspscrepository;
         private readonly IRepository<Prosol_Servicecodelogic> _ServicecodelogicRepository;
        private readonly IRepository<Prosol_ServiceDefaultAttr> _ServiceDefaultatt;
        private readonly IEmailSettings _Emailservc;
        // private readonly IRepository<Prosol_Abbrevate> _abbreivateRepository;
        //  private readonly IRepository<Prosol_MSAttribute> _MSAttrRepository;

        public ServiceCreationService(IRepository<Prosol_MSAttribute> ServiceCreateMainSubATTRIRepository,
                                       IRepository<Prosol_SSubSubCode> ServiceCreateSubSubRepository,
                                      IRepository<Prosol_RequestService> ServiceCreateREQSERRepository,
                                      IRepository<Prosol_ServiceGroup> Servicegroup,
                                      IRepository<Prosol_ServiceCodeRunningNo> Service_rno,
                                      IRepository<Prosol_Users> ServiceReviewer,
                                      IRepository<Prosol_ServiceCharacteristicValue> abvRepository,
                                       IRepository<Prosol_Abbrevate> abbreivateRepository,
                                        IRepository<Prosol_Attribute> attributeRepository,
                                             IRepository<Prosol_MSAttribute> attributesRepository,
                                             IRepository<Prosol_Servicecodelogic> codelogicrepository,
                                             IRepository<Prosol_UNSPSC> unspscrepository,
                                             IRepository<Prosol_Servicecodelogic> ServicecodelogicRepository,
                                              IRepository<Prosol_ServiceDefaultAttr> ServiceDefaultatt, IEmailSettings Emailservc)
                                      //IRepository<Prosol_Attribute> attributeRepository,
                                  //  IRepository<Prosol_Abbrevate> abbreivateRepository,
                                   //   IRepository<Prosol_MSAttribute> MSAttrRepository
        {     
           
                   this._ServiceCreateMainSubATTRIRepository = ServiceCreateMainSubATTRIRepository;
                   this._ServiceCreateSubSubRepository = ServiceCreateSubSubRepository;
                   this._ServiceCreateREQSERRepository = ServiceCreateREQSERRepository;
                   this._Servicegroup = Servicegroup;
                   this._Service_rno = Service_rno;
                   this._ServiceReviewer = ServiceReviewer;
                   this._abvRepository = abvRepository;
                   this._abbreivateRepository = abbreivateRepository;
                   this._attributeRepository = attributeRepository;
                   this._CharacteristicRepository = attributesRepository;
                     this._codelogicrepository = codelogicrepository;
                    this._unspscrepository = unspscrepository;
            this._ServicecodelogicRepository = ServicecodelogicRepository;
            this._Emailservc = Emailservc;
            this._ServiceDefaultatt = ServiceDefaultatt;
            // this._attributeRepository = attributeRepository;
            //  this._abbreivateRepository = abbreivateRepository;
            // this._MSAttrRepository = MSAttrRepository;

        }

        public string getRunningNo (string mc, string sc, string ssc)
        {
            var query = Query.And(Query.EQ("Maincode", mc), Query.EQ("Subcode", sc), Query.EQ("Subsubcode", ssc));
            var sort = SortBy.Ascending("Runningno");
            var rn = _Service_rno.FindAll(query, sort).ToList();

            var values = new Prosol_ServiceCodeRunningNo();
            values.Maincode = mc;
            values.Subcode = sc;
            values.Subsubcode = ssc;
            if (rn.Count > 0)
                values.Runningno = rn[rn.Count - 1].Runningno + 1;
            else
                values.Runningno = 1;



            var res = _Service_rno.Add(values);

            if(rn.Count > 0)
            {
                int rr = rn[rn.Count - 1].Runningno;
                int lenth = rr.ToString().Length;
                //if (lenth == 1)
                //{
                //    if (rr != 9)
                //        return "000" + (++rr).ToString();
                //    else
                //        return "00" + (++rr).ToString();

                //}
                //else if (lenth == 2)
                //{
                //    if (rr != 99)
                //        return "00" + (++rr).ToString();
                //    else
                //        return "0" + (++rr).ToString();
                //}
                //else if (lenth == 3)
                //{
                //    if (rr != 999)
                //        return "0" + (++rr).ToString();
                //    else
                //        return (++rr).ToString();
                //}
                //else
                //{
                //    return (++rr).ToString();
                //}




                if (lenth == 1)
                {
                    if (rr != 9)
                        return "0000" + (++rr).ToString();
                  //  else if(rr != 99)
                       // return "000" + (++rr).ToString();
                    else
                        return "000" + (++rr).ToString();
                }
                else if (lenth == 2)
                {
                    if (rr != 99)
                        return "000" + (++rr).ToString();
                    //else if (rr !=999)
                    //    return "00" + (++rr).ToString();
                    else
                        return "00" + (++rr).ToString();
                }
                else if (lenth == 3)
                {
                    if (rr != 999)
                        return "00" + (++rr).ToString();
                   //else if (rr != 9999)
                   //     return "0" + (++rr).ToString();
                    else
                        return "0" + (++rr).ToString();
           
                }
                else if (lenth == 4)
                {
                    if (rr != 9999)
                        return "0" + (++rr).ToString();
                    else
                        return (++rr).ToString();
                }
                else
                {
                    return (++rr).ToString();
                }

            }
            else
            {
                return "00001";
            }       
       }
        public IEnumerable<Prosol_MSAttribute> GetMainSubAttributeTable(string SubCode)
        {
            var query =  Query.EQ("Activity", SubCode);           
            var vn = _ServiceCreateMainSubATTRIRepository.FindAll(query).ToList();
            return vn;
        }

        public List<Prosol_ServiceGroup> getgroupcodeforcatagory(string catagory)
        {
            var query = Query.EQ("SeviceCategorycode", catagory);
            var result = _Servicegroup.FindAll(query).ToList();
            return result;

        }
        public IEnumerable<Prosol_RequestService> checkDuplicate(string Short, string itemid)
        {
            var qur = Query.And(Query.NE("ItemId", itemid), Query.EQ("ShortDesc", Short));
            var list = _ServiceCreateREQSERRepository.FindAll(qur);
            return list;
        }
        public bool InsertServiceCreation(Prosol_RequestService prs)
        {
            var res = _ServiceCreateREQSERRepository.Add(prs);
           return true;
        }

        //public string getabbr(string ab)
        //{
        //    var query123 = Query.EQ("CharacteristicValue",ab);
        //    var res = _abvRepository.FindAll(query123).ToList();

        //   if(res.Count > 0)
        //    {
        //        return res[0].ValueAbbreviation;
        //    }
        //    else
        //    {
        //        return ab;
        //    }
        //}

        public string getabbr(string ab)
        {
            var query123 = Query.EQ("Value", ab);
            var res = _abbreivateRepository.FindAll(query123).ToList();

            if (res.Count > 0)
            {
                return res[0].Abbrevated;
            }
            else
            {
                return ab;
            }
        }

        public bool updateServiceReview(Prosol_RequestService prs)
        {
            var res = _ServiceCreateREQSERRepository.Add(prs);
            return true;
        }

        public bool updateServiceRelease(Prosol_RequestService prs)
        {
            if (prs.ServiceStatus == "Completed")
            {
               
                    //service
                    BasicHttpBinding binding = new BasicHttpBinding();
                    binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                    binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

                    EndpointAddress endpoint = new EndpointAddress("http://s4hana1909.training.com:8701/sap/bc/srt/rfc/sap/z_servicemaster_com_srv/800/z_servicemaster_com_srv/zser_com");
                    z_servicemaster_com_srvClient client = new z_servicemaster_com_srvClient(binding, endpoint);
                    client.ClientCredentials.UserName.UserName = "TEST4";
                    client.ClientCredentials.UserName.Password = "Vline@123";

                    var x2 = new Z_SERVICEMASTER_CREATE();
                    var servicefields = new ZASMDINPUT();
                    var servicedescfields = new BAPISRV_ASMDT();
                    var servicedescfields1 = new BAPISRV_ASMDT[1];
                    servicefields.VAL_CLASS = prs.Valuationcode;
                    servicefields.BASE_UOM = prs.UomCode;
                    servicefields.SERV_CAT = prs.ServiceCategoryCode;
                    servicedescfields.LANGUAGE = "E";
                    servicedescfields.SHORT_TEXT = prs.ShortDesc;
                    servicedescfields1[0] = servicedescfields;
                    x2.ZSERVICEINPUT = servicefields;
                    x2.ZSERVICE_DESCRIPTION = servicedescfields1;
                    //var SapResponse = client.Z_SERVICEMASTER_CREATE(x2);
                    //var response = SapResponse.ZSERVICE;
                    //prs.ServiceCode = SapResponse.ZSERVICE;
                }

                var res = _ServiceCreateREQSERRepository.Add(prs);
                var query1 = Query.EQ("ItemId", prs.ItemId);
                var rejcde = _ServiceCreateREQSERRepository.FindOne(query1);

                if (rejcde != null)
                {

                    var qryUser = Query.EQ("Userid", rejcde.requester.UserId);
                    var UsrRes = _ServiceReviewer.FindOne(qryUser);
                var tbl = new DataTable();
                tbl.Columns.Add("Item Code");
                tbl.Columns.Add("Service Code");
                tbl.Columns.Add("Legacy");
                tbl.Columns.Add("Short Desc");
                tbl.Columns.Add("Long Desc");
                tbl.Columns.Add("UOM");

                // var slname = _ItemRequestService.getslname(pr1.storage_Location);
                var row = tbl.NewRow();
                row["Item Code"] = prs.ItemId;
                row["Service Code"] = prs.ServiceCode;
                row["Legacy"] = prs.Legacy;
                row["Short Desc"] = prs.ShortDesc;
                row["Long Desc"] = prs.LongDesc;
                row["UOM"] = prs.UomName;
                tbl.Rows.Add(row);
                string subjectt = "Service code created for Request: " + rejcde.ItemId;

                   var x = _Emailservc.sendmail(UsrRes.EmailId, subjectt, _Emailservc.getmailbody(tbl));
                }
        
            return true;
        }
        public Prosol_RequestService getdatalistforupdate(string id)
        {
            var qiuery = Query.EQ("_id", new ObjectId(id));
            var res = _ServiceCreateREQSERRepository.FindOne(qiuery);
            return res;
        }


        

              public bool rejectitem(string id, Prosol_UpdatedBy pu,string rejectedas,string Remarks)
        {
            var qiuery = Query.EQ("_id", new ObjectId(id));
            var res = _ServiceCreateREQSERRepository.FindOne(qiuery);

            res.Last_updatedBy = pu;

            Prosol_Reject_Service prs = new Prosol_Reject_Service();
            prs.UserId = pu.UserId;
            prs.Name = pu.Name;
            prs.UpdatedOn = DateTime.Now;
            prs.Reject_reason = Remarks;
            prs.Rejected_as = rejectedas;
            res.RejectedBy = prs;
            res.ServiceStatus = "Rejected";
            res.Remarks = Remarks;

            var res1 = _ServiceCreateREQSERRepository.Add(res);

            return res1;
        }
        //public bool GetCodeForclarifiyItems(Prosol_RequestService rs,string itemid,string remark, string userid, string username)
        //{
        //    var query = Query.EQ("ItemId", itemid);
        //    var vn = _ServiceCreateREQSERRepository.FindAll(query).ToList();
        // var q = Query.EQ("ItemId", vn[0].ItemId);
        //    var up = Update.Set("Reject_reason", remark).Set("ServiceStatus", "Clarification").Set("Clarification_On", DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));
        //    var flg = UpdateFlags.Upsert;
        //    _ServiceCreateREQSERRepository.Update(q, up, flg);

        //    var query1 = Query.EQ("ItemId", vn[0].ItemId);
        //    var rejcde = _ServiceCreateREQSERRepository.FindAll(query1).ToList();
        //    var qryUser = Query.EQ("Userid", rejcde[0].requester.UserId);
        //    var UsrRes = _ServiceReviewer.FindOne(qryUser);
        //    string subjectt = "Item Need Clarification";
        //    string body = "<html><body><table><tr><td style='padding-left: 50px;padding-top: 10px;'>Request Id</td><td style='padding-left: 50px;padding-top: 10px;'>" + vn[0].ItemId + "</td></tr><tr><td style='padding-left: 50px;padding-top: 10px;'>Clarification remarks</td><td style='padding-left: 50px;padding-top: 10px;'>" + remark + "</td></tr></table></body></html>";

        //    var x = _Emailservc.sendmail(UsrRes.EmailId, subjectt, body);
        //  return true;
        //}


        //public bool GetCodeForclarifiyItems(Prosol_RequestService prs)
        //{
        //    var res = _ServiceCreateREQSERRepository.Add(prs);
        //    return true;
        //}
        //public bool GetCodeForclarifiyItems1(Prosol_RequestService rs, string itemid, string remark, string userid, string username)
        //{
        //    var query = Query.EQ("ItemId", itemid);
        //    var vn = _ServiceCreateREQSERRepository.FindAll(query).ToList();
        //    var q = Query.EQ("ItemId", vn[0].ItemId);
        //    var up = Update.Set("Reject_reason", remark).Set("ServiceStatus", "Clarification").Set("Clarification_On", DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));
        //    var flg = UpdateFlags.Upsert;
        //    _ServiceCreateREQSERRepository.Update(q, up, flg);

        //    var query1 = Query.EQ("ItemId", vn[0].ItemId);
        //    var rejcde = _ServiceCreateREQSERRepository.FindAll(query1).ToList();
        //    var qryUser = Query.EQ("Userid", rejcde[0].requester.UserId);
        //    var UsrRes = _ServiceReviewer.FindOne(qryUser);
        //    string subjectt = "Item Need Clarification";
        //    string body = "<html><body><table><tr><td style='padding-left: 50px;padding-top: 10px;'>Request Id</td><td style='padding-left: 50px;padding-top: 10px;'>" + vn[0].ItemId + "</td></tr><tr><td style='padding-left: 50px;padding-top: 10px;'>Clarification remarks</td><td style='padding-left: 50px;padding-top: 10px;'>" + remark + "</td></tr></table></body></html>";

        //    var x = _Emailservc.sendmail(UsrRes.EmailId, subjectt, body);
        //    return true;
        //}
        //public bool GetCodeForclarifiyItems2(Prosol_RequestService rs, string itemid, string remark, string userid, string username)
        //{
        //    var query = Query.EQ("ItemId", itemid);
        //    var vn = _ServiceCreateREQSERRepository.FindAll(query).ToList();
        //    var q = Query.EQ("ItemId", vn[0].ItemId);
        //    var up = Update.Set("Reject_reason", remark).Set("ServiceStatus", "Clarification").Set("Clarification_On", DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));
        //    var flg = UpdateFlags.Upsert;
        //    _ServiceCreateREQSERRepository.Update(q, up, flg);

        //    var query1 = Query.EQ("ItemId", vn[0].ItemId);
        //    var rejcde = _ServiceCreateREQSERRepository.FindAll(query1).ToList();
        //    var qryUser = Query.EQ("Userid", rejcde[0].requester.UserId);
        //    var UsrRes = _ServiceReviewer.FindOne(qryUser);
        //    string subjectt = "Item Need Clarification";
        //    string body = "<html><body><table><tr><td style='padding-left: 50px;padding-top: 10px;'>Request Id</td><td style='padding-left: 50px;padding-top: 10px;'>" + vn[0].ItemId + "</td></tr><tr><td style='padding-left: 50px;padding-top: 10px;'>Clarification remarks</td><td style='padding-left: 50px;padding-top: 10px;'>" + remark + "</td></tr></table></body></html>";

        //    var x = _Emailservc.sendmail(UsrRes.EmailId, subjectt, body);
        //    return true;
        //}
        public bool senttoReview(string _id, string Remarks)
        {
            var qiuery = Query.EQ("_id", new ObjectId(_id));
            var res = _ServiceCreateREQSERRepository.FindOne(qiuery);
            res.ServiceStatus = "Review";
            res.RequestStatus = "Review";
            if (Remarks != null)
            {
                res.Remarks = Remarks;
            }
            else
            {
                res.Remarks = "";
            }
            var res1 = _ServiceCreateREQSERRepository.Add(res);
            return res1;
        }

        
             public bool senttocleanser(string _id,string Remarks)
        {
            var qiuery = Query.EQ("_id", new ObjectId(_id));
            var res = _ServiceCreateREQSERRepository.FindOne(qiuery);
            res.ServiceStatus = "Cleanse";
            res.RequestStatus = "Cleanse";
            if (Remarks != null)
            {
                res.Remarks = Remarks;
            }
            else
            {
                res.Remarks = "";
            }
            var res1 = _ServiceCreateREQSERRepository.Add(res);
            return res1;
        }

        public IEnumerable<Prosol_Users> LoadReviewer()
        {
            //var query = Query.And(Query.EQ("Usertype", "Approver"), Query.EQ("Islive", "Active"));
            //var approver = _ServiceReviewer.FindAll(query).ToList();
            //return approver;

            //////////
            var query = Query.And(Query.EQ("Roles.Name", "Reviewer"), Query.EQ("Islive", "Active"));
            // string[] arr = { "Cataloguer" };
            // var query = Query.In("Usertype", new BsonArray(arr));
            var res = _ServiceReviewer.FindAll(query).ToList();
            return res;




        }
        
            public IEnumerable<Prosol_Users> LoadReleaser()
        {
            var query = Query.And(Query.EQ("Roles.Name", "Releaser"), Query.EQ("Islive", "Active"));
            var approver = _ServiceReviewer.FindAll(query).ToList();
            return approver;

            /////


        }
        public IEnumerable<Prosol_SSubSubCode> GetSubSubList(string MainCode,string SubCode)
        {

            var query = Query.And(Query.EQ("MainCode", MainCode), Query.EQ("SubCode", SubCode));
            var vn = _ServiceCreateSubSubRepository.FindAll(query).ToList();
            return vn;
        }

        //defaultattribute

        public IEnumerable<Prosol_ServiceDefaultAttr> Defaultattribute()
        {

          
            var vn = _ServiceDefaultatt.FindAll().ToList();
            return vn;
        }
        public IEnumerable<Prosol_RequestService> getdatalistforCleanser(string Cleanser)
        {
           var query = Query.And(Query.EQ("Cleanser", Cleanser), Query.Or(Query.EQ("ServiceStatus", "Cleanse"),Query.EQ("RequestStatus", "Clarification"), Query.EQ("ServiceStatus", "Cleansed")));

            //var query = Query.And(Query.EQ("ServiceStatus", "Cleanse"), Query.EQ("Cleanser", Cleanser));

            var result = _ServiceCreateREQSERRepository.FindAll(query).ToList();
            return result;
        }

        public IEnumerable<Prosol_RequestService> getdatalistforReviewer(string Reviewer)
        {

            var query = Query.And(Query.EQ("Reviewer.UserId", Reviewer), Query.Or(Query.EQ("ServiceStatus", "Review"), Query.EQ("ServiceStatus", "QC")));

            // var query = Query.And(Query.EQ("ServiceStatus", "Review"), Query.EQ("Reviewer.UserId", Reviewer));

            var result = _ServiceCreateREQSERRepository.FindAll(query).ToList();
            return result;
        }

        public IEnumerable<Prosol_RequestService> getdatalistforReleaser(string Releaser)
        {

            var query = Query.And(Query.EQ("Releaser.UserId", Releaser), Query.Or(Query.EQ("ServiceStatus", "Release"), Query.EQ("ServiceStatus", "QA")));
            //var query = Query.And(Query.EQ("ServiceStatus", "Release"), Query.EQ("Releaser.UserId", Releaser));

            var result = _ServiceCreateREQSERRepository.FindAll(query).ToList();
            return result;
        }


        //public IEnumerable<Prosol_ServiceActivity> getActivitySC(string ServiceGroupcode)
        //{

        //    var query = Query.And(Query.EQ("Islive", true), Query.EQ("ServiceGroupcode", ServiceGroupcode));
        //    var vn = _ServiceCreateACTRepository.FindAll(query).ToList();
        //    return vn;
        //}

        //checkvalue

        public string CheckValue(string Noun, string Modifier, string Attribute, string Value)
        {
            string A = "true";
            string B = "false";
            var Qry = Query.EQ("Attribute", Attribute);
            var AttributeMaster = _attributeRepository.FindOne(Qry);
            if (AttributeMaster != null && AttributeMaster.ValueList != null && AttributeMaster.ValueList.Length > 0)
            {
                return A;
            }
            else
            {
                var query = Query.EQ("Value", Value);
                var obj = _abbreivateRepository.FindOne(query);
                if (obj != null && obj.Abbrevated != "" )
                {
                    //int flg = 0;
                    //string[] sAr = { obj._id.ToString() };
                    ////var Qry = Query.And(Query.EQ("Attribute", Attribute), Query.In("ValueList", new BsonArray(sAr)));
                    ////var AttributeMaster = _attributeRepository.FindOne(Qry);
                    ////if (AttributeMaster != null)
                    ////{
                    ////    flg = 1;

                    ////}
                    //var Qry1 = Query.And(Query.EQ("MainCode", MainCode), Query.EQ("SubCode", SubCode), Query.EQ("Attributes", Attribute), Query.In("Values", new BsonArray(sAr)));
                    //var TemplateMaster = _CharacteristicRepository.FindOne(Qry1);
                    //if (TemplateMaster != null)
                    //{
                    //    flg = 1;
                    //}
                    //if (flg == 0)
                    //    return false;
                    //else return true;
                    return obj.Abbrevated;

                }
                //else return false;
                else return B;
            }


        }

        //addvalue
        public bool AddValue1(string Noun, string Modifier, string Attribute, string Value, string abb)
        {
            var query = Query.EQ("Value", Value);
            var obj = _abbreivateRepository.FindOne(query);
            if (obj != null )
            {

                string[] sAr = { obj._id.ToString() };
                //var Qry = Query.EQ("Attribute", Attribute);
                //var AttributeMaster = _attributeRepository.FindOne(Qry);
                //if (AttributeMaster != null && AttributeMaster.ValueList != null && AttributeMaster.ValueList.Length > 0)
                //{
                //    var lstobj = AttributeMaster.ValueList.ToList();
                //    lstobj.Add(obj._id.ToString());
                //    AttributeMaster.ValueList = lstobj.ToArray();
                //    _attributeRepository.Add(AttributeMaster);

                //}
                var Qry1 = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.EQ("Attributes", Attribute));
                var TemplateMaster = _CharacteristicRepository.FindOne(Qry1);
                if (TemplateMaster != null && TemplateMaster.Values != null)
                {
                    var lstobj = TemplateMaster.Values.ToList();
                    lstobj.Add(obj._id.ToString());
                    TemplateMaster.Values = lstobj.ToArray();
                    _CharacteristicRepository.Add(TemplateMaster);
                }
                else
                {
                    var str = new string[1];
                    str[0] = obj._id.ToString();
                    TemplateMaster.Values = str;
                    _CharacteristicRepository.Add(TemplateMaster);

                }
                if (obj.Abbrevated == "" || obj.Abbrevated == null)
                {
                    obj.Abbrevated = abb.ToUpper();
                    obj.Approved = "Yes";
                    _abbreivateRepository.Add(obj);
                }

            }
            else
            {
                var newobj = new Prosol_Abbrevate();
                if (abb == null || abb == "undefined")
                {
                    newobj.Abbrevated = null;
                }
                else newobj.Abbrevated = abb.ToUpper();
                newobj.Value = Value;
                newobj.Approved = "Yes";
                _abbreivateRepository.Add(newobj);


                query = Query.EQ("Value", Value);
                obj = _abbreivateRepository.FindOne(query);
                if (obj != null)
                {

                    string[] sAr = { obj._id.ToString() };
                    //var Qry = Query.EQ("Attribute", Attribute);
                    //var AttributeMaster = _attributeRepository.FindOne(Qry);
                    //if (AttributeMaster != null && AttributeMaster.ValueList != null && AttributeMaster.ValueList.Length>0)
                    //{
                    //    var lstobj = AttributeMaster.ValueList.ToList();
                    //    lstobj.Add(obj._id.ToString());
                    //    AttributeMaster.ValueList = lstobj.ToArray();
                    //    _attributeRepository.Add(AttributeMaster);

                    //}
                    var Qry1 = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.EQ("Attributes", Attribute));
                    var TemplateMaster = _CharacteristicRepository.FindOne(Qry1);
                    if (TemplateMaster != null && TemplateMaster.Values != null)
                    {
                        var lstobj = TemplateMaster.Values.ToList();
                        lstobj.Add(obj._id.ToString());
                        TemplateMaster.Values = lstobj.ToArray();
                        _CharacteristicRepository.Add(TemplateMaster);
                    }
                    else
                    {
                        if(TemplateMaster !=null)
                        { 
                        var str = new string[1];
                        str[0] = obj._id.ToString();
                        TemplateMaster.Values = str;
                        _CharacteristicRepository.Add(TemplateMaster);
                        }
                    }
                }

            }
            return true;


        }
        public IEnumerable<Prosol_UNSPSC> getCOMMList(string sKey)
        {

            string[] str = { "Class", "ClassTitle", "Commodity", "CommodityTitle"};
            var fields = Fields.Include(str);
            var qry1 = Query.Or(Query.Matches("ClassTitle", BsonRegularExpression.Create(new Regex(sKey.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));

            var Result = _unspscrepository.FindAll(fields, qry1).ToList();
            return Result;
        }
        public IEnumerable<Prosol_UNSPSC> getCOMMCOMMList(string sKey)
        {

            string[] str = { "Class", "ClassTitle", "Commodity", "CommodityTitle" };
            var fields = Fields.Include(str);
            var qry1 = Query.Or(Query.Matches("CommodityTitle", BsonRegularExpression.Create(new Regex(sKey.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));

            var Result = _unspscrepository.FindAll(fields, qry1).ToList();
            return Result;
        }

        public bool codesaveforlogic(Prosol_Servicecodelogic data)
        {
            bool res = false;


            var res1 = _codelogicrepository.FindAll().ToList();
            if (res1.Count() > 0)
            {
                data._id = res1[0]._id;
                res = _codelogicrepository.Add(data);
            }
            else
            {
                var query = Query.EQ("SERVICECODELOGIC", data.SERVICECODELOGIC);
                var vn = _codelogicrepository.FindAll(query).ToList();
                if (vn.Count == 0)
                {
                    res = _codelogicrepository.Add(data);
                }
            }
            return res;
        }
        public string[] loadversionforservice()
        {
            return _unspscrepository.AutoSearch1("Version");
            //  retu
        }
        public Prosol_Servicecodelogic Showdataservice()
        {
            //var sort = SortBy.Descending("UpdatedOn");
            var shwusr = _codelogicrepository.FindOne();
            return shwusr;
        }


        public Prosol_Servicecodelogic Showdatacodelogic()
        {
            //var sort = SortBy.Descending("UpdatedOn");
            var shwusr = _ServicecodelogicRepository.FindOne();
            return shwusr;
        }
        public string getclchk()
        {
            var res = _ServicecodelogicRepository.FindAll().ToList();
            return res[0].SERVICECODELOGIC;
        }
        public string generateServicecodeunspsc(string LogicCode)
        {
            string code = "";
            var sort = SortBy.Descending("_id");
            var query = Query.Matches("ServiceCode", BsonRegularExpression.Create(new Regex(LogicCode)));
            var ServiceCode = _ServiceCreateREQSERRepository.FindAll(query, sort).ToList();
            if (ServiceCode != null && ServiceCode.Count > 0)
            {
                code = ServiceCode[0].ServiceCode;
            }
            return code;

        }
        //public string generateServicecodeunspsc(string SubCode)
        //{
        //    // string code = "";
        //    var query = Query.Matches("ServiceCode", BsonRegularExpression.Create(new Regex(string.Format("^{0}", SubCode.TrimStart().TrimEnd()), RegexOptions.IgnoreCase)));
        //    var rn = _ServiceCreateREQSERRepository.FindAll(query).ToList();


        //    if (rn != null && rn.Count > 0)
        //    {



        //        //if (rn.Count > 0)
        //        //{
        //        int rr = rn.Count;
        //        //  int lenth = rn.Count.ToString().Length;
        //        if (rn.Count.ToString().Length == 1)
        //        {
        //            if (rr != 9)
        //                return SubCode + "00000" + (++rr).ToString();
        //            else
        //                return SubCode + "0000" + (++rr).ToString();
        //        }
        //        else if (rn.Count.ToString().Length == 2)
        //        {
        //            if (rr != 99)
        //                return SubCode + "0000" + (++rr).ToString();
        //            else
        //                return SubCode + "000" + (++rr).ToString();
        //        }
        //        else if (rn.Count.ToString().Length == 3)
        //        {
        //            if (rr != 999)
        //                return SubCode + "000" + (++rr).ToString();
        //            else
        //                return SubCode + "00" + (++rr).ToString();
        //        }
        //        else if (rn.Count.ToString().Length == 4)
        //        {
        //            if (rr != 9999)
        //                return SubCode + "00" + (++rr).ToString();
        //            else
        //                return SubCode + "0" + (++rr).ToString();
        //        }
        //        else if (rn.Count.ToString().Length == 5)
        //        {
        //            if (rr != 99999)
        //                return SubCode + "0" + (++rr).ToString();
        //            else
        //                return SubCode + (++rr).ToString();
        //        }
        //        else
        //        {
        //            return SubCode + (++rr).ToString();
        //        }
        //    }
        //    else
        //    {
        //        return SubCode + "000001";
        //    }
        //}

        public List<Prosol_RequestService> searchMaster1(string sCode, string iCode,string sSource, string sShort, string sLong, string sCategory, string sGroup, string sUser, string sStatus)
        {
            string[] strArr = { "ItemId", "ServiceCode", "Legacy", "ServiceCategoryName", "ServiceGroupName", "ShortDesc", "LongDesc", "ServiceStatus", "requester", "Cleanserr", "Reviewer", "Releaser", "Rework", "Reworkcat"};
            List<searchObj> LstObj = new List<searchObj>();

            searchObj sObj = new searchObj();
            sObj.SearchColumn = "ItemId";
            sObj.SearchKey = sCode;

            LstObj.Add(sObj);

            sObj = new searchObj();
            sObj.SearchColumn = "ServiceCode";
            sObj.SearchKey = iCode;

            LstObj.Add(sObj);

            sObj = new searchObj();
            sObj.SearchColumn = "Legacy";
            sObj.SearchKey = sSource;

            LstObj.Add(sObj);

            sObj = new searchObj();
            sObj.SearchColumn = "ShortDesc";
            sObj.SearchKey = sShort;

            LstObj.Add(sObj);
            sObj = new searchObj();
            sObj.SearchColumn = "LongDesc";
            sObj.SearchKey = sLong;

            LstObj.Add(sObj);

            sObj = new searchObj();
            sObj.SearchColumn = "ServiceCategoryName";
            sObj.SearchKey = sCategory;

            LstObj.Add(sObj);

            sObj = new searchObj();
            sObj.SearchColumn = "ServiceGroupName";
            sObj.SearchKey = sGroup;

            LstObj.Add(sObj);

            sObj = new searchObj();
            sObj.SearchColumn = "sUser";
            sObj.SearchKey = sUser;
            LstObj.Add(sObj);



            LstObj.Add(sObj);
            sObj = new searchObj();
            sObj.SearchColumn = "sStatus";
            sObj.SearchKey = sStatus;
            LstObj.Add(sObj);

            List<Prosol_RequestService> newResultList = new List<Prosol_RequestService>();
            //  List<Prosol_Datamaster> RemoveResultList = new List<Prosol_Datamaster>();
            int flg = 0;
            foreach (searchObj mdl in LstObj)
            {
                if (mdl.SearchKey != "" && mdl.SearchKey != null && mdl.SearchKey != "null" && mdl.SearchKey != "undefined")
                {
                    if (flg == 0)
                    {
                        List<Prosol_RequestService> sResult = new List<Prosol_RequestService>();
                        if (mdl.SearchColumn == "sUser")
                        {
                            sResult = UserSearch(strArr, mdl.SearchKey);

                        }
                        else if (mdl.SearchColumn == "sStatus")
                        {
                            sResult = StatusSearch(strArr, mdl.SearchKey);
                        }
                        else if(mdl.SearchColumn == "ServiceCode")
                        {
                            sResult = SearchFncode(strArr, mdl.SearchColumn, mdl.SearchKey);
                        }
                        else
                        {
                            sResult = SearchFn(strArr, mdl.SearchColumn, mdl.SearchKey);
                        }
                        if (sResult != null && sResult.Count > 0)
                        {

                            foreach (Prosol_RequestService pmdl in sResult)
                            {
                                newResultList.Add(pmdl);
                                flg = 1;
                                //  RemoveResultList.Add(pmdl);
                            }

                        }
                        else break;
                    }
                    else
                    {
                        //  var sResult = SearchFn(strArr, mdl.SearchColumn, mdl.SearchKey);
                        if (mdl.SearchKey.Contains("*"))
                        {
                            string[] splt = mdl.SearchKey.Split('*');
                            if (splt.Length > 2)
                            {
                                //contains                              
                                foreach (string str in splt)
                                {
                                    if (str != "")
                                    {
                                        int flgg = 0;
                                        var query1 = new List<Prosol_RequestService>();
                                        if (mdl.SearchColumn == "ItemId")
                                        {
                                            if (flgg == 0)
                                            {
                                                query1 = (from x in newResultList where x.ItemId.Contains(str) select x).ToList();
                                            }
                                            if (flgg == 1)
                                            {
                                                query1 = query1.Where(x => x.ItemId.Contains(str)).ToList();

                                            }
                                        }
                                        if (mdl.SearchColumn == "ServiceCode")
                                        {
                                            if (flgg == 0)
                                            {
                                                query1 = (from x in newResultList where x.ServiceCode.Contains(str) select x).ToList();
                                            }
                                            if (flgg == 1)
                                            {
                                                query1 = query1.Where(x => x.ServiceCode.Contains(str)).ToList();

                                            }
                                        }
                                        if (mdl.SearchColumn == "Legacy")
                                        {
                                            if (flgg == 0)
                                            {
                                                query1 = (from x in newResultList where x.Legacy.Contains(str) select x).ToList();
                                            }
                                            if (flgg == 1)
                                            {
                                                query1 = query1.Where(x => x.Legacy.Contains(str)).ToList();

                                            }

                                            // newResultList = (from x in newResultList where x.Legacy.Contains(str) select x).ToList();
                                        }
                                        if (mdl.SearchColumn == "ShortDesc")
                                        {
                                            if (flgg == 0)
                                            {
                                                query1 = (from x in newResultList where x.ShortDesc.Contains(str) select x).ToList();
                                            }
                                            if (flgg == 1)
                                            {
                                                query1 = query1.Where(x => x.ShortDesc.Contains(str)).ToList();

                                            }
                                            // newResultList = (from x in newResultList where x.Shortdesc.Contains(str) select x).ToList();
                                        }
                                        if (mdl.SearchColumn == "LongDesc")
                                        {
                                            if (flgg == 0)
                                            {
                                                query1 = (from x in newResultList where x.LongDesc.Contains(str) select x).ToList();
                                            }
                                            if (flgg == 1)
                                            {
                                                query1 = query1.Where(x => x.LongDesc.Contains(str)).ToList();

                                            }
                                            // newResultList = (from x in newResultList where x.Longdesc.Contains(str) select x).ToList();
                                        }
                                        newResultList = query1;
                                        flgg = 1;
                                    }
                                }

                            }
                            else
                            {
                                if (mdl.SearchKey != "" && mdl.SearchKey != null && mdl.SearchKey != "null" && mdl.SearchKey != "undefined")
                                {
                                    if (sCode.IndexOf('*') > 0)
                                    {

                                        //Start with



                                        if (mdl.SearchColumn == "ItemId")
                                            newResultList = (from x in newResultList where x.ItemId.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();
                                        if (mdl.SearchColumn == "ServiceCode")
                                            newResultList = (from x in newResultList where x.ServiceCode.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();
                                        if (mdl.SearchColumn == "Legacy")
                                            newResultList = (from x in newResultList where x.Legacy.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();
                                        if (mdl.SearchColumn == "ShortDesc")
                                            newResultList = (from x in newResultList where x.ShortDesc.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();
                                        if (mdl.SearchColumn == "LongDesc")
                                            newResultList = (from x in newResultList where x.LongDesc.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();

                                    }
                                    else
                                    {
                                        //End with



                                        if (mdl.SearchColumn == "ItemId")
                                            newResultList = (from x in newResultList where x.ItemId.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();
                                        if (mdl.SearchColumn == "ServiceCode")
                                            newResultList = (from x in newResultList where x.ServiceCode.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();
                                        if (mdl.SearchColumn == "Legacy")
                                            newResultList = (from x in newResultList where x.Legacy.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();
                                        if (mdl.SearchColumn == "ShortDesc")
                                            newResultList = (from x in newResultList where x.ShortDesc.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();
                                        if (mdl.SearchColumn == "LongDesc")
                                            newResultList = (from x in newResultList where x.LongDesc.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();


                                    }
                                }

                            }

                        }
                        else
                        {
                            if (mdl.SearchKey != "" && mdl.SearchKey != null && mdl.SearchKey != "null" && mdl.SearchKey != "undefined")
                            {


                                if (mdl.SearchColumn == "ItemId")
                                    newResultList = (from x in newResultList where x.ItemId != null && x.ItemId.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "ServiceCode")
                                    newResultList = (from x in newResultList where x.ServiceCode != null && x.ServiceCode.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "Legacy")
                                    newResultList = (from x in newResultList where x.Legacy != null && x.Legacy.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "ShortDesc")
                                    newResultList = (from x in newResultList where x.ShortDesc != null && x.ShortDesc.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "LongDesc")
                                    newResultList = (from x in newResultList where x.LongDesc != null && x.LongDesc.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "ServiceCategoryName")
                                    newResultList = (from x in newResultList where x.ServiceCategoryName != null && x.ServiceCategoryName.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "ServiceGroupName")
                                    newResultList = (from x in newResultList where x.ServiceGroupName != null && x.ServiceGroupName.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "sUser")
                                    newResultList = (from x in newResultList where x.Cleanserr != null && (x.Cleanserr.Name.Equals(mdl.SearchKey) || (x.Reviewer != null && x.Reviewer.Name.Equals(mdl.SearchKey)) || (x.Releaser != null && x.Releaser.Name.Equals(mdl.SearchKey))) select x).ToList();
 
                                if (mdl.SearchColumn == "sStatus")
                                {
                                    string sta = "", sta1 = "";
                                    if (sStatus == "Catalogue")
                                    {
                                        sta = "Cleanse"; sta1 = "Cleansed";
                                    }
                                    //else if (sStatus == "Clarification")
                                    //{
                                    //    sta = -1; sta1 = -1;
                                    //}
                                    else if (sStatus == "QC")
                                    {
                                        sta = "Review"; sta1 = "QC";
                                    }
                                    else if (sStatus == "QA")
                                    {
                                        sta = "Release"; sta1 = "QA";
                                    }
                                    else
                                    {
                                        sta = "Completed"; sta1 = "Completed";
                                    }
                                    //if (sStatus == "Catalogue Rework")
                                    //{
                                    //    newResultList = (from x in newResultList where (x.Reworkcat != null && (x.ItemStatus == 0 || x.ItemStatus == 1)) select x).ToList();
                                    //}
                                    //else if (sStatus == "QC Rework")
                                    //{
                                    //    newResultList = (from x in newResultList where (x.Rework != null && (x.ItemStatus == 2 || x.ItemStatus == 3)) select x).ToList();
                                    //}
                                    //else
                                    {
                                        newResultList = (from x in newResultList where (x.ServiceStatus == sta || x.ServiceStatus == sta1) select x).ToList();
                                    }

                                }
                            }

                        }
                    }
                }
            }
            return newResultList;
        }
        public string[] showall_user()

        {
            var res = _ServiceReviewer.AutoSearch1("UserName");
            return res;


        }

        public virtual IEnumerable<Prosol_ServiceGroup> GetcatList()
        {
            var sort = SortBy.Ascending("SeviceCategoryname");
            var arrResult = _Servicegroup.FindAll(sort).ToList();
            return arrResult;

        }

        private List<Prosol_RequestService> SearchFn(string[] strArr, string ColumnName, string sCode)
        {
            var fields = Fields.Include(strArr);
            if (sCode.Contains('*'))
            {


                var QryLst = new List<IMongoQuery>();
                var QryLst1 = new List<IMongoQuery>();
                string[] sepArr = sCode.Split('*');
                if (sepArr.Length > 2)
                {
                    foreach (string str in sepArr)
                    {
                        if (str != "")
                        {
                            var Qry1 = Query.Matches(ColumnName, BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd(), RegexOptions.IgnoreCase)));
                            QryLst.Add(Qry1);

                        }
                    }
                }
                else
                {
                    foreach (string str in sepArr)
                    {
                        if (str != "")
                        {
                            if (sCode.IndexOf('*') > 0)
                            {
                                //Start with
                                var Qry1 = Query.Matches(ColumnName, BsonRegularExpression.Create(new Regex(string.Format("^{0}", str.TrimStart().TrimEnd()), RegexOptions.IgnoreCase)));
                                QryLst.Add(Qry1);

                            }
                            else
                            {
                                //End with

                                var Qry1 = Query.Matches(ColumnName, BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd() + "$", RegexOptions.IgnoreCase)));
                                QryLst.Add(Qry1);


                            }

                        }
                    }


                }
                var query = Query.Or(QryLst);
                var arrResult = _ServiceCreateREQSERRepository.FindAll(fields, query).ToList();
                return arrResult;
            }
            else
            {
                var Qry1 = Query.EQ(ColumnName, sCode.TrimStart().TrimEnd());
                var arrResult = _ServiceCreateREQSERRepository.FindAll(fields, Qry1).ToList();
                return arrResult;

            }

        }
        //code

        private List<Prosol_RequestService> SearchFncode(string[] strArr, string ColumnName, string iCode)
        {
            var fields = Fields.Include(strArr);
            if (iCode.Contains('*'))
            {


                var QryLst = new List<IMongoQuery>();
                var QryLst1 = new List<IMongoQuery>();
                string[] sepArr = iCode.Split('*');
                if (sepArr.Length > 2)
                {
                    foreach (string str in sepArr)
                    {
                        if (str != "")
                        {
                            var Qry1 = Query.Matches(ColumnName, BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd(), RegexOptions.IgnoreCase)));
                            QryLst.Add(Qry1);

                        }
                    }
                }
                else
                {
                    foreach (string str in sepArr)
                    {
                        if (str != "")
                        {
                            if (iCode.IndexOf('*') > 0)
                            {
                                //Start with
                                var Qry1 = Query.Matches(ColumnName, BsonRegularExpression.Create(new Regex(string.Format("^{0}", str.TrimStart().TrimEnd()), RegexOptions.IgnoreCase)));
                                QryLst.Add(Qry1);

                            }
                            else
                            {
                                //End with

                                var Qry1 = Query.Matches(ColumnName, BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd() + "$", RegexOptions.IgnoreCase)));
                                QryLst.Add(Qry1);


                            }

                        }
                    }


                }
                var query = Query.Or(QryLst);
                var arrResult = _ServiceCreateREQSERRepository.FindAll(fields, query).ToList();
                return arrResult;
            }
            else
            {
                var Qry1 = Query.EQ(ColumnName, iCode.TrimStart().TrimEnd());
                var arrResult = _ServiceCreateREQSERRepository.FindAll(fields, Qry1).ToList();
                return arrResult;

            }

        }
        //csi
        //csi
        public Prosol_RequestService GetSingleItemser(string ItemId)
        {

            var query = Query.EQ("ItemId", ItemId);
            var arrResult = _ServiceCreateREQSERRepository.FindOne(query);
            return arrResult;

        }
        private List<Prosol_RequestService> UserSearch(string[] strArr, string sUser)
        {
            var fields = Fields.Include(strArr);
            var Qry1 = Query.Or(Query.EQ("Cleanserr.Name", sUser), Query.EQ("Reviewer.Name", sUser), Query.EQ("Releaser.Name", sUser));
            var arrResult = _ServiceCreateREQSERRepository.FindAll(fields, Qry1).ToList();
            return arrResult;
        }
        private List<Prosol_RequestService> StatusSearch(string[] strArr, string sStatus)
        {
            string sta = "", sta1 = "";
            if (sStatus == "Catalogue")
            {
                sta = "Cleanse"; sta1 = "Cleansed";
            }
            //else if (sStatus == "Clarification")
            //{
            //    sta = -1; sta1 = -1;
            //}
            else if (sStatus == "QC")
            {
                sta = "Review"; sta1 = "QC";
            }
            else if (sStatus == "QA")
            {
                sta = "Release"; sta1 = "QA";
            }
            else
            {
                sta = "Completed"; sta1 = "Completed";
            }
            var fields = Fields.Include(strArr);
            var Qry1 = Query.Or(Query.EQ("ServiceStatus", sta), Query.EQ("ServiceStatus", sta1));
            //if (sStatus == "Catalogue Rework")

            //{
            //    Qry1 = Query.And(Query.NE("Reworkcat", BsonNull.Value), Query.Or(Query.EQ("ItemStatus", 0), Query.EQ("ItemStatus", 1)));

            //}
            //else if (sStatus == "QC Rework")

            //{
            //    Qry1 = Query.And(Query.NE("Rework", BsonNull.Value), Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)));

            //}

            var arrResult = _ServiceCreateREQSERRepository.FindAll(fields, Qry1).ToList();
            return arrResult;

        }


        //public string generateServicecodeitem(string itemcode)
        // {
        //     // string code = "";
        //     var query = Query.Matches("ServiceCode", BsonRegularExpression.Create(new Regex(string.Format("^{0}", itemcode.TrimStart().TrimEnd()), RegexOptions.IgnoreCase)));
        //     var rn = _ServiceCreateREQSERRepository.FindAll(query).ToList();


        //     if (rn != null && rn.Count > 0)
        //     {



        //         //if (rn.Count > 0)
        //         //{
        //         int rr = rn.Count;
        //         //  int lenth = rn.Count.ToString().Length;
        //         if (rn.Count.ToString().Length == 1)
        //         {
        //             if (rr != 9)
        //                 return itemcode + "00000" + (++rr).ToString();
        //             else
        //                 return itemcode + "0000" + (++rr).ToString();
        //         }
        //         else if (rn.Count.ToString().Length == 2)
        //         {
        //             if (rr != 99)
        //                 return itemcode + "0000" + (++rr).ToString();
        //             else
        //                 return itemcode + "000" + (++rr).ToString();
        //         }
        //         else if (rn.Count.ToString().Length == 3)
        //         {
        //             if (rr != 999)
        //                 return itemcode + "000" + (++rr).ToString();
        //             else
        //                 return itemcode + "00" + (++rr).ToString();
        //         }
        //         else if (rn.Count.ToString().Length == 4)
        //         {
        //             if (rr != 9999)
        //                 return itemcode + "00" + (++rr).ToString();
        //             else
        //                 return itemcode + "0" + (++rr).ToString();
        //         }
        //         else if (rn.Count.ToString().Length == 5)
        //         {
        //             if (rr != 99999)
        //                 return itemcode + "0" + (++rr).ToString();
        //             else
        //                 return itemcode + (++rr).ToString();
        //         }
        //         else
        //         {
        //             return itemcode + (++rr).ToString();
        //         }
        //     }
        //     else
        //     {
        //         return itemcode;
        //     }
        // }
    }
}
