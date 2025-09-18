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
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using ExcelLibrary.Office.Excel;
using System.Drawing;
using System.Web.UI;
using Microsoft.Ajax.Utilities;

namespace ProsolOnline.Controllers
{
    public class BomController : Controller
    {
        private readonly I_Bom _BomService;
        private readonly ICatalogue _CatalogueService;
        private readonly ISearch _SearchService;
        public BomController(I_Bom BomService, ICatalogue catalogueService, ISearch searchService)
        {
            _BomService = BomService;
            _CatalogueService = catalogueService;
            _SearchService = searchService;

        }
        [Authorize]
        public ActionResult EquipBom()
        {
            if (CheckAccess("Equipment Bom") == 1)
                return View("EquipBom");
            else if (CheckAccess("Equipment Bom") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult MatBom()
        {
            if (CheckAccess("Material Bom") == 1)
                return View("MatBom");
            else if (CheckAccess("Material Bom") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult MasterBom()
        {
            if (CheckAccess("Equipment Master") == 1)
                return View("MasterBom");
            else if (CheckAccess("Master Bom") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult BomDashboard()
        {
            if (CheckAccess("Dashboard") == 1)
                return View("BomDashboard");
            else if (CheckAccess("Dashboard") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult Bomtracking()
        {
            if (CheckAccess("Hierarchy Bom") == 1)
                return View("Bomtracking");
            else if (CheckAccess("Tracking Bom") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult Csir()
        {
            if (CheckAccess("CSIR") == 1)
                return View("Csir");
            else if (CheckAccess("CSIR") == 0)
                return View("Denied");
            else return View("Login");

        }
        public ActionResult BomReport()
        {
            if (CheckAccess("Bom Report") == 1)
                return View("BomReport");
            else if (CheckAccess("Bom Report") == 0)
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
        public JsonResult funlocsearch(string sKey, int currentPage, int maxRows)
        {
            var srch = _BomService.funlocsearch(sKey);
            var lstfunloc = new List<Prosol_Funloc>();
            foreach (Prosol_Funloc fun in srch)
            {
                var pro = new Prosol_Funloc();
                //if (fun.TechIdentNo != null && fun.TechIdentNo != "")
                //{
                //    var srch1 = _BomService.funequip(fun.TechIdentNo);
                //    if (srch1.Count != 0)
                //    {
                //        pro.status = "Completed";
                //    }
                //}



                pro.FunctLocation = fun.FunctLocation;
                pro.FunctDesc = fun.FunctDesc;
                pro.SupFunctLoc = fun.SupFunctLoc;
                pro.Objecttype = fun.Objecttype;
                pro.TechIdentNo = fun.TechIdentNo;
                pro.EquipDesc = fun.EquipDesc;
                pro.Manufacturer = fun.Manufacturer;
                pro.ManufCon = fun.ManufCon;
                pro.Modelno = fun.Modelno;
                pro.ManufSerialNo = fun.ManufSerialNo;
                pro.ABCindic = fun.ABCindic;

                lstfunloc.Add(pro);
            }
            var lstfunloc1 = lstfunloc.OrderBy(d => d.FunctLocation.Length);

            PagingGroup pageList = new PagingGroup();
            pageList.totItem = lstfunloc1.ToList().Count;
            var lstTmp = (from prsl in lstfunloc1 select prsl)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            pageList.FunlocList = lstTmp.ToList();
            double pageCount = (double)((decimal)lstTmp.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;


            var jsonResult = Json(pageList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        [Authorize]
        public JsonResult searchsuperfunloc(string sKey)
        {
            var srch = _BomService.funlocsearch(sKey);
            var lstfunloc = new List<Prosol_Funloc>();
            foreach (Prosol_Funloc fun in srch)
            {
                var pro = new Prosol_Funloc();
               
                pro.FunctLocation = fun.FunctLocation;
                pro.FunctDesc = fun.FunctDesc;
                pro.SupFunctLoc = fun.SupFunctLoc;
                pro.Objecttype = fun.Objecttype;
                pro.TechIdentNo = fun.TechIdentNo;
                pro.EquipDesc = fun.EquipDesc;
                pro.Manufacturer = fun.Manufacturer;
                pro.ManufCon = fun.ManufCon;
                pro.Modelno = fun.Modelno;
                pro.ManufSerialNo = fun.ManufSerialNo;
                pro.ABCindic = fun.ABCindic;

                lstfunloc.Add(pro);
            }
            


            var jsonResult = Json(lstfunloc, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        [Authorize]
        public JsonResult manfacsearch(string sKey)
        {
            var srch = _BomService.manfacsearch(sKey);
            var lstfunloc = new List<Prosol_equipbom>();
            foreach (Prosol_equipbom fun in srch)
            {
                var pro = new Prosol_equipbom();

                var srch1 = _BomService.funequip(fun.TechIdentNo);
                pro.FunctLocation = fun.FunctLocation;
                pro.FunctDesc = fun.FunctDesc;
                pro.SupFunctLoc = fun.SupFunctLoc;
                pro.Objecttype = fun.Objecttype;
                pro.TechIdentNo = fun.TechIdentNo;
                pro.EquipDesc = fun.EquipDesc;
                pro.Manufacturer = fun.Manufacturer;
                pro.ManufCon = fun.ManufCon;
                pro.Modelno = fun.Modelno;
                pro.ManufSerialNo = fun.ManufSerialNo;
                pro.ABCindic = fun.ABCindic;

                lstfunloc.Add(pro);
            }

            var jsonResult = Json(lstfunloc, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        [Authorize]
        public JsonResult funlocsearch1(int currentPage, int maxRows)
        {
            track pageList = new track();
            track var1 = new track();
            var queryCollection = System.Web.HttpUtility.ParseQueryString(HttpContext.Request.UrlReferrer.Query);
            if (queryCollection.AllKeys.Contains("id") && !string.IsNullOrEmpty(queryCollection.Get("id")))
            {
                string value = queryCollection.Get("id");
                var get = _BomService.getspares(value);
                var lst = new List<Prosol_equipbom>();
                var res = new List<Prosol_MaterialBom>();
                var tst = new List<Prosol_MaterialBom>();
               
                if (get.Count != 0)
                {
                    foreach (Prosol_equipbom fun in get)
                    {
                        var pro = new Prosol_equipbom();
                        pro.FunctLocation = fun.FunctLocation;
                        pro.FunctDesc = fun.FunctDesc;
                        pro.SupFunctLoc = fun.SupFunctLoc;
                        pro.Objecttype = fun.Objecttype;
                        pro.TechIdentNo = fun.TechIdentNo;
                        pro.EquipDesc = fun.EquipDesc;
                        pro.Manufacturer = fun.Manufacturer;
                        pro.ManufCon = fun.ManufCon;
                        pro.Modelno = fun.Modelno;
                        pro.ManufSerialNo = fun.ManufSerialNo;
                        pro.Itemcode = fun.Itemcode;
                        pro.Shortdesc = fun.Shortdesc;
                        pro.partqnt = fun.partqnt;
                        pro.itemcat = fun.itemcat;
                        lst.Add(pro);

                        if (pro.itemcat == "I")
                        {
                            var pre = _BomService.getmatspares(pro.Itemcode);

                            foreach (Prosol_MaterialBom no in pre)
                            {
                                var got = new Prosol_MaterialBom();
                                got.HeaderBID = no.HeaderBID;
                                got.Itemcode = no.Itemcode;
                                got.Shortdesc = no.Shortdesc;
                                got.partqnt = no.partqnt;
                                got.itemcat = no.itemcat;
                                res.Add(got);
                                if (got.itemcat == "I")
                                {
                                    var pre1 = _BomService.getmatspares(got.Itemcode);

                                    foreach (Prosol_MaterialBom no1 in pre1)
                                    {
                                        var get1 = new Prosol_MaterialBom();
                                        get1.HeaderBID = no1.HeaderBID;
                                        get1.Itemcode = no1.Itemcode;
                                        get1.Shortdesc = no1.Shortdesc;
                                        get1.partqnt = no1.partqnt;
                                        get1.itemcat = no1.itemcat;
                                        tst.Add(get1);

                                    }


                                }
                            }


                        }
                    }

                    pageList.eqpbom = lst;
                    pageList.matbom = res;
                    pageList.mmatbom = tst;
                }
                else
                {

                    return Json("true", JsonRequestBehavior.AllowGet);

                }
            }
            
            var srch = _BomService.funlocsearch1().OrderBy(x=>x._id);
            var lstfunloc = new List<Prosol_Funloc>();
            foreach (Prosol_Funloc fun in srch)
            {
                var pro = new Prosol_Funloc();
                //if (fun.TechIdentNo != null && fun.TechIdentNo != "")
                //{
                //    var srch1 = _BomService.funequip(fun.TechIdentNo);
                //    if (srch1.Count != 0)
                //    {
                //        pro.status = "Completed";
                //    }
                //}



                pro.FunctLocation = fun.FunctLocation;
                pro.FunctDesc = fun.FunctDesc;
                pro.SupFunctLoc = fun.SupFunctLoc;
                pro.Objecttype = fun.Objecttype;
                pro.TechIdentNo = fun.TechIdentNo;
                pro.EquipDesc = fun.EquipDesc;
                pro.EquipCategory = fun.EquipCategory;
                pro.Manufacturer = fun.Manufacturer;
                pro.ManufCon = fun.ManufCon;
                pro.Modelno = fun.Modelno;
                pro.ManufSerialNo = fun.ManufSerialNo;
                pro.ABCindic = fun.ABCindic;

                lstfunloc.Add(pro);
            }


          
            pageList.totItem = lstfunloc.Count;
            var lstTmp = (from prsl in lstfunloc select prsl).OrderBy(prsl => prsl._id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            pageList.lst = lstTmp;
            double pageCount = (double)((decimal)lstTmp.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;

            var jsonResult = Json(pageList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        [Authorize]
        public JsonResult funlocsearchb(int currentPage, int maxRows)
        {
            var srch = _BomService.funlocsearch2();
           
            var lstfunloc = new List<Prosol_Funloc>();
            foreach (Prosol_Funloc fun in srch)
            {
                var pro = new Prosol_Funloc();
                if (fun.TechIdentNo != null && fun.TechIdentNo != "")
                {

                    pro.FunctLocation = fun.FunctLocation;
                    pro.FunctDesc = fun.FunctDesc;
                    pro.SupFunctLoc = fun.SupFunctLoc;
                    pro.Objecttype = fun.Objecttype;
                    pro.TechIdentNo = fun.TechIdentNo;
                    pro.EquipDesc = fun.EquipDesc;
                    pro.EquipCategory = fun.EquipCategory;
                    pro.Manufacturer = fun.Manufacturer;
                    pro.ManufCon = fun.ManufCon;
                    pro.Modelno = fun.Modelno;
                    pro.ManufSerialNo = fun.ManufSerialNo;
                    pro.ABCindic = fun.ABCindic;
                    pro.Weight = fun.Weight;
                    pro.UOM = fun.UOM;
                    pro.Size = fun.Size;
                    pro.AcquisValue = fun.AcquisValue;
                    pro.AcquistnDat = fun.AcquistnDat;
                    pro.Manufacturer = fun.Manufacturer;
                    pro.ManufCon = fun.ManufCon;
                    pro.Modelno = fun.Modelno;
                    pro.ConstructYear = fun.ConstructYear;
                    pro.ConstructMth = fun.ConstructMth;
                    pro.ManufPartNo = fun.ManufPartNo;
                    pro.ManufSerialNo = fun.ManufSerialNo;
                    pro.AuthGroup = fun.AuthGroup;
                    pro.Startupdate = fun.Startupdate;
                    pro.MaintPlant = fun.MaintPlant;
                    pro.Companycode = fun.Companycode;
                    pro.Asset = fun.Asset;
                    pro.Subno = fun.Subno;
                    pro.ConID = fun.ConID;
                    pro.Catalogprofile = fun.Catalogprofile;
                    pro.Mainworkcenter = fun.Mainworkcenter;



                    lstfunloc.Add(pro);
                }
            }
            PagingGroup pageList = new PagingGroup();
            pageList.totItem = lstfunloc.ToList().Count;

            var lstTmp = (from prsl in lstfunloc select prsl).Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            pageList.FunlocList = lstTmp;

            double pageCount = (double)((decimal)lstTmp.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;

            var jsonResult = Json(pageList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        public JsonResult funlocsearc4(int currentPage, int maxRows)
        {
            // var srch = _BomService.funlocsearch2();
            var srch = new List<FunlocModel>();
            var srch1 = _BomService.funlocsearch2();
            foreach (Prosol_Funloc mdl in srch1)
            {
                var Mdl = new FunlocModel();
                Mdl._id = Convert.ToString(mdl._id);
                Mdl.FunctLocation = mdl.FunctLocation;
                Mdl.FunctLocCat = mdl.FunctLocCat;
                Mdl.FunctDesc = mdl.FunctDesc;
                Mdl.ABCindic = mdl.ABCindic;
                Mdl.SupFunctLoc = mdl.SupFunctLoc;
                Mdl.Objecttype = mdl.Objecttype;
                Mdl.Startdate = mdl.Startdate;
                Mdl.AuthGroup = mdl.AuthGroup;
                Mdl.MainWrk = mdl.MainWrk;
                Mdl.WBSelement = mdl.WBSelement;
                Mdl.Plantsection = mdl.Plantsection;
                Mdl.Catalogprofile = mdl.Catalogprofile;
                Mdl.Singleinst = mdl.Singleinst;
                Mdl.Manufacturer = mdl.Manufacturer;
                Mdl.ManufCon = mdl.ManufCon;
                Mdl.Modelno = mdl.Modelno;
                Mdl.ManufSerialNo = mdl.ManufSerialNo;
                Mdl.FunclocClass1 = mdl.FunclocClass1;
                Mdl.AuthGroup = mdl.AuthGroup;
                Mdl.Comment = mdl.Comment;
                Mdl.Asset = mdl.Asset;
                srch.Add(Mdl);    
            }


            PagingGroupfun pageList = new PagingGroupfun();
            pageList.totItem = srch.ToList().Count;

            var lstTmp = (from prsl in srch select prsl).Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            pageList.FunlocList = lstTmp;

            double pageCount = (double)((decimal)lstTmp.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;            var jsonResult = Json(pageList, JsonRequestBehavior.AllowGet);            jsonResult.MaxJsonLength = int.MaxValue;            return jsonResult;
          //  return this.Json(srch, JsonRequestBehavior.AllowGet);

        }

        public class INOUT
        {
            public List<Prosol_IOModel> IN { set; get; }
            public List<Prosol_IOModel> OUT { set; get; }
        }
        public JsonResult getinrecord()
        {


            var objList = _BomService.getinrecord();

            DateTime date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);


            string st1 = DateTime.Parse(date.ToString()).ToString("MM");
            string st2 = DateTime.Parse(date.ToString()).ToString("yyyy");



            var IN = new List<Prosol_IOModel>();
            var OUT = new List<Prosol_IOModel>();
            foreach (Prosol_IOModel mdl in objList)
            {

                if (mdl.INtime != null)
                {
                    string str1 = DateTime.Parse(mdl.INtime.ToString()).ToString("MM");
                    string str2 = DateTime.Parse(mdl.INtime.ToString()).ToString("yyyy");
                    if (st1 == str1 && st2 == str2)
                    {

                        var obj = new Prosol_IOModel();

                        obj.Materialcode = mdl.Materialcode;
                        obj.FunctLocation = mdl.FunctLocation;
                        obj.TechIdentNo = mdl.TechIdentNo;
                        obj.INtime = mdl.INtime;


                        IN.Add(obj);

                    }
                }
                else if (mdl.OUTtime != null)
                {
                    string str1 = DateTime.Parse(mdl.OUTtime.ToString()).ToString("MM");
                    string str2 = DateTime.Parse(mdl.OUTtime.ToString()).ToString("yyyy");
                    if (st1 == str1 && st2 == str2)
                    {
                        var obj1 = new Prosol_IOModel();

                        obj1.Materialcode = mdl.Materialcode;
                        obj1.FunctLocation = mdl.FunctLocation;
                        obj1.TechIdentNo = mdl.TechIdentNo;
                        obj1.OUTtime = mdl.OUTtime;
                        OUT.Add(obj1);
                    }
                }






            }
            INOUT INOUT = new INOUT();
            INOUT.IN = IN;
            INOUT.OUT = OUT;
            return this.Json(INOUT, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Trackload()
        {

            string fromdate = Request.Form["fromdate"];
            string todate = Request.Form["todate"];
            string option = Request.Form["option"];
            var materialcode = Request.Form["materialcode"];
            /////
            if (materialcode != "" && materialcode != null && materialcode != "undefined")
            {
                string[] codestr;

                List<Prosol_IOModel> get_assigndata = new List<Prosol_IOModel>();
                List<Prosol_IOModel> gd_data = new List<Prosol_IOModel>();
                //List<multiplecodelist> getcode = new List<multiplecodelist>();
                List<string> code_split = new List<string>();
                codestr = materialcode.Split(',');
                foreach (string n in codestr)
                {
                    if (!code_split.Contains(n.ToString().Trim()))
                        code_split.Add(n.ToString().Trim());
                }
                foreach (string cdn in code_split)
                {
                    gd_data = _BomService.trackmulticodelist(cdn).ToList();
                    get_assigndata.AddRange(gd_data);

                }

                var lst = new List<multiplecodelist1>();
                foreach (Prosol_IOModel mdl in get_assigndata)
                {
                    var obj = new multiplecodelist1();

                    obj.Materialcode = mdl.Materialcode;
                    //obj.CreatedOn = mdl.CreatedOn;
                    if (mdl.Createdon != null)
                    {
                        DateTime date = DateTime.Parse(Convert.ToString(mdl.Createdon));

                        obj.Createdon = date.ToString("dd/MM/yyyy");
                    }
                    //  obj.CreatedOn = date.ToString("dd/MM/yyyy");
                    // DateTime date = DateTime.Parse(Convert.ToString(mdl.CreatedOn));
                    obj.FunctLocation = mdl.FunctLocation;
                    obj.TechIdentNo = mdl.TechIdentNo;
                    if (mdl.INtime != null)
                    {
                        DateTime date1 = DateTime.Parse(Convert.ToString(mdl.INtime));

                        obj.INtime = date1.ToString("dd/MM/yyyy");
                    }
                    // obj.INtime = mdl.INtime;

                    if (mdl.OUTtime != null)
                    {
                        DateTime date11 = DateTime.Parse(Convert.ToString(mdl.OUTtime));

                        obj.OUTtime = date11.ToString("dd/MM/yyyy");
                    }
                    // obj.OUTtime = mdl.OUTtime;

                    // obj.Plant = "";
                    lst.Add(obj);

                }

                return Json(lst, JsonRequestBehavior.AllowGet);

            }
            else
            {
                // var Exportlist = null;
                var Exportlist = _BomService.Trackload(materialcode, fromdate, todate, option).ToList();
                // return Json(Exportlist, JsonRequestBehavior.AllowGet);
                var jsonResult = Json(Exportlist, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
        }

        public JsonResult DownloadTrack1()
        {
            var materialcode = Request.Form["materialcode"];
            Session["materialcode"] = materialcode;

            string[] codestr;
            List<Prosol_IOModel> get_assigndata = new List<Prosol_IOModel>();
            List<Prosol_IOModel> gd_data = new List<Prosol_IOModel>();
            //List<multiplecodelist> getcode = new List<multiplecodelist>();
            List<string> code_split = new List<string>();
            codestr = materialcode.Split(',');
            foreach (string n in codestr)
            {
                if (!code_split.Contains(n.ToString().Trim()))
                    code_split.Add(n.ToString().Trim());
            }
            foreach (string cdn in code_split)
            {
                gd_data = _BomService.trackmulticodelist(cdn).ToList();
                get_assigndata.AddRange(gd_data);

            }
            var cunt = gd_data.Count;
            return Json(cunt, JsonRequestBehavior.AllowGet);

        }
        public void DownloadMulticode()
        {
            var materialcode = Session["materialcode"].ToString();
            string[] codestr;
            List<Prosol_IOModel> get_assigndata = new List<Prosol_IOModel>();
            List<Prosol_IOModel> gd_data = new List<Prosol_IOModel>();
            //List<multiplecodelist> getcode = new List<multiplecodelist>();
            List<string> code_split = new List<string>();
            codestr = materialcode.Split(',');
            foreach (string n in codestr)
            {
                if (!code_split.Contains(n.ToString().Trim()))
                    code_split.Add(n.ToString().Trim());
            }
            foreach (string cdn in code_split)
            {
                gd_data = _BomService.trackmulticodelist(cdn).ToList();
                get_assigndata.AddRange(gd_data);

            }

            var lst = new List<multiplecodelist1>();
            foreach (Prosol_IOModel mdl in get_assigndata)
            {
                var obj = new multiplecodelist1();

                obj.Materialcode = mdl.Materialcode;
                //obj.CreatedOn = mdl.CreatedOn;
                if (mdl.Createdon != null)
                {
                    DateTime date = DateTime.Parse(Convert.ToString(mdl.Createdon));

                    obj.Createdon = date.ToString("dd/MM/yyyy");
                }
                //  obj.CreatedOn = date.ToString("dd/MM/yyyy");
                // DateTime date = DateTime.Parse(Convert.ToString(mdl.CreatedOn));
                obj.FunctLocation = mdl.FunctLocation;
                obj.TechIdentNo = mdl.TechIdentNo;
                if (mdl.INtime != null)
                {
                    DateTime date1 = DateTime.Parse(Convert.ToString(mdl.INtime));

                    obj.INtime = date1.ToString("dd/MM/yyyy");
                }
                if (mdl.OUTtime != null)
                {
                    DateTime date11 = DateTime.Parse(Convert.ToString(mdl.OUTtime));

                    obj.OUTtime = date11.ToString("dd/MM/yyyy");
                }

                // obj.INtime = mdl.INtime;
                // obj.OUTtime = mdl.OUTtime;

                lst.Add(obj);
            }
            var strJson = JsonConvert.SerializeObject(lst);
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));

            //string file = Server.MapPath("~/common/") + "TrackReport.xls";
            //Response.ClearContent();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", "TrackReport.xls"));
            //Response.ContentType = "application/ms-excel";

            //Workbook workbook = new Workbook();
            //Worksheet worksheet = new Worksheet("First Sheet");

            //int i = 0;

            //foreach (DataRow dr in dt.Rows)
            //{
            //    int j = 0;
            //    foreach (DataColumn dc in dt.Columns)
            //    {
            //        if (i == 0)
            //        {
            //            worksheet.Cells[i, j] = new Cell(Convert.ToString(dc));
            //            j++;
            //        }
            //        else
            //        {
            //            worksheet.Cells[i, j] = new Cell(Convert.ToString(dr[j]));
            //            j++;
            //        }
            //    }
            //    i++;
            //}

            //workbook.Worksheets.Add(worksheet);
            //workbook.Save(file);

            //Response.TransmitFile(@"/Common/TrackReport.xls");
            //Response.End();


            //Response.ClearContent();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "TrackReport.xls"));
            //Response.ContentType = "application/ms-excel";
            //string str = string.Empty;
            //for (int i = 0; i < dt.Columns.Count; i++)
            //{
            //    Response.Write(dt.Columns[i].ToString().ToUpper() + "\t");
            //}

            //Response.Write("\n");
            //foreach (DataRow dr in dt.Rows)
            //{
            //    str = "";
            //    for (int j = 0; j < dt.Columns.Count; j++)
            //    {
            //        if (dr[j].ToString().Contains('\n'))
            //        {
            //            dr[j] = dr[j].ToString().Replace('\n', ' ');
            //        }
            //        Response.Write(str + dr[j]);
            //        str = "\t";
            //    }
            //    Response.Write("\n");
            //}
            //Session["materialcode"] = "";
            //Response.Flush();
            //Response.End();


            ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
            wbook.Worksheets.Add(dt, "tab1");
            // Prepare the response
            HttpResponseBase httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Provide you file name here
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"TrackReportINOUT.xlsx\"");

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();



        }
        public void DownloadTrack(string materialcode, string Fromdate, string Todate, string option)
        {



            if (option == "undefined")
                option = null;

            if (Fromdate == "null")
                Fromdate = null;
            if (Todate == "null")
                Todate = null;

            var res = _BomService.Trackload(materialcode, Fromdate, Todate, option).ToList();

            DataTable dt = new DataTable();

            foreach (IDictionary<string, object> row in res)
            {
                foreach (KeyValuePair<string, object> entry in row)
                {
                    if (!dt.Columns.Contains(entry.Key.ToString()))
                    {
                        dt.Columns.Add(entry.Key);
                    }
                }
                dt.Rows.Add(row.Values.ToArray());
            }


            ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
            wbook.Worksheets.Add(dt, "tab1");
            // Prepare the response
            HttpResponseBase httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Provide you file name here
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"TrackReport.xlsx\"");

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();

        }

        [Authorize]
        public JsonResult frstmat(int currentPage, int maxRows)
        {
            var srchList = _BomService.frstmat();
            var lstCatalogue = new List<CatalogueModel>();
            foreach (Prosol_Datamaster cat in srchList)
            {
                var proCat = new CatalogueModel();
                //var srch1 = _BomService.matequip(cat.Itemcode);
                //if (srch1.Count != 0)
                //{
                //    proCat.status = "Completed";
                //}

                proCat._id = cat._id.ToString();
                proCat.Itemcode = cat.Materialcode;
                //  proCat.Materialcode = cat.Materialcode;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;
                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.Manufacturer = cat.Manufacturer;
                proCat.Partno = cat.Partno;


                lstCatalogue.Add(proCat);
            }
            PagingGroup1 pageList = new PagingGroup1();
            pageList.totItem = lstCatalogue.Count;
            var lstTmp = (from prsl in lstCatalogue select prsl).Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            pageList.matList = lstTmp;

            double pageCount = (double)((decimal)lstTmp.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;

            var jsonResult = Json(pageList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        [Authorize]
        public JsonResult srch(string sKey)
        {
            var equip = _BomService.srch(sKey);
            var lstequip = new List<EquipBomModel>();
            foreach (Prosol_equipbom EQ in equip)
            {
                var Equip = new EquipBomModel();
                Equip.Itemcode = EQ.Itemcode;
                Equip.Longdesc = EQ.Longdesc;
                Equip.Shortdesc = EQ.Shortdesc;
                Equip.partqnt = Convert.ToInt16(EQ.partqnt);
                Equip.itemcat = EQ.itemcat;


                lstequip.Add(Equip);
            }

            var jsonResult = Json(lstequip, JsonRequestBehavior.AllowGet);

            return jsonResult;

        }
        [Authorize]
        public JsonResult srchbom(string sKey)
        {
            var bom = _BomService.srchbom(sKey);
            var lstbom = new List<MaterialBom>();
            foreach (Prosol_MaterialBom BM in bom)
            {
                var Bom = new MaterialBom();
                Bom.Itemcode = BM.Itemcode;
                Bom.Longdesc = BM.Longdesc;
                Bom.Shortdesc = BM.Shortdesc;
                Bom.partqnt = Convert.ToInt16(BM.partqnt);
                Bom.itemcat = BM.itemcat;


                lstbom.Add(Bom);
            }

            var jsonResult = Json(lstbom, JsonRequestBehavior.AllowGet);

            return jsonResult;

        }
        [Authorize]
        public JsonResult searching(string sKey)
        {
            var srchList = _SearchService.SearchDesc(sKey, "Description");
            if (srchList.Count == 0)
            {
                srchList = _SearchService.SearchDesc(sKey, "");
            }
            var lstCatalogue = new List<CatalogueModel>();
            foreach (Prosol_Datamaster cat in srchList)
            {
                var proCat = new CatalogueModel();
                proCat._id = cat._id.ToString();
                proCat.Itemcode = cat.Materialcode;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;

                lstCatalogue.Add(proCat);
            }
            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        [Authorize]
        public JsonResult BulkMat(string sKey)
        {
            Session["mat"] = sKey;
            var srchList = _BomService.BulkMat1(sKey);
            var lstCatalogue = new List<CatalogueModel>();
            foreach (Prosol_Datamaster cat in srchList)
            {
                var ee = new List<EquipBomModel>();
                var report = _BomService.getreport(cat.Materialcode, true, true, true).ToList();
                if (report.Count == 0)
                {
                    var matcheck = _BomService.getreport12(cat.Materialcode).ToList();

                    foreach (Prosol_MaterialBom E in matcheck)
                    {


                        var w = _BomService.getreport(E.HeaderBID, true, true, true).ToList();
                        if (w.Count == 0)
                        {
                            var MQ = _BomService.getreport12(E.HeaderBID).ToList();
                            foreach (Prosol_MaterialBom mQ1 in MQ)
                            {

                                var w1 = _BomService.getreport(mQ1.HeaderBID, true, true, true).ToList();


                                foreach (Prosol_equipbom EQ in w1)
                                {

                                    var e = new EquipBomModel();

                                    e.TechIdentNo = EQ.TechIdentNo;
                                    e.FunctLocation = EQ.FunctLocation;
                                    e.partqnt = Convert.ToInt16(E.partqnt);
                                    e.ABCindic = EQ.ABCindic;

                                    ee.Add(e);

                                }



                            }
                        }
                        else
                        {


                            foreach (Prosol_equipbom EQ in w)
                            {

                                var e = new EquipBomModel();

                                e.TechIdentNo = EQ.TechIdentNo;
                                e.FunctLocation = EQ.FunctLocation;
                                e.partqnt = Convert.ToInt16(E.partqnt);
                                e.ABCindic = EQ.ABCindic;

                                ee.Add(e);

                            }
                        }
                    }
                }

                foreach (Prosol_equipbom EQ in report)
                {
                    if (EQ.itemcat != "I")
                    {
                        var e = new EquipBomModel();

                        e.TechIdentNo = EQ.TechIdentNo;
                        e.FunctLocation = EQ.FunctLocation;
                        e.partqnt = Convert.ToInt16(EQ.partqnt);
                        e.ABCindic = EQ.ABCindic;
                        ee.Add(e);
                    }
                }



                var proCat = new CatalogueModel();
                proCat.Itemcode = cat.Materialcode;

                proCat.Total_Equipment = ee.Count.ToString();
                double total = ee.Sum(item => Convert.ToInt16(item.partqnt));
                proCat.Total_PartQuantity = Convert.ToInt16(total);


                lstCatalogue.Add(proCat);
            }
            var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }


        [Authorize]
        public void BulkMatDownload()

        {
            string item1 = Session["mat"].ToString();
            var srchList = _BomService.BulkMat1(item1);
            var ee = new List<EquipBomModel>();

            foreach (Prosol_Datamaster cat in srchList)
            {

                var report = _BomService.getreport(cat.Materialcode, true, true, true).ToList();
                if (report.Count == 0)
                {
                    var matcheck = _BomService.getreport12(cat.Materialcode).ToList();

                    foreach (Prosol_MaterialBom E in matcheck)
                    {


                        var w = _BomService.getreport(E.HeaderBID, true, true, true).ToList();
                        if (w.Count == 0)
                        {
                            var MQ = _BomService.getreport12(E.HeaderBID).ToList();
                            foreach (Prosol_MaterialBom mQ1 in MQ)
                            {

                                var w1 = _BomService.getreport(mQ1.HeaderBID, true, true, true).ToList();


                                foreach (Prosol_equipbom EQ in w1)
                                {

                                    var e = new EquipBomModel();
                                    e.Itemcode = cat.Materialcode;
                                    e.TechIdentNo = EQ.TechIdentNo;
                                    e.FunctLocation = EQ.FunctLocation;
                                    e.partqnt = Convert.ToInt16(E.partqnt);
                                    e.ABCindic = EQ.ABCindic;

                                    ee.Add(e);

                                }



                            }
                        }
                        else
                        {


                            foreach (Prosol_equipbom EQ in w)
                            {

                                var e = new EquipBomModel();
                                e.Itemcode = cat.Materialcode;
                                e.TechIdentNo = EQ.TechIdentNo;
                                e.FunctLocation = EQ.FunctLocation;
                                e.partqnt = Convert.ToInt16(E.partqnt);
                                e.ABCindic = EQ.ABCindic;

                                ee.Add(e);

                            }
                        }
                    }
                }

                foreach (Prosol_equipbom EQ in report)
                {
                    if (EQ.itemcat != "I")
                    {
                        var e = new EquipBomModel();
                        e.Itemcode = cat.Materialcode;
                        e.TechIdentNo = EQ.TechIdentNo;
                        e.FunctLocation = EQ.FunctLocation;
                        e.partqnt = Convert.ToInt16(EQ.partqnt);
                        e.ABCindic = EQ.ABCindic;
                        ee.Add(e);
                    }
                }


                //var proCat = new CatalogueModel();
                //proCat.Itemcode = cat.Itemcode;

                //proCat.Total_Equipment = ee.Count.ToString();
                //double total = ee.Sum(item => Convert.ToInt16(item.partqnt));
                //proCat.Total_PartQuantity = Convert.ToInt16(total);


                //lstCatalogue.Add(proCat);
            }

            var strJson = JsonConvert.SerializeObject(ee);
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0 }", "MaterialSummary.xls"));
            Response.ContentType = "application/ms-excel";
            string str = string.Empty;


            var emptyRows =
                       dt.Select()
                           .Where(
                               row =>
                                   dt.Columns.Cast<DataColumn>()
                                       .All(column => string.IsNullOrEmpty(row[column].ToString()))).ToArray();
            Array.ForEach(emptyRows, x => x.Delete());

            var emptyColumns =
                dt.Columns.Cast<DataColumn>()
                    .Where(column => dt.Select().All(row => string.IsNullOrEmpty(row[column].ToString())))
                    .ToArray();
            Array.ForEach(emptyColumns, column => dt.Columns.Remove(column));
            var Columns =
                   dt.Columns.Cast<DataColumn>()
                       .Where(column => dt.Select().All(row => string.IsNullOrEmpty(row[column].ToString())))
                       .ToArray();
            Array.ForEach(Columns, column => dt.Columns.Remove(column));
            dt.AcceptChanges();


            for (int i = 0; i < dt.Columns.Count; i++)

            {

                Response.Write(dt.Columns[i].ToString().ToUpper() + "\t");

            }
            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dr[j].ToString().Contains('\n'))
                    {
                        dr[j] = dr[j].ToString().Replace('\n', ' ');
                    }
                    Response.Write(str + dr[j]);
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.Flush();
            Response.End();

        }
        [Authorize]
        public void DetailDownload()

        {
            string item1 = Session["mat"].ToString();
            var srchList = _BomService.BulkMat1(item1);
            var ee = new List<EquipBomModel>();

            foreach (Prosol_Datamaster cat in srchList)
            {

                var report = _BomService.getreport(cat.Materialcode, true, true, true).ToList();
                if (report.Count == 0)
                {
                    var matcheck = _BomService.getreport12(cat.Materialcode).ToList();

                    foreach (Prosol_MaterialBom E in matcheck)
                    {


                        var w = _BomService.getreport(E.HeaderBID, true, true, true).ToList();
                        if (w.Count == 0)
                        {
                            var MQ = _BomService.getreport12(E.HeaderBID).ToList();
                            foreach (Prosol_MaterialBom mQ1 in MQ)
                            {

                                var w1 = _BomService.getreport(mQ1.HeaderBID, true, true, true).ToList();


                                foreach (Prosol_equipbom EQ in w1)
                                {

                                    var e = new EquipBomModel();
                                    e.Itemcode = E.Itemcode;
                                    e.Shortdesc = E.Shortdesc;
                                    e.Longdesc = E.Longdesc;
                                    e.itemcat = E.itemcat;
                                    e.partqnt = Convert.ToInt16(E.partqnt);
                                    e.EquipDesc = EQ.EquipDesc;
                                    e.FunctDesc = EQ.FunctDesc;
                                    e.FunctLocation = EQ.FunctLocation;
                                    e.SupFunctLoc = EQ.SupFunctLoc;
                                    e.ManufSerialNo = EQ.ManufSerialNo;
                                    e.Objecttype = EQ.Objecttype;
                                    e.ManufCon = EQ.ManufCon;
                                    e.Modelno = EQ.Modelno;
                                    e.Manufacturer = EQ.Manufacturer;
                                    e.TechIdentNo = EQ.TechIdentNo;
                                    e.partqnt = Convert.ToInt16(E.partqnt);
                                    e.ABCindic = EQ.ABCindic;

                                    ee.Add(e);

                                }



                            }
                        }
                        else
                        {


                            foreach (Prosol_equipbom EQ in w)
                            {

                                var e = new EquipBomModel();
                                e.Itemcode = E.Itemcode;
                                e.Shortdesc = E.Shortdesc;
                                e.Longdesc = E.Longdesc;
                                e.itemcat = E.itemcat;
                                e.partqnt = Convert.ToInt16(E.partqnt);
                                e.EquipDesc = EQ.EquipDesc;
                                e.FunctDesc = EQ.FunctDesc;
                                e.FunctLocation = EQ.FunctLocation;
                                e.SupFunctLoc = EQ.SupFunctLoc;
                                e.ManufSerialNo = EQ.ManufSerialNo;
                                e.Objecttype = EQ.Objecttype;
                                e.ManufCon = EQ.ManufCon;
                                e.Modelno = EQ.Modelno;
                                e.Manufacturer = EQ.Manufacturer;
                                e.TechIdentNo = EQ.TechIdentNo;
                                e.partqnt = Convert.ToInt16(E.partqnt);
                                e.ABCindic = EQ.ABCindic;

                                ee.Add(e);

                            }
                        }
                    }
                }

                foreach (Prosol_equipbom EQ in report)
                {
                    if (EQ.itemcat != "I")
                    {
                        var e = new EquipBomModel();
                        e.Itemcode = cat.Materialcode;
                        e.TechIdentNo = EQ.TechIdentNo;
                        e.FunctLocation = EQ.FunctLocation;
                        e.partqnt = Convert.ToInt16(EQ.partqnt);
                        e.ABCindic = EQ.ABCindic;
                        ee.Add(e);
                    }
                }


                //var proCat = new CatalogueModel();
                //proCat.Itemcode = cat.Itemcode;

                //proCat.Total_Equipment = ee.Count.ToString();
                //double total = ee.Sum(item => Convert.ToInt16(item.partqnt));
                //proCat.Total_PartQuantity = Convert.ToInt16(total);


                //lstCatalogue.Add(proCat);
            }
            var listWithoutCol = ee.Select(x => new
            {
                x.Itemcode,
                x.Shortdesc,
                x.Longdesc,
                x.partqnt,
                x.itemcat,
                x.FunctLocation,
                x.FunctDesc,
                x.SupFunctLoc,
                x.Objecttype,
                x.TechIdentNo,
                x.EquipDesc,
                x.Manufacturer,
                x.ManufCon,
                x.Modelno,
                x.ManufSerialNo,
                x.ABCindic
            }).ToList();
            if (ee.Count != 0)
            {
                var strJson = JsonConvert.SerializeObject(listWithoutCol);
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "BulkDetailReport.xls"));
                Response.ContentType = "application/ms-excel";
                string str = string.Empty;


                var emptyRows =
                           dt.Select()
                               .Where(
                                   row =>
                                       dt.Columns.Cast<DataColumn>()
                                           .All(column => string.IsNullOrEmpty(row[column].ToString()))).ToArray();
                Array.ForEach(emptyRows, x => x.Delete());

                var emptyColumns =
                    dt.Columns.Cast<DataColumn>()
                        .Where(column => dt.Select().All(row => string.IsNullOrEmpty(row[column].ToString())))
                        .ToArray();
                Array.ForEach(emptyColumns, column => dt.Columns.Remove(column));
                dt.AcceptChanges();



                for (int i = 0; i < dt.Columns.Count; i++)

                {
                    Response.Write(dt.Columns[i].ToString().ToUpper() + "\t");
                }
                Response.Write("\n");
                foreach (DataRow dr in dt.Rows)
                {
                    str = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dr[j].ToString().Contains('\n'))
                        {
                            dr[j] = dr[j].ToString().Replace('\n', ' ');
                        }
                        Response.Write(str + dr[j]);
                        str = "\t";
                    }
                    Response.Write("\n");
                }
                Response.Flush();
                Response.End();

            }

        }
        [Authorize]
        public JsonResult searching1(string sKey, int currentPage, int maxRows)
        {
            var srchList = _SearchService.SearchDesc(sKey, "Description");
            if (srchList.Count == 0)
            {
              srchList = _SearchService.SearchDesc(sKey, "");
            }
            var lstCatalogue = new List<CatalogueModel>();
            foreach (Prosol_Datamaster cat in srchList)
            {
                var proCat = new CatalogueModel();
                //var srch1 = _BomService.matequip(cat.Itemcode);
                //if (srch1.Count != 0)
                //{
                //    proCat.status = "Completed";
                //}

                proCat._id = cat._id.ToString();

                proCat.Itemcode = cat.Materialcode;
                proCat.Shortdesc = cat.Shortdesc;
                proCat.Longdesc = cat.Longdesc;
                proCat.Noun = cat.Noun;
                proCat.Modifier = cat.Modifier;
                proCat.Manufacturer = cat.Manufacturer;
                proCat.Partno = cat.Partno;


                lstCatalogue.Add(proCat);
            }
            PagingGroup1 pageList = new PagingGroup1();
            pageList.totItem = lstCatalogue.Count;
            var lstTmp = (from prsl in lstCatalogue select prsl).Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            pageList.matList = lstTmp;

            double pageCount = (double)((decimal)lstTmp.Count() / Convert.ToDecimal(maxRows));
            pageList.PageCount = (int)Math.Ceiling(pageCount);
            pageList.CurrentPageIndex = currentPage;

            var jsonResult = Json(pageList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }


        //public JsonResult searching11(string sKey, bool inda,bool indb,bool indc)
        //{
        //    var srchList = _BomService.searching11(sKey, inda, indb, indc);
        //    var lstCatalogue = new List<CatalogueModel>();
        //    foreach (Prosol_Datamaster cat in srchList)
        //    {

        //        var proCat = new CatalogueModel();
        //        proCat._id = cat._id.ToString();
        //        proCat.Itemcode = cat.Itemcode;
        //        proCat.Shortdesc = cat.Shortdesc;
        //        proCat.Longdesc = cat.Longdesc;
        //        proCat.Noun = cat.Noun;
        //        proCat.Modifier = cat.Modifier;
        //        proCat.Manufacturer = cat.Manufacturer;
        //        proCat.Partno = cat.Partno;
        //        lstCatalogue.Add(proCat);
        //    }
        //    var jsonResult = Json(lstCatalogue, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;

        //}


        [Authorize]
        public JsonResult Create()
        {
            // int res = 0;            
            var item1 = Request.Form["request"];
            string item2 = JsonConvert.DeserializeObject<string>(Request.Form["FunctLocation"]);
            string item3 = JsonConvert.DeserializeObject<string>(Request.Form["FunctDesc"]);
            string item4 = JsonConvert.DeserializeObject<string>(Request.Form["Objecttype"]);
            string item5 = JsonConvert.DeserializeObject<string>(Request.Form["TechIdentNo"]);
            string item6 = JsonConvert.DeserializeObject<string>(Request.Form["SupFunctLoc"]);
            string item7 = JsonConvert.DeserializeObject<string>(Request.Form["EquipDesc"]);
            string item8 = "";
            item8 = JsonConvert.DeserializeObject<string>(Request.Form["Manufacturer"] != null ? Request.Form["Manufacturer"] : "");
            string item9 = "";
            item9 = JsonConvert.DeserializeObject<string>(Request.Form["ManufCon"] != null ? Request.Form["ManufCon"] : "");
            string item10 = "";
            item10 = JsonConvert.DeserializeObject<string>(Request.Form["Modelno"] != null ? Request.Form["Modelno"] : "");
            string item11 = "";
            item11 = JsonConvert.DeserializeObject<string>(Request.Form["ManufSerialNo"] != null ? Request.Form["ManufSerialNo"] : "");
            string item12 = "";
            item12 = JsonConvert.DeserializeObject<string>(Request.Form["ABCindic"] != null ? Request.Form["ABCindic"] : "");


          
         

            var eqpmodel1 = JsonConvert.DeserializeObject<List<EquipBomModel>>(item1);

            List<Prosol_equipbom> ert = new List<Prosol_equipbom>();

            foreach (EquipBomModel EBM in eqpmodel1)
            {
                Prosol_equipbom eee = new Prosol_equipbom();
                eee.FunctLocation = item2.ToString();
                eee.FunctDesc = item3.ToString();
                eee.Objecttype = item4.ToString();
                eee.TechIdentNo = item5.ToString();
                eee.SupFunctLoc = item6.ToString();
                eee.EquipDesc = item7.ToString();
                eee.Manufacturer = item8;
                eee.ManufCon = item9;
                eee.Modelno = item10;
                eee.ManufSerialNo = item11;
                eee.ABCindic = item12;
                eee.itemcat = EBM.itemcat;
                eee.Itemcode = EBM.Itemcode;
                eee.partqnt = EBM.partqnt.ToString();
                eee.Shortdesc = EBM.Shortdesc;
                eee.Longdesc = EBM.Longdesc;
                var s = _BomService.get(EBM.Itemcode);
                foreach (Prosol_ERPInfo r in s)
                {
                    eee.Materialtype = r.Materialtype;
                }
                ert.Add(eee);

            }
            int res = _BomService.InsertData(ert);

            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult CreateBom()
        {
            // int res = 0;            
            var item1 = Request.Form["request"];
            string item2 = JsonConvert.DeserializeObject<string>(Request.Form["Itemcode"]);
            string item3 = JsonConvert.DeserializeObject<string>(Request.Form["Noun"]);
            string item4 = JsonConvert.DeserializeObject<string>(Request.Form["Modifier"]);
            string item5 = "";
            item5 = JsonConvert.DeserializeObject<string>(Request.Form["Manufacturer"] != null ? Request.Form["Manufacturer"] : "");
            string item6 = "";
            item6 = JsonConvert.DeserializeObject<string>(Request.Form["Partno"] != null ? Request.Form["Partno"] : "");


            var matmodel1 = JsonConvert.DeserializeObject<List<MaterialBom>>(item1);
            List<Prosol_MaterialBom> lstbom = new List<Prosol_MaterialBom>();

            foreach (MaterialBom EBM1 in matmodel1)
            {
                Prosol_MaterialBom EBM = new Prosol_MaterialBom();
                EBM.HeaderBID = item2.ToString();
                EBM.Noun = item3.ToString();
                EBM.Modifier = item4.ToString();
                EBM.Manufacturer = item5 != null ? item5.ToString() : "";
                EBM.Partno = item6 != null ? item6.ToString() : "";
                EBM.Itemcode = EBM1.Itemcode.Trim().ToString();
                EBM.Shortdesc = EBM1.Shortdesc.Trim().ToString();
                EBM.Longdesc = EBM1.Longdesc.Trim().ToString();
                EBM.partqnt = EBM1.partqnt.ToString();
                EBM.itemcat = EBM1.itemcat.ToString();
                lstbom.Add(EBM);

            }
            int res = _BomService.InsertMatData(lstbom);

            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
     
        //track
        [Authorize]
        public JsonResult TrackBom(string sKey)
        {
            var srch = _BomService.funlocsearch(sKey);
            var lstfunloc = new List<Prosol_Funloc>();
            foreach (Prosol_Funloc fun in srch)
            {
                var pro = new Prosol_Funloc();
                pro.FunctLocation = fun.FunctLocation;
                pro.FunctDesc = fun.FunctDesc;
                pro.SupFunctLoc = fun.SupFunctLoc;
                pro.Objecttype = fun.Objecttype;
                pro.TechIdentNo = fun.TechIdentNo;
                pro.EquipDesc = fun.EquipDesc;
                pro.Manufacturer = fun.Manufacturer;
                pro.ManufCon = fun.ManufCon;
                pro.Modelno = fun.Modelno;
                pro.ManufSerialNo = fun.ManufSerialNo;

                lstfunloc.Add(pro);
            }
            var jsonResult = Json(lstfunloc, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        [Authorize]
        public JsonResult getitemb(string sKey)
        {
            var get = _BomService.getspares(sKey);
            var lst = new List<Prosol_equipbom>();
            var res = new List<Prosol_MaterialBom>();
            var tst = new List<Prosol_MaterialBom>();

            if (get.Count != 0)
            {
                foreach (Prosol_equipbom fun in get)
                {
                    var pro = new Prosol_equipbom();
                    pro.FunctLocation = fun.FunctLocation;
                    pro.FunctDesc = fun.FunctDesc;
                    pro.SupFunctLoc = fun.SupFunctLoc;
                    pro.Objecttype = fun.Objecttype;
                    pro.TechIdentNo = fun.TechIdentNo;
                    pro.EquipDesc = fun.EquipDesc;
                    pro.Manufacturer = fun.Manufacturer;
                    pro.ManufCon = fun.ManufCon;
                    pro.Modelno = fun.Modelno;
                    pro.ManufSerialNo = fun.ManufSerialNo;
                    pro.Itemcode = fun.Itemcode;
                    pro.Shortdesc = fun.Shortdesc;
                    pro.partqnt = fun.partqnt;
                    pro.itemcat = fun.itemcat;
                    
                    lst.Add(pro);

                    if (pro.itemcat == "I")
                    {
                        var pre = _BomService.getmatspares(pro.Itemcode);

                        foreach (Prosol_MaterialBom no in pre)
                        {
                            var got = new Prosol_MaterialBom();
                            got.HeaderBID = no.HeaderBID;
                            got.Itemcode = no.Itemcode;
                            got.Shortdesc = no.Shortdesc;
                            got.partqnt = no.partqnt;
                            got.itemcat = no.itemcat;
                            res.Add(got);
                            if (got.itemcat == "I")
                            {
                                var pre1 = _BomService.getmatspares(got.Itemcode);

                                foreach (Prosol_MaterialBom no1 in pre1)
                                {
                                    var get1 = new Prosol_MaterialBom();
                                    get1.HeaderBID = no1.HeaderBID;
                                    get1.Itemcode = no1.Itemcode;
                                    get1.Shortdesc = no1.Shortdesc;
                                    get1.partqnt = no1.partqnt;
                                    get1.itemcat = no1.itemcat;
                                    tst.Add(get1);

                                }


                            }
                        }


                    }
                }
                track var1 = new track();
                var1.eqpbom = lst;
                var1.matbom = res;
                var1.mmatbom = tst;
                var jsonResult = Json(var1, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            else
            {

                return Json("true", JsonRequestBehavior.AllowGet);

            }

        }

        [Authorize]
        public JsonResult getitem(string sKey)
        {
            var Plst1 = new List<Prosol_Funloc>();
            var Plst2 = new List<Prosol_Funloc>();
            var Plst3 = new List<Prosol_Funloc>();
            var Plst4 = new List<Prosol_Funloc>();
            var Plst5 = new List<Prosol_Funloc>();
            var Plst6 = new List<Prosol_Funloc>();
            var Plst7 = new List<Prosol_Funloc>();
           
            var value = _BomService.gethei(sKey);
            if (value.Count != 0)
            {
                foreach (Prosol_Funloc v in value)
                {
                    Prosol_Funloc l1 = new Prosol_Funloc();
                    l1.SupFunctLoc = v.SupFunctLoc;
                    l1.FunctLocation = v.FunctLocation;
                    l1.FunctDesc = v.FunctDesc;
                    l1.EquipDesc = v.EquipDesc;
                    l1.Manufacturer = v.Manufacturer;
                    l1.ManufCon = v.ManufCon;
                    l1.Modelno = v.Modelno;
                    l1.ManufSerialNo = v.ManufSerialNo;
                    l1.FunctLocCat = v.FunctLocCat;
                    l1.Objecttype = v.Objecttype;
                    l1.Modelno = v.Modelno;
                    l1.TechIdentNo = v.TechIdentNo;
                    l1.mcpcode = v.mcpcode;
                    
                    var value1 = _BomService.gethei(v.FunctLocation);
                    if (value1.Count != 0)
                    {
                        l1.status = "yes";
                    }
                    else
                    {
                        l1.status = "no";
                    }
                    Plst1.Add(l1);
                 
                    if(value1.Count != 0)
                    {
                        foreach (Prosol_Funloc v1 in value1)
                        {
                            Prosol_Funloc l2 = new Prosol_Funloc();
                            l2.SupFunctLoc = v1.SupFunctLoc;
                            l2.FunctLocation = v1.FunctLocation;
                            l2.FunctDesc = v1.FunctDesc;
                            l2.EquipDesc = v1.EquipDesc;
                            l2.Manufacturer = v1.Manufacturer;
                            l2.ManufCon = v1.ManufCon;
                            l2.Modelno = v1.Modelno;
                            l2.ManufSerialNo = v1.ManufSerialNo;
                            l2.FunctLocCat = v1.FunctLocCat;
                            l2.Objecttype = v1.Objecttype;
                            l2.Modelno = v1.Modelno;
                            l2.TechIdentNo = v1.TechIdentNo;
                            l2.mcpcode = v1.mcpcode;
                            var value2 = _BomService.gethei(v1.FunctLocation);
                            if (value2.Count != 0)
                            {
                                l2.status = "yes";
                            }
                            else
                            {
                                l2.status = "no";
                            }
                            Plst2.Add(l2);
                           
                            if (value2.Count != 0)
                            {
                                foreach (Prosol_Funloc v2 in value2)
                                {
                                    Prosol_Funloc l3 = new Prosol_Funloc();
                                    l3.SupFunctLoc = v2.SupFunctLoc;
                                    l3.FunctLocation = v2.FunctLocation;
                                    l3.FunctDesc = v2.FunctDesc;
                                    l3.EquipDesc = v2.EquipDesc;
                                    l3.Manufacturer = v2.Manufacturer;
                                    l3.ManufCon = v2.ManufCon;
                                    l3.Modelno = v2.Modelno;
                                    l3.ManufSerialNo = v2.ManufSerialNo;
                                    l3.FunctLocCat = v2.FunctLocCat;
                                    l3.Objecttype = v2.Objecttype;
                                    l3.Modelno = v2.Modelno;
                                    l3.TechIdentNo = v2.TechIdentNo;
                                    l3.mcpcode = v2.mcpcode;
                                    var value3 = _BomService.gethei(v2.FunctLocation);
                                    if (value3.Count != 0)
                                    {
                                        l3.status = "yes";
                                    }
                                    else
                                    {
                                        l3.status = "no";
                                    }
                                    Plst3.Add(l3);
                                   
                                    if (value3.Count != 0)
                                    {
                                        foreach (Prosol_Funloc v3 in value3)
                                        {
                                            Prosol_Funloc l4 = new Prosol_Funloc();
                                            l4.SupFunctLoc = v3.SupFunctLoc;
                                            l4.FunctLocation = v3.FunctLocation;
                                            l4.FunctDesc = v3.FunctDesc;
                                            l4.EquipDesc = v3.EquipDesc;
                                            l4.Manufacturer = v3.Manufacturer;
                                            l4.ManufCon = v3.ManufCon;
                                            l4.Modelno = v3.Modelno;
                                            l4.ManufSerialNo = v3.ManufSerialNo;
                                            l4.FunctLocCat = v3.FunctLocCat;
                                            l4.Objecttype = v3.Objecttype;
                                            l4.Modelno = v3.Modelno;
                                            l4.TechIdentNo = v3.TechIdentNo;
                                            l4.mcpcode = v3.mcpcode;

                                            var value4 = _BomService.gethei(v3.FunctLocation);
                                            if (value4.Count != 0)
                                            {
                                                l4.status = "yes";
                                            }
                                            else
                                            {
                                                l4.status = "no";
                                            }
                                            Plst4.Add(l4);
                                         
                                            if (value4.Count != 0)
                                            {
                                                foreach (Prosol_Funloc v4 in value4)
                                                {
                                                    Prosol_Funloc l5 = new Prosol_Funloc();
                                                    l5.SupFunctLoc = v4.SupFunctLoc;
                                                    l5.FunctLocation = v4.FunctLocation;
                                                    l5.FunctDesc = v4.FunctDesc;
                                                    l5.EquipDesc = v4.EquipDesc;
                                                    l5.Manufacturer = v4.Manufacturer;
                                                    l5.ManufCon = v4.ManufCon;
                                                    l5.Modelno = v4.Modelno;
                                                    l5.ManufSerialNo = v4.ManufSerialNo;
                                                    l5.FunctLocCat = v4.FunctLocCat;
                                                    l5.Objecttype = v4.Objecttype;
                                                    l5.Modelno = v4.Modelno;
                                                    l5.TechIdentNo = v4.TechIdentNo;
                                                    l5.mcpcode = v4.mcpcode;

                                                    var value5 = _BomService.gethei(v4.FunctLocation);
                                                    if (value5.Count != 0)
                                                    {
                                                        l5.status = "yes";
                                                    }
                                                    else
                                                    {
                                                        l5.status = "no";
                                                    }
                                                    Plst5.Add(l5);
                                                    
                                                    if (value5.Count != 0)
                                                    {
                                                        foreach (Prosol_Funloc v5 in value5)
                                                        {
                                                            Prosol_Funloc l6 = new Prosol_Funloc();
                                                            l6.SupFunctLoc = v5.SupFunctLoc;
                                                            l6.FunctLocation = v5.FunctLocation;
                                                            l6.FunctDesc = v5.FunctDesc;
                                                            l6.EquipDesc = v5.EquipDesc;
                                                            l6.Manufacturer = v5.Manufacturer;
                                                            l6.ManufCon = v5.ManufCon;
                                                            l6.Modelno = v5.Modelno;
                                                            l6.ManufSerialNo = v5.ManufSerialNo;
                                                            l6.FunctLocCat = v5.FunctLocCat;
                                                            l6.Objecttype = v5.Objecttype;
                                                            l6.Modelno = v5.Modelno;
                                                            l6.TechIdentNo = v5.TechIdentNo;
                                                            l6.mcpcode = v5.mcpcode;
                                                            var value6 = _BomService.gethei(v5.FunctLocation);
                                                            if (value5.Count != 0)
                                                            {
                                                                l6.status = "yes";
                                                            }
                                                            else
                                                            {
                                                                l6.status = "no";
                                                            }
                                                            Plst6.Add(l6);
                                                        
                                                            if (value6.Count != 0)
                                                            {
                                                                foreach (Prosol_Funloc v6 in value6)
                                                                {
                                                                    Prosol_Funloc l7 = new Prosol_Funloc();
                                                                    l7.SupFunctLoc = v6.SupFunctLoc;
                                                                    l7.FunctLocation = v6.FunctLocation;
                                                                    l7.FunctDesc = v6.FunctDesc;
                                                                    l7.EquipDesc = v6.EquipDesc;
                                                                    l7.Manufacturer = v6.Manufacturer;
                                                                    l7.ManufCon = v6.ManufCon;
                                                                    l7.Modelno = v6.Modelno;
                                                                    l7.ManufSerialNo = v6.ManufSerialNo;
                                                                    l7.FunctLocCat = v6.FunctLocCat;
                                                                    l7.Objecttype = v6.Objecttype;
                                                                    l7.Modelno = v6.Modelno;
                                                                    l7.TechIdentNo = v6.TechIdentNo;
                                                                    l7.mcpcode = v6.mcpcode;
                                                                    Plst7.Add(l7);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
               
                }
                track var1 = new track();
                var1.fun1 = Plst1;
                var1.fun2 = Plst2;
                var1.fun3 = Plst3;
                var1.fun4 = Plst4;
                var1.fun5 = Plst5;
                var1.fun6 = Plst6;
                var1.fun7 = Plst7;
              
                var jsonResult = Json(var1, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

            }
            else
            {

                return Json("true", JsonRequestBehavior.AllowGet);

            }
        }


        public class track
        {
            public List<Prosol_Funloc> fun1 { set; get; }
            public List<Prosol_Funloc> fun2 { set; get; }
            public List<Prosol_Funloc> fun3 { set; get; }
            public List<Prosol_Funloc> fun4 { set; get; }
            public List<Prosol_Funloc> fun5 { set; get; }
            public List<Prosol_Funloc> fun6 { set; get; }
            public List<Prosol_Funloc> fun7 { set; get; }
           
            public List<Prosol_equipbom> eqpbom { set; get; }
            public List<Prosol_MaterialBom> matbom { set; get; }
            public List<Prosol_MaterialBom> mmatbom { set; get; }

            public List<Prosol_Funloc> lst { set; get; }
            public int CurrentPageIndex { get; set; }
            public int totItem { get; set; }
          
            public int PageCount { get; set; }

        }
        //Bulk Data upload
        [Authorize]
        public JsonResult BulkFunloc()
        {
            int Res1 = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    Res1 = _BomService.BulkFunloc(file);
                }
            }
            return this.Json(Res1, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult BulkEquip()
        {
            int Res1 = 0;
            var file = Request.Files.Count > 0 ? Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                {
                    Res1 = _BomService.BulkEquip(file);
                }
            }
            return this.Json(Res1, JsonRequestBehavior.AllowGet);
        }


        //public class clsss
        //{
        //    public List<FunlocModel> listtt { get; set; }
        //    public string emptyval { get; set; }
        //}

        [Authorize]
        public JsonResult BulkBom(string sKey)
        {

            TempData["value"] = sKey;
            var srch = _BomService.funlocsearch(sKey);
            var lstfunloc = new List<FunlocModel>();
            //string emptyval = "";
            foreach (Prosol_Funloc fun in srch)
            {
                //var result1 = getitem1(fun.TechIdentNo);
                //if (result1.Count == 0)
                //{
                //    emptyval = emptyval + " , " + fun.TechIdentNo;
                //}
                var pro = new FunlocModel();
                var srch1 = _BomService.funequip(fun.TechIdentNo);
                if (srch1.Count != 0)
                {
                    pro.status = "Completed";
                }

                pro._id = fun._id.ToString();
                pro.FunctLocation = fun.FunctLocation;
                pro.FunctDesc = fun.FunctDesc;
                pro.SupFunctLoc = fun.SupFunctLoc;
                pro.Objecttype = fun.Objecttype;
                pro.TechIdentNo = fun.TechIdentNo;
                pro.EquipDesc = fun.EquipDesc;
                pro.Manufacturer = fun.Manufacturer;
                pro.ManufCon = fun.ManufCon;
                pro.Modelno = fun.Modelno;
                pro.ManufSerialNo = fun.ManufSerialNo;

                lstfunloc.Add(pro);


            }

            //clsss res = new clsss();
            //res.listtt = lstfunloc;
            //res.emptyval = emptyval;
            var jsonResult = Json(lstfunloc, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }



        [Authorize]
        public List<ProsolEXBom> getitem1(string sKey)
        {

            var get = _BomService.getspares(sKey);

            var lst = new List<ProsolEXBom>();
            if (get.Count != 0)
            {

                foreach (Prosol_equipbom fun in get)
                {
                    string FunctLocation = "";
                    string FunctDesc = "";
                    string Objecttype = "";
                    string TechIdentNo = "";
                    string EquipDesc = "";
                    string Manufacturer = "";
                    string Modelno = "";
                    string ManufSerialNo = "";
                    string Itemcode1 = "";
                    string Shortdesc1 = "";
                    string Itemcode2 = "";
                    string Shortdesc2 = "";
                    string Itemcode3 = "";
                    string Shortdesc3 = "";
                    string Itemcode4 = "";
                    string Shortdesc4 = "";

                    FunctLocation = fun.FunctLocation;
                    FunctDesc = fun.FunctDesc;
                    Objecttype = fun.Objecttype;
                    TechIdentNo = fun.TechIdentNo;
                    EquipDesc = fun.EquipDesc;
                    Manufacturer = fun.Manufacturer;
                    Modelno = fun.Modelno;
                    ManufSerialNo = fun.ManufSerialNo;
                    if (fun.itemcat == "I")
                    {
                        Itemcode1 = fun.Itemcode;
                        Shortdesc1 = fun.Shortdesc;

                        var pre = _BomService.getmatspares(fun.Itemcode);

                        foreach (Prosol_MaterialBom no in pre)
                        {
                            if (no.itemcat == "I")
                            {
                                Itemcode2 = no.Itemcode;
                                Shortdesc2 = no.Shortdesc;

                                var pre1 = _BomService.getmatspares(no.Itemcode);
                                foreach (Prosol_MaterialBom no1 in pre1)
                                {
                                    if (no1.itemcat == "I")
                                    {
                                        Itemcode3 = no1.Itemcode;
                                        Shortdesc3 = no1.Shortdesc;

                                        var pre2 = _BomService.getmatspares(no1.Itemcode);
                                        foreach (Prosol_MaterialBom no2 in pre2)
                                        {
                                            if (no1.itemcat == "I")
                                            {

                                                Itemcode4 = no2.Itemcode;
                                                Shortdesc4 = no2.Shortdesc;
                                                var pre3 = _BomService.getmatspares(no2.Itemcode);
                                                foreach (Prosol_MaterialBom no3 in pre3)
                                                {
                                                    var pro4 = new ProsolEXBom();
                                                    pro4.Flo = FunctLocation;
                                                    pro4.FlocText = FunctDesc;
                                                    pro4.Object_Type = Objecttype;
                                                    pro4.Eqno = TechIdentNo;
                                                    pro4.Equipment_Text = EquipDesc;
                                                    pro4.MNFR = Manufacturer;
                                                    pro4.MNFR_model = Modelno;
                                                    pro4.MNFR_serial = ManufSerialNo;
                                                    pro4.CostructID_1 = Itemcode1;
                                                    pro4.CostructID_1_short = Shortdesc1;
                                                    pro4.CostructID_2 = Itemcode2;
                                                    pro4.CostructID_2_short = Shortdesc2;
                                                    pro4.CostructID_3 = Itemcode3;
                                                    pro4.CostructID_3_short = Shortdesc3;
                                                    pro4.CostructID_4 = Itemcode4;
                                                    pro4.CostructID_4_short = Shortdesc4;
                                                    pro4.Bom_Spare_Id = no3.Itemcode;
                                                    pro4.Short = no3.Shortdesc;
                                                    pro4.Category = no3.itemcat;
                                                    pro4.Part_Quantity = no3.partqnt;
                                                    lst.Add(pro4);
                                                }
                                            }
                                            else
                                            {
                                                var pro4 = new ProsolEXBom();
                                                pro4.Flo = FunctLocation;
                                                pro4.FlocText = FunctDesc;
                                                pro4.Object_Type = Objecttype;
                                                pro4.Eqno = TechIdentNo;
                                                pro4.Equipment_Text = EquipDesc;
                                                pro4.MNFR = Manufacturer;
                                                pro4.MNFR_model = Modelno;
                                                pro4.MNFR_serial = ManufSerialNo;
                                                pro4.CostructID_1 = Itemcode1;
                                                pro4.CostructID_1_short = Shortdesc1;
                                                pro4.CostructID_2 = Itemcode2;
                                                pro4.CostructID_2_short = Shortdesc2;
                                                pro4.CostructID_3 = Itemcode3;
                                                pro4.CostructID_3_short = Shortdesc3;
                                                pro4.Bom_Spare_Id = no2.Itemcode;
                                                pro4.Short = no2.Shortdesc;
                                                pro4.Category = no2.itemcat;
                                                pro4.Part_Quantity = no2.partqnt;


                                                lst.Add(pro4);
                                            }
                                            // lst.Add(pro);
                                        }
                                    }
                                    else
                                    {
                                        var pro4 = new ProsolEXBom();
                                        pro4.Flo = FunctLocation;
                                        pro4.FlocText = FunctDesc;
                                        pro4.Object_Type = Objecttype;
                                        pro4.Eqno = TechIdentNo;
                                        pro4.Equipment_Text = EquipDesc;
                                        pro4.MNFR = Manufacturer;
                                        pro4.MNFR_model = Modelno;
                                        pro4.MNFR_serial = ManufSerialNo;
                                        pro4.CostructID_1 = Itemcode1;
                                        pro4.CostructID_1_short = Shortdesc1;
                                        pro4.CostructID_2 = Itemcode2;
                                        pro4.CostructID_2_short = Shortdesc2;
                                        pro4.Bom_Spare_Id = no1.Itemcode;
                                        pro4.Short = no1.Shortdesc;
                                        pro4.Category = no1.itemcat;
                                        pro4.Part_Quantity = no1.partqnt;
                                        lst.Add(pro4);
                                    }
                                    // lst.Add(pro);
                                }
                            }
                            else
                            {
                                var pro4 = new ProsolEXBom();
                                pro4.Flo = FunctLocation;
                                pro4.FlocText = FunctDesc;
                                pro4.Object_Type = Objecttype;
                                pro4.Eqno = TechIdentNo;
                                pro4.Equipment_Text = EquipDesc;
                                pro4.MNFR = Manufacturer;
                                pro4.MNFR_model = Modelno;
                                pro4.MNFR_serial = ManufSerialNo;
                                pro4.CostructID_1 = Itemcode1;
                                pro4.CostructID_1_short = Shortdesc1;
                                pro4.Bom_Spare_Id = no.Itemcode;
                                pro4.Short = no.Shortdesc;
                                pro4.Category = no.itemcat;
                                pro4.Part_Quantity = no.partqnt;
                                lst.Add(pro4);
                            }
                        }
                        //  lst.Add(pro);
                    }
                    else
                    {
                        var pro = new ProsolEXBom();
                        pro.Flo = FunctLocation;
                        pro.FlocText = FunctDesc;
                        pro.Object_Type = Objecttype;
                        pro.Eqno = TechIdentNo;
                        pro.Equipment_Text = EquipDesc;
                        pro.MNFR = Manufacturer;
                        pro.MNFR_model = Modelno;
                        pro.MNFR_serial = ManufSerialNo;
                        pro.Bom_Spare_Id = fun.Itemcode;
                        pro.Short = fun.Shortdesc;
                        pro.Category = fun.itemcat;
                        pro.Part_Quantity = fun.partqnt;
                        lst.Add(pro);
                    }
                    //  lst.Add(pro);
                }
            }

            return lst;
        }


        //[Authorize]
        //public ress getitem1(string sKey)
        //{
        //    var get = _BomService.getspares(sKey);
        //    var lst = new List<Prosol_equipbom>();
        //    var res = new List<Prosol_MaterialBom>();
        //    var tst = new List<Prosol_MaterialBom>();

        //    if (get.Count != 0)
        //    {
        //        foreach (Prosol_equipbom fun in get)
        //        {
        //            var pro = new Prosol_equipbom();
        //            pro.FunctLocation = fun.FunctLocation;
        //            pro.FunctDesc = fun.FunctDesc;
        //            pro.SupFunctLoc = fun.SupFunctLoc;
        //            pro.Objecttype = fun.Objecttype;
        //            pro.TechIdentNo = fun.TechIdentNo;
        //            pro.EquipDesc = fun.EquipDesc;
        //            pro.Manufacturer = fun.Manufacturer;
        //            pro.ManufCon = fun.ManufCon;
        //            pro.Modelno = fun.Modelno;
        //            pro.Itemcode = fun.Itemcode;
        //            pro.Shortdesc = fun.Shortdesc;
        //            pro.partqnt = fun.partqnt;
        //            pro.itemcat = fun.itemcat;
        //            lst.Add(pro);

        //            if (pro.itemcat == "I")
        //            {
        //                var pre = _BomService.getmatspares(pro.Itemcode);

        //                foreach (Prosol_MaterialBom no in pre)
        //                {
        //                    var got = new Prosol_MaterialBom();
        //                    got.HeaderBID = no.HeaderBID;
        //                    got.Itemcode = no.Itemcode;
        //                    got.Shortdesc = no.Shortdesc;
        //                    got.partqnt = no.partqnt;
        //                    got.itemcat = no.itemcat;
        //                    res.Add(got);
        //                    if (got.itemcat == "I")
        //                    {
        //                        var pre1 = _BomService.getmatspares(got.Itemcode);

        //                        foreach (Prosol_MaterialBom no1 in pre1)
        //                        {
        //                            var get1 = new Prosol_MaterialBom();
        //                            get1.HeaderBID = no1.HeaderBID;
        //                            get1.Itemcode = no1.Itemcode;
        //                            get1.Shortdesc = no1.Shortdesc;
        //                            get1.partqnt = no1.partqnt;
        //                            get1.itemcat = no1.itemcat;
        //                            tst.Add(get1);

        //                        } 


        //                    }
        //                }


        //            }
        //        }
        //        ress var1 = new ress();
        //        var1.r_ebom = lst;
        //        var1.r_m1bom = res;
        //        var1.r_m2bom = tst;

        //        return var1;

        //    }
        //    else
        //    {

        //        return null;

        //    }

        //}


        [HttpGet]
        public void Download(string sKey)
        {
            var result = getitem1(sKey);
            var strJson = JsonConvert.SerializeObject(result);
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "BomTrack.xls"));
            Response.ContentType = "application/ms-excel";
            string str = string.Empty;


            var emptyRows =
                       dt.Select()
                           .Where(
                               row =>
                                   dt.Columns.Cast<DataColumn>()
                                       .All(column => string.IsNullOrEmpty(row[column].ToString()))).ToArray();
            Array.ForEach(emptyRows, x => x.Delete());

            var emptyColumns =
                dt.Columns.Cast<DataColumn>()
                    .Where(column => dt.Select().All(row => string.IsNullOrEmpty(row[column].ToString())))
                    .ToArray();
            Array.ForEach(emptyColumns, column => dt.Columns.Remove(column));
            dt.AcceptChanges();



            for (int i = 0; i < dt.Columns.Count; i++)

            {
                Response.Write(dt.Columns[i].ToString().ToUpper() + "\t");
            }
            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dr[j].ToString().Contains('\n'))
                    {
                        dr[j] = dr[j].ToString().Replace('\n', ' ');
                    }
                    Response.Write(str + dr[j]);
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.Flush();
            Response.End();



        }

        [Authorize]

        public void BulkDownload()

        {
            string item1 = TempData["value"].ToString();
            var srch = _BomService.funlocsearch(item1);
            List<ProsolEXBom> lst = new List<ProsolEXBom>();
            foreach (Prosol_Funloc s in srch)
            {

                var result1 = getitem1(s.TechIdentNo);
                if (result1 != null)
                {
                    foreach (ProsolEXBom r in result1)
                    {
                        lst.Add(r);
                    }
                }
            }
            if (lst.Count != 0)
            {
                var strJson = JsonConvert.SerializeObject(lst);
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "BulkBomTrack.xls"));
                Response.ContentType = "application/ms-excel";
                string str = string.Empty;


                var emptyRows =
                           dt.Select()
                               .Where(
                                   row =>
                                       dt.Columns.Cast<DataColumn>()
                                           .All(column => string.IsNullOrEmpty(row[column].ToString()))).ToArray();
                Array.ForEach(emptyRows, x => x.Delete());

                var emptyColumns =
                    dt.Columns.Cast<DataColumn>()
                        .Where(column => dt.Select().All(row => string.IsNullOrEmpty(row[column].ToString())))
                        .ToArray();
                Array.ForEach(emptyColumns, column => dt.Columns.Remove(column));
                dt.AcceptChanges();



                for (int i = 0; i < dt.Columns.Count; i++)

                {
                    Response.Write(dt.Columns[i].ToString().ToUpper() + "\t");
                }
                Response.Write("\n");
                foreach (DataRow dr in dt.Rows)
                {
                    str = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dr[j].ToString().Contains('\n'))
                        {
                            dr[j] = dr[j].ToString().Replace('\n', ' ');
                        }
                        Response.Write(str + dr[j]);
                        str = "\t";
                    }
                    Response.Write("\n");
                }
                Response.Flush();
                Response.End();

            }

        }

        [Authorize]
        public JsonResult getreport(string sKey, bool inda, bool indb, bool indc)
        {
            var ee = new List<EquipBomModel>();
            var report = _BomService.getreport(sKey, inda, indb, indc).ToList();
            if (report.Count == 0)
            {
                var matcheck = _BomService.getreport12(sKey).ToList();

                foreach (Prosol_MaterialBom E in matcheck)
                {


                    var w = _BomService.getreport(E.HeaderBID, inda, indb, indc).ToList();
                    if (w.Count == 0)
                    {
                        var MQ = _BomService.getreport12(E.HeaderBID).ToList();
                        foreach (Prosol_MaterialBom mQ1 in MQ)
                        {

                            var w1 = _BomService.getreport(mQ1.HeaderBID, inda, indb, indc).ToList();


                            foreach (Prosol_equipbom EQ in w1)
                            {

                                var e = new EquipBomModel();

                                e.TechIdentNo = EQ.TechIdentNo;
                                e.FunctLocation = EQ.FunctLocation;
                                e.FunctDesc = EQ.FunctDesc;
                                e.partqnt = Convert.ToInt16(E.partqnt);
                                e.ABCindic = EQ.ABCindic;

                                ee.Add(e);

                            }



                        }
                    }
                    else
                    {


                        foreach (Prosol_equipbom EQ in w)
                        {

                            var e = new EquipBomModel();

                            e.TechIdentNo = EQ.TechIdentNo;
                            e.FunctLocation = EQ.FunctLocation;
                            e.FunctDesc = EQ.FunctDesc;
                            e.partqnt = Convert.ToInt16(E.partqnt);
                            e.ABCindic = EQ.ABCindic;

                            ee.Add(e);

                        }
                    }
                }
            }

            foreach (Prosol_equipbom EQ in report)
            {
                if (EQ.itemcat != "I")
                {
                    var e = new EquipBomModel();

                    e.TechIdentNo = EQ.TechIdentNo;
                    e.FunctLocation = EQ.FunctLocation;
                    e.FunctDesc = EQ.FunctDesc;
                    e.partqnt = Convert.ToInt16(EQ.partqnt);
                    e.ABCindic = EQ.ABCindic;
                    ee.Add(e);
                }
            }

            return Json((ee), JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public void Download2(string sKey, bool inda, bool indb, bool indc)
        {

            var ee = new List<EquipBomModel>();
            var report = _BomService.getreport(sKey, inda, indb, indc).ToList();
            if (report.Count == 0)
            {
                var matcheck = _BomService.getreport12(sKey).ToList();

                foreach (Prosol_MaterialBom E in matcheck)
                {


                    var w = _BomService.getreport(E.HeaderBID, inda, indb, indc).ToList();
                    if (w.Count == 0)
                    {
                        var MQ = _BomService.getreport12(E.HeaderBID).ToList();
                        foreach (Prosol_MaterialBom mQ1 in MQ)
                        {

                            var w1 = _BomService.getreport(mQ1.HeaderBID, inda, indb, indc).ToList();


                            foreach (Prosol_equipbom EQ in w1)
                            {

                                var e = new EquipBomModel();

                                e.TechIdentNo = EQ.TechIdentNo;
                                e.FunctLocation = EQ.FunctLocation;
                                e.FunctDesc = EQ.FunctDesc;
                                e.partqnt = Convert.ToInt16(E.partqnt);
                                e.ABCindic = EQ.ABCindic;
                            
                                ee.Add(e);

                            }



                        }
                    }
                    else
                    {


                        foreach (Prosol_equipbom EQ in w)
                        {

                            var e = new EquipBomModel();

                            e.TechIdentNo = EQ.TechIdentNo;
                            e.FunctLocation = EQ.FunctLocation;
                            e.FunctDesc = EQ.FunctDesc;
                            e.partqnt = Convert.ToInt16(E.partqnt);
                            e.ABCindic = EQ.ABCindic;
                          
                            ee.Add(e);

                        }
                    }
                }
            }

            foreach (Prosol_equipbom EQ in report)
            {
                if (EQ.itemcat != "I")
                {
                    var e = new EquipBomModel();

                    e.TechIdentNo = EQ.TechIdentNo;
                    e.FunctLocation = EQ.FunctLocation;
                    e.FunctDesc = EQ.FunctDesc;
                    e.partqnt = Convert.ToInt16(EQ.partqnt);
                    e.ABCindic = EQ.ABCindic;
                  
                    ee.Add(e);
                }
            }

            var strJson = JsonConvert.SerializeObject(ee);
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "BomReport.xls"));
            Response.ContentType = "application/ms-excel";
            string str = string.Empty;


            var emptyRows =
                       dt.Select()
                           .Where(
                               row =>
                                   dt.Columns.Cast<DataColumn>()
                                       .All(column => string.IsNullOrEmpty(row[column].ToString()))).ToArray();
            Array.ForEach(emptyRows, x => x.Delete());

            var emptyColumns =
                dt.Columns.Cast<DataColumn>()
                    .Where(column => dt.Select().All(row => string.IsNullOrEmpty(row[column].ToString())))
                    .ToArray();
            Array.ForEach(emptyColumns, column => dt.Columns.Remove(column));
            dt.AcceptChanges();



            for (int i = 0; i < dt.Columns.Count; i++)

            {
                Response.Write(dt.Columns[i].ToString().ToUpper() + "\t");
            }
            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dr[j].ToString().Contains('\n'))
                    {
                        dr[j] = dr[j].ToString().Replace('\n', ' ');
                    }
                    Response.Write(str + dr[j]);
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.Flush();
            Response.End();



        }
        //master

        [Authorize]
        public void Downloadmaster()
        {
            var result = _BomService.master().ToList();
            var strJson = JsonConvert.SerializeObject(result);
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Equipment Master.xls"));
            Response.ContentType = "application/ms-excel";
            string str = string.Empty;
            dt.Columns.Remove("_ID");

            for (int i = 0; i < dt.Columns.Count; i++)

            {
                Response.Write(dt.Columns[i].ToString().ToUpper() + "\t");
            }
            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dr[j].ToString().Contains('\n'))
                    {
                        dr[j] = dr[j].ToString().Replace('\n', ' ');
                    }
                    Response.Write(str + dr[j]);
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.Flush();
            Response.End();



        }
        
             [Authorize]
        public void Downloadmasterdata()
        {
            var result = _BomService.masterdata().ToList();
            var strJson = JsonConvert.SerializeObject(result);
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(strJson, (typeof(DataTable)));
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Material Master.xls"));
            Response.ContentType = "application/ms-excel";
            string str = string.Empty;
            dt.Columns.Remove("_ID");

            for (int i = 0; i < dt.Columns.Count; i++)

            {
                Response.Write(dt.Columns[i].ToString().ToUpper() + "\t");
            }
            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dr[j].ToString().Contains('\n'))
                    {
                        dr[j] = dr[j].ToString().Replace('\n', ' ');
                    }
                    Response.Write(str + dr[j]);
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.Flush();
            Response.End();



        }
        public class total2
        {

            public List<Prosol_Vendor> vendor { set; get; }
            public List<total3> tag { set; get; }

        }
        public class total3
        {

            public string Tag { set; get; }
            public string Modelno { set; get; }
            public string ManufSerialNo { set; get; }
            public string Units { set; get; }

        }
        [Authorize]
        public JsonResult get(string key1)
        {
            var mand = _BomService.mandcsearch(key1);


            var man = _BomService.manfacsearch(key1);
            var res = man.ToDictionary(x => x);
            var lstfunloc = new List<total3>();
            foreach (Prosol_equipbom fun in man)
            {
                var e = new total3();

                e.Tag = fun.FunctLocation;
                e.Modelno = fun.Modelno;
                e.ManufSerialNo = fun.ManufSerialNo;
                e.Units = "1";
                lstfunloc.Add(e);


            }

            total2 c = new total2();
            {
                c.vendor = mand.ToList();
                c.tag = lstfunloc.ToList();
            }
            return Json((c), JsonRequestBehavior.AllowGet);
        }

        //dashboard
        public class total
        {
            public int A { set; get; }
            public int B { set; get; }
            public int C { set; get; }
            // public List<Prosol_equipbom> AA { set; get; }
            public int AA { get; set; }
            public int BB { set; get; }
            public int CC { set; get; }
            public int count { set; get; }
            public int sub { set; get; }
            public int matcount { set; get; }
            public int count2 { set; get; }
            public int Sub1 { set; get; }
            public List<sub> typemat { set; get; }

        }
        public class sub
        {
            public string code { get; set; }
            public int cunt { get; set; }
        }
        [Authorize]
        public JsonResult Total()
        {
            int cnt = 0;
            var A = new List<Prosol_Funloc>();
            var B = new List<Prosol_Funloc>();
            var C = new List<Prosol_Funloc>();
            var AA = new List<Prosol_equipbom>();
            var BB = new List<Prosol_equipbom>();
            var CC = new List<Prosol_equipbom>();
            var R = new List<Prosol_Funloc>();

            var report = _BomService.Total().ToList();
            foreach (Prosol_Funloc mdl in report)
            {
                if(mdl.TechIdentNo != null)
                {

                if (mdl.ABCindic == "A")
                {
                    A.Add(mdl);
                }
                if (mdl.ABCindic == "B")
                {
                    B.Add(mdl);
                }
                if (mdl.ABCindic == "C")
                {
                    C.Add(mdl);
                }
                    cnt++;
                }
            }
            var report2 = _BomService.complete().ToList();

            var distinctList = report2.DistinctBy(x => x.TechIdentNo).ToList();

            foreach (Prosol_equipbom mdl in distinctList)
            {
                if (mdl.ABCindic == "A")
                {
                    AA.Add(mdl);
                }
                if (mdl.ABCindic == "B")
                {
                    BB.Add(mdl);
                }
                if (mdl.ABCindic == "C")
                {
                    CC.Add(mdl);
                }

            }




            int Subtract;
            Subtract = cnt - distinctList.Count;
            total all = new total();
            all.A = A.Count();
            all.B = B.Count();
            all.C = C.Count();
            all.AA = AA.Count();
            all.BB = BB.Count();
            all.CC = CC.Count();
            all.count = cnt;
            all.sub = Subtract;

            return Json((all), JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult Total2()
        {
            var ee = new List<EquipBomModel>();

            var matype = new List<sub>();
            var material = _BomService.totalmat().ToList();
            var mat = _BomService.allmat().ToList();
            var mat2 = _BomService.allmat2().ToList();
            var rr = mat.DistinctBy(x => x.Itemcode).ToList();
            var rr1 = mat2.DistinctBy(x => x.HeaderBID).ToList();
            var rr2 = mat2.DistinctBy(x => x.Itemcode).ToList();
            var rr3 = rr1.Concat(rr2).ToList();
            var rr4 = rr3.DistinctBy(x => x.Itemcode).ToList();


            int Subtract1;
            Subtract1 = material.Count - rr4.Count;

            var mattype = _BomService.matty("Material type").ToList();

            foreach (Prosol_Master md in mattype)

            {

                sub e = new sub();
                var pre = _BomService.Codecount(md.Code);
                e.code = md.Code;
                e.cunt = pre.Count();
                matype.Add(e);
            }
            total all = new total();
            all.matcount = material.Count();
            all.count2 = rr4.Count();
            all.typemat = matype.ToList();
            all.Sub1 = Subtract1;
            return Json((all), JsonRequestBehavior.AllowGet);

        }
        public class multiplecodelist1
        {
            public string Materialcode { get; set; }
            public string FunctLocation { get; set; }
            public string TechIdentNo { get; set; }
            public string INtime { get; set; }
            public string OUTtime { get; set; }
            public string Createdon { get; set; }
        }
        public class PagingGroup
        {
            ///<summary>
            /// Gets or sets Customers.
            ///</summary>
            public List<Prosol_Funloc> FunlocList { get; set; }

            ///<summary>
            /// Gets or sets CurrentPageIndex.
            ///</summary>
            public int CurrentPageIndex { get; set; }
            public int totItem { get; set; }
            ///<summary>
            /// Gets or sets PageCount.
            ///</summary>
            public int PageCount { get; set; }
        }
        public class PagingGroupfun
        {
            ///<summary>
            /// Gets or sets Customers.
            ///</summary>
            public List<FunlocModel> FunlocList { get; set; }

            ///<summary>
            /// Gets or sets CurrentPageIndex.
            ///</summary>
            public int CurrentPageIndex { get; set; }
            public int totItem { get; set; }
            ///<summary>
            /// Gets or sets PageCount.
            ///</summary>
            public int PageCount { get; set; }
        }
        public class PagingGroup1
        {
            ///<summary>
            /// Gets or sets Customers.
            ///</summary>
            public List<CatalogueModel> matList { get; set; }

            ///<summary>
            /// Gets or sets CurrentPageIndex.
            ///</summary>
            public int CurrentPageIndex { get; set; }
            public int totItem { get; set; }
            ///<summary>
            /// Gets or sets PageCount.
            ///</summary>
            public int PageCount { get; set; }
        }
    }
}

