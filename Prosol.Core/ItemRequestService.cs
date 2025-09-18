using Prosol.Core.Interface;
using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using System.Text.RegularExpressions;

// for mail
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Web.Mvc;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System.Web;
using System.IO;
using Excel;
using System.Data;

namespace Prosol.Core.Interface
{

    public partial class ItemRequestService : I_ItemRequest
    {
        private readonly IEmailSettings _Emailservc;
        private readonly IRepository<Prosol_Plants> _ItemRequestRepository;
        private readonly IRepository<Prosol_Master> _ItemRequestSLRepository;
        private readonly IRepository<Prosol_GroupCodes> _ItemRequestGCRepository;
        private readonly IRepository<Prosol_SubGroupCodes> _ItemRequestSGCRepository;
        private readonly IRepository<Prosol_Request> _ItemRequest_Request_Repository;
        private readonly IRepository<Prosol_Users> _ItemRequest_Users_Repository;
        private readonly IRepository<Prosol_RequestRunning> _ItemRequest_RequestRunning_Repository;
        private readonly IRepository<Prosol_ItemIdRunning> _ItemRequest_ItemIdRunning_Repository;
        private readonly IRepository<Prosol_Datamaster> _DatamasterRepository;
        private readonly IRepository<Prosol_Attachment> _attchmentRepository;

        public ItemRequestService(IRepository<Prosol_Datamaster> datamasterRepository, IRepository<Prosol_Plants> ItemRequestRepository,
            IRepository<Prosol_Master> ItemRequestSLRepository,
            IRepository<Prosol_GroupCodes> ItemRequestGCRepository,
            IRepository<Prosol_SubGroupCodes> ItemRequestSGCRepository,
            IRepository<Prosol_Request> ItemRequest_Request_Repository,
            IRepository<Prosol_Users> ItemRequest_Users_Repository,
            IRepository<Prosol_RequestRunning> ItemRequest_RequestRunning_Repository,
         IRepository<Prosol_ItemIdRunning> ItemRequest_ItemIdRunning_Repository, IEmailSettings Emailservc,
         IRepository<Prosol_Attachment> attchmentRepository)
        {
            this._DatamasterRepository = datamasterRepository;
            this._ItemRequestRepository = ItemRequestRepository;
            this._ItemRequestSLRepository = ItemRequestSLRepository;
            this._ItemRequestGCRepository = ItemRequestGCRepository;
            this._ItemRequestSGCRepository = ItemRequestSGCRepository;
            this._ItemRequest_Request_Repository = ItemRequest_Request_Repository;
            this._ItemRequest_Users_Repository = ItemRequest_Users_Repository;
            this._ItemRequest_RequestRunning_Repository = ItemRequest_RequestRunning_Repository;
            this._ItemRequest_ItemIdRunning_Repository = ItemRequest_ItemIdRunning_Repository;
            this._Emailservc = Emailservc;
            this._attchmentRepository = attchmentRepository;
        }


