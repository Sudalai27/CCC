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

namespace Prosol.Core
{
    public partial class passwordrecovery_service : I_pwRecovery
    {
        private readonly IRepository<Prosol_Users> _pw_recovery;
        private readonly IRepository<Prosol_RandomPC> _pw_random;

        public passwordrecovery_service(IRepository<Prosol_Users> pw_recovery, IRepository<Prosol_RandomPC> pw_random)
        {
            this._pw_recovery = pw_recovery;
            this._pw_random = pw_random;
        }


        public IEnumerable<Prosol_Users> sendemail_forPR(string email)
        {
            var query = Query.EQ("EmailId", email);

            var Data = _pw_recovery.FindAll(query);
            
            return Data;
        }

       public  bool updatepassword(string pw, string userid, int rndm)
        {
            //var query1 = Query.EQ("Userid", userid);

            //var Data = _pw_recovery.FindAll(query1).ToList();
            Random rrr = new Random();

            int num = rrr.Next(20000, 500000);


            var queryy = Query.And( Query.EQ("userid", userid) , Query.EQ("rndm",rndm));
            var Randoms = _pw_random.FindAll(queryy).ToList();
            
            if(Randoms.Count > 0)
            {
               bool bov = _pw_random.Delete(queryy);
            }
            else
            {
                return false;
            }


            if (userid != "undefined")
            {
                var query = Query.EQ("Userid", userid);
                var items = Update.Set("Password", pw);
                var flag = UpdateFlags.Upsert;
                var result = _pw_recovery.Update(query, items, flag);
                return result;
            }
            else
                return false;    
            
                
        }

        public bool saveRandom(string userid, int rndm)
        {
            var  i_r = new Prosol_RandomPC();
           // i_r._Id = ObjectId.GenerateNewId();
            i_r.userid = userid;
            i_r.rndm = rndm;           
            bool res1 = _pw_random.Add(i_r);
            return res1;
        }
    }
}