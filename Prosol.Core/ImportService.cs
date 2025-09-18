using Excel;
using MongoDB.Driver.Builders;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using System.Net;
using ExcelLibrary.Office.Excel;


namespace Prosol.Core
{

    public partial class ImportService : I_Import
    {

        private readonly IRepository<Prosol_Import> _UserimprtRepository;
        private readonly IRepository<Prosol_Duplicate> _UserdupRepository;
        private readonly IRepository<Prosol_Datamaster> _UserdataRepository;
        private readonly IRepository<Prosol_ERPInfo> _erpRepository;
        public ImportService(IRepository<Prosol_Import> Userimprtservice,
            IRepository<Prosol_Duplicate> Userdupservice,
            IRepository<Prosol_Datamaster> Userdataservice,
            IRepository<Prosol_ERPInfo> erpRepository)
        {

            this._UserimprtRepository = Userimprtservice;
            this._UserdupRepository = Userdupservice;
            this._UserdataRepository = Userdataservice;
            this._erpRepository = erpRepository;
        }

        public DataTable loaddata(HttpPostedFileBase file)
        {
            int cunt = 0;
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
            List<Prosol_Import> loaddata = new List<Prosol_Import>();
            if (file.FileName.EndsWith(".xls"))
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (file.FileName.EndsWith(".xlsx"))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            reader.IsFirstRowAsColumnNames = true;
            var res = reader.AsDataSet();
            reader.Close();
            DataTable dt = res.Tables[0];
            if (dt.Columns[0].ColumnName == "Materialcode" && dt.Columns[1].ColumnName == "LegacyData(Madatory)" && dt.Columns[2].ColumnName == "Noun" && dt.Columns[3].ColumnName == "Modifier" && dt.Columns[4].ColumnName == "PhysicalVerification" && dt.Columns[5].ColumnName == "UOM" && dt.Columns[6].ColumnName == "AssignTo")
            {
                return dt;
            }
            else
            {
               
                return null;
            }

          
        }



        public DataTable downloadDup(DataTable dt_dup)
        {
            return dt_dup;
        }


        public bool save(IEnumerable<Prosol_Duplicate> listdup)
        {
            var getdata = _UserdupRepository.Add(listdup);
            return true;
        }

        public IEnumerable<Prosol_Duplicate> show()
        {
            string[] plntfield = { "Itemcode", "Materialcode", "Legacy", "Noun", "Modifier", "Legacy2", "UOM", "username" };
            var fields = Fields.Include(plntfield).Exclude("_id");
            var gtcode = _UserdupRepository.FindAll(fields).ToList();
            return gtcode;
        }

        public IEnumerable<Prosol_Datamaster> checkitem()
        {
            string[] itemfield = { "Itemcode" };
            var fields = Fields.Include(itemfield).Exclude("_id");
            var getitemcode = _UserdataRepository.FindAll(fields).ToList();
            return getitemcode;
        }

        public bool Import_submit(IEnumerable<Prosol_Datamaster> listimport)
        {
            List<Prosol_ERPInfo> LstErp = new List<Prosol_ERPInfo>();
            foreach (Prosol_Datamaster mdl in listimport)
            {
                var lmd = new Prosol_ERPInfo();
                lmd.Itemcode = mdl.Itemcode;
                LstErp.Add(lmd);
            }
            var res = _UserdataRepository.Add(listimport);
            _erpRepository.Add(LstErp);
            return true;
        }

        public void delete()
        {
            // var delete = -_UserdupRepository.Delete(Prosol_Duplicate.Prosol_Duplicate);
            _UserdupRepository.DeleteAll();
        }
    }

}
