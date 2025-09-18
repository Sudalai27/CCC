using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prosol.Core.Interface;
using Prosol.Core.Model;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using System.Globalization;
using System.ServiceModel;

namespace Prosol.Core
{
    public partial class loginservice : I_login
    {
        private readonly IRepository<Prosol_Users> _loginUsers;
        private readonly IRepository<Prosol_Idealtime> _Idealtime;
        public loginservice(IRepository<Prosol_Users> loginUsers, IRepository<Prosol_Idealtime> Idealtime)
        {   
            this._loginUsers = loginUsers;
            this._Idealtime = Idealtime;
        }


        public IEnumerable<Prosol_Users> checklogin_details(string Username, string Password)
        {

            //Material
            //BasicHttpBinding binding = new BasicHttpBinding();
            //binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            //binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            //EndpointAddress endpoint = new EndpointAddress("http://s4hana1909.training.com:8701/sap/bc/srt/rfc/sap/zmaterial_create_rfc_srv/800/zmaterial_create_srv/zmat");
            //ZMATERIAL_CREATE_RFC_SRVClient client = new ZMATERIAL_CREATE_RFC_SRVClient(binding, endpoint);
            //client.ClientCredentials.UserName.UserName = "TEST4";
            //client.ClientCredentials.UserName.Password = "Vline@123";

            //var x1 = new ZMATERIAL_CREATE_RFC();
            //ZMATERIAL_INPUT[] mulMaterial = new ZMATERIAL_INPUT[1];
            //var sapfileds = new ZMATERIAL_INPUT();
            //var sapfiledsdesc = new BAPI_MAKT();
            //var sapfileds1 = new BAPI_MAKT[1];
            //var sapfileds2 = new BAPIMATINR[1];
            ////  mulMaterial[0] = sapfileds;
            //sapfileds.MATERIAL = "";
            //sapfileds.IND_SECTOR = "M";
            //sapfileds.MATL_TYPE = "FERT";
            //sapfileds.BASIC_VIEW = "X";
            //sapfileds.BASE_UOM = "EA";
            //sapfileds.SIZE_DIM = "14INC";
            //sapfileds.NET_WEIGHT = 7500;
            //sapfileds.UNIT_OF_WT = "G";
            //sapfileds.BASE_UOM2 = "X";
            //sapfiledsdesc.LANGU = "E";
            //sapfiledsdesc.MATL_DESC = "test";
            //sapfileds1[0] = sapfiledsdesc;
            //x1.ZMATERIALINPUT = sapfileds;
            //x1.ZMATDESC = sapfileds1;
            //x1.ZMATERIAL = sapfileds2;
            //var SapResponse = client.ZMATERIAL_CREATE_RFC(x1);

            //var response = SapResponse.ZRETURN;


          


            // var query = Query.And(Query.EQ("UserName", Username), Query.EQ("Password", Password));
            var query = Query.And(Query.Matches("UserName", BsonRegularExpression.Create(new Regex(Username.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))), Query.EQ("Password", Password));
            var Data = _loginUsers.FindAll(query);           
            return Data;
        }
        public void UpdateLoginDate(string uid)
        {
            var query = Query.EQ("Userid", uid);
            var Updte = Update.Set("Lastlogin",DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));
            var flg = UpdateFlags.Upsert;
            var res = _loginUsers.Update(query, Updte, flg);
            //
            //var lmd = new Prosol_Idealtime();
            //lmd.userid = uid;
            //lmd.Logintime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //_Idealtime.Add(lmd);

        }
        public void UpdateLogOutDate(string uid)
        {
            var query = Query.And(Query.EQ("userid", uid), Query.NE("Logintime", BsonNull.Value), Query.EQ("Logouttime", BsonNull.Value));
            var Updte = Update.Set("Logouttime", DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));
            var flg = UpdateFlags.Upsert;
            var res = _Idealtime.Update(query, Updte, flg);
        }

        public List<Prosol_Idealtime> getIdeal(string uid)
        {
            var x = Convert.ToString(DateTime.Now.Date);
            var date = DateTime.Parse(Convert.ToString(x, new CultureInfo("en-US", true)));
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            var date1 = DateTime.Parse(Convert.ToString(x, new CultureInfo("en-US", true)));
            date1 = date1.AddDays(1);
            date1 = DateTime.SpecifyKind(date1, DateTimeKind.Utc);
            var query = Query.And(Query.EQ("userid", uid), Query.NE("Logouttime", BsonNull.Value), Query.GTE("Logintime", BsonDateTime.Create(date)), Query.LTE("Logintime", BsonDateTime.Create(date1)));
            var res = _Idealtime.FindAll(query).ToList();
            return res;

        }

    }
}
