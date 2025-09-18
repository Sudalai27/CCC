using Prosol.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Prosol.Core.Model;
using MongoDB.Bson.IO;
using Newtonsoft.Json;

namespace ProsolOnline.Controllers
{
    public class TaskListController : Controller
    {
        private readonly I_Bom _BomService;

        public TaskListController(I_Bom BomService)
        {
            _BomService = BomService;

        }
        // GET: TaskList
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TaskList()
        {
            return View();
        }

        public JsonResult getmcpwithcondition(string discipline, string drive, string equipmnt)
        {
            var res = _BomService.getmcpwithcondition(discipline, drive, equipmnt);

            return this.Json(res, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CreateTasklist()
        {
            var Equipment = Request.Form["Equipment"].ToString();
            var Model = Request.Form["Model"].ToString();
            var Make = Request.Form["Make"].ToString();
            var tasklist1 = Request.Form["tasklist1"];

            List<Tasklist_operationSequence> tlos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Tasklist_operationSequence>>(tasklist1);

            Prosol_Tasklist ptl = new Prosol_Tasklist();
            ptl.Equipment = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(Equipment);
            ptl.Model = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(Model);
            ptl.Make = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(Make);
            ptl.TLOS = tlos;

            var res = _BomService.CreateTasklist(ptl);

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}