        public List<Prosol_Datamaster> getpossibledup(string term)
        {
            string[] strArr = { "Itemcode", "Shortdesc", "Longdesc" };
            var fields = Fields.Include(strArr);

            //if (term.Contains(' '))
            //{
            var QryLst = new List<IMongoQuery>();
            var QryLst1 = new List<IMongoQuery>();
            string[] sepArr = term.Split(' ');
            if (sepArr.Length > 2)
            {
                foreach (string str in sepArr)
                {
                    if (str != "")
                    {

                        var Qry1 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Shortdesc", BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));
                        var Qry2 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Longdesc", BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));

                        // var dynmic = Query.Or(Qry1, Qry2);
                        QryLst.Add(Qry1);
                        QryLst1.Add(Qry2);
                    }
                }
            }
            else
            {
                foreach (string str in sepArr)
                {
                    if (str != "")
                    {
                        //var Qry1 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Shortdesc", new MongoRegex(string.Format("^{0}", searchKey), "i")
                        var Qry1 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Shortdesc", BsonRegularExpression.Create(new Regex(string.Format("^{0}", str.TrimStart().TrimEnd()), RegexOptions.IgnoreCase))));
                        // var Qry1 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Shortdesc", BsonRegularExpression.Create(new Regex(str, RegexOptions.IgnoreCase))));
                        var Qry2 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Longdesc", BsonRegularExpression.Create(new Regex(string.Format("^{0}", str.TrimStart().TrimEnd()), RegexOptions.IgnoreCase))));

                        // var dynmic = Query.Or(Qry1, Qry2);
                        QryLst.Add(Qry1);
                        QryLst1.Add(Qry2);
                    }
                }
            }
            var query = Query.And(QryLst);
            var arrResult = _DatamasterRepository.FindAll(fields, query).ToList();

            query = Query.And(QryLst1);
            var LongResult = _DatamasterRepository.FindAll(fields, query).ToList();
            foreach (Prosol_Datamaster mdl in LongResult)
            {
                if (-1 == arrResult.FindIndex(f => f.Itemcode.Equals(mdl.Itemcode)))
                    arrResult.Add(mdl);

            }
            return arrResult;
            //}
            //else
            //{
            //    var Qry1 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Shortdesc", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));
            //    var Qry2 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Longdesc", BsonRegularExpression.Create(new Regex(term.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));


            //    var arrResult = _DatamasterRepository.FindAll(fields, Qry1).ToList();
            //    var LongResult = _DatamasterRepository.FindAll(fields, Qry2).ToList();
            //    foreach (Prosol_Datamaster mdl in LongResult)
            //    {
            //        if (-1 == arrResult.FindIndex(f => f.Itemcode.Equals(mdl.Itemcode)))
            //            arrResult.Add(mdl);

            //    }
            //    return arrResult;
            //}           

        }
    
        public IEnumerable<Prosol_Plants> getplantCode_Name()
        {
            var query = Query.EQ("Islive", true);
            var plantdetails = _ItemRequestRepository.FindAll(query).ToList();
            return plantdetails;
        }

        public IEnumerable<Prosol_Master> getStorageCode_Name(string value)
        {
            var query = Query.And(Query.EQ("Plantcode", BsonRegularExpression.Create(new Regex(value, RegexOptions.IgnoreCase))), Query.EQ("Label", "Storage location"), Query.EQ("Islive", true));
            //  var query = Query.Matches("Plantcode", BsonRegularExpression.Create(new Regex(value, RegexOptions.IgnoreCase)));
            var Storagedetails = _ItemRequestSLRepository.FindAll(query).ToList();
            return Storagedetails;
        }

        public IEnumerable<Prosol_Master> getsl()
        {
            var query = Query.And(Query.EQ("Label", "Storage location"), Query.EQ("Islive", true));
            //  var query = Query.Matches("Plantcode", BsonRegularExpression.Create(new Regex(value, RegexOptions.IgnoreCase)));
            var Storagedetails = _ItemRequestSLRepository.FindAll(query).ToList();
            return Storagedetails;
        }
        public IEnumerable<Prosol_Master> findmasterdata()
        {
            // var query = Query.And(Query.EQ("Label", "Storage location"), Query.EQ("Islive", true));
            //  var query = Query.Matches("Plantcode", BsonRegularExpression.Create(new Regex(value, RegexOptions.IgnoreCase)));
            var Storagedetails = _ItemRequestSLRepository.FindAll().ToList();
            return Storagedetails;
        }


        public IEnumerable<Prosol_GroupCodes> getgroupCode_Name()
        {


            var sort = SortBy.Ascending("title");
            string[] incl = { "code", "title" };
            var query = Fields.Include(incl);
            var groupcodes = _ItemRequestGCRepository.FindAll(query, sort).ToList();
            return groupcodes;
        }
        public IEnumerable<Prosol_SubGroupCodes> getsubgroupCode_Name(string str)
        {
            var query = Query.EQ("groupCode", str);
            var groupcodes = _ItemRequestSGCRepository.FindAll(query).ToList();
            return groupcodes;
        }

        public IEnumerable<Prosol_SubGroupCodes> getsgcode_Name()
        {
            var groupcodes = _ItemRequestSGCRepository.FindAll().ToList();
            return groupcodes;
        }

