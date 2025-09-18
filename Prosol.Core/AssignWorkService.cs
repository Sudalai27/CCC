using Prosol.Core.Interface;
using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using MongoDB.Bson;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using ExcelLibrary.Office.Excel;
using System.Text.RegularExpressions;

namespace Prosol.Core
{
    public partial class AssignWorkService : I_Assignwork
    {
        private readonly IRepository<Prosol_Plants> _UserplantRepository;
        private readonly IRepository<Prosol_Datamaster> _UserassignwrkRepository;
        private readonly IRepository<Prosol_Users> _UsercreateRepository;

        public AssignWorkService(IRepository<Prosol_Datamaster> _Userassignwrkservice, IRepository<Prosol_Plants> Userplantervice, IRepository<Prosol_Users> Usercreateservice)
        {
            this._UserplantRepository = Userplantervice;
            this._UserassignwrkRepository = _Userassignwrkservice;
            this._UsercreateRepository = Usercreateservice;
        }

        public bool assign_submit(string item, Prosol_Datamaster pd)
        {
            var query = Query.EQ("Itemcode", item);
            var vn = _UserassignwrkRepository.FindAll(query).ToList();
            if (vn != null)
            {
                if (vn.Count == 1)
                {
                    vn[0].Catalogue = pd.Catalogue;
                    vn[0].CreatedOn = pd.CreatedOn;
                    vn[0].UpdatedOn = pd.UpdatedOn;
                    var res = _UserassignwrkRepository.Add(vn[0]);
                }
            }
            return true;
        }


        public bool reassign_submit(string item, string role, Prosol_Datamaster reasgn)
        {
            var query = Query.EQ("Itemcode", item);
            var vn = _UserassignwrkRepository.FindAll(query).ToList();
            if (vn != null)
            {
                if (role == "Cataloguer")
                {
                    if (vn.Count == 1)
                    {
                        vn[0].Catalogue = reasgn.Catalogue;
                        var res = _UserassignwrkRepository.Add(vn[0]);
                    }
                }
                else if (role == "Reviewer")
                {
                    if (vn.Count == 1)
                    {
                        vn[0].Review = reasgn.Review;
                        var res = _UserassignwrkRepository.Add(vn[0]);
                    }

                }
                else
                {
                    if (vn.Count == 1)
                    {
                        vn[0].Release = reasgn.Release;
                        var res = _UserassignwrkRepository.Add(vn[0]);
                    }

                }
            }
            return true;
        }
        //pvdata
        public bool PVUSER(string item, string role, Prosol_Datamaster reasgn)
        {
            var query = Query.EQ("Itemcode", item);
            var vn = _UserassignwrkRepository.FindAll(query).ToList();
            if (vn != null)
            {
                if (role == "PV User")
                {
                    if (vn.Count == 1)
                    {
                        vn[0].PVuser = reasgn.PVuser;
                        vn[0].ItemStatus = 12;
                        vn[0].PVstatus = "Pending";
                        var res = _UserassignwrkRepository.Add(vn[0]);
                    }
                }
                //var id = Query.EQ("Id", item);
                //var vn1 = _apimodel.FindAll(query).ToList();

                //if(vn1 != null)
                //{
                //    vn1[0].UserId = Convert.ToInt16(reasgn.PVuser.UserId);
                //    //vn1[0].ItemStatus = 12;
                //    vn1[0].status = "Pending";
                //    var res = _apimodel.Add(vn1[0]);

                //}
                //else if (role == "Reviewer")
                //{
                //    if (vn.Count == 1)
                //    {
                //        vn[0].Review = reasgn.Review;
                //        var res = _UserassignwrkRepository.Add(vn[0]);
                //    }

                //}
                //else
                //{
                //    if (vn.Count == 1)
                //    {
                //        vn[0].Release = reasgn.Release;
                //        var res = _UserassignwrkRepository.Add(vn[0]);
                //    }

                //}
            }
            return true;
        }
        public IEnumerable<Prosol_Plants> plnt(string Plantcode)
        {
            var query = Query.EQ("Plantcode", Plantcode);
            var shwusrs1 = _UserplantRepository.FindAll(query).ToList();
            return shwusrs1;
        }

