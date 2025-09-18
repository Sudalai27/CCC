using Prosol.Core.Interface;
using Prosol.Core.Model;
using ProsolOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Globalization;

namespace ProsolOnline.Controllers
{
    public class DashboardController : Controller
    {

        private readonly IDashboard _DashboardService;
        private readonly IUserCreate _Usercreateservice;
        public DashboardController(IDashboard DashboardService, IUserCreate usrservice)
        {
            _DashboardService = DashboardService;
            _Usercreateservice = usrservice;
        }
        // GET: Dashboard
        //[Authorize]
        //public ActionResult Index()
        //{
        //    string pages = Convert.ToString(Session["access"]);
        //    if (!string.IsNullOrEmpty(pages))
        //    {


        //        return View("Index");
        //    }
        //    else return View("Login");
        //}
        [Authorize]
        public ActionResult HomeIndex()
        {
            string pages = Convert.ToString(Session["access"]);
            if (!string.IsNullOrEmpty(pages))
            {

                ViewBag.P02_C12 = "nav-active";
                return View();
            }
            else return View("Login");
        }
        [Authorize]
        public ActionResult Index()
        {
            if (CheckAccess("Dashboard") == 1)
                return View("index");
            else if (CheckAccess("Dashboard") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult Guideline()
        {
            if (CheckAccess("Guide") == 1)
                return View("Guideline");
            else if (CheckAccess("Guide") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult Docs()
        {
            if (CheckAccess("Guide") == 1)
                return View("Docs");
            else if (CheckAccess("Guide") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult Dashboard1()
        {
                return View("Dashboard1");

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
        public PartialViewResult GetMenu()
        {
            setAccess();
            return PartialView("~/Views/Shared/mymenu.cshtml");
        }
        private void setAccess()
        {

          
            ViewBag.P1 = "none";
            ViewBag.P2 = "none";
            ViewBag.P3 = "none";
            ViewBag.P4 = "none";
            ViewBag.P5 = "none";
            ViewBag.P6 = "none";
            ViewBag.P7 = "none";
            ViewBag.P8 = "none";
            ViewBag.P9 = "none";
          

            ViewBag.p1c1 = "none";
            ViewBag.p1c2 = "none";
            ViewBag.p1c3 = "none";

            ViewBag.p2c1 = "pointer-events:none;color:#ccc;";
            ViewBag.p2c2 = "pointer-events:none;color:#ccc;";

            ViewBag.p3c1 = "pointer-events:none;color:#ccc;";
            ViewBag.p3c2 = "pointer-events:none;color:#ccc;";
            ViewBag.p3c3 = "pointer-events:none;color:#ccc;";

            ViewBag.p4c1 = "pointer-events:none;color:#ccc;";
            ViewBag.p4c2 = "pointer-events:none;color:#ccc;";
            ViewBag.p4c3 = "pointer-events:none;color:#ccc;";
            ViewBag.p4c4 = "pointer-events:none;color:#ccc;";
            ViewBag.p4c5 = "pointer-events:none;color:#ccc;";
            ViewBag.p4c6 = "pointer-events:none;color:#ccc;";

            ViewBag.p5c1 = "pointer-events:none;color:#ccc;";
            ViewBag.p5c2 = "pointer-events:none;color:#ccc;";
            ViewBag.p5c3 = "pointer-events:none;color:#ccc;";
            ViewBag.p5c4 = "pointer-events:none;color:#ccc;";
            ViewBag.p5c5 = "pointer-events:none;color:#ccc;";
            ViewBag.p5c6 = "pointer-events:none;color:#ccc;";
            ViewBag.p5c7 = "pointer-events:none;color:#ccc;";
            ViewBag.p5c8 = "pointer-events:none;color:#ccc;";
            ViewBag.p5c9 = "pointer-events:none;color:#ccc;";
            ViewBag.p5c10 = "pointer-events:none;color:#ccc;";
            ViewBag.p5c11 = "pointer-events:none;color:#ccc;";
            ViewBag.p5c12 = "pointer-events:none;color:#ccc;";

            ViewBag.p6c1 = "pointer-events:none;color:#ccc;";
            ViewBag.p6c2 = "pointer-events:none;color:#ccc;";
            ViewBag.p6c3 = "pointer-events:none;color:#ccc;";
            ViewBag.p6c4 = "pointer-events:none;color:#ccc;";
            ViewBag.p6c5 = "pointer-events:none;color:#ccc;";
            ViewBag.p6c6 = "pointer-events:none;color:#ccc;";

          
           

            // ViewBag.p7c1 = "none";
            //ViewBag.p7c2 = "none";

            ViewBag.p8c1 = "none";
            ViewBag.p8c2 = "none";

            ViewBag.p9c1 = "pointer-events:none;color:#ccc;";
            ViewBag.p9c2 = "pointer-events:none;color:#ccc;";

            //ViewBag.p2c1 = "none";
            //ViewBag.p2c2 = "none";

            //ViewBag.p3c1 = "none";
            //ViewBag.p3c2 = "none";
            //ViewBag.p3c3 = "none";

            //ViewBag.p4c1 = "none";
            //ViewBag.p4c2 = "none";
            //ViewBag.p4c3 = "none";
            //ViewBag.p4c4 = "none";
            //ViewBag.p4c5 = "none";

            //ViewBag.p5c1 = "none";
            //ViewBag.p5c2 = "none";
            //ViewBag.p5c3 = "none";
            //ViewBag.p5c4 = "none";
            //ViewBag.p5c5 = "none";
            //ViewBag.p5c6 = "none";
            //ViewBag.p5c7 = "none";
            //ViewBag.p5c8 = "none";
            //ViewBag.p5c9 = "none";
            //ViewBag.p5c10 = "none";
            //ViewBag.p5c11 = "none";
            //ViewBag.p5c12 = "none";

            //ViewBag.p6c1 = "none";
            //ViewBag.p6c2 = "none";
            //ViewBag.p6c3 = "none";
            //ViewBag.p6c4 = "none";
            //ViewBag.p6c5 = "none";
            //ViewBag.p6c6 = "none";


            // ViewBag.p7c1 = "none";
            //ViewBag.p7c2 = "none";

            //ViewBag.p8c1 = "none";
            //ViewBag.p8c2 = "none";

            //ViewBag.p9c1 = "none";
            //ViewBag.p9c2 = "none";


            //ViewBag.q1 = "block";

            //ViewBag.q1a1 = "none";
            //ViewBag.q1a2 = "none";
            //ViewBag.q1a3 = "none";
            //ViewBag.q1a4 = "none";
            //ViewBag.q1a5 = "none";
            //ViewBag.q1a6 = "none";
            //ViewBag.q1a7 = "none";
            //ViewBag.q1a8 = "none";

            //servicemaster

            ViewBag.q1 = "none";

            ViewBag.q1a1 = "none";
            ViewBag.q1a2 = "none";
            ViewBag.q1a3 = "none";
            //ViewBag.q1a4 = "none";
            //ViewBag.q1a5 = "none";
            //ViewBag.q1a6 = "none";
            ViewBag.q1a7 = "none";
            ViewBag.q1a8 = "none";
            ViewBag.q1a9 = "none";
            ViewBag.q1y1 = "none";


            ViewBag.q2 = "none";

            ViewBag.q2a4 = "none";
            ViewBag.q2a5 = "none";
            ViewBag.q2a6 = "none";
            ViewBag.q2a7 = "none";

            //guide

            ViewBag.P01 = "none";
            ViewBag.P01_C11 = "none";
            ViewBag.P01_C12 = "none";
            ViewBag.P01_C13 = "none";
            ViewBag.P01_C14 = "none";
            ViewBag.P02 = "none";
            ViewBag.P02_C11 = "none";
            //equpmt
            ViewBag.z10 = "none";
            //ViewBag.z1c1 = "none";
            //ViewBag.z1c2 = "none";
            ViewBag.z1c1 = "pointer-events:none;color:#ccc;";
            ViewBag.z1c2 = "pointer-events:none;color:#ccc;";
            //BOM
            ViewBag.q8 = "none";
            ViewBag.q8c1 = "none";
            ViewBag.q8c2 = "none";
            ViewBag.q8c3 = "none";
            ViewBag.q8c4 = "none";
            ViewBag.q8c5 = "none";
            ViewBag.q8c6 = "none";
            ViewBag.q8c7 = "none";
            //asset
            ViewBag.A8 = "none";
            ViewBag.A8c1 = "none";
            ViewBag.A8c2 = "none";
            ViewBag.A8c3 = "none";
            ViewBag.A8c4 = "none";
            ViewBag.A8c5 = "none";
            ViewBag.A8c6 = "none";
            ViewBag.A8c7 = "none";
            ViewBag.A8c8 = "none";
            ViewBag.A8c9 = "none";

            //BP
            ViewBag.x1 = "none";
            ViewBag.x1c1 = "none";
            ViewBag.x1c2 = "none";
            ViewBag.x1c3 = "none";
            ViewBag.x1c4 = "none";

            ViewBag.P1_C1W = "cursor: not-allowed";
            ViewBag.P1_C3W = "cursor: not-allowed";
            ViewBag.P3_C1W = "cursor: not-allowed";
            ViewBag.P3_C3W = "cursor: not-allowed";
            ViewBag.P4_C1W = "cursor: not-allowed";
            ViewBag.P4_C2W = "cursor: not-allowed";
            ViewBag.P4_C3W = "cursor: not-allowed";
            ViewBag.P4_C4W = "cursor: not-allowed";
            ViewBag.P4_C5W = "cursor: not-allowed";
            ViewBag.P4_C6W = "cursor: not-allowed";
            ViewBag.P5_C1W = "cursor: not-allowed";
            ViewBag.P5_C2W = "cursor: not-allowed";
            ViewBag.P5_C3W = "cursor: not-allowed";
            ViewBag.P5_C4W = "cursor: not-allowed";
            ViewBag.P5_C5W = "cursor: not-allowed";
            ViewBag.P5_C6W = "cursor: not-allowed";
            ViewBag.P5_C7W = "cursor: not-allowed";
            ViewBag.P5_C8W = "cursor: not-allowed";
            ViewBag.P5_C9W = "cursor: not-allowed";
            ViewBag.P5_C10W = "cursor: not-allowed";
            ViewBag.P5_C11W = "cursor: not-allowed";
            ViewBag.P5_C12W = "cursor: not-allowed";
            ViewBag.P6_C1W = "cursor: not-allowed";
            ViewBag.P6_C2W = "cursor: not-allowed";
            ViewBag.P6_C3W = "cursor: not-allowed";
            ViewBag.P6_C4W = "cursor: not-allowed";
            ViewBag.P6_C5W = "cursor: not-allowed";
            ViewBag.P6_C6W = "cursor: not-allowed";
            ViewBag.P9_C1W = "cursor: not-allowed";
            ViewBag.P9_C2W = "cursor: not-allowed";
            ViewBag.Z1_C1W = "cursor: not-allowed";
            ViewBag.Z1_C2W = "cursor: not-allowed";
            string pth = Request.Url.AbsoluteUri.ToString().ToLower();
            string pages = Convert.ToString(Session["access"]);
            string Modules = Convert.ToString(Session["Modules"]);
            String[] stringArray = pages.Split(',');
            String[] ModulesArray = Modules.Split(',');
            if (stringArray.Length > 0)
            {
               // Material
                if (Array.IndexOf(ModulesArray, "Material") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                {
                    if (pth.Contains("dashboard/index"))
                    {
                        ViewBag.t10 = "active";
                        ViewBag.P0 = "block";
                        ViewBag.P0_C1 = "nav-active";
                    }
                    if (pth.Contains("dashboard") && !pth.Contains("dashboard/homeindex"))
                    {
                        ViewBag.t10 = "active";
                        ViewBag.P0 = "block";
                        ViewBag.P0_C1 = "nav-active";
                    }

                    if (Array.IndexOf(stringArray, "Import") > -1)
                    {
                        ViewBag.P1 = "block";
                        ViewBag.p1c1 = "block";
                        if (pth.Contains("file/index") || pth.Contains("file/import") || (pth.EndsWith("file") && !pth.EndsWith("myprofile")))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p10 = "nav-active";
                            ViewBag.P1_C1 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "Export") > -1)
                    {
                        ViewBag.P9 = "block";
                        ViewBag.p9c2 = "block";
                        if (pth.Contains("report/export"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p90 = "nav-active";
                            ViewBag.P9_C2 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "Assign Work") > -1)
                    {
                        ViewBag.P1 = "block";
                        ViewBag.p1c3 = "block";
                        if (pth.Contains("file/assignwork"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p10 = "nav-active";
                            ViewBag.P1_C3 = "nav-active";
                        }
                    }

                    //Search
                    if (Array.IndexOf(stringArray, "Search by REF") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P2 = "block";
                        ViewBag.p2c1 = "block";
                        if (pth.Contains("searchbyreference/index") || (pth.Contains("searchbyreference/searchbyref") || pth.EndsWith("searchbyreference")))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p20 = "nav-active";
                            ViewBag.P2_C1 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "Master Search") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P2 = "block";
                        ViewBag.p2c2 = "block";
                        if (pth.Contains("search/index") || (pth.Contains("search/searchbydesc")))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p20 = "nav-active";
                            ViewBag.P2_C2 = "nav-active";
                        }
                    }
                    //New Items
                    if (Array.IndexOf(stringArray, "Request Item") > -1)
                    {
                        ViewBag.P3 = "block";
                        ViewBag.p3c1 = "block";
                        if (pth.Contains("materialRequest/index") || pth.EndsWith("materialrequest"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p30 = "nav-active";
                            ViewBag.P3_C1 = "nav-active";
                            //ViewBag.t10 = "nav-expanded";

                        }
                    }
                    if (Array.IndexOf(stringArray, "Request Log") > -1)
                    {
                        ViewBag.P3 = "block";
                        ViewBag.p3c2 = "block";
                        if (pth.Contains("requestlog/index") || pth.EndsWith("requestlog"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p30 = "nav-active";
                            ViewBag.P3_C2 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "Approve Item") > -1)
                    {
                        ViewBag.P3 = "block";
                        ViewBag.p3c3 = "block";
                        if (pth.Contains("approver/index") || pth.EndsWith("approver"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p30 = "nav-active";
                            ViewBag.P3_C3 = "nav-active";
                        }
                    }

                    if (Array.IndexOf(stringArray, "New Equipment") > -1)
                    {
                        ViewBag.Z7 = "block";
                        ViewBag.z1c1 = "block";
                        if (pth.Contains("equipment/equipments"))
                        {
                            ViewBag.z10 = "nav-expanded";
                            ViewBag.t10 = "nav-expanded";
                            ViewBag.Z1_C1 = "nav-active";


                        }
                    }
                    if (Array.IndexOf(stringArray, "Search") > -1)
                    {
                        ViewBag.Z7 = "block";
                        ViewBag.z1c2 = "block";
                        if (pth.Contains("equipment/srchequipment"))
                        {
                            ViewBag.z10 = "nav-expanded";
                            ViewBag.t10 = "nav-expanded";
                            ViewBag.Z1_C2 = "nav-active";

                        }
                    }

                    //Catalogue
                    if (Array.IndexOf(stringArray, "Catalogue") > -1)
                    {
                        ViewBag.P4 = "block";
                        ViewBag.p4c1 = "block";
                        if (pth.Contains("catalogue/index") || pth.EndsWith("catalogue"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p40 = "nav-active";
                            ViewBag.P4_C1 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "Review") > -1)
                    {
                        ViewBag.P4 = "block";
                        ViewBag.p4c2 = "block";
                        if (pth.Contains("catalogue/review"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p40 = "nav-active";
                            ViewBag.P4_C2 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "Release") > -1)
                    {
                        ViewBag.P4 = "block";
                        ViewBag.p4c3 = "block";
                        if (pth.Contains("catalogue/release"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p40 = "nav-active";
                            ViewBag.P4_C3 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "BulkUpload") > -1)
                    {
                        ViewBag.P4 = "block";
                        ViewBag.p4c4 = "block";
                        if (pth.Contains("catalogue/bulk"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p40 = "nav-active";
                            ViewBag.P4_C4 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "PV Data") > -1)
                    {
                        ViewBag.P4 = "block";
                        ViewBag.p4c5 = "block";
                        if (pth.Contains("catalogue/pvdata"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p40 = "nav-active";
                           
                            ViewBag.P4_C5 = "nav-active";
                        }
                    }
                    //Tools

                    if (Array.IndexOf(stringArray, "General") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P5 = "block";
                        ViewBag.p5c1 = "block";
                        if (pth.Contains("general/general") || pth.Contains("general/index") || pth.EndsWith("general"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p50 = "nav-active";
                            ViewBag.P5_C1 = "nav-active";

                        }

                    }
                    if (Array.IndexOf(stringArray, "Plant") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P5 = "block";
                        ViewBag.p5c2 = "block";
                        if (pth.Contains("master/plant"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p50 = "nav-active";
                            ViewBag.P5_C2 = "nav-active";

                        }
                    }
                    if (Array.IndexOf(stringArray, "Mrp data") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P5 = "block";
                        ViewBag.p5c3 = "block";
                        if (pth.Contains("mrp/index") || pth.Contains("mrp/mrp") || pth.EndsWith("mrp"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p50 = "nav-active";
                            ViewBag.P5_C3 = "nav-active";

                        }

                    }
                    if (Array.IndexOf(stringArray, "Sales & Others") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P5 = "block";
                        ViewBag.p5c4 = "block";
                        if (pth.Contains("master/index") || pth.Contains("master/sales") || pth.EndsWith("master"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p50 = "nav-active";
                            ViewBag.P5_C4 = "nav-active";

                        }
                    }

                    if (Array.IndexOf(stringArray, "General Settings") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P5 = "block";
                        ViewBag.p5c6 = "block";
                        if (pth.Contains("generalsettings/index") || pth.EndsWith("generalsettings"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p50 = "nav-active";
                            ViewBag.P5_C6 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "Sequence Master") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P5 = "block";
                        ViewBag.p5c7 = "block";
                        if (pth.Contains("sequence/index") || pth.EndsWith("sequence"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p50 = "nav-active";
                            ViewBag.P5_C7 = "nav-active";

                        }
                    }
                    if (Array.IndexOf(stringArray, "UOM Settings") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P5 = "block";
                        ViewBag.p5c8 = "block";
                        if (pth.Contains("sequence/uom"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p50 = "nav-active";
                            ViewBag.P5_C8 = "nav-active";

                        }
                    }
                    //if (Array.IndexOf(stringArray, "Add Logics") > -1)
                    //{
                    //    ViewBag.P5 = "block";
                    //    ViewBag.p5c9 = "block";
                    //    if (pth.Contains("generalsettings/logics"))
                    //    {
                    //        ViewBag.p50 = "nav-expanded";
                    //        ViewBag.P5_C9 = "nav-active";

                    //    }
                    //}
                    if (Array.IndexOf(stringArray, "UNSPSC Master") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P5 = "block";
                        ViewBag.p5c10 = "block";
                        if (pth.Contains("generalsettings/unspsc"))
                        {
                            ViewBag.p50 = "nav-active";
                            ViewBag.P5_C10 = "nav-active";
                            ViewBag.t10 = "active";
                        }
                    }

                    if (Array.IndexOf(stringArray, "Value Standardization") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P5 = "block";
                        ViewBag.p5c11 = "block";
                        if (pth.Contains("valueStandardisation/index") || pth.EndsWith("valuestandardisation"))
                        {
                            ViewBag.p50 = "nav-active";
                            ViewBag.P7_C11 = "nav-active";
                            ViewBag.t10 = "active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "Email Settings") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P5 = "block";
                        ViewBag.p5c12 = "block";
                        if (pth.Contains("settings/emailsettings") || pth.EndsWith("emailsettings"))
                        {
                            ViewBag.p50 = "nav-active";
                            ViewBag.P5_C12 = "nav-active";
                            ViewBag.t10 = "active";
                        }
                    }
                    //Dictionary

                    if (Array.IndexOf(stringArray, "View") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P6 = "block";
                        ViewBag.p6c1 = "block";
                        if (pth.Contains("dictionary/view"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p60 = "nav-active";
                            ViewBag.P6_C1 = "nav-active";

                        }
                    }
                    if (Array.IndexOf(stringArray, "Create") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P6 = "block";
                        ViewBag.p6c2 = "block";
                        if (pth.Contains("dictionary/index") || pth.Contains("dictionary/add") || pth.EndsWith("dictionary"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p60 = "nav-active";
                            ViewBag.P6_C2 = "nav-active";

                        }
                    }
                    if (Array.IndexOf(stringArray, "Characteristics") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P6 = "block";
                        ViewBag.p6c3 = "block";
                        if (pth.Contains("dictionary/characteristics"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p60 = "nav-active";
                            ViewBag.P6_C3 = "nav-active";

                        }
                    }
                    if (Array.IndexOf(stringArray, "ValueMaster") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P6 = "block";
                        ViewBag.p6c4 = "block";
                        if (pth.Contains("dictionary/values"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p60 = "nav-active";
                            ViewBag.P6_C4 = "nav-active";

                        }
                    }
                    if (Array.IndexOf(stringArray, "Code Logic") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P6 = "block";
                        ViewBag.p6c5 = "block";
                        if (pth.Contains("dictionary/codelogic"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p60 = "nav-active";
                            ViewBag.P6_C5 = "nav-active";

                        }
                    }
                    if (Array.IndexOf(stringArray, "Logic") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P6 = "block";
                        ViewBag.p6c6 = "block";
                        if (pth.Contains("dictionary/logic"))
                        {
                            ViewBag.t10 = "active";
                            ViewBag.p60 = "nav-active";
                            ViewBag.P6_C6 = "nav-active";

                        }
                    }
                    if (Array.IndexOf(stringArray, "Tracking") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P9 = "block";
                        ViewBag.p9c1 = "block";
                        if (pth.Contains("report/tracking"))
                        {
                            ViewBag.p90 = "nav-active";
                            ViewBag.P9_C1 = "nav-active";
                            ViewBag.t10 = "active";
                        }
                    }
                    //Prosol peak

                    //if (Array.IndexOf(stringArray, "Value Standardization") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    //{
                    //    ViewBag.P7 = "block";
                    //    ViewBag.p7c1 = "block";
                    //    if (pth.Contains("valueStandardisation/index") || pth.EndsWith("valuestandardisation"))
                    //    {
                    //        ViewBag.p70 = "nav-expanded";
                    //        ViewBag.P7_C1 = "nav-active";

                    //    }
                    //}
                    //if (Array.IndexOf(stringArray, "Repository cleansing") > -1)
                    //{
                    //    ViewBag.P7 = "block";
                    //    ViewBag.p7c2 = "block";
                    //}

                    //Users

                    if (Array.IndexOf(stringArray, "User") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P8 = "block";
                        ViewBag.p8c1 = "block";
                        if (pth.Contains("user/index") || pth.Contains("user/update") || pth.EndsWith("user"))
                        {

                            ViewBag.p80 = "active";
                            ViewBag.P8_C1 = "nav-active";

                        }
                    }
                    if (Array.IndexOf(stringArray, "Access permissions") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.P8 = "block";
                        ViewBag.p8c2 = "block";
                        if (pth.Contains("user/access"))
                        {
                            ViewBag.p80 = "active";
                            ViewBag.P8_C2 = "nav-active";

                        }
                    }
                   

                }



                if (Array.IndexOf(ModulesArray, "Service") > -1)
                {
                    //service master

                    if (Array.IndexOf(stringArray, "Service Creation") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.q2 = "block";
                        ViewBag.q2a4 = "block";
                        if (pth.Contains("servicemaster/servicecreation"))
                        {
                            ViewBag.q10 = "active";
                            ViewBag.q11 = "nav-active";
                            ViewBag.q2_a4 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "Request Service") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.q1 = "block";
                        ViewBag.q1a2 = "block";
                        if (pth.Contains("servicemaster/requestservice"))
                        {
                            ViewBag.q10 = "active";
                            ViewBag.q1_a2 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "Approve Service") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.q1 = "block";
                        ViewBag.q1a9 = "block";
                        if (pth.Contains("servicemaster/ApproveService"))
                        {
                            ViewBag.q10 = "active";
                            ViewBag.q1_a9 = "nav-active";
                        }
                    }

                    if (Array.IndexOf(stringArray, "Service Review") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.q2 = "block";
                        ViewBag.q2a5 = "block";
                        if (pth.Contains("servicereview/index") || pth.Contains("servicereview"))
                        {
                            ViewBag.q10 = "active";
                            ViewBag.q11 = "nav-active";
                            ViewBag.q2_a5 = "nav-active";

                        }
                    }
                    if (Array.IndexOf(stringArray, "Service Release") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.q2 = "block";
                        ViewBag.q2a6 = "block";
                        if (pth.Contains("servicerelease/index") || pth.Contains("servicerelease"))
                        {
                            ViewBag.q10 = "active";
                            ViewBag.q11 = "nav-active";
                            ViewBag.q2_a6 = "nav-active";

                        }
                    }
                    if (Array.IndexOf(stringArray, "Service Mapping") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.q2 = "block";
                        ViewBag.q2a7 = "block";
                        if (pth.Contains("servicemaster/servicemapping") || pth.Contains("servicemapping"))
                        {
                            ViewBag.q10 = "active";
                            ViewBag.q11 = "nav-active";
                            ViewBag.q2_a7 = "nav-active";

                        }
                    }
                    if (Array.IndexOf(stringArray, "Service Master") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.q1 = "block";
                        ViewBag.q1a7 = "block";
                        if (pth.Contains("servicemaster/create"))
                        {
                            ViewBag.q10 = "active";
                            ViewBag.q1_a7 = "nav-active";

                        }
                    }
                    if (Array.IndexOf(stringArray, "Service Report") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.q1 = "block";
                        ViewBag.q1a8 = "block";
                        if (pth.Contains("servicereport/index") || pth.Contains("servicereport"))
                        {
                            ViewBag.q10 = "active";
                            ViewBag.q1_a8 = "nav-active";

                        }
                    }
                    if (Array.IndexOf(stringArray, "Service Search") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.q1 = "block";
                        ViewBag.q1a1 = "block";
                        if (pth.Contains("servicesearch/servicesearch"))
                        {
                            ViewBag.q10 = "active";
                            ViewBag.q1_a1 = "nav-active";

                        }
                    }

                    if (Array.IndexOf(stringArray, "Service Dashboard") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.q1 = "block";
                        ViewBag.q1y1 = "block";
                        if (pth.Contains("servicesearch/servicedashboard"))
                        {
                            ViewBag.q10 = "active";
                            ViewBag.q1_y1 = "nav-active";
                            ViewBag.t10 = "block";
                        }
                    }

                }
                if (Array.IndexOf(ModulesArray, "BOM") > -1)
                {

                    //bom
                    if (Array.IndexOf(stringArray, "Equipment Bom") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.q8 = "block";
                        ViewBag.q8c1 = "block";
                        if (pth.Contains("bom/equipbom"))
                        {
                            ViewBag.q80 = "active";
                            ViewBag.q8_c1 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "Material Bom") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.q8 = "block";
                        ViewBag.q8c2 = "block";
                        if (pth.Contains("bom/matbom"))
                        {
                            ViewBag.q80 = "active";
                            ViewBag.q8_c2 = "nav-active";
                        }
                    }

                    if (Array.IndexOf(stringArray, "Dashboard") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.q8 = "block";
                        ViewBag.q8c6 = "block";
                        if (pth.Contains("bom/bomdashboard"))
                        {
                            ViewBag.q80 = "active";
                            ViewBag.q8_c6 = "nav-active";
                            ViewBag.t10 = "block";
                        }
                    }
                    if (Array.IndexOf(stringArray, "Hierarchy Bom") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.q8 = "block";
                        ViewBag.q8c4 = "block";
                        if (pth.Contains("bom/bomtracking"))
                        {
                            ViewBag.q80 = "active";
                            ViewBag.q8_c4 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "Bom Report") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.q8 = "block";
                        ViewBag.q8c5 = "block";
                        if (pth.Contains("bom/bomreport"))
                        {
                            ViewBag.q80 = "active";
                            ViewBag.q8_c5 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "Equipment Master") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.A8 = "block";
                        ViewBag.A8c7 = "block";
                        if (pth.Contains("bom/masterbom"))
                        {
                            ViewBag.A80 = "active";
                            ViewBag.A8_c7 = "nav-active";
                        }

                    }

                }

        
                    if (Array.IndexOf(ModulesArray, "Asset") > -1)
                    {

                    //Asset
                    if (Array.IndexOf(stringArray, "CSIR") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.q8 = "block";
                        ViewBag.q8c7 = "block";
                        if (pth.Contains("bom/csir"))
                        {
                            ViewBag.q80 = "active";
                            ViewBag.q8_c7 = "nav-active";
                        }
                    }

                    if (Array.IndexOf(stringArray, "Functional Location") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.A8 = "block";
                        ViewBag.A8c6 = "block";
                        if (pth.Contains("asset/functloc"))
                        {
                            ViewBag.A80 = "active";
                            ViewBag.A8_c6 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "Equipment") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.A8 = "block";
                        ViewBag.A8c3 = "block";
                        if (pth.Contains("asset/equip"))
                        {
                            ViewBag.A80 = "active";
                            ViewBag.A8_c3 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "Asset Hierarchy") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.A8 = "block";
                        ViewBag.A8c4 = "block";
                        if (pth.Contains("asset/assethie"))
                        {
                            ViewBag.A80 = "active";
                            ViewBag.A8_c4 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "MCP") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.A8 = "block";
                        ViewBag.A8c5 = "block";
                        if (pth.Contains("asset/mcp"))
                        {
                            ViewBag.A80 = "active";
                            ViewBag.A8_c5 = "nav-active";
                        }
                    }

                    if (Array.IndexOf(stringArray, "TaskList") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.A8 = "block";
                        ViewBag.A8c8 = "block";
                        if (pth.Contains("tasklist/tasklist"))
                        {
                            ViewBag.A80 = "active";
                            ViewBag.A8_c8 = "nav-active";
                        }
                    }
                    if (Array.IndexOf(stringArray, "MP") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                        ViewBag.A8 = "block";
                        ViewBag.A8c9 = "block";
                        if (pth.Contains("asset/mp"))
                        {
                            ViewBag.A80 = "active";
                            ViewBag.A8_c9 = "nav-active";
                        }
                    }

                }
                if (Array.IndexOf(ModulesArray, "Vendor") > -1)
                {
                    //vendor
                    if (Array.IndexOf(stringArray, "Vendor Details") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                    {
                    //    ViewBag.P5 = "block";
                        ViewBag.p5c5 = "block";
                        if (pth.Contains("generalsettings/vendor"))
                        {
                       //     ViewBag.t10 = "nav-expanded";
                          //  ViewBag.p50 = "nav-expanded";
                            ViewBag.P5_C5 = "active";

                        }
                    }
                }
                //Guide
                if (Array.IndexOf(stringArray, "Guide") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                {
                    ViewBag.P01 = "block";
                    if (pth.Contains("dashboard/guideline"))
                    {
                        ViewBag.p80 = "active";
                        ViewBag.P01_C11 = "nav-active";
                        ViewBag.t10 = "block";

                    }
                }
                // Sample Docs
                if (Array.IndexOf(stringArray, "Guide") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                {
                    ViewBag.P02 = "block";
                    if (pth.Contains("dashboard/docs"))
                    {
                        ViewBag.p80 = "active";
                        ViewBag.P02_C11 = "nav-active";
                        ViewBag.t10 = "block";

                    }
                }
                //BP
                if (Array.IndexOf(stringArray, "BP-Creation") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                {
                    ViewBag.x1 = "block";
                    ViewBag.x1c1 = "block";
                    if (pth.Contains("businesspartner/creation"))
                    {
                        ViewBag.x1_1 = "active";
                        ViewBag.x1_c1 = "nav-active";
                    }
                }
                if (Array.IndexOf(stringArray, "BP-General") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                {
                    ViewBag.x1 = "block";
                    ViewBag.x1c2 = "block";
                    if (pth.Contains("businesspartner/general"))
                    {
                        ViewBag.x1_1 = "active";
                        ViewBag.x1_c2 = "nav-active";
                    }
                }

                if (Array.IndexOf(stringArray, "BP-Customer") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                {
                    ViewBag.x1 = "block";
                    ViewBag.x1c4 = "block";
                    if (pth.Contains("businesspartner/cust"))
                    {
                        ViewBag.x1_1 = "active";
                        ViewBag.x1_c4 = "nav-active";
                    }
                }
                if (Array.IndexOf(stringArray, "BP-Vendor") > -1 || Array.IndexOf(stringArray, "SuperAdmin") > -1)
                {
                    ViewBag.x1 = "block";
                    ViewBag.x1c3 = "block";
                    if (pth.Contains("businesspartner/ven"))
                    {
                        ViewBag.x1_1 = "active";
                        ViewBag.x1_c3 = "nav-active";
                    }
                }
            }

        }

        [Authorize]
        public JsonResult BindTotalItem()
        {
            var usrInfo = _Usercreateservice.getimage(Convert.ToString(Session["UserId"]));
            var ListDash = _DashboardService.BindTotalItem(usrInfo.Plantcode);
            try
            {

                foreach (Prosol_Dashboard dash in ListDash)
                {
                    if (dash.DataList != null)
                    {
                        var q = (from i in dash.DataList
                                 orderby i.UpdatedOn
                                 group i by DateTime.Parse(i.UpdatedOn.ToString()).ToString("yyyy-MM") into grp
                                 select new { Month = grp.Key, Count = grp.Count() }).ToList();
                        dash.DataList = new List<Prosol_Datamaster>();
                        for (int i = 0; i < q.Count; i++)
                        {
                            Prosol_Datamaster mdl = new Prosol_Datamaster();
                            mdl.Itemcode = q[i].Month;
                            mdl.ItemStatus = q[i].Count;
                            dash.DataList.Add(mdl);
                        }
                    }
                    if (dash.VendorDataList != null)
                    {
                        var q = (from i in dash.VendorDataList
                                 orderby i.UpdatedOn
                                 group i by DateTime.Parse(i.UpdatedOn.ToString()).ToString("yyyy-MM") into grp
                                 select new { Month = grp.Key, Count = grp.Count() }).ToList();
                        dash.VendorDataList = new List<Prosol_Vendor>();
                        for (int i = 0; i < q.Count; i++)
                        {
                            Prosol_Vendor mdl = new Prosol_Vendor();
                            mdl.Code = q[i].Month;
                            mdl.Name = q[i].Count.ToString();
                            dash.VendorDataList.Add(mdl);
                        }
                    }
                    if (dash.NMDataList != null)
                    {
                        var q = (from i in dash.NMDataList
                                 orderby i.UpdatedOn
                                 group i by DateTime.Parse(i.UpdatedOn.ToString()).ToString("yyyy-MM") into grp
                                 select new { Month = grp.Key, Count = grp.Count() }).ToList();
                        dash.NMDataList = new List<Prosol_NounModifiers>();
                        for (int i = 0; i < q.Count; i++)
                        {
                            Prosol_NounModifiers mdl = new Prosol_NounModifiers();
                            mdl.Noun = q[i].Month;
                            mdl.Modifier = q[i].Count.ToString();
                            dash.NMDataList.Add(mdl);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var log = new Prosol.Common.LogManager();
                log.LogError(log.GetType(), e);
                return Json(new
                {
                    success = false,
                    errors = e.ToString()
                }, JsonRequestBehavior.AllowGet);
                // res =-1;
            }

            return this.Json(ListDash, JsonRequestBehavior.AllowGet);

        }
        public JsonResult BindReleaser()
        {
            var usrInfo = _Usercreateservice.getimage(Convert.ToString(Session["UserId"]));
            var ListDash = _DashboardService.BindReleaser(usrInfo.Plantcode, usrInfo.Roles, Convert.ToString(Session["UserId"]));
            return this.Json(ListDash, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindReview()
        {
            var usrInfo = _Usercreateservice.getimage(Convert.ToString(Session["UserId"]));
            var ListDash = _DashboardService.BindReview(usrInfo.Plantcode, usrInfo.Roles, Convert.ToString(Session["UserId"]));
            return this.Json(ListDash, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindCatalogue()
        {
            var usrInfo = _Usercreateservice.getimage(Convert.ToString(Session["UserId"]));
            var ListDash = _DashboardService.BindCatalogue(usrInfo.Plantcode, usrInfo.Roles, Convert.ToString(Session["UserId"]));
            return this.Json(ListDash, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult BindApprove()
        //{
        //    var usrInfo = _Usercreateservice.getimage(Convert.ToString(Session["UserId"]));
        //    var ListDash = _DashboardService.BindApprove(usrInfo.Plantcode, usrInfo.Roles, Convert.ToString(Session["UserId"]));

        //    return this.Json(ListDash, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult BindRequest()
        //{
        //    var usrInfo = _Usercreateservice.getimage(Convert.ToString(Session["UserId"]));
        //    var ListDash = _DashboardService.BindRequest(usrInfo.Plantcode, usrInfo.Roles, Convert.ToString(Session["UserId"]));
        //    return this.Json(ListDash, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult BindReviewTarget()
        {
            var usrInfo = _Usercreateservice.getimage(Convert.ToString(Session["UserId"]));
            if (usrInfo.Plantcode != null)
            {
                var ListDash = _DashboardService.bindReviewTarget(usrInfo.Plantcode, Convert.ToString(Session["UserId"]));
                return this.Json(ListDash, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return null;
            }
        }
        public JsonResult BindCatalogueTarget()
        {
            var usrInfo = _Usercreateservice.getimage(Convert.ToString(Session["UserId"]));
            if (usrInfo.Plantcode != null)
            {
                var ListDash = _DashboardService.bindCatalogueTarget(usrInfo.Plantcode, Convert.ToString(Session["UserId"]));
                return this.Json(ListDash, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return null;
            }
        }
        //public JsonResult BindApproveTarget()
        //{
        //    var usrInfo = _Usercreateservice.getimage(Convert.ToString(Session["UserId"]));
        //    if (usrInfo.Plantcode != null)
        //    {
        //        var ListDash = _DashboardService.bindApproveTarget(usrInfo.Plantcode, Convert.ToString(Session["UserId"]));
        //        return this.Json(ListDash, JsonRequestBehavior.AllowGet);

        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //public JsonResult BindRequestTarget()
        //{
        //    var usrInfo = _Usercreateservice.getimage(Convert.ToString(Session["UserId"]));
        //    if (usrInfo.Plantcode != null)
        //    {
        //        var ListDash = _DashboardService.bindRequestTarget(usrInfo.Plantcode, Convert.ToString(Session["UserId"]));
        //        return this.Json(ListDash, JsonRequestBehavior.AllowGet);

        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public JsonResult BindItemHistory()
        {
            var usrInfo = _Usercreateservice.getimage(Convert.ToString(Session["UserId"]));
            var ListDash = _DashboardService.BindItemHistory(usrInfo.Plantcode);
            var jsonResult = Json(ListDash, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }


        //ItemHistoryGO
        //wrote by saikat chowdhury 07/01/2019


        public JsonResult BindItem(string dte, string dte1)
        {
            //string Uid = Session["userid"].ToString();
            //var usrInfo = _Usercreateservice.getimage(Uid);
            //int flg = 0;
            //foreach (TargetExn ent in usrInfo.Roles)
            //{
            //    if (ent.Name == "Cataloguer" || ent.Name == "Reviewer" || ent.Name == "Releaser")
            //        flg = 1;
            //} 
            //if(flg == 0)
            //{
            //    var dataList = _DashboardService.BindRequest(Convert.ToDateTime(dte), Convert.ToDateTime(dte1), Uid);
            //    var LstDash = new List<Prosol_ActionHistory>();
            //    if(dataList.Count > 0)
            //    {
            //        int cunt = 0;
            //        var mdl = new Prosol_ActionHistory();
            //        mdl.CreatedOn = DateTime.Parse(dte.ToString());

            //        foreach (var pmdl in dataList.Select(x => Convert.ToDateTime(x.requestedOn).Date).Distinct())
            //        {
            //            cunt = 0; int tot = 0; int clf = 0;
            //            mdl = new Prosol_ActionHistory();
            //            mdl.CreatedOn = Convert.ToDateTime(pmdl);

            //            var p = (from i in dataList where Convert.ToDateTime(i.requestedOn).Date == Convert.ToDateTime(pmdl).Date && i.Type == "Completed" && (i.itemStatus == "Approved") select i).ToList();
            //            mdl.Completed = p.Count;

            //            p = (from i in dataList where Convert.ToDateTime(i.requestedOn).Date == Convert.ToDateTime(pmdl).Date && i.Type == "Pending" && (i.itemStatus != "Approved") select i).ToList();
            //            mdl.Pending = p.Count;
            //            cunt = cunt + p.Count;

            //            mdl.Pending = cunt;
            //            mdl.Completed = tot - cunt;
            //            mdl.total = tot;
            //            LstDash.Add(mdl);
            //        }


            //    }
            //    var jsonResult = Json(LstDash, JsonRequestBehavior.AllowGet);
            //    jsonResult.MaxJsonLength = int.MaxValue;
            //    return jsonResult;
            //}
            //else
            //{
            var ListDash = _DashboardService.BindItemHistory2(Convert.ToDateTime(dte), Convert.ToDateTime(dte1));
            var jsonResult = Json(ListDash, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            // }
        }

        //[Authorize]
        //public ActionResult Index()
        //{
        //    if (CheckAccess("Guide") == 1)
        //        return View("Update");
        //    else if (CheckAccess("User") == 0)
        //        return View("Denied");
        //    else return View("Login");
        //}
        public JsonResult overalldatalist(string dte, string dte1)
        {

            var dataList = _DashboardService.BindOverAll(Convert.ToDateTime(dte), Convert.ToDateTime(dte1));
            var jsonResult = Json(dataList, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }
        public JsonResult fileslist()
        {
            string folderPath = Server.MapPath("~/Guideline/");
            //var filePaths = Directory.GetFiles(folderPath);
           var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
            var jsonResult = Json(filePaths, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }
        public JsonResult fileslist1()
        {
            string folderPath = Server.MapPath("~/Attachment/sampledocs/");
            //var filePaths = Directory.GetFiles(folderPath);
            var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
            var jsonResult = Json(filePaths, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }
        public JsonResult fileslist2()
        {
            string folderPath = Server.MapPath("~/Attachment/sampledocs1/");
            //var filePaths = Directory.GetFiles(folderPath);
            var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
            var jsonResult = Json(filePaths, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }
        public JsonResult fileslist3()
        {
            string folderPath = Server.MapPath("~/Attachment/sampledocs2/");
            //var filePaths = Directory.GetFiles(folderPath);
            var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
            var jsonResult = Json(filePaths, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }
        public JsonResult fileslist4()
        {
            string folderPath = Server.MapPath("~/Attachment/sampledocs3/");
            //var filePaths = Directory.GetFiles(folderPath);
            var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
            var jsonResult = Json(filePaths, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }
        public JsonResult fileslist5()
        {
            string folderPath = Server.MapPath("~/Attachment/sampledocs4/");
            //var filePaths = Directory.GetFiles(folderPath);
            var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
            var jsonResult = Json(filePaths, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }
        public JsonResult fileslist6()
        {
            string folderPath = Server.MapPath("~/Attachment/sampledocs5/");
            //var filePaths = Directory.GetFiles(folderPath);
            var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
            var jsonResult = Json(filePaths, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }
        [Authorize]
        public JsonResult Upload()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                string folderPath = Server.MapPath("~/Guideline/");
                var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
                if(filePaths.Contains(file.FileName))
                {
                    res = 2;
                }
                if (file.FileName.ToUpper().EndsWith(".PDF") || file.FileName.ToUpper().EndsWith(".DOCX"))
                {
                    if (res != 2)
                    {
                        file.SaveAs(Path.Combine(Server.MapPath("~/Guideline/"), file.FileName));
                        res = 1;
                    }
                    
                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult UploadBulk()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                string folderPath = Server.MapPath("~/Attachment/sampledocs/");
                var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
                if (filePaths.Contains(file.FileName))
                {
                    res = 2;
                }
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    if (res != 2)
                    {
                        file.SaveAs(Path.Combine(Server.MapPath("~/Attachment/sampledocs/"), file.FileName));
                        res = 1;
                    }

                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult UploadBulk1()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                string folderPath = Server.MapPath("~/Attachment/sampledocs1/");
                var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
                if (filePaths.Contains(file.FileName))
                {
                    res = 2;
                }
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    if (res != 2)
                    {
                        file.SaveAs(Path.Combine(Server.MapPath("~/Attachment/sampledocs1/"), file.FileName));
                        res = 1;
                    }

                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult UploadBulk2()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                string folderPath = Server.MapPath("~/Attachment/sampledocs2/");
                var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
                if (filePaths.Contains(file.FileName))
                {
                    res = 2;
                }
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    if (res != 2)
                    {
                        file.SaveAs(Path.Combine(Server.MapPath("~/Attachment/sampledocs2/"), file.FileName));
                        res = 1;
                    }

                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult UploadBulk3()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                string folderPath = Server.MapPath("~/Attachment/sampledocs3/");
                var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
                if (filePaths.Contains(file.FileName))
                {
                    res = 2;
                }
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    if (res != 2)
                    {
                        file.SaveAs(Path.Combine(Server.MapPath("~/Attachment/sampledocs3/"), file.FileName));
                        res = 1;
                    }

                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult UploadBulk4()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                string folderPath = Server.MapPath("~/Attachment/sampledocs4/");
                var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
                if (filePaths.Contains(file.FileName))
                {
                    res = 2;
                }
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    if (res != 2)
                    {
                        file.SaveAs(Path.Combine(Server.MapPath("~/Attachment/sampledocs4/"), file.FileName));
                        res = 1;
                    }

                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult UploadBulk5()
        {
            int res = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                string folderPath = Server.MapPath("~/Attachment/sampledocs5/");
                var filePaths = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToList();
                if (filePaths.Contains(file.FileName))
                {
                    res = 2;
                }
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    if (res != 2)
                    {
                        file.SaveAs(Path.Combine(Server.MapPath("~/Attachment/sampledocs5/"), file.FileName));
                        res = 1;
                    }

                }
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult Deletefile(string FileName)
        {
            int res = 0;
            string path = System.IO.Path.Combine(Server.MapPath("~/Guideline/" + FileName)); 
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();
                res = 1;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult Deletefile1(string FileName)
        {
            int res = 0;
            string path = System.IO.Path.Combine(Server.MapPath("~/Attachment/sampledocs/" + FileName));
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();
                res = 1;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Deletefile2(string FileName)
        {
            int res = 0;
            string path = System.IO.Path.Combine(Server.MapPath("~/Attachment/sampledocs1/" + FileName));
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();
                res = 1;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Deletefile3(string FileName)
        {
            int res = 0;
            string path = System.IO.Path.Combine(Server.MapPath("~/Attachment/sampledocs2/" + FileName));
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();
                res = 1;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Deletefile4(string FileName)
        {
            int res = 0;
            string path = System.IO.Path.Combine(Server.MapPath("~/Attachment/sampledocs3/" + FileName));
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();
                res = 1;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Deletefile5(string FileName)
        {
            int res = 0;
            string path = System.IO.Path.Combine(Server.MapPath("~/Attachment/sampledocs4/" + FileName));
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();
                res = 1;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Deletefile6(string FileName)
        {
            int res = 0;
            string path = System.IO.Path.Combine(Server.MapPath("~/Attachment/sampledocs5/" + FileName));
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();
                res = 1;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        //Dashboard
        public JsonResult getRepPurg()
        {
            var purgLst = _DashboardService.getRepPurg();
            return Json(purgLst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getRepPurgItems()
        {
            var purgLst = _DashboardService.getRepPurg().OrderBy(purg => purg.Plantcode).ToList();
            var totalItms = _DashboardService.getReqItems().ToList();

            var result = purgLst.Select(purg => new
            {
                purgCode = purg.Plantname,
                itmsCount = totalItms.Count(item => item.Plant == purg.Plantcode).ToString()
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllOpenReqs()
        {
            List<int> masLst = new List<int> { 0, -1, 1, 2, 3, 4, 5, 6, 11, 13 };
            var reqItms = _DashboardService.getReqItems().ToList();
            var masItms = _DashboardService.getMasterItems().ToList();

            var result = new List<object>();

            foreach (var mas in masLst)
            {
                var filteredItems = masItms.Where(item => item.ItemStatus == mas);
                string req;
                string itmsCount;

                switch (mas)
                {
                    case 11:
                        filteredItems = filteredItems.Where(item => item.PVstatus == "Pending" && item.PVuser != null);
                        req = "PV Pending";
                        break;
                    case 0:
                    case 1:
                    case 13:
                        filteredItems = filteredItems.Where(item => item.PVstatus == "Completed" && item.Catalogue != null);
                        req = "Catalogue";
                        break;
                    case -1:
                        req = "Clarification";
                        break;
                    case 2:
                    case 3:
                        req = "QC";
                        break;
                    case 4:
                    case 5:
                        req = "QA";
                        break;
                    case 6:
                        req = "Released";
                        break;
                    default:
                        req = "";
                        break;
                }

                itmsCount = filteredItems.Count().ToString();

                var purgAndItmsCount = new
                {
                    req,
                    itmsCount
                };

                result.Add(purgAndItmsCount);
            }

            result.Add(new
            {
                req = "Total",
                itmsCount = _DashboardService.getSKUItems().Count().ToString()
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getTotalReqs(int tyear)
        {
            List<string> monthLst = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var reqItms = _DashboardService.getReqItems().ToList();

            var data = new List<object>();
            var yearsList = new List<object>();

            foreach (var mth in monthLst)
            {
                var index = monthLst.IndexOf(mth) + 1;
                var filteredItems = reqItms.Where(item => item.CreatedOn.HasValue && item.CreatedOn.Value.Year == tyear && item.CreatedOn.Value.Month == index);

                var purgAndItmsCount = new
                {
                    rep = filteredItems.Count().ToString(),
                    month = mth
                };

                data.Add(purgAndItmsCount);
            }

            foreach (var req in reqItms)
            {
                if (req.CreatedOn.HasValue)
                {
                    var year = req.CreatedOn.Value.Year.ToString();
                    if (!yearsList.Any(y => y.Equals(new { year })))
                    {
                        yearsList.Add(new { year });
                    }
                }
            }

            var result = new { data, yearsList };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getTotalPendReqs(int tyear)
        {
            List<string> monthLst = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var reqItms = _DashboardService.getPendItems().ToList();

            var data = new List<object>();
            var yearsList = new List<object>();

            foreach (var mth in monthLst)
            {
                var index = monthLst.IndexOf(mth) + 1;
                var filteredItems = reqItms.Where(item => item.CreatedOn.HasValue && item.CreatedOn.Value.Year == tyear && item.CreatedOn.Value.Month == index);

                var purgAndItmsCount = new
                {
                    rep = filteredItems.Count().ToString(),
                    month = mth
                };

                data.Add(purgAndItmsCount);
            }

            foreach (var req in reqItms)
            {
                if (req.CreatedOn.HasValue)
                {
                    var year = req.CreatedOn.Value.Year.ToString();
                    if (!yearsList.Any(y => y.Equals(new { year })))
                    {
                        yearsList.Add(new { year });
                    }
                }
            }

            var result = new { data, yearsList };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendItems(int year)
        {
            List<string> monthLst = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var masItms = _DashboardService.getMasterItems().ToList();

            var data = new List<object>();
            var yearsList = new List<object>();

            foreach (var month in monthLst)
            {
                var index = monthLst.IndexOf(month) + 1;
                var filteredMasItems = masItms.Where(item => item.CreatedOn.HasValue && item.CreatedOn.Value.Year == year && item.CreatedOn.Value.Month == index);

                var pen = filteredMasItems.Count(item => item.ItemStatus == 0);
                var app = filteredMasItems.Count(item => item.ItemStatus == 1 || item.ItemStatus == 2);
                var fin = filteredMasItems.Count(item => item.ItemStatus == 3 || item.ItemStatus == 4);
                var clf = filteredMasItems.Count(item => item.ItemStatus == -1 || item.ItemStatus == -2 || item.ItemStatus == -3);
                var qc = filteredMasItems.Count(item => item.ItemStatus == 5 || item.ItemStatus == 6);

                var itmsCount = pen + app + fin + clf + qc;

                var purgAndItmsCount = new
                {
                    year,
                    month,
                    itmsCount
                };

                data.Add(purgAndItmsCount);
            }

            foreach (var req in masItms)
            {
                if (req.CreatedOn.HasValue)
                {
                    var fyear = req.CreatedOn.Value.Year.ToString();
                    if (!yearsList.Any(y => y.Equals(new { fyear })))
                    {
                        yearsList.Add(new { fyear });
                    }
                }
            }

            var result = new { data, yearsList };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        static int GetMonthNumber(string monthName)
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            for (int i = 1; i <= 12; i++)
            {
                if (string.Compare(monthName, dtfi.GetMonthName(i), true) == 0)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}

