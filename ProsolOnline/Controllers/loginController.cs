using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

using Prosol.Core.Interface;
using Prosol.Core.Model;
using System.Web.Security;

namespace ProsolOnline.Controllers
{
    public class loginController : Controller
    {
        private readonly I_login _loginService;
        private readonly IUserAccess _Useraccessservice;
        public loginController(I_login loginService, IUserAccess usracservice)
        {
            _loginService = loginService;
            _Useraccessservice = usracservice;
        }
        // GET: login
        public ActionResult Index()
        {

            //string id = Convert.ToString(Session["userid"]);
            //if (id != "")
            //{
            //    _loginService.UpdateLogOutDate(id);
            //}

            Session["userid"] = null;
           
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            return View("Login");
        }

        public JsonResult checklogin_details(string UserName,string Password)
        {
            var Data = _loginService.checklogin_details(UserName,Password).ToList();
            if (Data.Count > 0)
            {
                if (Data[0].Islive == "Active")
                {

                    if (DateTime.Now < Convert.ToDateTime("01/01/2026"))
                    {
                        FormsAuthentication.SetAuthCookie(Data[0].UserName, false);

                        Session["fullname"] = Data[0].FirstName + " " + Data[0].LastName;
                        Session["userid"] = Data[0].Userid;
                        Session["username"] = Data[0].UserName;
                        Session["islive"] = Data[0].Islive;
                        Session["Modules"] = Data[0].Modules;
                        List<TargetExn> roles = Data[0].Roles;
                        String[] Modules = Data[0].Modules;
                        if(Modules != null)
                        {
                            string Module = "";
                            foreach (string mdl in Modules)
                            {
                                Module = Module + mdl + ",";
                            }
                            Session["Modules"] = Module;
                        }
                       
                        string[] arr = new string[roles.Count];
                        if (roles.Count == 1)
                        {
                            Session["rolecat"] = roles[0].Name == "Cataloguer" ? "cataloguer" : "";
                        }
                        else
                        {
                            Session["rolecat"] = "";
                        }


                        if (Data[0].Lastlogin != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            DateTime dateTime;
                            if (DateTime.TryParse(Data[0].Lastlogin.ToString(), System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out dateTime))
                            {
                                Session["lastlogin"] = Convert.ToDateTime(Data[0].Lastlogin.ToString()).ToString("MMM dd yyyy hh:mm tt");

                            }
                        }
                        _loginService.UpdateLoginDate(Data[0].Userid.ToString());

                        if (roles[0].Name == "SuperAdmin")
                        {
                            Session["access"] = "SuperAdmin";
                            Session["show"] = "true";
                        }
                        else
                        {
                            Session["show"] = "false";
                            string id = Convert.ToString(Session["userid"]);
                            var mx = _Useraccessservice.search(id).ToList();
                            string pages = "";
                            foreach (Prosol_Access mdl in mx)
                            {
                                pages = pages + mdl.Pages + ",";
                            }
                            Session["access"] = pages;

                        }
                        //List<double> value = new List<double>();
                        //var x = _loginService.getIdeal(Data[0].Userid.ToString());
                        //foreach (Prosol_Idealtime y in x)
                        //{
                        //    double a = Convert.ToDateTime(y.Logouttime).Subtract(Convert.ToDateTime(y.Logintime)).TotalSeconds;
                        //    value.Add(a);
                        //}
                        //int total = value.Sum(a => Convert.ToInt32(a));
                        //TimeSpan timespan = TimeSpan.FromSeconds(total);
                        //int min = timespan.Minutes;
                        //Session["Idealtime"] = min + " Min";
                       

                        return Json(new { result = "Active" }, JsonRequestBehavior.AllowGet);
                    }else
                    {
                        return Json(new { result = "Expired" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else return Json(new { result = "InActive" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { result = "Invalid" }, JsonRequestBehavior.AllowGet);
        }
    }
}