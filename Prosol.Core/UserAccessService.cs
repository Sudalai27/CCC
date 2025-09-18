using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using Prosol.Common;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using MongoDB.Driver;

namespace Prosol.Core
{
    public partial class UserAccessService : IUserAccess
    {
        private readonly IRepository<Prosol_Access> _UseraccessRepository;
        private readonly IRepository<Prosol_Users> _UsercreateRepository;
        private readonly IRepository<Prosol_Pages> _UserpageRepository;

        public UserAccessService(IRepository<Prosol_Users> Usercreateservice,IRepository<Prosol_Access> Useraccessservice, IRepository<Prosol_Pages> Userpageservice)
        {
            this._UseraccessRepository = Useraccessservice;
            this._UserpageRepository = Userpageservice;
            this._UsercreateRepository = Usercreateservice;
        }
      
        public IEnumerable<Prosol_Users> getuser()
        {
            var query = Query.EQ("Islive", "Active");          
            string[] Userfield = { "Userid", "UserName" };
            var fields = Fields.Include(Userfield).Exclude("_id");
            var gtuser = _UsercreateRepository.FindAll(fields, query).ToList();
            return gtuser;
        }


        public IEnumerable<Prosol_Pages> getpages()
        {
            string[] OrderBy = { "Status" };
            string[] pagefield = { "Pages", "Status" };
            var fields = Fields.Include(pagefield).Exclude("_id");
            var mx = _UserpageRepository.FindAll().ToList();
            return mx;
        }

        public IEnumerable<Prosol_Access> search(string id)
        {

            string[] pagefield = { "Pages", "Status" };
            var fields = Fields.Include(pagefield).Exclude("_id");
            var query = Query.And(Query.EQ("Userid", id),Query.EQ("Status", "1"));
            var mx = _UseraccessRepository.FindAll(query).ToList();
            return mx;
        }

        public bool pagelimit(string id, string pagename)
        {
            var query = Query.And(Query.EQ("Userid", id), Query.EQ("Status", "1"), Query.EQ("Pages", pagename));
            var mx = _UseraccessRepository.FindOne(query);
            if (mx != null)
                return true;
            else
                return false;
        }

        public bool delete(string id)
        {
            var query = Query.EQ("Userid", id);
            bool mx = _UseraccessRepository.Delete(query);
            return mx;
        }


        public bool submit(Prosol_Access acs)
        {
            var res = _UseraccessRepository.Add(acs);
            return res;
        }

        public virtual IEnumerable<Prosol_Users> AutoSearchUserName(string term)
        {
            string[] pagefield = { "UserName", "Userid" };
            var fields = Fields.Include(pagefield).Exclude("_id");
            var query = Query.Matches("UserName", BsonRegularExpression.Create(new Regex(term, RegexOptions.IgnoreCase)));
            var arrResult = _UsercreateRepository.FindAll(fields, query);
            return arrResult;

        }
        public void setAccess()
        {


        }
        public bool CheckAccess()
        {

            return true;

        }
    }

}