        public IEnumerable<Prosol_Datamaster> loaddata()
        {
            string[] assign_field = { "Itemcode", "Materialcode", "Legacy", "Noun", "Modifier", "Legacy2", "UOM" };
            var fields = Fields.Include(assign_field).Exclude("_id");
            var query = Query.And(Query.EQ("Catalogue", BsonNull.Value), Query.EQ("ItemStatus", 0));
            var getdata = _UserassignwrkRepository.FindAll(fields, query).ToList();
            return getdata;
        }
        public IEnumerable<Prosol_Datamaster> loaddata1()
        {
            string[] assign_field = { "Itemcode", "Legacy", "Noun", "Modifier", "Legacy2", "UOM" };
            var fields = Fields.Include(assign_field).Exclude("_id");
            // var query = Query.And(Query.EQ("Catalogue", BsonNull.Value), Query.EQ("ItemStatus", 11));
            var query = Query.EQ("ItemStatus", 11);
            var getdata = _UserassignwrkRepository.FindAll(fields, query).ToList();
            return getdata;
        }
        public IEnumerable<Prosol_Datamaster> reloaddata(string role, string username)
        {
            if (role != null && username != null)
            {
                IMongoQuery query;
                string[] assign_field = { "Itemcode", "Materialcode", "Legacy", "Noun", "Modifier", "Legacy2", "UOM" };
                var fields = Fields.Include(assign_field).Exclude("_id");
                if (role == "Cataloguer")
                    query = Query.And(Query.EQ("ItemStatus", 0), Query.EQ("Catalogue.Name", username));
                else if (role == "Reviewer")
                    query = Query.And(Query.EQ("ItemStatus", 2), Query.EQ("Review.Name", username));
                else
                    query = Query.And(Query.EQ("ItemStatus", 4), Query.EQ("Release.Name", username));

                var getdata = _UserassignwrkRepository.FindAll(fields, query).ToList();
                return getdata;
            }
            else
            {
                List<Prosol_Datamaster> getdata = null;
                return getdata;

            }
        }

        public IEnumerable<Prosol_Datamaster> check_item(string itemcode)
        {
            // string[] chec_flds = { "Catalogue", "Catalogue.UserId", "Catalogue.Name" };
            // var fields = Fields.Include(chec_flds).Exclude("_id");
            var query = Query.And(Query.EQ("Materialcode", itemcode), Query.EQ("ItemStatus", 0));
            var getdata = _UserassignwrkRepository.FindAll(query).ToList();
            return getdata;

        }








        public IEnumerable<Prosol_Datamaster> multicode_search(string codestr)
        {
            string[] search_field = { "Itemcode", "Materialcode", "Legacy", "Noun", "Modifier", "Legacy2", "UOM" };
            var fields = Fields.Include(search_field).Exclude("_id");
            var query = Query.And(Query.EQ("ItemStatus", 0), Query.EQ("Itemcode", codestr));
            var getdata = _UserassignwrkRepository.FindAll(fields, query).ToList();
            return getdata;
        }


        public IEnumerable<Prosol_Datamaster> multicode_reassignsearch(string role, string username)
        {
            IMongoQuery query;
            string[] search_field = { "Itemcode", "Materialcode", "Legacy", "Noun", "Modifier", "Legacy2", "UOM" };
            var fields = Fields.Include(search_field).Exclude("_id");
            if (role == "Cataloguer")
                query = Query.And(Query.EQ("ItemStatus", 0), Query.EQ("Catalogue.Name", username));
            else if (role == "Reviewer")
                query = Query.And(Query.EQ("ItemStatus", 2), Query.EQ("Review.Name", username));
            else
                query = Query.And(Query.EQ("ItemStatus", 4), Query.EQ("Release.Name", username));

            var getdata = _UserassignwrkRepository.FindAll(fields, query).ToList();
            return getdata;
        }

        public virtual IEnumerable<Prosol_Users> AutoSearchUserName(string term)
        {
            string[] pagefield = { "UserName", "Userid" };
            var fields = Fields.Include(pagefield).Exclude("_id");
            var query = Query.Matches("UserName", BsonRegularExpression.Create(new Regex(term, RegexOptions.IgnoreCase)));
            var arrResult = _UsercreateRepository.FindAll(fields, query);
            return arrResult;

        }

        public IEnumerable<Prosol_Users> getuser(string role, string[] plants)
        {
            string[] userflds = { "UserName", "Userid" };
            var fields = Fields.Include(userflds).Exclude("_id");
            var query = Query.And(Query.EQ("Roles.Name", role), Query.In("Plantcode", new BsonArray(plants)), Query.EQ("Islive", "Active"));
            var mxplnt = _UsercreateRepository.FindAll(fields, query).ToList();
            return mxplnt;
        }
        public IEnumerable<Prosol_Users> getuserpv(string role)
        {
            string[] userflds = { "UserName", "Userid" };
            var fields = Fields.Include(userflds).Exclude("_id");
            var query = Query.And(Query.EQ("Roles.Name", role), Query.EQ("Islive", "Active"));
            var mxplnt = _UsercreateRepository.FindAll(fields, query).ToList();
            return mxplnt;
        }

        public IEnumerable<Prosol_Users> userdetails(string username)
        {
            var query = Query.EQ("UserName", username);
            var shwusrs = _UsercreateRepository.FindAll(query).ToList();
            return shwusrs;
        }

        public IEnumerable<Prosol_Users> getuseronly(string username)
        {
            string[] userflds = { "UserName", "Userid" };
            var fields = Fields.Include(userflds).Exclude("_id");
            var query = Query.And(Query.EQ("UserName", username), Query.EQ("Islive", "Active"));
            var mxplnt = _UsercreateRepository.FindAll(fields, query).ToList();
            return mxplnt;
        }

        public bool download(List<Prosol_Datamaster> selecteditem)
        {
            throw new NotImplementedException();
        }
    }
}
