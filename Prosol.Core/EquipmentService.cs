
using Excel;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Prosol.Core
{
    public class EquipmentService : IEquipment
    {

        private readonly IRepository<Prosol_Vendor> _VendorRepository;
        private readonly IRepository<Prosol_Datamaster> _DatamasterRepository;
        private readonly IRepository<Prosol_Sequence> _SequenceRepository;
        private readonly IRepository<Prosol_UOMSettings> _UOMRepository;
        private readonly IRepository<Prosol_NounModifiers> _nounModifierRepository;
        private readonly IRepository<Prosol_Users> _usersRepository;
        private readonly IRepository<Prosol_Abbrevate> _abbreivateRepository;
        private readonly IRepository<Prosol_ERPInfo> _erpRepository;
        private readonly IRepository<Prosol_Plants> _PlanttRepository;
        private readonly IRepository<Prosol_Attachment> _attchmentRepository;
        private readonly IRepository<Prosol_Charateristics> _CharacteristicRepository;
        private readonly IRepository<Prosol_UOMMODEL> _uomlistRepository;
        private readonly IRepository<Prosol_HSNModel> _HSNlistRepository;
        private readonly IRepository<Prosol_Attribute> _attributeRepository;
        private readonly IRepository<Prosol_UOM> _uomRepository;
        private readonly IRepository<Prosol_Logic> _logicRepository;
        private readonly IRepository<Prosol_Request> _ProsolRequest;
        private readonly IRepository<Prosol_Vendortype> _VendortypeRepository;
        // private readonly IRepository<Prosol_ApiModel> _apimodel;
        private readonly IRepository<Prosol_MaterialBom> _matbomRepository;

        private readonly IRepository<Prosol_Funloc> _FunlocRepository;
        private readonly IRepository<Prosol_equipbom> _EquipmentRepository;
        private readonly IRepository<Prosol_CodeLogic> _CodeLogic;
        private readonly IRepository<Prosol_Reftype> _ReftypeRepository;
        private readonly IRepository<Prosol_EquipDictionary> _EquipDictionaryRepository;

        public EquipmentService(IRepository<Prosol_Vendor> vendorRepository,
            IRepository<Prosol_Datamaster> datamasterRepository,
            IRepository<Prosol_Sequence> seqRepository,
            IRepository<Prosol_UOMSettings> UOMRepository,
            IRepository<Prosol_HSNModel> HSNlistRepository,
            IRepository<Prosol_NounModifiers> nounModifierRepository,
            IRepository<Prosol_Users> usersRepository,
            IRepository<Prosol_Abbrevate> abbreivateRepository,
            IRepository<Prosol_ERPInfo> erpRepository,
            IRepository<Prosol_Attachment> attchmentRepository,
            IRepository<Prosol_Plants> PlantRepository,
            IRepository<Prosol_Charateristics> attributesRepository,
            IRepository<Prosol_UOMMODEL> uomlistRepository,
            IRepository<Prosol_Attribute> attributeRepository,
            IRepository<Prosol_UOM> uomRepository,
            IRepository<Prosol_Logic> logicRepository,
            IRepository<Prosol_Request> ProsolRequest, IEmailSettings Emailservc,
            I_ItemRequest ItemRequestService,
            IRepository<Prosol_Vendortype> VendortypeRepository,
            // IRepository<Prosol_ApiModel> apimodel,
            IRepository<Prosol_MaterialBom> MatbomRepository,
            IRepository<Prosol_Funloc> FunlocRepository,
            IRepository<Prosol_equipbom> EquipmentRepository,
              IRepository<Prosol_CodeLogic> CodeLogic,
              IRepository<Prosol_EquipDictionary> EquipDictionary,
                 IRepository<Prosol_Reftype> ReftypeRepository)
        {

            this._VendorRepository = vendorRepository;
            this._DatamasterRepository = datamasterRepository;
            this._SequenceRepository = seqRepository;
            this._UOMRepository = UOMRepository;
            this._nounModifierRepository = nounModifierRepository;
            this._usersRepository = usersRepository;
            this._abbreivateRepository = abbreivateRepository;
            this._erpRepository = erpRepository;
            this._attchmentRepository = attchmentRepository;
            this._PlanttRepository = PlantRepository;
            this._CharacteristicRepository = attributesRepository;
            this._uomlistRepository = uomlistRepository;
            this._attributeRepository = attributeRepository;
            this._uomRepository = uomRepository;
            this._HSNlistRepository = HSNlistRepository;
            this._HSNlistRepository = HSNlistRepository;
            this._EquipDictionaryRepository = EquipDictionary;
            this._ProsolRequest = ProsolRequest;


            this._VendortypeRepository = VendortypeRepository;
            // this._apimodel = apimodel;
            this._matbomRepository = MatbomRepository;

            this._FunlocRepository = FunlocRepository;
            this._EquipmentRepository = EquipmentRepository;
            this._CodeLogic = CodeLogic;
            this._logicRepository = logicRepository;

            this._ReftypeRepository = ReftypeRepository;

        }


        public void change_statusforedit(string userid, string itemcode)
        {
            var qry = Query.And(Query.EQ("Materialcode", itemcode), Query.EQ("Release.UserId", userid));
            var res = _DatamasterRepository.FindOne(qry);
            if (res != null)
            {
                res.ItemStatus = 5;
                _DatamasterRepository.Add(res);
            }

        }
        public virtual int BulkData(HttpPostedFileBase file)
        {
            int cunt = 0;
            Prosol_Datamaster pd = new Prosol_Datamaster();
            Prosol_UpdatedBy userobj = new Prosol_UpdatedBy();
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
            foreach (DataRow drw in dt.Rows)
            {
                var ress = new Prosol_Datamaster();

                var query1 = Query.And(Query.EQ("Materialcode", drw[0].ToString()));
                var mdl1 = _DatamasterRepository.FindOne(query1);
                if (mdl1 == null)
                {
                    ress.Materialcode = drw[0].ToString();
                    ress.Itemcode = drw[0].ToString();
                    ress.Noun = drw[1].ToString();
                    ress.Modifier = drw[2].ToString();
                    ress.UOM = drw[3].ToString();
                    ress.Legacy = drw[4].ToString();
                    ress.Legacy2 = drw[5].ToString();
                    ress.Additionalinfo = drw[6].ToString();
                    ress.ItemStatus = Convert.ToInt16(drw[9]);

                    var username = drw[7].ToString();
                    var query = Query.EQ("UserName", username);
                    var userid = _usersRepository.FindOne(query);
                    userobj.UserId = userid.Userid.ToString();
                    userobj.Name = username;
                    userobj.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                    ress.Catalogue = userobj;
                    if (ress.ItemStatus == 2 || ress.ItemStatus == 3)
                    {
                        ress.Review = userobj;
                    }
                    if (ress.ItemStatus == 4 || ress.ItemStatus == 5)
                    {
                        ress.Review = userobj;
                        ress.Release = userobj;
                    }
                    if (ress.ItemStatus == 6)
                    {
                        ress.Review = userobj;
                        ress.Release = userobj;
                    }

                    ress.CreatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    ress.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    _DatamasterRepository.Add(ress);
                    var qy = Query.EQ("Itemcode", ress.Itemcode);
                    var obj = _erpRepository.FindOne(qy);
                    if (obj == null)
                    {
                        var obj1 = new Prosol_ERPInfo();
                        obj1.Itemcode = ress.Itemcode;
                        obj1.Plant = userid.Plantcode[0];
                        _erpRepository.Add(obj1);

                    }
                    cunt++;
                }
                else if (mdl1.Additionalinfo == "" || mdl1.Additionalinfo == null)
                {
                    ress.Additionalinfo = drw[6].ToString();
                    var q = Query.EQ("Materialcode", mdl1.Materialcode);
                    var up = Update.Set("Additionalinfo", drw[6].ToString());
                    var flg = UpdateFlags.Multi;
                    var result = _DatamasterRepository.Update(q, up, flg);
                    cunt++;
                }
            }
            return cunt;
        }

        public virtual int BulkAttribute(HttpPostedFileBase file)
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
            foreach (DataRow drw in dt.Rows)
            {
                if (drw[3] != null && drw[3].ToString() != "")
                {

                    var ress = new Prosol_Abbrevate();
                    var query = Query.EQ("Value", drw[2].ToString());
                    var mdl = _abbreivateRepository.FindOne(query);
                    if (mdl == null)
                    {
                        ress.Value = drw[2].ToString();
                        ress.Abbrevated = drw[3].ToString();
                        _abbreivateRepository.Add(ress);
                    }
                }


            }
            DataTable resTbl = dt.AsEnumerable().GroupBy(x => x.Field<string>("MATERIAL")).Select(x => x.First()).CopyToDataTable();
            foreach (DataRow dr in resTbl.Rows)
            {


                if (dr[0].ToString() != "")
                {

                    var attrList = new List<Prosol_AttributeList>();

                    attrList = (from DataRow drw in dt.Rows
                                where drw["MATERIAL"].ToString() == dr[0].ToString()


                                select new Prosol_AttributeList()
                                {


                                    Characteristic = drw["CHARACTERISTIC"].ToString(),
                                    Value = drw["VALUES"].ToString(),
                                    UOM = drw["UNIT"].ToString(),
                                    SourceUrl = drw["SourceUrl"].ToString(),
                                    Source = drw["Source"].ToString()
                                }).ToList();


                    var query = Query.EQ("Materialcode", dr[0].ToString());
                    var mdl = _DatamasterRepository.FindOne(query);
                    if (mdl != null)
                    {
                        var l = new List<Prosol_AttributeList>();
                        foreach (Prosol_AttributeList v in attrList)
                        {
                            var m = new Prosol_AttributeList();
                            var d = Query.And(Query.EQ("Noun", mdl.Noun), Query.EQ("Modifier", mdl.Modifier), Query.EQ("Characteristic", v.Characteristic));
                            var m1 = _CharacteristicRepository.FindOne(d);

                            m.Characteristic = v.Characteristic;
                            m.Value = v.Value;
                            m.UOM = v.UOM;
                            m.ShortSquence = m1.ShortSquence;
                            m.Squence = m1.Squence;
                            m.Source = v.Source;
                            m.SourceUrl = v.SourceUrl;
                            l.Add(m);


                        }
                        mdl.Characteristics = l;
                        _DatamasterRepository.Add(mdl);
                        cunt++;
                    }

                }
            }

            return cunt;

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

            DataTable resTbl = dt.AsEnumerable().GroupBy(x => x.Field<string>("MATERIAL")).Select(x => x.First()).CopyToDataTable();

            //  var LstNM = new List<Prosol_Datamaster>();
            foreach (DataRow dr in resTbl.Rows)
            {
                if (dr[0].ToString() != "")
                {

                    var attrList = new List<Vendorsuppliers>();
                    attrList = (from DataRow drw in dt.Rows
                                where drw["MATERIAL"].ToString() == dr[0].ToString()
                                select new Vendorsuppliers()
                                {
                                    Type = drw["Vendor Type"].ToString(),
                                    Name = drw["Name"].ToString(),
                                    Refflag = drw["Ref Flag"].ToString(),
                                    RefNo = drw["Ref No"].ToString(),
                                    s = Convert.ToInt16(drw["S"]),
                                    l = Convert.ToInt16(drw["L"])
                                }).ToList();

                    var query = Query.EQ("Materialcode", dr[0].ToString());
                    var mdl = _DatamasterRepository.FindOne(query);
                    if (mdl != null)
                    {

                        mdl.Vendorsuppliers = attrList;

                        _DatamasterRepository.Add(mdl);
                        cunt++;
                    }

                }
            }

            return cunt;

        }

        public virtual int BulkEquip(HttpPostedFileBase file)
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

            DataTable resTbl = dt.AsEnumerable().GroupBy(x => x.Field<string>("MATERIAL")).Select(x => x.First()).CopyToDataTable();


            foreach (DataRow dr in resTbl.Rows)
            {
                if (dr[0].ToString() != "")
                {

                    var mdl = new Equipments();
                    mdl.Name = dr[1].ToString();
                    mdl.Modelno = dr[3].ToString();
                    mdl.Tagno = dr[5].ToString();
                    mdl.Serialno = dr[6].ToString();
                    mdl.Additionalinfo = dr[2].ToString();
                    mdl.Manufacturer = dr[4].ToString();
                    mdl.Soureurl = dr[7].ToString();
                    mdl.ENS = Convert.ToInt32(dr[8].ToString());
                    mdl.EMS = Convert.ToInt32(dr[9].ToString());

                    var query = Query.EQ("Materialcode", dr[0].ToString());
                    var mdl1 = _DatamasterRepository.FindOne(query);
                    if (mdl1 != null)
                    {

                        mdl1.Equipment = mdl;

                        _DatamasterRepository.Add(mdl1);
                        cunt++;
                    }

                }
            }

            return cunt;

        }
        public virtual int BulkShortLong(HttpPostedFileBase file)
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
            foreach (DataRow drw in dt.Rows)
            {
                var ress = new Prosol_Datamaster();

                var query1 = Query.And(Query.EQ("Materialcode", drw[0].ToString()));
                var mdl1 = _DatamasterRepository.FindOne(query1);
                if (mdl1 != null)
                {
                    var ListCharas = mdl1.Characteristics;
                    var vendorsuppliersList = mdl1.Vendorsuppliers;
                    var LstVendors = new List<Vendorsuppliers>();
                    if (vendorsuppliersList != null && vendorsuppliersList.Count > 0)
                    {
                        foreach (Vendorsuppliers LstAtt in vendorsuppliersList)
                        {
                            if ((LstAtt.Name != null && LstAtt.Name != "") || (LstAtt.RefNo != "" && LstAtt.RefNo != null))
                            {

                                var vndMdl = new Vendorsuppliers();
                                vndMdl.slno = LstAtt.slno;
                                vndMdl.Code = LstAtt.Code;
                                vndMdl.Name = LstAtt.Name;
                                vndMdl.Type = LstAtt.Type;
                                vndMdl.RefNo = LstAtt.RefNo;
                                vndMdl.Refflag = LstAtt.Refflag;
                                vndMdl.s = LstAtt.s;
                                vndMdl.l = LstAtt.l;
                                vndMdl.shortmfr = LstAtt.shortmfr;
                                LstVendors.Add(vndMdl);
                            }


                        }
                    }
                    var lstCharateristics = new List<Prosol_AttributeList>();
                    if (ListCharas != null)
                    {

                        foreach (Prosol_AttributeList LstAtt in ListCharas)
                        {
                            var AttrMdl = new Prosol_AttributeList();
                            AttrMdl.Characteristic = LstAtt.Characteristic;
                            AttrMdl.Value = LstAtt.Value;
                            AttrMdl.UOM = LstAtt.UOM;
                            AttrMdl.Squence = LstAtt.Squence;
                            AttrMdl.ShortSquence = LstAtt.ShortSquence;
                            AttrMdl.Source = LstAtt.Source;
                            lstCharateristics.Add(AttrMdl);

                        }
                    }
                    var proCat = new Prosol_Datamaster();
                    proCat.Itemcode = mdl1.Itemcode != null ? mdl1.Itemcode : "";
                    proCat.Noun = mdl1.Noun;
                    proCat.Modifier = mdl1.Modifier;
                    proCat.Partno = mdl1.Partno;
                    proCat.Characteristics = lstCharateristics;
                    proCat.Vendorsuppliers = LstVendors;
                    proCat.Additionalinfo = mdl1.Additionalinfo;
                    //Equipment
                    var Equipment = mdl1.Equipment;
                    if (Equipment != null)
                    {
                        var EquModel = mdl1.Equipment;
                        var Equi_mdl = new Equipments();
                        if (EquModel != null)
                        {
                            Equi_mdl.Name = EquModel.Name;
                            Equi_mdl.Manufacturer = EquModel.Manufacturer;
                            Equi_mdl.Modelno = EquModel.Modelno;
                            Equi_mdl.Tagno = EquModel.Tagno;
                            Equi_mdl.Serialno = EquModel.Serialno;
                            Equi_mdl.Additionalinfo = EquModel.Additionalinfo;
                            Equi_mdl.ENS = EquModel.ENS;
                            Equi_mdl.EMS = EquModel.EMS;
                            proCat.Equipment = Equi_mdl;

                        }
                    }


                    var dd = GenerateShortLong(proCat);
                    var shrt = dd[0];
                    var lng = dd[1];
                    var q = Query.EQ("Materialcode", drw[0].ToString());
                    var up = Update.Set("Shortdesc", shrt).Set("Longdesc", lng);
                    var flg = UpdateFlags.Multi;
                    var result = _DatamasterRepository.Update(q, up, flg);

                    cunt++;
                }

            }
            return cunt;
        }


        public IEnumerable<Prosol_Vendor> GetVendorList(string term)
        {

            var query = Query.And(Query.EQ("Enabled", true), Query.Matches("Name", BsonRegularExpression.Create(new Regex(term, RegexOptions.IgnoreCase))));
            var arrResult = _VendorRepository.FindAll(query);
            return arrResult;
        }


        public string ShortDesc(Prosol_Datamaster cat)
        {

            string mfrref = "";

            var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
            var NMList = _nounModifierRepository.FindOne(FormattedQuery);
            var sort = SortBy.Ascending("Seq").Ascending("Description");

            var query = (NMList != null && NMList.Formatted == 1) ? Query.EQ("Description", "Short_Generic") : Query.EQ("Description", "Short_OEM");
            string ShortStr = "", strNM = "";

            var seqList = _SequenceRepository.FindAll(query, sort).ToList();
            var UOMSet = _UOMRepository.FindOne();

            var AbbrList = _abbreivateRepository.FindAll();
            //Short_Generic
            List<shortFrame> lst = new List<shortFrame>();
            foreach (Prosol_Sequence sq in seqList)
            {
                if (sq.Required == "Yes")
                {
                    switch (sq.CatId)
                    {
                        case 101:


                            if (NMList.Formatted == 1 || NMList.Formatted == 2)
                            {
                                //var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Noun, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                //if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                //    ShortStr += abbObj.Abbrevated + sq.Separator;
                                //else ShortStr += cat.Noun + sq.Separator;


                                if (NMList.Nounabv != null && NMList.Nounabv != "")
                                    ShortStr += NMList.Nounabv + sq.Separator;
                                else ShortStr += cat.Noun + sq.Separator;
                            }
                            else
                            {
                                if (cat.Characteristics != null)
                                {
                                    var sObj = cat.Characteristics.OrderBy(x => x.Squence).FirstOrDefault();


                                    //var abbObj = (from Abb in AbbrList where Abb.Value.Equals(sObj.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                    //if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                    //    ShortStr += abbObj.Abbrevated + sq.Separator;
                                    //else ShortStr += sObj.Value + sq.Separator;

                                    string tmpstr = "";
                                    if (sObj.Value.Contains(' '))
                                    {
                                        //fore space split

                                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(sObj.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                            ShortStr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
                                        else
                                        {
                                            string[] spaceSpt = sObj.Value.Split(' ');
                                            foreach (string spceStr in spaceSpt)
                                            {
                                                abbObj = (from Abb in AbbrList where Abb.Value.Equals(spceStr, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                                if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                                    ShortStr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
                                                else
                                                    ShortStr += spceStr.TrimStart().TrimEnd() + ' ';

                                            }
                                            ShortStr = ShortStr.TrimEnd(' ');
                                        }
                                        ShortStr = ShortStr.Trim(',');
                                        if (UOMSet.Short_space == "with space")
                                        {
                                            if (sObj.UOM != null && sObj.UOM != "")
                                                ShortStr += tmpstr.TrimStart().TrimEnd() + " " + sObj.UOM + sq.Separator;
                                            else ShortStr += tmpstr.TrimStart().TrimEnd() + sq.Separator;
                                        }
                                        else
                                        {
                                            if (sObj.UOM != null && sObj.UOM != "")
                                                ShortStr += tmpstr.TrimStart().TrimEnd() + sObj.UOM + sq.Separator;
                                            else ShortStr += tmpstr.TrimStart().TrimEnd() + sq.Separator;
                                        }
                                        // frmMdl.values = tmpstr;

                                    }
                                    else
                                    {
                                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(sObj.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                        {
                                            // Abbreivated
                                            if (UOMSet.Short_space == "with space")
                                            {
                                                if (sObj.UOM != null && sObj.UOM != "")
                                                    ShortStr += abbObj.Abbrevated.TrimStart().TrimEnd() + " " + sObj.UOM + sq.Separator;
                                                else ShortStr += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
                                            }
                                            else
                                            {
                                                if (sObj.UOM != null && sObj.UOM != "")
                                                    ShortStr += abbObj.Abbrevated.TrimStart().TrimEnd() + sObj.UOM + sq.Separator;
                                                else ShortStr += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
                                            }
                                            //frmMdl.values = strValue;
                                        }
                                        else
                                        {
                                            // Abbreivated not exist

                                            if (UOMSet.Short_space == "with space")
                                            {
                                                if (sObj.UOM != null && sObj.UOM != "")
                                                    ShortStr += sObj.Value.TrimStart().TrimEnd() + " " + sObj.UOM + sq.Separator;
                                                else ShortStr += sObj.Value.TrimStart().TrimEnd() + sq.Separator;
                                            }
                                            else
                                            {
                                                if (sObj.UOM != null && sObj.UOM != "")
                                                    ShortStr += sObj.Value.TrimStart().TrimEnd() + sObj.UOM + sq.Separator;
                                                else ShortStr += sObj.Value.TrimStart().TrimEnd() + sq.Separator;
                                            }
                                            //frmMdl.values = strValue;
                                        }
                                    }

                                    //ShortStr += tmpstr;
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
                        //case 102:
                        //    if (cat.Modifier != "NO DEFINER" && cat.Modifier != "NO MODIFIER")
                        //    {
                        //        if (NMList.Formatted == 1)
                        //        {

                        //            if (NMList.Modifierabv != null && NMList.Modifierabv != "")
                        //                ShortStr += NMList.Modifierabv + sq.Separator;
                        //            else ShortStr += cat.Modifier + sq.Separator;



                        //            strNM = ShortStr;
                        //        }
                        //    }
                        //    break;
                        case 103:
                            int flg = 0;


                            //  int[] arrPos= new int[cat.Characteristics.Count];
                            //  string[] arrVal = new string[cat.Characteristics.Count];
                            int i = 0;
                            if (cat.Characteristics != null)
                            {
                                foreach (Prosol_AttributeList chM in cat.Characteristics.OrderBy(x => x.ShortSquence))
                                {
                                    if (NMList.Formatted == 1 || flg == 1 || NMList.Formatted == 2)
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
                                                        //for space split
                                                        if (str.Contains(' '))
                                                        {
                                                            abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                                            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                                                tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
                                                            else
                                                            {
                                                                string[] spaceSpt = str.Split(' ');
                                                                foreach (string spceStr in spaceSpt)
                                                                {
                                                                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(spceStr, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                                                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                                                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
                                                                    else
                                                                        tmpstr += spceStr.TrimStart().TrimEnd() + ' ';

                                                                }
                                                                tmpstr = tmpstr.TrimEnd(' ');
                                                            }


                                                        }
                                                        else
                                                        {

                                                            abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                                            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                                                tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
                                                            else tmpstr += str.TrimStart().TrimEnd() + ',';
                                                        }
                                                    }
                                                }
                                                tmpstr = tmpstr.Trim(',');
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
                                                string tmpstr = "";
                                                if (chM.Value.Contains(' '))
                                                {
                                                    //fore space split

                                                    var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
                                                    else
                                                    {
                                                        string[] spaceSpt = chM.Value.Split(' ');
                                                        foreach (string spceStr in spaceSpt)
                                                        {
                                                            abbObj = (from Abb in AbbrList where Abb.Value.Equals(spceStr, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                                            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                                                tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
                                                            else
                                                                tmpstr += spceStr.TrimStart().TrimEnd() + ' ';

                                                        }
                                                        tmpstr = tmpstr.TrimEnd(' ');
                                                    }
                                                    tmpstr = tmpstr.TrimEnd(',');
                                                    if (UOMSet.Short_space == "with space")
                                                    {
                                                        if (chM.UOM != null && chM.UOM != "")
                                                            strValue += tmpstr.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
                                                        else strValue += tmpstr.TrimStart().TrimEnd() + sq.Separator;
                                                    }
                                                    else
                                                    {
                                                        if (chM.UOM != null && chM.UOM != "")
                                                            strValue += tmpstr.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
                                                        else strValue += tmpstr.TrimStart().TrimEnd() + sq.Separator;
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

                                            }

                                            lst.Add(frmMdl);

                                            ShortStr = strNM;
                                            string pattern = " X ";
                                            foreach (shortFrame sMdl in lst)
                                            {
                                                string[] strtmp = { " X " };
                                                var x = sMdl.values.Split(strtmp, StringSplitOptions.RemoveEmptyEntries);
                                                if (Regex.IsMatch(x[0], "^[0-9]+$", RegexOptions.Compiled))
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
                                            else
                                            {
                                                vs.shortmfr = vs.Name;
                                            }
                                        }
                                        //else
                                        //{
                                        //    vs.shortmfr = vs.Name;
                                        //}

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
                                        string prefix = "";
                                        if (vs.Refflag != null)
                                        {
                                            var flag = Query.EQ("Type", vs.Refflag);
                                            var code = _ReftypeRepository.FindOne(flag);
                                            if (code != null)
                                            {
                                                prefix = code.Code + ":";
                                            }
                                        }
                                        var frmMdl = new shortFrame();
                                        frmMdl.position = 100;
                                        frmMdl.values = prefix + vs.RefNo.Trim() + sq.Separator;
                                        lst.Add(frmMdl);
                                        // ShortStr = strNM;

                                        ShortStr += prefix + vs.RefNo.Trim() + sq.Separator;
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
                            //foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
                            //{
                            //    if (vs.s == 1 && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined")
                            //    {
                            //        var frmMdl = new shortFrame();
                            //        frmMdl.position = 100;
                            //        frmMdl.values = vs.RefNo.Trim() + sq.Separator;
                            //        lst.Add(frmMdl);
                            //        // ShortStr = strNM;

                            //        ShortStr += vs.RefNo.Trim() + sq.Separator;
                            //        if (!checkLength(ShortStr, seqList[0].ShortLength))
                            //        {
                            //            ShortStr = ShortStr.Trim();
                            //            char[] chr = sq.Separator.ToCharArray();
                            //            ShortStr = ShortStr.TrimEnd(chr[0]);
                            //            while (ShortStr.Length > seqList[0].ShortLength)
                            //            {
                            //                int lstIndx = ShortStr.LastIndexOf(chr[0]);
                            //                if (lstIndx > -1)
                            //                {
                            //                    if (lst.Count > 0)
                            //                    {
                            //                        if (ShortStr.Substring(lstIndx).Length > 1)
                            //                            lst.RemoveAt(lst.Count - 1);
                            //                    }
                            //                    ShortStr = ShortStr.Remove(lstIndx);

                            //                }
                            //                else
                            //                {
                            //                    lstIndx = ShortStr.LastIndexOf(' ');
                            //                    ShortStr = ShortStr.Remove(lstIndx);
                            //                    if (lst.Count > 0)
                            //                    {
                            //                        int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
                            //                        lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
                            //                    }
                            //                }

                            //            }
                            //            ShortStr = ShortStr + chr[0];
                            //        }
                            //        break;
                            //    }

                            //}
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

                            if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "" && (cat.Equipment.ENS == 1 || NMList.Formatted == 0))
                            {
                                var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Equipment.Name, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                {
                                    ShortStr += "F/" + abbObj.Abbrevated + sq.Separator;
                                }
                                else
                                {
                                    if (cat.Equipment.Name.Contains(' '))
                                    {
                                        string tmpstr = "";

                                        string[] strsplt = cat.Equipment.Name.Split(' ');
                                        foreach (string str in strsplt)
                                        {
                                            abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                                tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
                                            else tmpstr += str.TrimStart().TrimEnd() + ' ';
                                        }

                                        tmpstr = tmpstr.TrimEnd(' ');
                                        ShortStr += "F/" + tmpstr + sq.Separator;
                                    }
                                    else
                                    {
                                        ShortStr += "F/" + cat.Equipment.Name + sq.Separator;
                                    }

                                }
                            }
                            //ShortStr += "F/" + cat.Equipment.Name + sq.Separator;

                            break;
                        //case 108:

                        //    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "" && cat.Equipment.ENS == 1)
                        //    {
                        //        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Equipment.Name, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                        //        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                        //        {
                        //            ShortStr += "F/" + abbObj.Abbrevated + sq.Separator;
                        //        }
                        //        else
                        //            ShortStr += "F/" + cat.Equipment.Name + sq.Separator;
                        //    }
                        //    //ShortStr += "F/" + cat.Equipment.Name + sq.Separator;

                        //    break;
                        case 109:
                            if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
                                ShortStr += cat.Equipment.Manufacturer + sq.Separator;
                            break;
                        case 110:
                            if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "" && (cat.Equipment.EMS == 1 || NMList.Formatted == 0))
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
                        //case 108:
                        //    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "")
                        //        ShortStr += cat.Equipment.Name + sq.Separator;
                        //    break;
                        //case 109:
                        //    if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
                        //        ShortStr += cat.Equipment.Manufacturer + sq.Separator;
                        //    break;
                        //case 110:
                        //    if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "")
                        //        ShortStr += cat.Equipment.Modelno + sq.Separator;
                        //    break;
                        //case 111:
                        //    if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "")
                        //        ShortStr += cat.Equipment.Tagno + sq.Separator;
                        //    break;
                        //case 112:
                        //    if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "")
                        //        ShortStr += cat.Equipment.Serialno + sq.Separator;
                        //    break;
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
                ShortStr = ShortStr.TrimStart(',').Trim();
                int lsIndx = ShortStr.Length;
                string str = ShortStr.Substring(lsIndx - 1, 1);
                if (str == seqList[0].Separator.Trim())
                {
                    ShortStr = ShortStr.Remove(lsIndx - 1);
                }
            }
            return ShortStr;


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
        private string LongDesc(Prosol_Datamaster cat)
        {

            // var vendortype = _VendortypeRepository.FindAll().ToList();
            var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
            var NMList = _nounModifierRepository.FindAll(FormattedQuery).ToList();
            var sort = SortBy.Ascending("Seq").Ascending("Description");
            var query = Query.EQ("Description", "Long");
            string LongStr = "";
            var seqList = _SequenceRepository.FindAll(query, sort).ToList();
            var UOMSet = _UOMRepository.FindOne();
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
                            // else LongStr += cat.Noun + " ";
                            break;
                        case 102:
                            if (NMList[0].Formatted == 1)
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


                                    if (vs.l == 1 && g == 0 && vs.Name != null && vs.Name != "")
                                    {
                                        LongStr += vs.Type + ":" + vs.Name + sq.Separator;
                                        g = 1;
                                    }
                                    if (vs.l == 1 && g == 1 && vs.RefNo != null && vs.RefNo != "")
                                    {
                                        LongStr += vs.Refflag + ":" + vs.RefNo + sq.Separator;
                                    }
                                    else
                                    {
                                        if (vs.l == 1 && vs.RefNo != null && vs.RefNo != "")
                                        {
                                            LongStr += vs.Refflag + ":" + vs.RefNo + sq.Separator;
                                        }





                                    }

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


        public IEnumerable<Prosol_HSNModel> getHSNList(string sKey)
        {

            string[] str = { "HSNID", "Desc" };
            var fields = Fields.Include(str);
            var qry1 = Query.Or(Query.Matches("HSNID", BsonRegularExpression.Create(new Regex(sKey.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))),

                Query.Matches("Desc", BsonRegularExpression.Create(new Regex(sKey.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))));

            var Result = _HSNlistRepository.FindAll(fields, qry1).ToList();
            return Result;
        }
        public int WriteData(Prosol_Datamaster cat, string stus)
        {
            var res = 0;
            foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
            {
                if (vs.RefNo != null && vs.RefNo != "")
                    vs.RefNoDup = Regex.Replace(vs.RefNo, @"[^\w\d]", "");

                if (vs.Name != null && vs.Name != "")
                    vs.Name = vs.Name.Replace(",", "");
            }
            cat.Shortdesc = ShortDesc(cat);
            cat.Longdesc = LongDesc(cat);

            var query = Query.EQ("Itemcode", cat.Itemcode);
            var vn = _DatamasterRepository.FindAll(query).ToList();
            if (vn != null && vn.Count > 0)
            {


                var dupCheck = checkDuplicate(cat);

                if (stus == "Yes" && dupCheck != null && dupCheck.Count > 0)
                {
                    if (stus == "Yes" && (cat.Materialcode == "" || cat.Materialcode == null))
                    {
                        var qy = Query.EQ("Itemcode", cat.Itemcode);
                        _DatamasterRepository.Delete(qy);
                        _erpRepository.Delete(qy);
                        res = 2;
                    }
                    foreach (Prosol_Datamaster dm in dupCheck)
                    {
                        if (stus == "Yes" && dm.Itemcode != cat.Itemcode && cat.Materialcode != "" && cat.Materialcode != null && (dm.Materialcode != null && dm.Materialcode != ""))
                        {

                            cat.Duplicates = dm.Materialcode;
                            cat.Catalogue = vn[0].Catalogue;
                            cat.Review = vn[0].Review;
                            cat.Release = vn[0].Release;

                            if (cat.ItemStatus == 1)
                            {
                                cat.Catalogue.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);


                            }
                            if (cat.ItemStatus == 3)
                            {
                                cat.Review.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                            }
                            if (cat.ItemStatus == 5)
                            {
                                cat.Release.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                            }

                            cat.Rework = vn[0].Rework;
                            cat.Reworkcat = vn[0].Reworkcat;
                            cat.Junk = vn[0].Junk;
                            _DatamasterRepository.Add(cat);
                            res = 1;
                            break;

                        }

                    }

                }
                else if (dupCheck != null && dupCheck.Count > 0)
                {

                    cat.Duplicates = null;
                    cat.Catalogue = vn[0].Catalogue;
                    cat.Review = vn[0].Review;
                    cat.Release = vn[0].Release;
                    cat.PVuser = vn[0].PVuser;

                    if (cat.ItemStatus == 1)
                    {
                        cat.Catalogue.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }
                    if (cat.ItemStatus == 3)
                    {
                        cat.Review.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }
                    if (cat.ItemStatus == 5)
                    {
                        cat.Release.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }
                    if (cat.ItemStatus == 13)
                    {
                        cat.PVuser.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }

                    cat.Rework = vn[0].Rework;
                    cat.Reworkcat = vn[0].Reworkcat;
                    cat.Junk = vn[0].Junk;
                    if (vn[0].Equipments != null && vn[0].Equipments.Count > 0 && cat.Equipments != null)
                    {

                        vn[0].Equipments.Add(cat.Equipments[0]);
                    }
                    _DatamasterRepository.Add(cat);

                }
                else if (dupCheck == null)
                {
                    cat.Duplicates = null;
                    cat.Catalogue = vn[0].Catalogue;
                    cat.Review = vn[0].Review;
                    cat.Release = vn[0].Release;
                    cat.PVuser = vn[0].PVuser;

                    if (cat.ItemStatus == 1)
                    {
                        cat.Catalogue.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }
                    if (cat.ItemStatus == 3)
                    {
                        cat.Review.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }
                    if (cat.ItemStatus == 5)
                    {
                        cat.Release.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }
                    if (cat.ItemStatus == 13)
                    {
                        cat.PVuser.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }


                    cat.Rework = vn[0].Rework;
                    cat.Reworkcat = vn[0].Reworkcat;
                    cat.Junk = vn[0].Junk;

                    if (vn[0].Equipments != null && vn[0].Equipments.Count > 0 && cat.Equipments != null)
                    {
                        vn[0].Equipments.Add(cat.Equipments[0]);
                    }
                    _DatamasterRepository.Add(cat);
                }



            }
            else
            {

                var dupCheck = checkDuplicate(cat);
                if (stus == "Yes" && dupCheck != null && dupCheck.Count > 0)
                {


                    res = 3;
                }
                else if (dupCheck != null && dupCheck.Count > 0)
                {

                    _DatamasterRepository.Add(cat);

                }
                else if (dupCheck == null)
                {

                    if (cat.ItemStatus == 1)
                    {
                        cat.Catalogue.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }
                    if (cat.ItemStatus == 3)
                    {
                        cat.Review.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }
                    if (cat.ItemStatus == 5)
                    {
                        cat.Release.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }

                    _DatamasterRepository.Add(cat);
                }


            }

            // var queryyyy = Query.And(Query.EQ("Catalogue.UserId", "10"), Query.EQ("ItemStatus", 0));
            // var reeee = _DatamasterRepository.FindAll(queryyyy);
            // int vallll = reeee.Count();


            //// Update

            // foreach (Prosol_Datamaster pd in reeee)
            // {
            //  pd.Review = null;
            // pd.Release = null;
            //  pd.Review.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //pd.Release.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //pd.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //pd.ItemStatus = 6;

            //    pd.Catalogue.Name = "KHAMIZ";

            //    _DatamasterRepository.Add(pd);
            //}

            // Delete

            //foreach(Prosol_Datamaster pd in reeee)
            //{
            //    var queryyy = Query.EQ("Itemcode", pd.Itemcode);
            //    _DatamasterRepository.Delete(queryyy);
            //    _erpRepository.Delete(queryyy);
            //}

            return res;
        }

        public bool WriteERPInfo(Prosol_ERPInfo erp)
        {
            var Qry = Query.EQ("Itemcode", erp.Itemcode);
            var resmdl = _erpRepository.FindAll(Qry).ToList();
            if (resmdl != null && resmdl.Count > 0)
            {
                erp._id = resmdl[0]._id;
                var res = _erpRepository.Add(erp);
                return res;

            }
            else
            {
                var res = _erpRepository.Add(erp);
                return res;

            }


        }
        //apimodel


        //public bool writeapimodel(Prosol_ApiModel apiml)
        //{
        //    var Qry = Query.EQ("Id", apiml.Id);
        //    var resmdl = _apimodel.FindAll(Qry).ToList();
        //    if (resmdl != null && resmdl.Count > 0)
        //    {
        //        apiml.Id = resmdl[0].Id;
        //        var res = _apimodel.Add(apiml);
        //        return res;

        //    }
        //    else
        //    {
        //        var res = _apimodel.Add(apiml);
        //        return res;

        //    }


        //}



        public int WriteAttachment(List<Prosol_Attachment> lisAttachment, HttpFileCollectionBase files)
        {
            int i = 0;
            foreach (Prosol_Attachment atmnt in lisAttachment)
            {
                if (atmnt.FileName != null)
                {
                    if (files[i] != null && files[i].ContentLength > 0)
                    {
                        atmnt.ContentType = files[i].ContentType;
                        atmnt.FileId = _attchmentRepository.GridFsUpload(files[i].InputStream, atmnt.FileName);
                        i++;
                    }

                }
            }
            int res = _attchmentRepository.Add(lisAttachment);
            return res;
        }
        public int WriteDataList(List<Prosol_Datamaster> catList)
        {
            int cont = 0;
            bool res = false;
            foreach (Prosol_Datamaster mdl in catList)
            {
                var query = Query.And(Query.EQ("Itemcode", mdl.Itemcode), Query.EQ("Noun", mdl.Noun), Query.EQ("Modifier", mdl.Modifier));
                var oneResult = _DatamasterRepository.FindOne(query);

                var cataloguer = new Prosol_UpdatedBy();
                cataloguer.UserId = oneResult.Catalogue.UserId;
                cataloguer.Name = oneResult.Catalogue.Name;
                cataloguer.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                oneResult.Materialcode = mdl.Materialcode;

                oneResult.Catalogue = cataloguer;
                oneResult.Review = mdl.Review;
                oneResult.UpdatedOn = mdl.UpdatedOn;
                oneResult.ItemStatus = mdl.ItemStatus;
                //  oneResult.Logic = mdl.Logic;
                if (oneResult.ItemStatus == 2)
                {
                    res = _DatamasterRepository.Add(oneResult);

                    var Qry1 = Query.EQ("Equipments.Itemcode", oneResult.Itemcode);
                    var MsLst = _DatamasterRepository.FindAll(Qry1).ToList();
                    if (MsLst != null && MsLst.Count > 0)
                    {
                        foreach (Prosol_Datamaster dml in MsLst)
                        {
                            dml.ItemStatus = mdl.ItemStatus;
                            dml.Catalogue = cataloguer;
                            dml.Review = mdl.Review;
                            dml.UpdatedOn = mdl.UpdatedOn;
                            // dml.Logic = mdl.Logic;
                            _DatamasterRepository.Add(dml);
                        }

                    }

                }

                if (oneResult.ItemStatus == 6)
                {
                    int flg = 0;
                    //res = _DatamasterRepository.Add(oneResult);
                    // flg = 1;



                    var fQry = Query.EQ("FunctLocation", mdl.Junk);
                    var fobj = _FunlocRepository.FindOne(fQry);
                    if (fobj != null)
                    {

                        res = _DatamasterRepository.Add(oneResult);
                        flg = 1;
                        // Funcational location
                        fobj.ConID = oneResult.Materialcode;
                        _FunlocRepository.Add(fobj);


                        //Equipment bom

                        var eObj = new Prosol_equipbom();

                        eObj.Itemcode = oneResult.Materialcode;

                        var delQry = Query.And(Query.EQ("Itemcode", eObj.Itemcode), Query.EQ("TechIdentNo", fobj.TechIdentNo));
                        _EquipmentRepository.Delete(delQry);

                        eObj.Shortdesc = oneResult.Shortdesc;
                        eObj.Longdesc = oneResult.Longdesc;
                        eObj.partqnt = "1";
                        eObj.itemcat = "I";

                        eObj.FunctLocation = mdl.Junk;

                        eObj.FunctDesc = fobj.FunctDesc;
                        eObj.SupFunctLoc = fobj.SupFunctLoc;
                        eObj.Objecttype = fobj.Objecttype;
                        eObj.TechIdentNo = fobj.TechIdentNo;
                        eObj.EquipDesc = fobj.EquipDesc;
                        eObj.Manufacturer = fobj.Manufacturer;
                        eObj.ManufCon = fobj.ManufCon;
                        eObj.Modelno = fobj.Modelno;
                        eObj.ManufSerialNo = fobj.ManufSerialNo;
                        eObj.ABCindic = fobj.ABCindic;

                        _EquipmentRepository.Add(eObj);

                    }
                    if (flg == 1)
                    {
                        var Qry1 = Query.EQ("Equipments.Itemcode", oneResult.Itemcode);
                        var MsLst = _DatamasterRepository.FindAll(Qry1).ToList();
                        if (MsLst != null && MsLst.Count > 0)
                        {

                            var cdeLgc = _CodeLogic.FindOne();
                            foreach (Prosol_Datamaster dml in MsLst)
                            {

                                if (cdeLgc.CODELOGIC == "Customized Code")
                                {
                                    //  string Lcode = proCat.Maincode + proCat.Subcode + proCat.Subsubcode;
                                    // if (dml.Materialcode == null || dml.Materialcode == "")

                                    // dml.Materialcode = getMaterialCode(proCat.Maincode + dml.Subcode + dml.Subsubcode, 1);

                                }
                                else if (cdeLgc.CODELOGIC == "UNSPSC Code")
                                {
                                    if (dml.Materialcode == null || dml.Materialcode == "")
                                        dml.Materialcode = getMaterialCode(dml.Unspsc);
                                }
                                else
                                {
                                    dml.Materialcode = dml.Itemcode;
                                }
                                dml.ItemStatus = mdl.ItemStatus;
                                dml.Catalogue = cataloguer;
                                dml.Review = mdl.Review;
                                dml.UpdatedOn = mdl.UpdatedOn;
                                //   dml.Logic = mdl.Logic;
                                _DatamasterRepository.Add(dml);


                                //Material Bom

                                var bomObj = new Prosol_MaterialBom();
                                bomObj.HeaderBID = mdl.Materialcode;

                                bomObj.Noun = dml.Noun;
                                bomObj.Modifier = dml.Modifier;
                                bomObj.Shortdesc = dml.Shortdesc;
                                bomObj.Longdesc = dml.Longdesc;
                                bomObj.itemcat = "L";
                                bomObj.Itemcode = dml.Materialcode;
                                if (dml.Vendorsuppliers != null && dml.Vendorsuppliers.Count > 0)
                                {
                                    foreach (Vendorsuppliers vn in dml.Vendorsuppliers)
                                    {
                                        if (vn.Refflag == "PART NUMBER")
                                            bomObj.Partno = vn.RefNo;
                                    }
                                }

                                if (dml.Equipments != null && dml.Equipments.Count > 0)
                                {
                                    foreach (EquList eq in dml.Equipments)
                                    {
                                        if (eq.Itemcode == mdl.Itemcode)
                                            bomObj.partqnt = Convert.ToString(eq.PartQty);
                                    }
                                }
                                var delSpQry = Query.And(Query.EQ("HeaderBID", bomObj.HeaderBID), Query.EQ("Itemcode", bomObj.Itemcode));
                                _matbomRepository.Delete(delSpQry);
                                _matbomRepository.Add(bomObj);



                            }

                        }
                    }
                }

                if (res) cont++;

            }
            return cont;
        }
        public int RemoveEquipment(List<Prosol_Datamaster> catList)
        {
            int cont = 0;
            bool res = false;
            foreach (Prosol_Datamaster mdl in catList)
            {
                var query = Query.EQ("Itemcode", mdl.Itemcode);
                var oneResult = _DatamasterRepository.FindOne(query);
                var Qry1 = Query.EQ("Equipments.Itemcode", oneResult.Itemcode);
                var MsLst = _DatamasterRepository.FindAll(Qry1).ToList();
                if (MsLst != null && MsLst.Count > 0)
                {
                    foreach (Prosol_Datamaster dml in MsLst)
                    {
                        if (dml.Equipments != null && dml.Equipments.Count == 1 && dml.ItemStatus != 6)
                        {
                            var qrr = Query.EQ("Itemcode", dml.Itemcode);
                            _DatamasterRepository.Delete(qrr);
                        }
                        if (dml.Equipments != null && dml.Equipments.Count > 1)
                        {
                            var lst = new List<EquList>();
                            foreach (EquList ls in dml.Equipments)
                            {
                                if (ls.Itemcode != mdl.Itemcode)
                                    lst.Add(ls);
                            }
                            dml.Equipments = lst;
                            _DatamasterRepository.Add(dml);
                        }
                    }

                }
                res = _DatamasterRepository.Delete(query);

                if (res) cont++;

            }
            return cont;
        }

        public int CopyEquip(List<Prosol_Datamaster> catList)
        {
            int cont = 0;
            bool res = false;
            foreach (Prosol_Datamaster mdl in catList)
            {
                var query = Query.EQ("Itemcode", mdl.Itemcode);
                var oneResult = _DatamasterRepository.FindOne(query);
                oneResult._id = new MongoDB.Bson.ObjectId();
                oneResult.Itemcode = generateRequest();
                res = _DatamasterRepository.Add(oneResult);

                if (res) cont++;

            }
            return cont;
        }

        public int RemoveSpare(List<Prosol_Datamaster> catList, string equip)
        {
            int cont = 0;
            bool res = false;
            foreach (Prosol_Datamaster mdl in catList)
            {
                var query = Query.EQ("Itemcode", mdl.Itemcode);
                var dml = _DatamasterRepository.FindOne(query);

                if (dml != null)
                {

                    if (dml.Equipments != null && dml.Equipments.Count == 1 && dml.ItemStatus != 6)
                    {
                        var qrr = Query.EQ("Itemcode", dml.Itemcode);
                        res = _DatamasterRepository.Delete(qrr);
                    }
                    if (dml.Equipments != null && dml.Equipments.Count > 0)
                    {
                        var lst = new List<EquList>();
                        foreach (EquList ls in dml.Equipments)
                        {
                            if (ls.Itemcode != equip)
                                lst.Add(ls);
                        }
                        dml.Equipments = lst;
                        res = _DatamasterRepository.Add(dml);
                    }


                }


                if (res) cont++;

            }
            return cont;
        }

        public int CopySpare(List<Prosol_Datamaster> catList, string equip)
        {
            int cont = 0;
            bool res = false;
            foreach (Prosol_Datamaster mdl in catList)
            {
                var query = Query.EQ("Itemcode", mdl.Itemcode);
                var dml = _DatamasterRepository.FindOne(query);
                dml._id = new MongoDB.Bson.ObjectId();
                dml.Itemcode = generateRequest();

                if (dml.Equipments != null && dml.Equipments.Count == 1 && dml.ItemStatus != 6)
                {
                    res = _DatamasterRepository.Add(dml);
                }
                if (dml.Equipments != null && dml.Equipments.Count > 1)
                {
                    var lst = new List<EquList>();
                    foreach (EquList ls in dml.Equipments)
                    {
                        if (ls.Itemcode == equip)
                            lst.Add(ls);
                    }
                    dml.Equipments = lst;
                    res = _DatamasterRepository.Add(dml);
                }


                if (res) cont++;

            }
            return cont;
        }
        private string generateRequest()
        {
            string itmCode = "";
            var ICode = getItem();

            if (ICode != "")
            {


                long serialinr = Convert.ToInt64(ICode);
                serialinr++;
                string addincr = "";
                switch (serialinr.ToString().Length)
                {
                    case 1:
                        addincr = "0000" + serialinr;
                        break;
                    case 2:
                        addincr = "000" + serialinr;
                        break;
                    case 3:
                        addincr = "00" + serialinr;
                        break;
                    case 4:
                        addincr = "0" + serialinr;
                        break;
                    default:
                        addincr = serialinr.ToString();
                        break;

                }
                itmCode = addincr;

            }
            else
            {
                itmCode = "00001";
            }
            return itmCode;
        }
        public string PotentialEquip(List<Prosol_Datamaster> catList)
        {
            string dupList = "";
            foreach (Prosol_Datamaster mdl in catList)
            {

                var query = Query.EQ("Itemcode", mdl.Itemcode);
                var oneResult = _DatamasterRepository.FindOne(query);
                if (oneResult != null)
                {
                    var dupLst = checkDuplicate(oneResult);
                    if (dupLst != null && dupLst.Count > 0)
                    {
                        dupList = dupList + "Duplicate codes for " + mdl.Itemcode + " : ";
                        foreach (Prosol_Datamaster dd in dupLst)
                        {
                            dupList = dupList + dd.Itemcode + ",";
                        }
                    }
                }



            }
            return dupList;
        }
        public List<Prosol_Funloc> GetFunLocList(string srch)
        {
            var Qry = Query.Or(Query.EQ("FunctLocation", srch), Query.EQ("Modelno", srch), Query.EQ("ManufSerialNo", srch));
            var fLst = _FunlocRepository.FindAll(Qry).ToList();
            return fLst;
        }
        public int AddexistSpares(List<Prosol_Datamaster> catList)
        {
            int cont = 0;
            bool res = false;
            foreach (Prosol_Datamaster mdl in catList)
            {
                int flg = 0;
                var query = Query.EQ("Itemcode", mdl.Itemcode);
                var obj = _DatamasterRepository.FindOne(query);
                if (obj != null)
                {
                    if (obj.Equipments != null)
                    {
                        foreach (EquList dm in obj.Equipments)
                        {
                            if (dm.Itemcode == mdl.Equipments[0].Itemcode)
                            {

                                dm.PartQty = mdl.Equipments[0].PartQty;
                                flg = 1;
                            }
                        }

                        if (flg == 0)
                            obj.Equipments.Add(mdl.Equipments[0]);


                        if (obj.category == null)
                            obj.category = "Spare";
                    }
                    else
                    {
                        obj.Equipments = mdl.Equipments;
                        obj.category = "Spare";
                    }

                }
                res = _DatamasterRepository.Add(obj);
                if (res) cont++;

            }
            return cont;
        }
        public int WriteRDataList(List<Prosol_Datamaster> catList)
        {
            int cont = 0;
            bool res = false;
            foreach (Prosol_Datamaster mdl in catList)
            {
                var query = Query.And(Query.EQ("Itemcode", mdl.Itemcode), Query.EQ("Noun", mdl.Noun), Query.EQ("Modifier", mdl.Modifier));
                var oneResult = _DatamasterRepository.FindOne(query);


                var Review = new Prosol_UpdatedBy();
                Review.UserId = oneResult.Review.UserId;
                Review.Name = oneResult.Review.Name;
                Review.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);


                oneResult.Review = Review;

                oneResult.Release = mdl.Release;
                oneResult.UpdatedOn = mdl.UpdatedOn;
                oneResult.ItemStatus = mdl.ItemStatus;

                res = _DatamasterRepository.Add(oneResult);
                if (res) cont++;

            }
            return cont;
        }
        public int WriteReleaseDataList(List<Prosol_Datamaster> catList)
        {
            int cont = 0;
            bool res = false;
            foreach (Prosol_Datamaster mdl in catList)
            {
                var query = Query.And(Query.EQ("Itemcode", mdl.Itemcode), Query.EQ("Noun", mdl.Noun), Query.EQ("Modifier", mdl.Modifier));
                var oneResult = _DatamasterRepository.FindOne(query);

                var Release = new Prosol_UpdatedBy();
                Release.UserId = oneResult.Release.UserId;
                Release.Name = oneResult.Release.Name;
                Release.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                oneResult.Release = Release;

                oneResult.UpdatedOn = mdl.UpdatedOn;
                oneResult.ItemStatus = mdl.ItemStatus;
                oneResult.Materialcode = mdl.Materialcode;

                res = _DatamasterRepository.Add(oneResult);
                if (res) cont++;

            }
            return cont;
        }
        public bool UpdateData(List<Prosol_Datamaster> Listcat)
        {
            var res = false;

            var qryList = new List<IMongoQuery>();
            foreach (Prosol_Datamaster cat in Listcat)
            {
                if (cat.Characteristics.Count > 0)
                {
                    var query = Query.And(Query.EQ("Itemcode", cat.Itemcode), Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
                    var vn = _DatamasterRepository.FindAll(query).ToList();
                    if (vn != null)
                    {
                        if (vn.Count == 1)
                        {
                            vn[0].Characteristics = cat.Characteristics;
                            res = _DatamasterRepository.Add(vn[0]);
                        }
                    }
                }
            }
            return res;
        }

        public IEnumerable<Prosol_Datamaster> GetDataList(int sts, int sts1, string uId)
        {
            var sort = SortBy.Ascending("ItemStatus").Descending("UpdatedOn");
            //var query = Query.And(Query.EQ("Catalogue.UserId", uId),  Query.NE("ItemStatus", 6));
            //var arrResult = _DatamasterRepository.FindAll(query, sort);
            //return arrResult;

            var query =
                Query.Or(
                Query.And(Query.EQ("Catalogue.UserId", uId), Query.EQ("Review.UserId", uId), Query.NE("ItemStatus", 6)),
                Query.And(Query.EQ("Catalogue.UserId", uId), Query.LT("ItemStatus", 2)),
                 Query.And(Query.EQ("Review.UserId", uId), Query.GT("ItemStatus", 1), Query.NE("ItemStatus", 6))
                );
            var arrResult = _DatamasterRepository.FindAll(query, sort).ToList();
            if (arrResult != null && arrResult.Count > 0)
            {
                return arrResult;
            }
            else
            {
                return arrResult;
            }

            //var query = Query.And(Query.And(Query.EQ("Catalogue.UserId", uId), Query.EQ("Review.UserId", uId)), Query.NE("ItemStatus", 6));
            //var arrResult = _DatamasterRepository.FindAll(query, sort).ToList();
            //if (arrResult != null && arrResult.Count > 0)
            //{
            //    return arrResult;
            //}
            //else
            //{

            //    query = Query.And(Query.EQ("Catalogue.UserId", uId), Query.NE("ItemStatus", 6));
            //    arrResult = _DatamasterRepository.FindAll(query, sort).ToList();
            //    if (arrResult != null && arrResult.Count > 0)
            //    {
            //        return arrResult;
            //    }
            //    else
            //    {
            //        query = Query.And(Query.EQ("Review.UserId", uId), Query.NE("ItemStatus", 6));
            //        arrResult = _DatamasterRepository.FindAll(query, sort).ToList();
            //        return arrResult;
            //    }
            //}

        }
        public IEnumerable<Prosol_Datamaster> GetSparesList(string itmcde)
        {
            var Qry1 = Query.EQ("Equipments.Itemcode", itmcde);
            var MsLst = _DatamasterRepository.FindAll(Qry1).ToList();
            return MsLst;

        }
        public IEnumerable<Prosol_Datamaster> FetchExist(string srchcde)
        {
            if (srchcde.Contains(','))
            {
                var MsLst = new List<Prosol_Datamaster>();
                string[] spt = srchcde.Split(',');
                foreach (string str in spt)
                {
                    if (str != "")
                    {
                        var Qry1 = Query.And(Query.EQ("ItemStatus", 6), Query.Or(Query.EQ("Vendorsuppliers.RefNo", srchcde), Query.EQ("Materialcode", srchcde), Query.EQ("Itemcode", srchcde)));
                        var obj = _DatamasterRepository.FindOne(Qry1);
                        if (obj != null)
                            MsLst.Add(obj);

                    }
                }
                return MsLst;

            }
            else
            {
                var Qry1 = Query.And(Query.EQ("ItemStatus", 6), Query.Or(Query.EQ("Vendorsuppliers.RefNo", srchcde), Query.EQ("Materialcode", srchcde), Query.EQ("Itemcode", srchcde)));
                var MsLst = _DatamasterRepository.FindAll(Qry1).ToList();
                return MsLst;
            }


        }
        public IEnumerable<Prosol_Datamaster> GetRDataList(int sts, int sts1, string uId)
        {
            var sort = SortBy.Ascending("ItemStatus").Descending("UpdatedOn");
            var query = Query.And(Query.EQ("Review.UserId", uId), Query.Or(Query.EQ("ItemStatus", sts), Query.EQ("ItemStatus", sts1)));
            var arrResult = _DatamasterRepository.FindAll(query, sort);
            return arrResult;

        }
        public IEnumerable<Prosol_Datamaster> GetReleaseDataList(int sts, int sts1, string uId)
        {
            var sort = SortBy.Ascending("ItemStatus").Descending("UpdatedOn");
            var query = Query.And(Query.EQ("Release.UserId", uId), Query.Or(Query.EQ("ItemStatus", sts), Query.EQ("ItemStatus", sts1)));
            var arrResult = _DatamasterRepository.FindAll(query, sort);
            return arrResult;

        }
        //pvuser
        public IEnumerable<Prosol_Datamaster> GetDataListpv(string Pending, string uId)
        {
            var sort = SortBy.Ascending("PVstatus").Descending("UpdatedOn");
            var query = Query.And(Query.EQ("PVuser.UserId", uId), Query.EQ("PVstatus", Pending));
            var arrResult = _DatamasterRepository.FindAll(query, sort);
            return arrResult;

        }
        public IEnumerable<Prosol_Datamaster> GetTotalItem()
        {
            var query = Query.And(Query.NE("Noun", BsonNull.Value), Query.NE("Noun", ""));
            var arrResult = _DatamasterRepository.FindAll(query);
            return arrResult;

        }
        public Prosol_Datamaster GetSingleItem(string Itemcode)
        {

            //****   vendor    ***//

            //var resven = _VendorRepository.FindAll();

            //foreach(Prosol_Vendor pv in resven)
            //{
            //    if(pv.ShortDescName != null)
            //    pv.ShortDescName = pv.ShortDescName.Trim();
            //    if (pv.Name != null)
            //        pv.Name = pv.Name.Trim();

            //    _VendorRepository.Add(pv);
            //}



            //****   Datamaster    ***//

            //var qry = Query.Or(Query.EQ("Noun", "BOLT"), Query.EQ("Modifier","HEXAGON HEAD"));
            //var resu = _DatamasterRepository.FindAll(qry);


            //foreach (Prosol_Datamaster pe in resu)
            //{
            //    //var qry1 = Query.EQ("Itemcode", pe.Itemcode);
            //    //_DatamasterRepository.Delete(qry1);
            //    //_erpRepository.Delete(qry1);
            //    //pe.ItemStatus = 6;
            //    //pe.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //    pe.Additionalinfo = "";
            //    _DatamasterRepository.Add(pe);
            //}
            // _DatamasterRepository.add

            // var query = Query.And(Query.EQ("Itemcode", Itemcode), Query.EQ("Catalogue.UserId", uId), Query.Or(Query.EQ("ItemStatus", sts), Query.EQ("ItemStatus", sts1)));
            var query = Query.EQ("Itemcode", Itemcode);
            var arrResult = _DatamasterRepository.FindOne(query);
            return arrResult;

        }
        public Prosol_Datamaster GetRSingleItem(string Itemcode)
        {
            var query = Query.EQ("Itemcode", Itemcode);
            var arrResult = _DatamasterRepository.FindOne(query);
            return arrResult;

        }
        public Prosol_Datamaster GetReleaseSingleItem(string Itemcode)
        {
            var query = Query.EQ("Itemcode", Itemcode);
            var arrResult = _DatamasterRepository.FindOne(query);
            return arrResult;

        }

        public Prosol_ERPInfo GetERPInfo(string Itemcode)
        {
            var query = Query.EQ("Itemcode", Itemcode);
            var genRes = _erpRepository.FindOne(query);
            return genRes;

        }
        public List<Prosol_Datamaster> GetEquip(string EName)
        {
            string[] strArr = { "Equipment.Name" };
            var fields = Fields.Include(strArr).Exclude("_id");

            //  var query = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.EQ("Characteristic", Attribute));
            //  var query = Query.Matches("Equipment.Name", EName);
            var Lstobj = _DatamasterRepository.FindAll(fields).Distinct().ToList();

            return Lstobj;
        }

        //public Prosol_Generalinfo GetGeneralinfo(string Itemcode)
        //{
        //    var query = Query.EQ("Itemcode", Itemcode);
        //    var genRes = _generalinfoRepository.FindOne(query);
        //    return genRes;

        //}
        //public Prosol_Plantinfo GetPlantinfo(string Itemcode)
        //{
        //    var query = Query.EQ("Itemcode", Itemcode);
        //    var plantRes = _plantinfoRepository.FindOne(query);
        //    return plantRes;

        //}
        //public Prosol_MRPdata GetMRPdata(string Itemcode)
        //{
        //    var query = Query.EQ("Itemcode", Itemcode);
        //    var mrpRes = _mrpdataRepository.FindOne(query);
        //    return mrpRes;

        //}
        //public Prosol_Salesinfo GetSalesinfo(string Itemcode)
        //{
        //    var query = Query.EQ("Itemcode", Itemcode);
        //    var salesRes = _salesinfoRepository.FindOne(query);
        //    return salesRes;

        //}
        public IEnumerable<Prosol_Attachment> GetAttachment(string Itemcode)
        {
            var query = Query.EQ("Itemcode", Itemcode);
            var attres = _attchmentRepository.FindAll(query);
            return attres;

        }
        public IEnumerable<Prosol_Users> getReviewerList()
        {
            var query = Query.And(Query.EQ("Islive", "Active"), Query.EQ("Usertype", "Reviewer"));
            var shwusr = _usersRepository.FindAll(query);
            return shwusr;
        }
        public IEnumerable<Prosol_Users> getReleaserList()
        {
            var query = Query.And(Query.EQ("Islive", "Active"), Query.EQ("Usertype", "Releaser"));
            var shwusr = _usersRepository.FindAll(query);
            return shwusr;
        }
        public bool Deletefile(string id, string imgId)
        {


            if (imgId != null)
                _attchmentRepository.GridFsDel(Query.EQ("_id", new ObjectId(imgId)));
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _attchmentRepository.Delete(query);
            return res;

        }

        // getting file name to delete file from folder // bellsha
        public string GetDeletefile(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _attchmentRepository.FindOne(query);
            return res.FileName;

        }

        //  var query = Query.Matches("Materialcode", BsonRegularExpression.Create(new Regex(string.Format("^{0}", LogicCode.TrimStart().TrimEnd()), RegexOptions.IgnoreCase)));

        public string getlastinsertedfilename(string Itemcode)
        {
            var sort = SortBy.Descending("Date_ts");
            var query = Query.And(Query.EQ("Itemcode", Itemcode), Query.Or(Query.EQ("ContentType", "image/jpeg"), Query.EQ("ContentType", "image/png"), Query.EQ("ContentType", "image/JPG"), Query.EQ("ContentType", "image/JPEG"), Query.EQ("ContentType", "image/PNG")));
            var res = _attchmentRepository.FindAll(query, sort).ToList();
            // var ressss = res;
            string ddd = "";
            foreach (Prosol_Attachment pa in res)
            {
                if (pa.FileName.Contains(".jpeg") || pa.FileName.Contains(".JPEG") || pa.FileName.Contains(".JPG") || pa.FileName.Contains(".jpg") || pa.FileName.Contains(".png") || pa.FileName.Contains(".PNG"))
                {
                    ddd = pa.FileName;
                    break;
                }
            }

            if (ddd != "")
                return ddd;
            else
                return "1";

        }

        public string getlastinsertedpdfname(string Itemcode)
        {
            var sort = SortBy.Descending("Date_ts");
            var query = Query.EQ("Itemcode", Itemcode);
            var res = _attchmentRepository.FindAll(query, sort).ToList();
            // var ressss = res;
            string ddd = "";
            foreach (Prosol_Attachment pa in res)
            {
                if (pa.FileName.Contains(".pdf") || pa.FileName.Contains(".PDF"))
                {
                    ddd = pa.FileName;
                    break;
                }
            }

            if (ddd != "")
                return ddd;
            else
                return "1";

        }




        public string getsm_value(string value1)
        {
            string resval = "";

            var qwe = Query.EQ("Value", value1);
            var value = _abbreivateRepository.FindOne(qwe);

            if (value != null)
            {
                if (value.Abbrevated == "" || value.Abbrevated == null)
                    resval = value1;
                else
                    resval = value.Abbrevated;

            }
            else
            {

                if (value1.Contains(" "))
                {
                    string[] strArr = null;
                    char[] splitchar = { ' ' };
                    strArr = value1.Split(splitchar);


                    foreach (string strrr in strArr)
                    {
                        qwe = Query.EQ("Value", strrr);

                        value = _abbreivateRepository.FindOne(qwe);

                        if (value != null)
                        {
                            if (value.Abbrevated == "")
                                resval = resval + " " + strrr;
                            else
                                resval = resval + " " + value.Abbrevated;
                        }
                        else
                        {
                            resval = resval + " " + strrr;
                        }

                    }
                }
                else
                {
                    qwe = Query.EQ("Value", value1);
                    value = _abbreivateRepository.FindOne(qwe);

                    if (value != null)
                    {
                        if (value.Abbrevated == "")
                            resval = value1;
                        else
                            resval = resval + " " + value.Abbrevated;

                        //  resval = value.Abbrevated;
                    }
                    else
                    {
                        resval = value1;
                    }

                }
            }

            return resval;
        }


        public byte[] Downloadfile(string imgId)
        {
            if (imgId != null)
            {
                var query1 = Query.EQ("_id", new ObjectId(imgId));
                byte[] byt = _attchmentRepository.GridFsFindOne(query1);
                return byt;
                // if (byt != null)
                // String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(byt));              
            }
            else return null;
        }

        public List<Prosol_Attachment> findattachmentss(string str)
        {
            var queryyy = Query.EQ("Itemcode", str);
            return _attchmentRepository.FindAll(queryyy).ToList();
        }

        //public List<Prosol_Datamaster> checkPartno(string Partno, string icode)
        //{
        //    string result = Partno != null ? Regex.Replace(Partno, @"[^\w\d]", "") : "";
        //    var query = Query.And(Query.And(Query.EQ("Partnodup", result), Query.NE("Partnodup", "")), Query.NE("Itemcode", icode));
        //    var vn = _DatamasterRepository.FindAll(query).ToList();
        //    if (vn != null)
        //    {
        //        if (vn.Count > 0)
        //        {
        //            return vn;
        //        }
        //        else return null;
        //    }
        //    else return null;
        //}

        public List<Prosol_Datamaster> checkPartno(string Partno, string icode, string Flag)
        {
            string result = Partno != null ? Regex.Replace(Partno, @"[^\w\d]", "") : "";
            var query = Query.And(Query.And(Query.EQ("Vendorsuppliers.Refflag", Flag), Query.EQ("Vendorsuppliers.RefNoDup", result), Query.NE("Vendorsuppliers.RefNoDup", ""), Query.NE("Vendorsuppliers.Refflag", "SERIAL NUMBER"), Query.NE("Vendorsuppliers.Refflag", "TAG NUMBER")), Query.NE("Itemcode", icode));
            var vn = _DatamasterRepository.FindAll(query).ToList();
            if (vn != null)
            {
                if (vn.Count > 0)
                {
                    return vn;
                }
                else return null;
            }
            else return null;
        }
        public String[] GenerateShortLong(Prosol_Datamaster cat)
        {
            String[] strArr = { "", "" };
            strArr[0] = ShortDesc(cat);
            strArr[1] = LongDesc(cat);
            return strArr;
        }


        public List<Prosol_Datamaster> checkDuplicate(Prosol_Datamaster cat)
        {
            foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
            {
                if (vs.RefNo != null && vs.RefNo != "")
                    vs.RefNoDup = Regex.Replace(vs.RefNo, @"[^\w\d]", "");

                if (vs.Name != null && vs.Name != "")
                    vs.Name = vs.Name.Replace(",", "");
            }
            cat.Shortdesc = ShortDesc(cat);
            // cat.Longdesc = LongDesc(cat);
            // cat.Partnodup = cat.Partno != null ? Regex.Replace(cat.Partno, @"[^\w\d]", "") : "";
            int flg = 0;
            var dupList = new List<Prosol_Datamaster>();
            if (cat.Vendorsuppliers != null)
            {
                foreach (Vendorsuppliers vsup in cat.Vendorsuppliers)
                {
                    if (vsup.Refflag != null && vsup.Refflag != "" && vsup.Refflag != "DRAWING NUMBER" && vsup.Refflag != "POSITION NUMBER" && vsup.Refflag != "SERIAL NUMBER" && vsup.Refflag != "TAG NUMBER")
                    {

                        if (vsup.RefNo != null && vsup.RefNo != "" && vsup.Refflag != null && vsup.RefNoDup != null && vsup.Refflag != "")
                        {
                            flg = 1;
                            //  var query = Query.And(Query.EQ("Vendorsuppliers.Refflag", vsup.Refflag), Query.EQ("Vendorsuppliers.RefNo", vsup.RefNo));
                            // var query = Query.And(Query.ElemMatch("Vendorsuppliers", Query.And(Query.EQ("Refflag", vsup.Refflag), Query.EQ("RefNo", vsup.RefNo))));


                            // var query = Query.Matches("Vendorsuppliers.RefNo", BsonRegularExpression.Create(new Regex(stringFormat("^{0}", temp), RegexOptions.IgnoreCase)));
                            var query = Query.EQ("Vendorsuppliers.RefNoDup", vsup.RefNoDup);
                            var vn = _DatamasterRepository.FindAll(query).ToList();
                            if (vn != null && vn.Count > 1)
                            {
                                foreach (Prosol_Datamaster dm in vn)
                                {
                                    if (dm.Itemcode != cat.Itemcode && -1 == dupList.FindIndex(f => f.Itemcode.Equals(dm.Itemcode)))
                                    {
                                        dupList.Add(dm);

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
                    var Lst = _DatamasterRepository.FindAll(ary).ToList();
                    if (Lst != null && Lst.Count > 1)
                    {
                        foreach (Prosol_Datamaster dm in Lst)
                        {

                            if (dm.Itemcode != cat.Itemcode && -1 == dupList.FindIndex(f => f.Itemcode.Equals(dm.Itemcode)))
                            {
                                dupList.Add(dm);

                            }


                        }
                    }
                }

                var query = Query.And(Query.EQ("Shortdesc", cat.Shortdesc), Query.EQ("Noun", cat.Noun));
                var vn = _DatamasterRepository.FindAll(query).ToList();
                if (vn != null && vn.Count > 1)
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
                            if (cat.Vendorsuppliers != null && dm.Vendorsuppliers != null)
                            {
                                var newList = new List<Vendorsuppliers>();
                                foreach (Vendorsuppliers vsup in dm.Vendorsuppliers)
                                {
                                    if (vsup.RefNoDup != "" && vsup.RefNoDup != null)
                                        newList.Add(vsup);

                                }
                                var newList1 = new List<Vendorsuppliers>();
                                foreach (Vendorsuppliers mdl in cat.Vendorsuppliers)
                                {
                                    if (mdl.RefNoDup != "" && mdl.RefNoDup != null)
                                        newList1.Add(mdl);

                                }

                                if (newList.Count > 0 && newList1.Count > 0)
                                {
                                    foreach (Vendorsuppliers vsup in newList)


                                    {
                                        foreach (Vendorsuppliers mdl in newList1)
                                        {
                                            if (vsup.RefNoDup != null && vsup.RefNoDup != "" && mdl.RefNoDup != null && mdl.RefNoDup != vsup.RefNoDup)
                                            {



                                                ind = 1;






                                            }
                                            //if (vsup.RefNoDup == null && (mdl.RefNoDup != null && mdl.RefNoDup != ""))
                                            //{

                                            //    ind = 1;


                                            //}
                                            //if (vsup.RefNoDup != null && (mdl.RefNoDup == null && mdl.RefNoDup == ""))
                                            //{

                                            //    ind = 1;


                                            //}

                                        }
                                    }
                                }
                                if (newList.Count > 0 && newList1.Count == 0)



                                {
                                    ind = 1;
                                }
                                if (newList.Count == 0 && newList1.Count > 0)
                                {
                                    ind = 1;
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
                                if (dm.Itemcode != cat.Itemcode && -1 == dupList.FindIndex(f => f.Itemcode.Equals(dm.Itemcode)))
                                {
                                    dupList.Add(dm);




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

        }

        //public List<Prosol_Datamaster> checkDuplicate(Prosol_Datamaster cat)
        //{
        //    cat.Shortdesc = ShortDesc(cat);
        //    // cat.Longdesc = LongDesc(cat);
        //    // cat.Partnodup = cat.Partno != null ? Regex.Replace(cat.Partno, @"[^\w\d]", "") : "";
        //    int flg = 0;
        //    var dupList = new List<Prosol_Datamaster>();
        //    if (cat.Vendorsuppliers != null)
        //    {
        //        foreach (Vendorsuppliers vsup in cat.Vendorsuppliers)
        //        {
        //            if (vsup.Refflag != null && vsup.Refflag != "" && vsup.Refflag != "DRAWING & POSITION NUMBER")
        //            {

        //                if (vsup.RefNo != null && vsup.RefNo != "" && vsup.Refflag !=null && vsup.Refflag != "")
        //                {
        //                    flg = 1;
        //                    //  var query = Query.And(Query.EQ("Vendorsuppliers.Refflag", vsup.Refflag), Query.EQ("Vendorsuppliers.RefNo", vsup.RefNo));
        //                    // var query = Query.And(Query.ElemMatch("Vendorsuppliers", Query.And(Query.EQ("Refflag", vsup.Refflag), Query.EQ("RefNo", vsup.RefNo))));
        //                    var query = Query.EQ("Vendorsuppliers.RefNo", vsup.RefNo);
        //                    var vn = _DatamasterRepository.FindAll(query).ToList();
        //                    if (vn != null && vn.Count > 1)
        //                    {
        //                        foreach (Prosol_Datamaster dm in vn)
        //                        {
        //                            if (dm.Itemcode != cat.Itemcode && -1 == dupList.FindIndex(f => f.Itemcode.Equals(dm.Itemcode)))
        //                            {
        //                                dupList.Add(dm);

        //                            }


        //                        }

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    if (flg == 0)
        //    {
        //        var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
        //    var NMList = _nounModifierRepository.FindAll(FormattedQuery).ToList();

        //    if (NMList != null && NMList.Count > 0 && NMList[0].Formatted == 1)
        //    {
        //        var QryLst = new List<IMongoQuery>();
        //        var ChList = _CharacteristicRepository.FindAll(FormattedQuery).ToList();
        //        if (ChList != null && ChList.Count > 0 && cat.Characteristics != null && cat.Characteristics.Count > 0)
        //        {
        //            foreach (Prosol_Charateristics chMdl in ChList)
        //            {
        //                if (chMdl.Mandatory == "Yes")
        //                {
        //                    foreach (Prosol_AttributeList ml in cat.Characteristics)
        //                    {
        //                        if (chMdl.Characteristic == ml.Characteristic)
        //                        {
        //                            if (ml.Value != null && ml.Value != "")
        //                            {
        //                                var Qry1 = Query.And(Query.ElemMatch("Characteristics", Query.And(Query.EQ("Characteristic", ml.Characteristic), Query.EQ("Value", ml.Value))));
        //                                // var Qry1 = Query.And(Query.EQ("Characteristics.Characteristic", ml.Characteristic), Query.EQ("Characteristics.Value", ml.Value));
        //                                QryLst.Add(Query.And(Qry1));
        //                            }
        //                            else
        //                            {
        //                                var Qry1 = Query.And(Query.ElemMatch("Characteristics", Query.And(Query.EQ("Characteristic", ml.Characteristic), Query.Or(Query.EQ("Value", ""), Query.EQ("Value", BsonNull.Value)))));
        //                                // var Qry1 = Query.And(Query.EQ("Characteristics.Characteristic", ml.Characteristic), Query.Or(Query.EQ("Characteristics.Value", ""), Query.EQ("Characteristics.Value", BsonNull.Value)));
        //                                QryLst.Add(Query.And(Qry1));

        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        if (QryLst.Count > 0)
        //        {
        //            QryLst.Add(Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier), Query.Or(Query.EQ("Vendorsuppliers", BsonNull.Value), Query.EQ("Vendorsuppliers", new BsonArray()))));


        //            var query = Query.And(QryLst);
        //            // var query = Query.And(Query.EQ("Shortdesc", cat.Shortdesc), Query.EQ("Duplicates", BsonNull.Value));
        //            var vn = _DatamasterRepository.FindAll(query).ToList();
        //            if (vn != null && vn.Count > 1)
        //            {
        //                foreach (Prosol_Datamaster dm in vn)
        //                {
        //                    //if (!dupList.Contains(dm))
        //                    if (dm.Itemcode!=cat.Itemcode && - 1 == dupList.FindIndex(f => f.Itemcode.Equals(dm.Itemcode)))
        //                    {
        //                        dupList.Add(dm);
        //                    }
        //                }

        //            }
        //        }

        //    }


        //        if (NMList != null && NMList.Count > 0 && NMList[0].Formatted == 0)
        //        {
        //            var QryLst = new List<IMongoQuery>();
        //            QryLst.Add(Query.And(Query.EQ("Shortdesc", cat.Shortdesc), Query.EQ("Noun", cat.Noun)));
        //            if (cat.Vendorsuppliers != null)
        //            {
        //                foreach (Vendorsuppliers vsup in cat.Vendorsuppliers)
        //                {
        //                    if ((vsup.Name == null || vsup.Name == "") && vsup.RefNo != null && vsup.RefNo != "" && vsup.Refflag == "DRAWING & POSITION NUMBER")
        //                    {
        //                        var Qry2 = Query.EQ("Vendorsuppliers.RefNo", vsup.RefNo);
        //                        QryLst.Add(Qry2);

        //                    }
        //                    else if (vsup.Name != null && vsup.Name != "" && vsup.RefNo != null && vsup.RefNo != "" && vsup.Refflag == "DRAWING & POSITION NUMBER")
        //                    {

        //                        var Qry2 = Query.And(Query.EQ("Vendorsuppliers.RefNo", vsup.RefNo), Query.EQ("Vendorsuppliers.Name", vsup.Name));
        //                        QryLst.Add(Qry2);

        //                    }
        //                    else if (vsup.Name != null && vsup.Name != "" && (vsup.RefNo != null || vsup.RefNo == "") && (vsup.Refflag == "" || vsup.Refflag == null))
        //                    {
        //                        var Qry2 = Query.EQ("Vendorsuppliers.Name", vsup.Name);
        //                        QryLst.Add(Qry2);
        //                    }
        //                }
        //            }

        //            var query = Query.And(QryLst);
        //            var vn = _DatamasterRepository.FindAll(query).ToList();
        //            if (vn != null && vn.Count > 1)
        //            {
        //                foreach (Prosol_Datamaster dm in vn)
        //                {
        //                    int indc = 0;

        //                    if (dm.Equipment != null && dm.Equipment.Modelno != null && dm.Equipment.Modelno != "" && cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "" && dm.Equipment.Modelno != cat.Equipment.Modelno)
        //                    {
        //                        indc = 1;
        //                    }
        //                    else if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "" && (dm.Equipment == null || dm.Equipment.Modelno == null || dm.Equipment.Modelno == ""))
        //                    {
        //                        indc = 1;
        //                    }
        //                    else if (dm.Equipment != null && dm.Equipment.Modelno != null && dm.Equipment.Modelno != "" && (cat.Equipment == null || cat.Equipment.Modelno == null || cat.Equipment.Modelno == ""))
        //                    {
        //                        indc = 1;
        //                    }

        //                    if (dm.Characteristics != null && cat.Characteristics != null)
        //                    {
        //                        int ind = 0;
        //                        foreach (Prosol_AttributeList mdol in dm.Characteristics)
        //                        {
        //                            foreach (Prosol_AttributeList mdl in cat.Characteristics)
        //                            {
        //                                if (mdl.Characteristic == mdol.Characteristic && mdl.Value != null && mdl.Value != "" && mdol.Value != null && mdol.Value != "")
        //                                {
        //                                    if (mdl.Value != mdol.Value)
        //                                    {
        //                                        ind = 1;
        //                                    }

        //                                }

        //                            }
        //                        }
        //                        if (ind == 0 && indc == 0)
        //                        {
        //                            if (dm.Itemcode != cat.Itemcode && -1 == dupList.FindIndex(f => f.Itemcode.Equals(dm.Itemcode)))
        //                            {
        //                                dupList.Add(dm);
        //                            }
        //                        }
        //                    }

        //                }

        //            }


        //        }

        //    }
        //    if (dupList != null && dupList.Count > 0)
        //    {
        //        return dupList;
        //    }
        //    else return null;


        //    //var query = Query.And(Query.EQ("Shortdesc", cat.Shortdesc), Query.NE("Shortdesc", ""), Query.NE("Itemcode", cat.Itemcode));
        //    //var vn = _DatamasterRepository.FindAll(query).ToList();
        //    //if (vn != null)
        //    //{
        //    //    if (vn.Count > 0)
        //    //    {
        //    //        return vn;
        //    //    }
        //    //    else return null;
        //    //}
        //    //else return null;
        //}

        public bool Reworkreview(string itemcode)
        {
            var res = false;
            var query = Query.EQ("Itemcode", itemcode);
            var vn = _DatamasterRepository.FindOne(query);
            if (vn != null)
            {
                vn.ItemStatus = 0;
                vn.Reworkcat = "rev";
                // vn.Review = null;               
                res = _DatamasterRepository.Add(vn);

            }
            return res;

        }
        public bool Reworkrelease(string itemcode)
        {
            var res = false;
            var query = Query.EQ("Itemcode", itemcode);
            var vn = _DatamasterRepository.FindOne(query);
            if (vn != null)
            {
                vn.ItemStatus = 2;
                vn.Rework = "rel";
                //  vn.Release = null;
                res = _DatamasterRepository.Add(vn);

            }
            return res;

        }

        public List<string> GetValuesList(string Noun, string Modifier, string Characteristic)
        {
            var Lst = new List<string>();
            string[] strArr = { "Characteristics.Characteristic", "Characteristics.Value" };
            var fields = Fields.Include(strArr).Exclude("_id");

            var query = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.EQ("Characteristics.Characteristic", Characteristic));
            var arrResult = _DatamasterRepository.FindAll(fields, query);
            foreach (Prosol_Datamaster mdl in arrResult)
            {
                foreach (Prosol_AttributeList att in mdl.Characteristics)
                {
                    if (att.Characteristic == Characteristic)
                    {
                        if (!Lst.Contains(att.Value) && att.Value != null)
                            Lst.Add(att.Value);
                    }
                }
            }
            return Lst;
        }

        public List<string> GetValues(string Noun, string Modifier, string Attribute)
        {
            var Lst1 = new List<string>();

            var Qry = Query.EQ("Attribute", Attribute);
            var AttributeList = _attributeRepository.FindOne(Qry);

            if (AttributeList != null && AttributeList.ValueList != null)
            {
                var Lst = new List<ObjectId>();
                string[] strArr = { "Value" };
                var fields = Fields.Include(strArr).Exclude("_id");
                foreach (string str in AttributeList.ValueList)
                {
                    Lst.Add(new ObjectId(str));
                }
                var query = Query.In("_id", new BsonArray(Lst));
                var arrResult = _abbreivateRepository.FindAll(fields, query);
                foreach (Prosol_Abbrevate mdl in arrResult)
                {
                    Lst1.Add(mdl.Value);

                }
            }
            if (Attribute != null)
            {

                string[] strArr = { "Values" };
                var fields = Fields.Include(strArr).Exclude("_id");

                var query = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.EQ("Characteristic", Attribute));
                var Lstobj = _CharacteristicRepository.FindAll(fields, query).ToList();

                if (Lstobj != null && Lstobj.Count > 0 && Lstobj[0].Values != null)
                {

                    var Lst = new List<ObjectId>();
                    foreach (string str in Lstobj[0].Values)
                    {
                        Lst.Add(new ObjectId(str));
                    }
                    query = Query.In("_id", new BsonArray(Lst));
                    var arrResult = _abbreivateRepository.FindAll(query);
                    foreach (Prosol_Abbrevate mdl in arrResult)
                    {
                        Lst1.Add(mdl.Value);

                    }
                }
            }

            return Lst1;
        }
        public string CheckValue(string Noun, string Modifier, string Attribute, string Value)
        {
            string B = "false";
            var query = Query.EQ("Value", Value);
            var obj = _abbreivateRepository.FindOne(query);
            if (obj != null)
            {
                return obj.Abbrevated;

            }
            else return B;

        }



        public bool AddValue(string Noun, string Modifier, string Attribute, string Value, string abb, string user)
        {
            var query = Query.And(Query.EQ("Value", Value), Query.EQ("Abbrevated", abb));
            var obj = _abbreivateRepository.FindOne(query);
            if (obj != null)
            {

                string[] sAr = { obj._id.ToString() };
                var Qry = Query.EQ("Attribute", Attribute);
                var AttributeMaster = _attributeRepository.FindOne(Qry);
                if (AttributeMaster != null && AttributeMaster.ValueList != null && AttributeMaster.ValueList.Length > 0)
                {
                    var lstobj = AttributeMaster.ValueList.ToList();
                    lstobj.Add(obj._id.ToString());
                    AttributeMaster.ValueList = lstobj.ToArray();
                    _attributeRepository.Add(AttributeMaster);

                }
                var Qry1 = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.EQ("Characteristic", Attribute));
                var TemplateMaster = _CharacteristicRepository.FindOne(Qry1);
                if (TemplateMaster != null && TemplateMaster.Values != null)
                {
                    var lstobj = TemplateMaster.Values.ToList();
                    lstobj.Add(obj._id.ToString());
                    TemplateMaster.Values = lstobj.ToArray();
                    _CharacteristicRepository.Add(TemplateMaster);
                }
                else
                {
                    var str = new string[1];
                    str[0] = obj._id.ToString();
                    TemplateMaster.Values = str;
                    _CharacteristicRepository.Add(TemplateMaster);

                }

            }
            else
            {
                var newobj = new Prosol_Abbrevate();
                if (abb == null || abb == "null")
                {
                    newobj.Abbrevated = "";
                }
                else newobj.Abbrevated = abb;
                newobj.Value = Value;
                newobj.User = user;

                _abbreivateRepository.Add(newobj);
                query = Query.And(Query.EQ("Value", Value), Query.EQ("Abbrevated", abb));
                obj = _abbreivateRepository.FindOne(query);
                if (obj != null)
                {

                    string[] sAr = { obj._id.ToString() };
                    var Qry = Query.EQ("Attribute", Attribute);
                    var AttributeMaster = _attributeRepository.FindOne(Qry);
                    if (AttributeMaster != null && AttributeMaster.ValueList != null && AttributeMaster.ValueList.Length > 0)
                    {
                        var lstobj = AttributeMaster.ValueList.ToList();
                        lstobj.Add(obj._id.ToString());
                        AttributeMaster.ValueList = lstobj.ToArray();
                        _attributeRepository.Add(AttributeMaster);

                    }
                    var Qry1 = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.EQ("Characteristic", Attribute));
                    var TemplateMaster = _CharacteristicRepository.FindOne(Qry1);
                    if (TemplateMaster != null && TemplateMaster.Values != null)
                    {
                        var lstobj = TemplateMaster.Values.ToList();
                        lstobj.Add(obj._id.ToString());
                        TemplateMaster.Values = lstobj.ToArray();
                        _CharacteristicRepository.Add(TemplateMaster);
                    }
                    else
                    {
                        var str = new string[1];
                        str[0] = obj._id.ToString();
                        TemplateMaster.Values = str;
                        _CharacteristicRepository.Add(TemplateMaster);

                    }
                }

            }
            var x = CheckValue(Noun, Modifier, Attribute, Value);
            return true;


        }
        public List<Prosol_Attribute> GetUnits()
        {


            // var Qry = Query.EQ("Attribute", Attribute);
            var AttributeList = _attributeRepository.FindAll().ToList();

            if (AttributeList.Count > 0)
            {
                foreach (Prosol_Attribute md in AttributeList)
                {
                    var Lst1 = new List<string>();
                    var Lst = new List<ObjectId>();
                    string[] strArr = { "Unitname" };
                    var fields = Fields.Include(strArr).Exclude("_id");
                    if (md.UOMList != null && md.UOMList.Length > 0)
                    {
                        foreach (string str in md.UOMList)
                        {
                            Lst.Add(new ObjectId(str));
                        }
                    }
                    var query = Query.In("_id", new BsonArray(Lst));
                    var arrResult = _uomRepository.FindAll(fields, query);
                    foreach (Prosol_UOM mdl in arrResult)
                    {
                        Lst1.Add(mdl.Unitname);

                    }
                    md.UOMList = Lst1.ToArray();
                }

            }

            return AttributeList;
        }
        public int checkValidate(string Attribute)
        {
            var Qry = Query.EQ("Attribute", Attribute);
            var AttributeList = _attributeRepository.FindOne(Qry);
            if (AttributeList != null)
                return AttributeList.Validation;
            else
                return 0;
        }
        public List<Prosol_Datamaster> GetsimItemsList(string Noun, string Modifier)
        {
            string[] strArr = { "Itemcode", "Shortdesc", "Characteristics" };
            var fields = Fields.Include(strArr).Exclude("_id");
            List<Prosol_Datamaster> lst = new List<Prosol_Datamaster>();
            var query = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.NE("Characteristics", BsonNull.Value), Query.GTE("ItemStatus", 1));
            var arrResult = _DatamasterRepository.FindAll(fields, query).ToList();
            foreach (Prosol_Datamaster mdl in arrResult)
            {
                // int totChar = mdl.Characteristics.Count;
                var totVal = (from i in mdl.Characteristics where i.Value != null && i.Value != "" select i).ToList().Count;
                //  mdl.ItemStatus = ((totVal * 100) / totChar);
                mdl.ItemStatus = 0;
                if (totVal > 0)
                    lst.Add(mdl);
            }
            return lst;
        }
        public IEnumerable<Prosol_Plants> getplantCode_Name()
        {
            var query = Query.EQ("Islive", true);
            var plantdetails = _PlanttRepository.FindAll(query).ToList();
            return plantdetails;
        }
        public IEnumerable<Prosol_UOMMODEL> getuomlist()
        {

            //var query = Query.EQ("UOMNAME", UOMNAME);
            var vn = _uomlistRepository.FindAll().ToList();
            return vn;
        }
        //public IEnumerable<Prosol_UOM> getuomlist()
        //{





        //    var vn = _uomRepository.FindAll().ToList();
        //    return vn;
        //}





        public string getItem()
        {
            string code = "";
            var sort = SortBy.Descending("_id");

            var query = Query.Matches("Itemcode", new BsonRegularExpression("^[0-9]*$"));
            //  var query = Query.Matches("Itemcode", Regex.Matches(Itemcode, @"\d{1,2}")
            var Itmcode = _DatamasterRepository.FindAll(query, sort).ToList();
            if (Itmcode != null && Itmcode.Count > 0)
            {
                code = Itmcode[0].Itemcode;
            }
            return code;
        }
        public string getMaterialCode(string LogicCode)
        {
            string code = "";
            var sort = SortBy.Ascending("_id");
            //   var query = Query.Matches("Materialcode", BsonRegularExpression.Create(new Regex(LogicCode, RegexOptions.IgnoreCase)));
            var query = Query.Matches("Materialcode", BsonRegularExpression.Create(new Regex(string.Format("^{0}", LogicCode.TrimStart().TrimEnd()), RegexOptions.IgnoreCase)));
            var Materialcode = _DatamasterRepository.FindAll(query, sort).ToList();
            if (Materialcode != null && Materialcode.Count > 0)
            {
                //increament

                //foreach (Prosol_Datamaster mdl in Materialcode.OrderBy(x=>x.Materialcode))
                //{
                //    int cunt = Regex.Matches(mdl.Materialcode, @"[a-zA-Z]").Count;
                //    if (cunt == 0)
                //    {
                //        code = mdl.Materialcode;
                //        long inc = Convert.ToInt64(mdl.Materialcode);
                //        if (!Materialcode.Any(x => x.Materialcode == Convert.ToString(inc+1)))
                //        {                          
                //            code = mdl.Materialcode;
                //            break;
                //        }

                //    }
                //}

                //decreament




                // const string chars = "0123456789ABCDFGHIJKLMNOPQRSTUVWXYZ";
                // var combi = new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());

                //if (Materialcode.Count < 30000)
                //{
                //    for (char c = 'A'; c <= 'Z'; c++)
                //    {
                //        if (c != 'E')
                //        {
                //            string runCode = "";
                //            for (int i = 1; i < 1000; i++)
                //            {
                //                switch (i.ToString().Length)
                //                {
                //                    case 1:
                //                        runCode = c + "00" + i;
                //                        break;
                //                    case 2:
                //                        runCode = c + "0" + i;
                //                        break;
                //                    default:
                //                        runCode = c + i.ToString();
                //                        break;
                //                }
                //                if (!Materialcode.Any(x => x.Materialcode == Convert.ToString(LogicCode + runCode)))
                //                {
                //                    code = LogicCode + runCode;
                //                    break;
                //                }
                //            }
                //        }
                //        if (code != "")
                //            break;
                //    }
                //    //for (int i = 0; 100000 > i; i++)
                //    //{
                //    //    LBL:
                //    //    string combi = RandomPassword();
                //    //    if (combi.Contains('E'))
                //    //    {
                //    //        goto LBL;
                //    //    }
                //    //    if (!Materialcode.Any(x => x.Materialcode == Convert.ToString(LogicCode + combi)))
                //    //    {
                //    //        code = LogicCode + combi;
                //    //        break;
                //    //    }
                //    //}
                //}
                //else
                //{
                for (char c = 'A'; c <= 'Z'; c++)
                {
                    for (char ch = 'A'; ch <= 'Z'; ch++)
                    {
                        string str = c + ch.ToString();

                        if (!str.Contains('E'))
                        {
                            string runCode = "";
                            for (int i = 1; i < 100; i++)
                            {
                                switch (i.ToString().Length)
                                {
                                    case 1:
                                        runCode = str + "0" + i;
                                        break;
                                    default:
                                        runCode = str + i.ToString();
                                        break;
                                }
                                if (!Materialcode.Any(x => x.Materialcode == Convert.ToString(LogicCode + runCode)))
                                {
                                    code = LogicCode + runCode;
                                    break;
                                }
                            }
                        }
                        if (code != "")
                            break;
                    }
                    if (code != "")
                        break;
                }

                //for (int i = 0; 100000 > i; i++)
                //{
                //    LBL1:
                //    string combi = RandomPasswordDouble();
                //    if (combi.Contains('E'))
                //    {
                //        goto LBL1;
                //    }
                //    if (!Materialcode.Any(x => x.Materialcode == Convert.ToString(LogicCode + combi)))
                //    {
                //        code = LogicCode + combi;
                //        break;
                //    }
                //}
                // }



                //long decr = Convert.ToInt64(LogicCode + "9999");
                //for (int i = 9999; i > 0; i--)
                //{
                //    if (!Materialcode.Any(x => x.Materialcode == Convert.ToString(decr)))
                //    {
                //        code = decr.ToString();
                //        break;
                //    }
                //    decr--;
                //}
            }
            if (code == "")
            {

                //LBL:
                //string combi = RandomPassword();
                //if (combi.Contains('E'))
                //{
                //    goto LBL;
                //}
                //code = LogicCode + combi;

                code = LogicCode + "A001";
            }
            return code;

        }

        //Logic

        public List<Prosol_Logic> checkLogic(string Attribute)
        {
            var query = Query.Or(Query.EQ("AttributeName1", Attribute), Query.EQ("AttributeName2", Attribute), Query.EQ("AttributeName3", Attribute), Query.EQ("AttributeName4", Attribute));
            var LogicRes = _logicRepository.FindAll(query).ToList();
            return LogicRes;
        }

        public string getunitforvalue(string Value)
        {
            var qureyyy = Query.EQ("Value", Value);
            var res = _abbreivateRepository.FindAll(qureyyy).ToList();
            if (res.Count > 0)
                return res[0].vunit;
            else
                return null;
        }

        //reject

        public bool GetCodeForRejectedItems(string Itemcode, string Remarks, string userid, string username)
        {
            var query = Query.EQ("Itemcode", Itemcode);
            var vn = _DatamasterRepository.FindAll(query).ToList();
            Prosol_Datamaster pd = vn[0];
            pd.ItemStatus = -1;
            pd.Remarks = Remarks;
            pd.Materialcode = null;
            pd.Shortdesc = null;
            pd.Longdesc = null;
            pd.Unspsc = null;
            Prosol_UpdatedBy pu = new Prosol_UpdatedBy();
            pu.Name = username;
            pu.UserId = userid;
            pu.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            pd.RejectedBy = pu;

            _DatamasterRepository.Add(pd);



            //var query1 = Query.EQ("itemId", Itemcode);

            //var rejcde = _ProsolRequest.FindAll(query1).ToList();

            //if (rejcde != null && rejcde.Count > 0)
            //{
            //    ////string str = DateTime.Parse(rejcde[0].rejectedOn.ToString()).ToString("dd/MM/yyyy");
            //    //string str = "";
            //    //if (vn != null && vn.Count > 0 && vn[0].Remarks != null && vn[0].Remarks != "" && vn[0].Remarks.Contains("CNX OPERATION"))
            //    //{
            //    //    int inx = vn[0].Remarks.LastIndexOf("CNX OPERATION : ");
            //    //    str = vn[0].Remarks.Substring(inx + 16);
            //    //    str = str + " " + vn[0].RevRemarks;
            //    //}
            //    //else str = vn[0].Remarks + " " + vn[0].RevRemarks;


            //    rejcde[0].rejectedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //    rejcde[0].itemStatus = "Clarification";
            //    rejcde[0].Remark = vn[0].Remarks + " " + vn[0].RevRemarks;
            //    //rejcde[0].Remark = "ABG : "+rejcde[0].Remark+" CNX OPERATION : "+ str;
            //    _ProsolRequest.Add(rejcde[0]);

            //    var qryUser = Query.EQ("Userid", rejcde[0].requester);
            //    var UsrRes = _usersRepository.FindOne(qryUser);

            //    qryUser = Query.EQ("Userid", rejcde[0].approver);
            //    var Usrapp = _usersRepository.FindOne(qryUser);

            //    _Emailservc.sendmail(UsrRes.EmailId, "Item rejection", Itemcode + " requested Item has been Clarification");


            //    _Emailservc.sendmail(Usrapp.EmailId, "Item rejection", Itemcode + " approved Item has been Clarification");


            //}



            return true;
        }

        public int GetCodeForRejectedItems(List<Prosol_Datamaster> lpdm)
        {
            foreach (Prosol_Datamaster pd in lpdm)
            {
                var qry = Query.EQ("Itemcode", pd.Itemcode);
                _erpRepository.Delete(qry);
                _DatamasterRepository.Delete(qry);
            }
            return lpdm.Count();
        }


        public bool GetCodeForRejectedItems1(string Itemcode, string RevRemarks, string userid, string username)
        {
            var query = Query.EQ("Itemcode", Itemcode);
            var vn = _DatamasterRepository.FindAll(query).ToList();
            Prosol_Datamaster pd = vn[0];
            pd.ItemStatus = -1;
            pd.RevRemarks = RevRemarks;
            pd.Materialcode = null;
            pd.Shortdesc = null;
            pd.Longdesc = null;
            pd.Unspsc = null;
            Prosol_UpdatedBy pu = new Prosol_UpdatedBy();
            pu.Name = username;
            pu.UserId = userid;
            pu.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            pd.RejectedBy = pu;

            _DatamasterRepository.Add(pd);

            //var query1 = Query.EQ("itemId", Itemcode);

            //var rejcde = _ProsolRequest.FindAll(query1).ToList();
            //if (rejcde != null && rejcde.Count > 0)
            //{

            //    rejcde[0].rejectedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //    rejcde[0].itemStatus = "Clarification";
            //    // rejcde[0].Remark = "ABG : " + rejcde[0].Remark + " CNX OPERATION : " + str;
            //    rejcde[0].Remark = vn[0].Remarks + " " + vn[0].RevRemarks;
            //    _ProsolRequest.Add(rejcde[0]);

            //    ////string str = DateTime.Parse(rejcde[0].rejectedOn.ToString()).ToString("dd/MM/yyyy");
            //    //rejcde[0].rejectedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //    //rejcde[0].itemStatus = "rejected";
            //    //_ProsolRequest.Add(rejcde[0]);

            //    var qryUser = Query.EQ("Userid", rejcde[0].requester);
            //    var UsrRes = _usersRepository.FindOne(qryUser);

            //    qryUser = Query.EQ("Userid", rejcde[0].approver);
            //    var Usrapp = _usersRepository.FindOne(qryUser);

            //    _Emailservc.sendmail(UsrRes.EmailId, "Item rejection", Itemcode + " requested Item has been Clarification ");


            //    _Emailservc.sendmail(Usrapp.EmailId, "Item rejection", Itemcode + " approved Item has been Clarification");


            //}
            //var query2 = Query.EQ("Itemcode", Itemcode);
            //var cdrj = _DatamasterRepository.FindAll(query2).ToList();
            //if (cdrj != null && cdrj.Count > 0)
            //{
            //    var qryUser = Query.EQ("Userid", cdrj[0].Catalogue.UserId);
            //    var UsrRes = _usersRepository.FindOne(qryUser);
            //    _Emailservc.sendmail(UsrRes.EmailId, "Item rejection", Itemcode + " catalogued by you has been Clarification");

            //}


            return true;
        }
        public bool GetCodeForRejectedItems2(string Itemcode, string RelRemarks, string userid, string username)
        {
            var query = Query.EQ("Itemcode", Itemcode);
            var vn = _DatamasterRepository.FindAll(query).ToList();
            Prosol_Datamaster pd = vn[0];
            pd.ItemStatus = -1;
            pd.Materialcode = null;
            pd.RelRemarks = RelRemarks;
            Prosol_UpdatedBy pu = new Prosol_UpdatedBy();
            pu.Name = username;
            pu.UserId = userid;
            pu.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            pd.RejectedBy = pu;

            _DatamasterRepository.Add(pd);
            //var query1 = Query.EQ("itemId", Itemcode);

            //var rejcde = _ProsolRequest.FindAll(query1).ToList();
            //if (rejcde != null && rejcde.Count > 0)
            //{
            //    //string str = DateTime.Parse(rejcde[0].rejectedOn.ToString()).ToString("dd/MM/yyyy");
            //    rejcde[0].rejectedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //    rejcde[0].itemStatus = "rejected";
            //    _ProsolRequest.Add(rejcde[0]);

            //    var qryUser = Query.EQ("Userid", rejcde[0].requester);
            //    var UsrRes = _usersRepository.FindOne(qryUser);

            //    qryUser = Query.EQ("Userid", rejcde[0].approver);
            //    var Usrapp = _usersRepository.FindOne(qryUser);

            //    _Emailservc.sendmail(UsrRes.EmailId, "Item rejection", Itemcode + " requested by you has been rejected");


            //    _Emailservc.sendmail(Usrapp.EmailId, "Item rejection", Itemcode + " approved by you has been rejected");


            //}

            //var query3 = Query.EQ("Itemcode", Itemcode);
            //var rejcde1 = _DatamasterRepository.FindAll(query3).ToList();
            //if (rejcde1 != null && rejcde1.Count > 0)
            //{
            //    var qryUser = Query.EQ("Userid", rejcde1[0].Catalogue.UserId);
            //    var UsrRes = _usersRepository.FindOne(qryUser);

            //    qryUser = Query.EQ("Userid", rejcde1[0].Review.UserId);
            //    var UsrRes1 = _usersRepository.FindOne(qryUser);

            //    _Emailservc.sendmail(UsrRes.EmailId, "Item rejection", Itemcode + " catalogued by you has been rejected");

            //    _Emailservc.sendmail(UsrRes1.EmailId, "Item rejection", Itemcode + " Reviewed by you has been rejected");

            //}



            return true;
        }
        //pv data

        public bool PVDATA(string Itemcode, string Remarks, string userid, string username)

        {
            var query = Query.EQ("Itemcode", Itemcode);
            var vn = _DatamasterRepository.FindAll(query).ToList();
            Prosol_Datamaster pd = vn[0];
            pd.PVstatus = "YES";
            pd.Remarks = Remarks;
            pd.Materialcode = null;
            pd.Shortdesc = null;
            pd.Longdesc = null;
            pd.Unspsc = null;
            Prosol_UpdatedBy pu = new Prosol_UpdatedBy();
            pu.Name = username;
            pu.UserId = userid;
            pu.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            pd.RejectedBy = pu;

            _DatamasterRepository.Add(pd);



            return true;
        }
        public List<Prosol_Datamaster> searchMaster(string sCode, string sSource, string sShort, string sLong, string sNoun, string sModifier, string sUser, string sType, string sStatus)
        {
            string[] strArr = { "Itemcode", "Materialcode", "Legacy", "Noun", "Modifier", "Shortdesc", "Longdesc", "ItemStatus", "Catalogue", "Review", "Release", "Rework", "Reworkcat", "Junk" };
            List<searchObj> LstObj = new List<searchObj>();
            searchObj sObj = new searchObj();
            sObj.SearchColumn = "Materialcode";
            sObj.SearchKey = sCode;
            LstObj.Add(sObj);



            sObj = new searchObj();
            sObj.SearchColumn = "Legacy";
            sObj.SearchKey = sSource;
            LstObj.Add(sObj);
            sObj = new searchObj();
            sObj.SearchColumn = "Shortdesc";
            sObj.SearchKey = sShort;
            LstObj.Add(sObj);
            sObj = new searchObj();
            sObj.SearchColumn = "Longdesc";
            sObj.SearchKey = sLong;
            LstObj.Add(sObj);

            sObj = new searchObj();
            sObj.SearchColumn = "Noun";
            sObj.SearchKey = sNoun;
            LstObj.Add(sObj);

            sObj = new searchObj();
            sObj.SearchColumn = "Modifier";
            sObj.SearchKey = sModifier;
            LstObj.Add(sObj);

            sObj = new searchObj();
            sObj.SearchColumn = "sUser";
            sObj.SearchKey = sUser;
            LstObj.Add(sObj);

            sObj = new searchObj();
            sObj.SearchColumn = "Junk";
            sObj.SearchKey = sType;

            LstObj.Add(sObj);
            sObj = new searchObj();
            sObj.SearchColumn = "sStatus";
            sObj.SearchKey = sStatus;
            LstObj.Add(sObj);

            List<Prosol_Datamaster> newResultList = new List<Prosol_Datamaster>();
            //  List<Prosol_Datamaster> RemoveResultList = new List<Prosol_Datamaster>();
            int flg = 0;
            foreach (searchObj mdl in LstObj)
            {
                if (mdl.SearchKey != "" && mdl.SearchKey != null && mdl.SearchKey != "null" && mdl.SearchKey != "undefined")
                {
                    if (flg == 0)
                    {
                        List<Prosol_Datamaster> sResult = new List<Prosol_Datamaster>();
                        if (mdl.SearchColumn == "sUser")
                        {
                            sResult = UserSearch(strArr, mdl.SearchKey);

                        }
                        else if (mdl.SearchColumn == "sStatus")
                        {
                            sResult = StatusSearch(strArr, mdl.SearchKey);
                        }
                        else
                        {
                            sResult = SearchFn(strArr, mdl.SearchColumn, mdl.SearchKey);
                        }
                        if (sResult != null && sResult.Count > 0)
                        {

                            foreach (Prosol_Datamaster pmdl in sResult)
                            {
                                newResultList.Add(pmdl);
                                flg = 1;
                                //  RemoveResultList.Add(pmdl);
                            }

                        }
                        else break;
                    }
                    else
                    {
                        //  var sResult = SearchFn(strArr, mdl.SearchColumn, mdl.SearchKey);
                        if (mdl.SearchKey.Contains("*"))
                        {
                            string[] splt = mdl.SearchKey.Split('*');
                            if (splt.Length > 2)
                            {
                                //contains                              
                                foreach (string str in splt)
                                {
                                    if (str != "")
                                    {
                                        int flgg = 0;
                                        var query1 = new List<Prosol_Datamaster>();
                                        if (mdl.SearchColumn == "Itemcode")
                                        {
                                            if (flgg == 0)
                                            {
                                                query1 = (from x in newResultList where x.Materialcode.Contains(str) select x).ToList();
                                            }
                                            if (flgg == 1)
                                            {
                                                query1 = query1.Where(x => x.Materialcode.Contains(str)).ToList();

                                            }
                                        }
                                        if (mdl.SearchColumn == "Legacy")
                                        {
                                            if (flgg == 0)
                                            {
                                                query1 = (from x in newResultList where x.Legacy.Contains(str) select x).ToList();
                                            }
                                            if (flgg == 1)
                                            {
                                                query1 = query1.Where(x => x.Legacy.Contains(str)).ToList();

                                            }

                                            // newResultList = (from x in newResultList where x.Legacy.Contains(str) select x).ToList();
                                        }
                                        if (mdl.SearchColumn == "Shortdesc")
                                        {
                                            if (flgg == 0)
                                            {
                                                query1 = (from x in newResultList where x.Shortdesc.Contains(str) select x).ToList();
                                            }
                                            if (flgg == 1)
                                            {
                                                query1 = query1.Where(x => x.Shortdesc.Contains(str)).ToList();

                                            }
                                            // newResultList = (from x in newResultList where x.Shortdesc.Contains(str) select x).ToList();
                                        }
                                        if (mdl.SearchColumn == "Longdesc")
                                        {
                                            if (flgg == 0)
                                            {
                                                query1 = (from x in newResultList where x.Longdesc.Contains(str) select x).ToList();
                                            }
                                            if (flgg == 1)
                                            {
                                                query1 = query1.Where(x => x.Longdesc.Contains(str)).ToList();

                                            }
                                            // newResultList = (from x in newResultList where x.Longdesc.Contains(str) select x).ToList();
                                        }
                                        newResultList = query1;
                                        flgg = 1;
                                    }
                                }

                            }
                            else
                            {
                                if (mdl.SearchKey != "" && mdl.SearchKey != null && mdl.SearchKey != "null" && mdl.SearchKey != "undefined")
                                {
                                    if (sCode.IndexOf('*') > 0)
                                    {

                                        //Start with



                                        if (mdl.SearchColumn == "Materialcode")
                                            newResultList = (from x in newResultList where x.Materialcode.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();
                                        if (mdl.SearchColumn == "Legacy")
                                            newResultList = (from x in newResultList where x.Legacy.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();
                                        if (mdl.SearchColumn == "Shortdesc")
                                            newResultList = (from x in newResultList where x.Shortdesc.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();
                                        if (mdl.SearchColumn == "Longdesc")
                                            newResultList = (from x in newResultList where x.Longdesc.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();

                                    }
                                    else
                                    {
                                        //End with



                                        if (mdl.SearchColumn == "Materialcode")
                                            newResultList = (from x in newResultList where x.Materialcode.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();
                                        if (mdl.SearchColumn == "Legacy")
                                            newResultList = (from x in newResultList where x.Legacy.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();
                                        if (mdl.SearchColumn == "Shortdesc")
                                            newResultList = (from x in newResultList where x.Shortdesc.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();
                                        if (mdl.SearchColumn == "Longdesc")
                                            newResultList = (from x in newResultList where x.Longdesc.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();


                                    }
                                }

                            }

                        }
                        else
                        {
                            if (mdl.SearchKey != "" && mdl.SearchKey != null && mdl.SearchKey != "null" && mdl.SearchKey != "undefined")
                            {


                                if (mdl.SearchColumn == "Materialcode")
                                    newResultList = (from x in newResultList where x.Materialcode != null && x.Itemcode.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "Legacy")
                                    newResultList = (from x in newResultList where x.Legacy != null && x.Legacy.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "Shortdesc")
                                    newResultList = (from x in newResultList where x.Shortdesc != null && x.Shortdesc.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "Longdesc")
                                    newResultList = (from x in newResultList where x.Longdesc != null && x.Longdesc.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "Noun")
                                    newResultList = (from x in newResultList where x.Noun != null && x.Noun.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "Modifier")
                                    newResultList = (from x in newResultList where x.Modifier != null && x.Modifier.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "sUser")
                                    newResultList = (from x in newResultList where x.Catalogue != null && (x.Catalogue.Name.Equals(mdl.SearchKey) || (x.Review != null && x.Review.Name.Equals(mdl.SearchKey)) || (x.Release != null && x.Release.Name.Equals(mdl.SearchKey))) select x).ToList();
                                if (mdl.SearchColumn == "Junk")
                                    newResultList = (from x in newResultList where x.Junk != null && x.Junk.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "sStatus")
                                {
                                    int sta = 0, sta1 = 1;
                                    if (sStatus == "Catalogue")
                                    {
                                        sta = 0; sta1 = 1;
                                    }
                                    else if (sStatus == "Clarification")
                                    {
                                        sta = -1; sta1 = -1;
                                    }
                                    else if (sStatus == "QC")
                                    {
                                        sta = 2; sta1 = 3;
                                    }
                                    else if (sStatus == "QA")
                                    {
                                        sta = 4; sta1 = 5;
                                    }
                                    else
                                    {
                                        sta = 6; sta1 = 6;
                                    }
                                    if (sStatus == "Catalogue Rework")
                                    {
                                        newResultList = (from x in newResultList where (x.Reworkcat != null && (x.ItemStatus == 0 || x.ItemStatus == 1)) select x).ToList();
                                    }
                                    else if (sStatus == "QC Rework")
                                    {
                                        newResultList = (from x in newResultList where (x.Rework != null && (x.ItemStatus == 2 || x.ItemStatus == 3)) select x).ToList();
                                    }
                                    else
                                    {
                                        newResultList = (from x in newResultList where (x.ItemStatus == sta || x.ItemStatus == sta1) select x).ToList();
                                    }

                                }
                            }

                        }
                    }
                }
            }
            return newResultList;
        }


        //PV DATA
        public List<Prosol_Datamaster> searchmasterpv(string sCode, string Plant, string StorageLocation, string storagebin, string sNoun, string sModifier)
        {
            string[] strArr = { "Itemcode", "Materialcode", "Legacy", "Noun", "Modifier", "Shortdesc", "Longdesc", "ItemStatus", "Catalogue", "Review", "Release", "Rework", "Reworkcat", "Junk", "Storage_Location1", "Storage_Bin1", "Plant" };
            List<searchObj> LstObj = new List<searchObj>();

            searchObj sObj = new searchObj();
            sObj.SearchColumn = "Itemcode";
            sObj.SearchKey = sCode;
            LstObj.Add(sObj);



            sObj = new searchObj();
            sObj.SearchColumn = "Plant";
            sObj.SearchKey = Plant;
            LstObj.Add(sObj);

            sObj = new searchObj();
            sObj.SearchColumn = "Storage_Location1";
            sObj.SearchKey = StorageLocation;
            LstObj.Add(sObj);

            sObj = new searchObj();
            sObj.SearchColumn = "Storage_Bin1";
            sObj.SearchKey = storagebin;
            LstObj.Add(sObj);

            sObj = new searchObj();
            sObj.SearchColumn = "Noun";
            sObj.SearchKey = sNoun;
            LstObj.Add(sObj);

            sObj = new searchObj();
            sObj.SearchColumn = "Modifier";
            sObj.SearchKey = sModifier;
            LstObj.Add(sObj);

            //sObj = new searchObj();
            //sObj.SearchColumn = "sUser";
            //sObj.SearchKey = sUser;
            //LstObj.Add(sObj);

            //sObj = new searchObj();
            //sObj.SearchColumn = "Junk";
            //sObj.SearchKey = sType;
            //LstObj.Add(sObj);

            //sObj = new searchObj();
            //sObj.SearchColumn = "sStatus";
            //sObj.SearchKey = sStatus;
            //LstObj.Add(sObj);

            List<Prosol_Datamaster> newResultList = new List<Prosol_Datamaster>();
            //  List<Prosol_Datamaster> RemoveResultList = new List<Prosol_Datamaster>();
            int flg = 0;
            foreach (searchObj mdl in LstObj)
            {
                if (mdl.SearchKey != "" && mdl.SearchKey != null && mdl.SearchKey != "null" && mdl.SearchKey != "undefined")
                {
                    if (flg == 0)
                    {
                        List<Prosol_Datamaster> sResult = new List<Prosol_Datamaster>();
                        if (mdl.SearchColumn == "sUser")
                        {
                            sResult = UserSearch(strArr, mdl.SearchKey);

                        }
                        else if (mdl.SearchColumn == "sStatus")
                        {
                            sResult = StatusSearch(strArr, mdl.SearchKey);
                        }
                        else
                        {
                            sResult = SearchFn(strArr, mdl.SearchColumn, mdl.SearchKey);
                        }
                        if (sResult != null && sResult.Count > 0)
                        {

                            foreach (Prosol_Datamaster pmdl in sResult)
                            {
                                newResultList.Add(pmdl);
                                flg = 1;
                                //  RemoveResultList.Add(pmdl);
                            }

                        }
                        else break;
                    }
                    else
                    {
                        //  var sResult = SearchFn(strArr, mdl.SearchColumn, mdl.SearchKey);
                        if (mdl.SearchKey.Contains("*"))
                        {
                            string[] splt = mdl.SearchKey.Split('*');
                            if (splt.Length > 2)
                            {
                                //contains                              
                                foreach (string str in splt)
                                {
                                    if (str != "")
                                    {
                                        int flgg = 0;
                                        var query1 = new List<Prosol_Datamaster>();
                                        if (mdl.SearchColumn == "Itemcode")
                                        {
                                            if (flgg == 0)
                                            {
                                                query1 = (from x in newResultList where x.Itemcode.Contains(str) select x).ToList();
                                            }
                                            if (flgg == 1)
                                            {
                                                query1 = query1.Where(x => x.Itemcode.Contains(str)).ToList();

                                            }
                                        }
                                        if (mdl.SearchColumn == "Storage_Location1")
                                        {
                                            if (flgg == 0)
                                            {
                                                query1 = (from x in newResultList where x.Legacy.Contains(str) select x).ToList();
                                            }
                                            if (flgg == 1)
                                            {
                                                query1 = query1.Where(x => x.Legacy.Contains(str)).ToList();

                                            }

                                            // newResultList = (from x in newResultList where x.Legacy.Contains(str) select x).ToList();
                                        }
                                        if (mdl.SearchColumn == "Storage_Bin1")
                                        {
                                            if (flgg == 0)
                                            {
                                                query1 = (from x in newResultList where x.Shortdesc.Contains(str) select x).ToList();
                                            }
                                            if (flgg == 1)
                                            {
                                                query1 = query1.Where(x => x.Shortdesc.Contains(str)).ToList();

                                            }
                                            // newResultList = (from x in newResultList where x.Shortdesc.Contains(str) select x).ToList();
                                        }
                                        if (mdl.SearchColumn == "Noun")
                                        {
                                            if (flgg == 0)
                                            {
                                                query1 = (from x in newResultList where x.Legacy.Contains(str) select x).ToList();
                                            }
                                            if (flgg == 1)
                                            {
                                                query1 = query1.Where(x => x.Legacy.Contains(str)).ToList();

                                            }

                                            // newResultList = (from x in newResultList where x.Legacy.Contains(str) select x).ToList();
                                        }
                                        if (mdl.SearchColumn == "Modifier")
                                        {
                                            if (flgg == 0)
                                            {
                                                query1 = (from x in newResultList where x.Shortdesc.Contains(str) select x).ToList();
                                            }
                                            if (flgg == 1)
                                            {
                                                query1 = query1.Where(x => x.Shortdesc.Contains(str)).ToList();

                                            }
                                            // newResultList = (from x in newResultList where x.Shortdesc.Contains(str) select x).ToList();
                                        }

                                        newResultList = query1;
                                        flgg = 1;
                                    }
                                }

                            }
                            else
                            {
                                if (mdl.SearchKey != "" && mdl.SearchKey != null && mdl.SearchKey != "null" && mdl.SearchKey != "undefined")
                                {
                                    if (sCode.IndexOf('*') > 0)
                                    {

                                        //Start with



                                        if (mdl.SearchColumn == "Itemcode")
                                            newResultList = (from x in newResultList where x.Materialcode.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();
                                        //if (mdl.SearchColumn == "Noun")
                                        //    newResultList = (from x in newResultList where x.Legacy.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();
                                        //if (mdl.SearchColumn == "Modifier")
                                        //    newResultList = (from x in newResultList where x.Shortdesc.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();


                                    }
                                    else
                                    {
                                        //End with



                                        if (mdl.SearchColumn == "Itemcode")
                                            newResultList = (from x in newResultList where x.Materialcode.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();
                                        //if (mdl.SearchColumn == "Noun")
                                        //    newResultList = (from x in newResultList where x.Legacy.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();
                                        //if (mdl.SearchColumn == "Modifier")
                                        //    newResultList = (from x in newResultList where x.Shortdesc.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();



                                    }
                                }

                            }

                        }
                        else
                        {
                            if (mdl.SearchKey != "" && mdl.SearchKey != null && mdl.SearchKey != "null" && mdl.SearchKey != "undefined")
                            {


                                if (mdl.SearchColumn == "Itemcode")
                                    newResultList = (from x in newResultList where x.Materialcode != null && x.Itemcode.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "Storage_Location1")
                                    newResultList = (from x in newResultList where x.Storage_Location1 != null && x.Storage_Location1.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "Storage_Bin1")
                                    newResultList = (from x in newResultList where x.Storage_Bin1 != null && x.Storage_Bin1.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "Noun")
                                    newResultList = (from x in newResultList where x.Noun != null && x.Noun.Equals(mdl.SearchKey) select x).ToList();
                                if (mdl.SearchColumn == "Modifier")
                                    newResultList = (from x in newResultList where x.Modifier != null && x.Modifier.Equals(mdl.SearchKey) select x).ToList();

                                //if (mdl.SearchColumn == "sUser")
                                //    newResultList = (from x in newResultList where x.Catalogue != null && (x.Catalogue.Name.Equals(mdl.SearchKey) || (x.Review != null && x.Review.Name.Equals(mdl.SearchKey)) || (x.Release != null && x.Release.Name.Equals(mdl.SearchKey))) select x).ToList();
                                //if (mdl.SearchColumn == "Junk")
                                //    newResultList = (from x in newResultList where x.Junk != null && x.Junk.Equals(mdl.SearchKey) select x).ToList();
                                //if (mdl.SearchColumn == "sStatus")
                                //{
                                //    int sta = 0, sta1 = 1;
                                //    if (sStatus == "Catalogue")
                                //    {
                                //        sta = 0; sta1 = 1;
                                //    }
                                //    else if (sStatus == "Clarification")
                                //    {
                                //        sta = -1; sta1 = -1;
                                //    }
                                //    else if (sStatus == "QC")
                                //    {
                                //        sta = 2; sta1 = 3;
                                //    }
                                //    else if (sStatus == "QA")
                                //    {
                                //        sta = 4; sta1 = 5;
                                //    }
                                //    else
                                //    {
                                //        sta = 6; sta1 = 6;
                                //    }
                                //    if (sStatus == "Catalogue Rework")
                                //    {
                                //        newResultList = (from x in newResultList where (x.Reworkcat != null && (x.ItemStatus == 0 || x.ItemStatus == 1)) select x).ToList();
                                //    }
                                //    else if (sStatus == "QC Rework")
                                //    {
                                //        newResultList = (from x in newResultList where (x.Rework != null && (x.ItemStatus == 2 || x.ItemStatus == 3)) select x).ToList();
                                //    }
                                //    else
                                //    {
                                //        newResultList = (from x in newResultList where (x.ItemStatus == sta || x.ItemStatus == sta1) select x).ToList();
                                //    }

                                //}
                            }

                        }
                    }
                }
            }
            return newResultList;
        }
        private List<Prosol_Datamaster> SearchFn(string[] strArr, string ColumnName, string sCode)
        {
            var fields = Fields.Include(strArr);
            if (sCode.Contains('*'))
            {


                var QryLst = new List<IMongoQuery>();
                var QryLst1 = new List<IMongoQuery>();
                string[] sepArr = sCode.Split('*');
                if (sepArr.Length > 2)
                {
                    foreach (string str in sepArr)
                    {
                        if (str != "")
                        {
                            var Qry1 = Query.Matches(ColumnName, BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd(), RegexOptions.IgnoreCase)));
                            QryLst.Add(Qry1);

                        }
                    }
                }
                else
                {
                    foreach (string str in sepArr)
                    {
                        if (str != "")
                        {
                            if (sCode.IndexOf('*') > 0)
                            {
                                //Start with
                                var Qry1 = Query.Matches(ColumnName, BsonRegularExpression.Create(new Regex(string.Format("^{0}", str.TrimStart().TrimEnd()), RegexOptions.IgnoreCase)));
                                QryLst.Add(Qry1);

                            }
                            else
                            {
                                //End with

                                var Qry1 = Query.Matches(ColumnName, BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd() + "$", RegexOptions.IgnoreCase)));
                                QryLst.Add(Qry1);


                            }

                        }
                    }


                }
                var query = Query.And(QryLst);
                var arrResult = _DatamasterRepository.FindAll(fields, query).ToList();
                return arrResult;
            }
            else
            {
                var Qry1 = Query.EQ(ColumnName, sCode.TrimStart().TrimEnd());
                var arrResult = _DatamasterRepository.FindAll(fields, Qry1).ToList();
                return arrResult;

            }

        }

        private List<Prosol_Datamaster> UserSearch(string[] strArr, string sUser)
        {
            var fields = Fields.Include(strArr);
            var Qry1 = Query.Or(Query.EQ("Catalogue.Name", sUser), Query.EQ("Review.Name", sUser), Query.EQ("Release.Name", sUser));
            var arrResult = _DatamasterRepository.FindAll(fields, Qry1).ToList();
            return arrResult;
        }
        private List<Prosol_Datamaster> StatusSearch(string[] strArr, string sStatus)
        {
            int sta = 0, sta1 = 1;
            if (sStatus == "Catalogue")
            {
                sta = 0; sta1 = 1;
            }
            else if (sStatus == "Clarification")
            {
                sta = -1; sta1 = -1;
            }
            else if (sStatus == "QC")
            {
                sta = 2; sta1 = 3;
            }
            else if (sStatus == "QA")
            {
                sta = 4; sta1 = 5;
            }
            else
            {
                sta = 6; sta1 = 6;
            }
            var fields = Fields.Include(strArr);
            var Qry1 = Query.Or(Query.EQ("ItemStatus", sta), Query.EQ("ItemStatus", sta1));
            if (sStatus == "Catalogue Rework")




            {
                Qry1 = Query.And(Query.NE("Reworkcat", BsonNull.Value), Query.Or(Query.EQ("ItemStatus", 0), Query.EQ("ItemStatus", 1)));


            }
            else if (sStatus == "QC Rework")




            {
                Qry1 = Query.And(Query.NE("Rework", BsonNull.Value), Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)));




            }

            var arrResult = _DatamasterRepository.FindAll(fields, Qry1).ToList();
            return arrResult;



        }

        public string[] showall_user()



        {
            var res = _usersRepository.AutoSearch1("UserName");
            return res;


        }



        public virtual int BulkDictionary(HttpPostedFileBase file)
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

            var LstNM = new List<Prosol_EquipDictionary>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].ToString() != "" && dr[1].ToString() != "" && dr[3].ToString() != "" && dr[4].ToString() != "" && dr[5].ToString() != "")
                {


                    var Mdl = new Prosol_EquipDictionary();
                    Mdl.Noun = dr[0].ToString();
                    Mdl.Modifier = dr[1].ToString();
                    Mdl.SpareList = dr[2].ToString();
                  
                    Mdl.NCP = dr[3].ToString();
                    Mdl.CP = dr[4].ToString();
                    Mdl.SpareCategory = dr[5].ToString();
                    LstNM.Add(Mdl);
                    cunt++;
                }
            }

            if (LstNM.Count > 0)
            {
                cunt = _EquipDictionaryRepository.Add(LstNM);

            }
            return cunt;

        }
        public List<Prosol_EquipDictionary> Getsparelist(string Noun, string Modifier)
        {
            var Qry1 = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier));
            var arrResult = _EquipDictionaryRepository.FindAll(Qry1).ToList();
            return arrResult;
        }
    }
    
    //public class searchObj
    //{
    //    public string SearchKey { set; get; }
    //    public string SearchColumn { set; get; }
    //}

}
