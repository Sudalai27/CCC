using Excel;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Prosol.Core
{
    public class GeneralSettingService : IGeneralSettings
    {
        private readonly IRepository<Prosol_UOM> _UOMRepository;
        private readonly IRepository<Prosol_Vendor> _VendorRepository;
        private readonly IRepository<Prosol_Abbrevate> _AbbrevateRepository;
        private readonly IRepository<Prosol_Attribute> _AttributesRepository;
        private readonly IRepository<Prosol_AssetAttributes> _AssetAttributesRepository;
        private readonly IRepository<Prosol_Charateristics> _CharacteristicRepository;
        private readonly IRepository<Prosol_Logics> _LogicsRepository;
        private readonly IRepository<Prosol_Vendortype> _VendortypeRepository;
        private readonly IRepository<Prosol_Reftype> _ReftypeRepository;
        private readonly IRepository<Prosol_UNSPSC> _unspscRepository;
        private readonly IRepository<Prosol_GroupCodes> _groupcodeRepository;
        private readonly IRepository<Prosol_SubGroupCodes> _subgroupcodeRepository;
        private readonly IRepository<Prosol_Datamaster> _datamasterRepository;
        private readonly IRepository<Prosol_AssetMaster> _assetmasterRepository;
        private readonly IRepository<Prosol_ERPInfo> _erpRepository;
        private readonly IRepository<Prosol_HSNModel> _HSNMODELRepository;
        private readonly IRepository<Prosol_NounModifiers> _nounModifierRepository;
        private readonly IRepository<Prosol_Sequence> _SequenceRepository;
        private readonly IRepository<Prosol_UOMSettings> _UOMsettingRepository;
        private readonly IRepository<Prosol_UOMMODEL> _UOMMODELRepository;
        private readonly IRepository<Prosol_SubSubGroupCode> _SubSubGroupRepository;
        private readonly IRepository<Prosol_CodeLogic> _CodeLogicRepository;
        private readonly IRepository<Prosol_EquipmentType> _EquipRepository;
        private readonly IRepository<Prosol_EquipmentClass> _EquipClassRepository;
        private readonly IRepository<Prosol_Master> _masterRepository;
        private readonly IRepository<Prosol_Access> _accessRepository;

        private readonly IRepository<Prosol_Users> _UsersRepository;
        public GeneralSettingService(IRepository<Prosol_UOM> uomRepository,
            IRepository<Prosol_Vendor> vendorRepository,
            IRepository<Prosol_Abbrevate> abbrevateRepository,
            IRepository<Prosol_Charateristics> attributesRepository,
            IRepository<Prosol_AssetAttributes> AssetAttributesRepository,
            IRepository<Prosol_Logics> logicsRepository,
             IRepository<Prosol_HSNModel> HSNMODELRepository,
            IRepository<Prosol_Vendortype> VendortypeRepository,
            IRepository<Prosol_Reftype> ReftypeRepository,
            IRepository<Prosol_UNSPSC> unspscRepository,
            IRepository<Prosol_GroupCodes> groupcodeRepository,
            IRepository<Prosol_SubGroupCodes> subgroupcodeRepository,
             IRepository<Prosol_AssetMaster> assetmasterRepository,
             IRepository<Prosol_Datamaster> datamasterRepository,
             IRepository<Prosol_ERPInfo> erpRepository,

              IRepository<Prosol_Sequence> seqRepository,
            IRepository<Prosol_UOMSettings> UOMsettingRepository,
            IRepository<Prosol_NounModifiers> nounModifierRepository,
             IRepository<Prosol_UOMMODEL> UOMMODELRepository,
              IRepository<Prosol_SubSubGroupCode> SubSubGroupRepository,
              IRepository<Prosol_CodeLogic> codelogicRepository,
              IRepository<Prosol_Users> usersRepository,
               IRepository<Prosol_Attribute> AttributesRepository,
                IRepository<Prosol_EquipmentType> EquipRepository,
                IRepository<Prosol_Master> masterRepository,
                IRepository<Prosol_Access> accessRepository,
                IRepository<Prosol_EquipmentClass> EquipClassRepository
              )
        {
            this._UOMRepository = uomRepository;
            this._VendorRepository = vendorRepository;
            this._AbbrevateRepository = abbrevateRepository;
            this._HSNMODELRepository = HSNMODELRepository;
            this._CharacteristicRepository = attributesRepository;
            this._LogicsRepository = logicsRepository;
            this._VendortypeRepository = VendortypeRepository;
            this._ReftypeRepository = ReftypeRepository;
            this._unspscRepository = unspscRepository;
            this._groupcodeRepository = groupcodeRepository;
            this._subgroupcodeRepository = subgroupcodeRepository;
            this._datamasterRepository = datamasterRepository;
            this._assetmasterRepository = assetmasterRepository;
            this._erpRepository = erpRepository;

            this._UOMsettingRepository = UOMsettingRepository;
            this._nounModifierRepository = nounModifierRepository;
            this._SequenceRepository = seqRepository;
            this._UOMMODELRepository = UOMMODELRepository;
            this._SubSubGroupRepository = SubSubGroupRepository;
            this._CodeLogicRepository = codelogicRepository;
            this._UsersRepository = usersRepository;
            this._AttributesRepository = AttributesRepository;
            this._AssetAttributesRepository = AssetAttributesRepository;
            this._EquipRepository = EquipRepository;
            this._EquipClassRepository = EquipClassRepository;
            this._masterRepository = masterRepository;
            this._accessRepository = accessRepository;
        }

        // Get Unit List
        public IEnumerable<Prosol_UOM> Getunit()
        {
            return _UOMRepository.FindAll();
        }

        //Group Codes
        public virtual bool CreateGroupcode(Prosol_GroupCodes grp)
        {
            var res = false;
            var query = Query.And(Query.EQ("code", grp.code), Query.EQ("title", grp.title));
            var um = _groupcodeRepository.FindAll(query).ToList();
            if (um.Count == 0 || (um.Count == 1 && um[0]._id == grp._id))
            {
                res = _groupcodeRepository.Add(grp);
            }
            return res;
        }
        public virtual IEnumerable<Prosol_GroupCodes> GetGroupcodeList()
        {

            var grpList = _groupcodeRepository.FindAll();
            return grpList;
        }
        public virtual IEnumerable<Prosol_GroupCodes> GetGroupcodeList(string srchtxt)
        {
            var query = Query.Or(Query.Matches("code", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("title", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))));
            var groupList = _groupcodeRepository.FindAll(query);
            return groupList;
        }
        public virtual bool DeleteGroupcode(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _groupcodeRepository.Delete(query);
            return res;

        }


        //Sub Group Codes
        public virtual bool CreateSubGroupcode(Prosol_SubGroupCodes grp)
        {
            var res = false;
            var query = Query.And(Query.EQ("code", grp.code), Query.EQ("title", grp.title));
            var um = _subgroupcodeRepository.FindAll(query).ToList();
            if (um.Count == 0 || (um.Count == 1 && um[0]._id == grp._id))
            {
                res = _subgroupcodeRepository.Add(grp);
            }
            return res;
        }
        public virtual IEnumerable<Prosol_SubGroupCodes> GetSubGroupcodeList()
        {

            var grpList = _subgroupcodeRepository.FindAll();
            return grpList;
        }
        public virtual IEnumerable<Prosol_SubGroupCodes> GetSubGroupcodeList1(string srchtxt)
        {
            var query = Query.Or(Query.Matches("code", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("title", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("groupCode", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("groupTitle", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))));
            var subgroupList = _subgroupcodeRepository.FindAll(query);
            return subgroupList;
        }
        public virtual bool DeleteSubGroupcode(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _subgroupcodeRepository.Delete(query);
            return res;

        }

        // Sub Sub Group Codes

        public virtual IEnumerable<Prosol_SubGroupCodes> GetSubGroupcodeList(string maingroup)
        {
            var query = Query.EQ("groupCode", maingroup);
            var grpList = _subgroupcodeRepository.FindAll(query);
            return grpList;
        }
        public bool InsertSubSubgroup(Prosol_SubSubGroupCode data, int update)
        {
            bool res = false;
            if (update == 1)
            {
                res = _SubSubGroupRepository.Add(data);
                return res;
            }
            else
            {
                //bool res = false;
                // data.Islive = true;
                var query = Query.And(Query.EQ("MainGroupCode", data.MainGroupCode), Query.EQ("SubGroupCode", data.SubGroupCode), Query.EQ("SubSubGroupCode", data.SubSubGroupCode));
                var vn = _SubSubGroupRepository.FindAll(query).ToList();
                if (vn.Count == 0)
                {
                    var query1 = Query.And(Query.EQ("MainGroupCode", data.MainGroupCode), Query.EQ("SubGroupCode", data.SubGroupCode), Query.EQ("SubSubGroupTitle", data.SubSubGroupTitle));
                    var vn1 = _SubSubGroupRepository.FindAll(query).ToList();
                    if (vn1.Count == 0)
                        res = _SubSubGroupRepository.Add(data);
                }
                return res;
            }
        }
        public IEnumerable<Prosol_SubSubGroupCode> ListofSubSubUser()
        {

            var shwusr1 = _SubSubGroupRepository.FindAll().ToList();
            return shwusr1;
        }
        public bool DeleteSubsubGroupcode(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _SubSubGroupRepository.Delete(query);
            return res;
        }
        public IEnumerable<Prosol_SubSubGroupCode> GetSubsubGroupcodeList(string SubGroupCode)
        {
            var query = Query.EQ("SubGroupCode", SubGroupCode);
            var grpList = _SubSubGroupRepository.FindAll(query);
            return grpList;
        }
        public virtual IEnumerable<Prosol_SubSubGroupCode> GetSubSubGroupListSearch(string srchtxt)
        {
            var query = Query.Or(Query.Matches("MainGroupCode", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("MainGroupTitle", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("SubGroupCode", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("SubGroupTitle", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("SubSubGroupCode", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("SubSubGroupTitle", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))));
            var subsubgroupList = _SubSubGroupRepository.FindAll(query);
            return subsubgroupList;
        }

        //UOM
        public virtual bool CreateUOM(Prosol_UOM uom)
        {
            uom.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            var res = false;
            var query = Query.Or(Query.EQ("Unitname", uom.Unitname), Query.EQ("UOMname", uom.UOMname));
            var um = _UOMRepository.FindAll(query).ToList();
            if (um.Count == 0 || (um.Count == 1 && um[0]._id == uom._id))
            {
                res = _UOMRepository.Add(uom);
            }
            return res;



            //bool res = false;
            //if (update == 1)
            //{
            //    res = _UOMRepository.Add(uom);
            //    return res;
            //}
            //else
            //{
            //    // bool res = false;
            //    // uom.Islive = true;
            //    var query = Query.Or(Query.EQ("UOMname", uom.UOMname), Query.EQ("Unitname", uom.Unitname));
            //    var vn = _UOMRepository.FindAll(query).ToList();
            //    if (vn.Count == 0)
            //    {
            //        res = _UOMRepository.Add(uom);
            //    }
            //    return res;
            //}

        }
        public virtual IEnumerable<Prosol_UOM> GetUOMList()
        {
            //  var fields = Fields.Exclude("Attribute");
            var uomList = _UOMRepository.FindAll();
            return uomList;
        }
        public virtual IEnumerable<Prosol_UOM> GetUOMList(string srchtxt)
        {
            var query = Query.Or(Query.Matches("UOMname", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Unitname", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))));
            var UOMList = _UOMRepository.FindAll(query);
            return UOMList;
        }
        public virtual bool DeleteUOM(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _UOMRepository.Delete(query);
            return res;

        }
        public string[] GetUOM(string label)
        {
            var fields = Fields.Include("Unitname").Exclude("_id");
            var res = _UOMRepository.FindAll(fields).ToList();
            string[] arr = null; int i = 0;
            foreach (Prosol_UOM md in res)
            {
                arr[i] = md.Unitname;
                i++;
            }
            return arr;
        }
        private static ImageFormat GetImageFormat(string imageType)
        {
            ImageFormat imageFormat;
            switch (imageType)
            {
                case ".jpg":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case ".jpeg":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case ".JPG":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case ".JPEG":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case ".png":
                    imageFormat = ImageFormat.Png;
                    break;
                case ".PNG":
                    imageFormat = ImageFormat.Png;
                    break;
                default:
                    throw new Exception("Unsupported image type !");
            }

            return imageFormat;
        }

        public virtual int BulkUOM(HttpPostedFileBase file)
        {
            int cunt = 0;
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
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
            if (dt.Columns[0].ColumnName == "UOMname" && dt.Columns[1].ColumnName == "Unitname")
            {
                var LstNM = new List<Prosol_UOM>();
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[0].ToString() != "" && dr[1].ToString() != "")
                    {
                        var Mdl = new Prosol_UOM();
                        // Mdl.Attribute = dr[0].ToString();
                        Mdl.UOMname = dr[0].ToString();
                        Mdl.Unitname = dr[1].ToString();
                        Mdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        LstNM.Add(Mdl);
                    }
                }
                if (LstNM.Count > 0)
                {
                    //IEnumerable<Prosol_NounModifiers> filteredList = LstNM.GroupBy(Prosol_NounModifiers => Prosol_NounModifiers.Modifier).Select(group => group.First())
                    //    .GroupBy(Prosol_NounModifiers => Prosol_NounModifiers.Modifier).Select(group => group.First());

                    List<Prosol_UOM> filteredList = LstNM.GroupBy(p => new { p.UOMname, p.Unitname }).Select(g => g.First()).ToList();
                    if (filteredList.Count > 0)
                    {
                        var fRes = new List<Prosol_UOM>();
                        foreach (Prosol_UOM nm in filteredList.ToList())
                        {
                            var query = Query.Or(Query.EQ("UOMname", nm.UOMname), Query.EQ("Unitname", nm.Unitname));
                            var ObjStr = _UOMRepository.FindOne(query);
                            if (ObjStr == null)
                            {
                                fRes.Add(nm);

                            }
                        }
                        cunt = _UOMRepository.Add(fRes);

                    }
                }
            }else
            {
                return -1;
            }
            return cunt;

        }

        // Item base UOM
        public bool InsertData(Prosol_UOMMODEL data, int update)
        {
            data.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            bool res = false;
            if (update == 1)
            {
                res = _UOMMODELRepository.Add(data);
                return res;
            }
            else
            {
                // bool res = false;
                // data.Islive = true;
                var query = Query.EQ("UOMNAME", data.UOMNAME);
                var vn = _UOMMODELRepository.FindAll(query).ToList();
                if (vn.Count == 0)
                {
                    res = _UOMMODELRepository.Add(data);
                }
                return res;
            }
        }
        public IEnumerable<Prosol_UOMMODEL> getlistuom()
        {
            //string[] Userfield = { "SeviceCategorycode", "SeviceCategoryname", "Islive", "_id" };
            //var fields = Fields.Include(Userfield);
            var shwusr1 = _UOMMODELRepository.FindAll().ToList();
            return shwusr1;
        }
        public virtual bool DeleteUOM1(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _UOMMODELRepository.Delete(query);
            return res;

        }


        //Vendor
        public virtual bool CreateVendor(Prosol_Vendor vendor)
        {
            vendor.Name = vendor.Name.Replace(",", "");
            vendor.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            var res = false;
            var query = Query.EQ("Name", vendor.Name);
            var vn = _VendorRepository.FindAll(query).ToList();
            if (vn.Count == 0 || (vn.Count == 1 && vn[0]._id == vendor._id))
            {
                res = _VendorRepository.Add(vendor);
            }
            return res;
        }
        public virtual IEnumerable<Prosol_Vendor> GetVendorList()
        {
            var vendorList = _VendorRepository.FindAll();
            return vendorList;
        }
        public virtual IEnumerable<Prosol_Vendor> GetVendorList(string srchtxt)
        {
            var query =  Query.Or(Query.Matches("Name", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Address", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))));
            var vendorList = _VendorRepository.FindAll(query);
            return vendorList;
        }
        public virtual bool DeleteVendor(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _VendorRepository.Delete(query);
            return res;

        }
        public string getNextCode()
        {
            var sort = SortBy.Ascending("_id");
            string[] strArr = { "Code" };
            // var fields = Fields.Include(strArr).Exclude("_id");
            var fields = Fields.Include(strArr);
            var temp = _VendorRepository.FindAll(fields, sort).ToList();
            if (temp.Count > 0)
            {
                var res = temp[temp.Count - 1].Code;
                return res;
            }
            else
            {
                return "0";
            }
        }
        private string generatCode(int cde)
        {
            string rollno = "";

            if (cde != 0)
            {

                cde++;
                switch (cde.ToString().Length)
                {
                    case 1:
                        rollno = "0000" + cde;
                        break;
                    case 2:
                        rollno = "000" + cde;
                        break;
                    case 3:
                        rollno = "00" + cde;
                        break;
                    case 4:
                        rollno = "0" + cde;
                        break;
                    case 5:
                        rollno = cde.ToString();
                        break;
                }

                return rollno;
            }
            else return rollno = "00001";
        }
        public bool DisableVendor(string id, bool sts)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("Enabled", sts);
            var flg = UpdateFlags.Upsert;
            var res = _VendorRepository.Update(query, Updae, flg);
            return res;

        }
        public virtual int BulkVendor(HttpPostedFileBase file)
        {
            int cunt = 0;
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
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
            if (dt.Columns[0].ColumnName == "ShortDesc Name" && dt.Columns[1].ColumnName == "Name" && dt.Columns[2].ColumnName == "Name2" 
                && dt.Columns[3].ColumnName == "Name3" && dt.Columns[4].ColumnName == "Name4"
                && dt.Columns[5].ColumnName == "Address" && dt.Columns[6].ColumnName == "Address2"
                && dt.Columns[7].ColumnName == "Address3" && dt.Columns[8].ColumnName == "Address4" && dt.Columns[9].ColumnName == "City"
                && dt.Columns[10].ColumnName == "Region" && dt.Columns[11].ColumnName == "Postal"
                && dt.Columns[12].ColumnName == "Country" && dt.Columns[13].ColumnName == "Phone" && dt.Columns[14].ColumnName == "Mobile" 
                && dt.Columns[15].ColumnName == "Fax" && dt.Columns[16].ColumnName == "Email" && dt.Columns[17].ColumnName == "Website")
            {
                string str = getNextCode();
                Int16 rl = Convert.ToInt16(str);
                var LstNM = new List<Prosol_Vendor>();

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[1].ToString() != "")
                    {
                        var Mdl = new Prosol_Vendor();
                        Mdl.Code = generatCode(rl++);
                        Mdl.ShortDescName = dr[0].ToString().Replace(",", "");
                        Mdl.Name = dr[1].ToString().Replace(",", "");
                        Mdl.Name2 = dr[2].ToString().Replace(",", "");
                        Mdl.Name3 = dr[3].ToString().Replace(",", "");
                        Mdl.Name4 = dr[4].ToString().Replace(",", "");
                        Mdl.Address = dr[5].ToString();
                        Mdl.Address2 = dr[6].ToString();
                        Mdl.Address3 = dr[7].ToString();
                        Mdl.Address4 = dr[8].ToString();
                        Mdl.City = dr[9].ToString();
                        Mdl.Region = dr[10].ToString();
                        Mdl.Postal = dr[11].ToString();
                        Mdl.Country = dr[12].ToString();
                        Mdl.Phone = dr[13].ToString();
                        Mdl.Mobile = dr[14].ToString();
                        Mdl.Fax = dr[15].ToString();
                        Mdl.Email = dr[16].ToString();
                        Mdl.Website = dr[17].ToString();

                        Mdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        Mdl.Enabled = true;
                        LstNM.Add(Mdl);
                    }
                }
                if (LstNM.Count > 0)
                {
                    //IEnumerable<Prosol_NounModifiers> filteredList = LstNM.GroupBy(Prosol_NounModifiers => Prosol_NounModifiers.Modifier).Select(group => group.First())
                    //    .GroupBy(Prosol_NounModifiers => Prosol_NounModifiers.Modifier).Select(group => group.First());

                    List<Prosol_Vendor> filteredList = LstNM.GroupBy(p => new { p.Name, p.Address }).Select(g => g.First()).ToList();
                    if (filteredList.Count > 0)
                    {
                        var fRes = new List<Prosol_Vendor>();
                        foreach (Prosol_Vendor nm in filteredList.ToList())
                        {
                            var query = Query.And(Query.EQ("Name", nm.Name), Query.EQ("Address", nm.Address));
                            var ObjStr = _VendorRepository.FindOne(query);
                            if (ObjStr == null)
                            {
                                fRes.Add(nm);

                            }
                        }
                        cunt = _VendorRepository.Add(fRes);

                    }
                }
            }
            else
            {

                return -1;
            }
           
            return cunt;

        }
        public IEnumerable<Prosol_Vendor> GetVendorLst(string term)
        {
            var query =  Query.Matches("Name", BsonRegularExpression.Create(new Regex(term, RegexOptions.IgnoreCase)));
            var arrResult = _VendorRepository.FindAll(query);
            return arrResult;
        }

        //Abbrevate
        public virtual bool CreateAbbr(Prosol_Abbrevate abb)
        {

            var res = false;
            var query = Query.And(Query.EQ("Value", abb.Value), Query.EQ("Abbrevated", abb.Abbrevated));
            var um = _AbbrevateRepository.FindAll(query).ToList();
            if (um.Count == 0 || (um.Count == 1 && um[0]._id == abb._id))
            {
            
                res = _AbbrevateRepository.Add(abb);
              
            }
            return res;
        }
        public virtual IEnumerable<Prosol_Abbrevate> GetAbbrList()
        {

            var abbrList = _AbbrevateRepository.FindAll();
            return abbrList;
        }
        public virtual IEnumerable<Prosol_Abbrevate> GetAbbrList(string srchtxt)
        {
            var query = Query.Or(Query.Matches("Value", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Abbrevated", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Equivalent", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("LikelyWords", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Approved", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))));
            var AbbrList = _AbbrevateRepository.FindAll(query);
            return AbbrList;
        }
        public virtual bool DeleteAbbr(string id,string val)
        {
            var query = Query.And(Query.EQ("Abbrevated", id), Query.EQ("Value", val));
            var res = _AbbrevateRepository.Delete(query);
            return res;

        }
        public virtual bool unAbbrDel(string id)
        {
            string[] str = { id };
            var Qry=  Query.And(Query.In("Values",new BsonArray(str)));
            var checkValLst = _CharacteristicRepository.FindAll(Qry).ToList();
            if(checkValLst!=null && checkValLst.Count > 0)
            {
                foreach(Prosol_Charateristics chtic in checkValLst)
                {
                    List<string> list = new List<string>(chtic.Values);
                    list.Remove(id);
                    chtic.Values = list.ToArray();
                    _CharacteristicRepository.Add(chtic);
                }

            }
            var query = Query.And(Query.EQ("_id", new ObjectId(id)));
            var res = _AbbrevateRepository.Delete(query);
            return res;

        }
        public virtual int BulkAbbri(HttpPostedFileBase file,string User)
        {
            int cunt = 0;
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
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

            var LstNM = new List<Prosol_Abbrevate>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].ToString() != "")
                {
                    var Mdl = new Prosol_Abbrevate();
                    Mdl.Value = dr[0].ToString();
                    Mdl.Abbrevated = dr[1].ToString();
                    Mdl.vunit = dr[2].ToString();
                    Mdl.Equivalent = dr[3].ToString();
                    Mdl.eunit = dr[4].ToString();
                    Mdl.LikelyWords = dr[5].ToString();
                    Mdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    Mdl.User = User;
                    Mdl.Approved = "Yes";
                    Mdl.ApprovedBy = User;
                    Mdl.ApprovedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                    LstNM.Add(Mdl);
                }
            }
            if (LstNM.Count > 0)
            {
                //IEnumerable<Prosol_NounModifiers> filteredList = LstNM.GroupBy(Prosol_NounModifiers => Prosol_NounModifiers.Modifier).Select(group => group.First())
                //    .GroupBy(Prosol_NounModifiers => Prosol_NounModifiers.Modifier).Select(group => group.First());

                List<Prosol_Abbrevate> filteredList = LstNM.GroupBy(p => new { p.Value, p.Abbrevated }).Select(g => g.First()).ToList();
                if (filteredList.Count > 0)
                {
                    var fRes = new List<Prosol_Abbrevate>();
                    foreach (Prosol_Abbrevate nm in filteredList.ToList())
                    {
                        var query = Query.EQ("Value", nm.Value);
                        var ObjStr = _AbbrevateRepository.FindOne(query);
                        if (ObjStr == null)
                        {
                            // fRes.Add(nm);
                            _AbbrevateRepository.Add(nm);
                            cunt++;
                        }
                        else
                        {
                           
                             var Updte = Update.Set("Abbrevated", nm.Abbrevated);                           
                             var flg = UpdateFlags.Upsert;
                            _AbbrevateRepository.Update(query, Updte, flg);
                            cunt++;
                        }
                    }
                    

                }
            }
            return cunt;

        }

        public virtual List<Prosol_Abbrevate> DownloadValuemaster(string FrmDte, string ToDte)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            if (FrmDte != "" && ToDte != "")
            {
                var dte = DateTime.SpecifyKind(Convert.ToDateTime(FrmDte), DateTimeKind.Utc);
                var dte1 = DateTime.SpecifyKind(Convert.ToDateTime(ToDte), DateTimeKind.Utc);

                string[] strArr = { "Value", "Abbrevated", "vunit", "Equivalent", "eunit", "LikelyWords", "User", "UpdatedOn", "Approved" };
                var fields = Fields.Include(strArr).Exclude("_id");
                var qry = Query.And(Query.GTE("UpdatedOn", BsonDateTime.Create(dte)), Query.LT("UpdatedOn", BsonDateTime.Create(dte1.AddDays(1))));
                var lstCha = _AbbrevateRepository.FindAll(fields, qry).ToList();
                return lstCha;
            }
            else
            {


                string[] strArr = { "Value", "Abbrevated", "vunit", "Equivalent", "eunit", "LikelyWords", "User", "UpdatedOn", "Approved" };
                var fields = Fields.Include(strArr).Exclude("_id");
                var lstCha = _AbbrevateRepository.FindAll(fields).ToList();
                return lstCha;
            }

        }
        public virtual IEnumerable<Prosol_Charateristics> getvaluelist()
        {

            var abbrList = _CharacteristicRepository.FindAll();
            return abbrList;
        }
        //Vendortype
        public virtual bool CreateVendortype(Prosol_Vendortype vendor)
        {
            var res = false;
            var query = Query.EQ("Type", vendor.Type);
            var um = _VendortypeRepository.FindAll(query).ToList();
            if (um.Count == 0 || (um.Count == 1 && um[0]._id == vendor._id))
            {
                res = _VendortypeRepository.Add(vendor);
            }
            return res;
        }
        public virtual IEnumerable<Prosol_Vendortype> GetVendortypeList()
        {
            var vendorList = _VendortypeRepository.FindAll();
            return vendorList;
        }
        public virtual bool DeleteVendortype(string id)
        {
            var query = Query.EQ("Type", id);
            var res = _VendortypeRepository.Delete(query);
            return res;

        }

        //Reftype
        public virtual bool CreateReftype(Prosol_Reftype refs)
        {
            var res = false;
            var query = Query.EQ("Type", refs.Type);
            var um = _ReftypeRepository.FindAll(query).ToList();
            if (um.Count == 0 || (um.Count == 1 && um[0]._id == refs._id))
            {
                res = _ReftypeRepository.Add(refs);
            }
            return res;
        }
        public virtual IEnumerable<Prosol_Reftype> GetReftypeList()
        {
            var refsList = _ReftypeRepository.FindAll();
            return refsList;
        }
        public virtual bool DeleteReftype(string id)
        {
            var query = Query.EQ("Type", id);
            var res = _ReftypeRepository.Delete(query);
            return res;

        }
        //Logics

        //public virtual IEnumerable<Prosol_Attributes> GetAttributesList()
        //{
        //    var sort = SortBy.Ascending("Order");
        //    var AttributesList = _AttributesRepository.FindAll(sort);
        //    return AttributesList;
        //}
        public virtual IEnumerable<Prosol_Charateristics> GetAttributesList()
        {
            var sort = SortBy.Ascending("Characteristic");
            var AttributesList = _CharacteristicRepository.FindAll(sort);
            return AttributesList;
        }
        public bool CreateLogics(Prosol_Logics logic)
        {
            var res = false;
            var query = Query.And(Query.EQ("Noun", logic.Noun), Query.EQ("Modifier", logic.Modifier));
            var um = _LogicsRepository.FindAll(query).ToList();

            if (um.Count == 0 || (um.Count == 1 && um[0]._id == logic._id))
            {
                _LogicsRepository.Add(logic);
            }
            return res;

        }
        public Prosol_Logics GetLogic(string Noun, string Modifier)
        {
            var query = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier));
            var lgic = _LogicsRepository.FindOne(query);
            return lgic;
        }
        //UNSPSC

        public virtual int BulkUNSPSC(HttpPostedFileBase file)
        {
            int cunt = 0;
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
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

            var LstNM = new List<Prosol_UNSPSC>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].ToString() != "" && dr[1].ToString() != "" && dr[3].ToString() != "" && dr[4].ToString() != "" && dr[5].ToString() != "" && dr[6].ToString() != "" && dr[7].ToString() != "" && dr[10].ToString() != "")
                {
                    if (dr[9].ToString() != "")
                    {
                        //var query = Query.And(Query.EQ("Commodity", dr[9].ToString()), Query.EQ("Version", dr[11].ToString()));
                        //var lgic = _unspscRepository.FindAll(query).ToList();
                        //if (lgic.Count == 0)
                        //{

                            var Mdl = new Prosol_UNSPSC();
                            Mdl.Noun = dr[0].ToString();
                            Mdl.Modifier = dr[1].ToString();
                          //  Mdl.value = dr[2].ToString();
                            Mdl.Segment = dr[2].ToString();
                            Mdl.SegmentTitle = dr[3].ToString();
                            Mdl.Family = dr[4].ToString();
                            Mdl.FamilyTitle = dr[5].ToString();
                            Mdl.Class = dr[6].ToString();
                            Mdl.ClassTitle = dr[7].ToString();
                            Mdl.Commodity = dr[8].ToString();
                            Mdl.CommodityTitle = dr[9].ToString();
                            Mdl.Version = dr[10].ToString();
                            LstNM.Add(Mdl);
                       // }
                    }
                    else
                    {
                        //var query = Query.And(Query.EQ("Class", dr[7].ToString()), Query.EQ("Version", dr[11].ToString()), Query.EQ("Commodity", ""));
                        //var lgic = _unspscRepository.FindAll(query).ToList();
                        //if (lgic.Count == 0)
                        //{


                            var Mdl = new Prosol_UNSPSC();
                            Mdl.Noun = dr[0].ToString();
                            Mdl.Modifier = dr[1].ToString();
                            Mdl.value = dr[2].ToString();
                            Mdl.Segment = dr[3].ToString();
                            Mdl.SegmentTitle = dr[4].ToString();
                            Mdl.Family = dr[5].ToString();
                            Mdl.FamilyTitle = dr[6].ToString();
                            Mdl.Class = dr[7].ToString();
                            Mdl.ClassTitle = dr[8].ToString();
                            Mdl.Commodity = dr[9].ToString();
                            Mdl.CommodityTitle = dr[10].ToString();
                            Mdl.Version = dr[11].ToString();
                            LstNM.Add(Mdl);
                        //}
                    }
                }
            }
            if (LstNM.Count > 0)
            {
                cunt = _unspscRepository.Add(LstNM);

            }
            return cunt;

        }

        public IEnumerable<Prosol_UNSPSC> GetUnspsc(string Noun, string Modifier)
        {
            var unspscVersion = _CodeLogicRepository.FindOne();
            if(Noun != null && Modifier != null )
            {
                var query = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.EQ("Version", unspscVersion.unspsc_Version));
                var LstUnspsc = _unspscRepository.FindAll(query).ToList();
                return LstUnspsc;
            }
            return null;
        }
        public IEnumerable<Prosol_UNSPSC> GetUnspsc()
        {
            var unspscVersion = _CodeLogicRepository.FindOne();
            //  var query = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.EQ("Version", unspscVersion.unspsc_Version));
            var query = Query.EQ("Version", unspscVersion.unspsc_Version);
            var LstUnspsc = _unspscRepository.FindAll(query).ToList();
            return LstUnspsc;
        }
        public virtual bool Delunspsc(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _unspscRepository.Delete(query);
            return res;

        }

        public IEnumerable<Prosol_UNSPSC> GetUnspscc()
        {
            var LstUnspsc = _unspscRepository.FindAll();
            return LstUnspsc;
        }

        public virtual IEnumerable<Prosol_UNSPSC> GetUNSPSCListSearch(string srchtxt)
        {
            var query = Query.Or(Query.Matches("Noun", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Modifier", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Segment", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("SegmentTitle", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Family", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("FamilyTitle", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Class", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("ClassTitle", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Commodity", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("CommodityTitle", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Version", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))));
            var unspscList = _unspscRepository.FindAll(query);
            return unspscList;
        }


        public string[] loadversion()
        {
            return _unspscRepository.AutoSearch1("Version");
            //  retu
        }

        public Prosol_Vendor getVendorAbbrForShortDesc(string mfr)
        {
            var queryyy = Query.EQ("Name", mfr);
            var res = _VendorRepository.FindOne(queryyy);
            return res;
        }
        //public Prosol_Abbrevate GetAbbr(string value, string abb)
        //{
        //    var query = Query.And(Query.EQ("Value", value), Query.EQ("Abbrevated", abb));
        //    var um = _AbbrevateRepository.FindOne(query);
        //    return um;
        //}
        //NEW CODE FOR VENDOR MASTER
        public Prosol_Vendor FINDVENDORMASTER(string mfr)
        {
            var queryyy = Query.EQ("Name", mfr);
            var res = _VendorRepository.FindOne(queryyy);
            return res;
        }


        //public virtual string BulkData1(HttpPostedFileBase file)
        //{
        //    int cunt = 0, lineId = 0;

        //    Stream stream = file.InputStream;
        //    IExcelDataReader reader = null;
        //    if (file.FileName.EndsWith(".xls"))
        //    {
        //        reader = ExcelReaderFactory.CreateBinaryReader(stream);
        //    }
        //    else if (file.FileName.EndsWith(".xlsx"))
        //    {
        //        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //    }
        //    reader.IsFirstRowAsColumnNames = true;
        //    var res = reader.AsDataSet();
        //    reader.Close();
        //    DataTable dt1 = res.Tables[0];

        //    string[] columns = { "UniqueId", "UserName" };


        //    if (dt1.Rows.Count > 0)
        //    {

        //        foreach (DataRow drw1 in dt1.Rows)
        //        {

        //            var query1 = Query.And(Query.EQ("Materialcode", drw1[0].ToString()));
        //            var mdl1 = _datamasterRepository.FindOne(query1);
        //            //var query2 = Query.And(Query.EQ("Noun", mdl1.Noun), Query.EQ("Modifier", mdl1.Modifier));
        //            //var mdl2 = _nounModifierRepository.FindOne(query2);

        //            if (mdl1 != null)
        //            {
        //                int flg = 0; int inc = 0, sup = 0;
        //                if (mdl1.Vendorsuppliers != null && mdl1.Vendorsuppliers.Count > 0)
        //                {

        //                    foreach (var v in mdl1.Vendorsuppliers)
        //                    {

        //                        //if (!string.IsNullOrEmpty(drw1["NAME_" + inc].ToString()) || !string.IsNullOrEmpty(drw1["NO_" + inc].ToString()))
        //                        //{
        //                        v.Type = drw1["LABEL_" + inc].ToString();
        //                        v.Name = drw1["NAME_" + inc].ToString();
        //                        v.Refflag = drw1["FLAG_" + inc].ToString();
        //                        v.RefNo = drw1["NO_" + inc].ToString();
        //                        v.s = Convert.ToInt16(drw1["S_" + inc]);
        //                        v.l = Convert.ToInt16(drw1["L_" + inc]);
        //                        //   }
        //                        inc++;


        //                    }
        //                    while (dt1.Columns.Contains("NAME_" + inc) && (!string.IsNullOrEmpty(drw1["NAME_" + inc].ToString()) || !string.IsNullOrEmpty(drw1["NO_" + inc].ToString()) || !string.IsNullOrEmpty(drw1["FLAG_" + inc].ToString())))
        //                    {
        //                        var v = new Vendorsuppliers();
        //                        v.Type = drw1["LABEL_" + inc].ToString();
        //                        v.Name = drw1["NAME_" + inc].ToString();
        //                        v.Refflag = drw1["FLAG_" + inc].ToString();
        //                        v.RefNo = drw1["NO_" + inc].ToString();
        //                        v.s = Convert.ToInt16(drw1["S_" + inc]);
        //                        v.l = Convert.ToInt16(drw1["L_" + inc]);
        //                        inc++;
        //                        mdl1.Vendorsuppliers.Add(v);
        //                    }
        //                }
        //                else
        //                {
        //                    while (dt1.Columns.Contains("NAME_" + inc) && (!string.IsNullOrEmpty(drw1["NAME_" + inc].ToString()) || !string.IsNullOrEmpty(drw1["NO_" + inc].ToString())))
        //                    {
        //                        mdl1.Vendorsuppliers = new List<Vendorsuppliers>();
        //                        var v = new Vendorsuppliers();
        //                        v.Type = drw1["LABEL_" + inc].ToString();
        //                        v.Name = drw1["NAME_" + inc].ToString();
        //                        v.Refflag = drw1["FLAG_" + inc].ToString();
        //                        v.RefNo = drw1["NO_" + inc].ToString();
        //                        v.s = Convert.ToInt16(drw1["S_" + inc]);
        //                        v.l = Convert.ToInt16(drw1["L_" + inc]);
        //                        mdl1.Vendorsuppliers.Add(v);
        //                        inc++;
        //                    }
        //                }

        //                //if (mdl1.Vendorsuppliers != null)
        //                //{
        //                //    foreach (var v in mdl1.Vendorsuppliers)
        //                //    {
        //                //        if (!string.IsNullOrEmpty(v.Name))
        //                //        {
        //                //            var q = Query.EQ("Name", v.Name);
        //                //            var m = _VendorRepository.FindOne(q);
        //                //            if (m == null)
        //                //            {
        //                //                return "Itemcode:- " + mdl1.Itemcode + " Please add Vendor in VendorMaster " + v.Name;
        //                //            }
        //                //        }

        //                //    }
        //                //}

        //                //if ((mdl2.Formatted == 0 || mdl2.Formatted == 2))
        //                //{
        //                //    int ven = 0;
        //                //    if (mdl1.Vendorsuppliers != null)
        //                //    {
        //                //        foreach (var v in mdl1.Vendorsuppliers)
        //                //        {
        //                //            if (v.s == 1)
        //                //            {
        //                //                ven = 1;
        //                //            }

        //                //        }
        //                //    }
        //                //    if (ven == 0)
        //                //    {
        //                //        return "Itemcode:- " + mdl1.Itemcode + " Please Check OPM,OEM Vendor";

        //                //    }
        //                //}

        //                _datamasterRepository.Add(mdl1);
        //                cunt++;
        //            }


        //        }
        //    }


        //    return cunt + " Items assigned successfully";


        //}
        public virtual string BulkData(HttpPostedFileBase file)
        {
            int cunt = 0, lineId = 0;

            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
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
            DataTable dt1 = res.Tables[0];

            if (dt1.Rows.Count > 0)
            {
                //var charLst = _CharacteristicRepository.FindAll().ToList();
                //foreach (var cha in charLst)
                //{
                //    string targetNoun = cha.Noun;
                //    string targetModifier = cha.Modifier;
                //    string characteristicToMove = "MATERIAL";
                //    int newSequenceForMaterial = 2;

                //    var materialItem = charLst.Find(item => item.Noun == targetNoun && item.Modifier == targetModifier && item.Characteristic == characteristicToMove);
                //    var targetItem = charLst.Find(item => item.Noun == targetNoun && item.Modifier == targetModifier && item.ShortSquence == newSequenceForMaterial);

                //    if (materialItem != null && targetItem != null)
                //    {
                //        int tempSequence = materialItem.ShortSquence;
                //        materialItem.ShortSquence = targetItem.ShortSquence;
                //        targetItem.ShortSquence = tempSequence;
                //    }
                //}

                //var groupedMatLst = matLst.GroupBy(m => m.Materialcode)
                //                          .Where(g => g.Count() > 1)
                //                          .SelectMany(g => g)
                //                          .ToList();

                //foreach (var item in groupedMatLst)
                //{
                //    var query = Query.EQ("Materialcode", item.Materialcode);
                //    var Updte = Update.Set("Materialcode", "delete");
                //    var flg = UpdateFlags.Upsert;

                //    var theresult = _datamasterRepository.Update(query, Updte, flg);
                //}

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query2 = Query.EQ("UniqueId", drw1[0].ToString());
                //    var mdl2 = _assetmasterRepository.FindOne(query2);
                //    if (mdl2 != null)
                //    {
                //        //mdl2.AdditionalNotes = drw1[1].ToString();
                //        //mdl2.RegNo = drw1[1].ToString();
                //        mdl2.RegNo = drw1[5].ToString();
                //        mdl2.InsuranceCat = drw1[6].ToString();
                //        mdl2.InsuranceNo = drw1[7].ToString();
                //        mdl2.InsuranceEndDate = drw1[8].ToString();
                //        //mdl2.ItemStatus = 6;
                //        _assetmasterRepository.Add(mdl2);
                //    }
                //    cunt++;
                //}


                //Access

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query2 = Query.EQ("Roles.Name", "PV User");
                //    var mdl2 = _UsersRepository.FindAll(query2).ToList();
                //    if (mdl2.Count > 0)
                //    {
                //        foreach (var m in mdl2)
                //        {
                //            var acs = new Prosol_Access();
                //            acs.Userid = m.Userid;
                //            acs.Pages = "Asset Assign";
                //            acs.Status = "1";
                //            acs.Createdon = DateTime.SpecifyKind(DateTime.Now,DateTimeKind.Utc);
                //            _accessRepository.Add(acs);
                //        }
                //    }
                //    cunt++;
                //}

                //Image Swap

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query1 = Query.Or(Query.EQ("AssetNo", drw1[0].ToString()), Query.EQ("UniqueId", drw1[0].ToString()));
                //    var mdl1 = _assetmasterRepository.FindOne(query1);
                //    var query2 = Query.Or(Query.EQ("AssetNo", drw1[1].ToString()), Query.EQ("UniqueId", drw1[1].ToString()));
                //    var mdl2 = _assetmasterRepository.FindOne(query2);
                //    if (mdl1 != null)
                //    {
                //        if (mdl2 != null)
                //        {
                //            //mdl2.AssetImages = new AssetImage();
                //            //mdl2.AssetImages.AssetImgs = mdl1.AssetImages.AssetImgs;
                //            //mdl2.AssetImages.MatImgs = mdl1.AssetImages.MatImgs;
                //            //mdl2.AssetImages.NamePlateImge = mdl1.AssetImages.NamePlateImge;

                //            mdl2.AssetImages.NamePlateImgeTwo = mdl1.AssetImages.NamePlateImgeTwo;

                //            //if (mdl2?.AssetImages?.NamePlateImgeTwo != null && mdl1?.AssetImages?.NamePlateImgeTwo != null)
                //            //{
                //            //    var namePlateList = mdl2.AssetImages.NamePlateImgeTwo.ToList();
                //            //    if (mdl1.AssetImages.NamePlateImgeTwo.Count() > 1)
                //            //    {
                //            //        namePlateList.Add(mdl1.AssetImages.NamePlateImgeTwo[1]);
                //            //    }
                //            //    mdl2.AssetImages.NamePlateImgeTwo = namePlateList.ToArray(); 
                //            //}
                //            //else
                //            //{
                //            //    mdl2.AssetImages.NamePlateImgeTwo = new string[0];
                //            //    var namePlateList = mdl2.AssetImages.NamePlateImgeTwo.ToList();
                //            //    if (mdl1.AssetImages.NamePlateImgeTwo.Count() > 1)
                //            //    {
                //            //        namePlateList.Add(mdl1.AssetImages.NamePlateImgeTwo[1]);
                //            //    }
                //            //    mdl2.AssetImages.NamePlateImgeTwo = namePlateList.ToArray();
                //            //}
                //            //mdl2.AssetImages.NewTagImage = mdl1.AssetImages.NewTagImage;
                //            //mdl2.NewTagNo = mdl1.NewTagNo;
                //            //mdl2.Description = drw1[2].ToString();
                //            //mdl2.Description_ = drw1[3].ToString();
                //            _assetmasterRepository.Add(mdl2);
                //        }
                //    }
                //    cunt++;
                //}

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query1 = Query.Or(Query.EQ("AssetNo", drw1[0].ToString()), Query.EQ("UniqueId", drw1[0].ToString()));
                //    var mdl1 = _AssetAttributesRepository.FindOne(query1);
                //    var query2 = Query.Or(Query.EQ("AssetNo", drw1[1].ToString()), Query.EQ("UniqueId", drw1[1].ToString()));
                //    var mdl2 = _AssetAttributesRepository.FindOne(query2);
                //    if (mdl1 != null)
                //    {
                //        if (mdl2 != null)
                //        {
                //            mdl2.Characterisitics = mdl1.Characterisitics;
                //            _AssetAttributesRepository.Add(mdl2);
                //        }
                //        else
                //        {
                //            mdl2 = new Prosol_AssetAttributes();
                //            mdl2.Characterisitics = mdl1.Characterisitics;
                //            _AssetAttributesRepository.Add(mdl2);
                //        }
                //    }
                //    cunt++;
                //}

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query1 = Query.Or(Query.EQ("AssetNo", drw1[0].ToString()), Query.EQ("UniqueId", drw1[0].ToString()));
                //    var mdl1 = _assetmasterRepository.FindOne(query1);
                //    if (mdl1 != null)
                //    {
                //        //var tag = new string[1];
                //        //tag[0] = drw1[1].ToString();
                //        //mdl1.AssetImages.NewTagImage = tag;
                //        mdl1.UniqueId = drw1[1].ToString();
                //        _assetmasterRepository.Add(mdl1);
                //    }
                //    cunt++;
                //}

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query2 = Query.EQ("UniqueId", drw1[0].ToString());
                //    var mdl2 = _assetmasterRepository.FindOne(query2);
                //    var imgC = drw1[2].ToString();
                //    int imgCnt = Convert.ToInt32(imgC);
                //    var maxiC = drw1[4].ToString();
                //    int maxImgCnt = Convert.ToInt32(maxiC);
                //    var npC = drw1[6].ToString();
                //    int npImgCnt = Convert.ToInt32(maxiC);
                //    if (mdl2 != null)
                //    {
                //        if (!string.IsNullOrEmpty(drw1[1].ToString()))
                //        {
                //            mdl2.AssetImages.AssetImgs = new string[imgCnt];
                //            for (var j = 0; j < imgCnt; j++)
                //            {
                //                string img = "https://d30f70hjt97rz5.cloudfront.net/" + drw1[1].ToString() + (j + 1) + ".jpg";
                //                mdl2.AssetImages.AssetImgs[j] = img;
                //            }
                //            mdl2.AssetImages.MatImgs = new string[maxImgCnt];
                //            for (var j = 0; j < maxImgCnt; j++)
                //            {
                //                string img = "https://d30f70hjt97rz5.cloudfront.net/" + drw1[3].ToString() + (j + 1) + ".jpg";
                //                mdl2.AssetImages.MatImgs[j] = img;
                //            }
                //            mdl2.AssetImages.NamePlateImge = new string[npImgCnt]; 
                //            for (var j = 0; j < npImgCnt; j++)
                //            {
                //                string img = "https://d30f70hjt97rz5.cloudfront.net/" + drw1[5].ToString() + (j + 1) + ".jpg";
                //                mdl2.AssetImages.NamePlateImge[j] = img;
                //            }
                //        }
                //    }

                //    _assetmasterRepository.Add(mdl2);
                //    cunt++;
                //}

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query1 = Query.EQ("AssetNo", drw1[0].ToString());
                //    var mdl = _assetmasterRepository.FindOne(query1);
                //    if (mdl != null)
                //    {
                //        int status = 1;
                //        if(drw1[1].ToString() == "Catalogue Pending")
                //        {
                //            status = 2;
                //        }
                //        else if(drw1[1].ToString() == "Catalogue Saved")
                //        {
                //            status = 3;
                //        }
                //        else if(drw1[1].ToString() == "QC Pending")
                //        {
                //            status = 4;
                //        }
                //        else if(drw1[1].ToString() == "QC Saved")
                //        {
                //            status = 5;
                //        }
                //        mdl.ItemStatus = status;
                //        //var usrQry = Query.Matches("UserName", BsonRegularExpression.Create(new Regex(drw1[2].ToString().TrimStart().TrimEnd(), RegexOptions.IgnoreCase)));
                //        var usrQry = Query.Matches("UserName", drw1[2].ToString());
                //        var usrInfo = _UsersRepository.FindOne(usrQry);
                //        var pv = new Prosol_UpdatedBy();
                //        pv.UserId = usrInfo.Userid;
                //        pv.Name = usrInfo.UserName;
                //        pv.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //        mdl.Catalogue = pv;
                //        _assetmasterRepository.Add(mdl);
                //        cunt++;
                //    }
                //}


                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query1 = Query.EQ("Materialcode", drw1[0].ToString());
                //    var mdl = _datamasterRepository.FindOne(query1);
                //    if (mdl != null)
                //    {
                //        mdl.Noun = drw1[1].ToString();
                //        mdl.Modifier = drw1[2].ToString();
                //        _datamasterRepository.Add(mdl);
                //        cunt++;
                //    }
                //}


                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query1 = Query.Or(Query.EQ("Itemcode", drw1[0].ToString()), Query.EQ("Materialcode", drw1[0].ToString()));
                //    var mdl = _datamasterRepository.FindOne(query1);
                //    if (mdl != null)
                //    {
                //        int idx = mdl.Characteristics.FindIndex(i => i.Characteristic == drw1[1].ToString());
                //        mdl.Characteristics[idx].Value = drw1[2].ToString();
                //        _datamasterRepository.Add(mdl);
                //        cunt++;
                //    }
                //}

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query1 = Query.EQ("UniqueId", drw1[0].ToString());
                //    var mdl = _assetmasterRepository.FindOne(query1);
                //    if (mdl != null)
                //    {
                //        mdl.Manufacturer = drw1[1].ToString();
                //        mdl.Manufacture = drw1[1].ToString();
                //        mdl.ModelNo = drw1[2].ToString();
                //        mdl.SerialNo = drw1[3].ToString();
                //        mdl.ModelYear = drw1[4].ToString();
                //        //mdl.RegNo = drw1[5].ToString();
                //        mdl.Soureurl = drw1[5].ToString();
                //        mdl.Rework_Remarks = drw1[6].ToString();
                //        _assetmasterRepository.Add(mdl);
                //        cunt++;
                //    }
                //}

                //var lst = _datamasterRepository.FindAll().ToList();
                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    int lstCount = lst.Count() + cunt;
                //    var mdl = new Prosol_Datamaster();
                //    var itemCode = "AD2" + lstCount.ToString("D7");
                //    mdl.Materialcode = drw1[0].ToString();
                //    mdl.Legacy2 = drw1[1].ToString();
                //    mdl.exPartno = drw1[2].ToString();
                //    mdl.Legacy = drw1[3].ToString();
                //    mdl.Itemcode = itemCode;
                //    _datamasterRepository.Add(mdl);
                //    cunt++;
                //}

                //var allQry = Query.Or(Query.Matches("AssetImages.AssetImgs", new BsonRegularExpression(new Regex("iStock", RegexOptions.IgnoreCase))),Query.Matches("AssetImages.MatImgs", new BsonRegularExpression(new Regex("iStock", RegexOptions.IgnoreCase))),Query.Matches("AssetImages.AssetImgs", new BsonRegularExpression(new Regex("iStock", RegexOptions.IgnoreCase))),Query.Matches("AssetImages.AssetImgs", new BsonRegularExpression(new Regex("iStock", RegexOptions.IgnoreCase))),Query.Matches("AssetImages.AssetImgs", new BsonRegularExpression(new Regex("iStock", RegexOptions.IgnoreCase))));

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query1 = Query.Or(Query.EQ("AssetNo", drw1[0].ToString()), Query.EQ("UniqueId", drw1[0].ToString()));
                //    var mdl1 = _assetmasterRepository.FindOne(query1);
                //    if (mdl1 != null)
                //    {
                //        mdl1.TechIdentNo = drw1[1].ToString();
                //        //mdl1.SiteId = drw1[1].ToString();
                //        //mdl1.WarrentyExpDate = drw1[2].ToString();
                //        //mdl1.CalCertExpDate = drw1[3].ToString();
                //        //mdl1.CertExpDate = drw1[4].ToString();
                //        //mdl1.InsuranceStartDate = drw1[5].ToString();
                //        //mdl1.InsuranceEndDate = drw1[6].ToString();
                //        //mdl1.LoadCertExpDate = drw1[7].ToString();

                //        _assetmasterRepository.Add(mdl1);
                //        cunt++;
                //    }
                //}

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var venQry = Query.EQ("Name", drw1[0].ToString());
                //    var ven = _VendorRepository.FindOne(venQry);
                //    var abQry = Query.EQ("Value", drw1[0].ToString());
                //    var ab = _AbbrevateRepository.FindOne(abQry);
                //    if (ven != null)
                //    {
                //        ven.ShortDescName = drw1[1].ToString();
                //        _VendorRepository.Add(ven);
                //    }
                //    else
                //    {
                //        ven = new Prosol_Vendor();
                //        ven.Name = drw1[0].ToString();
                //        ven.ShortDescName = drw1[1].ToString();
                //        _VendorRepository.Add(ven);
                //    }
                //    if (ab != null)
                //    {
                //        ab.Abbrevated = drw1[1].ToString();
                //        _AbbrevateRepository.Add(ab);
                //    }
                //    else
                //    {
                //        ab = new Prosol_Abbrevate();
                //        ab.Value = drw1[0].ToString();
                //        ab.Abbrevated = drw1[1].ToString();
                //        _AbbrevateRepository.Add(ab);
                //    }
                //    cunt++;
                //}

                //Swap

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query1 = Query.Or(Query.EQ("UniqueId", drw1[1].ToString()), Query.EQ("AssetNo", drw1[1].ToString()));
                //    var assetvalues = _assetmasterRepository.FindOne(query1);
                //    var query2 = Query.Or(Query.EQ("UniqueId", drw1[0].ToString()), Query.EQ("AssetNo", drw1[0].ToString()));
                //    var catasset = _assetmasterRepository.FindOne(query2);
                //    if (assetvalues != null)
                //    {
                //        if (assetvalues.AssetImages != null)
                //        {
                //            var Asset_imgs = new AssetImage();

                //            if (assetvalues.AssetImages.AssetImgs != null && assetvalues.AssetImages.AssetImgs.Length > 0)
                //                Asset_imgs.AssetImgs = assetvalues.AssetImages.AssetImgs;

                //            if (assetvalues.AssetImages.MatImgs != null && assetvalues.AssetImages.MatImgs.Length > 0)
                //                Asset_imgs.MatImgs = assetvalues.AssetImages.MatImgs;

                //            if (assetvalues.AssetImages.NamePlateImge != null && assetvalues.AssetImages.NamePlateImge.Length > 0)
                //                Asset_imgs.NamePlateImge = assetvalues.AssetImages.NamePlateImge;

                //            if (assetvalues.AssetImages.NamePlateImgeTwo != null && assetvalues.AssetImages.NamePlateImgeTwo.Length > 0)
                //                Asset_imgs.NamePlateImgeTwo = assetvalues.AssetImages.NamePlateImgeTwo;

                //            if (assetvalues.AssetImages.NamePlateText != null && assetvalues.AssetImages.NamePlateText.Length > 0)
                //                Asset_imgs.NamePlateText = assetvalues.AssetImages.NamePlateText;

                //            if (assetvalues.AssetImages.NamePlateTextTwo != null && assetvalues.AssetImages.NamePlateTextTwo.Length > 0)
                //                Asset_imgs.NamePlateTextTwo = assetvalues.AssetImages.NamePlateTextTwo;

                //            if (assetvalues.AssetImages.NameplateImgs != null && assetvalues.AssetImages.NameplateImgs.Length > 0)
                //                Asset_imgs.NameplateImgs = assetvalues.AssetImages.NameplateImgs;

                //            if (assetvalues.AssetImages.NewTagImage != null && assetvalues.AssetImages.NewTagImage.Length > 0)
                //                Asset_imgs.NewTagImage = assetvalues.AssetImages.NewTagImage;

                //            if (assetvalues.AssetImages.OldTagImage != null && assetvalues.AssetImages.OldTagImage.Length > 0)
                //                Asset_imgs.OldTagImage = assetvalues.AssetImages.OldTagImage;

                //            catasset.AssetImages = Asset_imgs;
                //        }

                //        if (assetvalues.GIS != null)
                //        {
                //            var gis = new Prosol_AssetGIS();
                //            gis.LattitudeStart = assetvalues.GIS.LattitudeStart;
                //            gis.LattitudeEnd = assetvalues.GIS.LattitudeEnd;
                //            gis.LongitudeStart = assetvalues.GIS.LongitudeStart;
                //            gis.LongitudeEnd = assetvalues.GIS.LongitudeEnd;
                //            gis.Lat_LongLength = assetvalues.GIS.Lat_LongLength;
                //            catasset.GIS = gis;
                //        }
                //        if (assetvalues.AssetCondition != null)
                //        {
                //            var Asset_Cndt = new Prosol_AssetCondition();

                //            Asset_Cndt.Corrosion = assetvalues.AssetCondition.Corrosion;
                //            Asset_Cndt.Damage = assetvalues.AssetCondition.Damage;
                //            Asset_Cndt.Leakage = assetvalues.AssetCondition.Leakage;
                //            Asset_Cndt.Vibration = assetvalues.AssetCondition.Vibration;
                //            Asset_Cndt.Temparature = assetvalues.AssetCondition.Temparature;
                //            Asset_Cndt.Smell = assetvalues.AssetCondition.Smell;
                //            Asset_Cndt.Noise = assetvalues.AssetCondition.Noise;
                //            Asset_Cndt.Rank = assetvalues.AssetCondition.Rank;
                //            Asset_Cndt.Asset_Condition = assetvalues.AssetCondition.Asset_Condition;
                //            Asset_Cndt.CorrosionImage = assetvalues.AssetCondition.CorrosionImage;
                //            Asset_Cndt.DamageImage = assetvalues.AssetCondition.DamageImage;
                //            Asset_Cndt.LeakageImage = assetvalues.AssetCondition.LeakageImage;
                //            catasset.AssetCondition = Asset_Cndt;
                //        }
                //        catasset.AssetQRCode = assetvalues.AssetQRCode;
                //        catasset.OldTagNo = assetvalues.OldTagNo;
                //        catasset.NewTagNo = assetvalues.NewTagNo;
                //        catasset.ManagedBy = assetvalues.ManagedBy;
                //        catasset.Ownedby = assetvalues.Ownedby;
                //        catasset.Operatedby = assetvalues.Operatedby;
                //        catasset.Maintainer = assetvalues.Maintainer;
                //        catasset.PresentLocation = assetvalues.PresentLocation;
                //        catasset.AdditionalNotes = assetvalues.AdditionalNotes;
                //        catasset.assetConditionRemarks = assetvalues.assetConditionRemarks;
                //        catasset.Remarks = assetvalues.Remarks;
                //        catasset.Rework_Remarks = assetvalues.Rework_Remarks;
                //        catasset.AdditionalNotes = assetvalues.AdditionalNotes;
                //        catasset.Manufacturer = assetvalues.Manufacturer;
                //        catasset.SerialNo = assetvalues.SerialNo;
                //        catasset.ModelNo = assetvalues.ModelNo;
                //        catasset.ModelYear = assetvalues.ModelYear;
                //        catasset.PVuser = assetvalues.PVuser;
                //        catasset.Catalogue = assetvalues.Catalogue;
                //        catasset.ItemStatus = 2;
                //        catasset.PVstatus = assetvalues.PVstatus;

                //        _assetmasterRepository.Add(catasset);
                //    }
                //    cunt++;
                //}

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query2 = Query.EQ("Materialcode", drw1[0].ToString());
                //    var mdl2 = _datamasterRepository.FindOne(query2);
                //    if (mdl2 != null)
                //    {
                //        mdl2.Noun = drw1[1].ToString();
                //        mdl2.Modifier = drw1[2].ToString();
                //        //mdl2.Equipment = new Equipments();
                //        //mdl2.Characteristics = new List<Prosol_AttributeList>();
                //        //mdl2.Vendorsuppliers = new List<Vendorsuppliers>();
                //        //mdl2.Shortdesc = null;
                //        //mdl2.Shortdesc_ = null;
                //        //mdl2.Longdesc = null;
                //        _datamasterRepository.Add(mdl2);
                //        cunt++;
                //    }
                //}

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query2 = Query.EQ("UniqueId", drw1[0].ToString());
                //    var mdl2 = _assetmasterRepository.FindOne(query2);
                //    if (mdl2 != null)
                //    {

                //        var bom = new BOM();
                //        if (mdl2.Bom != null)
                //        {
                //            bom = mdl2.Bom;
                //            bom.BOMId = mdl2.Bom.BOMId;
                //            bom.BOMDescription = mdl2.Bom.BOMDescription;
                //        }
                //        else
                //        {
                //            bom.BOMId = drw1[2].ToString();
                //            bom.BOMDescription = drw1[3].ToString();
                //        }
                //        if (mdl2.Bom != null && mdl2.Bom.Assembly != null)
                //            bom.Assembly = mdl2.Bom.Assembly;
                //        else
                //            bom.Assembly = new List<ASSEMBLY>();
                //        if (!string.IsNullOrEmpty(drw1[4].ToString()))
                //        {
                //            var assmb = new ASSEMBLY();
                //            assmb.AssemblyId = drw1[4].ToString();
                //            assmb.AssemblyDescription = drw1[5].ToString();
                //            bom.Assembly.Add(assmb);
                //        }
                //        if (mdl2.Bom != null && mdl2.Bom.Mat != null)
                //            bom.Mat = mdl2.Bom.Mat;
                //        else
                //            bom.Mat = new List<MAT>();
                //        if (!string.IsNullOrEmpty(drw1[6].ToString()))
                //        {
                //            var mtl = new MAT();
                //            mtl.Materialcode = drw1[6].ToString();
                //            mtl.MaterialDescription = drw1[7].ToString();
                //            mtl.Quantity = drw1[8].ToString();
                //            mtl.UOM = drw1[9].ToString();
                //            bom.Mat.Add(mtl);
                //        }
                //        mdl2.Bom = bom;
                //        _assetmasterRepository.Add(mdl2);
                //    }
                //    cunt++;
                //}
                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var qry = Query.And(Query.EQ("Noun", drw1[0].ToString()),Query.EQ("Modifier", drw1[1].ToString()),Query.EQ("Characteristic", drw1[2].ToString()),Query.EQ("Definition", "Equ"));
                //    var mdl2 = _CharacteristicRepository.FindOne(qry);
                //    if(mdl2 != null)
                //    {
                //        mdl2.ClassificationId = drw1[3].ToString();
                //        mdl2.HierarchyPath = drw1[4].ToString();
                //        mdl2.PDesc = drw1[5].ToString();
                //        mdl2.ClassLevel = drw1[6].ToString();
                //        _CharacteristicRepository.Add(mdl2);
                //        cunt++;
                //    }
                //}
                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query2 = Query.EQ("Materialcode", drw1[0].ToString());
                //    //var query2 = Query.And(Query.EQ("Noun", drw1[0].ToString()), Query.EQ("Modifier", drw1[1].ToString()), Query.EQ("RP", "Equ"));
                //    var mdl2 = _datamasterRepository.FindOne(query2);
                //    if (mdl2 != null)
                //    {
                //        mdl2.Remarks = drw1[1].ToString();
                //        //mdl2.UOM = drw1[1].ToString();
                //        //mdl2.exManufacturer = drw1[2].ToString();
                //        //mdl2.exPartno = drw1[3].ToString();
                //        _datamasterRepository.Add(mdl2);
                //        cunt++;
                //    }
                //}

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query2 = Query.And(Query.EQ("Noun", drw1[0].ToString()), Query.EQ("Modifier", drw1[1].ToString()), Query.EQ("RP", "Equ"));
                //    var mdl2 = _nounModifierRepository.FindOne(query2);
                //    if (mdl2 != null)
                //    {
                //        var cha = new Prosol_Charateristics();
                //        cha.Noun = drw1[0].ToString();
                //        cha.Modifier = drw1[1].ToString();
                //        cha.Characteristic = drw1[2].ToString();
                //        cha.Abbrivation = drw1[3].ToString();
                //        cha.Squence = Convert.ToInt32(drw1[4].ToString());
                //        cha.ShortSquence = Convert.ToInt32(drw1[4].ToString());
                //        cha.Mandatory = "No";
                //        cha.Definition = "Equ";
                //        _CharacteristicRepository.Add(cha);
                //        cunt++;
                //    }
                //}

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query2 = Query.And(Query.EQ("Noun", drw1[0].ToString()), Query.EQ("Modifier", drw1[1].ToString()), Query.EQ("Characteristic", "ADDITIONAL INFORMATION"), Query.EQ("Definition", "Equ"));
                //    var mdl2 = _CharacteristicRepository.FindOne(query2);
                //    if (mdl2 != null)
                //    {
                //        var query = Query.And(Query.EQ("Noun", drw1[0].ToString()), Query.EQ("Modifier", drw1[1].ToString()),  Query.EQ("Definition", "Equ"));
                //        var mdlCnt = _CharacteristicRepository.FindAll(query).ToList().Count().ToString();
                //        string seq = mdlCnt + "0";
                //        mdl2.Squence = Convert.ToInt32(seq);
                //        mdl2.ShortSquence = Convert.ToInt32(seq);
                //        _CharacteristicRepository.Add(mdl2);
                //        cunt++;
                //    }
                //}

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query2 = Query.EQ("Materialcode", drw1[0].ToString());
                //    var mdl2 = _datamasterRepository.FindOne(query2);
                //    if (mdl2 != null)
                //    {
                //        //mdl2 = new Prosol_Datamaster();
                //        //mdl2.Itemcode = "AD0" + drw1[0].ToString();
                //        //mdl2.Materialcode = drw1[0].ToString();
                //        //mdl2.Legacy2 = drw1[1].ToString();
                //        //mdl2.Legacy = drw1[2].ToString();
                //        //mdl2.exPartno = drw1[3].ToString();
                //        //mdl2.Noun = drw1[1].ToString();
                //        //mdl2.Modifier = drw1[2].ToString();
                //        //mdl2.Characteristics = new List<Prosol_AttributeList>();
                //        //mdl2.Shortdesc = null;
                //        //mdl2.Shortdesc_ = null;
                //        //mdl2.Longdesc = null;
                //        //mdl2.exPartno = drw1[1].ToString();
                //        //mdl2.Equipment = new Equipments();
                //        //mdl2.Vendorsuppliers = new List<Vendorsuppliers>();
                //        if (!string.IsNullOrEmpty(drw1[1].ToString()))
                //        {
                //            mdl2.AssetImages = new MatImage();
                //            string dumbCnt = drw1[1].ToString();
                //            var imgCnt = Convert.ToInt32(dumbCnt);
                //            for (var i = 0; i < imgCnt; i++)
                //            {
                //                mdl2.AssetImages.AssetImgs = new string[imgCnt];

                //                for (var j = 0; j < imgCnt; j++)
                //                {
                //                    string img = "https://d3m7ar6xbcugf1.cloudfront.net/" + mdl2.Materialcode + "_" + (j + 1) + ".jpg";
                //                    mdl2.AssetImages.AssetImgs[j] = img;
                //                }
                //            }
                //        }
                //        _datamasterRepository.Add(mdl2);
                //    }
                //    cunt++;
                //}


                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //var query2 = Query.EQ("Materialcode", drw1[0].ToString());
                //var mdl2 = _datamasterRepository.FindOne(query2);
                //if (mdl2 != null)
                //{
                //mdl2.Legacy2 = drw1[1].ToString();
                //mdl2.exNoun = drw1[1].ToString();
                //mdl2.exModifier = drw1[2].ToString();
                //mdl2.Noun = drw1[1].ToString();
                //mdl2.Modifier = drw1[2].ToString();
                //mdl2.Type = drw1[6].ToString();
                //mdl2.UOM = drw1[7].ToString();

                //var usrQry = Query.Matches("UserName", BsonRegularExpression.Create(new Regex(drw1[1].ToString().TrimStart().TrimEnd(), RegexOptions.IgnoreCase)));
                //var usrInfo = _UsersRepository.FindOne(usrQry);
                //var pv = new Prosol_UpdatedBy();
                //pv.UserId = usrInfo.Userid;
                //pv.Name = usrInfo.UserName;
                //pv.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //mdl2.PVuser = pv;
                //mdl2.PVstatus = "Completed";

                //_datamasterRepository.Add(mdl2);
                //}
                //cunt++;
                //}

                //foreach (DataRow drw1 in dt1.Rows)
                //{

                //    var query1 = Query.And(Query.EQ("Materialcode", drw1[0].ToString()));
                //    var mdl1 = _datamasterRepository.FindOne(query1);

                //    if (mdl1 != null)
                //    {
                //        int flg = 0; int inc = 0, sup = 0;
                //        if (mdl1.Vendorsuppliers != null && mdl1.Vendorsuppliers.Count > 0)
                //        {

                //            foreach (var v in mdl1.Vendorsuppliers)
                //            {

                //                //if (!string.IsNullOrEmpty(drw1["NAME_" + inc].ToString()) || !string.IsNullOrEmpty(drw1["NO_" + inc].ToString()))
                //                //{
                //                v.Type = drw1["LABEL_" + inc].ToString();
                //                v.Name = drw1["NAME_" + inc].ToString();
                //                v.Refflag = drw1["FLAG_" + inc].ToString();
                //                v.RefNo = drw1["NO_" + inc].ToString();
                //                v.s = 1;
                //                v.l = 1;
                //                //   }
                //                inc++;


                //            }
                //            while (dt1.Columns.Contains("NAME_" + inc) && (!string.IsNullOrEmpty(drw1["NAME_" + inc].ToString()) || !string.IsNullOrEmpty(drw1["NO_" + inc].ToString()) || !string.IsNullOrEmpty(drw1["FLAG_" + inc].ToString())))
                //            {
                //                var v = new Vendorsuppliers();
                //                v.Type = drw1["LABEL_" + inc].ToString();
                //                v.Name = drw1["NAME_" + inc].ToString();
                //                v.Refflag = drw1["FLAG_" + inc].ToString();
                //                v.RefNo = drw1["NO_" + inc].ToString();
                //                //v.s = Convert.ToInt16(drw1["S_" + inc]);
                //                //v.l = Convert.ToInt16(drw1["L_" + inc]);
                //                v.s = 1;
                //                v.l = 1;
                //                inc++;
                //                mdl1.Vendorsuppliers.Add(v);
                //            }
                //        }
                //        else
                //        {
                //            while (dt1.Columns.Contains("NAME_" + inc) && (!string.IsNullOrEmpty(drw1["NAME_" + inc].ToString()) || !string.IsNullOrEmpty(drw1["NO_" + inc].ToString())))
                //            {
                //                mdl1.Vendorsuppliers = new List<Vendorsuppliers>();
                //                var v = new Vendorsuppliers();
                //                v.Type = drw1["LABEL_" + inc].ToString();
                //                v.Name = drw1["NAME_" + inc].ToString();
                //                v.Refflag = drw1["FLAG_" + inc].ToString();
                //                v.RefNo = drw1["NO_" + inc].ToString();
                //                //v.s = Convert.ToInt16(drw1["S_" + inc]);
                //                //v.l = Convert.ToInt16(drw1["L_" + inc]);
                //                v.s = 1;
                //                v.l = 1;
                //                mdl1.Vendorsuppliers.Add(v);
                //                inc++;
                //            }
                //        }
                //    }

                //        //if (mdl1.Vendorsuppliers != null)
                //        //{
                //        //    foreach (var v in mdl1.Vendorsuppliers)
                //        //    {
                //        //        if (!string.IsNullOrEmpty(v.Name))
                //        //        {
                //        //            var q = Query.EQ("Name", v.Name);
                //        //            var m = _VendorRepository.FindOne(q);
                //        //            if (m == null)
                //        //            {
                //        //                return "Itemcode:- " + mdl1.Itemcode + " Please add Vendor in VendorMaster " + v.Name;
                //        //            }
                //        //        }

                //        //    }
                //        //}

                //        //if ((mdl2.Formatted == 0 || mdl2.Formatted == 2))
                //        //{
                //        //    int ven = 0;
                //        //    if (mdl1.Vendorsuppliers != null)
                //        //    {
                //        //        foreach (var v in mdl1.Vendorsuppliers)
                //        //        {
                //        //            if (v.s == 1)
                //        //            {
                //        //                ven = 1;
                //        //            }

                //        //        }
                //        //    }
                //        //    if (ven == 0)
                //        //    {
                //        //        return "Itemcode:- " + mdl1.Itemcode + " Please Check OPM,OEM Vendor";

                //        //    }
                //        //}

                //        _datamasterRepository.Add(mdl1);
                //        cunt++;
                //    }


                //}
                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query2 = Query.EQ("Materialcode", drw1[0].ToString());
                //    var mdl2 = _datamasterRepository.FindOne(query2);
                //    if (mdl2 != null)
                //    {
                //        var erpQry = Query.EQ("Itemcode", mdl2.Itemcode);
                //        var erpMdl = _erpRepository.FindOne(erpQry);
                //        if(erpMdl != null)
                //        {
                //            erpMdl.StorageLocation = drw1[1].ToString();
                //            erpMdl.AvailCheck = drw1[2].ToString();
                //            erpMdl.AvailCheck_ = drw1[3].ToString();
                //            erpMdl.StandardPrice_ = drw1[4].ToString();
                //            erpMdl.StorageBin = drw1[5].ToString();
                //            erpMdl.ReOrderPoint_ = drw1[6].ToString();
                //            erpMdl.AutomaticPO = drw1[7].ToString();
                //            erpMdl.AutomaticPO_ = drw1[8].ToString();
                //            erpMdl.PlannedDeliveryTime_ = drw1[9].ToString();
                //            erpMdl.Inspectiontype_ = drw1[10].ToString();
                //            erpMdl.BatchManagement = drw1[11].ToString();
                //            erpMdl.SafetyStock_ = drw1[12].ToString();
                //            erpMdl.Division_ = drw1[13].ToString();
                //            erpMdl.ReOrderPoint_ = drw1[14].ToString();
                //            erpMdl.MaxStockLevel_ = drw1[15].ToString();
                //            erpMdl.MinStockLevel_ = drw1[16].ToString();
                //            erpMdl.SalesText_ = drw1[17].ToString();
                //            _erpRepository.Add(erpMdl);
                //        }
                //        else
                //        {
                //            erpMdl = new Prosol_ERPInfo();
                //            erpMdl.Itemcode = mdl2.Itemcode;
                //            erpMdl.StorageLocation = drw1[1].ToString();
                //            erpMdl.AvailCheck = drw1[2].ToString();
                //            erpMdl.AvailCheck_ = drw1[3].ToString();
                //            erpMdl.StandardPrice_ = drw1[4].ToString();
                //            erpMdl.StorageBin = drw1[5].ToString();
                //            erpMdl.ReOrderPoint_ = drw1[6].ToString();
                //            erpMdl.AutomaticPO = drw1[7].ToString();
                //            erpMdl.AutomaticPO_ = drw1[8].ToString();
                //            erpMdl.PlannedDeliveryTime_ = drw1[9].ToString();
                //            erpMdl.Inspectiontype_ = drw1[10].ToString();
                //            erpMdl.BatchManagement = drw1[11].ToString();
                //            erpMdl.SafetyStock_ = drw1[12].ToString();
                //            erpMdl.Division_ = drw1[13].ToString();
                //            erpMdl.ReOrderPoint_ = drw1[14].ToString();
                //            erpMdl.MaxStockLevel_ = drw1[15].ToString();
                //            erpMdl.MinStockLevel_ = drw1[16].ToString();
                //            erpMdl.SalesText_ = drw1[17].ToString();
                //            _erpRepository.Add(erpMdl);
                //        }
                //    }
                //    cunt++;
                //}

                // Load all data from repositories once before the loop
                //var matLst = _datamasterRepository.FindAll().ToList();
                //var matCount = matLst.Count();

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var mdl2 = new Prosol_Datamaster();
                //    mdl2.Itemcode = "AD" + (++matCount).ToString("D8");

                //    var erpMdl = new Prosol_ERPInfo();
                //    erpMdl.Itemcode = mdl2.Itemcode;
                //    erpMdl.StorageLocation = drw1[5].ToString();
                //    erpMdl.AvailCheck = drw1[6].ToString();
                //    erpMdl.AvailCheck_ = drw1[7].ToString();
                //    erpMdl.StandardPrice_ = drw1[8].ToString();
                //    erpMdl.StorageBin = drw1[11].ToString();
                //    erpMdl.ReOrderPoint_ = drw1[12].ToString();
                //    erpMdl.AutomaticPO = drw1[15].ToString();
                //    erpMdl.AutomaticPO_ = drw1[16].ToString();
                //    erpMdl.PlannedDeliveryTime_ = drw1[17].ToString();
                //    erpMdl.Inspectiontype_ = drw1[18].ToString();
                //    erpMdl.BatchManagement = drw1[19].ToString();
                //    erpMdl.SafetyStock_ = drw1[20].ToString();
                //    erpMdl.Division_ = drw1[21].ToString();
                //    erpMdl.ReOrderPoint_ = drw1[22].ToString();
                //    erpMdl.MaxStockLevel_ = drw1[23].ToString();
                //    erpMdl.MinStockLevel_ = drw1[24].ToString();
                //    erpMdl.SalesText_ = drw1[25].ToString();
                //    _erpRepository.Add(erpMdl);

                //    mdl2.Materialcode = drw1[0].ToString();
                //    mdl2.Legacy = drw1[1].ToString();
                //    mdl2.exNoun = drw1[2].ToString();
                //    mdl2.exModifier = drw1[3].ToString();
                //    mdl2.Type = drw1[4].ToString();
                //    mdl2.ItemStatus = 0;
                //    mdl2.PVstatus = "Pending";

                //    var PV = new Prosol_UpdatedBy
                //    {
                //        UserId = "9",
                //        Name = "coda",
                //        UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                //    };
                //    mdl2.PVuser = PV;

                //    var vendorSupplierList = mdl2.Vendorsuppliers ?? new List<Vendorsuppliers>();
                //    if (vendorSupplierList.Count == 0)
                //    {
                //        vendorSupplierList.Add(new Vendorsuppliers());
                //    }
                //    vendorSupplierList[0].Type = "MANUFACTURER";
                //    vendorSupplierList[0].Name = drw1[9].ToString();
                //    vendorSupplierList[0].Refflag = "PART NUMBER";
                //    vendorSupplierList[0].RefNo = drw1[10].ToString();
                //    mdl2.Vendorsuppliers = vendorSupplierList;

                //    mdl2.UOM = drw1[13].ToString();
                //    _datamasterRepository.Add(mdl2);

                //    cunt++;
                //}


                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query2 = Query.EQ("Materialcode", drw1[0].ToString());
                //    //var query2 = Query.And(Query.EQ("Noun", drw1[0].ToString()), Query.EQ("Modifier", drw1[1].ToString()), Query.EQ("Characteristic", drw1[2].ToString()));
                //    //var query2 = Query.And(Query.EQ("Noun", drw1[0].ToString()), Query.EQ("Modifier", drw1[1].ToString()), Query.EQ("Characteristic", drw1[2].ToString()), Query.EQ("Definition", "MM"));
                //    var mdl2 = _datamasterRepository.FindOne(query2);
                //    //var mdl2 = _AssetAttributesRepository.FindAll().ToList();
                //    //var mdl2 = _datamasterRepository.FindOne(query2);
                //    if (mdl2 != null)
                //    {
                //        var erpQry = Query.EQ("Itemcode", mdl2.Itemcode);
                //        var erpMdl = _erpRepository.FindOne(erpQry);
                //        if (erpMdl == null)
                //            erpMdl = new Prosol_ERPInfo();
                //        //foreach(var dt in mdl2)
                //        //{
                //        //    var query2 = Query.EQ("UniqueId", dt.UniqueId);
                //        //    var mdl = _AssetAttributesRepository.FindAll().ToList();
                //        //    if(mdl.Count() > 1)
                //        //    {
                //        //        for(var idx=0;idx<= mdl.Count; idx++)
                //        //        {
                //        //            if(idx != 0)
                //        //            {
                //        //                mdl[idx].exNoun = null;
                //        //                _AssetAttributesRepository.Add(mdl[idx]);
                //        //            }
                //        //        }
                //        //    }
                //        //}

                //        erpMdl.Itemcode = mdl2.Itemcode;
                //        erpMdl.StorageLocation = drw1[5].ToString();
                //        erpMdl.AvailCheck = drw1[6].ToString();
                //        erpMdl.AvailCheck_ = drw1[7].ToString();
                //        erpMdl.StandardPrice_ = drw1[8].ToString();
                //        erpMdl.StorageBin = drw1[11].ToString();
                //        erpMdl.ReOrderPoint_ = drw1[12].ToString();
                //        erpMdl.AutomaticPO = drw1[15].ToString();
                //        erpMdl.AutomaticPO_ = drw1[16].ToString();
                //        erpMdl.PlannedDeliveryTime_ = drw1[17].ToString();
                //        erpMdl.Inspectiontype_ = drw1[18].ToString();
                //        erpMdl.BatchManagement = drw1[19].ToString();
                //        erpMdl.SafetyStock_ = drw1[20].ToString();
                //        erpMdl.Division_ = drw1[21].ToString();
                //        erpMdl.ReOrderPoint_ = drw1[22].ToString();
                //        erpMdl.MaxStockLevel_ = drw1[23].ToString();
                //        erpMdl.MinStockLevel_ = drw1[24].ToString();
                //        erpMdl.SalesText_ = drw1[25].ToString();
                //        _erpRepository.Add(erpMdl);
                //        mdl2.Legacy = drw1[1].ToString();
                //        mdl2.exNoun = drw1[2].ToString();
                //        mdl2.exModifier = drw1[3].ToString();
                //        mdl2.Type = drw1[4].ToString();
                //        mdl2.ItemStatus = 0;
                //        mdl2.PVstatus = "Pending";
                //        var PV = new Prosol_UpdatedBy();
                //        PV.UserId = "9";
                //        PV.Name = "coda";
                //        PV.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //        mdl2.PVuser = PV;

                //        if (mdl2.Vendorsuppliers != null)
                //        {
                //            mdl2.Vendorsuppliers[0].Type = "MANUFACTURER";
                //            mdl2.Vendorsuppliers[0].Name = drw1[9].ToString();
                //            mdl2.Vendorsuppliers[0].Refflag = "PART NUMBER";
                //            mdl2.Vendorsuppliers[0].RefNo = drw1[10].ToString();
                //        }
                //        else
                //        {
                //            mdl2.Vendorsuppliers = new List<Vendorsuppliers>();
                //            var ven = new Vendorsuppliers();
                //            if (!string.IsNullOrEmpty(drw1[9].ToString()))
                //            {
                //                ven.Type = "MANUFACTURER";
                //                ven.Name = drw1[9].ToString();
                //            }
                //            if (!string.IsNullOrEmpty(drw1[10].ToString()))
                //            {
                //                ven.Refflag = "PART NUMBER";
                //                ven.Name = drw1[10].ToString();
                //            }
                //            mdl2.Vendorsuppliers.Add(ven);

                //        }
                //        mdl2.UOM = drw1[13].ToString();
                //        _datamasterRepository.Add(mdl2);
                //        //mdl2.Abbrivation = "Yes";
                //        //int gan = 1;
                //        //foreach (var chr in mdl2.OrderBy(x => x.Squence))
                //        //{
                //        //    if (chr.Characteristic == "MATERIAL")
                //        //    {
                //        //        chr.Squence = 2;
                //        //        chr.ShortSquence = 2;
                //        //        if (gan == 2)                                
                //        //            gan = gan - 1;

                //        //    }
                //        //    else
                //        //    {
                //        //        if (gan == 2)
                //        //        {

                //        //            chr.Squence = 3;
                //        //            chr.ShortSquence = 3;
                //        //            gan= 3;
                //        //        }
                //        //        else
                //        //        {
                //        //            chr.Squence = gan;
                //        //            chr.ShortSquence = gan;

                //        //        }
                //        //    }
                //        //    gan++;
                //        //}


                //    }
                //    else
                //    {
                //        mdl2 = new Prosol_Datamaster();
                //        var matLst = _datamasterRepository.FindAll().ToList();
                //        var matCount = matLst.Count() + 1;
                //        mdl2.Itemcode = "AD" + matCount.ToString("D8");
                //        var erpQry = Query.EQ("Itemcode", mdl2.Itemcode);
                //        var erpMdl = _erpRepository.FindOne(erpQry);
                //        if (erpMdl == null)
                //            erpMdl = new Prosol_ERPInfo();
                //        //foreach(var dt in mdl2)
                //        //{
                //        //    var query2 = Query.EQ("UniqueId", dt.UniqueId);
                //        //    var mdl = _AssetAttributesRepository.FindAll().ToList();
                //        //    if(mdl.Count() > 1)
                //        //    {
                //        //        for(var idx=0;idx<= mdl.Count; idx++)
                //        //        {
                //        //            if(idx != 0)
                //        //            {
                //        //                mdl[idx].exNoun = null;
                //        //                _AssetAttributesRepository.Add(mdl[idx]);
                //        //            }
                //        //        }
                //        //    }
                //        //}

                //        erpMdl.Itemcode = mdl2.Itemcode;
                //        erpMdl.StorageLocation = drw1[5].ToString();
                //        erpMdl.AvailCheck = drw1[6].ToString();
                //        erpMdl.AvailCheck_ = drw1[7].ToString();
                //        erpMdl.StandardPrice_ = drw1[8].ToString();
                //        erpMdl.StorageBin = drw1[11].ToString();
                //        erpMdl.ReOrderPoint_ = drw1[12].ToString();
                //        erpMdl.AutomaticPO = drw1[15].ToString();
                //        erpMdl.AutomaticPO_ = drw1[16].ToString();
                //        erpMdl.PlannedDeliveryTime_ = drw1[17].ToString();
                //        erpMdl.Inspectiontype_ = drw1[18].ToString();
                //        erpMdl.BatchManagement = drw1[19].ToString();
                //        erpMdl.SafetyStock_ = drw1[20].ToString();
                //        erpMdl.Division_ = drw1[21].ToString();
                //        erpMdl.ReOrderPoint_ = drw1[22].ToString();
                //        erpMdl.MaxStockLevel_ = drw1[23].ToString();
                //        erpMdl.MinStockLevel_ = drw1[24].ToString();
                //        erpMdl.SalesText_ = drw1[25].ToString();
                //        _erpRepository.Add(erpMdl);
                //        mdl2.Legacy = drw1[1].ToString();
                //        mdl2.exNoun = drw1[2].ToString();
                //        mdl2.exModifier = drw1[3].ToString();
                //        mdl2.Type = drw1[4].ToString();
                //        mdl2.ItemStatus = 0;
                //        mdl2.PVstatus = "Pending";
                //        var PV = new Prosol_UpdatedBy();
                //        PV.UserId = "9";
                //        PV.Name = "coda";
                //        PV.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //        mdl2.PVuser = PV;
                //        if (mdl2.Vendorsuppliers != null)
                //        {
                //            mdl2.Vendorsuppliers[0].Type = "MANUFACTURER";
                //            mdl2.Vendorsuppliers[0].Name = drw1[9].ToString();
                //            mdl2.Vendorsuppliers[0].Refflag = "PART NUMBER";
                //            mdl2.Vendorsuppliers[0].RefNo = drw1[10].ToString();
                //        }
                //        else
                //        {
                //            mdl2.Vendorsuppliers = new List<Vendorsuppliers>();
                //            var ven = new Vendorsuppliers();
                //            if (!string.IsNullOrEmpty(drw1[9].ToString()))
                //            {
                //                ven.Type = "MANUFACTURER";
                //                ven.Name = drw1[9].ToString();
                //            }
                //            if (!string.IsNullOrEmpty(drw1[10].ToString()))
                //            {
                //                ven.Refflag = "PART NUMBER";
                //                ven.Name = drw1[10].ToString();
                //            }
                //            mdl2.Vendorsuppliers.Add(ven);

                //        }
                //        mdl2.UOM = drw1[13].ToString();
                //        _datamasterRepository.Add(mdl2);
                //        //mdl2.Abbrivation = "Yes";
                //        //int gan = 1;
                //        //foreach (var chr in mdl2.OrderBy(x => x.Squence))
                //        //{
                //        //    if (chr.Characteristic == "MATERIAL")
                //        //    {
                //        //        chr.Squence = 2;
                //        //        chr.ShortSquence = 2;
                //        //        if (gan == 2)                                
                //        //            gan = gan - 1;

                //        //    }
                //        //    else
                //        //    {
                //        //        if (gan == 2)
                //        //        {

                //        //            chr.Squence = 3;
                //        //            chr.ShortSquence = 3;
                //        //            gan= 3;
                //        //        }
                //        //        else
                //        //        {
                //        //            chr.Squence = gan;
                //        //            chr.ShortSquence = gan;

                //        //        }
                //        //    }
                //        //    gan++;
                //        //}


                //    }

                //    cunt++;
                //}

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query2 = Query.EQ("UniqueId", drw1[0].ToString());
                //    var mdl = _AssetAttributesRepository.FindAll(query2).ToList();
                //    if(mdl.Count() > 1)
                //    {
                //        //foreach (var data in mdl)
                //        //{
                //        //    DateTime id = data._id.CreationTime;
                //        //}
                //        //var sortedList = mdl.OrderByDescending(data => data._id.CreationTime).ToList();
                //        if(string.IsNullOrEmpty(mdl[0].exNoun) && string.IsNullOrEmpty(mdl[0].exModifier))
                //        {
                //            mdl[0].exNoun = !string.IsNullOrEmpty(mdl[mdl.Count() - 1].exNoun) ? mdl[mdl.Count() - 1].exNoun : "";
                //            mdl[0].exModifier = !string.IsNullOrEmpty(mdl[mdl.Count() - 1].exModifier) ? mdl[mdl.Count() - 1].exModifier : "";
                //            mdl[0].exCharacterisitics = mdl[mdl.Count() - 1].exCharacterisitics != null ? mdl[mdl.Count() - 1].exCharacterisitics : mdl[0].exCharacterisitics;
                //        }
                //        var dumbMdl = mdl[0];
                //        _AssetAttributesRepository.DeleteAll(query2);
                //        _AssetAttributesRepository.Add(dumbMdl);
                //        cunt++;
                //    }
                //}

                //Fetch Images by Foldername

                //string basePath = HttpContext.Current.Server.MapPath("~/AssetImages");

                ////Dictionary<string, List<string>> folderImages = new Dictionary<string, List<string>>();
                //var folderImages = new List<ImgLst>();
                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    string NP2Flg = drw1["NP2"].ToString();
                //    string NPFlg = drw1["NP1"].ToString();
                //    string AssetFlg = drw1["ASSET"].ToString();
                //    string MaximoFlg = drw1["MAXIMO"].ToString();
                //    string subFolderName = drw1["SUB FOLDER NAME"].ToString();
                //    string folderName = drw1["FOLDER NAME"].ToString();
                //    string uniqueId = drw1["UNIQUE ID"].ToString();
                //    string assetNo = drw1["ASSET NUMBER"].ToString();
                //    string folderPath = Path.Combine(basePath, folderName, subFolderName);
                //    string apiUrl = "https://d30f70hjt97rz5.cloudfront.net/";

                //    int assetImg = 1, maxImg = 1, npImg1 = 1, npImg2 = 1;
                //    List<string> images = new List<string>();
                //    var assetData = _assetmasterRepository.FindOne(Query.EQ("UniqueId", uniqueId));
                //    assetNo = assetData.AssetNo;
                //    if (Directory.Exists(folderPath))
                //    {
                //        var files = Directory.GetFiles(folderPath, "*.*")
                //            .Where(f => new[] { ".jpg", ".png", ".jpeg", ".gif" }
                //                .Any(ext => f.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                //            .ToList();

                //        if (!string.IsNullOrEmpty(MaximoFlg))
                //        {
                //            assetData.AssetImages.AssetImgs = (assetData.AssetImages.AssetImgs ?? new string[0])
                //                .Concat(assetData.AssetImages.MatImgs ?? new string[0]).ToArray();
                //            assetData.AssetImages.MatImgs = new string[0];
                //        }
                //        if (!string.IsNullOrEmpty(NPFlg))
                //        {
                //            assetData.AssetImages.NamePlateImgeTwo = (assetData.AssetImages.NamePlateImgeTwo ?? new string[0])
                //            .Concat(assetData.AssetImages.NamePlateImge ?? new string[0]).ToArray();
                //            assetData.AssetImages.NamePlateImge = new string[0];
                //        }

                //        var uploadTasks = new List<Task>();

                //        foreach (var img in files)
                //        {
                //            if (img.Contains("Asset Image"))
                //            {
                //                if (!string.IsNullOrEmpty(AssetFlg))
                //                {
                //                    string imgName = $"{assetNo}-{assetImg++}.jpeg";
                //                    UploadImage(img, imgName, newImg =>
                //                        assetData.AssetImages.AssetImgs = (assetData.AssetImages.AssetImgs ?? new string[0])
                //                            .Concat(newImg).ToArray());
                //                }
                //            }
                //            else if (img.Contains("Maximo Image"))
                //            {
                //                if (!string.IsNullOrEmpty(MaximoFlg))
                //                {
                //                    string imgName = $"{assetNo}MAXI-{maxImg++}.jpeg";
                //                    UploadImage(img, imgName, newImg =>
                //                        assetData.AssetImages.MatImgs = (assetData.AssetImages.MatImgs ?? new string[0])
                //                            .Concat(newImg).ToArray());
                //                }
                //            }
                //            else if (img.Contains("Nameplate 1"))
                //            {
                //                if (!string.IsNullOrEmpty(NPFlg))
                //                {
                //                    string imgName = $"{assetNo}NP1-{npImg1++}.jpeg";
                //                    UploadImage(img, imgName, newImg =>
                //                        assetData.AssetImages.NamePlateImge = (assetData.AssetImages.NamePlateImge ?? new string[0])
                //                            .Concat(newImg).ToArray());
                //                }
                //            }
                //            else if (img.Contains("Nameplate 2"))
                //            {
                //                if (!string.IsNullOrEmpty(NP2Flg))
                //                {
                //                    string imgName = $"{assetNo}NP2-{npImg2++}.jpeg";
                //                    UploadImage(img, imgName, newImg =>
                //                        assetData.AssetImages.NamePlateImgeTwo = (assetData.AssetImages.NamePlateImgeTwo ?? new string[0])
                //                            .Concat(newImg).ToArray());
                //                }
                //            }
                //        }


                //        Task.WaitAll(uploadTasks.ToArray());

                //        _assetmasterRepository.Add(assetData);

                //        images.AddRange(files);
                //    }
                //    else
                //    {
                //        images.Add("Folder Not Found");
                //    }

                //    var existing = folderImages.FirstOrDefault(x => x.UniqueId == uniqueId);
                //    if (existing != null)
                //    {
                //        existing.Images = images.ToArray();
                //    }
                //    else
                //    {
                //        folderImages.Add(new ImgLst
                //        {
                //            UniqueId = uniqueId,
                //            AssetNo = assetNo,
                //            Images = images.ToArray()
                //        });
                //    }
                //}


                //string basePath = HttpContext.Current.Server.MapPath("~/AssetImages");

                ////Dictionary<string, List<string>> folderImages = new Dictionary<string, List<string>>();
                //var folderImages = new List<ImgLst>();
                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    //string NP2Flg = drw1["NP2"].ToString();
                //    string NPFlg = drw1["NP1"].ToString();
                //    //string AssetFlg = drw1["ASSET"].ToString();
                //    //string MaximoFlg = drw1["MAXIMO"].ToString();
                //    //string subFolderName = drw1["SUB FOLDER NAME"].ToString();
                //    string folderName = drw1["FOLDER NAME"].ToString();
                //    string uniqueId = drw1["UNIQUE ID"].ToString();
                //    string assetNo = drw1["ASSET NUMBER"].ToString();
                //    string folderPath = Path.Combine(basePath, folderName);
                //    string apiUrl = "https://d30f70hjt97rz5.cloudfront.net/";

                //    int assetImg = 1, maxImg = 1, npImg1 = 1, npImg2 = 1;
                //    List<string> images = new List<string>();
                //    var assetData = _assetmasterRepository.FindOne(Query.EQ("UniqueId", uniqueId));
                //    assetNo = assetData.AssetNo;
                //    if (Directory.Exists(folderPath))
                //    {
                //        var files = Directory.GetFiles(folderPath, "*.*")
                //            .Where(f => new[] { ".jpg", ".png", ".jpeg", ".gif" }
                //                .Any(ext => f.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                //            .ToList();

                //        if (!string.IsNullOrEmpty(folderName) && folderName == "Replace"
                //            && assetData.AssetImages.NamePlateImge != null && assetData.AssetImages.NamePlateImge.Count() > 0)
                //        {
                //            int npCount = assetData.AssetImages.NamePlateImge.Count();
                //            var list = assetData.AssetImages.NamePlateImge;
                //            assetData.AssetImages.NamePlateImge = null;
                //            for (int i = 0; i < npCount-1; i++)
                //            {
                //                assetData.AssetImages.NamePlateImge[i] = list[i];
                //            }

                //        }


                //        var uploadTasks = new List<Task>();

                //        foreach (var img in files)
                //        {
                //            //if (img.Contains("Asset Image"))
                //            //{
                //            //    if (!string.IsNullOrEmpty(AssetFlg))
                //            //    {
                //            //        string imgName = $"{assetNo}-{assetImg++}.jpeg";
                //            //        UploadImage(img, imgName, newImg =>
                //            //            assetData.AssetImages.AssetImgs = (assetData.AssetImages.AssetImgs ?? new string[0])
                //            //                .Concat(newImg).ToArray());
                //            //    }
                //            //}
                //            //else if (img.Contains("Maximo Image"))
                //            //{
                //            //    if (!string.IsNullOrEmpty(MaximoFlg))
                //            //    {
                //            //        string imgName = $"{assetNo}MAXI-{maxImg++}.jpeg";
                //            //        UploadImage(img, imgName, newImg =>
                //            //            assetData.AssetImages.MatImgs = (assetData.AssetImages.MatImgs ?? new string[0])
                //            //                .Concat(newImg).ToArray());
                //            //    }
                //            //}
                //            if (img.Contains("Nameplate 1"))
                //            {
                //                if (!string.IsNullOrEmpty(NPFlg))
                //                {
                //                    string imgName = $"{assetNo}NP1-{npImg1++}_New.jpeg";
                //                    UploadImage(img, imgName, newImg =>
                //                        assetData.AssetImages.NamePlateImge = (assetData.AssetImages.NamePlateImge ?? new string[0])
                //                            .Concat(newImg).ToArray());
                //                }
                //            }
                //            //else if (img.Contains("Nameplate 2"))
                //            //{
                //            //    if (!string.IsNullOrEmpty(NP2Flg))
                //            //    {
                //            //        string imgName = $"{assetNo}NP2-{npImg2++}.jpeg";
                //            //        UploadImage(img, imgName, newImg =>
                //            //            assetData.AssetImages.NamePlateImgeTwo = (assetData.AssetImages.NamePlateImgeTwo ?? new string[0])
                //            //                .Concat(newImg).ToArray());
                //            //    }
                //            //}
                //        }


                //        Task.WaitAll(uploadTasks.ToArray());

                //        _assetmasterRepository.Add(assetData);

                //        images.AddRange(files);
                //    }
                //    else
                //    {
                //        images.Add("Folder Not Found");
                //    }

                //    var existing = folderImages.FirstOrDefault(x => x.UniqueId == uniqueId);
                //    if (existing != null)
                //    {
                //        existing.Images = images.ToArray();
                //    }
                //    else
                //    {
                //        folderImages.Add(new ImgLst
                //        {
                //            UniqueId = uniqueId,
                //            AssetNo = assetNo,
                //            Images = images.ToArray()
                //        });
                //    }
                //}

                //Single Attribute

                //foreach (DataRow drw1 in dt1.Rows)
                //{
                //    var query1 = Query.EQ("UniqueId", drw1[0].ToString());
                //    var mdl = _AssetAttributesRepository.FindOne(query1);
                //    if (mdl != null)
                //    {
                //        int idx = mdl.Characterisitics.FindIndex(i => i.Characteristic == drw1[1].ToString());
                //        if(idx != -1)
                //            mdl.Characterisitics[idx].Value = drw1[2].ToString();
                //        _AssetAttributesRepository.Add(mdl);
                //        cunt++;
                //    }
                //}

                //Existing Attributes Upload

                //foreach (DataRow drw in dt1.Rows)
                //{
                //    var quryy = Query.EQ("UniqueId", drw["EquipmentUniqueID"].ToString());
                //    var AttriMdl1 = _AssetAttributesRepository.FindOne(quryy);
                //    if (AttriMdl1 == null)
                //        AttriMdl1 = new Prosol_AssetAttributes();
                //    AttriMdl1.UniqueId = drw["EquipmentUniqueID"].ToString();
                //    AttriMdl1.exNoun = drw["Noun"].ToString();
                //    AttriMdl1.exModifier = drw["Modifier"].ToString();
                //    var attrList = new List<Asset_AttributeList>();
                //    attrList = (from DataRow drw1 in dt1.Rows
                //                where drw1["EquipmentUniqueID"].ToString() == drw[0].ToString()
                //                select new Asset_AttributeList()
                //                {
                //                    Characteristic = drw1["Attribute"] != DBNull.Value ? drw1["Attribute"].ToString() : "",
                //                    Value = drw1["Value"] != DBNull.Value ? drw1["Value"].ToString() : "",
                //                    UOM = "",
                //                    //  Mandatory = drw1["Mandatory"] != DBNull.Value ? drw1["Mandatory"].ToString() : "",
                //                    Squence = (short)0,
                //                    ShortSquence = (short)0
                //                }).ToList();
                //    AttriMdl1.exCharacterisitics = null;
                //    AttriMdl1.exCharacterisitics = attrList;
                //    _AssetAttributesRepository.Add(AttriMdl1);
                //    cunt++;
                //}

                //foreach (DataRow drw in dt1.Rows)
                //{
                //    var Qry = Query.And(Query.EQ("Noun",drw[0].ToString()),Query.EQ("Modifier",drw[1].ToString()),Query.EQ("Characteristic",drw[2].ToString()),Query.EQ("Definition" , "Equ"));
                //    var CharMdl = _CharacteristicRepository.FindOne(Qry);
                //    if(CharMdl != null)
                //    {
                //        var ValueQry = Query.EQ("Value", drw[3].ToString());
                //        var ValueMdl = _AbbrevateRepository.FindOne(ValueQry);
                //        if(ValueMdl != null)
                //        {
                //            var Values = CharMdl.Values == null || CharMdl.Values.Length == 0 ? new List<string>() : CharMdl.Values.ToList();
                //            Values.Add(ValueMdl._id.ToString());
                //            CharMdl.Values = Values.ToArray();
                //        }
                //        else
                //        {
                //            ValueMdl = new Prosol_Abbrevate();
                //            ValueMdl.Value = drw[3].ToString();
                //            ValueMdl.Abbrevated = drw[3].ToString();
                //            ValueMdl.User = "KHAMIZ";
                //            ValueMdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //            bool ValuesRes = _AbbrevateRepository.Add(ValueMdl);
                //            if (ValuesRes)
                //            {
                //                var Values = CharMdl.Values == null || CharMdl.Values.Length == 0 ? new List<string>() : CharMdl.Values.ToList();
                //                Values.Add(drw[3].ToString());
                //                CharMdl.Values = Values.ToArray();
                //            }
                //        }
                //        _CharacteristicRepository.Add(CharMdl);
                //    }
                //}

                //foreach (DataRow drw in dt1.Rows)
                //{
                //    if (!string.IsNullOrEmpty(drw[0].ToString()))
                //    {
                //        var Qry = Query.EQ("Itemcode", drw[0].ToString());
                //        var obj = _datamasterRepository.FindOne(Qry);
                //        if (obj == null)
                //        {
                //            obj = new Prosol_Datamaster();
                //            obj.Itemcode = drw[0].ToString();
                //            obj.Materialcode = drw[1].ToString();
                //            obj.exMaterialcode = drw[2].ToString();
                //            obj.UOM = drw[6].ToString();
                //            obj.Legacy = drw[3].ToString();
                //            obj.Legacy2 = drw[14].ToString();
                //            var erp = new Prosol_ERPInfo();
                //            erp.Itemcode = drw[0].ToString();
                //            erp.Materialtype = drw[4].ToString();
                //            erp.Materialtype_ = drw[5].ToString();
                //            erp.MaterialStrategicGroup = drw[7].ToString();
                //            erp.MaterialStrategicGroup_ = drw[8].ToString();
                //            erp.MRPType = drw[9].ToString();
                //            erp.MRPType_ = drw[10].ToString();
                //            erp.ReOrderPoint_ = drw[11].ToString();
                //            erp.SafetyStock_ = drw[12].ToString();
                //            erp.MaxStockLevel_ = drw[13].ToString();
                //            erp.StorageLocation = drw[16].ToString();
                //            erp.StorageLocation_ = drw[17].ToString();
                //            erp.StorageLocation2 = drw[18].ToString();
                //            erp.StorageLocation2_ = drw[19].ToString();
                //            erp.StorageLocation3 = drw[20].ToString();
                //            erp.StorageLocation3_ = drw[21].ToString();
                //            erp.StorageLocation4 = drw[22].ToString();
                //            erp.StorageLocation4_ = drw[23].ToString();
                //            erp.StorageLocation5 = drw[24].ToString();
                //            erp.StorageLocation5_ = drw[25].ToString();
                //            erp.StorageBin = drw[26].ToString();
                //            erp.StorageBin2 = drw[27].ToString();
                //            erp.StorageBin3 = drw[28].ToString();
                //            erp.StorageBin4 = drw[29].ToString();
                //            erp.StorageBin5 = drw[30].ToString();
                //            erp.Quantity_ = drw[31].ToString();
                //            erp.Quantity2_ = drw[32].ToString();
                //            erp.Quantity3_ = drw[33].ToString();
                //            erp.Quantity4_ = drw[34].ToString();
                //            erp.Quantity5_ = drw[35].ToString();
                //            erp.Price_Unit = drw[36].ToString();
                //            erp.Price_Unit2 = drw[37].ToString();
                //            erp.Price_Unit3 = drw[38].ToString();
                //            erp.Price_Unit4 = drw[39].ToString();
                //            erp.Price_Unit5 = drw[40].ToString();
                //            erp.Currency = drw[41].ToString();
                //            erp.Currency2 = drw[42].ToString();
                //            erp.Currency3 = drw[43].ToString();
                //            erp.Currency4 = drw[44].ToString();
                //            erp.Currency5 = drw[45].ToString();
                //            obj.StorageLocations = new List<StorageLoc>();

                //            for (int i = 0; i < 5; i++)
                //            {
                //                string locProp = i == 0 ? "StorageLocation" : $"StorageLocation{i + 1}";
                //                string locProp_ = i == 0 ? "StorageLocation_" : $"StorageLocation{i + 1}_";
                //                string binProp = i == 0 ? "StorageBin" : $"StorageBin{i + 1}";
                //                string qtyProp = i == 0 ? "Quantity_" : $"Quantity{i + 1}_";

                //                string location = erp.GetType().GetProperty(locProp)?.GetValue(erp)?.ToString();
                //                string location_ = erp.GetType().GetProperty(locProp_)?.GetValue(erp)?.ToString();
                //                string bin = erp.GetType().GetProperty(binProp)?.GetValue(erp)?.ToString();
                //                string qty = erp.GetType().GetProperty(qtyProp)?.GetValue(erp)?.ToString();

                //                if (!string.IsNullOrEmpty(location))
                //                {
                //                    var slObj = new StorageLoc
                //                    {
                //                        StorageLocation = location,
                //                        StorageLocation_ = location_,
                //                        DataCollection = new List<StorageBin>()
                //                    };

                //                    if (!string.IsNullOrEmpty(bin))
                //                    {
                //                        var sbObj = new StorageBin
                //                        {
                //                            Observation = bin,
                //                            sQty = !string.IsNullOrEmpty(qty) ? qty : "",
                //                            dQty = ""
                //                        };
                //                        slObj.DataCollection.Add(sbObj);
                //                    }

                //                    obj.StorageLocations.Add(slObj);
                //                }
                //            }

                //            _datamasterRepository.Add(obj);
                //            _erpRepository.Add(erp);
                //        }
                //    }
                //    //if (!string.IsNullOrEmpty(drw[1].ToString()))
                //    //{
                //    //    var Qry = Query.EQ("Materialcode", drw[1].ToString());
                //    //    var obj = _datamasterRepository.FindOne(Qry);
                //    //    if (obj != null)
                //    //    {
                //    //        var erpQry = Query.EQ("Itemcode", obj.Itemcode);
                //    //        var erp = _erpRepository.FindOne(erpQry);
                //    //        if (erp != null)
                //    //        {
                //    //            erp.StorageLocation = drw[16].ToString();
                //    //            erp.StorageLocation_ = drw[17].ToString();
                //    //            erp.StorageLocation2 = drw[18].ToString();
                //    //            erp.StorageLocation2_ = drw[19].ToString();
                //    //            erp.StorageLocation3 = drw[20].ToString();
                //    //            erp.StorageLocation3_ = drw[21].ToString();
                //    //            erp.StorageLocation4 = drw[22].ToString();
                //    //            erp.StorageLocation4_ = drw[23].ToString();
                //    //            erp.StorageLocation5 = drw[24].ToString();
                //    //            erp.StorageLocation5_ = drw[25].ToString();
                //    //            erp.StorageBin = drw[26].ToString();
                //    //            erp.StorageBin2 = drw[27].ToString();
                //    //            erp.StorageBin3 = drw[28].ToString();
                //    //            erp.StorageBin4 = drw[29].ToString();
                //    //            erp.StorageBin5 = drw[30].ToString();
                //    //            erp.Quantity_ = drw[31].ToString();
                //    //            erp.Quantity2_ = drw[32].ToString();
                //    //            erp.Quantity3_ = drw[33].ToString();
                //    //            erp.Quantity4_ = drw[34].ToString();
                //    //            erp.Quantity5_ = drw[35].ToString();
                //    //            erp.Price_Unit = drw[36].ToString();
                //    //            erp.Price_Unit2 = drw[37].ToString();
                //    //            erp.Price_Unit3 = drw[38].ToString();
                //    //            erp.Price_Unit4 = drw[39].ToString();
                //    //            erp.Price_Unit5 = drw[40].ToString();
                //    //            erp.Currency = drw[41].ToString();
                //    //            erp.Currency2 = drw[42].ToString();
                //    //            erp.Currency3 = drw[43].ToString();
                //    //            erp.Currency4 = drw[44].ToString();
                //    //            erp.Currency5 = drw[45].ToString();
                //    //            obj.StorageLocations = new List<StorageLoc>();

                //    //            for (int i = 0; i < 5; i++)
                //    //            {
                //    //                string locProp = i == 0 ? "StorageLocation" : $"StorageLocation{i + 1}";
                //    //                string locProp_ = i == 0 ? "StorageLocation_" : $"StorageLocation{i + 1}_";
                //    //                string binProp = i == 0 ? "StorageBin" : $"StorageBin{i + 1}";
                //    //                string qtyProp = i == 0 ? "Quantity_" : $"Quantity{i + 1}_";

                //    //                string location = erp.GetType().GetProperty(locProp)?.GetValue(erp)?.ToString();
                //    //                string location_ = erp.GetType().GetProperty(locProp_)?.GetValue(erp)?.ToString();
                //    //                string bin = erp.GetType().GetProperty(binProp)?.GetValue(erp)?.ToString();
                //    //                string qty = erp.GetType().GetProperty(qtyProp)?.GetValue(erp)?.ToString();

                //    //                if (!string.IsNullOrEmpty(location))
                //    //                {
                //    //                    var slObj = new StorageLoc
                //    //                    {
                //    //                        StorageLocation = location,
                //    //                        StorageLocation_ = location_,
                //    //                        DataCollection = new List<StorageBin>()
                //    //                    };

                //    //                    if (!string.IsNullOrEmpty(bin))
                //    //                    {
                //    //                        var sbObj = new StorageBin
                //    //                        {
                //    //                            Observation = bin,
                //    //                            sQty = !string.IsNullOrEmpty(qty) ? qty : "",
                //    //                            dQty = ""
                //    //                        };
                //    //                        slObj.DataCollection.Add(sbObj);
                //    //                    }

                //    //                    obj.StorageLocations.Add(slObj);
                //    //                }
                //    //            }

                //    //            _datamasterRepository.Add(obj);
                //    //            _erpRepository.Add(erp);
                //    //        }
                //    //    }
                //    //}
                //    cunt++;
                //}
                //foreach (DataRow drw in dt1.Rows)
                //{
                //    var Qry = Query.EQ("Materialcode", drw[1].ToString());
                //    var obj = _datamasterRepository.FindOne(Qry);
                //    if (obj != null)
                //    {
                //        if (obj.StorageLocations != null && obj.StorageLocations.Count()>0)
                //        {
                //            StorageLoc slData = (StorageLoc)obj.StorageLocations.Where(l => l.StorageLocation == drw[8].ToString());
                //            if(slData != null)
                //            {
                //                if (slData.DataCollection != null && obj.DataCollection.Count() > 0)
                //                {
                //                    StorageBin sbData = (StorageBin)obj.DataCollection.Where(l => l.Observation == drw[10].ToString());
                //                    if (sbData != null)
                //                    {
                //                        sbData.dQty = drw[11].ToString();
                //                    }
                //                    else
                //                    {
                //                        slData.DataCollection = new List<StorageBin>();
                //                        slData.DataCollection[0].Observation = drw[10].ToString();
                //                        slData.DataCollection[0].dQty = drw[11].ToString();
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                var slObj = new StorageLoc();
                //                slObj.StorageLocation = drw[8].ToString();
                //                slObj.StorageLocation_ = drw[9].ToString();
                //                slObj.DataCollection = new List<StorageBin>();
                //                slObj.DataCollection[0].Observation = drw[10].ToString();
                //                slObj.DataCollection[0].dQty = drw[11].ToString();
                //            }
                //        }
                //        _datamasterRepository.Add(obj);
                //    }
                //}

                //foreach (DataRow drw in dt1.Rows)
                //{
                //    var materialCode = drw[1].ToString();
                //    var storageLocation = drw[8].ToString();
                //    var storageLocationName = drw[9].ToString();
                //    var observation = drw[10].ToString();
                //    var qty = drw[11].ToString();

                //    var query = Query.EQ("Materialcode", materialCode);
                //    var obj = _datamasterRepository.FindOne(query);

                //    if (obj != null)
                //    {
                //        if (obj.StorageLocations == null)
                //            obj.StorageLocations = new List<StorageLoc>();

                //        // Find existing storage location
                //        var slData = obj.StorageLocations.FirstOrDefault(l => l.StorageLocation == storageLocation);

                //        if (slData != null)
                //        {
                //            if (slData.DataCollection == null)
                //                slData.DataCollection = new List<StorageBin>();

                //            // Find existing observation
                //            var sbData = slData.DataCollection.FirstOrDefault(l => l.Observation == observation);

                //            if (sbData != null)
                //            {
                //                // Update existing
                //                sbData.dQty = qty;
                //            }
                //            else
                //            {
                //                // Add new
                //                slData.DataCollection.Add(new StorageBin
                //                {
                //                    Observation = observation,
                //                    dQty = qty
                //                });
                //            }
                //        }
                //        else
                //        {
                //            // Create new storage location with a new data collection entry
                //            var slObj = new StorageLoc
                //            {
                //                StorageLocation = storageLocation,
                //                StorageLocation_ = storageLocationName,
                //                DataCollection = new List<StorageBin>
                //{
                //    new StorageBin
                //    {
                //        Observation = observation,
                //        dQty = qty
                //    }
                //}
                //            };

                //            obj.StorageLocations.Add(slObj);
                //        }

                //        // Save back (instead of Add, use Save/Update)
                //        _datamasterRepository.Add(obj);
                //    }
                //}
                //cunt++;
                foreach (DataRow drw in dt1.Rows)
                {
                    var materialCode = drw[0].ToString();
                    var uom = drw[1].ToString();
                    var storageLocation = drw[2].ToString();
                    var storageLocationName = drw[3].ToString();
                    var observation = drw[4].ToString();
                    var qty = drw[5].ToString();
                    //var qtyD = drw[12].ToString();
                    var qtyD = "";

                    var query = Query.EQ("Materialcode", materialCode);
                    var obj = _datamasterRepository.FindOne(query);

                    if (obj != null)
                    {
                        obj.UOM = drw[1].ToString();
                        //obj.Stock_Status = "Stock";

                        obj.StorageLocations = new List<StorageLoc>
                        {
                            new StorageLoc
                            {
                                StorageLocation = storageLocation,
                                StorageLocation_ = storageLocationName,
                                DataCollection = new List<StorageBin>
                                {
                                    new StorageBin
                                    {
                                        Observation = observation,
                                        sQty = qty,
                                        dQty = qtyD
                                    }
                                }
                            }
                        };

                        var erpQuery = Query.EQ("Itemcode", obj.Itemcode);
                        var erp = _erpRepository.FindOne(erpQuery);
                        if (erp != null)
                        {
                            erp.StorageLocation = storageLocation;
                            erp.StorageLocation_ = storageLocationName;
                            erp.StorageBin = observation;
                            erp.Quantity_ = qty;
                            //erp.Materialtype = drw[6].ToString();
                            //erp.MaterialStrategicGroup = drw[7].ToString();

                            _erpRepository.Add(erp);
                        }

                        _datamasterRepository.Add(obj);
                        cunt++;
                    }
                }

                //foreach (DataRow drw in dt1.Rows)
                //{
                //    var attributeName = drw[0].ToString();
                //    var unitName = drw[1].ToString();

                //    // Get attribute
                //    var abbQuery = Query.EQ("Attribute", attributeName);
                //    var abbLst = _AttributesRepository.FindOne(abbQuery);
                //    if (abbLst == null)
                //        abbLst = new Prosol_Attribute { Attribute = attributeName };

                //    if (abbLst.UOMList == null)
                //        abbLst.UOMList = new string[0];

                //    // Get or create UOM
                //    var uomQuery = Query.EQ("Unitname", unitName);
                //    var uomLst = _UOMRepository.FindOne(uomQuery);

                //    if (uomLst == null)
                //    {
                //        uomLst = new Prosol_UOM
                //        {
                //            Unitname = unitName
                //        };
                //        _UOMRepository.Add(uomLst);
                //    }

                //    // Append new UOM id uniquely
                //    abbLst.UOMList = (abbLst.UOMList ?? new string[0])
                //                     .Concat(new[] { uomLst._id.ToString() })
                //                     .Distinct()
                //                     .ToArray();

                //    // Save attribute (use Update/Upsert, not Add if it already exists)
                //    _AttributesRepository.Add(abbLst);

                //    cunt++;
                //}

                //// Now update all characteristics
                //foreach (DataRow drw in dt1.Rows)
                //{
                //    var attributeName = drw[0].ToString();

                //    var abbQuery = Query.EQ("Attribute", attributeName);
                //    var abbLst = _AttributesRepository.FindOne(abbQuery);

                //    if (abbLst == null)
                //        continue; // skip if attribute not found

                //    var chQuery = Query.EQ("Characteristic", attributeName);
                //    var chLst = _CharacteristicRepository.FindAll(chQuery);

                //    foreach (var ch in chLst)
                //    {
                //        ch.Uom = abbLst.UOMList;

                //        // Save updated characteristic
                //        _CharacteristicRepository.Add(ch);
                //    }
                //}

                //foreach (DataRow drw in dt1.Rows)
                //{
                //    var chQuery = Query.And(Query.EQ("Definition", "MM"), Query.NE("Uom", BsonNull.Value));
                //    var chLst = _CharacteristicRepository.FindAll(chQuery).ToList();
                //    var uniqueChLst = chLst
                //                        .GroupBy(x => x.Characteristic)
                //                        .Select(g => g.First())
                //                        .ToList();


                //    foreach (var ch in chLst)
                //    {
                //        var qry = Query.And(Query.EQ("Characteristic", ch.Characteristic), Query.EQ("Definition", "Equ"));
                //        var chaLst = _CharacteristicRepository.FindAll(qry).ToList();
                //        if (ch != null && ch.Uom != null && ch.Uom.Length > 0)
                //        {
                //            if (chaLst != null && chaLst.Count() > 0)
                //            {
                //                foreach (var cha in chaLst) {
                //                    cha.Uom = ch.Uom ?? new string[0];

                //                    // Save updated characteristic
                //                    _CharacteristicRepository.Add(cha);
                //                }
                //            }
                //        }
                //    }
                //}

            }
            return cunt + " Items assigned successfully";

        }

        private void UploadImage(string filePath, string imgName, Action<string[]> assign)
        {
            string apiUrl = "https://d30f70hjt97rz5.cloudfront.net/";
            if (!File.Exists(filePath))
                return;

            try
            {
                byte[] data = File.ReadAllBytes(filePath); // Synchronous file read

                string fullImgUrl = apiUrl + imgName;
                assign(new[] { fullImgUrl });
                string PostURL = "https://w6z8s5dwce.execute-api.me-south-1.amazonaws.com/dev/upload";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(PostURL);
                webRequest.Method = "POST";
                webRequest.Timeout = 300000; // 5 minutes
                webRequest.ReadWriteTimeout = 300000; // 5 minutes
                webRequest.Headers.Add("file-name", imgName);
                webRequest.Headers.Add("x-api-key", "Pw3IgcUkno9xGTf3VtUBn2Gmi9rKA3Sb7Fh8liIH");
                webRequest.ContentType = "image/jpeg";
                webRequest.ContentLength = data.Length;

                using (Stream postStream = webRequest.GetRequestStream())
                {
                    postStream.Write(data, 0, data.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                {
                    // Optional: Check status or log response
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        // Handle non-OK status
                        Console.WriteLine($"Upload failed: {response.StatusCode}");
                    }
                }
            }
            catch (WebException ex)
            {
                // Log and handle timeout or network error
                Console.WriteLine("Upload failed: " + ex.Message);
                //File.AppendAllText("upload_errors.log", $"{DateTime.Now}: {imgName} - {ex.Message}{Environment.NewLine}");
            }
        }


        //public virtual int BulkData(HttpPostedFileBase file)
        //{
        //    int cunt = 0;

        //    Stream stream = file.InputStream;
        //    IExcelDataReader reader = null;
        //    if (file.FileName.EndsWith(".xls"))
        //    {
        //        reader = ExcelReaderFactory.CreateBinaryReader(stream);
        //    }
        //    else if (file.FileName.EndsWith(".xlsx"))
        //    {
        //        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //    }
        //    reader.IsFirstRowAsColumnNames = true;
        //    var res = reader.AsDataSet();
        //    reader.Close();
        //    DataTable dt = res.Tables[0];

        //    foreach (DataRow drw in dt.Rows)

        //    {
        //        if (drw[0].ToString() != "")
        //        {

        //            var obj = new Prosol_EquipmentClass();
        //            var qry = Query.EQ("EquipmentClass", drw[0].ToString());

        //            var resul = _EquipClassRepository.FindOne(qry);
        //            var typ = new Prosol_EquipmentType();
        //            typ.EquClass_Id = resul._id.ToString();
        //            typ.EquipmentType = drw[1].ToString();
        //            typ.EquTypeCode = drw[1].ToString();
        //            typ.IsActive = true;
        //            _EquipRepository.Add(typ);


        //        }
        //    }
        //    return cunt;

        //}
        public List<Prosol_Datamaster> checkDuplicate(Prosol_Datamaster cat)
        {
           // cat.Shortdesc = ShortDesc(cat);
            // cat.Longdesc = LongDesc(cat);
            // cat.Partnodup = cat.Partno != null ? Regex.Replace(cat.Partno, @"[^\w\d]", "") : "";
            int flg = 0;
            var dupList = new List<Prosol_Datamaster>();
            if (cat.Vendorsuppliers != null)
            {
                foreach (Vendorsuppliers vsup in cat.Vendorsuppliers)
                {
                    if (vsup.Refflag != null && vsup.Refflag != ""  && vsup.Refflag != "DRAWING NUMBER" && vsup.Refflag != "POSITION NUMBER")
                    {

                        if (vsup.RefNo != null && vsup.RefNo != "" && vsup.Refflag != null && vsup.RefNoDup != null && vsup.Refflag != "")
                        {
                            flg = 1;
                            //  var query = Query.And(Query.EQ("Vendorsuppliers.Refflag", vsup.Refflag), Query.EQ("Vendorsuppliers.RefNo", vsup.RefNo));
                            // var query = Query.And(Query.ElemMatch("Vendorsuppliers", Query.And(Query.EQ("Refflag", vsup.Refflag), Query.EQ("RefNo", vsup.RefNo))));
                                                 

                           // var query = Query.Matches("Vendorsuppliers.RefNo", BsonRegularExpression.Create(new Regex(stringFormat("^{0}", temp), RegexOptions.IgnoreCase)));
                            var query = Query.EQ("Vendorsuppliers.RefNoDup", vsup.RefNoDup);
                            var vn = _datamasterRepository.FindAll(query).ToList();
                            if (vn != null && vn.Count > 1)
                            {
                                foreach (Prosol_Datamaster dm in vn)
                                {
                                    if (-1 == dupList.FindIndex(f => f.Itemcode.Equals(dm.Itemcode)))
                                    {
                                        dupList.Add(dm);
                                        dm.Referenceno = "PN DUP";
                                        dm.Duplicates = cat.Itemcode;
                                        _datamasterRepository.Add(dm);

                                    }


                                }

                            }
                        }
                    }
                }
            }
            if (flg == 0 && cat.Shortdesc != null && cat.Noun != null)
            { 
                var QryLst = new List<IMongoQuery>();
                // QryLst.Add(Query.And(Query.EQ("Shortdesc", cat.Shortdesc), Query.EQ("Noun", cat.Noun)));
                if (cat.Vendorsuppliers != null)
                {
                    foreach (Vendorsuppliers vsup in cat.Vendorsuppliers)
                    {
                        //  string temp = Regex.Replace(vsup.RefNo, @"[^\w\d]", "");

                        if (vsup.RefNo != null && vsup.RefNoDup != null && vsup.RefNo != "" && vsup.Refflag == "POSITION NUMBER")
                        {
                            if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "")
                            {
                            var Qry2 = Query.And(Query.EQ("Vendorsuppliers.Refflag", "POSITION NUMBER"), Query.EQ("Vendorsuppliers.RefNoDup", vsup.RefNoDup), Query.EQ("Equipment.Modelno", cat.Equipment.Modelno));
                            //  var Qry2 = Query.EQ("Vendorsuppliers.RefNo", vsup.RefNo);
                            QryLst.Add(Qry2);
                        }

                        }

                    }
                }
                if (QryLst.Count > 0)
                {
                    var ary = Query.And(QryLst);
                    var Lst = _datamasterRepository.FindAll(ary).ToList();
                    if (Lst != null && Lst.Count > 1)
                    {
                        foreach (Prosol_Datamaster dm in Lst)
                        {
                            //int indc = 0;

                            //if (dm.Equipment == null  || cat.Equipment == null)
                            //{
                            //    indc = 1;
                            //}
                            //if (dm.Equipment != null && dm.Equipment.Modelno != null && dm.Equipment.Modelno != "" && cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "" && dm.Equipment.Modelno != cat.Equipment.Modelno)
                            //{
                            //    indc = 1;
                            //}
                            //else if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "" && (dm.Equipment == null || dm.Equipment.Modelno == null || dm.Equipment.Modelno == ""))
                            //{
                            //    indc = 1;
                            //}
                            //else if (dm.Equipment != null && dm.Equipment.Modelno != null && dm.Equipment.Modelno != "" && (cat.Equipment == null || cat.Equipment.Modelno == null || cat.Equipment.Modelno == ""))
                            //{
                            //    indc = 1;
                            //}
                            //if (indc == 0)
                            //{
                                if (-1 == dupList.FindIndex(f => f.Itemcode.Equals(dm.Itemcode)))
                                {
                                    dupList.Add(dm);
                                    dm.Referenceno = "PS DUP";
                                    dm.Duplicates = cat.Itemcode;
                                    _datamasterRepository.Add(dm);
                                }
                           // }

                        }
                    }
                }
              
                var query=Query.And(Query.EQ("Shortdesc", cat.Shortdesc), Query.EQ("Noun", cat.Noun));
                var vn = _datamasterRepository.FindAll(query).ToList();
                if (vn != null && vn.Count > 1)
                {
                    int cunn = 0;
                    foreach (Prosol_Datamaster dm in vn)
                    {
                        if (dm.Characteristics != null && cat.Characteristics != null)
                        {
                            int ind = 0;

                            //character checking
                            foreach (Prosol_AttributeList mdol in dm.Characteristics)
                            {
                                foreach (Prosol_AttributeList mdl in cat.Characteristics)
                                {
                                    if (mdl.Characteristic == mdol.Characteristic && mdl.Value != null && mdl.Value != "" && mdol.Value != null && mdol.Value != "")
                                    {
                                        if (mdl.Value != mdol.Value)
                                        {
                                            ind = 1;
                                        }

                                    }

                                }
                            }

                            //vendor checking
                            if (cat.Vendorsuppliers != null)
                            {
                                foreach (Vendorsuppliers vsup in dm.Vendorsuppliers)
                                {
                                    foreach (Vendorsuppliers mdl in cat.Vendorsuppliers)
                                    {
                                        if (vsup.RefNoDup != null && vsup.RefNoDup != "" && mdl.RefNoDup != null && mdl.RefNoDup!=vsup.RefNoDup)
                                        {
                                           
                                                ind = 1;
                                           

                                        }
                                        if (vsup.RefNoDup == null  && (mdl.RefNoDup != null || mdl.RefNoDup != ""))
                                        {

                                            ind = 1;


                                        }
                                        if (vsup.RefNoDup != null && (mdl.RefNoDup == null || mdl.RefNoDup == ""))
                                        {

                                            ind = 1;


                                        }

                                    }
                                }                
                            }

                            //equipment checking

                            if (dm.Equipment != null && dm.Equipment.Modelno != null && dm.Equipment.Modelno != "" && cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "" && dm.Equipment.Modelno != cat.Equipment.Modelno)
                            {
                                ind = 1;
                            }
                            else if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "" && (dm.Equipment == null || dm.Equipment.Modelno == null || dm.Equipment.Modelno == ""))
                            {
                                ind = 1;
                            }
                            else if (dm.Equipment != null && dm.Equipment.Modelno != null && dm.Equipment.Modelno != "" && (cat.Equipment == null || cat.Equipment.Modelno == null || cat.Equipment.Modelno == ""))
                            {
                                ind = 1;
                            }
                            if (ind == 0)
                            {
                                cunn = cunn + 1;

                            }
                        }
                    }
                    if (cunn > 1)
                    {
                        foreach (Prosol_Datamaster dm in vn)
                        {
                            if (dm.Characteristics != null && cat.Characteristics != null)
                            {
                                int ind = 0;

                                //character checking
                                foreach (Prosol_AttributeList mdol in dm.Characteristics)
                                {
                                    foreach (Prosol_AttributeList mdl in cat.Characteristics)
                                    {
                                        if (mdl.Characteristic == mdol.Characteristic && mdl.Value != null && mdl.Value != "" && mdol.Value != null && mdol.Value != "")
                                        {
                                            if (mdl.Value != mdol.Value)
                                            {
                                                ind = 1;
                                            }

                                        }

                                    }
                                }

                                //vendor checking
                                if (cat.Vendorsuppliers != null)
                                {
                                    foreach (Vendorsuppliers vsup in dm.Vendorsuppliers)
                                    {
                                        foreach (Vendorsuppliers mdl in cat.Vendorsuppliers)
                                        {
                                            if (vsup.RefNoDup != null && vsup.RefNoDup != "" && mdl.RefNoDup != null && mdl.RefNoDup != vsup.RefNoDup)
                                            {

                                                ind = 1;


                                            }
                                            if (vsup.RefNoDup == null && (mdl.RefNoDup != null || mdl.RefNoDup != ""))
                                            {

                                                ind = 1;


                                            }
                                            if (vsup.RefNoDup != null && (mdl.RefNoDup == null || mdl.RefNoDup == ""))
                                            {

                                                ind = 1;


                                            }

                                        }
                                    }
                                }

                                //equipment checking

                                if (dm.Equipment != null && dm.Equipment.Modelno != null && dm.Equipment.Modelno != "" && cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "" && dm.Equipment.Modelno != cat.Equipment.Modelno)
                                {
                                    ind = 1;
                                }
                                else if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "" && (dm.Equipment == null || dm.Equipment.Modelno == null || dm.Equipment.Modelno == ""))
                                {
                                    ind = 1;
                                }
                                else if (dm.Equipment != null && dm.Equipment.Modelno != null && dm.Equipment.Modelno != "" && (cat.Equipment == null || cat.Equipment.Modelno == null || cat.Equipment.Modelno == ""))
                                {
                                    ind = 1;
                                }
                                if (ind == 0)
                                {
                                    if (-1 == dupList.FindIndex(f => f.Itemcode.Equals(dm.Itemcode)))
                                    {
                                        dupList.Add(dm);
                                        dm.Referenceno = "CH DUP";
                                        dm.Duplicates = cat.Itemcode;
                                        _datamasterRepository.Add(dm);
                                    }

                                }
                            }
                        }
                    }

                }              

            }
            if (dupList != null && dupList.Count > 0)
            {
                return dupList;
            }
            else return null;

            //var query = Query.And(Query.EQ("Shortdesc", cat.Shortdesc), Query.NE("Shortdesc", ""), Query.NE("Itemcode", cat.Itemcode));
            //var vn = _DatamasterRepository.FindAll(query).ToList();
            //if (vn != null)
            //{
            //    if (vn.Count > 0)
            //    {
            //        return vn;
            //    }
            //    else return null;
            //}
            //else return null;
        }


        public List<Dictionary<string, object>> BulkAttribute(HttpPostedFileBase file)
        {
            int cunt = 0;
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
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

            //  DataTable resTbl = dt.AsEnumerable().GroupBy(x => x.Field<string>("MATERIAL")).Select(x => x.First()).CopyToDataTable();

            //  var LstNM = new List<Prosol_Datamaster>();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].ToString() != "")
                {

                    // Horizontal

                    //var ListattMdl = new List<Prosol_AttributeList>();
                    //for (int i=1; i <= dr.ItemArray.Length - 5;)
                    //{
                    //    if (dr[i] != null)
                    //    {
                    //        var attMdl = new Prosol_AttributeList();
                    //        attMdl.Characteristic = dr[i++].ToString();
                    //        attMdl.Value = dr[i++].ToString(); 
                    //        attMdl.UOM = dr[i++].ToString();
                    //        attMdl.ShortSquence =Convert.ToInt16(dr[i++]);
                    //        attMdl.Squence = Convert.ToInt16(dr[i++]);
                    //        ListattMdl.Add(attMdl);
                    //    }
                    //    else break;
                    //}                

                    //Vertical


                    var attrList = new List<Prosol_AttributeList>();
                    attrList = (from DataRow drw in dt.Rows
                                where drw["MATERIAL"].ToString() == dr[0].ToString()
                                select new Prosol_AttributeList()
                                {
                                    Characteristic = drw["Attributes"].ToString(),
                                    Value = drw["Values"].ToString(),
                                    UOM = drw["UOM"].ToString()

                                }).ToList();


                    int i = 1;
                    row = new Dictionary<string, object>();
                    row.Add("Unique ID", dr[0].ToString());
                    row.Add("Noun", dr[1].ToString());
                    row.Add("Modifier", dr[2].ToString());
                    foreach (var at in attrList)
                    {

                        row.Add("Attribute" + i, at.Characteristic);
                        row.Add("Value" + i, at.Value);
                        row.Add("UOM" + i, at.UOM);
                        //row.Add("Source" + i, at.Source);
                        // row.Add("SourceUrl" + i, at.SourceUrl);
                        i++;

                    }
                    rows.Add(row);
                    //var attrList = new List<Vendorsuppliers>();
                    //attrList = (from DataRow drw in dt.Rows
                    //            where drw["MATERIAL"].ToString() == dr[0].ToString()
                    //            select new Vendorsuppliers()
                    //            {
                    //                Type = drw["Vendor Type"].ToString(),
                    //                Name = drw["Name"].ToString(),
                    //                Refflag = drw["Ref Flag"].ToString(),
                    //                RefNo = drw["Ref No"].ToString(),
                    //                s = Convert.ToInt16(drw["S"]),
                    //                l = Convert.ToInt16(drw["L"])
                    //            }).ToList();


                }
            }

            return rows;

        }
        public virtual int BulkHSN(HttpPostedFileBase file)
        {
            int cunt = 0;
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
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

            var LstNM = new List<Prosol_HSNModel>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].ToString() != "" && dr[1].ToString() != "")
                {
                    var Mdl = new Prosol_HSNModel();
                    Mdl.Noun = dr[0].ToString();
                    Mdl.Modifier = dr[1].ToString();
                    Mdl.HSNID = dr[2].ToString();
                    Mdl.Desc = dr[3].ToString();
                    Mdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    LstNM.Add(Mdl);
                }
            }
            if (LstNM.Count > 0)
            {
                //IEnumerable<Prosol_NounModifiers> filteredList = LstNM.GroupBy(Prosol_NounModifiers => Prosol_NounModifiers.Modifier).Select(group => group.First())
                //    .GroupBy(Prosol_NounModifiers => Prosol_NounModifiers.Modifier).Select(group => group.First());

                List<Prosol_HSNModel> filteredList = LstNM.GroupBy(p => new { p.Noun, p.Modifier }).Select(g => g.First()).ToList();
                if (filteredList.Count > 0)
                {
                    var fRes = new List<Prosol_HSNModel>();
                    foreach (Prosol_HSNModel nm in filteredList.ToList())
                    {
                        var query = Query.And(Query.EQ("Noun", nm.Noun), Query.EQ("Modifier", nm.Modifier));
                        var ObjStr = _HSNMODELRepository.FindOne(query);
                        if (ObjStr == null)
                        {
                            fRes.Add(nm);

                        }
                    }
                    cunt = _HSNMODELRepository.Add(fRes);

                }
            }
            return cunt;

        }
        public virtual bool CreateHSN(Prosol_HSNModel hsn)
        {
            hsn.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            var res = false;
            var query = Query.And(Query.EQ("Noun", hsn.Noun), Query.EQ("Modifier", hsn.Modifier));
            var hn = _HSNMODELRepository.FindAll(query).ToList();
            if (hn.Count == 0 || (hn.Count == 1 && hn[0]._id == hsn._id))
            {
                res = _HSNMODELRepository.Add(hsn);
            }
            return res;

        }
        public virtual bool CreateHSN1(string Noun, string Modifier, string HSNID, string Desc)
        {

            var res = false;
            var query = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier));
            var hn = _HSNMODELRepository.FindAll(query).ToList();
            if (hn.Count == 0)
            {
                var Mdl = new Prosol_HSNModel();
                Mdl.Noun = Noun;
                Mdl.Modifier = Modifier;
                Mdl.HSNID = HSNID;
                Mdl.Desc = Desc;
                Mdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                res = _HSNMODELRepository.Add(Mdl);
            }
            return res;

        }
        public virtual IEnumerable<Prosol_HSNModel> GetHSNList(string srchtxt)
        {
            var query = Query.Or(Query.Matches("HSNID", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))),
                Query.Matches("Noun", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))),
                Query.Matches("Modifier", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))),
                Query.Matches("Desc", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))));
            var HSNList = _HSNMODELRepository.FindAll(query);
            return HSNList;
        }
        public virtual IEnumerable<Prosol_HSNModel> GetHSNList()
        {
            var HSNList = _HSNMODELRepository.FindAll();
            return HSNList;
        }
        public virtual bool DeleteHSN(string HSNID)
        {
            var query = Query.EQ("HSNID", HSNID);
            var res = _HSNMODELRepository.Delete(query);
            return res;

        }
        public Prosol_HSNModel GetHsn(string Noun, string Modifier)
        {
            var query = Query.And(Query.EQ("Noun", BsonRegularExpression.Create(new Regex(Noun, RegexOptions.IgnoreCase))), Query.EQ("Modifier", BsonRegularExpression.Create(new Regex(Modifier, RegexOptions.IgnoreCase))));
            var HSN = _HSNMODELRepository.FindOne(query);
            return HSN;
        }

        private string ShortDesc(Prosol_Datamaster cat,List<Prosol_Sequence> seqList,Prosol_UOMSettings UOMSet,IEnumerable<Prosol_Abbrevate> AbbrList)
        {

            string mfrref = "";

            var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
            var NMList = _nounModifierRepository.FindAll(FormattedQuery).ToList();
            var chList = _CharacteristicRepository.FindAll(FormattedQuery).ToList();
            //foreach(Prosol_Charateristics dml in chList)
            //{
            //    foreach (Prosol_AttributeList mdl in cat.Characteristics)
            //    {
            //        if(dml.Characteristic==mdl.Characteristic)
            //        {
            //            mdl.ShortSquence = dml.ShortSquence;
            //            mdl.ShortSquence = dml.ShortSquence;
            //        }

            //    }

            //}
            string ShortStr = "", strNM = "";
         
            //Short_Generic
            List<shortFrame> lst = new List<shortFrame>();
            foreach (Prosol_Sequence sq in seqList)
            {
                if (sq.Required == "Yes")
                {
                    switch (sq.CatId)
                    {
                        case 101:

                            if (NMList[0].Formatted == 1)
                            {
                                //var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Noun, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                //if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                //    ShortStr += abbObj.Abbrevated + sq.Separator;
                                //else ShortStr += cat.Noun + sq.Separator;


                                if (NMList[0].Nounabv != null && NMList[0].Nounabv != "")
                                    ShortStr += NMList[0].Nounabv + sq.Separator;
                                else ShortStr += cat.Noun + sq.Separator;
                            }
                            else
                            {
                                if (cat.Characteristics != null)
                                {
                                    var sObj = cat.Characteristics.OrderBy(x => x.Squence).FirstOrDefault();
                                    var abbObj = (from Abb in AbbrList where Abb.Value.Equals(sObj.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                        ShortStr += abbObj.Abbrevated + sq.Separator;
                                    else ShortStr += sObj.Value + sq.Separator;
                                }
                            }
                            strNM = ShortStr;
                            //var nounabbr = (from Abb in AbbrList where Abb.Value.Equals(cat.Noun, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                            //if (nounabbr != null && (nounabbr.Abbrevated != null && nounabbr.Abbrevated != ""))
                            //{
                            //    ShortStr += nounabbr.Abbrevated + sq.Separator;
                            //}
                            //else ShortStr += cat.Noun + sq.Separator;

                            break;
                        case 102:
                            if (cat.Modifier != "NO DEFINER" && cat.Modifier != "NO MODIFIER")
                            {
                                if (NMList[0].Formatted == 1)
                                {

                                    if (NMList[0].Modifierabv != null && NMList[0].Modifierabv != "")
                                        ShortStr += NMList[0].Modifierabv + sq.Separator;
                                    else ShortStr += cat.Modifier + sq.Separator;



                                    strNM = ShortStr;
                                }
                            }
                            break;
                        case 103:
                            int flg = 0;


                            //  int[] arrPos= new int[cat.Characteristics.Count];
                            //  string[] arrVal = new string[cat.Characteristics.Count];
                            int i = 0;
                            if (cat.Characteristics != null)
                            {
                                foreach (Prosol_AttributeList chM in cat.Characteristics.OrderBy(x => x.ShortSquence))
                                {
                                    if (NMList[0].Formatted == 1 || flg == 1)
                                    {

                                        if (chM.Value != null && chM.Value != "")
                                        {
                                            string strValue = "";
                                            var frmMdl = new shortFrame();
                                            frmMdl.position = chM.Squence;


                                            if (chM.Value.Contains(','))
                                            {
                                                string tmpstr = "";
                                                var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                                if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                                {
                                                    tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
                                                }
                                                else
                                                {
                                                    string[] strsplt = chM.Value.Split(',');
                                                    foreach (string str in strsplt)
                                                    {
                                                        abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                                            tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
                                                        else tmpstr += str.TrimStart().TrimEnd() + ',';
                                                    }
                                                }
                                                tmpstr = tmpstr.TrimEnd(',');
                                                if (UOMSet.Short_space == "with space")
                                                {
                                                    if (chM.UOM != null && chM.UOM != "")
                                                        strValue += tmpstr + " " + chM.UOM + sq.Separator;
                                                    else strValue += tmpstr + sq.Separator;
                                                }
                                                else
                                                {
                                                    if (chM.UOM != null && chM.UOM != "")
                                                        strValue += tmpstr + chM.UOM + sq.Separator;
                                                    else strValue += tmpstr + sq.Separator;
                                                }
                                                frmMdl.values = strValue;
                                            }
                                            else
                                            {
                                                var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                                if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                                {
                                                    // Abbreivated
                                                    if (UOMSet.Short_space == "with space")
                                                    {
                                                        if (chM.UOM != null && chM.UOM != "")
                                                            strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
                                                        else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
                                                    }
                                                    else
                                                    {
                                                        if (chM.UOM != null && chM.UOM != "")
                                                            strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
                                                        else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
                                                    }
                                                    frmMdl.values = strValue;
                                                }
                                                else
                                                {
                                                    // Abbreivated not exist

                                                    if (UOMSet.Short_space == "with space")
                                                    {
                                                        if (chM.UOM != null && chM.UOM != "")
                                                            strValue += chM.Value.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
                                                        else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
                                                    }
                                                    else
                                                    {
                                                        if (chM.UOM != null && chM.UOM != "")
                                                            strValue += chM.Value.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
                                                        else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
                                                    }
                                                    frmMdl.values = strValue;
                                                }
                                            }

                                            lst.Add(frmMdl);

                                            ShortStr = strNM;
                                            string pattern = " X ";
                                            foreach (shortFrame sMdl in lst)
                                            {
                                                foreach (Match match in Regex.Matches(sMdl.values, pattern))
                                                {
                                                    sMdl.values = Regex.Replace(sMdl.values, pattern, match.Value.TrimEnd().TrimStart());
                                                }
                                                // ShortStr += sMdl.values;
                                                string pattern1 = " x ";
                                                foreach (Match match in Regex.Matches(sMdl.values, pattern1))
                                                {
                                                    sMdl.values = Regex.Replace(sMdl.values, pattern1, match.Value.TrimEnd().TrimStart());
                                                }
                                                ShortStr += sMdl.values;
                                            }

                                            if (!checkLength(ShortStr, seqList[0].ShortLength))
                                            {
                                                ShortStr = ShortStr.Trim();
                                                char[] chr = sq.Separator.ToCharArray();
                                                ShortStr = ShortStr.TrimEnd(chr[0]);
                                                while (ShortStr.Length > seqList[0].ShortLength)
                                                {
                                                    int lstIndx = ShortStr.LastIndexOf(chr[0]);
                                                    if (lstIndx > -1)
                                                    {
                                                        if (lst.Count > 0)
                                                        {

                                                            if (ShortStr.Substring(lstIndx).Length > 1)
                                                            {
                                                                if (ShortStr.Substring(lstIndx).TrimStart(chr[0]) == lst[lst.Count - 1].values.TrimEnd(chr[0]))
                                                                {
                                                                    lst.RemoveAt(lst.Count - 1);
                                                                }
                                                                else
                                                                {
                                                                    int indx = lst[lst.Count - 1].values.TrimEnd(chr[0]).LastIndexOf(chr[0]);
                                                                    lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx) + chr[0];
                                                                }
                                                            }
                                                        }
                                                        ShortStr = ShortStr.Remove(lstIndx);

                                                    }
                                                    else
                                                    {
                                                        lstIndx = ShortStr.LastIndexOf(' ');
                                                        ShortStr = ShortStr.Remove(lstIndx);
                                                        if (lst.Count > 0)
                                                        {
                                                            int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
                                                            lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
                                                        }
                                                    }

                                                }
                                                ShortStr = ShortStr + chr[0];
                                            }
                                            i++;
                                        }
                                    }
                                    else flg = 1;
                                }
                                ShortStr = strNM;
                                foreach (shortFrame sMdl in lst.OrderBy(x => x.position))
                                {
                                    ShortStr += sMdl.values;
                                }
                            }

                            break;

                        case 104:
                            if (cat.Vendorsuppliers != null)
                            {
                                foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
                                {
                                    if (vs.s == 1)
                                    {
                                        if (vs.Name != null && vs.Name != "")
                                        {
                                            var querry = Query.EQ("Name", vs.Name);
                                            var shtmfr = _VendorRepository.FindOne(querry);
                                            if (shtmfr != null)
                                            {
                                                if (shtmfr.ShortDescName != null && shtmfr.ShortDescName != "")
                                                {
                                                    vs.shortmfr = shtmfr.ShortDescName;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            vs.shortmfr = vs.Name;
                                        }

                                        if (vs.shortmfr != null && vs.shortmfr != "" && vs.shortmfr != "undefined")
                                        {
                                            mfrref = vs.Name;
                                            var frmMdl = new shortFrame();
                                            frmMdl.position = 101;
                                            frmMdl.values = vs.shortmfr + sq.Separator;
                                            lst.Add(frmMdl);
                                            ShortStr += vs.shortmfr + sq.Separator;
                                            if (!checkLength(ShortStr, seqList[0].ShortLength))
                                            {
                                                ShortStr = ShortStr.Trim();
                                                char[] chr = sq.Separator.ToCharArray();
                                                ShortStr = ShortStr.TrimEnd(chr[0]);
                                                while (ShortStr.Length > seqList[0].ShortLength)
                                                {
                                                    int lstIndx = ShortStr.LastIndexOf(chr[0]);
                                                    if (lstIndx > -1)
                                                    {
                                                        if (lst.Count > 0)
                                                        {
                                                            if (ShortStr.Substring(lstIndx).Length > 1)
                                                                lst.RemoveAt(lst.Count - 1);
                                                        }
                                                        ShortStr = ShortStr.Remove(lstIndx);

                                                    }
                                                    else
                                                    {
                                                        lstIndx = ShortStr.LastIndexOf(' ');
                                                        ShortStr = ShortStr.Remove(lstIndx);
                                                        if (lst.Count > 0)
                                                        {
                                                            int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
                                                            lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
                                                        }
                                                    }
                                                }
                                                ShortStr = ShortStr + chr[0];
                                            }
                                            break;
                                        }

                                    }
                                }
                            }
                            break;
                        case 105:
                            if (cat.Vendorsuppliers != null)
                            {

                                foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
                                {
                                    if (vs.s == 1 && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined")
                                    {
                                        var frmMdl = new shortFrame();
                                        frmMdl.position = 100;
                                        frmMdl.values = vs.RefNo.Trim() + sq.Separator;
                                        lst.Add(frmMdl);
                                        // ShortStr = strNM;

                                        ShortStr += vs.RefNo.Trim() + sq.Separator;
                                        if (!checkLength(ShortStr, seqList[0].ShortLength))
                                        {
                                            ShortStr = ShortStr.Trim();
                                            char[] chr = sq.Separator.ToCharArray();
                                            ShortStr = ShortStr.TrimEnd(chr[0]);
                                            while (ShortStr.Length > seqList[0].ShortLength)
                                            {
                                                int lstIndx = ShortStr.LastIndexOf(chr[0]);
                                                if (lstIndx > -1)
                                                {
                                                    if (lst.Count > 0)
                                                    {
                                                        if (ShortStr.Substring(lstIndx).Length > 1)
                                                            lst.RemoveAt(lst.Count - 1);
                                                    }
                                                    ShortStr = ShortStr.Remove(lstIndx);

                                                }
                                                else
                                                {
                                                    lstIndx = ShortStr.LastIndexOf(' ');
                                                    ShortStr = ShortStr.Remove(lstIndx);
                                                    if (lst.Count > 0)
                                                    {
                                                        int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
                                                        lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
                                                    }
                                                }

                                            }
                                            ShortStr = ShortStr + chr[0];
                                        }
                                        break;
                                    }

                                }
                            }
                            break;
                        case 106:
                            if (cat.Application != null && cat.Application != "")
                                ShortStr += cat.Application + sq.Separator;
                            break;
                        case 107:
                            //if (cat.Drawingno != null && cat.Drawingno != "")
                            //    ShortStr += cat.Drawingno + sq.Separator;


                            foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
                            {
                                if (vs.Refflag.ToUpper() == "DRAWING & POSITION NUMBER" && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined" && vs.Name == mfrref && vs.Name != "" && vs.Name != null)
                                {
                                    ShortStr += vs.RefNo.Trim() + sq.Separator;
                                    break;
                                }
                            }
                            break;
                        case 108:
                            if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "")
                                ShortStr += cat.Equipment.Name + sq.Separator;
                            break;
                        case 109:
                            if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
                                ShortStr += cat.Equipment.Manufacturer + sq.Separator;
                            break;
                        case 110:
                            if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "")
                                ShortStr += cat.Equipment.Modelno + sq.Separator;
                            break;
                        case 111:
                            if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "")
                                ShortStr += cat.Equipment.Tagno + sq.Separator;
                            break;
                        case 112:
                            if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "")
                                ShortStr += cat.Equipment.Serialno + sq.Separator;
                            break;
                        case 113:
                            if (cat.Referenceno != null && cat.Referenceno != "")
                                ShortStr += cat.Referenceno + sq.Separator;
                            break;
                        case 114:
                            if (cat.Additionalinfo != null && cat.Additionalinfo != "")
                                ShortStr += cat.Additionalinfo + sq.Separator;
                            break;
                        case 115:
                            if (cat.Equipment != null && cat.Equipment.Additionalinfo != null && cat.Equipment.Additionalinfo != "")
                                ShortStr += cat.Equipment.Additionalinfo + sq.Separator;
                            break;


                    }
                    if (!checkLength(ShortStr, seqList[0].ShortLength))
                    {
                        ShortStr = ShortStr.Trim();
                        char[] chr = sq.Separator.ToCharArray();
                        ShortStr = ShortStr.TrimEnd(chr[0]);
                        while (ShortStr.Length > seqList[0].ShortLength)
                        {
                            int lstIndx = ShortStr.LastIndexOf(chr[0]);
                            ShortStr = ShortStr.Remove(lstIndx);
                        }
                        break;
                    }
                }
            }
            if (ShortStr.Length != seqList[0].ShortLength)
            {
                ShortStr = ShortStr.Trim();
                int lsIndx = ShortStr.Length;
                string str = ShortStr.Substring(lsIndx - 1, 1);
                if (str == seqList[0].Separator.Trim())
                {
                    ShortStr = ShortStr.Remove(lsIndx - 1);
                }
            }
            return ShortStr.ToUpper();
        }
      
        protected bool checkLength(string str, int len)
        {
            str = str.Trim();
            // int lstIndx = str.Length;
            //str = str.Remove(lstIndx - 1, 1);
            if (str.Length < len)
            {
                return true;
            }
            else return false;

        }
       
            
        private string LongDesc(Prosol_Datamaster cat, List<Prosol_Sequence> seqList, Prosol_UOMSettings UOMSet)
        {

            // var vendortype = _VendortypeRepository.FindAll().ToList();
            var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
            var NMList = _nounModifierRepository.FindAll(FormattedQuery).ToList();
          
            string LongStr = "";
         
            //Short_Generic
            foreach (Prosol_Sequence sq in seqList)
            {
                if (sq.Required == "Yes")
                {
                    switch (sq.CatId)
                    {
                        case 101:
                            if (NMList[0].Formatted == 1)
                                LongStr += cat.Noun + sq.Separator;
                            else LongStr += cat.Noun + " ";
                            break;
                        case 102:
                            if (cat.Modifier != "NO DEFINER" && cat.Modifier != "NO MODIFIER")
                                LongStr += cat.Modifier + sq.Separator;
                            break;
                        case 103:
                            if (cat.Characteristics != null)
                            {
                                foreach (Prosol_AttributeList chM in cat.Characteristics.OrderBy(x => x.Squence))
                                {
                                    if (chM.Value != null && chM.Value != "")
                                    {
                                        if (UOMSet.Long_space == "with space")
                                        {
                                            if (chM.UOM != null && chM.UOM != "")
                                                LongStr += chM.Characteristic + ":" + chM.Value + " " + chM.UOM + sq.Separator;
                                            else LongStr += chM.Characteristic + ":" + chM.Value + sq.Separator;
                                        }
                                        else
                                        {
                                            if (chM.UOM != null && chM.UOM != "")
                                                LongStr += chM.Characteristic + ":" + chM.Value + chM.UOM + sq.Separator;
                                            else LongStr += chM.Characteristic + ":" + chM.Value + sq.Separator;
                                        }

                                    }
                                }
                            }
                            break;

                        case 104:
                            //if (cat.Manufacturer != null && cat.Manufacturer != "")
                            //    LongStr += "Manufacturer:" + cat.Manufacturer + sq.Separator;
                            //break;
                            if (cat.Vendorsuppliers != null)
                            {

                                int g = 0;
                                foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
                                {

                                    if (vs.l == 1 && vs.Name != null && vs.Name != "")
                                    {
                                        LongStr += vs.Type + ":" + vs.Name + sq.Separator;

                                    }
                                    if (vs.l == 1 && vs.RefNo != null && vs.RefNo != "")
                                    {
                                        LongStr += vs.Refflag + ":" + vs.RefNo + sq.Separator;
                                    }
                                    //else
                                    //{
                                    //    if (vs.l == 1 && vs.RefNo != null && vs.RefNo != "")
                                    //    {
                                    //        LongStr += vs.Refflag + ":" + vs.RefNo + sq.Separator;
                                    //    }
                                    //}
                                }


                                //string[] mfrnames = new string[cat.Vendorsuppliers.Count];
                                //int iii = 0;
                                //foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
                                //{
                                //    if (!mfrnames.Contains(vs.Name))
                                //    {
                                //        mfrnames[iii] = vs.Name;
                                //        iii++;
                                //    }
                                //}
                                //foreach (string names in mfrnames)
                                //{
                                //    int g = 0;
                                //    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
                                //    {

                                //        if (vs.l == 1 && g == 0 && vs.Name == names && vs.Name != null && vs.Name != "")
                                //        {
                                //            LongStr += vs.Type + ":" + vs.Name + sq.Separator;
                                //            g = 1;
                                //        }
                                //        if (vs.l == 1 && vs.Name == names && vs.RefNo != null && vs.RefNo != "")
                                //        {
                                //            LongStr += vs.Refflag + ":" + vs.RefNo + sq.Separator;
                                //        }
                                //    }

                            }
                            break;

                        case 105:
                            //if (cat.Partno != null && cat.Partno != "")
                            //    LongStr += "Partno:" + cat.Partno + sq.Separator;
                            //break;


                            //foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
                            //{




                            //    if (vs.l == '1' && vs.Refflag == "PART NUMBER")
                            //    {
                            //        LongStr += vs.Type.ToUpper()+" "+ "PART NUMBER" + ":" + vs.RefNo + sq.Separator;

                            //    }


                            //}
                            break;

                        case 106:
                            if (cat.Application != null && cat.Application != "")
                                LongStr += "Application:" + cat.Application + sq.Separator;
                            break;
                        case 107:
                            //if (cat.Drawingno != null && cat.Drawingno != "")
                            //    LongStr += "Drawing NO.:" + cat.Drawingno + sq.Separator;
                            //break;


                            //foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
                            //{
                            //    if (vs.l == '1' && vs.Refflag == "DRAWING & POSITION NUMBER")
                            //    {
                            //        LongStr += vs.Type.ToUpper() + " DRAWING & POSITION NUMBER:" + vs.RefNo + sq.Separator;

                            //    }
                            //    if (vs.l == '1' && vs.Refflag == "MODEL NUMBER")
                            //    {
                            //        LongStr += vs.Type.ToUpper() + " MODEL NUMBER:" + vs.RefNo + sq.Separator;

                            //    }
                            //    if (vs.l == '1' && vs.Refflag == "REFERENCE NUMBER")
                            //    {
                            //        LongStr += vs.Type.ToUpper() + " REFERENCE NUMBER:" + vs.RefNo + sq.Separator;

                            //    }
                            //}

                            break;


                        case 108:
                            if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "")
                                LongStr += "Equipment Name:" + cat.Equipment.Name + sq.Separator;
                            break;
                        case 109:
                            if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
                                LongStr += "Equipment Manufacturer:" + cat.Equipment.Manufacturer + sq.Separator;
                            break;
                        case 110:
                            if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "")
                                LongStr += "Equipment Modelno:" + cat.Equipment.Modelno + sq.Separator;
                            break;
                        case 111:
                            if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "")
                                LongStr += "Equipment Tagno:" + cat.Equipment.Tagno + sq.Separator;
                            break;
                        case 112:
                            if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "")
                                LongStr += "Equipment Serialno:" + cat.Equipment.Serialno + sq.Separator;
                            break;
                        case 113:
                            if (cat.Referenceno != null && cat.Referenceno != "")
                                LongStr += "Position No.:" + cat.Referenceno + sq.Separator;
                            break;
                        case 114:
                            if (cat.Additionalinfo != null && cat.Additionalinfo != "")
                                LongStr += "Additional Information:" + cat.Additionalinfo + sq.Separator;
                            break;
                        case 115:
                            if (cat.Equipment != null && cat.Equipment.Additionalinfo != null && cat.Equipment.Additionalinfo != "")
                                LongStr += "Additional Information(Equipment):" + cat.Equipment.Additionalinfo + sq.Separator;
                            break;


                    }
                }
            }
            LongStr = LongStr.Trim();
            int lstIndx = LongStr.Length;
            LongStr = LongStr.Remove(lstIndx - 1, 1);
            return LongStr.ToUpper();
        }

        public bool Duplicatecheck(string id, bool sts)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("Islive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _ReftypeRepository.Update(query, Updae, flg);
            return res;

        }


        public virtual string BulkPVData(HttpPostedFileBase file)
        {
            int cunt = 0, lineId = 0;

            string ress = "";
            string errCodes = "";
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
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
            DataTable dt1 = res.Tables[0];

            if (dt1.Rows.Count > 0)
            {

                //var matLst = _datamasterRepository.FindAll().ToList();

                //var groupedMatLst = matLst.GroupBy(m => m.Materialcode)
                //                          .Where(g => g.Count() > 1)
                //                          .SelectMany(g => g)
                //                          .ToList();

                //foreach (var item in groupedMatLst)
                //{
                //    var query = Query.EQ("Materialcode", item.Materialcode);
                //    var Updte = Update.Set("Materialcode", "delete");
                //    var flg = UpdateFlags.Upsert;

                //    var theresult = _datamasterRepository.Update(query, Updte, flg);
                //}


                foreach (DataRow drw1 in dt1.Rows)
                {
                        var query2 = Query.Or(Query.EQ("Itemcode", drw1[0].ToString()), Query.EQ("Materialcode", drw1[0].ToString()));
                    var mdl2 = _datamasterRepository.FindOne(query2);
                    if (mdl2 != null)
                    {
                        //if (mdl2.PVstatus != "Pending")
                        //{
                        //    var erpQry = Query.EQ("Itemcode", mdl2.Itemcode);
                        //var erp = _erpRepository.FindOne(erpQry);
                        //if (erp == null)
                        //{
                        //    erp = new Prosol_ERPInfo();
                        //    erp.Itemcode = mdl2.Itemcode;
                        //}
                        //erp.StorageBin = drw1[1].ToString();
                        //erp.StorageLocation = drw1[3].ToString();
                        //_erpRepository.Add(erp);
                        mdl2.ItemStatus = 11;
                        mdl2.PVstatus = "Pending";
                        //var usrQry = Query.EQ("UserName", drw1[1].ToString());
                        var usrQry = Query.Matches("UserName", BsonRegularExpression.Create(new Regex(drw1[1].ToString().TrimStart().TrimEnd(), RegexOptions.IgnoreCase)));
                        var usrInfo = _UsersRepository.FindOne(usrQry);
                        var PV = new Prosol_UpdatedBy();
                        PV.UserId = usrInfo.Userid;
                        PV.Name = usrInfo.UserName;
                        PV.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        mdl2.PVuser = PV;
                        //mdl2.Catalogue = PV;
                        _datamasterRepository.Add(mdl2); cunt++;
                        //}
                        //else
                        //{
                        //    errCodes += drw1[0].ToString() + ",";
                        //}
                    }
                }

            }
            ress = cunt + " Items assigned successfully";
            if (errCodes != "")
            {
                if (errCodes.EndsWith(","))
                    errCodes = errCodes.TrimEnd(',');
                ress = $"{cunt} Items assigned successfully,\r\n{errCodes} this code(s) are already pv completed";
            }
            return cunt + " Items assigned successfully";

        }
        public virtual string BulkCatData(HttpPostedFileBase file)
        {
            int cunt = 0, lineId = 0;

            string ress = "";
            string errCodes = "";
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
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
            DataTable dt1 = res.Tables[0];

            if (dt1.Rows.Count > 0)
            {

                //var matLst = _datamasterRepository.FindAll().ToList();

                //var groupedMatLst = matLst.GroupBy(m => m.Materialcode)
                //                          .Where(g => g.Count() > 1)
                //                          .SelectMany(g => g)
                //                          .ToList();

                //foreach (var item in groupedMatLst)
                //{
                //    var query = Query.EQ("Materialcode", item.Materialcode);
                //    var Updte = Update.Set("Materialcode", "delete");
                //    var flg = UpdateFlags.Upsert;

                //    var theresult = _datamasterRepository.Update(query, Updte, flg);
                //}


                foreach (DataRow drw1 in dt1.Rows)
                {
                    var query2 = Query.Or(Query.EQ("Itemcode", drw1[0].ToString()), Query.EQ("Materialcode", drw1[0].ToString()));
                    var mdl2 = _datamasterRepository.FindOne(query2);
                    if (mdl2 != null)
                    {

                        //var erpQuery = Query.EQ("Itemcode", mdl2.Itemcode);
                        //var erpDt = _erpRepository.FindOne(erpQuery);
                        if (mdl2.PVstatus == "Completed")
                        {
                            mdl2.ItemStatus = 0;
                        //var usrQry = Query.EQ("UserName", drw1[1].ToString());
                        var usrQry = Query.Matches("UserName", BsonRegularExpression.Create(new Regex(drw1[1].ToString().TrimStart().TrimEnd(), RegexOptions.IgnoreCase)));
                        var usrInfo = _UsersRepository.FindOne(usrQry);
                        //foreach (TargetExn ent in usrInfo.Roles)
                        //{
                        //    if (ent.Name == "PV User")
                        //    {
                        //        var tmpStr = ent.TargetId;
                        //        var catQry = Query.Matches("Userid", tmpStr);
                        //        var catInfo = _UsersRepository.FindOne(usrQry);
                        //        var Cat = new Prosol_UpdatedBy();
                        //        Cat.UserId = catInfo.Userid;
                        //        Cat.Name = catInfo.UserName;
                        //        Cat.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        //        mdl2.Catalogue = Cat;
                        //    }
                        //}
                        var Cat = new Prosol_UpdatedBy();
                        Cat.UserId = usrInfo.Userid;
                        Cat.Name = usrInfo.UserName;
                        Cat.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        mdl2.Catalogue = Cat;
                        _datamasterRepository.Add(mdl2);
                        cunt++;
                    }
                    else
                    {
                        errCodes += drw1[0].ToString() + ",";
                    }
                }
                }

            }
            ress = cunt + " Items assigned successfully";
            if (errCodes != "")
            {
                if (errCodes.EndsWith(","))
                    errCodes = errCodes.TrimEnd(',');
                ress = $"{cunt} Items assigned successfully,\r\n{errCodes} this code(s) are pv not completed";
            }
            return ress;

        }
        public virtual string BulkQcData(HttpPostedFileBase file)
        {
            int cunt = 0, lineId = 0;
            string ress = "";
            string errCodes = "";
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
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
            DataTable dt1 = res.Tables[0];

            if (dt1.Rows.Count > 0)
            {

                //var matLst = _datamasterRepository.FindAll().ToList();

                //var groupedMatLst = matLst.GroupBy(m => m.Materialcode)
                //                          .Where(g => g.Count() > 1)
                //                          .SelectMany(g => g)
                //                          .ToList();

                //foreach (var item in groupedMatLst)
                //{
                //    var query = Query.EQ("Materialcode", item.Materialcode);
                //    var Updte = Update.Set("Materialcode", "delete");
                //    var flg = UpdateFlags.Upsert;

                //    var theresult = _datamasterRepository.Update(query, Updte, flg);
                //}


                foreach (DataRow drw1 in dt1.Rows)
                {
                    var query2 = Query.Or(Query.EQ("Itemcode", drw1[0].ToString()), Query.EQ("Materialcode", drw1[0].ToString()));
                    var mdl2 = _datamasterRepository.FindOne(query2);
                    if (mdl2 != null)
                    {
                        if (mdl2.ItemStatus > 1)
                        {
                            mdl2.ItemStatus = 2;
                            //var usrQry = Query.EQ("UserName", drw1[1].ToString());
                            var usrQry = Query.Matches("UserName", BsonRegularExpression.Create(new Regex(drw1[1].ToString().TrimStart().TrimEnd(), RegexOptions.IgnoreCase)));
                            var usrInfo = _UsersRepository.FindOne(usrQry);
                            var Qc = new Prosol_UpdatedBy();
                            Qc.UserId = usrInfo.Userid;
                            Qc.Name = usrInfo.UserName;
                            Qc.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                            mdl2.Review = Qc;
                            _datamasterRepository.Add(mdl2);
                            cunt++;
                        }
                        else
                        {
                            errCodes += drw1[0].ToString() + ",";
                        }
                    }
                }

            }
            ress = cunt + " Items assigned successfully";
            if (errCodes != "")
            {
                if (errCodes.EndsWith(","))
                    errCodes = errCodes.TrimEnd(',');
                ress = $"{cunt} Items assigned successfully,\n{errCodes} this code(s) are not catalogued";
            }
         return ress;

        }
        public virtual int BulkRework(HttpPostedFileBase file)
        {
            int cunt = 0;
            Stream stream = file.InputStream;
            IExcelDataReader reader = null;
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
            var usrLst = _UsersRepository.FindAll().ToList();
            foreach (DataRow drw in dt.Rows)
            {
                var query1 = Query.Or(Query.EQ("Materialcode", drw[0].ToString()), Query.EQ("Itemcode", drw[0].ToString()));
                var mdl1 = _datamasterRepository.FindOne(query1);
                if (mdl1 != null)
                {
                    mdl1.ItemStatus = drw[1].ToString() == "PV" ? 11 : drw[1].ToString() == "Cat" ? 0 : mdl1.ItemStatus;
                    mdl1.Reworkcat = drw[1].ToString() == "PV" ? "PV" : drw[1].ToString() == "Cat" ? "Rev" : "";
                    var upd = new Prosol_UpdatedBy();
                    if (!string.IsNullOrEmpty(drw[1].ToString()))
                    {
                        var usr = usrLst.FindAll(i => i.UserName == drw[2].ToString()).ToList();
                        upd.UserId = usr[0].Userid;
                        upd.Name = usr[0].UserName;
                        upd.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }
                    else
                    {
                        upd = drw[1].ToString() == "PV" ? mdl1.PVuser : drw[1].ToString() == "Cat" ? mdl1.Catalogue : mdl1.Catalogue;
                    }
                    if (drw[1].ToString() == "PV")
                    {
                        mdl1.PVuser = upd;
                        mdl1.PVstatus = "Pending";
                        mdl1.Catalogue = null;
                        mdl1.Review = null;
                    }
                    else
                    {
                        mdl1.Catalogue = upd;
                        mdl1.Review = null;
                    }
                    mdl1.RelRemarks = drw[3].ToString();
                    _datamasterRepository.Add(mdl1);
                    cunt++;
                }

            }


            return cunt;
        }
    }

    
    public class shortFrame
    {
        public int position { set; get; }
        public string values { set; get; }
    }
    public class shortFrame1
    {
        public int position { set; get; }
        public string values { set; get; }
        public string val { set; get; }
    }
    public class ImgLst
    {
        public string UniqueId { set; get; }
        public string AssetNo { set; get; }
        public string[] Images { set; get; }
    }
}
