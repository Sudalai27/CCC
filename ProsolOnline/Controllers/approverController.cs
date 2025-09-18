using Prosol.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Prosol.Core;
using Prosol.Core.Model;
using ProsolOnline.Models;
using Newtonsoft.Json;
using System.Data;

namespace ProsolOnline.Controllers
{
    public class approverController : Controller
    {
        private readonly IItemApprove _ItemApproveService;
        private readonly IUserCreate _UserCreateService;
        private readonly IEmailSettings _Emailservc;
        private readonly ICatalogue _CatalogueService;
        public approverController(IItemApprove ItemApproveService, IUserCreate UserCreateService,
  IEmailSettings Emailservc, ICatalogue catalogueService)
        {
            _ItemApproveService = ItemApproveService;
            _UserCreateService = UserCreateService;

            _Emailservc = Emailservc;
            _CatalogueService = catalogueService;
        }

        // GET: approver
        [Authorize]
        public ActionResult Index()
        {

            if (CheckAccess("Approve Item") == 1)
                return View();
            else if (CheckAccess("Approve Item") == 0)
                return View("Denied");
            else return View("Login");           
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
        public JsonResult getcataloguernames_id()
        {
            var cataloguer = _ItemApproveService.getcataloguernames_id();
            return this.Json(cataloguer, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult getRequested_Records()
        {
            var req_records = _ItemApproveService.get_itemsToApprove(Session["userid"].ToString());
            foreach (Prosol_Request prm in req_records)
            {
                // string str = prm.requestedOn.ToLongDateString();
                string str = DateTime.Parse(prm.requestedOn.ToString()).ToString("dd/MM/yyyy");
                // string str1 = str.Substring(0, 6) + " " +str.Substring(str.Length-4);
                prm.requestStatus = str;
            }

            return this.Json(req_records, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult getApproved_Records()
        {
            var app_records = _ItemApproveService.getApproved_Records(Session["userid"].ToString());
            foreach (Prosol_Request prm in app_records)
            {
              //  string str = prm.approvedOn.ToLongDateString();
                string str = DateTime.Parse(prm.approvedOn.ToString()).ToString("dd/MM/yyyy");
               // string str1 = str.Substring(0, 6) + " " + str.Substring(str.Length - 4);
                prm.requestStatus = str;
            }

            return this.Json(app_records, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult getRejected_Records()
        {
            var rej_records = _ItemApproveService.getRejected_Records(Session["userid"].ToString());
            foreach (Prosol_Request prm in rej_records)
            {
                int index = prm.rejectedOn.ToString().IndexOf(" ");

                string str = prm.rejectedOn != null ? prm.rejectedOn.ToString().Substring(0, index) : null;
                prm.requestStatus = str;
                //prm.rejectedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //prm.requestStatus = prm.rejectedOn.ToString().Substring(0, 10);
                //string str = prm.rejectedOn.ToLongDateString();
                //string str = DateTime.Parse(prm.rejectedOn.ToString()).ToString("dd/MM/yyyy");
                // string str1 = str.Substring(0, 6) + " " + str.Substring(str.Length - 4);
                //prm.requestStatus = str;
            }

            return this.Json(rej_records, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult getsingle_requested_record(string abcsony)
        {
            var singlerecord = _ItemApproveService.getsingle_requested_record(abcsony);

            return this.Json(singlerecord,JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult submit_approval()
        {
            var form_data = Request.Form["values"];

            var pro_req = JsonConvert.DeserializeObject<Prosol_Request>(form_data);
            Prosol_UpdatedBy pub = new Prosol_UpdatedBy();
            var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));
            string tmpStr = "";
            foreach (TargetExn ent in usrInfo.Roles)
            {
                if (ent.Name == "Approver") tmpStr = ent.TargetId;
            }
            pro_req.cataloguer = tmpStr;
            if (pro_req.itemStatus == "approved")
            {
              
                var user_deteails = _ItemApproveService.get_cataloguer_emailid(pro_req.cataloguer).ToList();
                pub.UserId = user_deteails[0].Userid;
                pub.Name = user_deteails[0].UserName;
                pub.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                pro_req.approvedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                bool result = _ItemApproveService.submit_approval(pro_req, pub);

                string to_mail = user_deteails[0].EmailId;
                string subject = "";
                // string userid = Data[0].Userid;
                // email email1 = new email();
                // email1.email_to = to_mail;
                //  email1.email_from = "codasol.madras@gmail.com";
                if (Session["username"].ToString() != null)
                    subject = "New Request approved by " + Session["username"].ToString() + " REQUEST ID : " + pro_req.requestId;
                else
                    subject = "New Request approved from Prosol";
                //string body = "<html><body><table><tr><td style='padding-left: 50px;padding-top: 10px;'>Item Code</td><td style='padding-left: 50px;padding-top: 10px;'>" + pro_req.itemId + "</td></tr><tr><td style='padding-left: 50px;padding-top: 10px;'>Source</td><td style='padding-left: 50px;padding-top: 10px;'>" + pro_req.source + "</td></tr></table></body></html>";
                // email1.IsBodyHtml = true;
                // email1.host = "smtp.gmail.com";
                // email1.enablessl = true;
                // email1.UseDefaultCredentials = true;
                //email1.Port = 587;
                // email1.password = "codasolwestmambalam";

                // EmailSettingService ems = new EmailSettingService();
                var tbl = new DataTable();
                tbl.Columns.Add("ItemId");
                tbl.Columns.Add("Legacy");
                tbl.Columns.Add("Storage Location");
                tbl.Columns.Add("Material Type");
                tbl.Columns.Add("Industrial Sector");
                tbl.Columns.Add("Material Group");
              
                    var row = tbl.NewRow();
                    row["ItemId"] = pro_req.itemId;
                    row["Legacy"] = pro_req.source;
                    row["Storage Location"] = pro_req.storage_Location;
                    row["Material Type"] = pro_req.Materialtype;
                    row["Industrial Sector"] = pro_req.Industrysector;
                    row["Material Group"] = pro_req.MaterialStrategicGroup;

                    tbl.Rows.Add(row);




                    bool val = _Emailservc.sendmail(to_mail, subject, _Emailservc.getmailbody(tbl));

                if (result == true)
                {
                    return this.Json("approved", JsonRequestBehavior.AllowGet);
                }
                else
                    return this.Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                pro_req.rejectedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                bool result = _ItemApproveService.submit_approval(pro_req, pub);

                if (result == true)
                {
                    var user_deteails = _ItemApproveService.get_req(pro_req.requester).ToList();
                    string to_mail = user_deteails[0].EmailId;
                    string subject = "Request Rejected from Prosol";
                    //string body = "<html><body><table><tr><td style='padding-left: 50px;padding-top: 10px;'>Request Id</td><td style='padding-left: 50px;padding-top: 10px;'>" + pro_req.requestId + "</td></tr><tr><td style='padding-left: 50px;padding-top: 10px;'>Item Code</td><td style='padding-left: 50px;padding-top: 10px;'>" + pro_req.itemId + "</td></tr><tr><td style='padding-left: 50px;padding-top: 10px;'>Reason for rejection</td><td style='padding-left: 50px;padding-top: 10px;'>" + pro_req.reason_rejection + "</td></tr></table></body></html>";
                    var tbl = new DataTable();
                    tbl.Columns.Add("ItemId");
                    tbl.Columns.Add("Legacy");

                    tbl.Columns.Add("Reason Rejection");

                    var row = tbl.NewRow();
                    row["ItemId"] = pro_req.itemId;
                    row["Legacy"] = pro_req.source;
                  
                    row["Reason Rejection"] = pro_req.reason_rejection;
                   
                    tbl.Rows.Add(row);
                    bool val = _Emailservc.sendmail(to_mail, subject, _Emailservc.getmailbody(tbl));

                    return this.Json("rejected", JsonRequestBehavior.AllowGet);
                }
                else
                    return this.Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        public void Downloadfile(string Itemcode)
        {
            var ListItems = _CatalogueService.GetAttachment(Itemcode).ToList();
            if (ListItems != null && ListItems.Count>0)
            {

                byte[] bytearr = _CatalogueService.Downloadfile(ListItems[0].FileId);

                Response.AddHeader("Content-type", ListItems[0].ContentType);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + ListItems[0].FileName);
                Response.BinaryWrite(bytearr);
                Response.Flush();
                Response.End();
            }
            // return this.Json("", JsonRequestBehavior.AllowGet);
        }
    }
}