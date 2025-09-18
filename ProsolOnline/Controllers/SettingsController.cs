using Prosol.Common;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using ProsolOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Prosol.Core;


namespace ProsolOnline.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IEmailSettings _EmailRepository;
        public SettingsController(IEmailSettings EmailRepository)
        {
            _EmailRepository = EmailRepository;
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
        // GET: Settings
        public ActionResult EmailSettings()
        {
            if (CheckAccess("Email Settings") == 1)
                return View();
            else if (CheckAccess("Email Settings") == 0)
                return View("Denied");
            else return View("Login");
           
        }
        public string SendEmail()
        {
            var obj = Request.Form["obj"];
            EmailSettings Model = JsonConvert.DeserializeObject<EmailSettings>(obj);
            Prosol_EmailSettings mdl = new Prosol_EmailSettings();

            string to_mail = Model.emailtest;

            string subject = "Test Mail";


            string body = "Test Mail";

            string val = _EmailRepository.emailTest(to_mail, subject, body);
            return val;




        }

        public bool SubmitEmail()
        {
            int update = 0;

            var obj = Request.Form["obj"];
            EmailSettings Model = JsonConvert.DeserializeObject<EmailSettings>(obj);

            int i = 1;
            foreach (char str in Model.BCC)
            {
                if (str == ' ' || str == ',' || str == ';')
                {
                    i++;
                }
            }
            var bccc = new string[i];
            i = 1;
            foreach (char str in Model.CC)
            {
                if (str == ' ' || str == ',' || str == ';')
                {
                    i++;
                }
            }
            var cccc = new string[i];
            string temp = "";

            i = 0;
            foreach (char str in Model.BCC)
            {
                if (str != ' ' && str != ',' && str != ';')
                    temp = temp + str;
                if (str == ' ' || str == ',' || str == ';')
                {
                    bccc[i] = temp;
                    temp = "";
                    i++;
                }

            }
            if (temp != "")
            {
                bccc[i] = temp;
            }
            //if(Model.BCC != "" && bccc != null)
            //{
            //    bccc[0] = temp;
            //}


            int j = 0;
            temp = "";
            foreach (char str in Model.CC)
            {
                if (str != ' ' && str != ',' && str != ';')
                    temp = temp + str;

                if (str == ' ' || str == ',' || str == ';')
                {
                    cccc[j] = temp;
                    temp = "";
                    j++;
                }
            }
            if (temp != "")
            {
                cccc[j] = temp;
            }


            Prosol_EmailSettings mdl = new Prosol_EmailSettings();
            if (Model._id != null && Model._id != "undefined")
            {
                update = 1;
                mdl._id = new MongoDB.Bson.ObjectId(Model._id);
            }


            mdl.FromId = Model.FromId;
            mdl.Password = Model.Password;
            mdl.ConformPassword = Model.ConformPassword;
            mdl.SMTPServerName = Model.SMTPServerName;
            mdl.PortNo = Model.PortNo;
            mdl.EnableSSL = Model.EnableSSL;
            mdl.CCC = cccc;
            mdl.BCCC = bccc;
            var getresult = _EmailRepository.insertemaildata(mdl, update);
            return getresult;



        }


        public JsonResult ShowEmaildata()
        {
            var objList = _EmailRepository.ShowEmaildata();

            {
                var obj = new EmailSettings();

                obj._id = objList._id.ToString();
                obj.FromId = objList.FromId;
                obj.Password = objList.Password;
                obj.ConformPassword = objList.ConformPassword;
                obj.SMTPServerName = objList.SMTPServerName;
                obj.PortNo = objList.PortNo;
                obj.EnableSSL = objList.EnableSSL;
                string strrrr = "";
                if (objList.CCC != null)
                {

                    foreach (string str in objList.CCC)
                    {
                        if (strrrr == "")
                        {
                            strrrr = strrrr + str;
                        }
                        else
                        {
                            strrrr = strrrr + "," + str;
                        }
                    }
                }
                obj.CC = strrrr;
                strrrr = "";
                if (objList.BCCC != null)
                {

                    foreach (string str in objList.BCCC)
                    {
                        if (strrrr == "")
                        {
                            strrrr = strrrr + str;
                        }
                        else
                        {
                            strrrr = strrrr + "," + str;
                        }
                    }
                }


                obj.BCC = strrrr;
                // obj.CODELOGIC = objList.CODELOGIC;
                //obj.Version = objList.unspsc_Version;
                return this.Json(obj, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult AutoCompleteEmailId(string term)
        {
            var arrStr = _EmailRepository.AutoCompleteEmailId(term);
            return this.Json(arrStr, JsonRequestBehavior.AllowGet);

        }



    }
}