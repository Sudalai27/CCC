using Newtonsoft.Json;
using Prosol.Common;
using Prosol.Core.Model;
using Prosol.Core.Interface;
using ProsolOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProsolOnline.Controllers
{
    public class SequenceController : Controller
    {
        private readonly ISequence _SequenceService;       
        public SequenceController(ISequence sequenceservice)
        {
            _SequenceService = sequenceservice;
           
        }
        // GET: GeneralSettings
        [Authorize]
        public ActionResult Index()
        {

            if (CheckAccess("Sequence Master") == 1)
                return View();
            else if (CheckAccess("Sequence Master") == 0)
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
        public ActionResult UOM()
        {

            if (CheckAccess("UOM Settings") == 1)
                return View();
            else if (CheckAccess("UOM Settings") == 0)
                return View("Denied");
            else return View("Login");         

        }


        [HttpPost]
        [Authorize]
        public JsonResult InsertSequence()
        {
            var seq1 = Request.Form["seque"];
            var ModelList = JsonConvert.DeserializeObject<List<SequenceModel>>(seq1);
            bool res = false;
            try
            {

                foreach (SequenceModel md in ModelList)
                {
                    TryUpdateModel(md);
                }

                if (ModelState.IsValid)
                {
                    var seqList = new List<Prosol_Sequence>();
                    foreach (SequenceModel Model in ModelList)
                    {
                        var sequ = new Prosol_Sequence();
                        sequ._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                        sequ.Category = Model.Category;
                        sequ.Description = Model.Description;
                        sequ.CatId = Model.CatId;
                        sequ.Seq = Convert.ToInt16(Model.Seq);
                        sequ.Required = Model.Required;
                        sequ.Separator = Model.Separator;
                        sequ.ShortLength = Convert.ToInt16(ModelList[0].ShortLength);

                        seqList.Add(sequ);
                    }
                    res = _SequenceService.CreateSequence(seqList);
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                 .Select(m => m.ErrorMessage).ToArray()
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSequenceList()
        {

            var seqList = _SequenceService.GetSequenceList();
            var lst = new List<SequenceModel>();
            foreach (Prosol_Sequence mdl in seqList)
            {
                var sequ = new SequenceModel();
                sequ._id = mdl._id.ToString();
                sequ.Category = mdl.Category;
                sequ.Description = mdl.Description;
                sequ.CatId = mdl.CatId;
                sequ.Seq = mdl.Seq.ToString();
                sequ.Required = mdl.Required;
                sequ.Separator = mdl.Separator;
                sequ.ShortLength = mdl.ShortLength.ToString();
                lst.Add(sequ);
            }
            return this.Json(lst, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [Authorize]
        public JsonResult InsertUOMSettings()
        {
            var sett = Request.Form["xSet"];
            var Model = JsonConvert.DeserializeObject<UOMSettingsModel>(sett);
            bool res = false;
            try
            {
                TryUpdateModel(Model);
                if (ModelState.IsValid)
                {

                    var sequ = new Prosol_UOMSettings();
                    sequ._id = Model._id == null ? new MongoDB.Bson.ObjectId() : new MongoDB.Bson.ObjectId(Model._id);
                    sequ.Short_space = Model.Short_space;
                    sequ.Long_space = Model.Long_space;
                    sequ.Short_required = Model.Short_required;
                    res = _SequenceService.CreateUOMSettings(sequ);
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                 .Select(m => m.ErrorMessage).ToArray()
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var log = new LogManager();
                log.LogError(log.GetType(), e);
                res = false;
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult GetUOMSettings()
        {

            var mdl = _SequenceService.GetUOMSettings();
            var sequ = new UOMSettingsModel();
            sequ._id = mdl._id.ToString();
            sequ.Short_space = mdl.Short_space;
            sequ.Long_space = mdl.Long_space;
            sequ.Short_required = mdl.Short_required;

            return this.Json(sequ, JsonRequestBehavior.AllowGet);

        }

    }

   

}