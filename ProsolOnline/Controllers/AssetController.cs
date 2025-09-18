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
    public class AssetController : Controller
    {
        private readonly I_Bom _BomService;
      
        public AssetController(I_Bom BomService)
        {
            _BomService = BomService;
         

        }
        [Authorize]
        public ActionResult FunctLoc()
        {
            if (CheckAccess("Functional Location") == 1)
                return View("FunctLoc");
            else if (CheckAccess("Functional Location") == 0)
                return View("Denied");
            else return View("Login");

        }

        [Authorize]
        public ActionResult MP()
        {        
            return View();

        }

        [Authorize]
        public ActionResult Equip()
        {
            if (CheckAccess("Equipment") == 1)
                return View("Equip");
            else if (CheckAccess("Equipment") == 0)
                return View("Denied");
            else return View("Login");

        }
        [Authorize]
        public ActionResult AssetHie()
        {
            if (CheckAccess("Asset Hierarchy") == 1)
                return View("AssetHie");
            else if (CheckAccess("Asset Hierarchy") == 0)
                return View("Denied");
            else return View("Login");

        }
        public ActionResult MCP()

        {
            if (CheckAccess("Asset Hierarchy") == 1)
                return View("MCP");
            else if (CheckAccess("Asset Hierarchy") == 0)
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
        public JsonResult DelFun(string id)
        {

            var delfun = _BomService.delfun(id);


            return this.Json(delfun, JsonRequestBehavior.AllowGet);
        }


     

        [Authorize]
        public JsonResult CreateFun()
        {
            var item = Request.Form["Data"];
            FunlocModel Fun = JsonConvert.DeserializeObject<FunlocModel>(item);
            var update = Request.Form["Update"];

            if(update == "yes")
            {
                DelFun(Fun._id);
            }
            


            var Mdl = new Prosol_Funloc();

            Mdl.FunctLocation = Fun.FunctLocation;
            Mdl.FunctLocCat = Fun.FunctLocCat;
            Mdl.FunctDesc = Fun.FunctDesc;
            Mdl.ABCindic = Fun.ABCindic;
            Mdl.SupFunctLoc = Fun.SupFunctLoc;
            Mdl.Objecttype = Fun.Objecttype;
            Mdl.Startdate = Fun.Startdate;
            Mdl.AuthGroup = Fun.AuthGroup;
            Mdl.MainWrk = Fun.MainWrk;
            Mdl.WBSelement = Fun.WBSelement;
            Mdl.Plantsection = Fun.Plantsection;
            Mdl.Catalogprofile = Fun.Catalogprofile;
            Mdl.Singleinst = Fun.Singleinst;
            Mdl.Manufacturer = Fun.Manufacturer;
            Mdl.ManufCon = Fun.ManufCon;
            Mdl.Modelno = Fun.Modelno;
            Mdl.ManufSerialNo = Fun.ManufSerialNo;
            Mdl.FunclocClass1 = Fun.FunclocClass1;

            Mdl.AuthGroup = Fun.AuthGroup;
            Mdl.Comment = Fun.Comment;
            Mdl.Asset = Fun.Asset;

            bool res = _BomService.Singlefun(Mdl);
            //    LstNM.Add(Mdl);



            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult CreateEquip()
        {
            var item = Request.Form["EquipData"];
            FunlocModel Fun = JsonConvert.DeserializeObject<FunlocModel>(item);
            var update = Request.Form["Update"];

            if (update == "yes")
            {
                DelEquip(Fun.TechIdentNo);
            }
            var Mdl = new Prosol_Funloc();
            Mdl.TechIdentNo = Fun.TechIdentNo;
            Mdl.FunctLocation = Fun.FunctLocation;
            Mdl.EquipDesc = Fun.EquipDesc;
            Mdl.EquipCategory = Fun.EquipCategory;
            Mdl.Weight = Fun.Weight;
            Mdl.UOM = Fun.UOM;
            Mdl.Size = Fun.Size;
            Mdl.AcquisValue = Fun.AcquisValue;
            Mdl.AcquistnDat = Fun.AcquistnDat;
            Mdl.Manufacturer = Fun.Manufacturer;
            Mdl.ManufCon = Fun.ManufCon;
            Mdl.Modelno = Fun.Modelno;
            Mdl.ConstructYear = Fun.ConstructYear;
            Mdl.ConstructMth = Fun.ConstructMth;
            Mdl.ManufPartNo = Fun.ManufPartNo;
            Mdl.ManufSerialNo = Fun.ManufSerialNo;
            Mdl.AuthGroup = Fun.AuthGroup;
            Mdl.Startupdate = Fun.Startupdate;
            Mdl.MaintPlant = Fun.MaintPlant;
            Mdl.Companycode = Fun.Companycode;
            Mdl.Asset = Fun.Asset;
            Mdl.Subno = Fun.Subno;
            Mdl.Catalogprofile = Fun.Catalogprofile;
            Mdl.Mainworkcenter = Fun.Mainworkcenter;
            Mdl.ConID = Fun.ConID;
            bool res = _BomService.Equipsingle(Mdl);
           
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult GenerateTechIdentNo()
        {
            Prosol_Funloc Mdl = new Prosol_Funloc();
            List<Prosol_Funloc> lsttec = new List<Prosol_Funloc>();
            var res = _BomService.funlocsearch2();
            int tn_i = 0;
            foreach (Prosol_Funloc r in res)
            {

                if (r.TechIdentNo != null)
                {
                    if (tn_i == 0)
                        tn_i = Convert.ToInt32(r.TechIdentNo);
                    else
                    {
                        if (tn_i < Convert.ToInt32(r.TechIdentNo))
                            tn_i = Convert.ToInt32(r.TechIdentNo);
                    }
                }


            }

            string techid = "";

            if (tn_i == 0)
            {
                techid = "10001";
            }

            else
            {
                techid = (tn_i + 1).ToString();
            }

            return this.Json(techid, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public JsonResult getitem(string sKey)
        {
            var l1 = new Prosol_Funloc();
            var l2 = new Prosol_equipbom();
         
            List<Prosol_Funloc> lst1 = new List<Prosol_Funloc>();
            List<Prosol_equipbom> lst2 = new List<Prosol_equipbom>();
          
            var value = _BomService.gethei(sKey);

            foreach(Prosol_Funloc v in value)
            {
               
                v.FunctLocation = l1.FunctLocation;
                v.FunctLocCat = l1.FunctLocCat;
                v.Objecttype = l1.Objecttype;
                v.Modelno = l1.Modelno;
                lst1.Add(v);

                var g2 = _BomService.get2(v.FunctLocation);

                foreach(Prosol_equipbom g in g2)
                {
                    g.Itemcode = l2.Itemcode;
                }


            }

            return this.Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getmcp( string eqpmnt, string model, string make)
        {
           var res =  _BomService.getmcpforMP(eqpmnt, model, make);

            if(res!= null)
                return Json(res, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);

        }


        public JsonResult getfunloc()
        {
            var srch = _BomService.getFUNLOCList();
            var hsn = new List<fun_loc>();
            foreach (Prosol_Funloc F in srch)
            {
                var pro = new fun_loc();

                pro.TechIdentNo = F.TechIdentNo;
                pro.FunctLocation = F.FunctLocation;
                pro.EquipDesc = F.EquipDesc;

                hsn.Add(pro);
            }
         
            var jsonResult = Json(hsn, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }


        public JsonResult getfunc_loc(string fl)
        {
            var res = _BomService.getfunc_loc(fl);
            if (res != null)
            {
                List<Prosol_Funloc> fll = new List<Prosol_Funloc>();
                fll.Add(res);
                return Json(fll, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        public JsonResult get_mcp_list()
        {
            var mcp_list = _BomService.get_mcp_list();

            return this.Json(mcp_list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult remove_mcp(string mcpcode)
        {
            var res = _BomService.remove_mcp(mcpcode);

            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
            
        public JsonResult updateMP()
        {
            var obj = Request.Form["obj"];
            Prosol_MaintenancePlan pm = JsonConvert.DeserializeObject<Prosol_MaintenancePlan>(obj);
            pm.UpdatedDate = DateTime.UtcNow.Date.ToString();
            pm.UpdatedBy = Session["username"].ToString();

            var res = _BomService.updateMP(pm);
            return Json(res, JsonRequestBehavior.AllowGet);
        }


       // [HttpPost]
        public JsonResult insertdata()
        {
            var obj = Request.Form["obj"];
        
            var fl_Array = Request.Form["fl_Array"];
            var decipline_desc = Request.Form["decipline_desc"];
            var Drivefunction_desc = Request.Form["Drivefunction_desc"];
            var equipment_desc = Request.Form["equipment_desc"];
            MCP Model = JsonConvert.DeserializeObject<MCP>(obj);
            prosol_Mcp mdl = new prosol_Mcp();
            mdl.decipline = Model.decipline;
            mdl.Drivefunction = Model.Drivefunction;
            mdl.equipment = Model.equipment;
            mdl.decipline_desc = decipline_desc;
            mdl.Drivefunction_desc = Drivefunction_desc;
            mdl.equipment_desc = equipment_desc;       
            mdl.functionlocation = fl_Array.Split(',');           
           
            mdl.mcpcode = genaratemcpcode(mdl.decipline, mdl.Drivefunction, mdl.equipment);                     
            if (Request.Files.Count > 0)
            {            
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                file.SaveAs(Path.Combine(Server.MapPath("~/BOMIMG/" + mdl.mcpcode + Request.Files[0].FileName.Substring(Request.Files[0].FileName.LastIndexOf('.')))));
                mdl.file = mdl.mcpcode + Request.Files[0].FileName.Substring(Request.Files[0].FileName.LastIndexOf('.'));
            }
            var getresult = _BomService.insetmcp(mdl);

           
            return this.Json(getresult, JsonRequestBehavior.AllowGet);
        }
        private string genaratemcpcode(string decipline, string drive,string equipment)
        {
            var code1 = decipline + drive + equipment;
            var code = _BomService.genaratemcpcode(code1);
            return code;
        }

        public FileResult Download(string mcpcode)
        {
            var res = _BomService.getsinglemcp(mcpcode);
            string sdsd = res.file;
                byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/BOMIMG/" + res.file));
                string fileName = res.file;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);           
        }


        
            public JsonResult DelEquip(string id)
        {

            var delEquip = _BomService.DelEquip(id);


            return this.Json(delEquip, JsonRequestBehavior.AllowGet);
        }
    }
   
}