        public IEnumerable<Prosol_Users> get_approvercode_name()
        {
            var query = Query.And(Query.EQ("Usertype", "Approver"), Query.EQ("Islive", "Active"));
            var approver = _ItemRequest_Users_Repository.FindAll(query).ToList();
            return approver;
        }

        public IEnumerable<Prosol_Users> get_approvercode_name_using_approverid(string app)
        {
            var query = Query.And(Query.EQ("Userid", app), Query.EQ("Islive", "Active"));
            var approver = _ItemRequest_Users_Repository.FindAll(query).ToList();
            return approver;
        }
      

        public int getlast_request_R_no(string str)
        {
            var sort = SortBy.Ascending("RunningNo");
            //  string[] order = { "RunningNo" };
            var query = Query.EQ("Sn", str);
            var result = _ItemRequest_RequestRunning_Repository.FindAll(query, sort).ToList();
            if (result.Count > 0)
            {
                int r_no = (result[result.Count - 1].RunningNo) + 1;

                //Prosol_RequestRunning R_R = new Prosol_RequestRunning();
                //R_R.Sn = str;
                //R_R.RunningNo = r_no;
                //bool RES = _ItemRequest_RequestRunning_Repository.Add(R_R);
                return r_no;
            }
            else
            {
                Prosol_RequestRunning R_R = new Prosol_RequestRunning();
                R_R.Sn = str;
                R_R.RunningNo = 1;
                //bool RES = _ItemRequest_RequestRunning_Repository.Add(R_R);

                return 1;
            }

        }
        private string generateRequest()
        {
            string itmCode = "";
            //    var ICode = _CatalogueService.getItem();
            //            public string getItem()
            //{
            string code = "";
            var sort = SortBy.Descending("_id");

            var query = Query.Matches("Itemcode", new BsonRegularExpression("^[0-9]*$"));
            //  var query = Query.Matches("Itemcode", Regex.Matches(Itemcode, @"\d{1,2}")
            var Itmcode = _DatamasterRepository.FindAll(query, sort).ToList();
            if (Itmcode != null && Itmcode.Count > 0)
            {
                code = Itmcode[0].Itemcode;
            }
            //    return code;
            //}
            if (code != "")
            {


                long serialinr = Convert.ToInt64(code);
                serialinr++;
                string addincr = "";
                switch (serialinr.ToString().Length)
                {
                    case 1:
                        addincr = "0000" + serialinr;
                        break;
                    case 2:
                        addincr = "000" + serialinr;
                        break;
                    case 3:
                        addincr = "00" + serialinr;
                        break;
                    case 4:
                        addincr = "0" + serialinr;
                        break;
                    default:
                        addincr = serialinr.ToString();
                        break;

                }
                itmCode = addincr;

            }
            else
            {
                itmCode = "00001";
            }
            return itmCode;
        }
        public bool insert_multiplerequest(List<Prosol_Request> request, string reqid, HttpFileCollectionBase files)
        {
            var lstAtt = new List<Prosol_Attachment>();
            int i = 1;
            foreach (Prosol_Request pr1 in request)
            {
                pr1.requestId = reqid;

                // pr1.requestedOn =DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                // var val = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                pr1.requestedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                pr1.itemStatus = "pending";

                //if ((pr1.itemId == null || pr1.itemId == "") && pr1.group != null && pr1.group != "" && pr1.group != "undefined")
                //{

                //    var sort = SortBy.Ascending("RunningNo");
                //    // string[] order = { "RunningNo" };
                //    var query = Query.And(Query.EQ("Group", pr1.group), Query.EQ("Subgroup", pr1.subGroup));
                //    var result = _ItemRequest_ItemIdRunning_Repository.FindAll(query, sort).ToList();


                //    if (result.Count > 0)
                //    {
                //        pr1.itemId = pr1.group + "-" + pr1.subGroup + "-" + ((result[result.Count - 1].RunningNo) + 1).ToString();


                //        Prosol_ItemIdRunning i_r = new Prosol_ItemIdRunning();
                //        i_r.Group = pr1.group;
                //        i_r.Subgroup = pr1.subGroup;
                //        i_r.RunningNo = result[result.Count - 1].RunningNo + 1;
                //        bool res1 = _ItemRequest_ItemIdRunning_Repository.Add(i_r);
                //    }
                //    else
                //    {
                //        pr1.itemId = pr1.group + "-" + pr1.subGroup + "-300001";

                //        Prosol_ItemIdRunning i_r = new Prosol_ItemIdRunning();
                //        i_r.Group = pr1.group;
                //        i_r.Subgroup = pr1.subGroup;
                //        i_r.RunningNo = 300001;
                //        bool res1 = _ItemRequest_ItemIdRunning_Repository.Add(i_r);
                //    }

                //}
                //else
                //{

                if (request.Count > 0)
                {
                    string addincr = "";
                    switch (i.ToString().Length)
                    {
                        case 1:
                            addincr = "00" + i;
                            break;
                        case 2:
                            addincr = "0" + i;
                            break;
                        default:
                            addincr = i.ToString();
                            break;

                    }
                    pr1.itemId = pr1.requestId + "-" + addincr;
                    i++;
                    //   }

                    if (files != null)
                    {
                        for (int g = 0; g < files.Count; g++)
                        {
                            if (pr1.attachment != "" && pr1.attachment == files[g].FileName)
                            {

                                if (files[g] != null && files[g].ContentLength > 0)
                                {
                                    var atmnt = new Prosol_Attachment();
                                    atmnt.Itemcode = pr1.itemId;
                                    atmnt.FileName = files[g].FileName;
                                    atmnt.ContentType = files[g].ContentType;
                                    atmnt.FileSize = GetFileSize(Convert.ToDouble(files[g].ContentLength));
                                    atmnt.FileId = _attchmentRepository.GridFsUpload(files[g].InputStream, atmnt.FileName);
                                    lstAtt.Add(atmnt);
                                }
                            }
                        }
                    }



                    //var query = Query.EQ("itemId", pr1.itemId);
                    //var result = _ItemRequest_Request_Repository.FindOne(query);
                    //pr1._id = result._id;

                }
            }

            if (lstAtt.Count > 0)
                _attchmentRepository.Add(lstAtt);

            int f_res = _ItemRequest_Request_Repository.Add(request);

            if (f_res > 0)
            {
                var sort = SortBy.Ascending("RunningNo");
                string date1 = DateTime.Now.ToString("MMddyyyy").ToUpper();
                var query1 = Query.EQ("Sn", date1);
                var result = _ItemRequest_RequestRunning_Repository.FindAll(query1, sort).ToList();
                if (result.Count > 0)
                {
                    int r_no1 = (result[result.Count - 1].RunningNo) + 1;

                    var query = Query.EQ("Sn", date1);
                    var Updte = Update.Set("RunningNo", Convert.ToString(r_no1));
                    var flg = UpdateFlags.Upsert;
                    var theResult = _ItemRequest_RequestRunning_Repository.Update(query, Updte, flg);


                }
                else
                {
                    Prosol_RequestRunning R_R = new Prosol_RequestRunning();
                    R_R.Sn = date1;
                    R_R.RunningNo = 1;
                    bool RES = _ItemRequest_RequestRunning_Repository.Add(R_R);
                    return true;
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        private string GetFileSize(double byteCount)
        {
            string size = "0 Bytes";
            if (byteCount >= 1073741824.0)
                size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";
            else if (byteCount >= 1048576.0)
                size = String.Format("{0:##.##}", byteCount / 1048576.0) + " MB";
            else if (byteCount >= 1024.0)
                size = String.Format("{0:##.##}", byteCount / 1024.0) + " KB";
            else if (byteCount > 0 && byteCount < 1024.0)
                size = byteCount.ToString() + " Bytes";

            return size;
        }
        public bool insert_Rerequest(List<Prosol_Request> request, HttpFileCollectionBase files)
        {
            var f_res = false;
            var qry = Query.EQ("itemId", request[0].itemId);
            var objReq = _ItemRequest_Request_Repository.FindOne(qry);
            if (objReq.itemStatus == "clarification")
            {
                objReq.itemStatus = "approved";
                objReq.reason_rejection = request[0].reason_rejection;
                f_res = _ItemRequest_Request_Repository.Add(objReq);

                var qry1 = Query.EQ("Itemcode", request[0].itemId);
                var objCat = _DatamasterRepository.FindOne(qry1);
                objCat.ItemStatus = 0;
                objCat.Legacy = request[0].source;
                objCat.Remarks = request[0].reason_rejection;
                _DatamasterRepository.Add(objCat);
                //return f_res;
            }
            else
            {
                objReq.itemStatus = "pending";
                objReq.reason_rejection = request[0].reason_rejection;
                objReq.group = request[0].group;
                objReq.subGroup = request[0].subGroup;
                objReq.Materialtype = request[0].Materialtype;
                objReq.Industrysector = request[0].Industrysector;
                objReq.MaterialStrategicGroup = request[0].MaterialStrategicGroup;
                objReq.source = request[0].source;
                objReq.plant = request[0].plant;
                objReq.storage_Location = request[0].storage_Location;
                //objReq.UnitPrice = request[0].UnitPrice;

                f_res = _ItemRequest_Request_Repository.Add(objReq);
                //return f_res;
            }


            if (files != null)
            {
                if (files[0] != null && files[0].ContentLength > 0)
                {
                    var Qry = Query.EQ("Itemcode", objReq.itemId);
                    var resForDel = _attchmentRepository.FindOne(Qry);
                    if (resForDel != null)
                    {
                        _attchmentRepository.GridFsDel(Query.EQ("_id", new ObjectId(resForDel.FileId)));

                        resForDel.Itemcode = objReq.itemId;
                        resForDel.FileName = files[0].FileName;
                        resForDel.ContentType = files[0].ContentType;
                        resForDel.FileSize = GetFileSize(Convert.ToDouble(files[0].ContentLength));
                        resForDel.FileId = _attchmentRepository.GridFsUpload(files[0].InputStream, resForDel.FileName);
                        _attchmentRepository.Add(resForDel);
                    }
                    else
                    {
                        var atmnt = new Prosol_Attachment();
                        atmnt.Itemcode = objReq.itemId;
                        atmnt.FileName = files[0].FileName;
                        atmnt.ContentType = files[0].ContentType;
                        atmnt.FileSize = GetFileSize(Convert.ToDouble(files[0].ContentLength));
                        atmnt.FileId = _attchmentRepository.GridFsUpload(files[0].InputStream, atmnt.FileName);
                        _attchmentRepository.Add(atmnt);
                    }


                }
            }


            return f_res;
        }

        public bool newRequest(Prosol_Request req_model)
        {
            //  var query = Query.And(Query.EQ("Userid", req_model.approver), Query.EQ("Userid", req_model.approver));
            var query = Query.And(Query.EQ("Userid", req_model.approver), Query.EQ("Islive", "Active"));
            var user_deteails = _ItemRequest_Users_Repository.FindAll(query).ToList();
            string email_to = user_deteails[0].EmailId;

            var query1 = Query.And(Query.EQ("Userid", req_model.requester), Query.EQ("Islive", "Active"));
            var From_user = _ItemRequest_Users_Repository.FindAll(query1).ToList();

            string from = From_user[0].EmailId;
            using (MailMessage mail = new MailMessage(from, email_to))
            {

                string to_mail = email_to;
                // string userid = Data[0].Userid;

                //Prosol_EmailSettings email1 = new Prosol_EmailSettings();

                //string email_too = to_mail;
                // email1.email_from = "codasol.madras@gmail.com";
                string subjectt = "New Item Request";
                string body = "You have received an item request for an item code " + req_model.group + "-" + req_model.subGroup + "-001 from " + From_user[0].UserName;
                // email1.IsBodyHtml = true;
                // email1.host = "smtp.gmail.com";
                //  email1.enablessl = true;
                // email1.UseDefaultCredentials = true;
                // email1.Port = 587;
                // email1.password = "codasolwestmambalam";

                //  emailservice es = new emailservice();
                //   EmailSettingService ems = new EmailSettingService();
                bool val = _Emailservc.sendmail(to_mail, subjectt, body);
            }
            try
            {

                req_model.approvedOn = null;
                bool result = _ItemRequest_Request_Repository.Add(req_model);
                //  smtp.Send(msg);
                return result;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public IEnumerable<Prosol_Master> getslname(string sl)
        {
            var query = Query.And(Query.EQ("Code",sl));
            var mx = _ItemRequestSLRepository.FindAll(query).ToList();
            return mx;
        }

        public bool deleteRequest(string Itemid)
        {
            var query = Query.EQ("itemId", Itemid);
            var Updte = Update.Set("itemStatus", "Deleted");
            var flg = UpdateFlags.Upsert;
            var theResult = _ItemRequest_Request_Repository.Update(query, Updte, flg);
            return theResult;

        }

        public List<Bulkrequest_load> loaddata(HttpPostedFileBase file)
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
            reader.Close();
            DataTable dt = res.Tables[0];
            string dkfl = dt.Columns[0].ToString();

            List<Bulkrequest_load> brl = new List<Bulkrequest_load>();

            if (dt.Columns[0].ToString() == "Plant" && dt.Columns[1].ToString() == "Storage Location")
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0] != null && row[0] != "")
                    {
                        Bulkrequest_load brl1 = new Bulkrequest_load();
                        brl1.plant = row["Plant"].ToString();
                        brl1.storage_Location = row["Storage Location"].ToString();
                        if (dt.Columns[2].ToString() == "Group")
                        {
                            brl1.group = row["Group"].ToString();
                            brl1.subGroup = row["Sub Group"].ToString();

                           // brl1.UnitPrice = row["Unit Price"].ToString();
                        }
                        brl1.source = row["Source"].ToString();
                        brl1.Materialtype = row["Material Type"].ToString();
                        brl1.Industrysector = row["Industrial Sector"].ToString();
                        brl1.MaterialStrategicGroup = row["Material Group"].ToString();


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
                        if (brl1 != null && !String.IsNullOrEmpty(brl1.plant))
                            brl.Add(brl1);
                    }
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


        public List<Prosol_Datamaster> getpossibledup_bulk(string term)
        {
            string[] strArr = { "Itemcode", "Shortdesc", "Longdesc" };
            var fields = Fields.Include(strArr);


            var QryLst = new List<IMongoQuery>();
            var QryLst1 = new List<IMongoQuery>();

            string[] sepArr = term.Split(' ');
            if (sepArr.Length > 2)
            {
                foreach (string str in sepArr)
                {
                    if (str != "")
                    {

                        var Qry1 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Shortdesc", BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));
                        var Qry2 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Longdesc", BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));

                        QryLst.Add(Qry1);
                        QryLst1.Add(Qry2);
                    }
                }
            }
            else
            {
                foreach (string str in sepArr)
                {
                    if (str != "")
                    {

                        var Qry1 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Shortdesc", BsonRegularExpression.Create(new Regex(string.Format("^{0}", str.TrimStart().TrimEnd()), RegexOptions.IgnoreCase))));

                        var Qry2 = Query.And(Query.GTE("ItemStatus", 2), Query.Matches("Longdesc", BsonRegularExpression.Create(new Regex(string.Format("^{0}", str.TrimStart().TrimEnd()), RegexOptions.IgnoreCase))));

                        QryLst.Add(Qry1);
                        QryLst1.Add(Qry2);
                    }
                }
            }
           var arrResult = new List<Prosol_Datamaster>();
            var LongResult = new List<Prosol_Datamaster>();
            if (QryLst.Count > 0)
            {
                var query = Query.And(QryLst);
                 arrResult = _DatamasterRepository.FindAll(fields, query).ToList();
            }
            if (QryLst1.Count > 0)
            {

                var query = Query.And(QryLst1);
                LongResult = _DatamasterRepository.FindAll(fields, query).ToList();
            }
                foreach (Prosol_Datamaster mdl in LongResult)
                {
                    if (-1 == arrResult.FindIndex(f => f.Itemcode.Equals(mdl.Itemcode)))
                        arrResult.Add(mdl);

                }

                return arrResult;


            }
        }
    
}