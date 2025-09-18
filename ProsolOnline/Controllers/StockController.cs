using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Prosol.WebApi;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Prosol.Core.Model;

namespace ProsolOnline.Controllers
{
    public class StockController : ApiController
    {

       // private readonly IUserRepository _StockService;
        private readonly IUserRepository _StockService = new UserRepository();


        public StockController()
        {
          //  _StockService = StockService;


        }

        [HttpGet]
        public string UsrAuth(string Name1)
        {
            return _StockService.AuthenUr(Name1);

        }


        [HttpGet]
        public IEnumerable<ApiModel> Getuserstock(string Name)
        {
            return _StockService.Getstock(Name);
        }

     ///   [HttpPost]
        public string Updateusrstock([FromBody] string usr)
        {
            try
            {
                var MobileReturnList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ApiModel>>(usr);
                return _StockService.Updatestock(MobileReturnList);
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }
       [HttpPost]
        public string inputIN([FromBody] string IN)
       {
            try
            {

                var List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IOModel>>(IN);
                return _StockService.insertIN(List);
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }
      [HttpPost]
        public string inputOUT([FromBody] string OUT)
        {
            try
            {

                var List1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IOModel>>(OUT);
                return _StockService.insertOUT(List1);
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }

        [HttpGet]
        public IEnumerable<RfidModel> Getequip()
        {
            return _StockService.Getstockrfid();
        }


    }
}
