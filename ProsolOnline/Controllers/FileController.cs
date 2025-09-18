using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProsolOnline.Models;
using Prosol.Common;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using ProsolOnline.ViewModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace ProsolOnline.Controllers
{
    public class FileController : Controller
    {

        private readonly I_Import _Userimprtservice;
        private readonly ICharateristics __charaservice;
        private readonly I_Assignwork _Assignworkservice;
        private readonly INounModifier _NounModifiService;
        private readonly IGeneralSettings _GeneralSettingService;
        private readonly IUserCreate _UserCreateService;
        private readonly ICatalogue _CatalogueService;

        DataTable dtdup = new DataTable();

        public object Amazon { get; private set; }

        public FileController(I_Import usrimpservice,
            ICharateristics charaservice,
            I_Assignwork asignwrkservice,
            INounModifier NMservice, IGeneralSettings generalSettingsservice, IUserCreate UserCreateService, ICatalogue catalogueService)
        {
            _Userimprtservice = usrimpservice;
            __charaservice = charaservice;
            _Assignworkservice = asignwrkservice;
            _NounModifiService = NMservice;
            _GeneralSettingService = generalSettingsservice;
            _UserCreateService = UserCreateService;
            _CatalogueService = catalogueService;

        }
        // GET: File

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
        public ActionResult Index()
        {
            if (CheckAccess("Import") == 1)
                return View("Import");
            else if (CheckAccess("Import") == 0)
                return View("Denied");
            else return View("Login");

        }

        [Authorize]
        public ActionResult Import()
        {
            if (CheckAccess("Import") == 1)
                return View();
            else if (CheckAccess("Import") == 0)
                return View("Denied");
            else return View("Login");

        }

        [Authorize]
        public ActionResult Assignwork()
        {
            if (CheckAccess("Assign Work") == 1)
                return View("Assignwork");
            else if (CheckAccess("Assign Work") == 0)
                return View("Denied");
            else return View("Login");

        }

        /* Assignwork page load data*/
        [HttpGet]
        [Authorize]
        public JsonResult get_assigndata()
        {
            var get_assigndata = _Assignworkservice.loaddata();
            var jsonResult = Json(get_assigndata, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        //pvdata
        public JsonResult get_assigndata1()
        {
            var get_assigndata = _Assignworkservice.loaddata1();
            var jsonResult = Json(get_assigndata, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        /* Reassign page load data*/
        [HttpGet]
        [Authorize]
        public JsonResult get_reassigndata(string role, string username)
        {
            var get_reassigndata = _Assignworkservice.reloaddata(role, username);
            var jsonResult = Json(get_reassigndata, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /* Assign page code search*/
        [HttpPost]
        [Authorize]
        public ActionResult search_code()
        {
            string[] codestr;
            var code = Request.Form["code"];
            List<Prosol_Datamaster> get_assigndata = new List<Prosol_Datamaster>();
            List<Prosol_Datamaster> gd_data = new List<Prosol_Datamaster>();
            List<string> code_split = new List<string>();
            codestr = code.Split(',');
            foreach (string n in codestr)
            {
                if (!code_split.Contains(n.ToString().Trim()))
                    code_split.Add(n.ToString().Trim());
            }
            foreach (string cdn in code_split)
            {
                gd_data = _Assignworkservice.multicode_search(cdn).ToList();
                get_assigndata.AddRange(gd_data);
            }
            return Json(get_assigndata, JsonRequestBehavior.AllowGet);
        }

        /* Reassign page load data based on User and role*/
        [HttpPost]
        [Authorize]
        public ActionResult reassignsearch_code()
        {
            var role = Request.Form["Role"];
            var username = Request.Form["UserName"];
            List<Prosol_Datamaster> get_reassigndata = new List<Prosol_Datamaster>();
            List<Prosol_Datamaster> gd_data = new List<Prosol_Datamaster>();
            List<string> reassigncode_split = new List<string>();
            gd_data = _Assignworkservice.multicode_reassignsearch(role, username).ToList();
            get_reassigndata.AddRange(gd_data);

            var jsonResult = Json(get_reassigndata, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            //return Json(get_reassigndata, JsonRequestBehavior.AllowGet);
        }

        /* Assignwork page submitdata to user*/
        [Authorize]
        public bool Assignwrk_submit()
        {
            var selection = Request.Form["selecteditem"];
            string FirstName = Request.Form["FirstName"];
            string Plantcde = Request.Form["PlantCode"];
            string Plantnme = Request.Form["PlantName"];
            var selecteditem = JsonConvert.DeserializeObject<List<string>>(selection);
            string term = FirstName;
            var userid = _Assignworkservice.AutoSearchUserName(term).ToList();
            if (selecteditem.Count > 0)
            {
                Prosol_Datamaster pd = new Prosol_Datamaster();
                Prosol_UpdatedBy userobj = new Prosol_UpdatedBy();

                userobj.UserId = userid[0].Userid;
                userobj.Name = FirstName;
                userobj.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                pd.Catalogue = userobj;
                pd.CreatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                pd.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);


                foreach (string item in selecteditem)
                {
                    //var check = _Assignworkservice.check_item(Convert.ToString(item)).ToList();
                    //if (check != null)
                    //{
                    var asgndata = _Assignworkservice.assign_submit(Convert.ToString(item), pd);
                    var resErp = _CatalogueService.GetERPInfo(item);
                    if (resErp != null)
                    {
                        resErp.Plant = Plantcde;
                        resErp.Plant_ = Plantnme;
                        _CatalogueService.WriteERPInfo(resErp);
                    }
                    // }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /* Reassign page submit data to user*/
        [Authorize]
        public bool reAssignwrk_submit()
        {
            var selection = Request.Form["selecteditem"];
            string FirstName = Request.Form["FirstName"];
            string username = Request.Form["Username"];
            Session["Username"] = username.ToString();
            string Role = Request.Form["Role"];
            //  Session["Role"] = Role.ToString();
            var selecteditem = JsonConvert.DeserializeObject<List<string>>(selection);
            string term = FirstName;
            var userid = _Assignworkservice.AutoSearchUserName(term).ToList();
            if (selecteditem.Count > 0)
            {
                Prosol_Datamaster reasgn = new Prosol_Datamaster();
                Prosol_UpdatedBy reuserobj = new Prosol_UpdatedBy();
                reuserobj.UserId = userid[0].Userid;
                reuserobj.Name = FirstName;
                reuserobj.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                if (Role == "Cataloguer")
                    reasgn.Catalogue = reuserobj;
                else if (Role == "Reviewer")
                    reasgn.Review = reuserobj;
                else
                    reasgn.Release = reuserobj;

                foreach (string item in selecteditem)
                {
                    var asgndata = _Assignworkservice.reassign_submit(Convert.ToString(item), Role, reasgn);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        //pvdata submit
        public bool Assignwrk_submitforpv(string Role)
        {
            var selection = Request.Form["selecteditem"];
            string FirstName = Request.Form["FirstName"];
            //  string username = Request.Form["Username"];
            // Session["Username"] = username.ToString();
            string Plantcde = Request.Form["PlantCode"];
            string Plantnme = Request.Form["PlantName"];
            //  string Role = Request.Form["User"];
            //  Session["Role"] = Role.ToString();
            var selecteditem = JsonConvert.DeserializeObject<List<string>>(selection);
            string term = FirstName;
            var userid = _Assignworkservice.AutoSearchUserName(term).ToList();
            //////
            // var selection = Request.Form["selecteditem"];
            // string FirstName = Request.Form["FirstName"];

            // var selecteditem = JsonConvert.DeserializeObject<List<string>>(selection);
            // string term = FirstName;
            // var userid = _Assignworkservice.AutoSearchUserName(term).ToList();


            if (selecteditem.Count > 0)
            {
                Prosol_Datamaster reasgn = new Prosol_Datamaster();
                Prosol_UpdatedBy reuserobj = new Prosol_UpdatedBy();
                reuserobj.UserId = userid[0].Userid;
                reuserobj.Name = FirstName;
                reuserobj.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //  if (Role == "PVuser")
                reasgn.PVuser = reuserobj;
                //else if (Role == "Reviewer")
                //    reasgn.Review = reuserobj;
                //else
                //    reasgn.Release = reuserobj;

                foreach (string item in selecteditem)
                {
                    var asgndata = _Assignworkservice.PVUSER(Convert.ToString(item), Role, reasgn);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        //public void Download(string get_assigndata)
        //{
        //    string cde = get_assigndata;
        //    List<Prosol_Datamaster> selecteditem = new List<Prosol_Datamaster>();
        //    List<Prosol_Datamaster> getcode = new List<Prosol_Datamaster>();
        //    string[] splt_str;
        //    List<string> code_splt = new List<string>();
        //    splt_str = cde.Split(',');
        //    foreach (string x in splt_str)
        //    {
        //        code_splt.Add(x);
        //    }
        //    foreach (string item in code_splt)
        //    {
        //        getcode = _Assignworkservice.multicode_search(item).ToList();
        //        selecteditem.AddRange(getcode);
        //    }
        //    //  var get_downloaddata = _Assignworkservice.download(selecteditem);
        //    string json = JsonConvert.SerializeObject(selecteditem);
        //    DataTable dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
        //    //    new DataTable();
        //    //dt.Columns.Add("Itemcode");
        //    //dt.Columns.Add("Legacy");
        //    //dt.Columns.Add("Noun");
        //    //dt.Columns.Add("Modifier");
        //    //dt.Columns.Add("Legacy2");
        //    //Response.Write("\n");
        //    //foreach (Prosol_Datamaster item in selecteditem)
        //    //{
        //    //    Prosol_Datamaster obp = new Prosol_Datamaster();
        //    //    var row = dt.NewRow();
        //    //    row["Itemcode"] = item.Itemcode;
        //    //    row["Legacy"] = item.Legacy;
        //    //    row["Noun"] = item.Noun;
        //    //    row["Modifier"] = item.Modifier;
        //    //    row["Legacy2"] = item.Legacy2;
        //    //    dt.Rows.Add(row);
        //    //}

        //    Response.ClearContent();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Assigndatas.xls"));
        //    Response.ContentType = "application/ms-excel";
        //    string str = string.Empty;
        //    for (int i = 1; i <= 5; i++)
        //    {
        //        Response.Write(dt.Columns[i].ToString().ToUpper() + "\t");
        //    }

        //    Response.Write("\n");
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        str = "";
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            Response.Write(str + dr[j]);
        //            str = "\t";
        //        }
        //        Response.Write("\n");
        //    }
        //    Response.End();

        //}       


        [Authorize]
        public JsonResult AutoCompleteUSN(string term)
        {
            var arrStr = _Assignworkservice.AutoSearchUserName(term);
            var result = arrStr.Select(i => new { i.UserName, i.Userid }).Distinct();
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        /*Get Userlist for assignwork*/
        [HttpGet]
        [Authorize]
        public JsonResult getuser(string role, string plant)
        {
            string[] plants = { plant };
            // var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));
            var userlist = _Assignworkservice.getuser(role, plants).ToList();
            return Json(userlist, JsonRequestBehavior.AllowGet);
        }


        /*Get Userlist of cataloguer role for assignwork*/
        [HttpGet]
        [Authorize]
        public JsonResult getuserassign(string plant)
        {
            string[] plantArr = { plant };
            // var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));
            string role = "Cataloguer";
            var userlist = _Assignworkservice.getuser(role, plantArr).ToList();
            return Json(userlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getuserassignpv()
        {
            // string[] plantArr = { plant };
            // var usrInfo = _UserCreateService.getimage(Convert.ToString(Session["UserId"]));
            string role = "PV User";
            var userlist = _Assignworkservice.getuserpv(role).ToList();
            return Json(userlist, JsonRequestBehavior.AllowGet);
        }

        /* Import page load data*/
        [HttpPost]
        [Authorize]
        public ActionResult Load_Data()
        {
            try
            {
                DataTable dtload = new DataTable();
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    if (file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                    {
                        var loaddata = _Userimprtservice.loaddata(file);
                        List<ImportModel> loaddata1 = new List<ImportModel>();
                        List<ImportModel> loadndup = new List<ImportModel>();
                        List<ImportModel> loaddup = new List<ImportModel>();
                        // var icode=  generateRequest();
                        // var tmp = (Convert.ToInt64(icode));
                        if (loaddata != null)
                        {
                            for (int ii = 0; ii < loaddata.Rows.Count; ii++)
                            {

                                // tmp = tmp + 1;
                                var dt_cl = new ImportModel();
                                dt_cl.Itemcode = loaddata.Rows[ii][0].ToString();
                                // dt_cl.Materialcode = loaddata.Rows[ii][0].ToString();
                                dt_cl.Legacy = loaddata.Rows[ii][1].ToString();
                                dt_cl.Noun = loaddata.Rows[ii][2].ToString();
                                dt_cl.Modifier = loaddata.Rows[ii][3].ToString();
                                dt_cl.PhysicalVerification = loaddata.Rows[ii][4].ToString();
                                dt_cl.UOM = loaddata.Rows[ii][5].ToString();
                                dt_cl.username = loaddata.Rows[ii][6].ToString();
                                // dt_cl.Type= loaddata.Rows[ii][7].ToString();
                                loaddata1.Add(dt_cl);
                            }
                            string[] Arritemcode = loaddata1.Select(p => p.Itemcode).ToArray();
                            foreach (ImportModel mdl in loaddata1)
                            {
                                string[] newArray = Arritemcode.Where(str => str == mdl.Itemcode).ToArray();
                                if (newArray.Length > 1)
                                {
                                    if (!loaddup.Contains(mdl))
                                    {
                                        var ldupd = new ImportModel();
                                        ldupd.Itemcode = mdl.Itemcode;
                                        //ldupd.Materialcode = mdl.Materialcode;
                                        ldupd.Legacy = mdl.Legacy;
                                        ldupd.Noun = mdl.Noun;
                                        ldupd.Modifier = mdl.Modifier;
                                        ldupd.PhysicalVerification = mdl.PhysicalVerification;
                                        ldupd.UOM = mdl.UOM;
                                        ldupd.username = mdl.username;
                                        //  ldupd.Type = mdl.Type;
                                        loaddup.Add(ldupd);
                                    }
                                }
                                else
                                {
                                    var ld = new ImportModel();
                                    ld.Itemcode = mdl.Itemcode;
                                    //  ld.Materialcode = mdl.Materialcode;
                                    ld.Legacy = mdl.Legacy;
                                    ld.Noun = mdl.Noun;
                                    ld.Modifier = mdl.Modifier;
                                    ld.PhysicalVerification = mdl.PhysicalVerification;
                                    ld.UOM = mdl.UOM;
                                    ld.username = mdl.username;
                                    // ld.Type = mdl.Type;
                                    loadndup.Add(ld);
                                }
                            }
                            DupVM duvm = new DupVM();
                            duvm.loaddup = loaddup;
                            duvm.loadndup = loadndup;
                            List<Prosol_Duplicate> listdup = new List<Prosol_Duplicate>();
                            foreach (var code in loaddup)
                            {
                                Prosol_Duplicate dup = new Prosol_Duplicate();
                                dup.Itemcode = code.Itemcode;
                                //  dup.Materialcode = code.Materialcode;
                                dup.Legacy = code.Legacy;
                                dup.Noun = code.Noun;
                                dup.Modifier = code.Modifier;
                                dup.Legacy2 = code.PhysicalVerification;
                                dup.UOM = code.UOM;
                                dup.username = code.username;
                                listdup.Add(dup);
                            }
                            _Userimprtservice.save(listdup);
                            var jsonResult1 = Json(duvm, JsonRequestBehavior.AllowGet);
                            jsonResult1.MaxJsonLength = int.MaxValue;
                            return jsonResult1;
                        }

                        else
                        {
                            return Json(null, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var GetLD = _Userimprtservice.loaddata(file);
                        return Json(GetLD, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    var GetLD = _Userimprtservice.loaddata(file);
                    return Json(GetLD, JsonRequestBehavior.AllowGet);
                }

            }

            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        private string generateRequest()
        {
            string itmCode = "";
            var ICode = _CatalogueService.getItem();

            if (ICode != "")
            {


                long serialinr = Convert.ToInt64(ICode);
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
        [Authorize]
        /* Import page duplicate download*/
        public void downloaddup()
        {
            DataTable dtdup = new DataTable();
            var gtcode = _Userimprtservice.show();
            _Userimprtservice.delete();
            if (gtcode.Count() > 0)
            {
                //dtdup.Columns.Add("Materialcode");
                dtdup.Columns.Add("Materialcode");
                dtdup.Columns.Add("Source Description");
                dtdup.Columns.Add("Noun");
                dtdup.Columns.Add("Modifier");
                dtdup.Columns.Add("Physical Data");
                dtdup.Columns.Add("UOM");
                dtdup.Columns.Add("AssignTo");

                Response.Write("\n");
                foreach (Prosol_Duplicate item in gtcode)
                {
                    Prosol_Duplicate obp = new Prosol_Duplicate();
                    var row = dtdup.NewRow();
                    row["Materialcode"] = item.Materialcode;
                    row["Source Description"] = item.Legacy;
                    row["Noun"] = item.Noun;
                    row["Modifier"] = item.Modifier;
                    row["Physical Data"] = item.Legacy2;
                    row["UOM"] = item.UOM;
                    row["AssignTo"] = item.username;
                    dtdup.Rows.Add(row);
                }
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Duplicatecode.xls"));
                Response.ContentType = "application/ms-excel";
                string str = string.Empty;
                for (int i = 0; i < dtdup.Columns.Count; i++)
                {
                    Response.Write(dtdup.Columns[i].ToString().ToUpper() + "\t");
                }
                Response.Write("\n");
                foreach (DataRow dr in dtdup.Rows)
                {
                    str = "";
                    for (int j = 0; j < dtdup.Columns.Count; j++)
                    {
                        if (dr[j].ToString().Contains('\n'))
                        {
                            dr[j] = dr[j].ToString().Replace('\n', ' ').Replace("\r\n", "").Replace("\t", "");
                        }
                        Response.Write(str + dr[j]);
                        str = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();

            }
        }


        //Scrub Data of import page
        [HttpPost]
        [Authorize]
        public JsonResult ScrubData()
        {
            var GetLD1 = Request.Form["GetLD"];
            var GetLD2 = JsonConvert.DeserializeObject<List<ImportModel>>(GetLD1);
            var NounList = _NounModifiService.GetNMinfo("");

          

            string[] ArrNoun = NounList.Select(p => p.Noun).Distinct().ToArray();
            // string[] ArrNounSys = NounList.Select(p => p.Similaritems).Distinct().ToArray();
            // string[] ArrNMAbb = NounList.Select(p => p.Nounabv).Distinct().ToArray();

            var scrubLst = _NounModifiService.GetScrubNMinfo();
           // string[] ScrubNoun = scrubLst.Select(p => p.Scrubnoun).Distinct().ToArray();
            foreach (ImportModel mdl in GetLD2)
            {
                string Legacy = mdl.Legacy;
                Legacy = Legacy.Replace(';', ' ');
                Legacy = Legacy.Replace(':', ' ');
                Legacy = Legacy.Replace(',', ' ');
               // Legacy = Legacy.Replace('.', ' ');
                Legacy = Legacy.Replace("  ", " ");
                string[] ArrLegacy = Legacy.Split(' ');
                var tempLst = new List<Prosol_NounModifiers>();
                var HighpriorityLst = new List<Prosol_NounModifiers>();
                int flg = 0;
                foreach (string str in ArrLegacy)
                {
                   // int g = 0;
                    if (!string.IsNullOrEmpty(str))
                    {
                        //if (ArrNoun.Contains(str.TrimEnd('.'), StringComparer.CurrentCultureIgnoreCase))
                        //{
                        //    mdl.Noun = str.ToUpper().TrimEnd('.');

                        //    var ModifierList = _NounModifiService.GetModifierList(str.ToUpper());

                        //    string[] ArrModifier = ModifierList.Select(p => p.Modifier).ToArray();

                        //    //  int flg = 0;
                        //    foreach (string Mstr in ArrLegacy)
                        //    {
                        //        if (Mstr != "")
                        //        {
                        //            if (ArrModifier.Contains(Mstr, StringComparer.CurrentCultureIgnoreCase))
                        //            {
                        //                flg = 1;
                        //                mdl.Modifier = Mstr.ToUpper();
                        //                break;

                        //            }
                        //        }
                        //    }
                        //    if (flg == 1)
                        //        break;
                        //   // mdl.Modifier = "NO MODIFIER";

                        //}
                        var NounandAbbr = NounList.Where(x => (x.Noun.StartsWith(str)) ||(x.Nounabv != null && x.Nounabv.StartsWith(str) )).ToList();
                        if (NounandAbbr != null && NounandAbbr.Count > 0)
                        {
                                 foreach (string Mstr in ArrLegacy)
                                {
                                    if (Mstr != "")
                                    {
                                        var ModifierandAbbr = NounList.Where(x => x.Noun == NounandAbbr[0].Noun && (x.Modifier.StartsWith(str) || x.Nounabv.Contains(" " + Mstr))).ToList();

                                        if (ModifierandAbbr != null && ModifierandAbbr.Count > 0)
                                        {
                                            mdl.Noun = ModifierandAbbr[0].Noun;
                                            mdl.Modifier = ModifierandAbbr[0].Modifier;
                                            flg = 1;
                                            break;

                                        }
                                    }
                                }
                                if (flg == 1)
                                    break;                         
                        }

                        //if (NounList.Where(x => x.Nounabv != null && x.Nounabv.Contains(" " + str.TrimEnd('.').ToUpper())).ToList() != null && NounList.Where(x => x.Nounabv != null && x.Nounabv.Contains(" " + str.TrimEnd('.').ToUpper())).ToList().Count > 0)
                        //{
                        //    var objLst = NounList.Where(x => x.Nounabv != null && x.Nounabv.Contains(" " + str.TrimEnd('.').ToUpper())).ToList();
                        //    if (objLst != null && objLst.Count > 0)
                        //    {
                        //        foreach (string Mstr in ArrLegacy)
                        //        {
                        //            if (Mstr != "")
                        //            {
                        //                var objLst1 = NounList.Where(x => x.Nounabv != null && x.Modifier == objLst[0].Modifier && x.Nounabv.StartsWith(Mstr.TrimEnd('.').ToUpper())).ToList();
                        //                if (objLst1 != null && objLst1.Count > 0)
                        //                {
                        //                    mdl.Noun = objLst1[0].Noun;
                        //                    mdl.Modifier = objLst1[0].Modifier;
                        //                    flg = 1;
                        //                    break;

                        //                }
                        //            }

                        //        }
                        //        if (flg == 1)
                        //            break;
                        //        //  mdl.Modifier = "NO MODIFIER";


                        //    }
                        //}
                        if (flg == 0)
                        {
                            var nmScrub = scrubLst.Where(x => x.Scrubnoun != null && x.Scrubnoun.StartsWith(str)).ToList();
                            if (nmScrub!=null && nmScrub.Count>0)
                            {                             
                                    
                                    foreach (string Mstr in ArrLegacy)
                                    {
                                        if (Mstr != "")
                                        {
                                            var objLst1 = scrubLst.Where(x => x.ScrubModifier != null && x.Scrubnoun == nmScrub[0].Scrubnoun && x.ScrubModifier.Contains(Mstr)).ToList();
                                            if (objLst1 != null && objLst1.Count > 0)
                                            {
                                                mdl.Noun = objLst1[0].Noun;
                                                mdl.Modifier = objLst1[0].Modifier;
                                                flg = 1;
                                                break;
                                            }
                                        }

                                    }
                                    if (flg == 1)
                                        break;              
                            }
                        }
                        if (flg == 0)
                        {
                            var SimNM = NounList.Where(x => x.ModifierEqu != null && x.ModifierEqu.Contains(str)).ToList();
                            if (SimNM != null && SimNM.Count > 0 && str.Length > 2)
                            {
                                foreach (string Mstr in ArrLegacy)
                                {
                                    if (Mstr != "")
                                    {
                                        var objLst1 = NounList.Where(x => x.ModifierEqu != null && x.Noun == SimNM[0].Noun && x.ModifierEqu.Contains(Mstr)).ToList();
                                        if (objLst1 != null && objLst1.Count > 0)
                                        {
                                            mdl.Noun = objLst1[0].Noun;
                                            mdl.Modifier = objLst1[0].Modifier;
                                            flg = 1;
                                            break;
                                        }
                                    }
                                }
                                if (flg == 1)
                                    break;
                            }
                        }
                        if (flg == 0)
                        {
                            var SimNM = NounList.Where(x => x.NounEqu != null && x.NounEqu.Contains(str)).ToList();
                            if (SimNM != null && SimNM.Count > 0 && str.Length > 2)
                            {
                                foreach (string Mstr in ArrLegacy)
                                {
                                    if (Mstr != "")
                                    {
                                        var objLst1 = NounList.Where(x => x.Modifier != null && x.Noun == SimNM[0].Noun && x.Modifier.Contains(Mstr)).ToList();
                                        if (objLst1 != null && objLst1.Count > 0)
                                        {
                                            mdl.Noun = objLst1[0].Noun;
                                            mdl.Modifier = objLst1[0].Modifier;
                                            flg = 1;
                                            break;
                                        }
                                    }
                                }
                                if (flg == 0)
                                {
                                    mdl.Modifier = "NO MODIFIER";
                                    flg = 1;
                                }
                                break;
                            }
                        }
                        //Noun contains
                        if (flg == 0)
                        {
                            if (ArrNoun.Contains(str.TrimEnd('.'), StringComparer.CurrentCultureIgnoreCase))
                            {
                                mdl.Noun = str.ToUpper().TrimEnd('.');

                                var ModifierList = _NounModifiService.GetModifierList(str.ToUpper());

                                string[] ArrModifier = ModifierList.Select(p => p.Modifier).ToArray();

                                //  int flg = 0;
                                foreach (string Mstr in ArrLegacy)
                                {
                                    if (Mstr != "")
                                    {
                                        if (ArrModifier.Contains(Mstr, StringComparer.CurrentCultureIgnoreCase))
                                        {
                                            flg = 1;
                                            mdl.Modifier = Mstr.ToUpper();


                                        }
                                    }
                                }
                                if (flg == 0)
                                    mdl.Modifier = "NO MODIFIER";
                                break;


                            }
                        }
                    }
                }
             
            }
            var jsonResult = Json(GetLD2, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult userdetails(string username)
        {

            var userdetails = _Assignworkservice.userdetails(username).ToList();

            return Json(userdetails, JsonRequestBehavior.AllowGet);
        }
        // *** Import and Auto Cleansing *** //
        [HttpPost, ValidateInput(false)]
        [Authorize]
        public ActionResult Import_Submit()
        {
            var GetLD1 = Request.Form["GetLD"];
            var GetLD2 = JsonConvert.DeserializeObject<List<ImportModel>>(GetLD1);

            List<Prosol_Datamaster> finalimport = new List<Prosol_Datamaster>();
            List<Prosol_Duplicate> listdup1 = new List<Prosol_Duplicate>();

            var checkitemcode = _Userimprtservice.checkitem();
            string[] chkitemcode = checkitemcode.Select(p => p.Itemcode).ToArray();
            foreach (var mdl1 in GetLD2)
            {
                string[] newArray = chkitemcode.Where(str => str == mdl1.Itemcode).ToArray();
                if (newArray.Length > 0)
                {
                    Prosol_Duplicate dup = new Prosol_Duplicate();
                    dup.Itemcode = mdl1.Itemcode;
                  //  dup.Materialcode = mdl1.Materialcode;
                    dup.Legacy = mdl1.Legacy;
                    dup.Noun = mdl1.Noun;
                    dup.Modifier = mdl1.Modifier;
                    dup.Legacy2 = mdl1.PhysicalVerification;
                    dup.UOM = mdl1.UOM;
                    dup.username = mdl1.username;

                    listdup1.Add(dup);

                }
                else
                {
                    var ld = new Prosol_Datamaster();

                    if (mdl1.username == "")
                    {
                        ld.Itemcode = mdl1.Itemcode;
                       // ld.Materialcode = mdl1.Materialcode;
                        ld.Legacy = mdl1.Legacy;
                        ld.Noun = mdl1.Noun;
                        ld.Modifier = mdl1.Modifier;
                        ld.Legacy2 = mdl1.PhysicalVerification;
                        ld.UOM = mdl1.UOM;
                       // ld.Junk = mdl1.Type;
                        ld.CreatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        finalimport.Add(ld);
                    }
                    else
                    {
                        var userobj = new Prosol_UpdatedBy();
                        var userdetails = _Assignworkservice.userdetails(mdl1.username).ToList();
                        if (userdetails != null && userdetails.Count > 0)
                        {
                            userobj.UserId = userdetails[0].Userid;
                            userobj.Name = userdetails[0].UserName;
                            userobj.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                            ld.Catalogue = userobj;
                        }
                        else ld.Catalogue = null;

                        ld.Itemcode = mdl1.Itemcode;
                       // ld.Materialcode = mdl1.Materialcode;
                        ld.Legacy = mdl1.Legacy;
                        ld.Noun = mdl1.Noun;
                        ld.Modifier = mdl1.Modifier;
                        ld.Legacy2 = mdl1.PhysicalVerification;
                        ld.UOM = mdl1.UOM;                       
                       // ld.Junk = mdl1.Type;
                        ld.CreatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        finalimport.Add(ld);
                    }
                }

            }
            _Userimprtservice.save(listdup1);

            var theResult1 = _Userimprtservice.Import_submit(finalimport);

            foreach (var mdl1 in finalimport)
            {

                if (mdl1.Catalogue != null)
                {
                    List<Prosol_Users> userdetails = _Assignworkservice.userdetails(mdl1.Catalogue.Name).ToList();
                    string Plantcde = userdetails[0].Plantcode[0];

                    List<Prosol_Plants> plnt = _Assignworkservice.plnt(Plantcde).ToList();
                    string plntname = plnt[0].Plantname;
                    //  var check = _Assignworkservice.check_item(Convert.ToString(mdl1.Itemcode)).ToList();
                    // if (check != null)
                    // {                       
                    var resErp = new Prosol_ERPInfo();
                    resErp.Itemcode = mdl1.Itemcode;
                    resErp.Plant = Plantcde;
                    resErp.Plant_ = plntname;
                    _CatalogueService.WriteERPInfo(resErp);

                    // }
                }
            }
                

            var jsonResult1 = Json(listdup1, JsonRequestBehavior.AllowGet);
            jsonResult1.MaxJsonLength = int.MaxValue;
            return jsonResult1;
        }

     



        [HttpGet]
        [Authorize]
        public JsonResult show()
        {
            var getdup = _Userimprtservice.show();
            return Json(getdup, JsonRequestBehavior.AllowGet);
        }

        //adp image download
        //public JsonResult Generatepdf()
        //{
        //    int cunt = 0;
        //    string selection = Request.Form["selecteditem"];
        //    var selecteditem = JsonConvert.DeserializeObject<List<Selection>>(selection);
        //    using (IAmazonS3 s3Client = new AmazonS3Client("AKIA3ODLTYZSURKAPRIN", "cQ+pllhE3gw0QNfRqZk81UhY2eFe5iCk4l97h6NU", Amazon.RegionEndpoint.MESouth1))
        //    {
        //        S3DirectoryInfo s3DirectoryInfo = new Amazon.S3.IO.S3DirectoryInfo(s3Client, "adport-images");
        //        var fList = s3DirectoryInfo.GetFiles("*");

        //        //try
        //        //{
        //        int h = 0;
        //        foreach (var item in selecteditem)
        //        {
        //            var E_Obj = _Assignworkservice.Getimage(item.Itemcode);


        //            if (E_Obj != null && E_Obj.AssetImages != null && E_Obj.AssetImages.AssetImgs != null)
        //            {

        //                int i = 1;
        //                //foreach (var str in E_Obj.AssetImages.AssetImgs)
        //                //{
        //                //    if (E_Obj.Modifier == "--")
        //                //        E_Obj.Modifier = "NO MODIFIER";
        //                //    //try
        //                //    //{
        //                //    var basePath = "D:\\MATERIAL_NM_IMAGES";
        //                //    var lst = str.Substring(str.LastIndexOf('/') + 1);
        //                //    for (int k = 0; k < fList.Length; k++)
        //                //    {
        //                //        if (lst.Contains(fList[k].Name))
        //                //        {
        //                //            //string folderPath = Path.Combine(basePath, item.Legacy);
        //                //            string folderPath = Path.Combine(basePath, E_Obj.Noun+","+ E_Obj.Modifier);
        //                //            if (!Directory.Exists(folderPath))
        //                //            {
        //                //                Directory.CreateDirectory(folderPath);
        //                //                //string destinationFolder = @"D:\MATERIAL_NM_IMAGES\" + item.Legacy + @"\";
        //                //                string destinationFolder = @"D:\MATERIAL_NM_IMAGES\" + E_Obj.Noun + "," + E_Obj.Modifier + @"\";
        //                //                //string filename = item.Legacy + "(" + i + ").JPG";
        //                //                string filename = E_Obj.Noun + "," + E_Obj.Modifier + "(" + i + ").JPG";

        //                //                fList[k].CopyToLocal(destinationFolder + filename, true);
        //                //            }
        //                //            else
        //                //            {
        //                //                //string destinationFolder = @"D:\MATERIAL_NM_IMAGES\" + item.Legacy + @"\";
        //                //                string destinationFolder = @"D:\MATERIAL_NM_IMAGES\" + E_Obj.Noun + "," + E_Obj.Modifier + @"\";
        //                //                //string filename = item.Legacy + "(" + i + ").JPG";
        //                //                string filename = E_Obj.Noun + "," + E_Obj.Modifier + "(" + i + ").JPG";

        //                //                fList[k].CopyToLocal(destinationFolder + filename, true);
        //                //            }
        //                //        }

        //                //    }
        //                //    i++;
        //                //    //}
        //                //    //catch (Exception e)
        //                //    //{

        //                //    //}
        //                //}
        //                foreach (var str in E_Obj.AssetImages.AssetImgs)
        //                {

        //                    //try
        //                    //{
        //                    var basePath = "D:\\ADPG_FZ_1142";
        //                    var lst = str.Substring(str.LastIndexOf('/') + 1);
        //                    for (int k = 0; k < fList.Length; k++)
        //                    {
        //                        if (lst.Contains(fList[k].Name))
        //                        {
        //                            string folderPath = Path.Combine(basePath, item.Legacy);
        //                            if (!Directory.Exists(folderPath))
        //                            {
        //                                Directory.CreateDirectory(folderPath);
        //                                string destinationFolder = @"D:\ADPG_FZ_1142\" + item.Legacy + @"\";
        //                                string filename = item.Legacy + "(" + i + ").JPG";

        //                                fList[k].CopyToLocal(destinationFolder + filename, true);
        //                            }
        //                            else
        //                            {
        //                                string destinationFolder = @"D:\ADPG_FZ_1142\" + item.Legacy + @"\";
        //                                string filename = item.Legacy + "(" + i + ").JPG";

        //                                fList[k].CopyToLocal(destinationFolder + filename, true);
        //                            }
        //                        }

        //                    }
        //                    i++;
        //                    //}
        //                    //catch (Exception e)
        //                    //{

        //                    //}
        //                }
        //            }
        //            if (E_Obj != null && E_Obj.AssetImages != null && E_Obj.AssetImages.MatImgs != null)
        //            {
        //                int i = 1;
        //                foreach (var str in E_Obj.AssetImages.MatImgs)
        //                {

        //                    //try
        //                    //{
        //                    var basePath = "D:\\ADPG_FZ_1142";
        //                    var lst = str.Substring(str.LastIndexOf('/') + 1);
        //                    for (int k = 0; k < fList.Length; k++)
        //                    {
        //                        if (lst.Contains(fList[k].Name))
        //                        {
        //                            string folderPath = Path.Combine(basePath, item.Legacy);
        //                            if (!Directory.Exists(folderPath))
        //                            {
        //                                Directory.CreateDirectory(folderPath);
        //                                string destinationFolder = @"D:\ADPG_FZ_1142\" + item.Legacy + @"\";
        //                                string filename = item.Legacy + "MAXIMO(" + i + ").JPG";

        //                                fList[k].CopyToLocal(destinationFolder + filename, true);
        //                            }
        //                            else
        //                            {
        //                                string destinationFolder = @"D:\ADPG_FZ_1142\" + item.Legacy + @"\";
        //                                string filename = item.Legacy + "MAXIMO(" + i + ").JPG";

        //                                fList[k].CopyToLocal(destinationFolder + filename, true);
        //                            }
        //                        }

        //                    }
        //                    i++;
        //                    //}
        //                    //catch (Exception e)
        //                    //{

        //                    //}
        //                }

        //            }
        //            if (E_Obj != null && E_Obj.AssetImages != null && E_Obj.AssetImages.NamePlateImge != null)
        //            {
        //                int i = 1;
        //                foreach (var str in E_Obj.AssetImages.NamePlateImge)
        //                {

        //                    //try
        //                    //{
        //                    var basePath = "D:\\ADPG_FZ_1142";
        //                    var lst = str.Substring(str.LastIndexOf('/') + 1);
        //                    for (int k = 0; k < fList.Length; k++)
        //                    {
        //                        if (lst.Contains(fList[k].Name))
        //                        {
        //                            string folderPath = Path.Combine(basePath, item.Legacy);
        //                            if (!Directory.Exists(folderPath))
        //                            {
        //                                Directory.CreateDirectory(folderPath);
        //                                string destinationFolder = @"D:\ADPG_FZ_1142\" + item.Legacy + @"\";
        //                                string filename = item.Legacy + "NP(" + i + ").JPG";

        //                                fList[k].CopyToLocal(destinationFolder + filename, true);
        //                            }
        //                            else
        //                            {
        //                                string destinationFolder = @"D:\ADPG_FZ_1142\" + item.Legacy + @"\";
        //                                string filename = item.Legacy + "NP(" + i + ").JPG";

        //                                fList[k].CopyToLocal(destinationFolder + filename, true);
        //                            }
        //                        }

        //                    }
        //                    i++;
        //                    //}
        //                    //catch (Exception e)
        //                    //{

        //                    //}
        //                }

        //            }
        //            if (E_Obj != null && E_Obj.AssetImages != null && E_Obj.AssetImages.NamePlateImgeTwo != null)
        //            {
        //                int i = 1;
        //                foreach (var str in E_Obj.AssetImages.NamePlateImgeTwo)
        //                {

        //                    //try
        //                    //{
        //                    var basePath = "D:\\ADPG_FZ_1142";
        //                    var lst = str.Substring(str.LastIndexOf('/') + 1);
        //                    for (int k = 0; k < fList.Length; k++)
        //                    {
        //                        if (lst.Contains(fList[k].Name))
        //                        {
        //                            string folderPath = Path.Combine(basePath, item.Legacy);
        //                            if (!Directory.Exists(folderPath))
        //                            {
        //                                Directory.CreateDirectory(folderPath);
        //                                string destinationFolder = @"D:\ADPG_FZ_1142\" + item.Legacy + @"\";
        //                                string filename = item.Legacy + "NP2(" + i + ").JPG";

        //                                fList[k].CopyToLocal(destinationFolder + filename, true);
        //                            }
        //                            else
        //                            {
        //                                string destinationFolder = @"D:\ADPG_FZ_1142\" + item.Legacy + @"\";
        //                                string filename = item.Legacy + "NP2(" + i + ").JPG";

        //                                fList[k].CopyToLocal(destinationFolder + filename, true);
        //                            }
        //                        }

        //                    }
        //                    i++;
        //                    //}
        //                    //catch (Exception e)
        //                    //{

        //                    //}
        //                }

        //            }
        //            if (E_Obj != null && E_Obj.AssetImages != null && E_Obj.AssetImages.OldTagImage != null)
        //            {
        //                int i = 1;
        //                foreach (var str in E_Obj.AssetImages.OldTagImage)
        //                {

        //                    //try
        //                    //{
        //                    var basePath = "D:\\ADPG_FZ_1142";
        //                    var lst = str.Substring(str.LastIndexOf('/') + 1);
        //                    for (int k = 0; k < fList.Length; k++)
        //                    {
        //                        if (lst.Contains(fList[k].Name))
        //                        {
        //                            string folderPath = Path.Combine(basePath, item.Legacy);
        //                            if (!Directory.Exists(folderPath))
        //                            {
        //                                Directory.CreateDirectory(folderPath);
        //                                string destinationFolder = @"D:\ADPG_FZ_1142\" + item.Legacy + @"\";
        //                                string filename = item.Legacy + "OLD(" + i + ").JPG";

        //                                fList[k].CopyToLocal(destinationFolder + filename, true);
        //                            }
        //                            else
        //                            {
        //                                string destinationFolder = @"D:\ADPG_FZ_1142\" + item.Legacy + @"\";
        //                                string filename = item.Legacy + "OLD(" + i + ").JPG";

        //                                fList[k].CopyToLocal(destinationFolder + filename, true);
        //                            }
        //                        }

        //                    }
        //                    i++;
        //                    //}
        //                    //catch (Exception e)
        //                    //{

        //                    //}
        //                }

        //            }
        //            if (E_Obj != null && E_Obj.AssetImages != null && E_Obj.AssetImages.NewTagImage != null)
        //            {
        //                int i = 1;
        //                foreach (var str in E_Obj.AssetImages.NewTagImage)
        //                {

        //                    //try
        //                    //{
        //                    var basePath = "D:\\ADPG_FZ_1142";
        //                    var lst = str.Substring(str.LastIndexOf('/') + 1);
        //                    for (int k = 0; k < fList.Length; k++)
        //                    {
        //                        if (lst.Contains(fList[k].Name))
        //                        {
        //                            string folderPath = Path.Combine(basePath, item.Legacy);
        //                            if (!Directory.Exists(folderPath))
        //                            {
        //                                Directory.CreateDirectory(folderPath);
        //                                string destinationFolder = @"D:\ADPG_FZ_1142\" + item.Legacy + @"\";
        //                                string filename = item.Legacy + "NEW(" + i + ").JPG";

        //                                fList[k].CopyToLocal(destinationFolder + filename, true);
        //                            }
        //                            else
        //                            {
        //                                string destinationFolder = @"D:\ADPG_FZ_1142\" + item.Legacy + @"\";
        //                                string filename = item.Legacy + "NEW(" + i + ").JPG";

        //                                fList[k].CopyToLocal(destinationFolder + filename, true);
        //                            }
        //                        }

        //                    }
        //                    i++;
        //                    //}
        //                    //catch (Exception e)
        //                    //{

        //                    //}
        //                }

        //            }
        //            cunt++;


        //        }

        //    }
        //    return Json(cunt, JsonRequestBehavior.AllowGet);
        //}
    }

    public class Selection
    {
        public string Itemcode { get; set; }
        public string Legacy { get; set; }
    }
}