using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Prosol.Core
{
    public partial class MasterService : IMaster
    {

        private readonly IRepository<Prosol_Master> _MasterRepository;
        private readonly IRepository<Prosol_Plants> _MasterplntRepository;
        private readonly IRepository<Prosol_Departments> _MasterdeptRepository;
        private readonly IRepository<Prosol_ServiceCategory> _servicecategoryRepository;

        public MasterService(IRepository<Prosol_Master> MasterRepository, IRepository<Prosol_Plants> MasterplntRepository, IRepository<Prosol_ServiceCategory> servicecategoryRepository, IRepository<Prosol_Departments> MasterdeptRepository)
        {
            this._MasterRepository = MasterRepository;
            this._MasterplntRepository = MasterplntRepository;
            this._MasterdeptRepository = MasterdeptRepository;
            this._servicecategoryRepository = servicecategoryRepository;
        }
        public bool InsertData(Prosol_Master data)
        {
            bool res = false;
            data.Islive = true;
            var query = Query.And(Query.EQ("Label", data.Label),(Query.Or(Query.EQ("Code", data.Code),Query.EQ("Title", data.Title))));
            var vn = _MasterRepository.FindAll(query).ToList();
            if (vn.Count == 0 || (vn.Count == 1 && vn[0]._id == data._id))
            {
                res = _MasterRepository.Add(data);
            }
            return res;

        }       
        public IEnumerable<Prosol_Master> GetDataList(string Label)
        {
            var query = Query.And(Query.EQ("Label", Label)/*,Query.EQ("Islive", true)*/);          
            var lst = _MasterRepository.FindAll(query);
            return lst;

        }
        //bin
        public virtual IEnumerable<Prosol_Master> getbincode(string lable, string StorageLocation)
        {
            var query = Query.EQ("Storagelocationcode", StorageLocation);
            //   var query = Query.And(Query.EQ("Label", lable), Query.EQ("Storagelocationcode", StorageLocation));
            var grpList = _MasterRepository.FindAll(query).ToList();
            return grpList;
        }

        public bool DelData(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("Islive", false);
            var flg = UpdateFlags.Upsert;
            var res = _MasterRepository.Update(query, Updae, flg);
            return res;
        }
        public IEnumerable<Prosol_Master> GetMaster()
        {           
            var lst = _MasterRepository.FindAll();
            return lst;

        }
        public bool DisableData(string id, bool sts)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("Islive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _MasterRepository.Update(query, Updae, flg);
            return res;
          }


       


        //Plant
        public bool InsertDataplnt(Prosol_Plants data)
        {
            bool res = false;
            data.Islive = true;
            var query = Query.Or(Query.EQ("Plantcode", data.Plantcode),Query.EQ("Plantname",data.Plantname));
            var vn = _MasterplntRepository.FindAll(query).ToList();
            if (vn.Count == 0 || (vn.Count == 1 && vn[0]._id == data._id))
            {
                res = _MasterplntRepository.Add(data);
            }
            return res;
        }
        public IEnumerable<Prosol_Plants> GetDataListplnt()
        {
           // var query = Query.EQ("Islive", true);
            var mxplnt = _MasterplntRepository.FindAll().ToList();
            return mxplnt;

        }
        public bool DelDataplnt(string id)
        {
            var query = Query.EQ("Plantcode", id);
            var res = _MasterplntRepository.Delete(query);
            return res;
        }
        public IEnumerable<Prosol_Plants> GetMasterplnt()
        {
            var lst = _MasterplntRepository.FindAll();
            return lst;
        }
        public bool DisablePlant(string id, bool sts)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("Islive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _MasterplntRepository.Update(query, Updae, flg);
            return res;

        }

        //Department
        public bool InsertDatawithdept(Prosol_Departments data)
        {
            bool res = false;
            data.Islive = true;
            var query = Query.EQ("Departmentname", data.Departmentname);
            var vn = _MasterdeptRepository.FindAll(query).ToList();
            if (vn.Count == 0 || (vn.Count == 1 && vn[0]._id == data._id))
            {
                res = _MasterdeptRepository.Add(data);
            }
            return res;
        }
        public IEnumerable<Prosol_Departments> GetDataListdept()
        {
            var lst = _MasterdeptRepository.FindAll();
            return lst;
        }
        public bool DelDatadept(string id)
        {
            var query = Query.EQ("Departmentname", id);
            var res = _MasterdeptRepository.Delete(query);
            return res;
        }
        public IEnumerable<Prosol_Departments> GetMasterdept()
        {
            var lst = _MasterdeptRepository.FindAll();
            return lst;
        }
        public bool DisableDept(string id, bool sts)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("Islive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _MasterdeptRepository.Update(query, Updae, flg);
            return res;

        }

        //userless
        public bool InsertDatawithplant(Prosol_Master data)
        {
            bool res = false;
            var query = Query.And(Query.EQ("Label", data.Label), Query.EQ("Islive", true), Query.EQ("Code", data.Code), Query.EQ("Plantcode", data.Plantcode));
            var vn = _MasterRepository.FindAll(query).ToList();
            if (vn.Count == 0 || (vn.Count == 1 && vn[0]._id == data._id))
            {
                res = _MasterRepository.Add(data);
            }
            return res;
        }

        public bool InsertDatawithstorage(Prosol_Master data)
        {
            bool res = false;
            var query = Query.And(Query.EQ("Label", "Storage bin"), Query.EQ("Islive", true), Query.EQ("Code", data.Code),Query.EQ("Plantcode", data.Plantcode),Query.EQ("Storagelocationcode", data.Storagelocationcode));
            var vn = _MasterRepository.FindAll(query).ToList();
            if (vn.Count == 0 || (vn.Count == 1 && vn[0]._id == data._id))
            {
                res = _MasterRepository.Add(data);
            }
            return res;
        }
        public IEnumerable<Prosol_Master> getstoragelocation(string plant)
        {
            string[] strfield = { "Code", "Title" };
            var fields = Fields.Include(strfield).Exclude("_id");
            var query = Query.And(Query.EQ("Label", "Storage location"), Query.EQ("Islive", true), Query.EQ("Plantcode", plant));
            var vn = _MasterRepository.FindAll(fields, query).ToList();
            return vn;
        }
        public IEnumerable<Prosol_Master> GetDataListstorage(string Label)
        {
            var query = Query.And(Query.EQ("Label", Label), Query.EQ("Islive", true));
            var lst = _MasterRepository.FindAll(query);
            return lst;
        }
        public bool DelDatastorage(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("Islive", "InActive");
            var flg = UpdateFlags.Upsert;
            var res = _MasterRepository.Update(query, Updae, flg);
            return res;
        }
        public IEnumerable<Prosol_Master> GetMasterstorage()
        {
            var lst = _MasterRepository.FindAll();
            return lst;
        }

        public IEnumerable<Prosol_ServiceCategory> Getcategory()
        {
            var lst = _servicecategoryRepository.FindAll();
            return lst;

        }
    }
}
