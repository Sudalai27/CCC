using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Prosol.Core;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using ProsolOnline.Models;
using Newtonsoft.Json;
using System.IO;
using System.Data;
using System.Net.Mail;

namespace ProsolOnline.Controllers
{
    public class MaterialRequestController : Controller
    {
        private readonly IEmailSettings _Emailservc;
        private readonly I_ItemRequest _ItemRequestService;
        private readonly IUserCreate _UserCreateService;
        private readonly ISearch _SearchService;
        public MaterialRequestController(I_ItemRequest ItemRequestService, IUserCreate UserCreateService, IEmailSettings Emailservc,
            ISearch searchService)
        {
            _ItemRequestService = ItemRequestService;
            _UserCreateService = UserCreateService;
            _Emailservc = Emailservc;
            _SearchService = searchService;
        }

        // GET: MaterialRequest
        [Authorize]
        public ActionResult Index()
        {
            if (CheckAccess("Request Item") == 1)
                return View("Req_Index");
            else if (CheckAccess("Request Item") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public JsonResult getpossibledup(string sKey)
        {
           // var s = sKey.ToUpper().Split(new string[] { " ", ",", ":", "*" }, StringSplitOptions.RemoveEmptyEntries);
            var srchList = _SearchService.SearchDesc(sKey, "Description");
            var lstCatalogue = new List<CatalogueModel>();
            foreach (Prosol_Datamaster cat in srchList)
            {
                int flg = 0;
                var proCat = new CatalogueModel();
                proCat._id = cat._id.ToString();
                proCat.Itemcode = cat.Itemcode;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;
                //var LstVendors = new List<Vendorsupplier>();
                //if (cat.Vendorsuppliers != null)
                //{
                //    foreach (Vendorsuppliers vndrs in cat.Vendorsuppliers)
                //    {

                //        if (vndrs.RefNo != null && vndrs.RefNo != "" && sKey.Contains(vndrs.RefNo))
                //        {
                //            flg = 1;
                //        }
                //    }
                //}
                //if (flg == 0)
                //{

                //    var res1 = proCat.Longdesc.Split(new string[] { " ", ",", ":" }, StringSplitOptions.RemoveEmptyEntries);

                //    var res = (from char l in proCat.Longdesc
                //               select new
                //               {
                //                   count = proCat.Longdesc.Split(new char[] { ' ', ',', ':' }).Sum(p => s.Contains(p) ? 1 : 0)
                //               }).OrderByDescending(p => p.count).First();

                //    int p1 = res.count;
                //    int p2 = res1.Length;
                //    var p3 = ((p1 * 100) / p2);
                //    proCat.Percentage = p3;
                //    proCat.width = p3 + "%";
                //}
                //else
                //{
                //    proCat.Percentage = 100;
                //    proCat.width = 100 + "%";
                //}
                lstCatalogue.Add(proCat);
            }
            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            // return this.Json(lstCatalogue, JsonRequestBehavior.AllowGet);

        }
        public int CheckAccess(string pageName)
        {
            string pages = Convert.ToString(Session["access"]);
            if (!string.IsNullOrEmpty(pages))
            {
                String[] stringArray = pages.Split(',');
                if (Array.IndexOf(stringArray, pageName) > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    return 1;
                else return 0;

            }
            else return -1;

        }
        [Authorize]
        public JsonResult getplantCode_Name()
        {
            var plantdetails = _ItemRequestService.getplantCode_Name();
            var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));

            List<Prosol_Plants> lst = new List<Prosol_Plants>();
            foreach (Prosol_Plants mdl in plantdetails)
            {
                if (usrInfo.Plantcode == null)
                {
                    lst.Add(mdl);
                }
                else
                {
                    foreach (string cd in usrInfo.Plantcode)
                    {
                        if (cd == mdl.Plantcode)
                        {
                            lst.Add(mdl);
                        }
                    }

                }
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult getStorageCode_Name(string Plantcode)
        {
            var storageDetails = _ItemRequestService.getStorageCode_Name(Plantcode);
            return this.Json(storageDetails, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult getgroupCode_Name()
        {
            var groupcodes = _ItemRequestService.getgroupCode_Name();
            return this.Json(groupcodes, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult getsubgroupCode_Name(string groupcode)
        {
            var subgroupcodes = _ItemRequestService.getsubgroupCode_Name(groupcode);
            return this.Json(subgroupcodes, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult get_approvercode_name()
        {
            var approver = _ItemRequestService.get_approvercode_name();
            return this.Json(approver, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult newRequest()
        {

            var mul_req_values = Request.Form["Single_request"];
            string date1 = DateTime.Now.ToString("MM/dd/yyyy");
            date1 = date1.Replace(@"-", "");
            date1 = date1.Replace(@"/", "");
            date1 = date1.Replace(@" ", "");

            int reqid = _ItemRequestService.getlast_request_R_no(date1);
            date1 = date1 + reqid.ToString();

            var files = Request.Files.Count > 0 ? Request.Files : null;

            var m_mul_req_values = JsonConvert.DeserializeObject<List<Prosol_Request>>(mul_req_values);
            var mirm = JsonConvert.DeserializeObject<List<ItemrequestModel>>(mul_req_values);
        
            var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));
            string tmpStr = "";
            foreach (TargetExn ent in usrInfo.Roles)
            {
                if (ent.Name == "Requester") tmpStr = ent.TargetId;
            }


            foreach (Prosol_Request pr2 in m_mul_req_values)
            {
                pr2.requester = Session["userid"].ToString();
                pr2.approver = tmpStr;
            }

            var tbl = new DataTable();
            tbl.Columns.Add("Plant");
            tbl.Columns.Add("Storage Location");
            tbl.Columns.Add("Material Type");
            tbl.Columns.Add("Industrial Sector");
            tbl.Columns.Add("Material Group");
            tbl.Columns.Add("Source");
            foreach (ItemrequestModel pr1 in mirm)
            {
                var row = tbl.NewRow();
                row["Plant"] = pr1.hplant;
                row["Storage Location"] = pr1.hstorage_Location;
                row["Material Type"] = pr1.hmaterialtype;
                row["Industrial Sector"] = pr1.hIndustrysector;
                row["Material Group"] = pr1.hMaterialStrategicGroup;
                row["Source"] = pr1.hsource;
                tbl.Rows.Add(row);



            }
            
            string app = m_mul_req_values[0].approver;
           
            try
            {
                var user_deteails = _ItemRequestService.get_approvercode_name_using_approverid(app).ToList();

                string to_mail = user_deteails[0].EmailId;
              
                string email_to = to_mail;
                string subject = "";
           
                if (Session["username"] != null)
                    subject = "New Request from " + Session["username"].ToString() + " REQUEST ID : " + date1;
            
                bool val = _Emailservc.sendmail(to_mail, subject, _Emailservc.getmailbody(tbl));
            }
            catch
            {
                string mul_resilt1 = "Error Sending Mail";
                return this.Json(mul_resilt1, JsonRequestBehavior.AllowGet);
            }
            // }

            Guid imgGuid = Guid.NewGuid();
            string imgName = "";
            if (files != null)
            {
                imgName = imgGuid + Path.GetExtension(files[0].FileName);
            }
            m_mul_req_values[0].attachment = imgName;
            m_mul_req_values[0].requestedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            bool mul_resilt = _ItemRequestService.insert_multiplerequest(m_mul_req_values, date1, null);

            if (mul_resilt == true)
            {
                if (files != null)
                {
                    string path = Server.MapPath("~/Attachment/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    files[0].SaveAs(path + imgName);

                }
                return this.Json(true, JsonRequestBehavior.AllowGet);

            }
            else
                return this.Json(false, JsonRequestBehavior.AllowGet);


            //// int running_no = getrunning_number(group+"-"+subgroup);
            //var request_model = new Prosol_Request();
            //request_model.requestId = "";
            //request_model.itemId = group + "-" + subgroup + "-001";
            //request_model.source = source;
            //request_model.plant = plant;
            //request_model.storage_Location = storage;
            //request_model.group = group;
            //request_model.subGroup = subgroup;
            //request_model.requestedOn =DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //request_model.approver = approver;
            //request_model.itemStatus = "pending";
            //request_model.requester = Session["userid"].ToString();

            //return _ItemRequestService.newRequest(request_model);

        }


        [Authorize]
        public JsonResult bulk_material_request()
        {

            var mul_req_values = Request.Form["Mul_request"];
            var irm = Request.Form["Mul_request"];
            string date1 = DateTime.Now.ToString("MM/dd/yyyy");
            date1 = date1.Replace(@"-", "");
            date1 = date1.Replace(@"/", "");
            date1 = date1.Replace(@" ", "");

            int reqid = _ItemRequestService.getlast_request_R_no(date1);
            string addincr = "";
            switch (reqid.ToString().Length)
            {

                case 1:
                    addincr = "00" + reqid;
                    break;
                case 2:
                    addincr = "0" + reqid;
                    break;
                default:
                    addincr = reqid.ToString();
                    break;

            }


            date1 = date1 + addincr;
        
            var files = Request.Files.Count > 0 ? Request.Files : null;

            var m_mul_req_values = JsonConvert.DeserializeObject<List<Prosol_Request>>(mul_req_values);
            var mirm = JsonConvert.DeserializeObject<List<ItemrequestModel>>(irm);
          
            foreach (Prosol_Request pr2 in m_mul_req_values)
            {
                pr2.requester = Session["userid"].ToString();
            }
            var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));
            string tmpStr = "";
            foreach (TargetExn ent in usrInfo.Roles)
            {
                if (ent.Name == "Requester") tmpStr = ent.TargetId;
            }
            foreach (Prosol_Request pr2 in m_mul_req_values)
            {
                pr2.requester = Session["userid"].ToString();
                pr2.approver = tmpStr;
            }
            
            string app = m_mul_req_values[0].approver;
           
            var tbl = new DataTable();
            tbl.Columns.Add("Plant");
            tbl.Columns.Add("Storage Location");
            tbl.Columns.Add("Material Type");
            tbl.Columns.Add("Industrial Sector");
            tbl.Columns.Add("Material Group");
            tbl.Columns.Add("Source");
            foreach (ItemrequestModel pr1 in mirm)
            {
                var row = tbl.NewRow();
                row["Plant"]= pr1.hplant;
                row["Storage Location"] = pr1.hstorage_Location;
                row["Material Type"] = pr1.hmaterialtype;
                row["Industrial Sector"] = pr1.hIndustrysector;
                row["Material Group"] = pr1.hMaterialStrategicGroup;
                row["Source"] = pr1.hsource;
                tbl.Rows.Add(row);
              
             
               
            }
            
            var user_deteails = _ItemRequestService.get_approvercode_name_using_approverid(app).ToList();
            string to_mail = user_deteails[0].EmailId;        
            string subject = "";
        
            if (Session["username"] != null)
                subject = "New Request from " + Session["username"].ToString()+" REQUEST ID : " + date1 ;
          
          
           

            // new
            bool mul_resilt = false;
            if (files != null)
            {
              
                 mul_resilt = _ItemRequestService.insert_multiplerequest(m_mul_req_values, date1, files);
           
            }
            else
            {
                 mul_resilt = _ItemRequestService.insert_multiplerequest(m_mul_req_values, date1, files);
               
            }
           
            if (mul_resilt )
            {
                bool val = _Emailservc.sendmail(to_mail, subject, _Emailservc.getmailbody(tbl));
                return this.Json(mul_resilt, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json(mul_resilt, JsonRequestBehavior.AllowGet);
            }
               
        }

        [Authorize]
        public JsonResult ReRequest()
        {

            var rerequestitem = Request.Form["Rerequest"];

            var files = Request.Files.Count > 0 ? Request.Files : null;

            var deReRequest = JsonConvert.DeserializeObject<List<Prosol_Request>>(rerequestitem);

            if (files != null)
            {
                //int rr = files.Count;
                //string[] filename1 = new string[rr];
                //for (int oo = 0; oo < files.Count; oo++)
                //{
                //    Guid imgGuid = Guid.NewGuid();
                //    filename1[oo] = imgGuid + Path.GetExtension(files[oo].FileName);
                //    deReRequest[oo].attachment = filename1[oo];
                //    deReRequest[oo].requestedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //}


                bool mul_resilt = _ItemRequestService.insert_Rerequest(deReRequest, files);

                //for (int oo = 0; oo < files.Count; oo++)
                //{
                //    if (mul_resilt == true)
                //    {
                //        if (files[oo] != null)
                //        {
                //            string path = Server.MapPath("~/Attachment/");
                //            if (!Directory.Exists(path))
                //            {
                //                Directory.CreateDirectory(path);
                //            }

                //            files[oo].SaveAs(path + filename1[oo]);


                //        }


                //    }
                //}



                //  if (mul_resilt == true)
                return this.Json(mul_resilt, JsonRequestBehavior.AllowGet);
                // else
                // return this.Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                bool mul_resilt = _ItemRequestService.insert_Rerequest(deReRequest, null);
                return this.Json(mul_resilt, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult bulkupload_material_request()
        {

            var mul_req_values = Request.Form["sResultbulk"];
            var irm = Request.Form["sResultbulk"];
            string date1 = DateTime.Now.ToString("MM/dd/yyyy");
            date1 = date1.Replace(@"-", "");
            date1 = date1.Replace(@"/", "");
            date1 = date1.Replace(@" ", "");

            int reqid = _ItemRequestService.getlast_request_R_no(date1);


            string addincr = "";
            switch (reqid.ToString().Length)
            {

                case 1:
                    addincr = "00" + reqid;
                    break;
                case 2:
                    addincr = "0" + reqid;
                    break;
                default:
                    addincr = reqid.ToString();
                    break;

            }


            date1 = date1 + addincr;

            var planttt = _ItemRequestService.getplantCode_Name();
            var sl = _ItemRequestService.getsl();
            var grp_b = _ItemRequestService.getgroupCode_Name();
            var sgcode = _ItemRequestService.getsgcode_Name();

            //newfeild

            var findmaster = _ItemRequestService.findmasterdata();


            var m_mul_req_values = JsonConvert.DeserializeObject<List<Prosol_Request>>(mul_req_values);
            var mirm = JsonConvert.DeserializeObject<List<ItemrequestModel>>(irm);
            //  var result = arrStr.Select(i => new { i.Noun, i.NounDefinition }).Distinct();
            //  var app_list = m_mul_req_values.Select(m => new { m.approver }).Distinct().ToList();

            foreach (Prosol_Request pr2 in m_mul_req_values)
            {
                int plantil = 0;
                int slil = 0;
                int gril = 0;
                int sgil = 0;
                foreach (Prosol_Plants pp in planttt)
                {
                    if (pr2.plant == pp.Plantname)
                    {
                        plantil = 1;
                        pr2.plant = pp.Plantcode;
                        break;
                    }
                }
                if (plantil == 0)
                {
                    return this.Json("Invalid Plant " + pr2.plant + " found", JsonRequestBehavior.AllowGet);
                }

                foreach (Prosol_Master pm in sl)
                {
                    if (pr2.plant == pm.Plantcode && pr2.storage_Location.ToUpper() == pm.Title.ToUpper())
                    {
                        slil = 1;
                        pr2.storage_Location = pm.Code;
                        break;
                    }
                }
                if (slil == 0)
                {
                    return this.Json("Invalid Storage Location " + pr2.storage_Location + " found", JsonRequestBehavior.AllowGet);
                }

                // Material Type

                if (pr2.Materialtype != null && pr2.Materialtype != "")
                {
                    var MatTypeRes = findmaster.Where(x => x.Label == "Material type" && x.Title.ToUpper() == pr2.Materialtype.ToUpper()).ToList();
                    if (MatTypeRes.Count == 0)
                    {
                        return this.Json("Material type " + pr2.Materialtype + " is not exist in the master", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        pr2.Materialtype = MatTypeRes[0].Code;
                    }
                }

                // Industry Sector
                if (pr2.Industrysector != null && pr2.Industrysector != "")
                {
                    var IndSecRes = findmaster.Where(x => x.Label == "Industry sector" && x.Title.ToUpper() == pr2.Industrysector.ToUpper()).ToList();
                    if (IndSecRes.Count == 0)
                    {
                        return this.Json("Industry sector " + pr2.Industrysector + " is not exist in the master", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        pr2.Industrysector = IndSecRes[0].Code;
                    }
                }

                //Mat. Strategic Grp       

                if (pr2.MaterialStrategicGroup != null && pr2.MaterialStrategicGroup != "")
                {
                    var MatStrgGrpRes = findmaster.Where(x => x.Label == "Material Strategic Group" && x.Title.ToUpper() == pr2.MaterialStrategicGroup.ToUpper()).ToList();
                    if (MatStrgGrpRes.Count == 0)
                    {
                        return this.Json("Material Group " + pr2.MaterialStrategicGroup + " is not exist in the master", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        pr2.MaterialStrategicGroup = MatStrgGrpRes[0].Code;
                    }
                }

                pr2.UnitPrice = pr2.UnitPrice;
                if (pr2.group != null && pr2.group != "")
                {
                    foreach (Prosol_GroupCodes pp in grp_b)
                    {
                        if (pr2.group == pp.title)
                        {
                            gril = 1;
                            pr2.group = pp.code;
                            break;
                        }
                    }
                    if (gril == 0)
                    {
                        return this.Json("Invalid Group " + pr2.group + " found", JsonRequestBehavior.AllowGet);
                    }
                }
                if (pr2.subGroup != null && pr2.subGroup != "")
                {
                    foreach (Prosol_SubGroupCodes pm in sgcode)
                    {
                        if (pr2.group == pm.groupCode && pr2.subGroup.ToUpper() == pm.title.ToUpper())
                        {
                            sgil = 1;
                            pr2.subGroup = pm.code;
                            break;
                        }
                    }
                    if (sgil == 0)
                    {
                        return this.Json("Invalid Group " + pr2.subGroup + " found", JsonRequestBehavior.AllowGet);
                    }

                }
            }



            foreach (Prosol_Request pr2 in m_mul_req_values)
            {
                pr2.requester = Session["userid"].ToString();
            }

            var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));
            string tmpStr = "";
            foreach (TargetExn ent in usrInfo.Roles)
            {
                if (ent.Name == "Requester") tmpStr = ent.TargetId;
            }
            foreach (Prosol_Request pr2 in m_mul_req_values)
            {
                pr2.requester = Session["userid"].ToString();
                pr2.approver = tmpStr;
            }


            var tbl = new DataTable();
            tbl.Columns.Add("Plant");
            tbl.Columns.Add("Storage Location");
            tbl.Columns.Add("Material Type");
            tbl.Columns.Add("Industrial Sector");
            tbl.Columns.Add("Material Group");
            tbl.Columns.Add("Source");
            foreach (Prosol_Request pr1 in m_mul_req_values)
            {
               // var slname = _ItemRequestService.getslname(pr1.storage_Location);
                var row = tbl.NewRow();
                row["Plant"] = pr1.plant;
                row["Storage Location"] = pr1.storage_Location;
                row["Material Type"] = pr1.Materialtype;
                row["Industrial Sector"] = pr1.Industrysector;
                row["Material Group"] = pr1.MaterialStrategicGroup;
                row["Source"] = pr1.source;
                tbl.Rows.Add(row);



            }



            //    var getemailtable = _ItemRequestService.getmail(m_mul_req_values);


            // foreach (var item in app_list)
            // {
            ////////////
            //string str1 = "<html><body> REQUEST ID : " + date1 + " <table style='border: 1px solid black;border-collapse:collapse'><tr><th style='border:1px solid black'>S.No</th><th style='border:1px solid black'>Plant</th><th style='border:1px solid black'>StorageLocation</th><th style='border:1px solid black'>Group</th><th style='border:1px solid black'>SubGroup</th><th style='border:1px solid black'>Source</th></tr>";
            //string str2 = "";

            //// string app = item.approver;
            string app = m_mul_req_values[0].approver;
            //int i = 0;
            //foreach (Prosol_Request pr1 in m_mul_req_values)
            //{
            //    // if (app == pr1.approver)
            //    // {
            //    str2 = "<tr><td style='border:1px solid black'>" + ++i + "</td><td style='border:1px solid black'>" + pr1.plant + "</td><td style='border:1px solid black'>" + pr1.storage_Location + "</td><td style='border:1px solid black'>" + pr1.group + "</td><td style='border:1px solid black'>" + pr1.subGroup + "</td><td style='border:1px solid black'>" + pr1.source + "</td></tr>";
            //    str1 = str1 + str2;
            //    // }
            //}

            //string str3 = "</table></body ></html > ";
            //str1 = str1 + str3;

            var user_deteails = _ItemRequestService.get_approvercode_name_using_approverid(app).ToList();




            string to_mail = user_deteails[0].EmailId;
            // string userid = Data[0].Userid;
            // email email1 = new email();
            string email_to = to_mail;
            string subject = "";
            // email1.email_from = "codasol.madras@gmail.com";
            if (Session["username"] != null)
                subject = "New Request from " + Session["username"].ToString() + " REQUEST ID : " + date1;
         //   string body = str1;
            //email1.IsBodyHtml = true;
            // email1.host = "smtp.gmail.com";
            // email1.enablessl = true;
            //email1.UseDefaultCredentials = true;
            //email1.Port = 587;
            //email1.password = "codasolwestmambalam";

            // EmailSettingService ems = new EmailSettingService();
            ////////////////////////
            bool val = _Emailservc.sendmail(to_mail, subject, _Emailservc.getmailbody(tbl));



            // }

            bool mul_resilt = _ItemRequestService.insert_multiplerequest(m_mul_req_values, date1, null);

            if (mul_resilt == true)
                return this.Json(true, JsonRequestBehavior.AllowGet);
            else
                return this.Json(false, JsonRequestBehavior.AllowGet);

        }
       
        //public DataTable CreateDataSource()
        //{
        //    DataTable dt = new DataTable();
        //    DataColumn identity = new DataColumn("ID", typeof(int));
        //    dt.Columns.Add(identity);
        //    dt.Columns.Add("Name", typeof(string));
        //    return dt;
        //}
        ////This is the AddRow method to add a new row in Table dt 
        //public void AddRow(int id, string name, DataTable dt)
        //{
        //    dt.Rows.Add(new object[] { id, name, pname });
        //}
        public JsonResult DelRequest(string Itemid)
        {
            var res = _ItemRequestService.deleteRequest(Itemid);
            return this.Json(res, JsonRequestBehavior.AllowGet);

        }
       
  
        public JsonResult Load_Data()
        {
            DataTable dtload = new DataTable();
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    var loaddata = _ItemRequestService.loaddata(file);
                    return Json(loaddata, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Check File Format", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Uploaded File is Emplty", JsonRequestBehavior.AllowGet);
            }
        }


    }
}
