using Excel;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using Prosol.Core.ServiceReference3;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Collections;
using System.Reflection;

namespace Prosol.Core
{
    public class CatalogueService : ICatalogue
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
        private readonly IRepository<Prosol_Attribute> _attributeRepository;
        private readonly IRepository<Prosol_UOM> _uomRepository;
        private readonly IRepository<Prosol_Logic> _logicRepository;
        private readonly IRepository<Prosol_Request> _ProsolRequest;
        private readonly I_ItemRequest _ItemRequestService;
        private readonly IEmailSettings _Emailservc;
        private readonly IRepository<Prosol_Vendortype> _VendortypeRepository;
        private readonly IRepository<Prosol_Reftype> _ReftypeRepository;
        private readonly IRepository<Prosol_HSNModel> _HSNlistRepository;
        private readonly IRepository<Prosol_UNSPSC> _unspsclistRepository;
        private readonly IRepository<Prosol_ERPLog> _erplogRepository;
        private readonly INounModifier _NounModifiService;

        public CatalogueService(IRepository<Prosol_Vendor> vendorRepository,
            IRepository<Prosol_Datamaster> datamasterRepository,
            IRepository<Prosol_Sequence> seqRepository,
            IRepository<Prosol_UOMSettings> UOMRepository,
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
              IRepository<Prosol_HSNModel> HSNModel,
            IRepository<Prosol_Logic> logicRepository,
             IRepository<Prosol_Reftype> ReftypeRepository,
              IRepository<Prosol_UNSPSC> unspsclistRepository,
               IRepository<Prosol_ERPLog> erplogRepository,
            IRepository<Prosol_Request> ProsolRequest, IEmailSettings Emailservc, I_ItemRequest ItemRequestService, IRepository<Prosol_Vendortype> VendortypeRepository,
            INounModifier NMservice)
        {

            this._HSNlistRepository = HSNModel;
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
            this._logicRepository = logicRepository;
            this._ProsolRequest = ProsolRequest;
            this._Emailservc = Emailservc;
            this._ItemRequestService = ItemRequestService;
            this._VendortypeRepository = VendortypeRepository;
            this._ReftypeRepository = ReftypeRepository;
            this._unspsclistRepository = unspsclistRepository;
            this._erplogRepository = erplogRepository;
            _NounModifiService = NMservice;
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

        //    // **update vendoesupplier in datamaster**

        //    //foreach (DataRow dr in dt.Rows)
        //    //{
        //    //    List<Vendorsuppliers> lvs = new List<Vendorsuppliers>();
        //    //    string str = dr[0].ToString();
        //    //    string str2 = dr[1].ToString();
        //    //    string str3 = dr[3].ToString();
        //    //    var qy = Query.And(Query.EQ("Materialcode", dr[0].ToString()), Query.EQ("ItemStatus", 0));

        //    //    var ress = _DatamasterRepository.FindOne(qy);


        //    //    if (ress != null)
        //    //    {

        //    //        int cou = 1;
        //    //        for (int rr = 1; rr <= 5; rr = rr + 2)
        //    //        {
        //    //            if (dr[rr].ToString() != "" & dr[rr].ToString() != null)
        //    //            {
        //    //                Vendorsuppliers vs = new Vendorsuppliers();
        //    //                vs.slno = cou;
        //    //                vs.Code = "";
        //    //                vs.Name = dr[rr + 1].ToString().Trim().ToUpper();
        //    //                vs.Type = "SUPPLIER";
        //    //                vs.RefNo = dr[rr].ToString().Trim().ToUpper();
        //    //                vs.RefNoDup = null;
        //    //                vs.Refflag = "SUPPLIER PART NUMBER";
        //    //                vs.s = 0;
        //    //                vs.l = 1;
        //    //                vs.shortmfr = null;
        //    //                lvs.Add(vs);
        //    //                cou++;
        //    //            }
        //    //        }
        //    //        ress.Vendorsuppliers = lvs;
        //    //        _DatamasterRepository.Add(ress);
        //    //    }

        //    //}

        //    //var query = Query.EQ("Description", "Short_OEM");
        //    //var query = Query.EQ("Description", "Long");
        //    //var sort = SortBy.Ascending("Seq").Ascending("Description");
        //    //var seqList = _SequenceRepository.FindAll(query, sort).ToList();
        //    //var UOMSet = _UOMsettingRepository.FindOne();
        //    //var AbbrList = _AbbrevateRepository.FindAll();

        //    //foreach (DataRow drw in dt.Rows)
        //    //{
        //    //    if (drw[0].ToString() != "")
        //    //    {
        //    //        var Qry = Query.EQ("Materialcode", drw[0].ToString());
        //    //        var ress = _DatamasterRepository.FindOne(Qry);
        //    //        if (ress != null)
        //    //        {
        //    //            if (ress.Vendorsuppliers != null)
        //    //            {
        //    //                foreach (Vendorsuppliers slm in ress.Vendorsuppliers)
        //    //                {
        //    //                    if (slm.Refflag == "DRAWING & POSITION NUMBER")
        //    //                    {
        //    //                        slm.Refflag = drw[1].ToString();
        //    //                        slm.RefNo = drw[2].ToString();

        //    //                    }

        //    //                }

        //    //            }

        //    //            var Lst = checkDuplicate(ress);
        //    //            if (Lst != null && Lst.Count > 1)
        //    //            {
        //    //                foreach (Prosol_Datamaster dd in Lst)
        //    //                {

        //    //                    dd.Duplicates = ress.Itemcode;
        //    //                    _DatamasterRepository.Add(dd);

        //    //                }
        //    //            }

        //    //            var mdl = new Equipments();
        //    //            mdl.Name = drw[1].ToString();
        //    //            mdl.Modelno = drw[2].ToString();
        //    //            mdl.Tagno = drw[3].ToString();
        //    //            mdl.Serialno = drw[4].ToString();
        //    //            mdl.Additionalinfo = drw[5].ToString();
        //    //            mdl.Manufacturer = drw[6].ToString();

        //    //            ress.Equipment = mdl;

        //    //            var ress = new Prosol_Datamaster();
        //    //            ress.Itemcode = drw[0].ToString();
        //    //            ress.Shortdesc = drw[1].ToString();
        //    //            ress.Longdesc = drw[2].ToString();
        //    //            ress.Legacy = drw[3].ToString();
        //    //            ress.Itemcode = drw[1].ToString();
        //    //            ress.Modifier = drw[2].ToString();
        //    //            ress.Additionalinfo = drw[1].ToString();
        //    //            var userobj = new Prosol_UpdatedBy();
        //    //            var qry = Query.EQ("UserName", drw[6].ToString());
        //    //            var userdetails = _usersRepository.FindOne(qry);
        //    //            upd.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        //    //            ress.Catalogue = upd;
        //    //            ress.Review = upd;
        //    //            ress.Release = upd;

        //    //            ress.ItemStatus = 4;
        //    //            ress.Shortdesc = ShortDesc(ress, seqList, UOMSet, AbbrList);
        //    //            ress.Longdesc = LongDesc(ress, seqList, UOMSet);
        //    //            ress.CreatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        //    //            var lmd = new Prosol_ERPInfo();
        //    //            lmd.Itemcode = drw[0].ToString();
        //    //            lmd.Plant = "CP";
        //    //            lmd.Plant_ = "Saudi Chemical Plant";
        //    //            _erpRepository.Add(lmd);

        //    //            if (ress.Equipment != null && ress.Equipment.Modelno != "")
        //    //            {
        //    //                ress.Equipment.Modelno = drw[1].ToString();
        //    //            }
        //    //            ress.Duplicates = drw[1].ToString();
        //    //            _DatamasterRepository.Add(ress);
        //    //            cunt++;

        //    //            // }
        //    //        }

        //    //    }
        //    //}
        //    //var qry = Query.And(Query.NE("Duplicates", BsonNull.Value), Query.NE("Duplicates", ""));
        //    //var ListObj = _DatamasterRepository.FindAll().ToList();

        //    //foreach (Prosol_Datamaster ress in ListObj)
        //    //{
        //    //    var qy = Query.EQ("Itemcode", ress.Itemcode);
        //    //    var obj = _erpRepository.FindOne(qy);
        //    //    if (obj == null)
        //    //    {
        //    //        obj.Itemcode = ress.Itemcode;
        //    //        obj.Plant = "CP";
        //    //        obj.Plant_ = "Advanced Petrochemical Company";
        //    //        _erpRepository.Add(obj);

        //    //    }

        //    //    if (ress.Noun != null && ress.Modifier != "" && ress.Noun != "" && ress.Modifier != null)
        //    //    {
        //    //        if (ress.Vendorsuppliers != null)
        //    //        {
        //    //            foreach (Vendorsuppliers mdl in ress.Vendorsuppliers)
        //    //            {
        //    //                if (mdl.RefNo != null && mdl.RefNo != "")
        //    //                    mdl.RefNoDup = Regex.Replace(mdl.RefNo, @"[^\w\d]", "");
        //    //            }
        //    //            _DatamasterRepository.Add(ress);
        //    //        }

        //    //        var Lst = checkDuplicate(ress);
        //    //        if (Lst != null && Lst.Count > 1)
        //    //        {
        //    //            foreach (Prosol_Datamaster dd in Lst)
        //    //            {

        //    //                dd.Duplicates = ress.Itemcode;
        //    //                _DatamasterRepository.Add(dd);

        //    //            }
        //    //        }
        //    //    }



        //    //    cunt++;
        //    //}

        //    return cunt;

        //}
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

                var query1 = Query.And(Query.EQ("Itemcode", drw[0].ToString()));
                var mdl1 = _DatamasterRepository.FindOne(query1);
                if (mdl1 != null)
                {

                    mdl1.Itemcode = drw[0].ToString();
                    if(!string.IsNullOrEmpty(drw[1].ToString()))
                    mdl1.Noun = drw[1].ToString();
                    if (!string.IsNullOrEmpty(drw[2].ToString()))
                        mdl1.Modifier = drw[2].ToString();
                    if (!string.IsNullOrEmpty(drw[3].ToString()))
                        mdl1.UOM = drw[3].ToString();
                    if (!string.IsNullOrEmpty(drw[4].ToString()))
                        mdl1.Legacy = drw[4].ToString();
                    if (!string.IsNullOrEmpty(drw[5].ToString()))
                        mdl1.Legacy2 = drw[5].ToString();
                    if (!string.IsNullOrEmpty(drw[6].ToString()))
                        mdl1.Additionalinfo = drw[6].ToString();
                    if (!string.IsNullOrEmpty(drw[8].ToString()))
                        mdl1.Remarks = drw[8].ToString();
                    if (drw[9].ToString() != null && drw[9].ToString() != "")
                    {
                        mdl1.ItemStatus = Convert.ToInt32(drw[9].ToString());
                    }
                    if (!string.IsNullOrEmpty(drw[10].ToString()))
                        mdl1.batch = drw[10].ToString();

                    if (!string.IsNullOrEmpty(drw[7].ToString()))
                    {
                        var username = drw[7].ToString();

                        //  var query = Query.EQ("UserName", username);
                        var query = Query.Matches("UserName", BsonRegularExpression.Create(new Regex(username, RegexOptions.IgnoreCase)));
                        var userid = _usersRepository.FindOne(query);
                        userobj.UserId = userid.Userid.ToString();
                        userobj.Name = username;
                        userobj.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        mdl1.Catalogue = userobj;
                        mdl1.CreatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        mdl1.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }



                    _DatamasterRepository.Add(mdl1);


                    //var qy = Query.EQ("Itemcode", mdl1.Itemcode);
                    //var obj = _erpRepository.FindOne(qy);
                    //if (obj == null)
                    //{
                    //    var obj1 = new Prosol_ERPInfo();
                    //    obj1.Itemcode = mdl1.Itemcode;
                    //    obj1.Plant = userid.Plantcode[0];
                    //    _erpRepository.Add(obj1);

                    //}
                    cunt++;
                }
                //else if (mdl1.Additionalinfo == "" || mdl1.Additionalinfo == null)
                //{
                //    // ress.Additionalinfo = drw[6].ToString();
                //    mdl1.Additionalinfo = drw[6].ToString();
                //    //var q = Query.EQ("Materialcode", mdl1.Materialcode);
                //    var q = Query.EQ("Itemcode", mdl1.Itemcode);
                //    var up = Update.Set("Additionalinfo", drw[6].ToString());
                //    var flg = UpdateFlags.Multi;
                //    var result = _DatamasterRepository.Update(q, up, flg);
                //    cunt++;
                //}
                //else
                //{
                //    ress.Materialcode = drw[0].ToString();
                //    ress.Itemcode = drw[0].ToString();
                //    ress.Noun = drw[1].ToString();
                //    ress.Modifier = drw[2].ToString();
                //    ress.UOM = drw[3].ToString();
                //    ress.Legacy = drw[4].ToString();
                //    ress.Legacy2 = drw[5].ToString();
                //    ress.Additionalinfo = drw[6].ToString();
                //    ress.Remarks = drw[8].ToString();
                //    ress.ItemStatus = Convert.ToInt32(drw[9].ToString());
                //    ress.batch = drw[10].ToString();
                //    var username = drw[7].ToString();
                //    var query = Query.Matches("UserName", BsonRegularExpression.Create(new Regex(username, RegexOptions.IgnoreCase)));
                //    // var query = Query.EQ("UserName", username);
                //    var userid = _usersRepository.FindOne(query);
                //    userobj.UserId = userid.Userid.ToString();
                //    userobj.Name = username;
                //    userobj.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //    ress.Catalogue = userobj;
                //    ress.CreatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //    ress.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //    _DatamasterRepository.Add(ress);


                //    var qy = Query.EQ("Itemcode", ress.Itemcode);
                //    var obj = _erpRepository.FindOne(qy);
                //    if (obj == null)
                //    {
                //        var obj1 = new Prosol_ERPInfo();
                //        obj1.Itemcode = ress.Itemcode;
                //        obj1.Plant = userid.Plantcode[0];
                //        _erpRepository.Add(obj1);

                //    }
                //    cunt++;
                //}
            }
            return cunt;
        }

        public virtual int BulkSingleAttribute(HttpPostedFileBase file)
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
            DataTable dt1 = res.Tables[0];

            foreach (DataRow drw1 in dt1.Rows)
            {
                var query1 = Query.Or(Query.EQ("Itemcode", drw1[0].ToString()), Query.EQ("Materialcode", drw1[0].ToString()));
                var mdl = _DatamasterRepository.FindOne(query1);
                if (mdl != null)
                {
                    int idx = mdl.Characteristics.FindIndex(i => i.Characteristic == drw1[1].ToString());
                    mdl.Characteristics[idx].Value = drw1[2].ToString();
                    _DatamasterRepository.Add(mdl);
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
            DataTable resTbl = dt.AsEnumerable().GroupBy(x => x.Field<string>("Itemcode")).Select(x => x.First()).CopyToDataTable();
            foreach (DataRow dr in resTbl.Rows)
            {


                if (dr[0].ToString() != "")
                {

                    var attrList = new List<Prosol_AttributeList>();

                    attrList = (from DataRow drw in dt.Rows
                                where drw["Itemcode"].ToString() == dr[0].ToString()


                                select new Prosol_AttributeList()
                                {


                                    Characteristic = drw["CHARACTERISTIC"].ToString(),
                                    Value = drw["VALUES"].ToString(),
                                    UOM = drw["UNIT"].ToString(),
                                    SourceUrl = drw["SourceUrl"].ToString(),
                                    Source = drw["Source"].ToString()
                                }).ToList();


                    var query = Query.Or(Query.EQ("Itemcode", dr[0].ToString()), Query.EQ("Materialcode", dr[0].ToString()));
                    var mdl = _DatamasterRepository.FindOne(query);
                    mdl.Characteristics = null;
                    _DatamasterRepository.Add(mdl);
                    if (mdl != null)
                    {
                        var l = new List<Prosol_AttributeList>();
                        foreach (Prosol_AttributeList v in attrList)
                        {
                            var m = new Prosol_AttributeList();
                            //var d = Query.And(Query.EQ("Noun", mdl.Noun), Query.EQ("Modifier", mdl.Modifier), Query.EQ("Characteristic", v.Characteristic));
                            //var m1 = _CharacteristicRepository.FindOne(d);

                            m.Characteristic = v.Characteristic;
                            m.Value = v.Value;
                            m.UOM = v.UOM;
                            m.ShortSquence = 0;
                            m.Squence = 0;
                            m.Source = v.Source;
                            m.SourceUrl = v.SourceUrl;
                            l.Add(m);


                        }
                        mdl.Characteristics = l;
                        var res1 = _DatamasterRepository.Add(mdl);
                        if (!res1)
                        {

                        }
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
            DataTable dt1 = res.Tables[0];

            //DataTable resTbl = dt1.AsEnumerable().GroupBy(x => x.Field<string>("Itemcode")).Select(x => x.First()).CopyToDataTable();

            //var LstNM = new List<Prosol_Datamaster>();
            foreach (DataRow drw1 in dt1.Rows)
            {

                var query1 = Query.And(Query.EQ("Materialcode", drw1[0].ToString()));
                var mdl1 = _DatamasterRepository.FindOne(query1);

                if (mdl1 != null)
                {
                    int flg = 0; int inc = 0, sup = 0;
                    if (mdl1.Vendorsuppliers != null && mdl1.Vendorsuppliers.Count > 0)
                    {

                        foreach (var v in mdl1.Vendorsuppliers)
                        {

                            //if (!string.IsNullOrEmpty(drw1["NAME_" + inc].ToString()) || !string.IsNullOrEmpty(drw1["NO_" + inc].ToString()))
                            //{
                            v.Type = drw1["LABEL_" + inc].ToString();
                            v.Name = drw1["NAME_" + inc].ToString();
                            v.Refflag = drw1["FLAG_" + inc].ToString();
                            v.RefNo = drw1["NO_" + inc].ToString();
                            v.s = 1;
                            v.l = 1;
                            //   }
                            inc++;


                        }
                        while (dt1.Columns.Contains("NAME_" + inc) && (!string.IsNullOrEmpty(drw1["NAME_" + inc].ToString()) || !string.IsNullOrEmpty(drw1["NO_" + inc].ToString()) || !string.IsNullOrEmpty(drw1["FLAG_" + inc].ToString())))
                        {
                            if (mdl1.Vendorsuppliers == null)
                                mdl1.Vendorsuppliers = new List<Vendorsuppliers>();

                            while (mdl1.Vendorsuppliers.Count <= inc)
                            {
                                mdl1.Vendorsuppliers.Add(new Vendorsuppliers()); 
                            }

                            var v = new Vendorsuppliers();
                            v.Type = drw1["LABEL_" + inc].ToString();
                            v.Name = drw1["NAME_" + inc].ToString();
                            v.Refflag = drw1["FLAG_" + inc].ToString();
                            v.RefNo = drw1["NO_" + inc].ToString();
                            v.s = 1;
                            v.l = 1;

                            //mdl1.Vendorsuppliers.Add(v);
                            mdl1.Vendorsuppliers[inc] = v;

                            inc++;

                        }
                    }
                    else
                    {
                        while (dt1.Columns.Contains("NAME_" + inc) && (!string.IsNullOrEmpty(drw1["NAME_" + inc].ToString()) || !string.IsNullOrEmpty(drw1["NO_" + inc].ToString())))
                        {
                            if (mdl1.Vendorsuppliers == null)
                                mdl1.Vendorsuppliers = new List<Vendorsuppliers>();

                            while (mdl1.Vendorsuppliers.Count <= inc)
                            {
                                mdl1.Vendorsuppliers.Add(new Vendorsuppliers()); 
                            }

                            var v = new Vendorsuppliers();
                            v.Type = drw1["LABEL_" + inc].ToString();
                            v.Name = drw1["NAME_" + inc].ToString();
                            v.Refflag = drw1["FLAG_" + inc].ToString();
                            v.RefNo = drw1["NO_" + inc].ToString();
                            v.s = 1;
                            v.l = 1;

                            //mdl1.Vendorsuppliers.Add(v);
                            mdl1.Vendorsuppliers[inc] = v;

                            inc++;
                        }
                    }
                }

                //if (mdl1.Vendorsuppliers != null)
                //{
                //    foreach (var v in mdl1.Vendorsuppliers)
                //    {
                //        if (!string.IsNullOrEmpty(v.Name))
                //        {
                //            var q = Query.EQ("Name", v.Name);
                //            var m = _VendorRepository.FindOne(q);
                //            if (m == null)
                //            {
                //                return "Itemcode:- " + mdl1.Itemcode + " Please add Vendor in VendorMaster " + v.Name;
                //            }
                //        }

                //    }
                //}

                //if ((mdl2.Formatted == 0 || mdl2.Formatted == 2))
                //{
                //    int ven = 0;
                //    if (mdl1.Vendorsuppliers != null)
                //    {
                //        foreach (var v in mdl1.Vendorsuppliers)
                //        {
                //            if (v.s == 1)
                //            {
                //                ven = 1;
                //            }

                //        }
                //    }
                //    if (ven == 0)
                //    {
                //        return "Itemcode:- " + mdl1.Itemcode + " Please Check OPM,OEM Vendor";

                //    }
                //}

                _DatamasterRepository.Add(mdl1);
                cunt++;
            }

            return cunt;

        }

        //OLD

        //public virtual int BulkVendor(HttpPostedFileBase file)
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
        //    DataTable dt1 = res.Tables[0];

        //    //DataTable resTbl = dt1.AsEnumerable().GroupBy(x => x.Field<string>("Itemcode")).Select(x => x.First()).CopyToDataTable();

        //    //var LstNM = new List<Prosol_Datamaster>();
        //    foreach (DataRow drw1 in dt1.Rows)
        //    {

        //        var query1 = Query.And(Query.EQ("Materialcode", drw1[0].ToString()));
        //        var mdl1 = _DatamasterRepository.FindOne(query1);

        //        if (mdl1 != null)
        //        {
        //            int flg = 0; int inc = 0, sup = 0;
        //            if (mdl1.Vendorsuppliers != null && mdl1.Vendorsuppliers.Count > 0)
        //            {

        //                foreach (var v in mdl1.Vendorsuppliers)
        //                {

        //                    //if (!string.IsNullOrEmpty(drw1["NAME_" + inc].ToString()) || !string.IsNullOrEmpty(drw1["NO_" + inc].ToString()))
        //                    //{
        //                    v.Type = drw1["LABEL_" + inc].ToString();
        //                    v.Name = drw1["NAME_" + inc].ToString();
        //                    v.Refflag = drw1["FLAG_" + inc].ToString();
        //                    v.RefNo = drw1["NO_" + inc].ToString();
        //                    v.s = 1;
        //                    v.l = 1;
        //                    //   }
        //                    inc++;


        //                }
        //                while (dt1.Columns.Contains("NAME_" + inc) && (!string.IsNullOrEmpty(drw1["NAME_" + inc].ToString()) || !string.IsNullOrEmpty(drw1["NO_" + inc].ToString()) || !string.IsNullOrEmpty(drw1["FLAG_" + inc].ToString())))
        //                {
        //                    var v = new Vendorsuppliers();
        //                    v.Type = drw1["LABEL_" + inc].ToString();
        //                    v.Name = drw1["NAME_" + inc].ToString();
        //                    v.Refflag = drw1["FLAG_" + inc].ToString();
        //                    v.RefNo = drw1["NO_" + inc].ToString();
        //                    //v.s = Convert.ToInt16(drw1["S_" + inc]);
        //                    //v.l = Convert.ToInt16(drw1["L_" + inc]);
        //                    v.s = 1;
        //                    v.l = 1;
        //                    inc++;
        //                    mdl1.Vendorsuppliers.Add(v);
        //                }
        //            }
        //            else
        //            {
        //                while (dt1.Columns.Contains("NAME_" + inc) && (!string.IsNullOrEmpty(drw1["NAME_" + inc].ToString()) || !string.IsNullOrEmpty(drw1["NO_" + inc].ToString())))
        //                {
        //                    mdl1.Vendorsuppliers = new List<Vendorsuppliers>();
        //                    var v = new Vendorsuppliers();
        //                    v.Type = drw1["LABEL_" + inc].ToString();
        //                    v.Name = drw1["NAME_" + inc].ToString();
        //                    v.Refflag = drw1["FLAG_" + inc].ToString();
        //                    v.RefNo = drw1["NO_" + inc].ToString();
        //                    //v.s = Convert.ToInt16(drw1["S_" + inc]);
        //                    //v.l = Convert.ToInt16(drw1["L_" + inc]);
        //                    v.s = 1;
        //                    v.l = 1;
        //                    mdl1.Vendorsuppliers.Add(v);
        //                    inc++;
        //                }
        //            }
        //        }

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

        //        _DatamasterRepository.Add(mdl1);
        //        cunt++;
        //    }

        //    return cunt;

        //}

        //public virtual int BulkVendor(HttpPostedFileBase file)
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

        //    DataTable resTbl = dt.AsEnumerable().GroupBy(x => x.Field<string>("Itemcode")).Select(x => x.First()).CopyToDataTable();

        //    //  var LstNM = new List<Prosol_Datamaster>();
        //    foreach (DataRow dr in resTbl.Rows)
        //    {
        //        if (dr[0].ToString() != "")
        //        {

        //            var attrList = new List<Vendorsuppliers>();
        //            attrList = (from DataRow drw in dt.Rows
        //                        where drw["Itemcode"].ToString() == dr[0].ToString()
        //                        select new Vendorsuppliers()
        //                        {
        //                            Type = drw["Vendor Type"].ToString(),
        //                            Name = drw["Name"].ToString(),
        //                            Refflag = drw["Ref Flag"].ToString(),
        //                            RefNo = drw["Ref No"].ToString(),
        //                            RefNoDup = drw["Ref No"] != null ? Regex.Replace(drw["Ref No"].ToString(), @"[^\w\d]", "") : "",
        //                            s = Convert.ToInt16(drw["S"]),
        //                            l = Convert.ToInt16(drw["L"])
        //                        }).ToList();

        //            var query = Query.EQ("Itemcode", dr[0].ToString());
        //            var mdl = _DatamasterRepository.FindOne(query);
        //            if (mdl != null)
        //            {

        //                mdl.Vendorsuppliers = attrList;

        //                _DatamasterRepository.Add(mdl);
        //                cunt++;
        //            }

        //        }
        //    }

        //    return cunt;

        //}

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

            DataTable resTbl = dt.AsEnumerable().GroupBy(x => x.Field<string>("Itemcode")).Select(x => x.First()).CopyToDataTable();


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
                    //mdl.Soureurl = dr[7].ToString();
                    mdl.ENS = Convert.ToInt32(dr[7].ToString());
                    mdl.EMS = Convert.ToInt32(dr[8].ToString());
                    mdl.SuperiorEquipment = dr[9].ToString();

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

        //public virtual int BulkShortLong(HttpPostedFileBase file)
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


        //        var query1 = Query.EQ("Materialcode", drw[0].ToString());
        //        var mdl1 = _DatamasterRepository.FindOne(query1);
        //        if (mdl1 != null)
        //        {

        //            var ListCharas = mdl1.Characteristics;

        //            var lstCharateristics = new List<Prosol_AttributeList>();
        //            if (ListCharas != null && ListCharas.Count > 0)
        //            {

        //                foreach (Prosol_AttributeList LstAtt in ListCharas)
        //                {
        //                    var AttrMdl = new Prosol_AttributeList();
        //                    AttrMdl.Characteristic = LstAtt.Characteristic;
        //                    AttrMdl.Value = LstAtt.Value;
        //                    AttrMdl.UOM = LstAtt.UOM;

        //                    var d = Query.And(Query.EQ("Noun", mdl1.Noun), Query.EQ("Modifier", mdl1.Modifier), Query.EQ("Characteristic", AttrMdl.Characteristic));
        //                    var m1 = _CharacteristicRepository.FindOne(d);
        //                    if (m1 != null)
        //                    {
        //                        AttrMdl.ShortSquence = m1.ShortSquence;
        //                        AttrMdl.Squence = m1.Squence;
        //                        AttrMdl.Source = LstAtt.Source;
        //                        lstCharateristics.Add(AttrMdl);
        //                    }

        //                }
        //            }
        //            mdl1.Shortdesc = ShortDesc(mdl1);
        //            mdl1.Shortdesc_ = ShortDesc1(mdl1);
        //            mdl1.Longdesc = LongDesc(mdl1);
        //            mdl1.MissingValue = MissingValue(mdl1);
        //            mdl1.EnrichedValue = PapulatedValue(mdl1);
        //            mdl1.exMissingValue = ExMissingValue(mdl1, mdl1.Longdesc);
        //            _DatamasterRepository.Add(mdl1);
        //            cunt++;
        //        }

        //    }


        //    return cunt;
        //}

        public virtual Tuple<int, string> BulkShortLong(HttpPostedFileBase file)
        {
            int count = 0;
            string failedItemCode = null;

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
                try
                {
                    var query1 = Query.Or(Query.EQ("Itemcode", drw[0].ToString()), Query.EQ("Materialcode", drw[0].ToString()));
                    var mdl1 = _DatamasterRepository.FindOne(query1);
                    if (mdl1 != null)
                    {
                        var ListCharas = mdl1.Characteristics;
                        var lstCharateristics = new List<Prosol_AttributeList>();

                        if (ListCharas != null && ListCharas.Count > 0)
                        {
                            foreach (Prosol_AttributeList LstAtt in ListCharas)
                            {
                                var AttrMdl = new Prosol_AttributeList
                                {
                                    Characteristic = LstAtt.Characteristic,
                                    Value = LstAtt.Value,
                                    UOM = LstAtt.UOM
                                };

                                var d = Query.And(Query.EQ("Noun", mdl1.Noun), Query.EQ("Modifier", mdl1.Modifier), Query.EQ("Characteristic", AttrMdl.Characteristic));
                                var m1 = _CharacteristicRepository.FindOne(d);

                                if (m1 != null)
                                {
                                    AttrMdl.ShortSquence = m1.ShortSquence;
                                    AttrMdl.Squence = m1.Squence;
                                    AttrMdl.Source = LstAtt.Source;
                                    lstCharateristics.Add(AttrMdl);
                                }
                            }
                        }

                        mdl1.Shortdesc = ShortDesc(mdl1);
                        //mdl1.Shortdesc_ = ShortDesc1(mdl1);
                        mdl1.Longdesc = LongDesc(mdl1);
                        mdl1.MissingValue = MissingValue(mdl1);
                        mdl1.EnrichedValue = PapulatedValue(mdl1);
                        mdl1.exMissingValue = ExMissingValue(mdl1, mdl1.Longdesc);
                        if (mdl1.ItemStatus == 0)
                            mdl1.ItemStatus = 1;

                        _DatamasterRepository.Add(mdl1);
                        count++;
                    }
                    else
                    {
                        failedItemCode = drw[0].ToString();
                    }
                }
                catch (Exception ex)
                {
                    failedItemCode = drw[0].ToString();
                    return new Tuple<int, string>(count, failedItemCode);
                }
            }

            return new Tuple<int, string>(count, failedItemCode);
        }
        public virtual Tuple<int, string> BulkUrl(HttpPostedFileBase file)
        {
            int count = 0;
            string failedItemCode = null;

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
                try
                {
                    var query1 = Query.Or(Query.EQ("Itemcode", drw[0].ToString()), Query.EQ("Materialcode", drw[0].ToString()));
                    var mdl1 = _DatamasterRepository.FindOne(query1);
                    if (mdl1 != null)
                    {
                        mdl1.Junk = null;
                        if (!string.IsNullOrEmpty(drw[1].ToString()))
                        {
                            mdl1.Specification = drw[1].ToString();
                        }
                        if (!string.IsNullOrEmpty(drw[2].ToString()))
                        {
                            mdl1.Maincode = drw[2].ToString();
                        }
                        if (!string.IsNullOrEmpty(drw[3].ToString()))
                        {
                            mdl1.Soureurl = drw[3].ToString();
                        }
                        if (!string.IsNullOrEmpty(drw[4].ToString()))
                        {
                            if (!string.IsNullOrEmpty(drw[5].ToString()))
                            {
                                var usrQry = Query.EQ("UserName", drw[5].ToString());
                                var usrInfo = _usersRepository.FindOne(usrQry);
                                var usr = new Prosol_UpdatedBy();
                                usr.Name = usrInfo.UserName;
                                usr.UserId = usrInfo.Userid;
                                usr.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                                if (drw[4].ToString() == "Catalogue Saved")
                                {
                                    mdl1.ItemStatus = 1;
                                    mdl1.Catalogue = usr;
                                }
                                else if (drw[4].ToString() == "QC Pending")
                                {
                                    mdl1.ItemStatus = 2;
                                    mdl1.Review = usr;
                                }
                                else if (drw[4].ToString() == "QC Saved")
                                {
                                    mdl1.ItemStatus = 3;
                                    mdl1.Review = usr;
                                }
                                else if (drw[4].ToString() == "Released")
                                {
                                    mdl1.ItemStatus = 6;
                                    mdl1.Release = usr;
                                }
                                else
                                {
                                    mdl1.ItemStatus = 0;
                                    mdl1.Catalogue = usr;
                                }
                            }
                        }
                        _DatamasterRepository.Add(mdl1);
                        count++;
                    }
                    else
                    {
                        failedItemCode = drw[0].ToString();
                    }
                }
                catch (Exception ex)
                {
                    failedItemCode = drw[0].ToString();
                    return new Tuple<int, string>(count, failedItemCode);
                }
            }

            return new Tuple<int, string>(count, failedItemCode);
        }

        //public virtual int BulkShortLong(HttpPostedFileBase file)
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


        //        var query1 = Query.And(Query.EQ("Materialcode", drw[0].ToString()));
        //        var mdl1 = _DatamasterRepository.FindOne(query1);
        //        if (mdl1 != null)
        //        {
        //            var ListCharas = mdl1.Characteristics;

        //            var lstCharateristics = new List<Prosol_AttributeList>();
        //            if (ListCharas != null && ListCharas.Count > 0)
        //            {

        //                foreach (Prosol_AttributeList LstAtt in ListCharas)
        //                {
        //                    var AttrMdl = new Prosol_AttributeList();
        //                    AttrMdl.Characteristic = LstAtt.Characteristic;
        //                    AttrMdl.Value = LstAtt.Value;
        //                    AttrMdl.UOM = LstAtt.UOM;

        //                    var d = Query.And(Query.EQ("Noun", mdl1.Noun), Query.EQ("Modifier", mdl1.Modifier), Query.EQ("Characteristic", AttrMdl.Characteristic));
        //                    var m1 = _CharacteristicRepository.FindOne(d);
        //                    if (m1 != null)
        //                    {
        //                        AttrMdl.ShortSquence = m1.ShortSquence;
        //                        AttrMdl.Squence = m1.Squence;
        //                        AttrMdl.Source = LstAtt.Source;
        //                        lstCharateristics.Add(AttrMdl);
        //                    }

        //                }
        //            }
        //            mdl1.Characteristics = lstCharateristics;
        //            if (drw[0].ToString() == "10011136")
        //            {

        //            }
        //            try
        //            {
        //                mdl1.Shortdesc = ShortDesc(mdl1);
        //                mdl1.Shortdesc_ = ShortDesc1(mdl1);
        //                mdl1.Longdesc = LongDesc(mdl1);


        //                //gan:
        //                //var Qry = Query.And(Query.NE("Itemcode", mdl1.Itemcode), Query.EQ("Shortdesc", mdl1.Shortdesc), Query.Or(Query.EQ("Vendorsuppliers", BsonNull.Value), Query.EQ("Vendorsuppliers", new BsonArray())));

        //                //var DaList = _DatamasterRepository.FindAll(Qry).ToList();
        //                //if (DaList != null && DaList.Count > 0)
        //                //{

        //                //    foreach (Prosol_Datamaster dm in DaList)
        //                //    {
        //                //        if (dm.Characteristics != null && mdl1.Characteristics != null)
        //                //        {
        //                //            foreach (Prosol_AttributeList mdol in dm.Characteristics)
        //                //            {
        //                //                int lnx = 0;
        //                //                foreach (Prosol_AttributeList mdl in mdl1.Characteristics)
        //                //                {
        //                //                    if (mdl.Characteristic == mdol.Characteristic)
        //                //                    {
        //                //                        if (mdl.Value == null)
        //                //                            mdl.Value = "";
        //                //                        if (mdol.Value == null)
        //                //                            mdol.Value = "";
        //                //                        if (mdl.Value != mdol.Value)
        //                //                        {
        //                //                            if (lnx > 1)
        //                //                            {
        //                //                                for (int dx = lnx; dx <= lnx; dx--)
        //                //                                {
        //                //                                    if (mdl1.Characteristics[dx - 1].Value != "" && mdl1.Characteristics[dx - 1].Value != null)
        //                //                                    {
        //                //                                        mdl1.Characteristics[dx - 1].Value = "";
        //                //                                        break;
        //                //                                    }
        //                //                                }
        //                //                                mdl1.Shortdesc = ShortDesc(mdl1);
        //                //                                goto gan;
        //                //                            }


        //                //                        }

        //                //                    }
        //                //                    lnx++;
        //                //                }
        //                //            }


        //                //        }
        //                //    }

        //                //}
        //                //if (mdl1.Noun != null && mdl1.Modifier != null && mdl1.Noun != "" && mdl1.Modifier != "" && mdl1.Characteristics!=null)
        //                //{
        //                //    mdl1.Longdesc = LongDesc(mdl1);

        //                //    _DatamasterRepository.Add(mdl1);
        //                //}

        //                var dupList = checkDuplicate(mdl1);

        //                if (dupList != null && dupList.Count > 0)
        //                {
        //                    if (dupList[0].Duplicates == null || dupList[0].Duplicates == "")
        //                    {

        //                        dupList[0].Duplicates = dupList[0].Materialcode;
        //                        _DatamasterRepository.Add(dupList[0]);


        //                    }

        //                    mdl1.ItemStatus = 6;
        //                    mdl1.Materialcode = dupList[0].Duplicates;
        //                    mdl1.Duplicates = dupList[0].Duplicates;
        //                    mdl1.Noun = dupList[0].Noun;
        //                    mdl1.Modifier = dupList[0].Modifier;
        //                    mdl1.Shortdesc = dupList[0].Shortdesc;
        //                    mdl1.Shortdesc_ = dupList[0].Shortdesc_;
        //                    mdl1.Longdesc = dupList[0].Longdesc;
        //                    mdl1.UOM = dupList[0].UOM;

        //                    mdl1.Vendorsuppliers = dupList[0].Vendorsuppliers;
        //                    mdl1.Equipment = dupList[0].Equipment;

        //                    mdl1.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        //                    if (mdl1.CreatedOn == null)
        //                    {
        //                        mdl1.CreatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        //                    }
        //                    _DatamasterRepository.Add(mdl1);

        //                }
        //                else
        //                {
        //                    if (mdl1.Duplicates != "" && mdl1.Duplicates != null)
        //                    {

        //                        if (mdl1.Itemcode == mdl1.Duplicates)
        //                        {
        //                            //Parent remove duplicate
        //                            var chkQry = Query.EQ("Duplicates", mdl1.Duplicates);
        //                            var duList = _DatamasterRepository.FindAll(chkQry).ToList();
        //                            if (duList != null && duList.Count > 0)
        //                            {
        //                                foreach (Prosol_Datamaster ob in duList)
        //                                {
        //                                    if (ob.Itemcode != mdl1.Itemcode)s
        //                                    {
        //                                        ob.Duplicates = null;
        //                                        ob.ItemStatus = 0;
        //                                        ob.Materialcode = ob.Itemcode;
        //                                        mdl1.Vendorsuppliers = null;
        //                                        mdl1.Equipment = null;

        //                                        if (ob.Review == null)
        //                                        {
        //                                            var review = new Prosol_UpdatedBy();
        //                                            review.UserId = ob.Catalogue.UserId;
        //                                            review.Name = ob.Catalogue.Name;
        //                                            review.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        //                                            ob.Review = review;
        //                                        }
        //                                        if (ob.Release == null)
        //                                        {

        //                                            var rels = new Prosol_UpdatedBy();
        //                                            rels.UserId = ob.Catalogue.UserId;
        //                                            rels.Name = ob.Catalogue.Name;
        //                                            rels.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        //                                            ob.Release = rels;

        //                                        }

        //                                        _DatamasterRepository.Add(ob);
        //                                    }
        //                                }

        //                            }

        //                        }
        //                        else
        //                        {
        //                            //single child remove duplicate
        //                            var chkQry = Query.EQ("Duplicates", mdl1.Duplicates);
        //                            var duList = _DatamasterRepository.FindAll(chkQry).ToList();
        //                            if (duList != null && duList.Count == 2)
        //                            {
        //                                foreach (Prosol_Datamaster ob in duList)
        //                                {
        //                                    if (ob.Itemcode == mdl1.Duplicates)
        //                                    {
        //                                        ob.Duplicates = null;
        //                                        _DatamasterRepository.Add(ob);
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }

        //                    mdl1.Duplicates = null;
        //                    mdl1.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        //                    _DatamasterRepository.Add(mdl1);

        //                }
        //            }
        //            catch
        //            {

        //            }
        //            if (cunt == 449)
        //            {

        //            }
        //            cunt++;
        //        }

        //    }


        //    return cunt;
        //}


        public IEnumerable<Prosol_Vendor> GetVendorList(string term)
        {

            var query = Query.And(Query.EQ("Enabled", true), Query.Matches("Name", BsonRegularExpression.Create(new Regex(term, RegexOptions.IgnoreCase))));
            var arrResult = _VendorRepository.FindAll(query);
            return arrResult;
        }

        //private string ShortDesc(Prosol_Datamaster cat)
        //{

        //    string mfrref = "";

        //    var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
        //    var NMList = _nounModifierRepository.FindAll(FormattedQuery).ToList();

        //    var sort = SortBy.Ascending("Seq").Ascending("Description");
        //    var query = Query.EQ("Description", "Short_OEM");
        //    string ShortStr = "", strNM = "";

        //    var seqList = _SequenceRepository.FindAll(query, sort).ToList();
        //    var UOMSet = _UOMRepository.FindOne();

        //    var AbbrList = _abbreivateRepository.FindAll();
        //    //Short_Generic
        //    List<shortFrame> lst = new List<shortFrame>();
        //    foreach (Prosol_Sequence sq in seqList)
        //    {
        //        if (sq.Required == "Yes")
        //        {
        //            switch (sq.CatId)
        //            {
        //                case 101:

        //                    if (NMList.Formatted == 1)
        //                    {
        //                        //var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Noun, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                        //if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                        //    ShortStr += abbObj.Abbrevated + sq.Separator;
        //                        //else ShortStr += cat.Noun + sq.Separator;


        //                        if (NMList.Nounabv != null && NMList.Nounabv != "")
        //                            ShortStr += NMList.Nounabv + sq.Separator;
        //                        else ShortStr += cat.Noun + sq.Separator;
        //                    }
        //                    else
        //                    {
        //                        if (cat.Characteristics != null)
        //                        {
        //                            var sObj = cat.Characteristics.OrderBy(x => x.Squence).FirstOrDefault();
        //                            var abbObj = (from Abb in AbbrList where Abb.Value.Equals(sObj.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                ShortStr += abbObj.Abbrevated + sq.Separator;
        //                            else ShortStr += sObj.Value + sq.Separator;
        //                        }
        //                    }
        //                    strNM = ShortStr;
        //                    //var nounabbr = (from Abb in AbbrList where Abb.Value.Equals(cat.Noun, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                    //if (nounabbr != null && (nounabbr.Abbrevated != null && nounabbr.Abbrevated != ""))
        //                    //{
        //                    //    ShortStr += nounabbr.Abbrevated + sq.Separator;
        //                    //}
        //                    //else ShortStr += cat.Noun + sq.Separator;

        //                    break;
        //                case 102:
        //                    if (cat.Modifier != "NO DEFINER" && cat.Modifier != "NO MODIFIER")
        //                    {
        //                        if (NMList.Formatted == 1)
        //                        {

        //                            if (NMList.Modifierabv != null && NMList.Modifierabv != "")
        //                                ShortStr += NMList.Modifierabv + sq.Separator;
        //                            else ShortStr += cat.Modifier + sq.Separator;



        //                            strNM = ShortStr;
        //                        }
        //                    }
        //                    break;
        //                case 103:
        //                    int flg = 0;


        //                    //  int[] arrPos= new int[cat.Characteristics.Count];
        //                    //  string[] arrVal = new string[cat.Characteristics.Count];
        //                    int i = 0;
        //                    if (cat.Characteristics != null)
        //                    {
        //                        foreach (Prosol_AttributeList chM in cat.Characteristics.OrderBy(x => x.ShortSquence))
        //                        {
        //                            if (NMList.Formatted == 1 || flg == 1)
        //                            {

        //                                if (chM.Value != null && chM.Value != "")
        //                                {
        //                                    string strValue = "";
        //                                    var frmMdl = new shortFrame();
        //                                    frmMdl.position = chM.ShortSquence;


        //                                    if (chM.Value.Contains(','))
        //                                    {
        //                                        string tmpstr = "";
        //                                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                        {
        //                                            tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                        }
        //                                        else
        //                                        {
        //                                            string[] strsplt = chM.Value.Split(',');
        //                                            foreach (string str in strsplt)
        //                                            {
        //                                                abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                                if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                                    tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                                else tmpstr += str.TrimStart().TrimEnd() + ',';
        //                                            }
        //                                        }
        //                                        tmpstr = tmpstr.TrimEnd(',');
        //                                        if (UOMSet.Short_space == "with space")
        //                                        {
        //                                            if (chM.UOM != null && chM.UOM != "")
        //                                                strValue += tmpstr + " " + chM.UOM + sq.Separator;
        //                                            else strValue += tmpstr + sq.Separator;
        //                                        }
        //                                        else
        //                                        {
        //                                            if (chM.UOM != null && chM.UOM != "")
        //                                                strValue += tmpstr + chM.UOM + sq.Separator;
        //                                            else strValue += tmpstr + sq.Separator;
        //                                        }
        //                                        frmMdl.values = strValue;
        //                                    }
        //                                    else
        //                                    {
        //                                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                        {
        //                                            // Abbreivated
        //                                            if (UOMSet.Short_space == "with space")
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                                else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            else
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                                else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            frmMdl.values = strValue;
        //                                        }
        //                                        else
        //                                        {
        //                                            // Abbreivated not exist

        //                                            if (UOMSet.Short_space == "with space")
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += chM.Value.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                                else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            else
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += chM.Value.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                                else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            frmMdl.values = strValue;
        //                                        }
        //                                    }

        //                                    lst.Add(frmMdl);

        //                                    ShortStr = strNM;
        //                                    string pattern = " X ";
        //                                    foreach (shortFrame sMdl in lst)
        //                                    {
        //                                        foreach (Match match in Regex.Matches(sMdl.values, pattern))
        //                                        {
        //                                            sMdl.values = Regex.Replace(sMdl.values, pattern, match.Value.TrimEnd().TrimStart());
        //                                        }
        //                                        // ShortStr += sMdl.values;
        //                                        string pattern1 = " x ";
        //                                        foreach (Match match in Regex.Matches(sMdl.values, pattern1))
        //                                        {
        //                                            sMdl.values = Regex.Replace(sMdl.values, pattern1, match.Value.TrimEnd().TrimStart());
        //                                        }
        //                                        ShortStr += sMdl.values;
        //                                    }

        //                                    if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                    {
        //                                        ShortStr = ShortStr.Trim();
        //                                        char[] chr = sq.Separator.ToCharArray();
        //                                        ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                        while (ShortStr.Length > seqList[0].ShortLength)
        //                                        {
        //                                            int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                            if (lstIndx > -1)
        //                                            {
        //                                                if (lst.Count > 0)
        //                                                {

        //                                                    if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                    {
        //                                                        if (ShortStr.Substring(lstIndx).TrimStart(chr[0]) == lst[lst.Count - 1].values.TrimEnd(chr[0]))
        //                                                        {
        //                                                            lst.RemoveAt(lst.Count - 1);
        //                                                        }
        //                                                        else
        //                                                        {
        //                                                            int indx = lst[lst.Count - 1].values.TrimEnd(chr[0]).LastIndexOf(chr[0]);
        //                                                            lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx) + chr[0];
        //                                                        }
        //                                                    }
        //                                                }
        //                                                ShortStr = ShortStr.Remove(lstIndx);

        //                                            }
        //                                            else
        //                                            {
        //                                                lstIndx = ShortStr.LastIndexOf(' ');
        //                                                ShortStr = ShortStr.Remove(lstIndx);
        //                                                if (lst.Count > 0)
        //                                                {
        //                                                    int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                    lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                                }
        //                                            }

        //                                        }
        //                                        ShortStr = ShortStr + chr[0];
        //                                    }
        //                                    i++;
        //                                }
        //                            }
        //                            else flg = 1;
        //                        }
        //                        ShortStr = strNM;
        //                        foreach (shortFrame sMdl in lst.OrderBy(x => x.position))
        //                        {
        //                            ShortStr += sMdl.values;
        //                        }
        //                    }

        //                    break;

        //                case 104:

        //                    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    {
        //                        if (vs.s == 1)
        //                        {
        //                            if(vs.Name!=null && vs.Name != "")
        //                            {
        //                                var querry = Query.EQ("Name", vs.Name);
        //                                var shtmfr = _VendorRepository.FindOne(querry);
        //                                if (shtmfr != null)
        //                                {
        //                                    if (shtmfr.ShortDescName != null && shtmfr.ShortDescName != "")
        //                                    {
        //                                        vs.shortmfr = shtmfr.ShortDescName;
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                vs.shortmfr = vs.Name;
        //                            }

        //                            if (vs.shortmfr != null && vs.shortmfr != "" && vs.shortmfr != "undefined")
        //                            {
        //                                mfrref = vs.Name;
        //                                var frmMdl = new shortFrame();
        //                                frmMdl.position = 101;
        //                                frmMdl.values = vs.shortmfr + sq.Separator;
        //                                lst.Add(frmMdl);
        //                                ShortStr += vs.shortmfr + sq.Separator;
        //                                if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                {
        //                                    ShortStr = ShortStr.Trim();
        //                                    char[] chr = sq.Separator.ToCharArray();
        //                                    ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                    while (ShortStr.Length > seqList[0].ShortLength)
        //                                    {
        //                                        int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                        if (lstIndx > -1)
        //                                        {
        //                                            if (lst.Count > 0)
        //                                            {
        //                                                if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                    lst.RemoveAt(lst.Count - 1);
        //                                            }
        //                                            ShortStr = ShortStr.Remove(lstIndx);

        //                                        }
        //                                        else
        //                                        {
        //                                            lstIndx = ShortStr.LastIndexOf(' ');
        //                                            ShortStr = ShortStr.Remove(lstIndx);
        //                                            if (lst.Count > 0)
        //                                            {
        //                                                int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                            }
        //                                        }
        //                                    }
        //                                    ShortStr = ShortStr + chr[0];
        //                                }
        //                                break;
        //                            }

        //                        }
        //                    }
        //                    break;
        //                case 105:                         


        //                    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    {
        //                        if (vs.s == 1 && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined")
        //                        {
        //                            var frmMdl = new shortFrame();
        //                            frmMdl.position = 100;
        //                            frmMdl.values = vs.RefNo.Trim() + sq.Separator;
        //                            lst.Add(frmMdl);
        //                            // ShortStr = strNM;

        //                            ShortStr += vs.RefNo.Trim() + sq.Separator;
        //                            if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                            {
        //                                ShortStr = ShortStr.Trim();
        //                                char[] chr = sq.Separator.ToCharArray();
        //                                ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                while (ShortStr.Length > seqList[0].ShortLength)
        //                                {
        //                                    int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                    if (lstIndx > -1)
        //                                    {
        //                                        if (lst.Count > 0)
        //                                        {
        //                                            if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                lst.RemoveAt(lst.Count - 1);
        //                                        }
        //                                        ShortStr = ShortStr.Remove(lstIndx);

        //                                    }
        //                                    else
        //                                    {
        //                                        lstIndx = ShortStr.LastIndexOf(' ');
        //                                        ShortStr = ShortStr.Remove(lstIndx);
        //                                        if (lst.Count > 0)
        //                                        {
        //                                            int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                            lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                        }
        //                                    }

        //                                }
        //                                ShortStr = ShortStr + chr[0];
        //                            }
        //                            break;
        //                        }

        //                    }
        //                    break;
        //                case 106:
        //                    if (cat.Application != null && cat.Application != "")
        //                        ShortStr += cat.Application + sq.Separator;
        //                    break;
        //                case 107:
        //                    //if (cat.Drawingno != null && cat.Drawingno != "")
        //                    //    ShortStr += cat.Drawingno + sq.Separator;


        //                    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    {
        //                        if (vs.Refflag.ToUpper() == "DRAWING & POSITION NUMBER" && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined" && vs.Name == mfrref && vs.Name != "" && vs.Name != null)
        //                        {
        //                            ShortStr += vs.RefNo.Trim() + sq.Separator;
        //                            break;
        //                        }
        //                    }
        //                    break;
        //                case 108:
        //                    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "")
        //                        ShortStr += cat.Equipment.Name + sq.Separator;
        //                    break;
        //                case 109:
        //                    if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
        //                        ShortStr += cat.Equipment.Manufacturer + sq.Separator;
        //                    break;
        //                case 110:
        //                    if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "")
        //                        ShortStr += cat.Equipment.Modelno + sq.Separator;
        //                    break;
        //                case 111:
        //                    if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "")
        //                        ShortStr += cat.Equipment.Tagno + sq.Separator;
        //                    break;
        //                case 112:
        //                    if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "")
        //                        ShortStr += cat.Equipment.Serialno + sq.Separator;
        //                    break;
        //                case 113:
        //                    if (cat.Referenceno != null && cat.Referenceno != "")
        //                        ShortStr += cat.Referenceno + sq.Separator;
        //                    break;
        //                case 114:
        //                    if (cat.Additionalinfo != null && cat.Additionalinfo != "")
        //                        ShortStr += cat.Additionalinfo + sq.Separator;
        //                    break;
        //                case 115:
        //                    if (cat.Equipment != null && cat.Equipment.Additionalinfo != null && cat.Equipment.Additionalinfo != "")
        //                        ShortStr += cat.Equipment.Additionalinfo + sq.Separator;
        //                    break;


        //            }
        //            if (!checkLength(ShortStr, seqList[0].ShortLength))
        //            {
        //                ShortStr = ShortStr.Trim();
        //                char[] chr = sq.Separator.ToCharArray();
        //                ShortStr = ShortStr.TrimEnd(chr[0]);
        //                while (ShortStr.Length > seqList[0].ShortLength)
        //                {
        //                    int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                    ShortStr = ShortStr.Remove(lstIndx);
        //                }
        //                break;
        //            }
        //        }
        //    }
        //    if (ShortStr.Length != seqList[0].ShortLength)
        //    {
        //        ShortStr = ShortStr.Trim();
        //        int lsIndx = ShortStr.Length;
        //        string str = ShortStr.Substring(lsIndx - 1, 1);
        //        if (str == seqList[0].Separator.Trim())
        //        {
        //            ShortStr = ShortStr.Remove(lsIndx - 1);
        //        }
        //    }
        //    return ShortStr.ToUpper();
        //}


        //CNX
        //private string ShortDesc(Prosol_Datamaster cat)
        //{

        //    string mfrref = "";

        //    var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
        //    var NMList = _nounModifierRepository.FindAll(FormattedQuery).ToList();

        //    var sort = SortBy.Ascending("Seq").Ascending("Description");
        //    var query = Query.EQ("Description", "Short_OEM");
        //    string ShortStr = "", strNM = "";

        //    var seqList = _SequenceRepository.FindAll(query, sort).ToList();
        //    var UOMSet = _UOMRepository.FindOne();

        //    var AbbrList = _abbreivateRepository.FindAll();
        //    //Short_Generic
        //    List<shortFrame> lst = new List<shortFrame>();
        //    foreach (Prosol_Sequence sq in seqList)
        //    {
        //        if (sq.Required == "Yes")
        //        {
        //            switch (sq.CatId)
        //            {
        //                case 101:

        //                    if (NMList.Formatted == 1)
        //                    {
        //                        //var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Noun, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                        //if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                        //    ShortStr += abbObj.Abbrevated + sq.Separator;
        //                        //else ShortStr += cat.Noun + sq.Separator;


        //                        if (NMList.Nounabv != null && NMList.Nounabv != "")
        //                            ShortStr += NMList.Nounabv + sq.Separator;
        //                        else ShortStr += cat.Noun + sq.Separator;
        //                    }
        //                    else
        //                    {
        //                        if (cat.Characteristics != null)
        //                        {
        //                            var sObj = cat.Characteristics.OrderBy(x => x.Squence).FirstOrDefault();
        //                            var abbObj = (from Abb in AbbrList where Abb.Value.Equals(sObj.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                ShortStr += abbObj.Abbrevated + sq.Separator;
        //                            else ShortStr += sObj.Value + sq.Separator;
        //                        }
        //                    }
        //                    strNM = ShortStr;
        //                    //var nounabbr = (from Abb in AbbrList where Abb.Value.Equals(cat.Noun, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                    //if (nounabbr != null && (nounabbr.Abbrevated != null && nounabbr.Abbrevated != ""))
        //                    //{
        //                    //    ShortStr += nounabbr.Abbrevated + sq.Separator;
        //                    //}
        //                    //else ShortStr += cat.Noun + sq.Separator;

        //                    break;
        //                case 102:
        //                    if (cat.Modifier != "NO DEFINER" && cat.Modifier != "NO MODIFIER")
        //                    {
        //                        if (NMList.Formatted == 1)
        //                        {

        //                            if (NMList.Modifierabv != null && NMList.Modifierabv != "")
        //                                ShortStr += NMList.Modifierabv + sq.Separator;
        //                            else ShortStr += cat.Modifier + sq.Separator;



        //                            strNM = ShortStr;
        //                        }
        //                    }
        //                    break;
        //                case 103:
        //                    int flg = 0;


        //                    //  int[] arrPos= new int[cat.Characteristics.Count];
        //                    //  string[] arrVal = new string[cat.Characteristics.Count];
        //                    int i = 0;
        //                    if (cat.Characteristics != null)
        //                    {
        //                        foreach (Prosol_AttributeList chM in cat.Characteristics.OrderBy(x => x.ShortSquence))
        //                        {
        //                            if (NMList.Formatted == 1 || flg == 1)
        //                            {

        //                                if (chM.Value != null && chM.Value != "")
        //                                {
        //                                    string strValue = "";
        //                                    var frmMdl = new shortFrame();
        //                                    frmMdl.position = chM.Squence;


        //                                    if (chM.Value.Contains(','))
        //                                    {
        //                                        string tmpstr = "";
        //                                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                        {
        //                                            tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                        }
        //                                        else
        //                                        {
        //                                            string[] strsplt = chM.Value.Split(',');
        //                                            foreach (string str in strsplt)
        //                                            {
        //                                                abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                                if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                                    tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                                else tmpstr += str.TrimStart().TrimEnd() + ',';
        //                                            }
        //                                        }
        //                                        tmpstr = tmpstr.TrimEnd(',');
        //                                        if (UOMSet.Short_space == "with space")
        //                                        {
        //                                            if (chM.UOM != null && chM.UOM != "")
        //                                                strValue += tmpstr + " " + chM.UOM + sq.Separator;
        //                                            else strValue += tmpstr + sq.Separator;
        //                                        }
        //                                        else
        //                                        {
        //                                            if (chM.UOM != null && chM.UOM != "")
        //                                                strValue += tmpstr + chM.UOM + sq.Separator;
        //                                            else strValue += tmpstr + sq.Separator;
        //                                        }
        //                                        frmMdl.values = strValue;
        //                                    }
        //                                    else
        //                                    {
        //                                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                        {
        //                                            // Abbreivated
        //                                            if (UOMSet.Short_space == "with space")
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                                else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            else
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                                else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            frmMdl.values = strValue;
        //                                        }
        //                                        else
        //                                        {
        //                                            // Abbreivated not exist

        //                                            if (UOMSet.Short_space == "with space")
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += chM.Value.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                                else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            else
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += chM.Value.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                                else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            frmMdl.values = strValue;
        //                                        }
        //                                    }

        //                                    lst.Add(frmMdl);

        //                                    ShortStr = strNM;
        //                                    string pattern = " X ";
        //                                    foreach (shortFrame sMdl in lst)
        //                                    {
        //                                        foreach (Match match in Regex.Matches(sMdl.values, pattern))
        //                                        {
        //                                            sMdl.values = Regex.Replace(sMdl.values, pattern, match.Value.TrimEnd().TrimStart());
        //                                        }
        //                                        // ShortStr += sMdl.values;
        //                                        string pattern1 = " x ";
        //                                        foreach (Match match in Regex.Matches(sMdl.values, pattern1))
        //                                        {
        //                                            sMdl.values = Regex.Replace(sMdl.values, pattern1, match.Value.TrimEnd().TrimStart());
        //                                        }
        //                                        ShortStr += sMdl.values;
        //                                    }

        //                                    if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                    {
        //                                        ShortStr = ShortStr.Trim();
        //                                        char[] chr = sq.Separator.ToCharArray();
        //                                        ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                        while (ShortStr.Length > seqList[0].ShortLength)
        //                                        {
        //                                            int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                            if (lstIndx > -1)
        //                                            {
        //                                                if (lst.Count > 0)
        //                                                {

        //                                                    if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                    {
        //                                                        if (ShortStr.Substring(lstIndx).TrimStart(chr[0]) == lst[lst.Count - 1].values.TrimEnd(chr[0]))
        //                                                        {
        //                                                            lst.RemoveAt(lst.Count - 1);
        //                                                        }
        //                                                        else
        //                                                        {
        //                                                            int indx = lst[lst.Count - 1].values.TrimEnd(chr[0]).LastIndexOf(chr[0]);
        //                                                            lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx) + chr[0];
        //                                                        }
        //                                                    }
        //                                                }
        //                                                ShortStr = ShortStr.Remove(lstIndx);

        //                                            }
        //                                            else
        //                                            {
        //                                                lstIndx = ShortStr.LastIndexOf(' ');
        //                                                ShortStr = ShortStr.Remove(lstIndx);
        //                                                if (lst.Count > 0)
        //                                                {
        //                                                    int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                    lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                                }
        //                                            }

        //                                        }
        //                                        ShortStr = ShortStr + chr[0];
        //                                    }
        //                                    i++;
        //                                }
        //                            }
        //                            else flg = 1;
        //                        }
        //                        ShortStr = strNM;
        //                        foreach (shortFrame sMdl in lst.OrderBy(x => x.position))
        //                        {
        //                            ShortStr += sMdl.values;
        //                        }
        //                    }

        //                    break;

        //                case 104:

        //                    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    {
        //                        if (vs.s == 1)
        //                        {
        //                            if (vs.Name != null && vs.Name != "")
        //                            {
        //                                var querry = Query.EQ("Name", vs.Name);
        //                                var shtmfr = _VendorRepository.FindOne(querry);
        //                                if (shtmfr != null)
        //                                {
        //                                    if (shtmfr.ShortDescName != null && shtmfr.ShortDescName != "")
        //                                    {
        //                                        vs.shortmfr = shtmfr.ShortDescName;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    vs.shortmfr = vs.Name;
        //                                }
        //                            }


        //                            if (vs.shortmfr != null && vs.shortmfr != "" && vs.shortmfr != "undefined")
        //                            {
        //                                mfrref = vs.Name;
        //                                var frmMdl = new shortFrame();
        //                                frmMdl.position = 101;
        //                                frmMdl.values = vs.shortmfr + sq.Separator;
        //                                lst.Add(frmMdl);
        //                                ShortStr += vs.shortmfr + sq.Separator;
        //                                if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                {
        //                                    ShortStr = ShortStr.Trim();
        //                                    char[] chr = sq.Separator.ToCharArray();
        //                                    ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                    while (ShortStr.Length > seqList[0].ShortLength)
        //                                    {
        //                                        int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                        if (lstIndx > -1)
        //                                        {
        //                                            if (lst.Count > 0)
        //                                            {
        //                                                if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                    lst.RemoveAt(lst.Count - 1);
        //                                            }
        //                                            ShortStr = ShortStr.Remove(lstIndx);

        //                                        }
        //                                        else
        //                                        {
        //                                            lstIndx = ShortStr.LastIndexOf(' ');
        //                                            ShortStr = ShortStr.Remove(lstIndx);
        //                                            if (lst.Count > 0)
        //                                            {
        //                                                int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                            }
        //                                        }
        //                                    }
        //                                    ShortStr = ShortStr + chr[0];
        //                                }
        //                                break;
        //                            }

        //                        }
        //                    }
        //                    break;
        //                case 105:


        //                    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    {
        //                        if (vs.s == 1 && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined")
        //                        {
        //                            string prefix = "";
        //                            if (vs.Refflag == "MODEL NUMBER")
        //                                prefix = "MM:";
        //                            if (vs.Refflag == "PART NUMBER")
        //                                prefix = "PN:";
        //                            if (vs.Refflag == "DRAWING & POSITION NUMBER")
        //                                prefix = "DWI:";
        //                            if (vs.Refflag == "DRAWING NUMBER")
        //                                prefix = "DW:";
        //                            if (vs.Refflag == "POSITION NUMBER")
        //                                prefix = "POS:";
        //                            if (vs.Refflag == "CATALOGUE NUMBER")
        //                                prefix = "Cat:";

        //                            var frmMdl = new shortFrame();
        //                            frmMdl.position = 100;
        //                            frmMdl.values = prefix + vs.RefNo.Trim() + sq.Separator;
        //                            lst.Add(frmMdl);
        //                            // ShortStr = strNM;

        //                            ShortStr += prefix + vs.RefNo.Trim() + sq.Separator;
        //                            if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                            {
        //                                ShortStr = ShortStr.Trim();
        //                                char[] chr = sq.Separator.ToCharArray();
        //                                ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                while (ShortStr.Length > seqList[0].ShortLength)
        //                                {
        //                                    int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                    if (lstIndx > -1)
        //                                    {
        //                                        if (lst.Count > 0)
        //                                        {
        //                                            if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                lst.RemoveAt(lst.Count - 1);
        //                                        }
        //                                        ShortStr = ShortStr.Remove(lstIndx);

        //                                    }
        //                                    else
        //                                    {
        //                                        lstIndx = ShortStr.LastIndexOf(' ');
        //                                        ShortStr = ShortStr.Remove(lstIndx);
        //                                        if (lst.Count > 0)
        //                                        {
        //                                            int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                            lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                        }
        //                                    }

        //                                }
        //                                ShortStr = ShortStr + chr[0];
        //                            }
        //                            break;
        //                        }

        //                    }
        //                    break;
        //                //case 106:
        //                //    if (cat.Application != null && cat.Application != "")
        //                //        ShortStr += cat.Application + sq.Separator;
        //                //    break;
        //                case 107:
        //                    //if (cat.Drawingno != null && cat.Drawingno != "")
        //                    //    ShortStr += cat.Drawingno + sq.Separator;


        //                    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    {
        //                        if (vs.Refflag.ToUpper() == "DRAWING & POSITION NUMBER" && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined" && vs.Name == mfrref && vs.Name != "" && vs.Name != null)
        //                        {
        //                            ShortStr += vs.RefNo.Trim() + sq.Separator;
        //                            break;
        //                        }
        //                    }
        //                    break;
        //                case 108:
        //                    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "")
        //                        ShortStr += cat.Equipment.Name + sq.Separator;
        //                    break;
        //                case 109:
        //                    if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
        //                        ShortStr += cat.Equipment.Manufacturer + sq.Separator;
        //                    break;
        //                case 110:
        //                    if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "")
        //                        ShortStr += cat.Equipment.Modelno + sq.Separator;
        //                    break;
        //                case 111:
        //                    if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "")
        //                        ShortStr += cat.Equipment.Tagno + sq.Separator;
        //                    break;
        //                case 112:
        //                    if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "")
        //                        ShortStr += cat.Equipment.Serialno + sq.Separator;
        //                    break;
        //                //case 113:
        //                //    if (cat.Referenceno != null && cat.Referenceno != "")
        //                //        ShortStr += cat.Referenceno + sq.Separator;
        //                //    break;
        //                case 114:
        //                    if (cat.Additionalinfo != null && cat.Additionalinfo != "")
        //                        ShortStr += cat.Additionalinfo + sq.Separator;
        //                    break;
        //                case 115:
        //                    if (cat.Equipment != null && cat.Equipment.Additionalinfo != null && cat.Equipment.Additionalinfo != "")
        //                        ShortStr += cat.Equipment.Additionalinfo + sq.Separator;
        //                    break;


        //            }
        //            if (!checkLength(ShortStr, seqList[0].ShortLength))
        //            {
        //                ShortStr = ShortStr.Trim();
        //                char[] chr = sq.Separator.ToCharArray();
        //                ShortStr = ShortStr.TrimEnd(chr[0]);
        //                while (ShortStr.Length > seqList[0].ShortLength)
        //                {
        //                    int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                    ShortStr = ShortStr.Remove(lstIndx);
        //                }
        //                break;
        //            }
        //        }
        //    }
        //    if (ShortStr.Length != seqList[0].ShortLength)
        //    {
        //        ShortStr = ShortStr.Trim();
        //        int lsIndx = ShortStr.Length;
        //        string str = ShortStr.Substring(lsIndx - 1, 1);
        //        if (str == seqList[0].Separator.Trim())
        //        {
        //            ShortStr = ShortStr.Remove(lsIndx - 1);
        //        }
        //    }
        //    return ShortStr.ToUpper();
        //}



        //SCC
        //public String[] GenerateShortLong(Prosol_Datamaster cat)
        //{
        //    String[] strArr = { "", "" };
        //    strArr[0] = ShortDesc(cat);
        //    strArr[1] = LongDesc(cat);
        //    return strArr;
        //}
        public String[] GenerateShortLong(Prosol_Datamaster cat)
        {

            String[] strArr = { "", "", "", "", "", "", "" };
            strArr[0] = ShortDesc(cat);
            strArr[1] = LongDesc(cat);
            cat.Shortdesc = strArr[0];
            cat.Longdesc = strArr[1];

            strArr[2] = MissingValue(cat);
            strArr[3] = PapulatedValue(cat);
            //strArr[4] = ShortDesc1(cat);

            // cat.Longdesc = strArr[1];
            strArr[5] = ExMissingValue(cat, cat.Longdesc);
            strArr[6] = "";

            return strArr;
        }

        private string MissingValue(Prosol_Datamaster cat)
        {
            // string tmpstr = Regex.Replace(cat.Legacy.Trim(), @"[^\w\d]", " ");
            // string[] legarray = tmpstr.Split(' ');
            //string[] legarray = cat.Legacy.Trim().Split(' ',',',':',';');
            string[] legarray = cat.Legacy.Split(new char[] { ' ', ',', ':', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(s => s.Trim())
                                .ToArray();
            string[] longlegarray = new string[0];
            if(longlegarray.Count() > 0)
            longlegarray = cat.Legacy2.Split(new char[] { ' ', ',', ':', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(s => s.Trim())
                                .ToArray();

            legarray.Concat(longlegarray);

            //  string tmpstr1 = Regex.Replace(cat.Longdesc.Trim(), @"[^\w\d]", " ");
            // string[] longarray = tmpstr1.Split(' ');
            string[] longarray = cat.Longdesc.Trim().Split(' ', ',', ':', ';');
            //string[] shortarray = cat.Equipment_Short.Trim().Split(' ', ',', ':', ';');
            string str = "";

            foreach (string arry in legarray)
            {
               
                if (!longarray.Contains(arry) /*&& !shortarray.Contains(arry)*/)
                {
                    if (arry != "EQUIPMENT")
                    {
                        str += arry + ",";
                    }
                    //  str = legarray + " ";

                }

            }
            if (str.EndsWith(","))
            {
                str = str.Substring(0, str.Length - 1);
            }
            bool containsWhitespaceOrComma = str.Any(c => char.IsWhiteSpace(c) || c == ',');

            if (containsWhitespaceOrComma)
            {
                string[] recheckArray = Regex.Split(str.Trim(), @"[\s,]+")
                .Where(s => !string.IsNullOrWhiteSpace(s))  
                .ToArray();
                str = "";
                foreach (var rearry in recheckArray)
                {
                    if (!longarray.Contains(rearry))
                    {
                        str += rearry + ",";
                    }
                }
            }
            //string[] recheckArray = str.Trim().Split(',').ToArray();
            //str = "";
            //foreach (var rearry in recheckArray)
            //{
            //    if (!cat.Longdesc.Contains(rearry))
            //    {
            //        str += rearry + ",";
            //    }
            //}
            if (str.EndsWith(","))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }
        //private string ExMissingValue(Prosol_Datamaster cat, string longDesc)
        //{
        //    // Extract the legacy description
        //    var legacy = ExLongDesc(cat);

        //    string[] legarray = legacy.Split(new char[] { ',', ':', ';' }, StringSplitOptions.RemoveEmptyEntries)
        //                        .Select(s => s.Trim())
        //                        .ToArray();

        //    string[] longarray = longDesc.Split(new char[] { ',', ':', ';' }, StringSplitOptions.RemoveEmptyEntries)
        //                         .Select(s => s.Trim())
        //                         .ToArray();

        //    string missingValues = "";

        //    foreach (string arry in legarray)
        //    {
        //        if (!longarray.Contains(arry, StringComparer.OrdinalIgnoreCase)) 
        //        {
        //            if (!string.Equals(arry, "EQUIPMENT", StringComparison.OrdinalIgnoreCase))
        //            {
        //                missingValues += arry + ",";
        //            }
        //        }
        //    }

        //    if (missingValues.EndsWith(","))
        //    {
        //        missingValues = missingValues.Substring(0, missingValues.Length - 1);
        //    }

        //    return missingValues;
        //}

        private string ExMissingValue(Prosol_Datamaster cat, string longDesc)
        {                                       
            var legacy = ExLongDesc(cat);

            string[] legarray = legacy.Split(new char[] { ',', ':', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(s => s.Trim())
                                      .ToArray();

            string missingValues = "";

            foreach (string phrase in legarray)
            {
                if (longDesc.IndexOf(phrase, StringComparison.OrdinalIgnoreCase) == -1)
                {
                    if (!string.Equals(phrase, "EQUIPMENT", StringComparison.OrdinalIgnoreCase))
                    {
                        missingValues += phrase + ",";
                    }
                }
            }

            if (missingValues.EndsWith(","))
            {
                missingValues = missingValues.Substring(0, missingValues.Length - 1);
            }

            return missingValues;
        }


        //private string ExMissingValue(Prosol_Datamaster cat, string longDesc)
        //{
        //    var legacy = ExLongDesc(cat);

        //    // Clean and split legacy description into phrases (case-insensitive)
        //    string[] legarray = legacy.Split(new char[] { ',', ':', ';' }, StringSplitOptions.RemoveEmptyEntries)
        //                        .Select(s => s.Trim().ToLower()) // Normalize to lowercase
        //                        .ToArray();

        //    // Clean and split long description into phrases (case-insensitive)
        //    string[] longarray = longDesc.Split(new char[] { ',', ':', ';' }, StringSplitOptions.RemoveEmptyEntries)
        //                         .Select(s => s.Trim().ToLower()) // Normalize to lowercase
        //                         .ToArray();

        //    // Prepare to store missing values
        //    List<string> missingValues = new List<string>();

        //    // Check for missing values in longDesc
        //    foreach (string arry in legarray)
        //    {
        //        if (!longarray.Contains(arry)) // Compare exact matches only
        //        {
        //            missingValues.Add(arry);
        //        }
        //    }

        //    // Join the missing values back into a string, comma-separated
        //    string result = string.Join(",", missingValues);

        //    return result;
        //}


        private string PapulatedValue(Prosol_Datamaster cat)
        {
            // string tmpstr = Regex.Replace(cat.Legacy.Trim(), @"[^\w\d]", " ");
            // string[] legarray = tmpstr.Split(' ');
            string[] legarray = cat.Legacy.Trim().Split(' ', ',', ':', ';');

            //  string tmpstr1 = Regex.Replace(cat.Longdesc.Trim(), @"[^\w\d]", " ");
            // string[] longarray = tmpstr1.Split(' ');
            string[] longarray = cat.Longdesc.Trim().Split(' ', ',', ':', ';');
            //string[] shortarray = cat.Equipment_Short.Trim().Split(' ', ',', ':', ';');

            string str = "";

            foreach (string arry in longarray)
            {

                if (!legarray.Contains(arry) && !cat.Legacy.Contains(arry))
                {
                    if (arry != "ADDITIONAL" && arry != "INFORMATION" && arry != "--")
                    {

                        str += arry + ",";
                    }
                    //  str = legarray + " ";

                }

            }
            if (str.EndsWith(","))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }
        //public string ShortDesc(Prosol_Datamaster cat)
        //{

        //    string mfrref = "";

        //    var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier), Query.EQ("RP", "MM"));
        //    var NMList = _nounModifierRepository.FindOne(FormattedQuery);
        //    var sort = SortBy.Ascending("Seq").Ascending("Description");

        //    var query = (NMList != null && NMList.Formatted == 1) ? Query.EQ("Description", "Short_Generic") : Query.EQ("Description", "Short_OEM");
        //    string ShortStr = "", strNM = "";
        //    cat.Additionalinfo = "";

        //    var seqList = _SequenceRepository.FindAll(query, sort).ToList();
        //    var UOMSet = _UOMRepository.FindOne();

        //    var AbbrList = _abbreivateRepository.FindAll();
        //    List<shortFrame> lst = new List<shortFrame>();
        //    foreach (Prosol_Sequence sq in seqList)
        //    {
        //        if (sq.Required == "Yes")
        //        {
        //            switch (sq.CatId)
        //            {
        //                case 101:
        //                    string nmStr = "";
        //                    //if (NMList != null && !string.IsNullOrEmpty(NMList.Nounabv))
        //                    //    ShortStr += NMList.Nounabv + sq.Separator;
        //                    //else ShortStr += cat.Noun + sq.Separator;


        //                    if (cat.Noun != "OEM" && cat.Noun != "PARTS")
        //                    {
        //                        if (cat.Modifier != "--" && cat.Modifier != "NO MODIFIER")
        //                            ShortStr += cat.Noun + sq.Separator + cat.Modifier + sq.Separator;
        //                        else
        //                            ShortStr += cat.Noun + sq.Separator;
        //                    }
        //                    else
        //                    {
        //                        ShortStr += "";
        //                    }

        //                    //if (cat.Modifier != "--" && cat.Modifier != "NO MODIFIER")
        //                    //{
        //                    //    if (NMList != null && !string.IsNullOrEmpty(NMList.Modifierabv))
        //                    //        ShortStr += NMList.Modifierabv + sq.Separator;
        //                    //    else
        //                    //        ShortStr += cat.Modifier + sq.Separator;
        //                    //}

        //                    strNM = ShortStr;

        //                    break;

        //                case 103:
        //                    string charStr = "";
        //                    int i = 0;
        //                    if (cat.Characteristics != null)
        //                    {
        //                        foreach (Prosol_AttributeList chM in cat.Characteristics.OrderBy(x => x.ShortSquence))
        //                        {

        //                            if (chM.Value != null && chM.Value != "" && chM.Value != "--")
        //                            {
        //                                string strValue = "";
        //                                var frmMdl = new shortFrame();
        //                                frmMdl.position = chM.Squence;


        //                                if (chM.Value.Contains(','))
        //                                {
        //                                    string tmpstr = "";
        //                                    var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                    {
        //                                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                    }
        //                                    else
        //                                    {
        //                                        string[] strsplt = chM.Value.Split(',');
        //                                        foreach (string str in strsplt)
        //                                        {
        //                                            //for space split
        //                                            if (str.Contains(' '))
        //                                            {
        //                                                abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                                if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                                    tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                                else
        //                                                {
        //                                                    string[] spaceSpt = str.Split(' ');
        //                                                    foreach (string spceStr in spaceSpt)
        //                                                    {
        //                                                        abbObj = (from Abb in AbbrList where Abb.Value.Equals(spceStr, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                                            tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
        //                                                        else
        //                                                            tmpstr += spceStr.TrimStart().TrimEnd() + ' ';

        //                                                    }
        //                                                    tmpstr = tmpstr.TrimEnd(' ');
        //                                                }


        //                                            }
        //                                            else
        //                                            {

        //                                                abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                                if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                                    tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                                else tmpstr += str.TrimStart().TrimEnd() + ',';
        //                                            }
        //                                        }
        //                                    }
        //                                    tmpstr = tmpstr.TrimEnd(',');
        //                                    if (UOMSet.Short_space == "with space")
        //                                    {
        //                                        if (chM.UOM != null && chM.UOM != "")
        //                                            strValue += tmpstr + " " + chM.UOM + sq.Separator;
        //                                        else strValue += tmpstr + sq.Separator;
        //                                    }
        //                                    else
        //                                    {
        //                                        if (chM.UOM != null && chM.UOM != "")
        //                                            strValue += tmpstr + chM.UOM + sq.Separator;
        //                                        else strValue += tmpstr + sq.Separator;
        //                                    }
        //                                    frmMdl.values = strValue;
        //                                }
        //                                else
        //                                {
        //                                    string tmpstr = "";
        //                                    if (chM.Value.Contains(' '))
        //                                    {
        //                                        //fore space split

        //                                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                            tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                        else
        //                                        {
        //                                            string[] spaceSpt = chM.Value.Split(' ');
        //                                            foreach (string spceStr in spaceSpt)
        //                                            {
        //                                                abbObj = (from Abb in AbbrList where Abb.Value.Equals(spceStr, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                                if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                                    tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
        //                                                else
        //                                                    tmpstr += spceStr.TrimStart().TrimEnd() + ' ';

        //                                            }
        //                                            tmpstr = tmpstr.TrimEnd(' ');
        //                                        }
        //                                        tmpstr = tmpstr.Trim(',');
        //                                        if (UOMSet.Short_space == "with space")
        //                                        {
        //                                            if (chM.UOM != null && chM.UOM != "")
        //                                                strValue += tmpstr.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                            else strValue += tmpstr.TrimStart().TrimEnd() + sq.Separator;
        //                                        }
        //                                        else
        //                                        {
        //                                            if (chM.UOM != null && chM.UOM != "")
        //                                                strValue += tmpstr.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                            else strValue += tmpstr.TrimStart().TrimEnd() + sq.Separator;
        //                                        }
        //                                        frmMdl.values = strValue;

        //                                    }
        //                                    else
        //                                    {
        //                                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                        {
        //                                            // Abbreivated
        //                                            if (UOMSet.Short_space == "with space")
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                                else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            else
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                                else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            frmMdl.values = strValue;
        //                                        }
        //                                        else
        //                                        {
        //                                            // Abbreivated not exist

        //                                            if (UOMSet.Short_space == "with space")
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += chM.Value.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                                else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            else
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += chM.Value.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                                else
        //                                                {
        //                                                    if (chM.Characteristic == "PART NUMBER")
        //                                                        strValue += "PN:" + chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                                    else
        //                                                        strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                                }
        //                                            }
        //                                            frmMdl.values = strValue;
        //                                        }
        //                                    }

        //                                }

        //                                lst.Add(frmMdl);

        //                                ShortStr = strNM;
        //                                string pattern = " X ";
        //                                foreach (shortFrame sMdl in lst)
        //                                {
        //                                    string[] strtmp = { " X " };
        //                                    var x = sMdl.values.Split(strtmp, StringSplitOptions.RemoveEmptyEntries);
        //                                    if (Regex.IsMatch(x[0], "^[0-9]+$", RegexOptions.Compiled))
        //                                    {
        //                                        foreach (Match match in Regex.Matches(sMdl.values, pattern))
        //                                        {
        //                                            sMdl.values = Regex.Replace(sMdl.values, pattern, match.Value.TrimEnd().TrimStart());
        //                                        }
        //                                        // ShortStr += sMdl.values;
        //                                        string pattern1 = " x ";
        //                                        foreach (Match match in Regex.Matches(sMdl.values, pattern1))
        //                                        {
        //                                            sMdl.values = Regex.Replace(sMdl.values, pattern1, match.Value.TrimEnd().TrimStart());
        //                                        }
        //                                    }
        //                                    ShortStr += sMdl.values;
        //                                }

        //                                if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                {
        //                                    ShortStr = ShortStr.Trim();
        //                                    char[] chr = sq.Separator.ToCharArray();
        //                                    ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                    while (ShortStr.Length > seqList[0].ShortLength)
        //                                    {
        //                                        int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                        if (lstIndx > -1)
        //                                        {
        //                                            if (lst.Count > 0)
        //                                            {

        //                                                if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                {
        //                                                    if (ShortStr.Substring(lstIndx).TrimStart(chr[0]) == lst[lst.Count - 1].values.TrimEnd(chr[0]))
        //                                                    {
        //                                                        lst.RemoveAt(lst.Count - 1);
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        int indx = lst[lst.Count - 1].values.TrimEnd(chr[0]).LastIndexOf(chr[0]);
        //                                                        lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx) + chr[0];
        //                                                    }
        //                                                }
        //                                            }
        //                                            ShortStr = ShortStr.Remove(lstIndx);

        //                                        }
        //                                        else
        //                                        {
        //                                            lstIndx = ShortStr.LastIndexOf(' ');
        //                                            ShortStr = ShortStr.Remove(lstIndx);
        //                                            if (lst.Count > 0)
        //                                            {
        //                                                if (lst[lst.Count - 1].values.Contains(' '))
        //                                                {
        //                                                    int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                    lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                                }
        //                                            }
        //                                        }

        //                                    }
        //                                    ShortStr = ShortStr + chr[0];
        //                                }
        //                                i++;
        //                            }

        //                        }
        //                        ShortStr = strNM;
        //                        foreach (shortFrame sMdl in lst.OrderBy(x => x.position))
        //                        {
        //                            ShortStr += sMdl.values;
        //                            //if (ShortStr.Length > 38)
        //                            //{
        //                            //    ShortStr = ShortStr.Replace(sMdl.values,"");
        //                            //}
        //                        }

        //                    }

        //                    break;

        //                case 104:
        //                    if (cat.Vendorsuppliers != null)
        //                    {
        //                        foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                        {
        //                            if (vs.s == 1)
        //                            {
        //                                if (vs.Name != null && vs.Name != "")
        //                                {
        //                                    var querry = Query.EQ("Name", vs.Name);
        //                                    var shtmfr = _VendorRepository.FindOne(querry);
        //                                    if (shtmfr != null)
        //                                    {
        //                                        if (shtmfr.ShortDescName != null && shtmfr.ShortDescName != "")
        //                                        {
        //                                            vs.shortmfr = shtmfr.ShortDescName;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        vs.shortmfr = vs.Name;
        //                                    }
        //                                }
        //                                //else
        //                                //{
        //                                //    vs.shortmfr = vs.Name;
        //                                //}

        //                                if (vs.shortmfr != null && vs.shortmfr != "" && vs.shortmfr != "undefined")
        //                                {
        //                                    mfrref = vs.Name;
        //                                    var frmMdl = new shortFrame();
        //                                    frmMdl.position = 101;
        //                                    frmMdl.values = vs.shortmfr + sq.Separator;
        //                                    lst.Add(frmMdl);
        //                                    ShortStr += vs.shortmfr + sq.Separator;
        //                                    if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                    {
        //                                        ShortStr = ShortStr.Trim();
        //                                        char[] chr = sq.Separator.ToCharArray();
        //                                        ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                        while (ShortStr.Length > seqList[0].ShortLength)
        //                                        {
        //                                            int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                            if (lstIndx > -1)
        //                                            {
        //                                                if (lst.Count > 0)
        //                                                {
        //                                                    if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                        lst.RemoveAt(lst.Count - 1);
        //                                                }
        //                                                ShortStr = ShortStr.Remove(lstIndx);

        //                                            }
        //                                            else
        //                                            {
        //                                                lstIndx = ShortStr.LastIndexOf(' ');
        //                                                ShortStr = ShortStr.Remove(lstIndx);
        //                                                if (lst.Count > 0)
        //                                                {
        //                                                    int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                    lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                                }
        //                                            }
        //                                        }
        //                                        ShortStr = ShortStr + chr[0];
        //                                    }
        //                                    break;
        //                                }

        //                            }
        //                        }
        //                    }
        //                    break;
        //                case 105:
        //                    if (cat.Vendorsuppliers != null)
        //                    {
        //                        foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                        {
        //                            if (vs.s == 1 && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined")
        //                            {
        //                                string prefix = "";
        //                                if (vs.Refflag != null)
        //                                {
        //                                    var flag = Query.EQ("Type", vs.Refflag);
        //                                    var code = _ReftypeRepository.FindOne(flag);
        //                                    if (code != null)
        //                                    {
        //                                        prefix = code.Code + ":";
        //                                    }
        //                                }
        //                                var frmMdl = new shortFrame();
        //                                frmMdl.position = 100;
        //                                frmMdl.values = vs.RefNo.Trim() + sq.Separator;
        //                                lst.Add(frmMdl);
        //                                //ShortStr = strNM;

        //                                ShortStr += prefix + vs.RefNo.Trim() + sq.Separator;
        //                                if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                {
        //                                    ShortStr = ShortStr.Trim();
        //                                    char[] chr = sq.Separator.ToCharArray();
        //                                    ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                    while (ShortStr.Length > seqList[0].ShortLength)
        //                                    {
        //                                        int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                        if (lstIndx > -1)
        //                                        {
        //                                            if (lst.Count > 0)
        //                                            {
        //                                                if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                    lst.RemoveAt(lst.Count - 1);
        //                                            }
        //                                            ShortStr = ShortStr.Remove(lstIndx);

        //                                        }
        //                                        else
        //                                        {
        //                                            lstIndx = ShortStr.LastIndexOf(' ');
        //                                            ShortStr = ShortStr.Remove(lstIndx);
        //                                            if (lst.Count > 0)
        //                                            {
        //                                                int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                            }
        //                                        }

        //                                    }
        //                                    ShortStr = ShortStr + chr[0];
        //                                }
        //                                break;
        //                            }

        //                        }
        //                    }

        //                    break;
        //                case 106:
        //                    if (cat.Application != null && cat.Application != "")
        //                        ShortStr += cat.Application + sq.Separator;
        //                    break;
        //                case 107:
        //                    //if (cat.Drawingno != null && cat.Drawingno != "")
        //                    //    ShortStr += cat.Drawingno + sq.Separator;


        //                    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    {
        //                        if (vs.Refflag.ToUpper() == "DRAWING & POSITION NUMBER" && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined" && vs.Name == mfrref && vs.Name != "" && vs.Name != null)
        //                        {
        //                            ShortStr += vs.RefNo.Trim() + sq.Separator;
        //                            break;
        //                        }
        //                    }
        //                    break;
        //                case 108:

        //                    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "")
        //                    {
        //                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Equipment.Name, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                        {
        //                            ShortStr += "F/" + abbObj.Abbrevated + sq.Separator;
        //                        }
        //                        else
        //                        {
        //                            if (cat.Equipment.Name.Contains(' '))
        //                            {
        //                                string tmpstr = "";

        //                                string[] strsplt = cat.Equipment.Name.Split(' ');
        //                                foreach (string str in strsplt)
        //                                {
        //                                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
        //                                    else tmpstr += str.TrimStart().TrimEnd() + ' ';
        //                                }

        //                                tmpstr = tmpstr.TrimEnd(' ');
        //                                ShortStr += "F/" + tmpstr + sq.Separator;
        //                            }
        //                            else
        //                            {
        //                                ShortStr += "F/" + cat.Equipment.Name + sq.Separator;
        //                            }

        //                        }
        //                        if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                        {
        //                            ShortStr = ShortStr.Trim();
        //                            char[] chr = sq.Separator.ToCharArray();
        //                            ShortStr = ShortStr.TrimEnd(chr[0]);
        //                            while (ShortStr.Length > seqList[0].ShortLength)
        //                            {
        //                                int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                ShortStr = ShortStr.Remove(lstIndx);
        //                            }
        //                            ShortStr += sq.Separator;
        //                            break;
        //                        }
        //                    }
        //                    //ShortStr += "F/" + cat.Equipment.Name + sq.Separator;

        //                    break;
        //                //case 108:

        //                //    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "" && cat.Equipment.ENS == 1)
        //                //    {
        //                //        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Equipment.Name, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                //        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                //        {
        //                //            ShortStr += "F/" + abbObj.Abbrevated + sq.Separator;
        //                //        }
        //                //        else
        //                //            ShortStr += "F/" + cat.Equipment.Name + sq.Separator;
        //                //    }
        //                //    //ShortStr += "F/" + cat.Equipment.Name + sq.Separator;

        //                //    break;
        //                case 109:
        //                    if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
        //                        ShortStr += cat.Equipment.Manufacturer + sq.Separator;
        //                    break;
        //                case 110:
        //                    if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "")
        //                        ShortStr += cat.Equipment.Modelno + sq.Separator;
        //                    break;
        //                case 111:
        //                    if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "")
        //                        ShortStr += cat.Equipment.Tagno + sq.Separator;
        //                    break;
        //                case 112:
        //                    if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "")
        //                        ShortStr += cat.Equipment.Serialno + sq.Separator;
        //                    break;
        //                //case 108:
        //                //    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "")
        //                //        ShortStr += cat.Equipment.Name + sq.Separator;
        //                //    break;
        //                //case 109:
        //                //    if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
        //                //        ShortStr += cat.Equipment.Manufacturer + sq.Separator;
        //                //    break;
        //                //case 110:
        //                //    if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "")
        //                //        ShortStr += cat.Equipment.Modelno + sq.Separator;
        //                //    break;
        //                //case 111:
        //                //    if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "")
        //                //        ShortStr += cat.Equipment.Tagno + sq.Separator;
        //                //    break;
        //                //case 112:
        //                //    if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "")
        //                //        ShortStr += cat.Equipment.Serialno + sq.Separator;
        //                //    break;
        //                case 113:
        //                    if (cat.Referenceno != null && cat.Referenceno != "")
        //                        ShortStr += cat.Referenceno + sq.Separator;
        //                    break;
        //                case 114:
        //                    if (cat.Additionalinfo != null && cat.Additionalinfo != "")
        //                        ShortStr += cat.Additionalinfo + sq.Separator;
        //                    break;
        //                case 115:
        //                    if (cat.Equipment != null && cat.Equipment.Additionalinfo != null && cat.Equipment.Additionalinfo != "")
        //                        ShortStr += cat.Equipment.Additionalinfo + sq.Separator;
        //                    break;


        //            }
        //            seqList[0].ShortLength = 38;
        //            if (!checkLength(ShortStr, seqList[0].ShortLength))
        //            {
        //                ShortStr = ShortStr.Trim();
        //                char[] chr = sq.Separator.ToCharArray();
        //                ShortStr = ShortStr.TrimEnd(chr[0]);
        //                while (ShortStr.Length > seqList[0].ShortLength)
        //                {
        //                    int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                    ShortStr = ShortStr.Remove(lstIndx);
        //                }
        //                break;
        //            }
        //        }
        //    }
        //    seqList[0].ShortLength = 36;
        //    if (ShortStr.Length > 0 && ShortStr.Length != seqList[0].ShortLength)
        //    {
        //        ShortStr = ShortStr.Trim();
        //        int lsIndx = ShortStr.Length;
        //        string str = ShortStr.Substring(lsIndx - 1, 1);
        //        if (str == seqList[0].Separator.Trim())
        //        {
        //            ShortStr = ShortStr.Remove(lsIndx - 1);
        //        }
        //    }
        //    if (ShortStr.Length > 0 && ShortStr.LastIndexOf(",") == ShortStr.Length - 1)
        //    {
        //        ShortStr = ShortStr.Substring(0, ShortStr.Length - 1);
        //    }
        //    return ShortStr;


        //}
        //public string ShortDesc1(Prosol_Datamaster cat)
        //{

        //    string mfrref = "";

        //    var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
        //    var NMList = _nounModifierRepository.FindOne(FormattedQuery);
        //    var sort = SortBy.Ascending("Seq").Ascending("Description");

        //    var query = (NMList != null && NMList.Formatted == 1) ? Query.EQ("Description", "Short_Generic") : Query.EQ("Description", "Short_OEM");
        //    string ShortStr = "", strNM = "";

        //    var seqList = _SequenceRepository.FindAll(query, sort).ToList();
        //    var UOMSet = _UOMRepository.FindOne();

        //    var AbbrList = _abbreivateRepository.FindAll();
        //    //Short_Generic
        //    List<shortFrame> lst = new List<shortFrame>();
        //    foreach (Prosol_Sequence sq in seqList)
        //    {
        //        if (sq.Required == "Yes")
        //        {
        //            switch (sq.CatId)
        //            {
        //                case 101:


        //                    if (NMList.Formatted == 1 || NMList.Formatted == 2)
        //                    {
        //                        //var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Noun, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                        //if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                        //    ShortStr += abbObj.Abbrevated + sq.Separator;
        //                        //else ShortStr += cat.Noun + sq.Separator;


        //                        if (NMList.Nounabv != null && NMList.Nounabv != "")
        //                            ShortStr += NMList.Nounabv + sq.Separator;
        //                        else ShortStr += cat.Noun + sq.Separator;
        //                        if (NMList.Modifierabv != null && NMList.Modifierabv != "")
        //                            ShortStr += NMList.Modifierabv + sq.Separator;
        //                        else
        //                        {
        //                            if (cat.Modifier != "--" && cat.Modifier != "NO MODIFIER")
        //                                ShortStr += cat.Modifier + sq.Separator;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (cat.Characteristics != null)
        //                        {
        //                            var sObj = cat.Characteristics.OrderBy(x => x.Squence).FirstOrDefault();


        //                            //var abbObj = (from Abb in AbbrList where Abb.Value.Equals(sObj.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            //if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                            //    ShortStr += abbObj.Abbrevated + sq.Separator;
        //                            //else ShortStr += sObj.Value + sq.Separator;

        //                            string tmpstr = "";
        //                            if (sObj.Value.Contains(' '))
        //                            {
        //                                //fore space split

        //                                var abbObj = (from Abb in AbbrList where Abb.Value.Equals(sObj.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                    ShortStr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                else
        //                                {
        //                                    string[] spaceSpt = sObj.Value.Split(' ');
        //                                    foreach (string spceStr in spaceSpt)
        //                                    {
        //                                        abbObj = (from Abb in AbbrList where Abb.Value.Equals(spceStr, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                            ShortStr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
        //                                        else
        //                                            ShortStr += spceStr.TrimStart().TrimEnd() + ' ';

        //                                    }
        //                                    ShortStr = ShortStr.TrimEnd(' ');
        //                                }
        //                                ShortStr = ShortStr.TrimEnd(',');
        //                                if (UOMSet.Short_space == "with space")
        //                                {
        //                                    if (sObj.UOM != null && sObj.UOM != "")
        //                                        ShortStr += tmpstr.TrimStart().TrimEnd() + " " + sObj.UOM + sq.Separator;
        //                                    else ShortStr += tmpstr.TrimStart().TrimEnd() + sq.Separator;
        //                                }
        //                                else
        //                                {
        //                                    if (sObj.UOM != null && sObj.UOM != "")
        //                                        ShortStr += tmpstr.TrimStart().TrimEnd() + sObj.UOM + sq.Separator;
        //                                    else ShortStr += tmpstr.TrimStart().TrimEnd() + sq.Separator;
        //                                }
        //                                // frmMdl.values = tmpstr;

        //                            }
        //                            else
        //                            {
        //                                var abbObj = (from Abb in AbbrList where Abb.Value.Equals(sObj.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                {
        //                                    // Abbreivated
        //                                    if (UOMSet.Short_space == "with space")
        //                                    {
        //                                        if (sObj.UOM != null && sObj.UOM != "")
        //                                            ShortStr += abbObj.Abbrevated.TrimStart().TrimEnd() + " " + sObj.UOM + sq.Separator;
        //                                        else ShortStr += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                    }
        //                                    else
        //                                    {
        //                                        if (sObj.UOM != null && sObj.UOM != "")
        //                                            ShortStr += abbObj.Abbrevated.TrimStart().TrimEnd() + sObj.UOM + sq.Separator;
        //                                        else ShortStr += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                    }
        //                                    //frmMdl.values = strValue;
        //                                }
        //                                else
        //                                {
        //                                    // Abbreivated not exist

        //                                    if (UOMSet.Short_space == "with space")
        //                                    {
        //                                        if (sObj.UOM != null && sObj.UOM != "")
        //                                            ShortStr += sObj.Value.TrimStart().TrimEnd() + " " + sObj.UOM + sq.Separator;
        //                                        else ShortStr += sObj.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                    }
        //                                    else
        //                                    {
        //                                        if (sObj.UOM != null && sObj.UOM != "")
        //                                            ShortStr += sObj.Value.TrimStart().TrimEnd() + sObj.UOM + sq.Separator;
        //                                        else ShortStr += sObj.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                    }
        //                                    //frmMdl.values = strValue;
        //                                }
        //                            }

        //                            //ShortStr += tmpstr;
        //                        }
        //                    }
        //                    strNM = ShortStr;
        //                    //var nounabbr = (from Abb in AbbrList where Abb.Value.Equals(cat.Noun, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                    //if (nounabbr != null && (nounabbr.Abbrevated != null && nounabbr.Abbrevated != ""))
        //                    //{
        //                    //    ShortStr += nounabbr.Abbrevated + sq.Separator;
        //                    //}
        //                    //else ShortStr += cat.Noun + sq.Separator;

        //                    break;
        //                //case 102:
        //                //    if (cat.Modifier != "NO DEFINER" && cat.Modifier != "NO MODIFIER")
        //                //    {
        //                //        if (NMList.Formatted == 1)
        //                //        {

        //                //            if (NMList.Modifierabv != null && NMList.Modifierabv != "")
        //                //                ShortStr += NMList.Modifierabv + sq.Separator;
        //                //            else ShortStr += cat.Modifier + sq.Separator;



        //                //            strNM = ShortStr;
        //                //        }
        //                //    }
        //                //    break;
        //                case 103:
        //                    int flg = 0;


        //                    //  int[] arrPos= new int[cat.Characteristics.Count];
        //                    //  string[] arrVal = new string[cat.Characteristics.Count];
        //                    int i = 0;
        //                    if (cat.Characteristics != null)
        //                    {
        //                        foreach (Prosol_AttributeList chM in cat.Characteristics.OrderBy(x => x.ShortSquence))
        //                        {
        //                            if (NMList.Formatted == 1 || flg == 1 || NMList.Formatted == 2)
        //                            {

        //                                if (chM.Value != null && chM.Value != "")
        //                                {
        //                                    string strValue = "";
        //                                    var frmMdl = new shortFrame();
        //                                    frmMdl.position = chM.Squence;


        //                                    if (chM.Value.Contains(','))
        //                                    {
        //                                        string tmpstr = "";
        //                                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                        {
        //                                            tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                        }
        //                                        else
        //                                        {
        //                                            string[] strsplt = chM.Value.Split(',');
        //                                            foreach (string str in strsplt)
        //                                            {
        //                                                //for space split
        //                                                if (str.Contains(' '))
        //                                                {
        //                                                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                                    else
        //                                                    {
        //                                                        string[] spaceSpt = str.Split(' ');
        //                                                        foreach (string spceStr in spaceSpt)
        //                                                        {
        //                                                            abbObj = (from Abb in AbbrList where Abb.Value.Equals(spceStr, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                                            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                                                tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
        //                                                            else
        //                                                                tmpstr += spceStr.TrimStart().TrimEnd() + ' ';

        //                                                        }
        //                                                        tmpstr = tmpstr.TrimEnd(' ');
        //                                                    }


        //                                                }
        //                                                else
        //                                                {

        //                                                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                                    else tmpstr += str.TrimStart().TrimEnd() + ',';
        //                                                }
        //                                            }
        //                                        }
        //                                        tmpstr = tmpstr.TrimEnd(',');
        //                                        if (UOMSet.Short_space == "with space")
        //                                        {
        //                                            if (chM.UOM != null && chM.UOM != "")
        //                                                strValue += tmpstr + " " + chM.UOM + sq.Separator;
        //                                            else strValue += tmpstr + sq.Separator;
        //                                        }
        //                                        else
        //                                        {
        //                                            if (chM.UOM != null && chM.UOM != "")
        //                                                strValue += tmpstr + chM.UOM + sq.Separator;
        //                                            else strValue += tmpstr + sq.Separator;
        //                                        }
        //                                        frmMdl.values = strValue;
        //                                    }
        //                                    else
        //                                    {
        //                                        string tmpstr = "";
        //                                        if (chM.Value.Contains(' '))
        //                                        {
        //                                            //fore space split

        //                                            var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                                tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                            else
        //                                            {
        //                                                string[] spaceSpt = chM.Value.Split(' ');
        //                                                foreach (string spceStr in spaceSpt)
        //                                                {
        //                                                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(spceStr, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
        //                                                    else
        //                                                        tmpstr += spceStr.TrimStart().TrimEnd() + ' ';

        //                                                }
        //                                                tmpstr = tmpstr.TrimEnd(' ');
        //                                            }
        //                                            tmpstr = tmpstr.Trim(',');
        //                                            if (UOMSet.Short_space == "with space")
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += tmpstr.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                                else strValue += tmpstr.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            else
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += tmpstr.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                                else strValue += tmpstr.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            frmMdl.values = strValue;

        //                                        }
        //                                        else
        //                                        {
        //                                            var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                            {
        //                                                // Abbreivated
        //                                                if (UOMSet.Short_space == "with space")
        //                                                {
        //                                                    if (chM.UOM != null && chM.UOM != "")
        //                                                        strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                                    else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                                }
        //                                                else
        //                                                {
        //                                                    if (chM.UOM != null && chM.UOM != "")
        //                                                        strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                                    else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                                }
        //                                                frmMdl.values = strValue;
        //                                            }
        //                                            else
        //                                            {
        //                                                // Abbreivated not exist

        //                                                if (UOMSet.Short_space == "with space")
        //                                                {
        //                                                    if (chM.UOM != null && chM.UOM != "")
        //                                                        strValue += chM.Value.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                                    else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                                }
        //                                                else
        //                                                {
        //                                                    if (chM.UOM != null && chM.UOM != "")
        //                                                        strValue += chM.Value.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                                    else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                                }
        //                                                frmMdl.values = strValue;
        //                                            }
        //                                        }

        //                                    }

        //                                    lst.Add(frmMdl);

        //                                    ShortStr = strNM;
        //                                    string pattern = " X ";
        //                                    foreach (shortFrame sMdl in lst)
        //                                    {
        //                                        string[] strtmp = { " X " };
        //                                        var x = sMdl.values.Split(strtmp, StringSplitOptions.RemoveEmptyEntries);
        //                                        if (Regex.IsMatch(x[0], "^[0-9]+$", RegexOptions.Compiled))
        //                                        {
        //                                            foreach (Match match in Regex.Matches(sMdl.values, pattern))
        //                                            {
        //                                                sMdl.values = Regex.Replace(sMdl.values, pattern, match.Value.TrimEnd().TrimStart());
        //                                            }
        //                                            // ShortStr += sMdl.values;
        //                                            string pattern1 = " x ";
        //                                            foreach (Match match in Regex.Matches(sMdl.values, pattern1))
        //                                            {
        //                                                sMdl.values = Regex.Replace(sMdl.values, pattern1, match.Value.TrimEnd().TrimStart());
        //                                            }
        //                                        }
        //                                        ShortStr += sMdl.values;
        //                                    }

        //                                    if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                    {
        //                                        ShortStr = ShortStr.Trim();
        //                                        char[] chr = sq.Separator.ToCharArray();
        //                                        ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                        while (ShortStr.Length > seqList[0].ShortLength)
        //                                        {
        //                                            int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                            if (lstIndx > -1)
        //                                            {
        //                                                if (lst.Count > 0)
        //                                                {

        //                                                    if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                    {
        //                                                        if (ShortStr.Substring(lstIndx).TrimStart(chr[0]) == lst[lst.Count - 1].values.TrimEnd(chr[0]))
        //                                                        {
        //                                                            lst.RemoveAt(lst.Count - 1);
        //                                                        }
        //                                                        else
        //                                                        {
        //                                                            int indx = lst[lst.Count - 1].values.TrimEnd(chr[0]).LastIndexOf(chr[0]);
        //                                                            lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx) + chr[0];
        //                                                        }
        //                                                    }
        //                                                }
        //                                                ShortStr = ShortStr.Remove(lstIndx);

        //                                            }
        //                                            else
        //                                            {
        //                                                lstIndx = ShortStr.LastIndexOf(' ');
        //                                                ShortStr = ShortStr.Remove(lstIndx);
        //                                                if (lst.Count > 0)
        //                                                {
        //                                                    int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                    lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                                }
        //                                            }

        //                                        }
        //                                        ShortStr = ShortStr + chr[0];
        //                                    }
        //                                    i++;
        //                                }
        //                            }
        //                            else flg = 1;
        //                        }
        //                        ShortStr = strNM;
        //                        foreach (shortFrame sMdl in lst.OrderBy(x => x.position))
        //                        {
        //                            ShortStr += sMdl.values;
        //                        }
        //                    }

        //                    break;

        //                case 104:
        //                    if (cat.Vendorsuppliers != null)
        //                    {
        //                        foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                        {
        //                            if (vs.s == 1)
        //                            {
        //                                if (vs.Name != null && vs.Name != "")
        //                                {
        //                                    var querry = Query.EQ("Name", vs.Name);
        //                                    var shtmfr = _VendorRepository.FindOne(querry);
        //                                    if (shtmfr != null)
        //                                    {
        //                                        if (shtmfr.ShortDescName != null && shtmfr.ShortDescName != "")
        //                                        {
        //                                            vs.shortmfr = shtmfr.ShortDescName;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        vs.shortmfr = vs.Name;
        //                                    }
        //                                }
        //                                //else
        //                                //{
        //                                //    vs.shortmfr = vs.Name;
        //                                //}

        //                                if (vs.shortmfr != null && vs.shortmfr != "" && vs.shortmfr != "undefined")
        //                                {
        //                                    mfrref = vs.Name;
        //                                    var frmMdl = new shortFrame();
        //                                    frmMdl.position = 101;
        //                                    frmMdl.values = vs.shortmfr + sq.Separator;
        //                                    lst.Add(frmMdl);
        //                                    ShortStr += vs.shortmfr + sq.Separator;
        //                                    if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                    {
        //                                        ShortStr = ShortStr.Trim();
        //                                        char[] chr = sq.Separator.ToCharArray();
        //                                        ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                        while (ShortStr.Length > seqList[0].ShortLength)
        //                                        {
        //                                            int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                            if (lstIndx > -1)
        //                                            {
        //                                                if (lst.Count > 0)
        //                                                {
        //                                                    if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                        lst.RemoveAt(lst.Count - 1);
        //                                                }
        //                                                ShortStr = ShortStr.Remove(lstIndx);

        //                                            }
        //                                            else
        //                                            {
        //                                                lstIndx = ShortStr.LastIndexOf(' ');
        //                                                ShortStr = ShortStr.Remove(lstIndx);
        //                                                if (lst.Count > 0)
        //                                                {
        //                                                    int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                    lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                                }
        //                                            }
        //                                        }
        //                                        ShortStr = ShortStr + chr[0];
        //                                    }
        //                                    break;
        //                                }

        //                            }
        //                        }
        //                    }
        //                    break;
        //                case 105:
        //                    if (cat.Vendorsuppliers != null)
        //                    {
        //                        foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                        {
        //                            if (vs.s == 1 && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined")
        //                            {
        //                                string prefix = "";
        //                                if (vs.Refflag != null)
        //                                {
        //                                    var flag = Query.EQ("Type", vs.Refflag);
        //                                    var code = _ReftypeRepository.FindOne(flag);
        //                                    if (code != null)
        //                                    {
        //                                        prefix = code.Code + ":";
        //                                    }
        //                                }
        //                                var frmMdl = new shortFrame();
        //                                frmMdl.position = 100;
        //                                //frmMdl.values = prefix + vs.RefNo.Trim() + sq.Separator;
        //                                frmMdl.values = vs.RefNo.Trim() + sq.Separator;
        //                                lst.Add(frmMdl);
        //                                // ShortStr = strNM;

        //                                //ShortStr += prefix + vs.RefNo.Trim() + sq.Separator;
        //                                ShortStr += vs.RefNo.Trim() + sq.Separator;
        //                                if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                {
        //                                    ShortStr = ShortStr.Trim();
        //                                    char[] chr = sq.Separator.ToCharArray();
        //                                    ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                    while (ShortStr.Length > seqList[0].ShortLength)
        //                                    {
        //                                        int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                        if (lstIndx > -1)
        //                                        {
        //                                            if (lst.Count > 0)
        //                                            {
        //                                                if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                    lst.RemoveAt(lst.Count - 1);
        //                                            }
        //                                            ShortStr = ShortStr.Remove(lstIndx);

        //                                        }
        //                                        else
        //                                        {
        //                                            lstIndx = ShortStr.LastIndexOf(' ');
        //                                            ShortStr = ShortStr.Remove(lstIndx);
        //                                            if (lst.Count > 0)
        //                                            {
        //                                                int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                            }
        //                                        }

        //                                    }
        //                                    ShortStr = ShortStr + chr[0];
        //                                }
        //                                break;
        //                            }

        //                        }
        //                    }
        //                    //foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    //{
        //                    //    if (vs.s == 1 && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined")
        //                    //    {
        //                    //        var frmMdl = new shortFrame();
        //                    //        frmMdl.position = 100;
        //                    //        frmMdl.values = vs.RefNo.Trim() + sq.Separator;
        //                    //        lst.Add(frmMdl);
        //                    //        // ShortStr = strNM;

        //                    //        ShortStr += vs.RefNo.Trim() + sq.Separator;
        //                    //        if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                    //        {
        //                    //            ShortStr = ShortStr.Trim();
        //                    //            char[] chr = sq.Separator.ToCharArray();
        //                    //            ShortStr = ShortStr.TrimEnd(chr[0]);
        //                    //            while (ShortStr.Length > seqList[0].ShortLength)
        //                    //            {
        //                    //                int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                    //                if (lstIndx > -1)
        //                    //                {
        //                    //                    if (lst.Count > 0)
        //                    //                    {
        //                    //                        if (ShortStr.Substring(lstIndx).Length > 1)
        //                    //                            lst.RemoveAt(lst.Count - 1);
        //                    //                    }
        //                    //                    ShortStr = ShortStr.Remove(lstIndx);

        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    lstIndx = ShortStr.LastIndexOf(' ');
        //                    //                    ShortStr = ShortStr.Remove(lstIndx);
        //                    //                    if (lst.Count > 0)
        //                    //                    {
        //                    //                        int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                    //                        lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                    //                    }
        //                    //                }

        //                    //            }
        //                    //            ShortStr = ShortStr + chr[0];
        //                    //        }
        //                    //        break;
        //                    //    }

        //                    //}
        //                    break;
        //                case 106:
        //                    if (cat.Application != null && cat.Application != "")
        //                        ShortStr += cat.Application + sq.Separator;
        //                    break;
        //                case 107:
        //                    //if (cat.Drawingno != null && cat.Drawingno != "")
        //                    //    ShortStr += cat.Drawingno + sq.Separator;


        //                    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    {
        //                        if (vs.Refflag.ToUpper() == "DRAWING & POSITION NUMBER" && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined" && vs.Name == mfrref && vs.Name != "" && vs.Name != null)
        //                        {
        //                            ShortStr += vs.RefNo.Trim() + sq.Separator;
        //                            break;
        //                        }
        //                    }
        //                    break;
        //                case 108:

        //                    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "" && (cat.Equipment.ENS == 1 || NMList.Formatted == 0))
        //                    {
        //                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Equipment.Name, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                        {
        //                            ShortStr += "F/" + abbObj.Abbrevated + sq.Separator;
        //                        }
        //                        else
        //                        {
        //                            if (cat.Equipment.Name.Contains(' '))
        //                            {
        //                                string tmpstr = "";

        //                                string[] strsplt = cat.Equipment.Name.Split(' ');
        //                                foreach (string str in strsplt)
        //                                {
        //                                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
        //                                    else tmpstr += str.TrimStart().TrimEnd() + ' ';
        //                                }

        //                                tmpstr = tmpstr.TrimEnd(' ');
        //                                ShortStr += "F/" + tmpstr + sq.Separator;
        //                            }
        //                            else
        //                            {
        //                                ShortStr += "F/" + cat.Equipment.Name + sq.Separator;
        //                            }

        //                        }
        //                    }
        //                    //ShortStr += "F/" + cat.Equipment.Name + sq.Separator;

        //                    break;
        //                //case 108:

        //                //    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "" && cat.Equipment.ENS == 1)
        //                //    {
        //                //        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Equipment.Name, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                //        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                //        {
        //                //            ShortStr += "F/" + abbObj.Abbrevated + sq.Separator;
        //                //        }
        //                //        else
        //                //            ShortStr += "F/" + cat.Equipment.Name + sq.Separator;
        //                //    }
        //                //    //ShortStr += "F/" + cat.Equipment.Name + sq.Separator;

        //                //    break;
        //                case 109:
        //                    if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
        //                        ShortStr += cat.Equipment.Manufacturer + sq.Separator;
        //                    break;
        //                case 110:
        //                    if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "" && (cat.Equipment.EMS == 1 || NMList.Formatted == 0))
        //                        ShortStr += cat.Equipment.Modelno + sq.Separator;
        //                    break;
        //                case 111:
        //                    if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "")
        //                        ShortStr += cat.Equipment.Tagno + sq.Separator;
        //                    break;
        //                case 112:
        //                    if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "")
        //                        ShortStr += cat.Equipment.Serialno + sq.Separator;
        //                    break;
        //                //case 108:
        //                //    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "")
        //                //        ShortStr += cat.Equipment.Name + sq.Separator;
        //                //    break;
        //                //case 109:
        //                //    if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
        //                //        ShortStr += cat.Equipment.Manufacturer + sq.Separator;
        //                //    break;
        //                //case 110:
        //                //    if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "")
        //                //        ShortStr += cat.Equipment.Modelno + sq.Separator;
        //                //    break;
        //                //case 111:
        //                //    if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "")
        //                //        ShortStr += cat.Equipment.Tagno + sq.Separator;
        //                //    break;
        //                //case 112:
        //                //    if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "")
        //                //        ShortStr += cat.Equipment.Serialno + sq.Separator;
        //                //    break;
        //                case 113:
        //                    if (cat.Referenceno != null && cat.Referenceno != "")
        //                        ShortStr += cat.Referenceno + sq.Separator;
        //                    break;
        //                case 114:
        //                    if (cat.Additionalinfo != null && cat.Additionalinfo != "")
        //                        ShortStr += cat.Additionalinfo + sq.Separator;
        //                    break;
        //                case 115:
        //                    if (cat.Equipment != null && cat.Equipment.Additionalinfo != null && cat.Equipment.Additionalinfo != "")
        //                        ShortStr += cat.Equipment.Additionalinfo + sq.Separator;
        //                    break;


        //            }
        //            if (!checkLength(ShortStr, seqList[0].ShortLength))
        //            {
        //                ShortStr = ShortStr.Trim();
        //                char[] chr = sq.Separator.ToCharArray();
        //                ShortStr = ShortStr.TrimEnd(chr[0]);
        //                while (ShortStr.Length > seqList[0].ShortLength)
        //                {
        //                    int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                    ShortStr = ShortStr.Remove(lstIndx);
        //                }
        //                break;
        //            }
        //        }
        //    }
        //    if (ShortStr.Length != seqList[0].ShortLength)
        //    {
        //        ShortStr = ShortStr.Trim();
        //        int lsIndx = ShortStr.Length;
        //        string str = ShortStr.Substring(lsIndx - 1, 1);
        //        if (str == seqList[0].Separator.Trim())
        //        {
        //            ShortStr = ShortStr.Remove(lsIndx - 1);
        //        }
        //    }
        //    return ShortStr;


        //}


        //Re-Worked

        //public string ShortDesc(Prosol_Datamaster cat)
        //{

        //    string mfrref = "";

        //    var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
        //    var NMList = _nounModifierRepository.FindOne(FormattedQuery);

        //    var sort = SortBy.Ascending("Seq").Ascending("Description");

        //    var query = (NMList != null && NMList.Formatted == 1) ? Query.EQ("Description", "Short_Generic") : Query.EQ("Description", "Short_OEM");
        //    string ShortStr = "", strNM = "", strVen = "", strVenn = "", strChar = "", strFinal = "";

        //    var seqList = _SequenceRepository.FindAll(query, sort).ToList();
        //    var UOMSet = _UOMRepository.FindOne();
        //    seqList[0].ShortLength = 38;
        //    int currentLen = 38;

        //    var AbbrList = _abbreivateRepository.FindAll();
        //    List<shortFrame> lst = new List<shortFrame>();


        //    if (cat.Noun == "OEM")
        //    {
        //        foreach (Prosol_Sequence sq in seqList)
        //        {
        //            if (sq.Required == "Yes")
        //            {
        //                switch (sq.CatId)
        //                {
        //                    case 101:
        //                        string nmStr = "";
        //                        //if (NMList != null && !string.IsNullOrEmpty(NMList.Nounabv))
        //                        //    ShortStr += NMList.Nounabv + sq.Separator;
        //                        //else ShortStr += cat.Noun + sq.Separator;


        //                        if (cat.Noun != "OEM" && cat.Noun != "PARTS")
        //                        {
        //                            if (cat.Modifier != "--" && cat.Modifier != "NO MODIFIER")
        //                                ShortStr += cat.Noun + sq.Separator + cat.Modifier + sq.Separator;
        //                            else
        //                                ShortStr += cat.Noun + sq.Separator;
        //                        }
        //                        else
        //                        {
        //                            ShortStr += "";
        //                        }

        //                        //if (cat.Modifier != "--" && cat.Modifier != "NO MODIFIER")
        //                        //{
        //                        //    if (NMList != null && !string.IsNullOrEmpty(NMList.Modifierabv))
        //                        //        ShortStr += NMList.Modifierabv + sq.Separator;
        //                        //    else
        //                        //        ShortStr += cat.Modifier + sq.Separator;
        //                        //}

        //                        strNM = ShortStr;

        //                        break;



        //                    case 103:
        //                        int flg = 0;
        //                        int i = 0;
        //                        if (cat.Characteristics != null)
        //                        {
        //                            var dumbCharacteristics = new List<Prosol_AttributeList>();
        //                            foreach (var cha in cat.Characteristics)
        //                            {
        //                                var dumbCha = new Prosol_AttributeList
        //                                {
        //                                    Characteristic = cha.Characteristic,
        //                                    Value = cha.Value,
        //                                    UOM = cha.UOM,
        //                                    ShortSquence = cha.ShortSquence,
        //                                    Source = cha.Source,
        //                                    SourceUrl = cha.SourceUrl,
        //                                    Squence = cha.Squence,
        //                                };
        //                                dumbCharacteristics.Add(dumbCha);
        //                            }

        //                            bool containsAdditionalInformation = cat.Characteristics
        //                                                                    .GroupBy(ix => ix.Characteristic)
        //                                                                    .Any(g => g.Key == "ADDITIONAL INFORMATION");
        //                            if (containsAdditionalInformation)
        //                            {
        //                                foreach (var ch in cat.Characteristics)
        //                                {
        //                                    if (ch.Characteristic == "ADDITIONAL INFORMATION")
        //                                    {
        //                                        if (!string.IsNullOrEmpty(ch.Value))
        //                                        {
        //                                            var Value = ch.Value;
        //                                            string pattern = @"(\w+(\s\w+)*):";
        //                                            MatchCollection matches = Regex.Matches(ch.Value, pattern);
        //                                            List<string> result = new List<string>();
        //                                            foreach (Match match in matches)
        //                                            {
        //                                                result.Add(match.Groups[1].Value);
        //                                            }

        //                                            //var qry = Query.EQ("Characteristic", "MM");
        //                                            //var charLst = _CharacteristicRepository.FindAll(qry).ToList();
        //                                            //var unqCharLst = charLst.GroupBy(p => p.Characteristic).Select(g => g.First()).ToList();
        //                                            foreach (var unq in result)
        //                                            {
        //                                                var qry = Query.EQ("Characteristic", unq);
        //                                                var charLst = _CharacteristicRepository.FindOne(qry);
        //                                                if (charLst != null)
        //                                                {
        //                                                    if (Value.Contains(charLst.Characteristic))
        //                                                    {
        //                                                        if (charLst.Abbrivation.Contains("_"))
        //                                                            charLst.Abbrivation = charLst.Abbrivation.Replace("_", " ");
        //                                                        Value = Value.Replace(charLst.Characteristic, charLst.Abbrivation);
        //                                                    }
        //                                                }
        //                                            }
        //                                            cat.Additionalinfo = Value;
        //                                        }
        //                                    }
        //                                }
        //                                //cat.Characteristics.RemoveAll(c => c.Characteristic == "ADDITIONAL INFORMATION");
        //                            }


        //                            foreach (var chM in dumbCharacteristics.OrderBy(x => x.ShortSquence))
        //                            {
        //                                if (NMList.Formatted == 1 || flg == 1 || NMList.Formatted == 2)//&& NMList.Formatted != 0
        //                                {

        //                                    if (chM.Value != null && chM.Value != "" && chM.Value != "--" && chM.Characteristic != "ADDITIONAL INFORMATION")
        //                                    {
        //                                        string strValue = "";
        //                                        var frmMdl = new shortFrame();
        //                                        frmMdl.position = chM.Squence;


        //                                        if (chM.Value.Contains(','))
        //                                        {
        //                                            string tmpstr = "";
        //                                            var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                            {
        //                                                tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                            }
        //                                            else
        //                                            {
        //                                                string[] strsplt = chM.Value.Split(',');
        //                                                foreach (string str in strsplt)
        //                                                {


        //                                                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                                    else tmpstr += str.TrimStart().TrimEnd() + ',';

        //                                                }
        //                                            }
        //                                            tmpstr = tmpstr.TrimEnd(',');
        //                                            if (UOMSet.Short_space == "with space")
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += tmpstr + " " + chM.UOM + sq.Separator;
        //                                                else strValue += tmpstr + sq.Separator;
        //                                            }
        //                                            else
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += tmpstr + chM.UOM + sq.Separator;
        //                                                else strValue += tmpstr + sq.Separator;
        //                                            }
        //                                            frmMdl.values = strValue;
        //                                        }
        //                                        else
        //                                        {
        //                                            string tmpstr = "";

        //                                            var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                            {
        //                                                // Abbreivated
        //                                                if (UOMSet.Short_space == "with space")
        //                                                {
        //                                                    if (chM.UOM != null && chM.UOM != "")
        //                                                        strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                                    else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                                }
        //                                                else
        //                                                {
        //                                                    if (chM.UOM != null && chM.UOM != "")
        //                                                        strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                                    else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                                }
        //                                                frmMdl.values = strValue;
        //                                            }
        //                                            else
        //                                            {
        //                                                // Abbreivated not exist

        //                                                if (UOMSet.Short_space == "with space")
        //                                                {
        //                                                    if (chM.UOM != null && chM.UOM != "")
        //                                                        strValue += chM.Value.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                                    else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                                }
        //                                                else
        //                                                {
        //                                                    if (chM.UOM != null && chM.UOM != "")
        //                                                        strValue += chM.Value.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                                    else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                                }
        //                                                frmMdl.values = strValue;
        //                                            }


        //                                        }

        //                                        lst.Add(frmMdl);

        //                                        ShortStr = strNM;
        //                                        string pattern = " X ";
        //                                        foreach (shortFrame sMdl in lst)
        //                                        {
        //                                            string[] strtmp = { " X " };
        //                                            var x = sMdl.values.Split(strtmp, StringSplitOptions.RemoveEmptyEntries);
        //                                            if (Regex.IsMatch(x[0], "^[0-9]+$", RegexOptions.Compiled))
        //                                            {
        //                                                foreach (Match match in Regex.Matches(sMdl.values, pattern))
        //                                                {
        //                                                    sMdl.values = Regex.Replace(sMdl.values, pattern, match.Value.TrimEnd().TrimStart());
        //                                                }
        //                                                // ShortStr += sMdl.values;
        //                                                string pattern1 = " x ";
        //                                                foreach (Match match in Regex.Matches(sMdl.values, pattern1))
        //                                                {
        //                                                    sMdl.values = Regex.Replace(sMdl.values, pattern1, match.Value.TrimEnd().TrimStart());
        //                                                }
        //                                            }
        //                                            ShortStr += sMdl.values;
        //                                        }

        //                                        if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                        {
        //                                            ShortStr = ShortStr.Trim();
        //                                            char[] chr = sq.Separator.ToCharArray();
        //                                            ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                            while (ShortStr.Length > seqList[0].ShortLength)
        //                                            {
        //                                                int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                                if (lstIndx > -1)
        //                                                {
        //                                                    if (lst.Count > 0)
        //                                                    {

        //                                                        if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                        {
        //                                                            if (ShortStr.Substring(lstIndx).TrimStart(chr[0]) == lst[lst.Count - 1].values.TrimEnd(chr[0]))
        //                                                            {
        //                                                                lst.RemoveAt(lst.Count - 1);
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                int indx = lst[lst.Count - 1].values.TrimEnd(chr[0]).LastIndexOf(chr[0]);
        //                                                                lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx) + chr[0];
        //                                                            }
        //                                                        }
        //                                                    }
        //                                                    ShortStr = ShortStr.Remove(lstIndx);

        //                                                }
        //                                                else
        //                                                {
        //                                                    lstIndx = ShortStr.LastIndexOf(' ');
        //                                                    ShortStr = ShortStr.Remove(lstIndx);
        //                                                    if (lst.Count > 0)
        //                                                    {
        //                                                        int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                        lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                                    }
        //                                                }

        //                                            }
        //                                            ShortStr = ShortStr + chr[0];
        //                                        }
        //                                        i++;
        //                                    }
        //                                }
        //                                else flg = 1;


        //                            }
        //                            ShortStr = strNM;
        //                            foreach (shortFrame sMdl in lst.OrderBy(x => x.position))
        //                            {
        //                                ShortStr += sMdl.values;
        //                            }

        //                        }

        //                        break;


        //                    case 104:
        //                        if (cat.Vendorsuppliers != null)
        //                        {
        //                            foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                            {
        //                                if (vs.s == 1)
        //                                {
        //                                    if (vs.Name != null && vs.Name != "")
        //                                    {
        //                                        var querry = Query.EQ("Name", vs.Name);
        //                                        var shtmfr = _VendorRepository.FindOne(querry);
        //                                        if (shtmfr != null)
        //                                        {
        //                                            if (shtmfr.ShortDescName != null && shtmfr.ShortDescName != "")
        //                                            {
        //                                                vs.shortmfr = shtmfr.ShortDescName;
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            vs.shortmfr = vs.Name;
        //                                        }
        //                                    }
        //                                    //else
        //                                    //{
        //                                    //    vs.shortmfr = vs.Name;
        //                                    //}

        //                                    if (vs.shortmfr != null && vs.shortmfr != "" && vs.shortmfr != "undefined")
        //                                    {
        //                                        mfrref = vs.Name;
        //                                        var frmMdl = new shortFrame();
        //                                        frmMdl.position = 101;
        //                                        frmMdl.values = vs.shortmfr + sq.Separator;
        //                                        lst.Add(frmMdl);
        //                                        ShortStr += vs.shortmfr + sq.Separator;
        //                                        if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                        {
        //                                            ShortStr = ShortStr.Trim();
        //                                            char[] chr = sq.Separator.ToCharArray();
        //                                            ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                            while (ShortStr.Length > seqList[0].ShortLength)
        //                                            {
        //                                                int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                                if (lstIndx > -1)
        //                                                {
        //                                                    if (lst.Count > 0)
        //                                                    {
        //                                                        if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                            lst.RemoveAt(lst.Count - 1);
        //                                                    }
        //                                                    ShortStr = ShortStr.Remove(lstIndx);

        //                                                }
        //                                                else
        //                                                {
        //                                                    lstIndx = ShortStr.LastIndexOf(' ');
        //                                                    ShortStr = ShortStr.Remove(lstIndx);
        //                                                    if (lst.Count > 0)
        //                                                    {
        //                                                        int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                        lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                                    }
        //                                                }
        //                                            }
        //                                            ShortStr = ShortStr + chr[0];
        //                                        }
        //                                        break;
        //                                    }

        //                                }
        //                            }
        //                        }
        //                        break;
        //                    case 105:
        //                        if (cat.Vendorsuppliers != null)
        //                        {
        //                            foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                            {
        //                                if (vs.s == 1 && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined")
        //                                {
        //                                    string prefix = "";
        //                                    if (vs.Refflag != null)
        //                                    {
        //                                        var flag = Query.EQ("Type", vs.Refflag);
        //                                        var code = _ReftypeRepository.FindOne(flag);
        //                                        if (code != null)
        //                                        {
        //                                            prefix = code.Code + ":";
        //                                        }
        //                                    }
        //                                    // ShortStr = strNM;
        //                                    if (vs.Refflag == "DRAWING NUMBER")
        //                                    {
        //                                        ShortStr += prefix + vs.RefNo.Trim() + sq.Separator;
        //                                        var frmMdl = new shortFrame();
        //                                        frmMdl.position = 100;
        //                                        frmMdl.values = prefix + vs.RefNo.Trim() + sq.Separator;
        //                                        lst.Add(frmMdl);
        //                                    }
        //                                    else
        //                                    {
        //                                        ShortStr += vs.RefNo.Trim() + sq.Separator;
        //                                        var frmMdl = new shortFrame();
        //                                        frmMdl.position = 100;
        //                                        frmMdl.values = vs.RefNo.Trim() + sq.Separator;
        //                                        lst.Add(frmMdl);
        //                                    }
        //                                    if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                    {
        //                                        ShortStr = ShortStr.Trim();
        //                                        char[] chr = sq.Separator.ToCharArray();
        //                                        ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                        while (ShortStr.Length > seqList[0].ShortLength)
        //                                        {
        //                                            int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                            if (lstIndx > -1)
        //                                            {
        //                                                if (lst.Count > 0)
        //                                                {
        //                                                    if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                        lst.RemoveAt(lst.Count - 1);
        //                                                }
        //                                                ShortStr = ShortStr.Remove(lstIndx);

        //                                            }
        //                                            else
        //                                            {
        //                                                lstIndx = ShortStr.LastIndexOf(' ');
        //                                                ShortStr = ShortStr.Remove(lstIndx);
        //                                                if (lst.Count > 0)
        //                                                {
        //                                                    int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                    lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                                }
        //                                            }

        //                                        }
        //                                        ShortStr = ShortStr + chr[0];
        //                                    }
        //                                    break;
        //                                }

        //                            }
        //                        }

        //                        break;
        //                    case 106:
        //                        if (cat.Application != null && cat.Application != "")
        //                            ShortStr += cat.Application + sq.Separator;
        //                        break;
        //                    case 107:
        //                        //if (cat.Drawingno != null && cat.Drawingno != "")
        //                        //    ShortStr += cat.Drawingno + sq.Separator;


        //                        foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                        {
        //                            if (vs.Refflag.ToUpper() == "DRAWING & POSITION NUMBER" && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined" && vs.Name == mfrref && vs.Name != "" && vs.Name != null)
        //                            {
        //                                ShortStr += vs.RefNo.Trim() + sq.Separator;
        //                                break;
        //                            }
        //                        }
        //                        break;
        //                    //case 108:

        //                    //    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "" && NMList.Formatted != 0)
        //                    //    {
        //                    //        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Equipment.Name, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                    //        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                    //        {
        //                    //            ShortStr += "F/" + abbObj.Abbrevated + sq.Separator;
        //                    //        }
        //                    //        else
        //                    //        {
        //                    //            if (cat.Equipment.Name.Contains(' '))
        //                    //            {
        //                    //                string tmpstr = "";

        //                    //                string[] strsplt = cat.Equipment.Name.Split(' ');
        //                    //                foreach (string str in strsplt)
        //                    //                {
        //                    //                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                    //                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                    //                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
        //                    //                    else tmpstr += str.TrimStart().TrimEnd() + ' ';
        //                    //                }

        //                    //                tmpstr = tmpstr.TrimEnd(' ');
        //                    //                ShortStr += "F/" + tmpstr + sq.Separator;
        //                    //            }
        //                    //            else
        //                    //            {
        //                    //                ShortStr += "F/" + cat.Equipment.Name + sq.Separator;
        //                    //            }

        //                    //        }
        //                    //        if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                    //        {
        //                    //            ShortStr = ShortStr.Trim();
        //                    //            char[] chr = sq.Separator.ToCharArray();
        //                    //            ShortStr = ShortStr.TrimEnd(chr[0]);
        //                    //            while (ShortStr.Length > seqList[0].ShortLength)
        //                    //            {
        //                    //                int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                    //                ShortStr = ShortStr.Remove(lstIndx);
        //                    //            }
        //                    //            ShortStr += sq.Separator;
        //                    //            break;
        //                    //        }
        //                    //    }
        //                    //    //ShortStr += "F/" + cat.Equipment.Name + sq.Separator;

        //                    //    break;
        //                    case 108:

        //                        if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "" && cat.Equipment.ENS == 1)
        //                        {
        //                            var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Equipment.Name, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                            {
        //                                ShortStr += "F/" + abbObj.Abbrevated + sq.Separator;
        //                            }
        //                            else
        //                                ShortStr += "F/" + cat.Equipment.Name + sq.Separator;
        //                        }
        //                        //ShortStr += "F/" + cat.Equipment.Name + sq.Separator;

        //                        break;
        //                    case 109:
        //                        if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "" && NMList.Formatted != 0)
        //                            ShortStr += cat.Equipment.Manufacturer + sq.Separator;
        //                        break;
        //                    case 110:
        //                        if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "" && NMList.Formatted != 0)
        //                            ShortStr += cat.Equipment.Modelno + sq.Separator;
        //                        break;
        //                    case 111:
        //                        if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "" && NMList.Formatted != 0)
        //                            ShortStr += cat.Equipment.Tagno + sq.Separator;
        //                        break;
        //                    case 112:
        //                        if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "" && NMList.Formatted != 0)
        //                            ShortStr += cat.Equipment.Serialno + sq.Separator;
        //                        break;
        //                    //case 108:
        //                    //    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "")
        //                    //        ShortStr += cat.Equipment.Name + sq.Separator;
        //                    //    break;
        //                    //case 109:
        //                    //    if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
        //                    //        ShortStr += cat.Equipment.Manufacturer + sq.Separator;
        //                    //    break;
        //                    //case 110:
        //                    //    if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "")
        //                    //        ShortStr += cat.Equipment.Modelno + sq.Separator;
        //                    //    break;
        //                    //case 111:
        //                    //    if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "")
        //                    //        ShortStr += cat.Equipment.Tagno + sq.Separator;
        //                    //    break;
        //                    //case 112:
        //                    //    if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "")
        //                    //        ShortStr += cat.Equipment.Serialno + sq.Separator;
        //                    //    break;
        //                    case 113:
        //                        if (cat.Referenceno != null && cat.Referenceno != "" && NMList.Formatted != 0)
        //                            ShortStr += cat.Referenceno + sq.Separator;
        //                        break;
        //                    case 114:
        //                        var charQry1 = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier), Query.EQ("Characteristic", "ADDITIONAL INFORMATION"));
        //                        var charLst1 = _CharacteristicRepository.FindOne(charQry1);
        //                        if (cat.Additionalinfo != null && cat.Additionalinfo != "")
        //                            ShortStr += charLst1.Abbrivation.Replace("_", " ") + ":" + cat.Additionalinfo + sq.Separator;
        //                        break;
        //                    case 115:
        //                        if (cat.Equipment != null && cat.Equipment.Additionalinfo != null && cat.Equipment.Additionalinfo != "" && NMList.Formatted != 0)
        //                            ShortStr += cat.Equipment.Additionalinfo + sq.Separator;
        //                        break;


        //                }
        //                if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                {
        //                    ShortStr = ShortStr.Trim();
        //                    char[] chr = sq.Separator.ToCharArray();
        //                    ShortStr = ShortStr.TrimEnd(chr[0]);
        //                    while (ShortStr.Length > seqList[0].ShortLength)
        //                    {
        //                        int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                        ShortStr = ShortStr.Remove(lstIndx);
        //                    }
        //                    break;
        //                }
        //            }
        //        }
        //        seqList[0].ShortLength = 38;
        //        if (ShortStr.Length != seqList[0].ShortLength)
        //        {
        //            ShortStr = ShortStr.Trim();
        //            int lsIndx = ShortStr.Length;
        //            string str = ShortStr.Substring(lsIndx - 1, 1);
        //            if (str == seqList[0].Separator.Trim())
        //            {
        //                ShortStr = ShortStr.Remove(lsIndx - 1);
        //            }
        //        }
        //    }

        //    else
        //    {
        //        foreach (Prosol_Sequence sq in seqList)
        //        {
        //            if (sq.Required == "Yes")
        //            {
        //                switch (sq.CatId)
        //                {
        //                    case 101:
        //                        //if (NMList != null && !string.IsNullOrEmpty(NMList.Nounabv))
        //                        //    ShortStr += NMList.Nounabv + sq.Separator;
        //                        //else ShortStr += cat.Noun + sq.Separator;


        //                        if (cat.Noun != "OEM" && cat.Noun != "PARTS")
        //                        {
        //                            if (cat.Modifier != "--" && cat.Modifier != "NO MODIFIER")
        //                                strNM += cat.Noun + sq.Separator + cat.Modifier + sq.Separator;
        //                            else
        //                                strNM += cat.Noun + sq.Separator;
        //                        }
        //                        else
        //                        {
        //                            strNM += "";
        //                        }

        //                        //if (cat.Modifier != "--" && cat.Modifier != "NO MODIFIER")
        //                        //{
        //                        //    if (NMList != null && !string.IsNullOrEmpty(NMList.Modifierabv))
        //                        //        ShortStr += NMList.Modifierabv + sq.Separator;
        //                        //    else
        //                        //        ShortStr += cat.Modifier + sq.Separator;
        //                        //}


        //                        break;
        //                }
        //                if (!checkLength(strNM, seqList[0].ShortLength))
        //                {
        //                    strNM = strNM.Trim();
        //                    char[] chr = sq.Separator.ToCharArray();
        //                    strNM = strNM.TrimEnd(chr[0]);
        //                    while (strNM.Length > seqList[0].ShortLength)
        //                    {
        //                        int lstIndx = strNM.LastIndexOf(chr[0]);
        //                        strNM = strNM.Remove(lstIndx);
        //                    }
        //                    break;
        //                }
        //            }
        //        }
        //        //strNM += ",";
        //        currentLen -= strNM.Length;
        //        if (currentLen > 0)
        //        {
        //            foreach (Prosol_Sequence sq in seqList)
        //            {
        //                if (sq.Required == "Yes")
        //                {
        //                    switch (sq.CatId)
        //                    {

        //                        case 105:
        //                            int venFlg = 0;
        //                            if (cat.Vendorsuppliers != null)
        //                            {
        //                                foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                                {
        //                                    if (venFlg == 0 && vs.s == 1 && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined")
        //                                    {
        //                                        string prefix = "";
        //                                        if (vs.Refflag != null)
        //                                        {
        //                                            var flag = Query.EQ("Type", vs.Refflag);
        //                                            var code = _ReftypeRepository.FindOne(flag);
        //                                            if (code != null)
        //                                            {
        //                                                prefix = code.Code + ":";
        //                                            }
        //                                        }
        //                                        // ShortStr = strNM;
        //                                        if (vs.Refflag == "DRAWING NUMBER")
        //                                        {
        //                                            strVenn += prefix + vs.RefNo.Trim() + sq.Separator;
        //                                            var frmMdl = new shortFrame();
        //                                            frmMdl.position = 100;
        //                                            frmMdl.values = prefix + vs.RefNo.Trim() + sq.Separator;
        //                                            lst.Add(frmMdl);
        //                                        }
        //                                        else
        //                                        {
        //                                            strVenn += vs.RefNo.Trim() + sq.Separator;
        //                                            var frmMdl = new shortFrame();
        //                                            frmMdl.position = 100;
        //                                            frmMdl.values = vs.RefNo.Trim() + sq.Separator;
        //                                            lst.Add(frmMdl);
        //                                        }
        //                                        if (!checkLength(strVenn, seqList[0].ShortLength))
        //                                        {
        //                                            strVenn = strVenn.Trim();
        //                                            char[] chr = sq.Separator.ToCharArray();
        //                                            strVenn = strVenn.TrimEnd(chr[0]);
        //                                            while (strVenn.Length > seqList[0].ShortLength)
        //                                            {
        //                                                int lstIndx = strVenn.LastIndexOf(chr[0]);
        //                                                if (lstIndx > -1)
        //                                                {
        //                                                    if (lst.Count > 0)
        //                                                    {
        //                                                        if (strVenn.Substring(lstIndx).Length > 1)
        //                                                            lst.RemoveAt(lst.Count - 1);
        //                                                    }
        //                                                    strVenn = strVenn.Remove(lstIndx);

        //                                                }
        //                                                else
        //                                                {
        //                                                    lstIndx = strVenn.LastIndexOf(' ');
        //                                                    strVenn = strVenn.Remove(lstIndx);
        //                                                    if (lst.Count > 0)
        //                                                    {
        //                                                        int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                        lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                                    }
        //                                                }

        //                                            }
        //                                            strVenn = strVenn + chr[0];
        //                                        }
        //                                        break;
        //                                    }

        //                                }
        //                                venFlg++;
        //                            }

        //                            break;
        //                    }
        //                    if (!checkLength(strVenn, currentLen))
        //                    {
        //                        strVenn = strVenn.Trim();
        //                        char[] chr = sq.Separator.ToCharArray();
        //                        strVenn = strVenn.TrimEnd(chr[0]);
        //                        while (strVenn.Length > seqList[0].ShortLength)
        //                        {
        //                            int lstIndx = strVenn.LastIndexOf(chr[0]);
        //                            strVenn = strVenn.Remove(lstIndx);
        //                        }
        //                        break;
        //                    }
        //                }
        //            }
        //            strVenn += ",";
        //            currentLen -= strVenn.Length;
        //        }
        //        if (currentLen > 0)
        //        {
        //            foreach (Prosol_Sequence sq in seqList)
        //            {
        //                if (sq.Required == "Yes")
        //                {
        //                    switch (sq.CatId)
        //                    {
        //                        case 104:
        //                            int mfrFlg = 0;
        //                            if (cat.Vendorsuppliers != null)
        //                            {
        //                                foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                                {
        //                                    if (mfrFlg == 0 && vs.s == 1 && !string.IsNullOrEmpty(vs.Name))
        //                                    {
        //                                        var Name = Query.EQ("Name", vs.Name);
        //                                        var shtmfr = _VendorRepository.FindOne(Name);

        //                                        // Determine the short manufacturer name
        //                                        vs.shortmfr = shtmfr?.ShortDescName ?? vs.Name;

        //                                        // Proceed only if the short manufacturer name is valid
        //                                        if (!string.IsNullOrEmpty(vs.shortmfr) && vs.shortmfr != "undefined")
        //                                        {
        //                                            var newEntry = vs.shortmfr + sq.Separator;

        //                                            // Check if adding the new entry exceeds the currentLen
        //                                            if (strVen.Length + newEntry.Length <= currentLen)
        //                                            {
        //                                                // Add the new entry
        //                                                strVen += newEntry;
        //                                                lst.Add(new shortFrame { position = 101, values = vs.shortmfr + sq.Separator });
        //                                            }
        //                                            else
        //                                            {
        //                                                // Trim strVen to respect currentLen
        //                                                while (strVen.Length + newEntry.Length > currentLen)
        //                                                {
        //                                                    // Only remove if there is something to remove
        //                                                    if (lst.Count > 0)
        //                                                    {
        //                                                        var lastEntry = lst[lst.Count - 1];
        //                                                        int lastEntryIndex = strVen.LastIndexOf(lastEntry.values);

        //                                                        // Ensure the index is valid
        //                                                        if (lastEntryIndex != -1)
        //                                                        {
        //                                                            strVen = strVen.Remove(lastEntryIndex, lastEntry.values.Length);
        //                                                            lst.RemoveAt(lst.Count - 1);
        //                                                        }
        //                                                        else
        //                                                        {
        //                                                            // If no valid index, exit the loop
        //                                                            break;
        //                                                        }
        //                                                    }
        //                                                }

        //                                                // Finally, add the new entry if there's space
        //                                                if (strVen.Length + newEntry.Length <= currentLen)
        //                                                {
        //                                                    strVen += newEntry;
        //                                                    lst.Add(new shortFrame { position = 101, values = vs.shortmfr + sq.Separator });
        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                    mfrFlg++;
        //                                }
        //                            }
        //                            break;
        //                    }

        //                    // Final check to ensure strVen does not exceed ShortLength defined in seqList[0]
        //                    if (!checkLength(strVen, currentLen))
        //                    {
        //                        strVen = strVen.Trim();
        //                        char[] chr = sq.Separator.ToCharArray();
        //                        strVen = strVen.TrimEnd(chr[0]);

        //                        // Trim until we meet the length requirement
        //                        while (strVen.Length > seqList[0].ShortLength)
        //                        {
        //                            int lstIndx = strVen.LastIndexOf(chr[0]);
        //                            // Ensure the index is valid before removing
        //                            if (lstIndx != -1)
        //                            {
        //                                strVen = strVen.Remove(lstIndx);
        //                            }
        //                            else
        //                            {
        //                                // If there's no separator left, just break
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            strVen += ","; // Append a comma at the end
        //            currentLen -= strVen.Length; // Update the current length
        //        }

        //        if (currentLen > 0)
        //        {
        //            foreach (Prosol_Sequence sq in seqList)
        //            {
        //                if (sq.Required == "Yes")
        //                {
        //                    switch (sq.CatId)
        //                    {
        //                        case 103:
        //                            if (cat.Characteristics != null)
        //                            {
        //                                bool containsAdditionalInformation = cat.Characteristics
        //                                    .GroupBy(i => i.Characteristic)
        //                                    .Any(g => g.Key == "ADDITIONAL INFORMATION");

        //                                if (containsAdditionalInformation)
        //                                {
        //                                    foreach (var ch in cat.Characteristics)
        //                                    {
        //                                        if (ch.Characteristic == "ADDITIONAL INFORMATION" && !string.IsNullOrEmpty(ch.Value))
        //                                        {
        //                                            var Value = ch.Value;
        //                                            string pattern = @"(\w+(\s\w+)*):";
        //                                            MatchCollection matches = Regex.Matches(ch.Value, pattern);
        //                                            List<string> result = new List<string>();

        //                                            foreach (Match match in matches)
        //                                            {
        //                                                result.Add(match.Groups[1].Value);
        //                                            }

        //                                            foreach (var unq in result)
        //                                            {
        //                                                var qry = Query.EQ("Characteristic", unq);
        //                                                var charLst = _CharacteristicRepository.FindOne(qry);

        //                                                if (charLst != null && Value.Contains(charLst.Characteristic))
        //                                                {
        //                                                    if (charLst.Abbrivation.Contains("_"))
        //                                                        charLst.Abbrivation = charLst.Abbrivation.Replace("_", " ");

        //                                                    Value = Value.Replace(charLst.Characteristic, charLst.Abbrivation);
        //                                                }
        //                                            }

        //                                            cat.Additionalinfo = Value;
        //                                        }
        //                                    }
        //                                }

        //                                // Initialize total added length
        //                                int remainingLength = currentLen - strChar.Length;

        //                                foreach (Prosol_AttributeList chM in cat.Characteristics.OrderBy(x => x.Squence))
        //                                {
        //                                    var charQry = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier), Query.EQ("Characteristic", chM.Characteristic));
        //                                    var charLst = _CharacteristicRepository.FindOne(charQry);
        //                                    var tempCha = charLst != null && !string.IsNullOrEmpty(charLst.Abbrivation)
        //                                        ? charLst.Abbrivation.Replace("_", " ")
        //                                        : charLst?.Characteristic;

        //                                    if (!string.IsNullOrEmpty(chM.Value) && chM.Characteristic != "ADDITIONAL INFORMATION" && chM.Value != "--")
        //                                    {
        //                                        string addition = string.IsNullOrEmpty(chM.UOM)
        //                                            ? $"{chM.Value}{sq.Separator}"
        //                                            : $"{chM.Value} {chM.UOM}{sq.Separator}";

        //                                        // Check if the addition fits within the remaining length
        //                                        if (addition.Length <= remainingLength)
        //                                        {
        //                                            strChar += addition;
        //                                            remainingLength -= addition.Length;  // Decrease remaining length
        //                                        }
        //                                    }
        //                                }

        //                                // If the strChar length exceeds the allowed length, trim it down
        //                                if (!checkLength(strChar, currentLen))
        //                                {
        //                                    strChar = strChar.Trim();
        //                                    char[] chr = sq.Separator.ToCharArray();
        //                                    strChar = strChar.TrimEnd(chr[0]);

        //                                    while (strChar.Length > seqList[0].ShortLength)
        //                                    {
        //                                        int lstIndx = strChar.LastIndexOf(chr[0]);
        //                                        strChar = strChar.Remove(lstIndx);
        //                                    }
        //                                }
        //                            }
        //                            break;
        //                    }
        //                }
        //            }


        //            strChar += ",";
        //            currentLen -= strChar.Length;
        //        }

        //        if (currentLen > 0)
        //        {
        //            foreach (Prosol_Sequence sq in seqList)
        //            {
        //                if (sq.Required == "Yes")
        //                {
        //                    switch (sq.CatId)
        //                    {
        //                        case 106:
        //                            if (cat.Application != null && cat.Application != "")
        //                                strFinal += cat.Application + sq.Separator;
        //                            break;
        //                        case 107:
        //                            //if (cat.Drawingno != null && cat.Drawingno != "")
        //                            //    strFinal += cat.Drawingno + sq.Separator;


        //                            foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                            {
        //                                if (vs.Refflag.ToUpper() == "DRAWING & POSITION NUMBER" && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined" && vs.Name == mfrref && vs.Name != "" && vs.Name != null)
        //                                {
        //                                    strFinal += vs.RefNo.Trim() + sq.Separator;
        //                                    break;
        //                                }
        //                            }
        //                            break;
        //                        //case 108:

        //                        //    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "" && NMList.Formatted != 0)
        //                        //    {
        //                        //        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Equipment.Name, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                        //        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                        //        {
        //                        //            strFinal += "F/" + abbObj.Abbrevated + sq.Separator;
        //                        //        }
        //                        //        else
        //                        //        {
        //                        //            if (cat.Equipment.Name.Contains(' '))
        //                        //            {
        //                        //                string tmpstr = "";

        //                        //                string[] strsplt = cat.Equipment.Name.Split(' ');
        //                        //                foreach (string str in strsplt)
        //                        //                {
        //                        //                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                        //                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                        //                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
        //                        //                    else tmpstr += str.TrimStart().TrimEnd() + ' ';
        //                        //                }

        //                        //                tmpstr = tmpstr.TrimEnd(' ');
        //                        //                strFinal += "F/" + tmpstr + sq.Separator;
        //                        //            }
        //                        //            else
        //                        //            {
        //                        //                strFinal += "F/" + cat.Equipment.Name + sq.Separator;
        //                        //            }

        //                        //        }
        //                        //        if (!checkLength(strFinal, seqList[0].ShortLength))
        //                        //        {
        //                        //            strFinal = strFinal.Trim();
        //                        //            char[] chr = sq.Separator.ToCharArray();
        //                        //            strFinal = strFinal.TrimEnd(chr[0]);
        //                        //            while (strFinal.Length > seqList[0].ShortLength)
        //                        //            {
        //                        //                int lstIndx = strFinal.LastIndexOf(chr[0]);
        //                        //                strFinal = strFinal.Remove(lstIndx);
        //                        //            }
        //                        //            strFinal += sq.Separator;
        //                        //            break;
        //                        //        }
        //                        //    }
        //                        //    //strFinal += "F/" + cat.Equipment.Name + sq.Separator;

        //                        //    break;
        //                        case 108:

        //                            if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "" && cat.Equipment.ENS == 1)
        //                            {
        //                                var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Equipment.Name, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                {
        //                                    strFinal += "F/" + abbObj.Abbrevated + sq.Separator;
        //                                }
        //                                else
        //                                    strFinal += "F/" + cat.Equipment.Name + sq.Separator;
        //                            }
        //                            //strFinal += "F/" + cat.Equipment.Name + sq.Separator;

        //                            break;
        //                        case 109:
        //                            if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "" && NMList.Formatted != 0)
        //                                strFinal += cat.Equipment.Manufacturer + sq.Separator;
        //                            break;
        //                        case 110:
        //                            if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "" && NMList.Formatted != 0)
        //                                strFinal += cat.Equipment.Modelno + sq.Separator;
        //                            break;
        //                        case 111:
        //                            if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "" && NMList.Formatted != 0)
        //                                strFinal += cat.Equipment.Tagno + sq.Separator;
        //                            break;
        //                        case 112:
        //                            if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "" && NMList.Formatted != 0)
        //                                strFinal += cat.Equipment.Serialno + sq.Separator;
        //                            break;
        //                        //case 108:
        //                        //    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "")
        //                        //        strFinal += cat.Equipment.Name + sq.Separator;
        //                        //    break;
        //                        //case 109:
        //                        //    if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
        //                        //        strFinal += cat.Equipment.Manufacturer + sq.Separator;
        //                        //    break;
        //                        //case 110:
        //                        //    if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "")
        //                        //        strFinal += cat.Equipment.Modelno + sq.Separator;
        //                        //    break;
        //                        //case 111:
        //                        //    if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "")
        //                        //        strFinal += cat.Equipment.Tagno + sq.Separator;
        //                        //    break;
        //                        //case 112:
        //                        //    if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "")
        //                        //        strFinal += cat.Equipment.Serialno + sq.Separator;
        //                        //    break;
        //                        case 113:
        //                            if (cat.Referenceno != null && cat.Referenceno != "" && NMList.Formatted != 0)
        //                                strFinal += cat.Referenceno + sq.Separator;
        //                            break;
        //                        case 114:
        //                            if (cat.Additionalinfo != null && cat.Additionalinfo != "" && NMList.Formatted != 0)
        //                                strFinal += cat.Additionalinfo + sq.Separator;
        //                            break;
        //                        case 115:
        //                            if (cat.Equipment != null && cat.Equipment.Additionalinfo != null && cat.Equipment.Additionalinfo != "" && NMList.Formatted != 0)
        //                                strFinal += cat.Equipment.Additionalinfo + sq.Separator;
        //                            break;


        //                    }
        //                    if (!checkLength(strFinal, seqList[0].ShortLength))
        //                    {
        //                        strFinal = strFinal.Trim();
        //                        char[] chr = sq.Separator.ToCharArray();
        //                        strFinal = strFinal.TrimEnd(chr[0]);
        //                        while (strFinal.Length > seqList[0].ShortLength)
        //                        {
        //                            int lstIndx = strFinal.LastIndexOf(chr[0]);
        //                            strFinal = strFinal.Remove(lstIndx);
        //                        }
        //                        break;
        //                    }
        //                }
        //            }
        //        }

        //        ShortStr = strNM + strChar + strVenn + strVen + strFinal;
        //        if (ShortStr.StartsWith(","))
        //        {
        //            ShortStr = ShortStr.TrimStart(',');
        //        }
        //        if (ShortStr.EndsWith(","))
        //        {
        //            ShortStr = ShortStr.TrimEnd(',');
        //        }
        //        if (ShortStr.Contains(",,"))
        //        {
        //            ShortStr = ShortStr.Replace(",,", ",");
        //        }
        //    }
        //    //seqList[0].ShortLength = 38;
        //    //if (ShortStr.Length != seqList[0].ShortLength)
        //    //{
        //    //    ShortStr = ShortStr.Trim();
        //    //    int lsIndx = ShortStr.Length;
        //    //    string str = ShortStr.Substring(lsIndx - 1, 1);
        //    //    if (str == seqList[0].Separator.Trim())
        //    //    {
        //    //        ShortStr = ShortStr.Remove(lsIndx - 1);
        //    //    }
        //    //}

        //    //string[] parts = ShortStr.Split(',');

        //    //parts[1] = $"{parts[1].Replace("MM", "")}X{parts[2]}";
        //    //string result = string.Join(",", parts[0], parts[1], parts[3], parts[4]);

        //    //Console.WriteLine(result);
        //    //ShortStr = result;
        //    return ShortStr;

        //}


        //OLD

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
            List<shortFrame> lst = new List<shortFrame>();
            foreach (Prosol_Sequence sq in seqList)
            {
                if (sq.Required == "Yes")
                {
                    switch (sq.CatId)
                    {
                        case 101:
                            string nmStr = "";
                            //if (NMList != null && !string.IsNullOrEmpty(NMList.Nounabv))
                            //    ShortStr += NMList.Nounabv + sq.Separator;
                            //else ShortStr += cat.Noun + sq.Separator;


                            if (cat.Noun != "OEM" /*&& cat.Noun != "KIT" */&& cat.Modifier != "PARTS")
                            {
                                if (cat.Modifier != "--" && cat.Modifier != "NO MODIFIER")
                                {
                                    if (NMList != null && !string.IsNullOrEmpty(NMList.Nounabv))
                                    {
                                        ShortStr += NMList.Nounabv + sq.Separator;
                                    }
                                    else
                                        ShortStr += cat.Noun + sq.Separator + cat.Modifier + sq.Separator;
                                }
                                else
                                {
                                    if (NMList != null && !string.IsNullOrEmpty(NMList.Nounabv))
                                        ShortStr += NMList.Nounabv + sq.Separator;
                                    else
                                        ShortStr += cat.Noun + sq.Separator;
                                }
                            }
                            //else if (cat.Noun == "KIT")
                            //{
                            //    ShortStr += cat.Characteristics[0].Value + sq.Separator;
                            //}
                            else
                            {
                                ShortStr += "";

                            }

                            //if (cat.Modifier != "--" && cat.Modifier != "NO MODIFIER")
                            //{
                            //    if (NMList != null && !string.IsNullOrEmpty(NMList.Modifierabv))
                            //        ShortStr += NMList.Modifierabv + sq.Separator;
                            //    else
                            //        ShortStr += cat.Modifier + sq.Separator;
                            //}

                            strNM = ShortStr;

                            break;

                        case 103:
                            int flg = 0;
                            int i = 0;

                            //Vendor Length
                            string ven = "";
                            string reff = "";
                            string dumbVen = "";

                            if (cat.Vendorsuppliers != null && cat.Vendorsuppliers.Count > 0)
                            {
                                if (cat.Vendorsuppliers[0].s == 1)
                                {
                                    if (!string.IsNullOrEmpty(cat.Vendorsuppliers[0].Name))
                                    {
                                        var querry = Query.EQ("Name", cat.Vendorsuppliers[0].Name);
                                        var shtmfr = _VendorRepository.FindOne(querry);
                                        if (shtmfr != null)
                                        {
                                            if (shtmfr.ShortDescName != null && shtmfr.ShortDescName != "")
                                            {
                                                ven = shtmfr.ShortDescName;
                                            }
                                        }
                                        else
                                        {
                                            ven = cat.Vendorsuppliers[0].Name;
                                        }
                                    }
                                    //var querrry = Query.EQ("Type", cat.Vendorsuppliers[0].Refflag);
                                    //var refLst = _ReftypeRepository.FindOne(querrry);
                                    //if (refLst != null)
                                    //{
                                    //    if (refLst.Code != null && shtmfr.Code != "")
                                    //    {
                                    //        reff = refLst.Code+":"+ cat.Vendorsuppliers[0].RefNo;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    reff = cat.Vendorsuppliers[0].RefNo;
                                    //}  
                                }
                            }

                            dumbVen = "," + reff + "," + ven;
                            if (dumbVen.EndsWith(","))
                                dumbVen = dumbVen.TrimEnd(',');
                            int charLen = 0;
                            charLen = seqList[0].ShortLength - dumbVen.Length;
                            //if (cat.Noun == "OEM" && cat.Noun != "KIT")
                            //    charLen = seqList[0].ShortLength;

                            if (cat.Characteristics != null)
                            {

                                var dumbCharacteristics = new List<Prosol_AttributeList>();
                                foreach (var cha in cat.Characteristics)
                                {
                                    var dumbCha = new Prosol_AttributeList
                                    {
                                        Characteristic = cha.Characteristic,
                                        Value = cha.Value,
                                        UOM = cha.UOM,
                                        ShortSquence = cha.ShortSquence,
                                        Source = cha.Source,
                                        SourceUrl = cha.SourceUrl,
                                        Squence = cha.Squence,
                                    };
                                    dumbCharacteristics.Add(dumbCha);
                                }

                                foreach (Prosol_AttributeList chM in dumbCharacteristics.OrderBy(x => x.ShortSquence))
                                {

                                    string dumbChar = ShortStr;
                                    if (chM.Value != null && chM.Value != "" && chM.Value != "--")
                                        dumbChar = ShortStr + chM.Value;

                                    if (chM.UOM != null && chM.UOM != "")
                                        dumbChar = dumbChar + chM.UOM;

                                    if (dumbChar.Length < charLen)
                                    {
                                        if (NMList.Formatted == 1 || flg == 1 || NMList.Formatted == 2)//&& NMList.Formatted != 0
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
                                                    string tmpstr = "";

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

                                    //if (chM.Value != null && chM.Value != "")
                                    //{
                                    //    string strValue = "";
                                    //    var frmMdl = new shortFrame();
                                    //    frmMdl.position = chM.Squence;


                                    //    if (chM.Value.Contains(','))
                                    //    {
                                    //        string tmpstr = "";
                                    //        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                    //        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                    //        {
                                    //            tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
                                    //        }
                                    //        else
                                    //        {
                                    //            string[] strsplt = chM.Value.Split(',');
                                    //            foreach (string str in strsplt)
                                    //            {
                                    //                //for space split
                                    //                if (str.Contains(' '))
                                    //                {
                                    //                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                    //                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                    //                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
                                    //                    else
                                    //                    {
                                    //                        string[] spaceSpt = str.Split(' ');
                                    //                        foreach (string spceStr in spaceSpt)
                                    //                        {
                                    //                            abbObj = (from Abb in AbbrList where Abb.Value.Equals(spceStr, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                    //                            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                    //                                tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
                                    //                            else
                                    //                                tmpstr += spceStr.TrimStart().TrimEnd() + ' ';

                                    //                        }
                                    //                        tmpstr = tmpstr.TrimEnd(' ');
                                    //                    }


                                    //                }
                                    //                else
                                    //                {

                                    //                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                    //                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                    //                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
                                    //                    else tmpstr += str.TrimStart().TrimEnd() + ',';
                                    //                }
                                    //            }
                                    //        }
                                    //        tmpstr = tmpstr.TrimEnd(',');
                                    //        if (UOMSet.Short_space == "with space")
                                    //        {
                                    //            if (chM.UOM != null && chM.UOM != "")
                                    //                strValue += tmpstr + " " + chM.UOM + sq.Separator;
                                    //            else strValue += tmpstr + sq.Separator;
                                    //        }
                                    //        else
                                    //        {
                                    //            if (chM.UOM != null && chM.UOM != "")
                                    //                strValue += tmpstr + chM.UOM + sq.Separator;
                                    //            else strValue += tmpstr + sq.Separator;
                                    //        }
                                    //        frmMdl.values = strValue;
                                    //    }
                                    //    else
                                    //    {
                                    //        string tmpstr = "";
                                    //        if (chM.Value.Contains(' '))
                                    //        {
                                    //            //fore space split

                                    //            var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                    //            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                    //                tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
                                    //            else
                                    //            {
                                    //                string[] spaceSpt = chM.Value.Split(' ');
                                    //                foreach (string spceStr in spaceSpt)
                                    //                {
                                    //                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(spceStr, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                    //                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                    //                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
                                    //                    else
                                    //                        tmpstr += spceStr.TrimStart().TrimEnd() + ' ';

                                    //                }
                                    //                tmpstr = tmpstr.TrimEnd(' ');
                                    //            }
                                    //            tmpstr = tmpstr.Trim(',');
                                    //            if (UOMSet.Short_space == "with space")
                                    //            {
                                    //                if (chM.UOM != null && chM.UOM != "")
                                    //                    strValue += tmpstr.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
                                    //                else strValue += tmpstr.TrimStart().TrimEnd() + sq.Separator;
                                    //            }
                                    //            else
                                    //            {
                                    //                if (chM.UOM != null && chM.UOM != "")
                                    //                    strValue += tmpstr.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
                                    //                else strValue += tmpstr.TrimStart().TrimEnd() + sq.Separator;
                                    //            }
                                    //            frmMdl.values = strValue;

                                    //        }
                                    //        else
                                    //        {
                                    //            var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
                                    //            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
                                    //            {
                                    //                // Abbreivated
                                    //                if (UOMSet.Short_space == "with space")
                                    //                {
                                    //                    if (chM.UOM != null && chM.UOM != "")
                                    //                        strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
                                    //                    else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
                                    //                }
                                    //                else
                                    //                {
                                    //                    if (chM.UOM != null && chM.UOM != "")
                                    //                        strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
                                    //                    else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
                                    //                }
                                    //                frmMdl.values = strValue;
                                    //            }
                                    //            else
                                    //            {
                                    //                // Abbreivated not exist

                                    //                if (UOMSet.Short_space == "with space")
                                    //                {
                                    //                    if (chM.UOM != null && chM.UOM != "")
                                    //                        strValue += chM.Value.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
                                    //                    else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
                                    //                }
                                    //                else
                                    //                {
                                    //                    if (chM.UOM != null && chM.UOM != "")
                                    //                        strValue += chM.Value.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
                                    //                    else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
                                    //                }
                                    //                frmMdl.values = strValue;
                                    //            }
                                    //        }

                                    //    }

                                    //    lst.Add(frmMdl);

                                    //    ShortStr = strNM;
                                    //    string pattern = " X ";
                                    //    foreach (shortFrame sMdl in lst)
                                    //    {
                                    //        string[] strtmp = { " X " };
                                    //        var x = sMdl.values.Split(strtmp, StringSplitOptions.RemoveEmptyEntries);
                                    //        if (Regex.IsMatch(x[0], "^[0-9]+$", RegexOptions.Compiled))
                                    //        {
                                    //            foreach (Match match in Regex.Matches(sMdl.values, pattern))
                                    //            {
                                    //                sMdl.values = Regex.Replace(sMdl.values, pattern, match.Value.TrimEnd().TrimStart());
                                    //            }
                                    //            // ShortStr += sMdl.values;
                                    //            string pattern1 = " x ";
                                    //            foreach (Match match in Regex.Matches(sMdl.values, pattern1))
                                    //            {
                                    //                sMdl.values = Regex.Replace(sMdl.values, pattern1, match.Value.TrimEnd().TrimStart());
                                    //            }
                                    //        }
                                    //        ShortStr += sMdl.values;
                                    //    }

                                    //    if (!checkLength(ShortStr, seqList[0].ShortLength))
                                    //    {
                                    //        ShortStr = ShortStr.Trim();
                                    //        char[] chr = sq.Separator.ToCharArray();
                                    //        ShortStr = ShortStr.TrimEnd(chr[0]);
                                    //        while (ShortStr.Length > seqList[0].ShortLength)
                                    //        {
                                    //            int lstIndx = ShortStr.LastIndexOf(chr[0]);
                                    //            if (lstIndx > -1)
                                    //            {
                                    //                if (lst.Count > 0)
                                    //                {

                                    //                    if (ShortStr.Substring(lstIndx).Length > 1)
                                    //                    {
                                    //                        if (ShortStr.Substring(lstIndx).TrimStart(chr[0]) == lst[lst.Count - 1].values.TrimEnd(chr[0]))
                                    //                        {
                                    //                            lst.RemoveAt(lst.Count - 1);
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            int indx = lst[lst.Count - 1].values.TrimEnd(chr[0]).LastIndexOf(chr[0]);
                                    //                            lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx) + chr[0];
                                    //                        }
                                    //                    }
                                    //                }
                                    //                ShortStr = ShortStr.Remove(lstIndx);

                                    //            }
                                    //            else
                                    //            {
                                    //                lstIndx = ShortStr.LastIndexOf(' ');
                                    //                ShortStr = ShortStr.Remove(lstIndx);
                                    //                if (lst.Count > 0)
                                    //                {
                                    //                    int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
                                    //                    lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
                                    //                }
                                    //            }

                                    //        }
                                    //        ShortStr = ShortStr + chr[0];
                                    //    }
                                    //    i++;
                                    //}

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
                                        //frmMdl.values = prefix + vs.RefNo.Trim() + sq.Separator;
                                        frmMdl.values = vs.RefNo.Trim() + sq.Separator;
                                        lst.Add(frmMdl);
                                        // ShortStr = strNM;

                                        //ShortStr += prefix+vs.RefNo.Trim() + sq.Separator;
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
                                    ShortStr += sq.Separator;
                                    break;
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
                ShortStr = ShortStr.Trim();
                int lsIndx = ShortStr.Length;
                string str = ShortStr.Substring(lsIndx - 1, 1);
                if (str == seqList[0].Separator.Trim())
                {
                    ShortStr = ShortStr.Remove(lsIndx - 1);
                }
            }
            return ShortStr;


        }

        //public string ShortDesc(Prosol_Datamaster cat)
        //{

        //    string mfrref = "";

        //    var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
        //    var NMList = _nounModifierRepository.FindOne(FormattedQuery);

        //    var sort = SortBy.Ascending("Seq").Ascending("Description");

        //    var query = (NMList != null && NMList.Formatted == 1) ? Query.EQ("Description", "Short_Generic") : Query.EQ("Description", "Short_OEM");
        //    string ShortStr = "", strNM = "";

        //    var seqList = _SequenceRepository.FindAll(query, sort).ToList();
        //    var UOMSet = _UOMRepository.FindOne();
        //    seqList[0].ShortLength = 38;

        //    var AbbrList = _abbreivateRepository.FindAll();
        //    List<shortFrame> lst = new List<shortFrame>();
        //    foreach (Prosol_Sequence sq in seqList)
        //    {
        //        if (sq.Required == "Yes")
        //        {
        //            switch (sq.CatId)
        //            {
        //                case 101:
        //                    string nmStr = "";
        //                    //if (NMList != null && !string.IsNullOrEmpty(NMList.Nounabv))
        //                    //    ShortStr += NMList.Nounabv + sq.Separator;
        //                    //else ShortStr += cat.Noun + sq.Separator;


        //                    if (cat.Noun != "OEM" && cat.Noun != "PARTS")
        //                    {
        //                        if (cat.Modifier != "--" && cat.Modifier != "NO MODIFIER")
        //                            ShortStr += cat.Noun + sq.Separator + cat.Modifier + sq.Separator;
        //                        else
        //                            ShortStr += cat.Noun + sq.Separator;
        //                    }
        //                    else
        //                    {
        //                        ShortStr += "";
        //                    }

        //                    //if (cat.Modifier != "--" && cat.Modifier != "NO MODIFIER")
        //                    //{
        //                    //    if (NMList != null && !string.IsNullOrEmpty(NMList.Modifierabv))
        //                    //        ShortStr += NMList.Modifierabv + sq.Separator;
        //                    //    else
        //                    //        ShortStr += cat.Modifier + sq.Separator;
        //                    //}

        //                    strNM = ShortStr;

        //                    break;



        //                case 103:
        //                    int flg = 0;
        //                    int i = 0;
        //                    if (cat.Characteristics != null)
        //                    {
        //                        var dumbCharacteristics = new List<Prosol_AttributeList>();
        //                        foreach (var cha in cat.Characteristics)
        //                        {
        //                            var dumbCha = new Prosol_AttributeList
        //                            {
        //                                Characteristic = cha.Characteristic,
        //                                Value = cha.Value,
        //                                UOM = cha.UOM,
        //                                ShortSquence = cha.ShortSquence,
        //                                Source = cha.Source,
        //                                SourceUrl = cha.SourceUrl,
        //                                Squence = cha.Squence,
        //                            };
        //                            dumbCharacteristics.Add(dumbCha);
        //                        }

        //                        bool containsAdditionalInformation = cat.Characteristics
        //                                                                .GroupBy(ix => ix.Characteristic)
        //                                                                .Any(g => g.Key == "ADDITIONAL INFORMATION");
        //                        if (containsAdditionalInformation)
        //                        {
        //                            foreach (var ch in cat.Characteristics)
        //                            {
        //                                if (ch.Characteristic == "ADDITIONAL INFORMATION")
        //                                {
        //                                    if (!string.IsNullOrEmpty(ch.Value))
        //                                    {
        //                                        var Value = ch.Value;
        //                                        string pattern = @"(\w+(\s\w+)*):";
        //                                        MatchCollection matches = Regex.Matches(ch.Value, pattern);
        //                                        List<string> result = new List<string>();
        //                                        foreach (Match match in matches)
        //                                        {
        //                                            result.Add(match.Groups[1].Value);
        //                                        }

        //                                        //var qry = Query.EQ("Characteristic", "MM");
        //                                        //var charLst = _CharacteristicRepository.FindAll(qry).ToList();
        //                                        //var unqCharLst = charLst.GroupBy(p => p.Characteristic).Select(g => g.First()).ToList();
        //                                        foreach (var unq in result)
        //                                        {
        //                                            var qry = Query.EQ("Characteristic", unq);
        //                                            var charLst = _CharacteristicRepository.FindOne(qry);
        //                                            if (charLst != null)
        //                                            {
        //                                                if (Value.Contains(charLst.Characteristic))
        //                                                {
        //                                                    if (charLst.Abbrivation.Contains("_"))
        //                                                        charLst.Abbrivation = charLst.Abbrivation.Replace("_", " ");
        //                                                    Value = Value.Replace(charLst.Characteristic, charLst.Abbrivation);
        //                                                }
        //                                            }
        //                                        }
        //                                        cat.Additionalinfo = Value;
        //                                    }
        //                                }
        //                            }
        //                            //cat.Characteristics.RemoveAll(c => c.Characteristic == "ADDITIONAL INFORMATION");
        //                        }


        //                        foreach (var chM in dumbCharacteristics.OrderBy(x => x.ShortSquence))
        //                        {
        //                            if (NMList.Formatted == 1 || flg == 1 || NMList.Formatted == 2)//&& NMList.Formatted != 0
        //                            {

        //                                if (chM.Value != null && chM.Value != "" && chM.Value != "--" && chM.Characteristic != "ADDITIONAL INFORMATION")
        //                                {
        //                                    string strValue = "";
        //                                    var frmMdl = new shortFrame();
        //                                    frmMdl.position = chM.Squence;


        //                                    if (chM.Value.Contains(','))
        //                                    {
        //                                        string tmpstr = "";
        //                                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                        {
        //                                            tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                        }
        //                                        else
        //                                        {
        //                                            string[] strsplt = chM.Value.Split(',');
        //                                            foreach (string str in strsplt)
        //                                            {


        //                                                abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                                if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                                    tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                                else tmpstr += str.TrimStart().TrimEnd() + ',';

        //                                            }
        //                                        }
        //                                        tmpstr = tmpstr.TrimEnd(',');
        //                                        if (UOMSet.Short_space == "with space")
        //                                        {
        //                                            if (chM.UOM != null && chM.UOM != "")
        //                                                strValue += tmpstr + " " + chM.UOM + sq.Separator;
        //                                            else strValue += tmpstr + sq.Separator;
        //                                        }
        //                                        else
        //                                        {
        //                                            if (chM.UOM != null && chM.UOM != "")
        //                                                strValue += tmpstr + chM.UOM + sq.Separator;
        //                                            else strValue += tmpstr + sq.Separator;
        //                                        }
        //                                        frmMdl.values = strValue;
        //                                    }
        //                                    else
        //                                    {
        //                                        string tmpstr = "";

        //                                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                        {
        //                                            // Abbreivated
        //                                            if (UOMSet.Short_space == "with space")
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                                else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            else
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                                else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            frmMdl.values = strValue;
        //                                        }
        //                                        else
        //                                        {
        //                                            // Abbreivated not exist

        //                                            if (UOMSet.Short_space == "with space")
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += chM.Value.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                                else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            else
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += chM.Value.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                                else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            frmMdl.values = strValue;
        //                                        }


        //                                    }

        //                                    lst.Add(frmMdl);

        //                                    ShortStr = strNM;
        //                                    string pattern = " X ";
        //                                    foreach (shortFrame sMdl in lst)
        //                                    {
        //                                        string[] strtmp = { " X " };
        //                                        var x = sMdl.values.Split(strtmp, StringSplitOptions.RemoveEmptyEntries);
        //                                        if (Regex.IsMatch(x[0], "^[0-9]+$", RegexOptions.Compiled))
        //                                        {
        //                                            foreach (Match match in Regex.Matches(sMdl.values, pattern))
        //                                            {
        //                                                sMdl.values = Regex.Replace(sMdl.values, pattern, match.Value.TrimEnd().TrimStart());
        //                                            }
        //                                            // ShortStr += sMdl.values;
        //                                            string pattern1 = " x ";
        //                                            foreach (Match match in Regex.Matches(sMdl.values, pattern1))
        //                                            {
        //                                                sMdl.values = Regex.Replace(sMdl.values, pattern1, match.Value.TrimEnd().TrimStart());
        //                                            }
        //                                        }
        //                                        ShortStr += sMdl.values;
        //                                    }

        //                                    if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                    {
        //                                        ShortStr = ShortStr.Trim();
        //                                        char[] chr = sq.Separator.ToCharArray();
        //                                        ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                        while (ShortStr.Length > seqList[0].ShortLength)
        //                                        {
        //                                            int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                            if (lstIndx > -1)
        //                                            {
        //                                                if (lst.Count > 0)
        //                                                {

        //                                                    if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                    {
        //                                                        if (ShortStr.Substring(lstIndx).TrimStart(chr[0]) == lst[lst.Count - 1].values.TrimEnd(chr[0]))
        //                                                        {
        //                                                            lst.RemoveAt(lst.Count - 1);
        //                                                        }
        //                                                        else
        //                                                        {
        //                                                            int indx = lst[lst.Count - 1].values.TrimEnd(chr[0]).LastIndexOf(chr[0]);
        //                                                            lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx) + chr[0];
        //                                                        }
        //                                                    }
        //                                                }
        //                                                ShortStr = ShortStr.Remove(lstIndx);

        //                                            }
        //                                            else
        //                                            {
        //                                                lstIndx = ShortStr.LastIndexOf(' ');
        //                                                ShortStr = ShortStr.Remove(lstIndx);
        //                                                if (lst.Count > 0)
        //                                                {
        //                                                    int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                    lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                                }
        //                                            }

        //                                        }
        //                                        ShortStr = ShortStr + chr[0];
        //                                    }
        //                                    i++;
        //                                }
        //                            }
        //                            else flg = 1;

        //                            //if (chM.Value != null && chM.Value != "")
        //                            //{
        //                            //    string strValue = "";
        //                            //    var frmMdl = new shortFrame();
        //                            //    frmMdl.position = chM.Squence;


        //                            //    if (chM.Value.Contains(','))
        //                            //    {
        //                            //        string tmpstr = "";
        //                            //        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            //        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                            //        {
        //                            //            tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                            //        }
        //                            //        else
        //                            //        {
        //                            //            string[] strsplt = chM.Value.Split(',');
        //                            //            foreach (string str in strsplt)
        //                            //            {
        //                            //                //for space split
        //                            //                if (str.Contains(' '))
        //                            //                {
        //                            //                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            //                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                            //                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                            //                    else
        //                            //                    {
        //                            //                        string[] spaceSpt = str.Split(' ');
        //                            //                        foreach (string spceStr in spaceSpt)
        //                            //                        {
        //                            //                            abbObj = (from Abb in AbbrList where Abb.Value.Equals(spceStr, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            //                            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                            //                                tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
        //                            //                            else
        //                            //                                tmpstr += spceStr.TrimStart().TrimEnd() + ' ';

        //                            //                        }
        //                            //                        tmpstr = tmpstr.TrimEnd(' ');
        //                            //                    }


        //                            //                }
        //                            //                else
        //                            //                {

        //                            //                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            //                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                            //                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                            //                    else tmpstr += str.TrimStart().TrimEnd() + ',';
        //                            //                }
        //                            //            }
        //                            //        }
        //                            //        tmpstr = tmpstr.TrimEnd(',');
        //                            //        if (UOMSet.Short_space == "with space")
        //                            //        {
        //                            //            if (chM.UOM != null && chM.UOM != "")
        //                            //                strValue += tmpstr + " " + chM.UOM + sq.Separator;
        //                            //            else strValue += tmpstr + sq.Separator;
        //                            //        }
        //                            //        else
        //                            //        {
        //                            //            if (chM.UOM != null && chM.UOM != "")
        //                            //                strValue += tmpstr + chM.UOM + sq.Separator;
        //                            //            else strValue += tmpstr + sq.Separator;
        //                            //        }
        //                            //        frmMdl.values = strValue;
        //                            //    }
        //                            //    else
        //                            //    {
        //                            //        string tmpstr = "";
        //                            //        if (chM.Value.Contains(' '))
        //                            //        {
        //                            //            //fore space split

        //                            //            var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            //            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                            //                tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                            //            else
        //                            //            {
        //                            //                string[] spaceSpt = chM.Value.Split(' ');
        //                            //                foreach (string spceStr in spaceSpt)
        //                            //                {
        //                            //                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(spceStr, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            //                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                            //                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
        //                            //                    else
        //                            //                        tmpstr += spceStr.TrimStart().TrimEnd() + ' ';

        //                            //                }
        //                            //                tmpstr = tmpstr.TrimEnd(' ');
        //                            //            }
        //                            //            tmpstr = tmpstr.Trim(',');
        //                            //            if (UOMSet.Short_space == "with space")
        //                            //            {
        //                            //                if (chM.UOM != null && chM.UOM != "")
        //                            //                    strValue += tmpstr.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                            //                else strValue += tmpstr.TrimStart().TrimEnd() + sq.Separator;
        //                            //            }
        //                            //            else
        //                            //            {
        //                            //                if (chM.UOM != null && chM.UOM != "")
        //                            //                    strValue += tmpstr.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                            //                else strValue += tmpstr.TrimStart().TrimEnd() + sq.Separator;
        //                            //            }
        //                            //            frmMdl.values = strValue;

        //                            //        }
        //                            //        else
        //                            //        {
        //                            //            var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            //            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                            //            {
        //                            //                // Abbreivated
        //                            //                if (UOMSet.Short_space == "with space")
        //                            //                {
        //                            //                    if (chM.UOM != null && chM.UOM != "")
        //                            //                        strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                            //                    else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                            //                }
        //                            //                else
        //                            //                {
        //                            //                    if (chM.UOM != null && chM.UOM != "")
        //                            //                        strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                            //                    else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                            //                }
        //                            //                frmMdl.values = strValue;
        //                            //            }
        //                            //            else
        //                            //            {
        //                            //                // Abbreivated not exist

        //                            //                if (UOMSet.Short_space == "with space")
        //                            //                {
        //                            //                    if (chM.UOM != null && chM.UOM != "")
        //                            //                        strValue += chM.Value.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                            //                    else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                            //                }
        //                            //                else
        //                            //                {
        //                            //                    if (chM.UOM != null && chM.UOM != "")
        //                            //                        strValue += chM.Value.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                            //                    else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                            //                }
        //                            //                frmMdl.values = strValue;
        //                            //            }
        //                            //        }

        //                            //    }

        //                            //    lst.Add(frmMdl);

        //                            //    ShortStr = strNM;
        //                            //    string pattern = " X ";
        //                            //    foreach (shortFrame sMdl in lst)
        //                            //    {
        //                            //        string[] strtmp = { " X " };
        //                            //        var x = sMdl.values.Split(strtmp, StringSplitOptions.RemoveEmptyEntries);
        //                            //        if (Regex.IsMatch(x[0], "^[0-9]+$", RegexOptions.Compiled))
        //                            //        {
        //                            //            foreach (Match match in Regex.Matches(sMdl.values, pattern))
        //                            //            {
        //                            //                sMdl.values = Regex.Replace(sMdl.values, pattern, match.Value.TrimEnd().TrimStart());
        //                            //            }
        //                            //            // ShortStr += sMdl.values;
        //                            //            string pattern1 = " x ";
        //                            //            foreach (Match match in Regex.Matches(sMdl.values, pattern1))
        //                            //            {
        //                            //                sMdl.values = Regex.Replace(sMdl.values, pattern1, match.Value.TrimEnd().TrimStart());
        //                            //            }
        //                            //        }
        //                            //        ShortStr += sMdl.values;
        //                            //    }

        //                            //    if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                            //    {
        //                            //        ShortStr = ShortStr.Trim();
        //                            //        char[] chr = sq.Separator.ToCharArray();
        //                            //        ShortStr = ShortStr.TrimEnd(chr[0]);
        //                            //        while (ShortStr.Length > seqList[0].ShortLength)
        //                            //        {
        //                            //            int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                            //            if (lstIndx > -1)
        //                            //            {
        //                            //                if (lst.Count > 0)
        //                            //                {

        //                            //                    if (ShortStr.Substring(lstIndx).Length > 1)
        //                            //                    {
        //                            //                        if (ShortStr.Substring(lstIndx).TrimStart(chr[0]) == lst[lst.Count - 1].values.TrimEnd(chr[0]))
        //                            //                        {
        //                            //                            lst.RemoveAt(lst.Count - 1);
        //                            //                        }
        //                            //                        else
        //                            //                        {
        //                            //                            int indx = lst[lst.Count - 1].values.TrimEnd(chr[0]).LastIndexOf(chr[0]);
        //                            //                            lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx) + chr[0];
        //                            //                        }
        //                            //                    }
        //                            //                }
        //                            //                ShortStr = ShortStr.Remove(lstIndx);

        //                            //            }
        //                            //            else
        //                            //            {
        //                            //                lstIndx = ShortStr.LastIndexOf(' ');
        //                            //                ShortStr = ShortStr.Remove(lstIndx);
        //                            //                if (lst.Count > 0)
        //                            //                {
        //                            //                    int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                            //                    lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                            //                }
        //                            //            }

        //                            //        }
        //                            //        ShortStr = ShortStr + chr[0];
        //                            //    }
        //                            //    i++;
        //                            //}

        //                        }
        //                        ShortStr = strNM;
        //                        foreach (shortFrame sMdl in lst.OrderBy(x => x.position))
        //                        {
        //                            ShortStr += sMdl.values;
        //                        }

        //                    }

        //                    break;


        //                case 104:
        //                    if (cat.Vendorsuppliers != null)
        //                    {
        //                        foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                        {
        //                            if (vs.s == 1)
        //                            {
        //                                if (vs.Name != null && vs.Name != "")
        //                                {
        //                                    var querry = Query.EQ("Name", vs.Name);
        //                                    var shtmfr = _VendorRepository.FindOne(querry);
        //                                    if (shtmfr != null)
        //                                    {
        //                                        if (shtmfr.ShortDescName != null && shtmfr.ShortDescName != "")
        //                                        {
        //                                            vs.shortmfr = shtmfr.ShortDescName;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        vs.shortmfr = vs.Name;
        //                                    }
        //                                }
        //                                //else
        //                                //{
        //                                //    vs.shortmfr = vs.Name;
        //                                //}

        //                                if (vs.shortmfr != null && vs.shortmfr != "" && vs.shortmfr != "undefined")
        //                                {
        //                                    mfrref = vs.Name;
        //                                    var frmMdl = new shortFrame();
        //                                    frmMdl.position = 101;
        //                                    frmMdl.values = vs.shortmfr + sq.Separator;
        //                                    lst.Add(frmMdl);
        //                                    ShortStr += vs.shortmfr + sq.Separator;
        //                                    if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                    {
        //                                        ShortStr = ShortStr.Trim();
        //                                        char[] chr = sq.Separator.ToCharArray();
        //                                        ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                        while (ShortStr.Length > seqList[0].ShortLength)
        //                                        {
        //                                            int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                            if (lstIndx > -1)
        //                                            {
        //                                                if (lst.Count > 0)
        //                                                {
        //                                                    if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                        lst.RemoveAt(lst.Count - 1);
        //                                                }
        //                                                ShortStr = ShortStr.Remove(lstIndx);

        //                                            }
        //                                            else
        //                                            {
        //                                                lstIndx = ShortStr.LastIndexOf(' ');
        //                                                ShortStr = ShortStr.Remove(lstIndx);
        //                                                if (lst.Count > 0)
        //                                                {
        //                                                    int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                    lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                                }
        //                                            }
        //                                        }
        //                                        ShortStr = ShortStr + chr[0];
        //                                    }
        //                                    break;
        //                                }

        //                            }
        //                        }
        //                    }
        //                    break;
        //                case 105:
        //                    if (cat.Vendorsuppliers != null)
        //                    {
        //                        foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                        {
        //                            if (vs.s == 1 && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined")
        //                            {
        //                                string prefix = "";
        //                                if (vs.Refflag != null)
        //                                {
        //                                    var flag = Query.EQ("Type", vs.Refflag);
        //                                    var code = _ReftypeRepository.FindOne(flag);
        //                                    if (code != null)
        //                                    {
        //                                        prefix = code.Code + ":";
        //                                    }
        //                                }
        //                                // ShortStr = strNM;
        //                                if (vs.Refflag == "DRAWING NUMBER")
        //                                {
        //                                    ShortStr += prefix + vs.RefNo.Trim() + sq.Separator;
        //                                    var frmMdl = new shortFrame();
        //                                    frmMdl.position = 100;
        //                                    frmMdl.values = prefix + vs.RefNo.Trim() + sq.Separator;
        //                                    lst.Add(frmMdl);
        //                                }
        //                                else
        //                                {
        //                                    ShortStr += vs.RefNo.Trim() + sq.Separator;
        //                                    var frmMdl = new shortFrame();
        //                                    frmMdl.position = 100;
        //                                    frmMdl.values = vs.RefNo.Trim() + sq.Separator;
        //                                    lst.Add(frmMdl);
        //                                }
        //                                if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                {
        //                                    ShortStr = ShortStr.Trim();
        //                                    char[] chr = sq.Separator.ToCharArray();
        //                                    ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                    while (ShortStr.Length > seqList[0].ShortLength)
        //                                    {
        //                                        int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                        if (lstIndx > -1)
        //                                        {
        //                                            if (lst.Count > 0)
        //                                            {
        //                                                if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                    lst.RemoveAt(lst.Count - 1);
        //                                            }
        //                                            ShortStr = ShortStr.Remove(lstIndx);

        //                                        }
        //                                        else
        //                                        {
        //                                            lstIndx = ShortStr.LastIndexOf(' ');
        //                                            ShortStr = ShortStr.Remove(lstIndx);
        //                                            if (lst.Count > 0)
        //                                            {
        //                                                int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                            }
        //                                        }

        //                                    }
        //                                    ShortStr = ShortStr + chr[0];
        //                                }
        //                                break;
        //                            }

        //                        }
        //                    }

        //                    break;
        //                case 106:
        //                    if (cat.Application != null && cat.Application != "")
        //                        ShortStr += cat.Application + sq.Separator;
        //                    break;
        //                case 107:
        //                    //if (cat.Drawingno != null && cat.Drawingno != "")
        //                    //    ShortStr += cat.Drawingno + sq.Separator;


        //                    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    {
        //                        if (vs.Refflag.ToUpper() == "DRAWING & POSITION NUMBER" && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined" && vs.Name == mfrref && vs.Name != "" && vs.Name != null)
        //                        {
        //                            ShortStr += vs.RefNo.Trim() + sq.Separator;
        //                            break;
        //                        }
        //                    }
        //                    break;
        //                //case 108:

        //                //    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "" && NMList.Formatted != 0)
        //                //    {
        //                //        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Equipment.Name, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                //        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                //        {
        //                //            ShortStr += "F/" + abbObj.Abbrevated + sq.Separator;
        //                //        }
        //                //        else
        //                //        {
        //                //            if (cat.Equipment.Name.Contains(' '))
        //                //            {
        //                //                string tmpstr = "";

        //                //                string[] strsplt = cat.Equipment.Name.Split(' ');
        //                //                foreach (string str in strsplt)
        //                //                {
        //                //                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                //                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                //                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
        //                //                    else tmpstr += str.TrimStart().TrimEnd() + ' ';
        //                //                }

        //                //                tmpstr = tmpstr.TrimEnd(' ');
        //                //                ShortStr += "F/" + tmpstr + sq.Separator;
        //                //            }
        //                //            else
        //                //            {
        //                //                ShortStr += "F/" + cat.Equipment.Name + sq.Separator;
        //                //            }

        //                //        }
        //                //        if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                //        {
        //                //            ShortStr = ShortStr.Trim();
        //                //            char[] chr = sq.Separator.ToCharArray();
        //                //            ShortStr = ShortStr.TrimEnd(chr[0]);
        //                //            while (ShortStr.Length > seqList[0].ShortLength)
        //                //            {
        //                //                int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                //                ShortStr = ShortStr.Remove(lstIndx);
        //                //            }
        //                //            ShortStr += sq.Separator;
        //                //            break;
        //                //        }
        //                //    }
        //                //    //ShortStr += "F/" + cat.Equipment.Name + sq.Separator;

        //                //    break;
        //                case 108:

        //                    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "" && cat.Equipment.ENS == 1)
        //                    {
        //                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Equipment.Name, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                        {
        //                            ShortStr += "F/" + abbObj.Abbrevated + sq.Separator;
        //                        }
        //                        else
        //                            ShortStr += "F/" + cat.Equipment.Name + sq.Separator;
        //                    }
        //                    //ShortStr += "F/" + cat.Equipment.Name + sq.Separator;

        //                    break;
        //                case 109:
        //                    if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "" && NMList.Formatted != 0)
        //                        ShortStr += cat.Equipment.Manufacturer + sq.Separator;
        //                    break;
        //                case 110:
        //                    if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "" && NMList.Formatted != 0)
        //                        ShortStr += cat.Equipment.Modelno + sq.Separator;
        //                    break;
        //                case 111:
        //                    if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "" && NMList.Formatted != 0)
        //                        ShortStr += cat.Equipment.Tagno + sq.Separator;
        //                    break;
        //                case 112:
        //                    if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "" && NMList.Formatted != 0)
        //                        ShortStr += cat.Equipment.Serialno + sq.Separator;
        //                    break;
        //                //case 108:
        //                //    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "")
        //                //        ShortStr += cat.Equipment.Name + sq.Separator;
        //                //    break;
        //                //case 109:
        //                //    if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
        //                //        ShortStr += cat.Equipment.Manufacturer + sq.Separator;
        //                //    break;
        //                //case 110:
        //                //    if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "")
        //                //        ShortStr += cat.Equipment.Modelno + sq.Separator;
        //                //    break;
        //                //case 111:
        //                //    if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "")
        //                //        ShortStr += cat.Equipment.Tagno + sq.Separator;
        //                //    break;
        //                //case 112:
        //                //    if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "")
        //                //        ShortStr += cat.Equipment.Serialno + sq.Separator;
        //                //    break;
        //                case 113:
        //                    if (cat.Referenceno != null && cat.Referenceno != "" && NMList.Formatted != 0)
        //                        ShortStr += cat.Referenceno + sq.Separator;
        //                    break;
        //                case 114:
        //                    var charQry1 = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier), Query.EQ("Characteristic", "ADDITIONAL INFORMATION"));
        //                    var charLst1 = _CharacteristicRepository.FindOne(charQry1);
        //                    if (cat.Additionalinfo != null && cat.Additionalinfo != "")
        //                        ShortStr += charLst1.Abbrivation.Replace("_", " ") + ":" + cat.Additionalinfo + sq.Separator;
        //                    break;
        //                case 115:
        //                    if (cat.Equipment != null && cat.Equipment.Additionalinfo != null && cat.Equipment.Additionalinfo != "" && NMList.Formatted != 0)
        //                        ShortStr += cat.Equipment.Additionalinfo + sq.Separator;
        //                    break;


        //            }
        //            if (!checkLength(ShortStr, seqList[0].ShortLength))
        //            {
        //                ShortStr = ShortStr.Trim();
        //                char[] chr = sq.Separator.ToCharArray();
        //                ShortStr = ShortStr.TrimEnd(chr[0]);
        //                while (ShortStr.Length > seqList[0].ShortLength)
        //                {
        //                    int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                    ShortStr = ShortStr.Remove(lstIndx);
        //                }
        //                break;
        //            }
        //        }
        //    }
        //    seqList[0].ShortLength = 38;
        //    if (ShortStr.Length != seqList[0].ShortLength)
        //    {
        //        ShortStr = ShortStr.Trim();
        //        int lsIndx = ShortStr.Length;
        //        string str = ShortStr.Substring(lsIndx - 1, 1);
        //        if (str == seqList[0].Separator.Trim())
        //        {
        //            ShortStr = ShortStr.Remove(lsIndx - 1);
        //        }
        //    }

        //    //string[] parts = ShortStr.Split(',');

        //    //parts[1] = $"{parts[1].Replace("MM", "")}X{parts[2]}";
        //    //string result = string.Join(",", parts[0], parts[1], parts[3], parts[4]);

        //    //Console.WriteLine(result);
        //    //ShortStr = result;
        //    return ShortStr;

        //}

        //public string ShortDesc(Prosol_Datamaster cat)
        //{
        //    string mfrref = "";
        //    var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
        //    var NMList = _nounModifierRepository.FindOne(FormattedQuery);

        //    var sort = SortBy.Ascending("Seq").Ascending("Description");

        //    var query = (NMList != null && NMList.Formatted == 1) ? Query.EQ("Description", "Short_Generic") : Query.EQ("Description", "Short_OEM");
        //    string ShortStr = "", strNM = "", case103Str = "", finalStr = "";

        //    var seqList = _SequenceRepository.FindAll(query, sort).ToList();
        //    var UOMSet = _UOMRepository.FindOne();
        //    seqList[0].ShortLength = 38;

        //    var AbbrList = _abbreivateRepository.FindAll();
        //    List<shortFrame> lst = new List<shortFrame>();

        //    foreach (Prosol_Sequence sq in seqList)
        //    {
        //        if (sq.Required == "Yes")
        //        {
        //            switch (sq.CatId)
        //            {
        //                case 101:
        //                    // Process Case 101
        //                    if (cat.Noun != "OEM" && cat.Noun != "PARTS")
        //                    {
        //                        if (cat.Modifier != "--" && cat.Modifier != "NO MODIFIER")
        //                            ShortStr += cat.Noun + sq.Separator + cat.Modifier + sq.Separator;
        //                        else
        //                            ShortStr += cat.Noun + sq.Separator;
        //                    }
        //                    else
        //                    {
        //                        ShortStr += "";
        //                    }
        //                    strNM = ShortStr; // Storing the base for 101
        //                    break;

        //                case 103:
        //                    int flg = 0;
        //                    int i = 0;
        //                    if (cat.Characteristics != null)
        //                    {
        //                        var dumbCharacteristics = new List<Prosol_AttributeList>();
        //                        foreach (var cha in cat.Characteristics)
        //                        {
        //                            var dumbCha = new Prosol_AttributeList
        //                            {
        //                                Characteristic = cha.Characteristic,
        //                                Value = cha.Value,
        //                                UOM = cha.UOM,
        //                                ShortSquence = cha.ShortSquence,
        //                                Source = cha.Source,
        //                                SourceUrl = cha.SourceUrl,
        //                                Squence = cha.Squence,
        //                            };
        //                            dumbCharacteristics.Add(dumbCha);
        //                        }




        //                        foreach (var chM in dumbCharacteristics.OrderBy(x => x.ShortSquence))
        //                        {
        //                            if (NMList.Formatted == 1 || flg == 1 || NMList.Formatted == 2)//&& NMList.Formatted != 0
        //                            {

        //                                if (chM.Value != null && chM.Value != "" && chM.Value != "--")
        //                                {
        //                                    string strValue = "";
        //                                    var frmMdl = new shortFrame();
        //                                    frmMdl.position = chM.Squence;


        //                                    if (chM.Value.Contains(','))
        //                                    {
        //                                        string tmpstr = "";
        //                                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                        {
        //                                            tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                        }
        //                                        else
        //                                        {
        //                                            string[] strsplt = chM.Value.Split(',');
        //                                            foreach (string str in strsplt)
        //                                            {


        //                                                abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                                if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                                    tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                                else tmpstr += str.TrimStart().TrimEnd() + ',';

        //                                            }
        //                                        }
        //                                        tmpstr = tmpstr.TrimEnd(',');
        //                                        if (UOMSet.Short_space == "with space")
        //                                        {
        //                                            if (chM.UOM != null && chM.UOM != "")
        //                                                strValue += tmpstr + " " + chM.UOM + sq.Separator;
        //                                            else strValue += tmpstr + sq.Separator;
        //                                        }
        //                                        else
        //                                        {
        //                                            if (chM.UOM != null && chM.UOM != "")
        //                                                strValue += tmpstr + chM.UOM + sq.Separator;
        //                                            else strValue += tmpstr + sq.Separator;
        //                                        }
        //                                        frmMdl.values = strValue;
        //                                    }
        //                                    else
        //                                    {
        //                                        string tmpstr = "";

        //                                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                        {
        //                                            // Abbreivated
        //                                            if (UOMSet.Short_space == "with space")
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                                else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            else
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                                else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            frmMdl.values = strValue;
        //                                        }
        //                                        else
        //                                        {
        //                                            // Abbreivated not exist

        //                                            if (UOMSet.Short_space == "with space")
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += chM.Value.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                                else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            else
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += chM.Value.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                                else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            frmMdl.values = strValue;
        //                                        }


        //                                    }

        //                                    lst.Add(frmMdl);

        //                                    case103Str = strNM;
        //                                    string pattern = " X ";
        //                                    foreach (shortFrame sMdl in lst)
        //                                    {
        //                                        string[] strtmp = { " X " };
        //                                        var x = sMdl.values.Split(strtmp, StringSplitOptions.RemoveEmptyEntries);
        //                                        if (Regex.IsMatch(x[0], "^[0-9]+$", RegexOptions.Compiled))
        //                                        {
        //                                            foreach (Match match in Regex.Matches(sMdl.values, pattern))
        //                                            {
        //                                                sMdl.values = Regex.Replace(sMdl.values, pattern, match.Value.TrimEnd().TrimStart());
        //                                            }
        //                                            // ShortStr += sMdl.values;
        //                                            string pattern1 = " x ";
        //                                            foreach (Match match in Regex.Matches(sMdl.values, pattern1))
        //                                            {
        //                                                sMdl.values = Regex.Replace(sMdl.values, pattern1, match.Value.TrimEnd().TrimStart());
        //                                            }
        //                                        }
        //                                        case103Str += sMdl.values;
        //                                    }

        //                                    if (!checkLength(case103Str, seqList[0].ShortLength))
        //                                    {
        //                                        case103Str = case103Str.Trim();
        //                                        char[] chr = sq.Separator.ToCharArray();
        //                                        case103Str = case103Str.TrimEnd(chr[0]);
        //                                        while (case103Str.Length > seqList[0].ShortLength)
        //                                        {
        //                                            int lstIndx = case103Str.LastIndexOf(chr[0]);
        //                                            if (lstIndx > -1)
        //                                            {
        //                                                if (lst.Count > 0)
        //                                                {

        //                                                    if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                    {
        //                                                        if (ShortStr.Substring(lstIndx).TrimStart(chr[0]) == lst[lst.Count - 1].values.TrimEnd(chr[0]))
        //                                                        {
        //                                                            lst.RemoveAt(lst.Count - 1);
        //                                                        }
        //                                                        else
        //                                                        {
        //                                                            int indx = lst[lst.Count - 1].values.TrimEnd(chr[0]).LastIndexOf(chr[0]);
        //                                                            lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx) + chr[0];
        //                                                        }
        //                                                    }
        //                                                }
        //                                                case103Str = ShortStr.Remove(lstIndx);

        //                                            }
        //                                            else
        //                                            {
        //                                                lstIndx = ShortStr.LastIndexOf(' ');
        //                                                case103Str = ShortStr.Remove(lstIndx);
        //                                                if (lst.Count > 0)
        //                                                {
        //                                                    int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                    lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                                }
        //                                            }

        //                                        }
        //                                        case103Str = ShortStr + chr[0];
        //                                    }
        //                                    i++;
        //                                }
        //                            }
        //                            else flg = 1;

        //                            //if (chM.Value != null && chM.Value != "")
        //                            //{
        //                            //    string strValue = "";
        //                            //    var frmMdl = new shortFrame();
        //                            //    frmMdl.position = chM.Squence;


        //                            //    if (chM.Value.Contains(','))
        //                            //    {
        //                            //        string tmpstr = "";
        //                            //        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            //        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                            //        {
        //                            //            tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                            //        }
        //                            //        else
        //                            //        {
        //                            //            string[] strsplt = chM.Value.Split(',');
        //                            //            foreach (string str in strsplt)
        //                            //            {
        //                            //                //for space split
        //                            //                if (str.Contains(' '))
        //                            //                {
        //                            //                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            //                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                            //                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                            //                    else
        //                            //                    {
        //                            //                        string[] spaceSpt = str.Split(' ');
        //                            //                        foreach (string spceStr in spaceSpt)
        //                            //                        {
        //                            //                            abbObj = (from Abb in AbbrList where Abb.Value.Equals(spceStr, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            //                            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                            //                                tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
        //                            //                            else
        //                            //                                tmpstr += spceStr.TrimStart().TrimEnd() + ' ';

        //                            //                        }
        //                            //                        tmpstr = tmpstr.TrimEnd(' ');
        //                            //                    }


        //                            //                }
        //                            //                else
        //                            //                {

        //                            //                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            //                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                            //                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                            //                    else tmpstr += str.TrimStart().TrimEnd() + ',';
        //                            //                }
        //                            //            }
        //                            //        }
        //                            //        tmpstr = tmpstr.TrimEnd(',');
        //                            //        if (UOMSet.Short_space == "with space")
        //                            //        {
        //                            //            if (chM.UOM != null && chM.UOM != "")
        //                            //                strValue += tmpstr + " " + chM.UOM + sq.Separator;
        //                            //            else strValue += tmpstr + sq.Separator;
        //                            //        }
        //                            //        else
        //                            //        {
        //                            //            if (chM.UOM != null && chM.UOM != "")
        //                            //                strValue += tmpstr + chM.UOM + sq.Separator;
        //                            //            else strValue += tmpstr + sq.Separator;
        //                            //        }
        //                            //        frmMdl.values = strValue;
        //                            //    }
        //                            //    else
        //                            //    {
        //                            //        string tmpstr = "";
        //                            //        if (chM.Value.Contains(' '))
        //                            //        {
        //                            //            //fore space split

        //                            //            var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            //            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                            //                tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                            //            else
        //                            //            {
        //                            //                string[] spaceSpt = chM.Value.Split(' ');
        //                            //                foreach (string spceStr in spaceSpt)
        //                            //                {
        //                            //                    abbObj = (from Abb in AbbrList where Abb.Value.Equals(spceStr, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            //                    if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                            //                        tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ' ';
        //                            //                    else
        //                            //                        tmpstr += spceStr.TrimStart().TrimEnd() + ' ';

        //                            //                }
        //                            //                tmpstr = tmpstr.TrimEnd(' ');
        //                            //            }
        //                            //            tmpstr = tmpstr.Trim(',');
        //                            //            if (UOMSet.Short_space == "with space")
        //                            //            {
        //                            //                if (chM.UOM != null && chM.UOM != "")
        //                            //                    strValue += tmpstr.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                            //                else strValue += tmpstr.TrimStart().TrimEnd() + sq.Separator;
        //                            //            }
        //                            //            else
        //                            //            {
        //                            //                if (chM.UOM != null && chM.UOM != "")
        //                            //                    strValue += tmpstr.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                            //                else strValue += tmpstr.TrimStart().TrimEnd() + sq.Separator;
        //                            //            }
        //                            //            frmMdl.values = strValue;

        //                            //        }
        //                            //        else
        //                            //        {
        //                            //            var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            //            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                            //            {
        //                            //                // Abbreivated
        //                            //                if (UOMSet.Short_space == "with space")
        //                            //                {
        //                            //                    if (chM.UOM != null && chM.UOM != "")
        //                            //                        strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                            //                    else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                            //                }
        //                            //                else
        //                            //                {
        //                            //                    if (chM.UOM != null && chM.UOM != "")
        //                            //                        strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                            //                    else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                            //                }
        //                            //                frmMdl.values = strValue;
        //                            //            }
        //                            //            else
        //                            //            {
        //                            //                // Abbreivated not exist

        //                            //                if (UOMSet.Short_space == "with space")
        //                            //                {
        //                            //                    if (chM.UOM != null && chM.UOM != "")
        //                            //                        strValue += chM.Value.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                            //                    else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                            //                }
        //                            //                else
        //                            //                {
        //                            //                    if (chM.UOM != null && chM.UOM != "")
        //                            //                        strValue += chM.Value.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                            //                    else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                            //                }
        //                            //                frmMdl.values = strValue;
        //                            //            }
        //                            //        }

        //                            //    }

        //                            //    lst.Add(frmMdl);

        //                            //    ShortStr = strNM;
        //                            //    string pattern = " X ";
        //                            //    foreach (shortFrame sMdl in lst)
        //                            //    {
        //                            //        string[] strtmp = { " X " };
        //                            //        var x = sMdl.values.Split(strtmp, StringSplitOptions.RemoveEmptyEntries);
        //                            //        if (Regex.IsMatch(x[0], "^[0-9]+$", RegexOptions.Compiled))
        //                            //        {
        //                            //            foreach (Match match in Regex.Matches(sMdl.values, pattern))
        //                            //            {
        //                            //                sMdl.values = Regex.Replace(sMdl.values, pattern, match.Value.TrimEnd().TrimStart());
        //                            //            }
        //                            //            // ShortStr += sMdl.values;
        //                            //            string pattern1 = " x ";
        //                            //            foreach (Match match in Regex.Matches(sMdl.values, pattern1))
        //                            //            {
        //                            //                sMdl.values = Regex.Replace(sMdl.values, pattern1, match.Value.TrimEnd().TrimStart());
        //                            //            }
        //                            //        }
        //                            //        ShortStr += sMdl.values;
        //                            //    }

        //                            //    if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                            //    {
        //                            //        ShortStr = ShortStr.Trim();
        //                            //        char[] chr = sq.Separator.ToCharArray();
        //                            //        ShortStr = ShortStr.TrimEnd(chr[0]);
        //                            //        while (ShortStr.Length > seqList[0].ShortLength)
        //                            //        {
        //                            //            int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                            //            if (lstIndx > -1)
        //                            //            {
        //                            //                if (lst.Count > 0)
        //                            //                {

        //                            //                    if (ShortStr.Substring(lstIndx).Length > 1)
        //                            //                    {
        //                            //                        if (ShortStr.Substring(lstIndx).TrimStart(chr[0]) == lst[lst.Count - 1].values.TrimEnd(chr[0]))
        //                            //                        {
        //                            //                            lst.RemoveAt(lst.Count - 1);
        //                            //                        }
        //                            //                        else
        //                            //                        {
        //                            //                            int indx = lst[lst.Count - 1].values.TrimEnd(chr[0]).LastIndexOf(chr[0]);
        //                            //                            lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx) + chr[0];
        //                            //                        }
        //                            //                    }
        //                            //                }
        //                            //                ShortStr = ShortStr.Remove(lstIndx);

        //                            //            }
        //                            //            else
        //                            //            {
        //                            //                lstIndx = ShortStr.LastIndexOf(' ');
        //                            //                ShortStr = ShortStr.Remove(lstIndx);
        //                            //                if (lst.Count > 0)
        //                            //                {
        //                            //                    int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                            //                    lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                            //                }
        //                            //            }

        //                            //        }
        //                            //        ShortStr = ShortStr + chr[0];
        //                            //    }
        //                            //    i++;
        //                            //}

        //                        }
        //                        case103Str = strNM;
        //                        foreach (shortFrame sMdl in lst.OrderBy(x => x.position))
        //                        {
        //                            case103Str += sMdl.values;
        //                        }

        //                    }
        //                    //if (cat.Characteristics != null)
        //                    //{
        //                    //    case103Str = "Your case 103 string logic here";  
        //                    //}
        //                    break;

        //                case 104:
        //                    if (cat.Vendorsuppliers != null)
        //                    {
        //                        foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                        {
        //                            if (vs.s == 1)
        //                            {
        //                                if (vs.Name != null && vs.Name != "")
        //                                {
        //                                    var querry = Query.EQ("Name", vs.Name);
        //                                    var shtmfr = _VendorRepository.FindOne(querry);
        //                                    if (shtmfr != null)
        //                                    {
        //                                        if (shtmfr.ShortDescName != null && shtmfr.ShortDescName != "")
        //                                        {
        //                                            vs.shortmfr = shtmfr.ShortDescName;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        vs.shortmfr = vs.Name;
        //                                    }
        //                                }
        //                                //else
        //                                //{
        //                                //    vs.shortmfr = vs.Name;
        //                                //}

        //                                if (vs.shortmfr != null && vs.shortmfr != "" && vs.shortmfr != "undefined")
        //                                {
        //                                    mfrref = vs.Name;
        //                                    var frmMdl = new shortFrame();
        //                                    frmMdl.position = 101;
        //                                    frmMdl.values = vs.shortmfr + sq.Separator;
        //                                    lst.Add(frmMdl);
        //                                    ShortStr += vs.shortmfr + sq.Separator;
        //                                    if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                    {
        //                                        ShortStr = ShortStr.Trim();
        //                                        char[] chr = sq.Separator.ToCharArray();
        //                                        ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                        while (ShortStr.Length > seqList[0].ShortLength)
        //                                        {
        //                                            int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                            if (lstIndx > -1)
        //                                            {
        //                                                if (lst.Count > 0)
        //                                                {
        //                                                    if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                        lst.RemoveAt(lst.Count - 1);
        //                                                }
        //                                                ShortStr = ShortStr.Remove(lstIndx);

        //                                            }
        //                                            else
        //                                            {
        //                                                lstIndx = ShortStr.LastIndexOf(' ');
        //                                                ShortStr = ShortStr.Remove(lstIndx);
        //                                                if (lst.Count > 0)
        //                                                {
        //                                                    int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                    lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                                }
        //                                            }
        //                                        }
        //                                        ShortStr = ShortStr + chr[0];
        //                                    }
        //                                    break;
        //                                }

        //                            }
        //                        }
        //                    }
        //                    break;
        //                case 105:
        //                    if (cat.Vendorsuppliers != null)
        //                    {
        //                        foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                        {
        //                            if (vs.s == 1 && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined")
        //                            {
        //                                string prefix = "";
        //                                if (vs.Refflag != null)
        //                                {
        //                                    var flag = Query.EQ("Type", vs.Refflag);
        //                                    var code = _ReftypeRepository.FindOne(flag);
        //                                    if (code != null)
        //                                    {
        //                                        prefix = code.Code + ":";
        //                                    }
        //                                }
        //                                // ShortStr = strNM;
        //                                if (vs.Refflag == "DRAWING NUMBER")
        //                                {
        //                                    ShortStr += prefix + vs.RefNo.Trim() + sq.Separator;
        //                                    var frmMdl = new shortFrame();
        //                                    frmMdl.position = 100;
        //                                    frmMdl.values = prefix + vs.RefNo.Trim() + sq.Separator;
        //                                    lst.Add(frmMdl);
        //                                }
        //                                else
        //                                {
        //                                    ShortStr += vs.RefNo.Trim() + sq.Separator;
        //                                    var frmMdl = new shortFrame();
        //                                    frmMdl.position = 100;
        //                                    frmMdl.values = vs.RefNo.Trim() + sq.Separator;
        //                                    lst.Add(frmMdl);
        //                                }
        //                                if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                {
        //                                    ShortStr = ShortStr.Trim();
        //                                    char[] chr = sq.Separator.ToCharArray();
        //                                    ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                    while (ShortStr.Length > seqList[0].ShortLength)
        //                                    {
        //                                        int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                        if (lstIndx > -1)
        //                                        {
        //                                            if (lst.Count > 0)
        //                                            {
        //                                                if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                    lst.RemoveAt(lst.Count - 1);
        //                                            }
        //                                            ShortStr = ShortStr.Remove(lstIndx);

        //                                        }
        //                                        else
        //                                        {
        //                                            lstIndx = ShortStr.LastIndexOf(' ');
        //                                            ShortStr = ShortStr.Remove(lstIndx);
        //                                            if (lst.Count > 0)
        //                                            {
        //                                                int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                            }
        //                                        }

        //                                    }
        //                                    ShortStr = ShortStr + chr[0];
        //                                }
        //                                break;
        //                            }

        //                        }
        //                    }

        //                    break;
        //            }

        //            // Check if ShortStr exceeds the length, trim if necessary
        //            if (!checkLength(ShortStr, seqList[0].ShortLength))
        //            {
        //                ShortStr = ShortStr.Trim();
        //                char[] chr = sq.Separator.ToCharArray();
        //                ShortStr = ShortStr.TrimEnd(chr[0]);
        //                while (ShortStr.Length > seqList[0].ShortLength)
        //                {
        //                    int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                    ShortStr = ShortStr.Remove(lstIndx);
        //                }
        //                break;
        //            }
        //        }
        //    }

        //    // Append Case 103 before Case 104/105 if there's space
        //    if (!string.IsNullOrEmpty(case103Str))
        //    {
        //        int remainingLength = seqList[0].ShortLength - ShortStr.Length;

        //        if (case103Str.Length <= remainingLength)
        //        {
        //            // Insert case103Str before case 104/105 text
        //            finalStr = strNM + case103Str + seqList[0].Separator + ShortStr.Substring(strNM.Length); // Insert case103Str between Case 101 and Case 104/105
        //        }
        //        else
        //        {
        //            finalStr = ShortStr; // No space for Case 103
        //        }
        //    }
        //    else
        //    {
        //        finalStr = ShortStr;
        //    }

        //    if (finalStr.Length != seqList[0].ShortLength)
        //    {
        //        finalStr = finalStr.Trim();
        //        int lsIndx = finalStr.Length;
        //        string str = finalStr.Substring(lsIndx - 1, 1);
        //        if (str == seqList[0].Separator.Trim())
        //        {
        //            finalStr = finalStr.Remove(lsIndx - 1);
        //        }
        //    }

        //    return finalStr;
        //}




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
        //public string ShortDesc1(Prosol_Datamaster cat)
        //{

        //    // var vendortype = _VendortypeRepository.FindAll().ToList();
        //    var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier), Query.EQ("RP", "MM"));
        //    var NMList = _nounModifierRepository.FindOne(FormattedQuery);
        //    var sort = SortBy.Ascending("Seq").Ascending("Description");
        //    var query = Query.EQ("Description", "Long");
        //    string LongStr = "";
        //    cat.Additionalinfo = "";
        //    var seqList = _SequenceRepository.FindAll(query, sort).ToList();
        //    var UOMSet = _UOMRepository.FindOne();
        //    //Short_Generic
        //    foreach (Prosol_Sequence sq in seqList)
        //    {
        //        if (sq.Required == "Yes")
        //        {
        //            switch (sq.CatId)
        //            {
        //                case 101:
        //                    if (cat.Noun != "OEM" && cat.Noun != "KIT")
        //                    {
        //                        if (NMList.Formatted == 1 || NMList.Formatted == 2)
        //                            LongStr += cat.Noun + sq.Separator;
        //                        // else LongStr += cat.Noun + " ";
        //                    }
        //                    break;
        //                case 102:
        //                    if (cat.Noun != "OEM" && cat.Noun != "KIT" && cat.Modifier != "PARTS" && cat.Modifier != "--")
        //                    {
        //                        if (NMList.Formatted == 1 || NMList.Formatted == 2)
        //                            if (cat.Modifier != "NO DEFINER" && cat.Modifier != "NO MODIFIER"/*&& cat.Modifier != "--"*/)
        //                                LongStr += cat.Modifier + sq.Separator;
        //                    }
        //                    break;
        //                case 103:
        //                    if (cat.Characteristics != null)
        //                    {
        //                        bool containsAdditionalInformation = cat.Characteristics
        //                                                                .GroupBy(i => i.Characteristic)
        //                                                                .Any(g => g.Key == "ADDITIONAL INFORMATION");
        //                        if (containsAdditionalInformation)
        //                        {
        //                            foreach (var ch in cat.Characteristics)
        //                            {
        //                                if (ch.Characteristic == "ADDITIONAL INFORMATION")
        //                                {
        //                                    if (!string.IsNullOrEmpty(ch.Value))
        //                                    {
        //                                        var Value = ch.Value;
        //                                        string pattern = @"(\w+(\s\w+)*):";
        //                                        MatchCollection matches = Regex.Matches(ch.Value, pattern);
        //                                        List<string> result = new List<string>();
        //                                        foreach (Match match in matches)
        //                                        {
        //                                            result.Add(match.Groups[1].Value);
        //                                        }

        //                                        //var qry = Query.EQ("Characteristic", "MM");
        //                                        //var charLst = _CharacteristicRepository.FindAll(qry).ToList();
        //                                        //var unqCharLst = charLst.GroupBy(p => p.Characteristic).Select(g => g.First()).ToList();
        //                                        foreach (var unq in result)
        //                                        {
        //                                            var qry = Query.EQ("Characteristic", unq);
        //                                            var charLst = _CharacteristicRepository.FindOne(qry);
        //                                            if (charLst != null)
        //                                            {
        //                                                if (Value.Contains(charLst.Characteristic))
        //                                                {
        //                                                    if (charLst.Abbrivation.Contains("_"))
        //                                                        charLst.Abbrivation = charLst.Abbrivation.Replace("_", " ");
        //                                                    Value = Value.Replace(charLst.Characteristic, charLst.Abbrivation);
        //                                                }
        //                                            }
        //                                        }
        //                                        cat.Additionalinfo = Value;
        //                                    }
        //                                }
        //                            }
        //                            //cat.Characteristics.RemoveAll(c => c.Characteristic == "ADDITIONAL INFORMATION");
        //                        }
        //                        int flg = 0;
        //                        foreach (Prosol_AttributeList chM in cat.Characteristics.OrderBy(x => x.Squence))
        //                        {
        //                            var tempCha = "";
        //                            var charQry = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier), Query.EQ("Characteristic", chM.Characteristic));
        //                            var charLst = _CharacteristicRepository.FindOne(charQry);
        //                            if (charLst != null)
        //                            {
        //                                if (!string.IsNullOrEmpty(charLst.Abbrivation))
        //                                {
        //                                    //chM.Characteristic = charLst.Abbrivation.Replace("_"," ");
        //                                    tempCha = charLst.Abbrivation.Replace("_", " ");
        //                                }
        //                                else
        //                                {
        //                                    tempCha = charLst.Characteristic;
        //                                }
        //                            }
        //                        if(chM.Value != null && chM.Value != "" && chM.Characteristic != "ADDITIONAL INFORMATION")
        //                            {
        //                                    if (UOMSet.Long_space == "with space")
        //                                    {
        //                                        if (chM.UOM != null && chM.UOM != "")
        //                                        {
        //                                            if (flg == 0 && chM.Characteristic == "PART NAME" || chM.Characteristic == "PART TYPE" || (cat.Noun == "KIT" && chM.Characteristic == "TYPE"))
        //                                                LongStr += chM.Value + " " + chM.UOM + sq.Separator;
        //                                            else
        //                                                LongStr += tempCha + ":" + chM.Value + " " + chM.UOM + sq.Separator;
        //                                        }
        //                                        else
        //                                        {
        //                                            if (flg == 0 && chM.Characteristic == "PART NAME" || chM.Characteristic == "PART TYPE" || (cat.Noun == "KIT" && chM.Characteristic == "TYPE"))
        //                                                LongStr += chM.Value + sq.Separator;
        //                                            else LongStr += tempCha + ":" + chM.Value + sq.Separator;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        if (chM.UOM != null && chM.UOM != "")
        //                                        {
        //                                            if (flg == 0 && chM.Characteristic == "PART NAME" || chM.Characteristic == "PART TYPE" || (cat.Noun == "KIT" && chM.Characteristic == "TYPE"))
        //                                                LongStr += chM.Value + " " + chM.UOM + sq.Separator;
        //                                            else
        //                                                LongStr += tempCha + ":" + chM.Value + chM.UOM + sq.Separator;
        //                                        }
        //                                        else
        //                                        {
        //                                            if (flg == 0 && chM.Characteristic == "PART NAME" || chM.Characteristic == "PART TYPE" || (cat.Noun == "KIT" && chM.Characteristic == "TYPE"))
        //                                                LongStr += chM.Value + sq.Separator;
        //                                            else LongStr += tempCha + ":" + chM.Value + sq.Separator;
        //                                        }
        //                                    }

        //                                }
        //                                flg = 1;
        //                            }
        //                        }
        //                        break;

        //                case 104:
        //                    //if (cat.Manufacturer != null && cat.Manufacturer != "")
        //                    //    LongStr += "Manufacturer:" + cat.Manufacturer + sq.Separator;
        //                    //break;
        //                    //if (cat.Vendorsuppliers != null)
        //                    //{

        //                    //    int g = 0;
        //                    //    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    //    {

        //                    //        if (vs.l == 1 && vs.Name != null && vs.Name != "")
        //                    //        {
        //                    //            LongStr += vs.Type + ":" + vs.Name + sq.Separator;

        //                    //        }
        //                    //        if (vs.l == 1 && vs.RefNo != null && vs.RefNo != "")
        //                    //        {
        //                    //            LongStr += vs.Refflag + ":" + vs.RefNo + sq.Separator;
        //                    //        }
        //                    //        //else
        //                    //        //{
        //                    //        //    if (vs.l == 1 && vs.RefNo != null && vs.RefNo != "")
        //                    //        //    {
        //                    //        //        LongStr += vs.Refflag + ":" + vs.RefNo + sq.Separator;
        //                    //        //    }
        //                    //        //}
        //                    //    }


        //                    //    //string[] mfrnames = new string[cat.Vendorsuppliers.Count];
        //                    //    //int iii = 0;
        //                    //    //foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    //    //{
        //                    //    //    if (!mfrnames.Contains(vs.Name))
        //                    //    //    {
        //                    //    //        mfrnames[iii] = vs.Name;
        //                    //    //        iii++;
        //                    //    //    }
        //                    //    //}
        //                    //    //foreach (string names in mfrnames)
        //                    //    //{
        //                    //    //    int g = 0;
        //                    //    //    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    //    //    {

        //                    //    //        if (vs.l == 1 && g == 0 && vs.Name == names && vs.Name != null && vs.Name != "")
        //                    //    //        {
        //                    //    //            LongStr += vs.Type + ":" + vs.Name + sq.Separator;
        //                    //    //            g = 1;
        //                    //    //        }
        //                    //    //        if (vs.l == 1 && vs.Name == names && vs.RefNo != null && vs.RefNo != "")
        //                    //    //        {
        //                    //    //            LongStr += vs.Refflag + ":" + vs.RefNo + sq.Separator;
        //                    //    //        }
        //                    //    //    }

        //                    //}
        //                    //break;
        //                    if (cat.Vendorsuppliers != null)
        //                    {
        //                        bool idxSts = false;
        //                        if (cat.Vendorsuppliers.Count() > 0)
        //                        {
        //                            if (cat.Noun != "OEM" /*&& cat.Vendorsuppliers[0].Refflag != "PART NUMBER"*/)
        //                            {
        //                                idxSts = true;
        //                            }
        //                        }
        //                        int g = 0;
        //                        var mfrCnt = 0;
        //                        var pnCnt = 0;
        //                        var mfrLst = new List<string>();
        //                        var pnLst = new List<string>();
        //                        foreach (Vendorsuppliers vn in cat.Vendorsuppliers)
        //                        {
        //                            if (vn.Refflag == "PART NUMBER" && vn.RefNo != null)
        //                            {
        //                                pnLst.Add("PART NUMBER");
        //                            }
        //                            if (vn.Type == "MANUFACTURER" && vn.Name != null)
        //                            {
        //                                mfrLst.Add("MANUFACTURER");
        //                            }
        //                        }
        //                        mfrCnt = mfrLst.Count();
        //                        pnCnt = pnLst.Count();
        //                        var mfr = 1;
        //                        var pn = 1;
        //                        if (LongStr.Contains("PN:") && pnCnt > 0)
        //                        {
        //                            LongStr = LongStr.Replace("PN:", "PN1:");
        //                            pnCnt = 2;
        //                            pn = 2;
        //                        }
        //                        if (LongStr.Contains("MFR:") && mfrCnt > 0)
        //                        {
        //                            //LongStr = LongStr.Replace("MFR:", "MFR1:");
        //                            LongStr = ConvertMFRToSequential(LongStr, out int mfrCount);
        //                            mfrCnt = mfrCount + 1;
        //                            mfr = mfrCount + 1;
        //                        }
        //                        foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                        {
        //                            if (vs.l == 1 && vs.RefNo != null && vs.RefNo != "")
        //                            {
        //                                if (vs.Refflag == "PART NUMBER")
        //                                {
        //                                    //LongStr += vs.Refflag + pn + ":" + vs.RefNo + sq.Separator;
        //                                    if (idxSts)
        //                                        LongStr += "PN" + (g+1) + ":" + vs.RefNo + sq.Separator;
        //                                    else if((pnCnt > 1 || mfrCnt > 1))
        //                                        LongStr += "PN" + pn + ":" + vs.RefNo + sq.Separator;
        //                                    pn++;
        //                                }
        //                                else if (vs.Refflag == "SUPPLIER PART NUMBER")
        //                                {
        //                                    //LongStr += vs.Refflag + pn + ":" + vs.RefNo + sq.Separator;
        //                                    LongStr += "SUPPLIER PN:" + vs.RefNo + sq.Separator;
        //                                    pn++;
        //                                }
        //                                else if (vs.Refflag == "MODEL NUMBER")
        //                                {
        //                                    //LongStr += vs.Refflag + pn + ":" + vs.RefNo + sq.Separator;
        //                                    LongStr += "MODEL NO:" + vs.RefNo + sq.Separator;
        //                                    pn++;
        //                                }
        //                                else
        //                                {
        //                                    //LongStr += vs.Refflag + ":" + vs.RefNo + sq.Separator;
        //                                    LongStr += getRefType(vs.Refflag) + ":" + vs.RefNo + sq.Separator;
        //                                }
        //                            }
        //                            if (vs.l == 1 && vs.Name != null && vs.Name != "")
        //                            {
        //                                if (vs.Type == "MANUFACTURER")
        //                                {
        //                                    //LongStr += vs.Type + mfr + ":" + vs.Name + sq.Separator;
        //                                    if (idxSts)
        //                                        LongStr += "MFR" + (g + 1) + ":" + vs.Name + sq.Separator;
        //                                    else if ((pnCnt > 1 || mfrCnt > 1))
        //                                        LongStr += "MFR" + mfr + ":" + vs.Name + sq.Separator;
        //                                    mfr++;
        //                                }
        //                                else if (vs.Type == "SUPPLIER")
        //                                {
        //                                    //LongStr += vs.Type + mfr + ":" + vs.Name + sq.Separator;
        //                                    LongStr += "SUPPLIER:" + vs.Name + sq.Separator;
        //                                    mfr++;
        //                                }
        //                                else
        //                                {
        //                                    //LongStr += vs.Type + ":" + vs.Name + sq.Separator;
        //                                    LongStr += "MFR:" + vs.Name + sq.Separator;
        //                                }
        //                            }
        //                            //else
        //                            //{
        //                            //    if (vs.l == 1 && vs.RefNo != null && vs.RefNo != "")
        //                            //    {
        //                            //        LongStr += vs.Refflag + ":" + vs.RefNo + sq.Separator;
        //                            //    }
        //                            //}
        //                            g++;
        //                        }

        //                        //string[] mfrnames = new string[cat.Vendorsuppliers.Count];
        //                        //int iii = 0;
        //                        //foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                        //{
        //                        //    if (!mfrnames.Contains(vs.Name))
        //                        //    {
        //                        //        mfrnames[iii] = vs.Name;
        //                        //        iii++;
        //                        //    }
        //                        //}
        //                        //foreach (string names in mfrnames)
        //                        //{
        //                        //    int g = 0;
        //                        //    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                        //    {

        //                        //        if (vs.l == 1 && g == 0 && vs.Name == names && vs.Name != null && vs.Name != "")
        //                        //        {
        //                        //            LongStr += vs.Type + ":" + vs.Name + sq.Separator;
        //                        //            g = 1;
        //                        //        }
        //                        //        if (vs.l == 1 && vs.Name == names && vs.RefNo != null && vs.RefNo != "")
        //                        //        {
        //                        //            LongStr += vs.Refflag + ":" + vs.RefNo + sq.Separator;
        //                        //        }
        //                        //    }

        //                    }
        //                    break;
        //                case 105:
        //                    //if (cat.Partno != null && cat.Partno != "")
        //                    //    LongStr += "Partno:" + cat.Partno + sq.Separator;
        //                    //break;


        //                    //foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    //{




        //                    //    if (vs.l == '1' && vs.Refflag == "PART NUMBER")
        //                    //    {
        //                    //        LongStr += vs.Type.ToUpper()+" "+ "PART NUMBER" + ":" + vs.RefNo + sq.Separator;

        //                    //    }


        //                    //}
        //                    break;

        //                case 106:
        //                    if (cat.Application != null && cat.Application != "")
        //                        LongStr += "APPLICATION:" + cat.Application + sq.Separator;
        //                    break;
        //                case 107:
        //                    //if (cat.Drawingno != null && cat.Drawingno != "")
        //                    //    LongStr += "Drawing NO.:" + cat.Drawingno + sq.Separator;
        //                    //break;


        //                    //foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    //{
        //                    //    if (vs.l == '1' && vs.Refflag == "DRAWING & POSITION NUMBER")
        //                    //    {
        //                    //        LongStr += vs.Type.ToUpper() + " DRAWING & POSITION NUMBER:" + vs.RefNo + sq.Separator;

        //                    //    }
        //                    //    if (vs.l == '1' && vs.Refflag == "MODEL NUMBER")
        //                    //    {
        //                    //        LongStr += vs.Type.ToUpper() + " MODEL NUMBER:" + vs.RefNo + sq.Separator;

        //                    //    }
        //                    //    if (vs.l == '1' && vs.Refflag == "REFERENCE NUMBER")
        //                    //    {
        //                    //        LongStr += vs.Type.ToUpper() + " REFERENCE NUMBER:" + vs.RefNo + sq.Separator;

        //                    //    }
        //                    //}

        //                    break;


        //                case 108:
        //                    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "")
        //                        LongStr += "EQUIPMENT NAME:" + cat.Equipment.Name + sq.Separator;
        //                    break;
        //                case 109:
        //                    if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
        //                        LongStr += "EQUIPMENT MANUFACTURER:" + cat.Equipment.Manufacturer + sq.Separator;
        //                    break;
        //                case 110:
        //                    if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "")
        //                        LongStr += "EQUIPMENT MODEL NO:" + cat.Equipment.Modelno + sq.Separator;
        //                    break;
        //                case 111:
        //                    if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "")
        //                        LongStr += "EQUIPMENT TAG NO:" + cat.Equipment.Tagno + sq.Separator;
        //                    break;
        //                case 112:
        //                    if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "")
        //                        LongStr += "EQUIPMENT SERIAL NO:" + cat.Equipment.Serialno + sq.Separator;
        //                    break;
        //                case 113:
        //                    if (cat.Referenceno != null && cat.Referenceno != "")
        //                        LongStr += "POSITION NO." + cat.Referenceno + sq.Separator;
        //                    break;
        //                case 114:
        //                    var charQry1 = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier), Query.EQ("Characteristic", "ADDITIONAL INFORMATION"));
        //                    var charLst1 = _CharacteristicRepository.FindOne(charQry1);
        //                    if (cat.Additionalinfo != null && cat.Additionalinfo != "")
        //                        LongStr += charLst1.Abbrivation.Replace("_", " ") + ":" + cat.Additionalinfo + sq.Separator;
        //                    break;
        //                case 115:
        //                    if (cat.Equipment != null && cat.Equipment.Additionalinfo != null && cat.Equipment.Additionalinfo != "")
        //                        //LongStr += "ADDITIONAL INFORMATION(EQUIPMENT):" + cat.Equipment.Additionalinfo + sq.Separator;
        //                        LongStr += "EQUIPMENT SPECIFICATION:" + cat.Equipment.Additionalinfo + sq.Separator;
        //                    break;

        //                case 119:
        //                    if (cat.Equipment != null && cat.Equipment.SuperiorEquipment != null && cat.Equipment.SuperiorEquipment != "")
        //                        LongStr += "SUPERIOR EQUIPMENT:" + cat.Equipment.SuperiorEquipment + sq.Separator;
        //                    break;


        //            }
        //            seqList[0].ShortLength = 240;
        //            if (!checkLength(LongStr, seqList[0].ShortLength))
        //            {
        //                LongStr = LongStr.Trim();
        //                char[] chr = sq.Separator.ToCharArray();
        //                LongStr = LongStr.TrimEnd(chr[0]);
        //                while (LongStr.Length > seqList[0].ShortLength)
        //                {
        //                    int lstIndx = LongStr.LastIndexOf(chr[0]);
        //                    LongStr = LongStr.Remove(lstIndx);
        //                }
        //                break;
        //            }
        //        }
        //    }
        //    //LongStr = LongStr.Trim();
        //    //int lstIndx = LongStr.Length;
        //    //LongStr = LongStr.Remove(lstIndx - 1, 1);

        //    //if (LongStr.Length != 240)
        //    //{
        //    //    LongStr = LongStr.Trim();
        //    //    int lsIndx = LongStr.Length;
        //    //    string str = LongStr.Substring(lsIndx - 1, 1);
        //    //    if (str == seqList[0].Separator.Trim())
        //    //    {
        //    //        LongStr = LongStr.Remove(lsIndx - 1);
        //    //    }
        //    //}

        //    //if (LongStr.Length < 240)
        //    //{
        //    //    LongStr = LongStr.PadRight(240);
        //    //}
        //    //else if (LongStr.Length > 240)
        //    //{
        //    //    LongStr = LongStr.Substring(0, 240);
        //    //}
        //    seqList[0].ShortLength = 240;
        //    if (LongStr.Length > 0 && LongStr.Length != seqList[0].ShortLength)
        //    {
        //        LongStr = LongStr.Trim();
        //        int lsIndx = LongStr.Length;
        //        string str = LongStr.Substring(lsIndx - 1, 1);
        //        if (str == seqList[0].Separator.Trim())
        //        {
        //            LongStr = LongStr.Remove(lsIndx - 1);
        //        }
        //    }
        //    return LongStr;
        //}
        public string LongDesc(Prosol_Datamaster cat)
        {

            // var vendortype = _VendortypeRepository.FindAll().ToList();
            var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
            var NMList = _nounModifierRepository.FindOne(FormattedQuery);
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
                            if (NMList.Formatted == 1 || NMList.Formatted == 2)
                                LongStr += cat.Noun + sq.Separator;
                            else LongStr += cat.Noun + " ";
                            break;
                        case 102:
                            if (NMList.Formatted == 1 || NMList.Formatted == 2)
                            {
                                if (cat.Modifier != "NO QUALIFIER" && cat.Modifier != "NO MODIFIER")
                                    LongStr += cat.Modifier + sq.Separator;
                            }
                            break;
                        case 103:
                            if (cat.Characteristics != null)
                            {
                                int flg = 0;
                                foreach (Prosol_AttributeList chM in cat.Characteristics.OrderBy(x => x.Squence))
                                {

                                    if (chM.Value != null && chM.Value != "" && chM.Characteristic != "ADDITIONAL INFORMATION")
                                    {
                                        if (UOMSet.Long_space == "with space")
                                        {
                                            if (chM.UOM != null && chM.UOM != "")
                                            {
                                                LongStr += chM.Characteristic + ":" + chM.Value + " " + chM.UOM + sq.Separator;
                                            }
                                            else
                                            {
                                                if (flg == 0 && (chM.Characteristic == "PART NAME" || chM.Characteristic == "PART TYPE"))
                                                    LongStr += chM.Value + sq.Separator;
                                                else LongStr += chM.Characteristic + ":" + chM.Value + sq.Separator;
                                            }
                                        }
                                        else
                                        {
                                            if (chM.UOM != null && chM.UOM != "")
                                            {
                                                LongStr += chM.Characteristic + ":" + chM.Value + chM.UOM + sq.Separator;
                                            }
                                            else
                                            {
                                                if (flg == 0 && chM.Characteristic == "PART NAME")
                                                    LongStr += chM.Value + sq.Separator;
                                                else LongStr += chM.Characteristic + ":" + chM.Value + sq.Separator;
                                            }
                                        }

                                    }
                                    flg = 1;
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
                                LongStr += "APPLICATION:" + cat.Application + sq.Separator;
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
                                LongStr += "EQUIPMENT NAME:" + cat.Equipment.Name + sq.Separator;
                            break;
                        case 109:
                            if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
                                LongStr += "EQUIPMENT MANUFACTURER:" + cat.Equipment.Manufacturer + sq.Separator;
                            break;
                        case 110:
                            if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "")
                                LongStr += "EQUIPMENT MODELNO:" + cat.Equipment.Modelno + sq.Separator;
                            break;
                        case 111:
                            if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "")
                                LongStr += "EQUIPMENT TAGNO:" + cat.Equipment.Tagno + sq.Separator;
                            break;
                        case 112:
                            if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "")
                                LongStr += "EQUIPMENT SERIALNO:" + cat.Equipment.Serialno + sq.Separator;
                            break;
                        case 113:
                            if (cat.Referenceno != null && cat.Referenceno != "")
                                LongStr += "POSITION NO." + cat.Referenceno + sq.Separator;
                            break;
                        case 114:
                            var add = cat.Characteristics.Where(x => x.Characteristic == "ADDITIONAL INFORMATION").ToList();
                            if (!string.IsNullOrEmpty(cat.Additionalinfo) && add.Count > 0 && !string.IsNullOrEmpty(add[0].Value))
                            {
                                LongStr += "ADDITIONAL INFORMATION:" + add[0].Value + "," + cat.Additionalinfo + sq.Separator;
                            }
                            else if (add.Count > 0 && !string.IsNullOrEmpty(add[0].Value))
                            {
                                LongStr += "ADDITIONAL INFORMATION:" + add[0].Value + sq.Separator;
                            }
                            else if (!string.IsNullOrEmpty(cat.Additionalinfo))
                            {
                                LongStr += "ADDITIONAL INFORMATION:" + cat.Additionalinfo + sq.Separator;
                            }
                            break;
                        case 115:
                            if (cat.Equipment != null && cat.Equipment.Additionalinfo != null && cat.Equipment.Additionalinfo != "")
                                LongStr += "ADDITIONAL INFORMATION(EQUIPMENT):" + cat.Equipment.Additionalinfo + sq.Separator;
                            break;


                    }
                }
            }
            LongStr = LongStr.Trim();
            int lstIndx = LongStr.Length;
            LongStr = LongStr.Remove(lstIndx - 1, 1);
            return LongStr.ToUpper();
        }
        public string ExLongDesc(Prosol_Datamaster cat)
        {
            var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier), Query.EQ("RP", "MM"));
            var NMList = _nounModifierRepository.FindOne(FormattedQuery);
            var sort = SortBy.Ascending("Seq").Ascending("Description");
            var query = Query.EQ("Description", "Long");
            string LongStr = "";
            cat.Additionalinfo = "";
            var seqList = _SequenceRepository.FindAll(query, sort).ToList();
            var UOMSet = _UOMRepository.FindOne();
            if (cat.exCharacteristics != null)
            {

                //bool containsAdditionalInformation = cat.exCharacteristics
                //                                        .GroupBy(i => i.Characteristic)
                //                                        .Any(g => g.Key == "ADDITIONAL INFORMATION");
                //if (containsAdditionalInformation)
                //{
                //    foreach (var ch in cat.exCharacteristics)
                //    {
                //        if (ch.Characteristic == "ADDITIONAL INFORMATION")
                //        {
                //            if (!string.IsNullOrEmpty(ch.Value))
                //            {
                //                var Value = ch.Value;
                //                string pattern = @"(\w+(\s\w+)*):";
                //                MatchCollection matches = Regex.Matches(ch.Value, pattern);
                //                List<string> result = new List<string>();
                //                foreach (Match match in matches)
                //                {
                //                    result.Add(match.Groups[1].Value);
                //                }

                //                //var qry = Query.EQ("Characteristic", "MM");
                //                //var charLst = _CharacteristicRepository.FindAll(qry).ToList();
                //                //var unqCharLst = charLst.GroupBy(p => p.Characteristic).Select(g => g.First()).ToList();
                //                foreach (var unq in result)
                //                {
                //                    var qry = Query.EQ("Characteristic", unq);
                //                    var charLst = _CharacteristicRepository.FindOne(qry);
                //                    if (charLst != null)
                //                    {
                //                        if (Value.Contains(charLst.Characteristic))
                //                        {
                //                            if (charLst.Abbrivation.Contains("_"))
                //                                charLst.Abbrivation = charLst.Abbrivation.Replace("_", " ");
                //                            Value = Value.Replace(charLst.Characteristic, charLst.Abbrivation);
                //                        }
                //                    }
                //                }
                //                cat.Additionalinfo = Value;
                //            }
                //        }
                //    }
                //    //cat.exCharacteristics.RemoveAll(c => c.Characteristic == "ADDITIONAL INFORMATION");
                //}
                int flg = 0;
                foreach (Prosol_AttributeList chM in cat.exCharacteristics.OrderBy(x => x.Squence))
                {
                    var tempCha = "";
                    var charQry = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier), Query.EQ("Characteristic", chM.Characteristic));
                    var charLst = _CharacteristicRepository.FindOne(charQry);
                    if (charLst != null)
                    {
                        if (!string.IsNullOrEmpty(charLst.Abbrivation))
                        {
                            //chM.Characteristic = charLst.Abbrivation.Replace("_"," ");
                            tempCha = charLst.Abbrivation.Replace("_", " ");
                        }
                        else
                        {
                            tempCha = charLst.Characteristic;
                        }
                    }
            if (chM.Value != null && chM.Value != "" )
                    {
                        if (UOMSet.Long_space == "with space")
                        {
                            if (chM.UOM != null && chM.UOM != "")
                            {
                                if (flg == 0 && chM.Characteristic == "PART NAME" || chM.Characteristic == "PART TYPE")
                                    LongStr += chM.Value + " " + chM.UOM + ",";
                                else
                                    LongStr += tempCha + ":" + chM.Value + " " + chM.UOM + ",";
                            }
                            else
                            {
                                if (flg == 0 && chM.Characteristic == "PART NAME" || chM.Characteristic == "PART TYPE")
                                    LongStr += chM.Value + ",";
                                else LongStr += tempCha + ":" + chM.Value + ",";
                            }
                        }
                        else
                        {
                            if (chM.UOM != null && chM.UOM != "")
                            {
                                if (flg == 0 && chM.Characteristic == "PART NAME" || chM.Characteristic == "PART TYPE")
                                    LongStr += chM.Value + " " + chM.UOM + ",";
                                else
                                    LongStr += tempCha + ":" + chM.Value + chM.UOM + ",";
                            }
                            else
                            {
                                if (flg == 0 && chM.Characteristic == "PART NAME" || chM.Characteristic == "PART TYPE")
                                    LongStr += chM.Value + ",";
                                else LongStr += tempCha + ":" + chM.Value + ",";
                            }
                        }

                    }
                    flg = 1;
                }
            }
            if (cat.exManufacturer != null)
                LongStr += cat.exManufacturer + ",";
            if (cat.exPartno != null)
                LongStr += cat.exPartno+ ",";
            if (LongStr.EndsWith(","))
            {
                LongStr = LongStr.Substring(0, LongStr.Length - 1);
            }
            return LongStr;
        }

        //private string LongDesc(Prosol_Datamaster cat)
        //{
        //    var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
        //    var NMList = _nounModifierRepository.FindAll(FormattedQuery).ToList();
        //    var sort = SortBy.Ascending("Seq").Ascending("Description");
        //    var query = Query.EQ("Description", "Long");
        //    string LongStr = "";
        //    var seqList = _SequenceRepository.FindAll(query, sort).ToList();
        //    var UOMSet = _UOMRepository.FindOne();
        //    //Short_Generic
        //    foreach (Prosol_Sequence sq in seqList)
        //    {
        //        if (sq.Required == "Yes")
        //        {
        //            switch (sq.CatId)
        //            {
        //                case 101:
        //                    if (NMList.Formatted == 1)
        //                        LongStr += cat.Noun + sq.Separator;
        //                    else LongStr += cat.Noun + " ";
        //                    break;
        //                case 102:
        //                    if (cat.Modifier != "NO DEFINER" && cat.Modifier != "NO MODIFIER")
        //                        LongStr += cat.Modifier + sq.Separator;
        //                    break;
        //                case 103:
        //                    foreach (Prosol_AttributeList chM in cat.Characteristics.OrderBy(x => x.Squence))
        //                    {
        //                        if (chM.Value != null && chM.Value != "")
        //                        {
        //                            if (UOMSet.Long_space == "with space")
        //                            {
        //                                if (chM.UOM != null && chM.UOM != "")
        //                                    LongStr += chM.Characteristic + ":" + chM.Value + " " + chM.UOM + sq.Separator;
        //                                else LongStr += chM.Characteristic + ":" + chM.Value + sq.Separator;
        //                            }
        //                            else
        //                            {
        //                                if (chM.UOM != null && chM.UOM != "")
        //                                    LongStr += chM.Characteristic + ":" + chM.Value + chM.UOM + sq.Separator;
        //                                else LongStr += chM.Characteristic + ":" + chM.Value + sq.Separator;
        //                            }

        //                        }
        //                    }
        //                    break;

        //                case 104:
        //                    if (cat.Manufacturer != null && cat.Manufacturer != "")
        //                        LongStr += "Manufacturer:" + cat.Manufacturer + sq.Separator;
        //                    break;
        //                case 105:
        //                    if (cat.Partno != null && cat.Partno != "")
        //                        LongStr += "Partno:" + cat.Partno + sq.Separator;
        //                    break;
        //                case 106:
        //                    if (cat.Application != null && cat.Application != "")
        //                        LongStr += "Application:" + cat.Application + sq.Separator;
        //                    break;
        //                case 107:
        //                    if (cat.Drawingno != null && cat.Drawingno != "")
        //                        LongStr += "Drawingno:" + cat.Drawingno + sq.Separator;
        //                    break;
        //                case 108:
        //                    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "")
        //                        LongStr += "Equipment Name:" + cat.Equipment.Name + sq.Separator;
        //                    break;
        //                case 109:
        //                    if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
        //                        LongStr += "Equipment Manufacturer:" + cat.Equipment.Manufacturer + sq.Separator;
        //                    break;
        //                case 110:
        //                    if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "")
        //                        LongStr += "Equipment Modelno:" + cat.Equipment.Modelno + sq.Separator;
        //                    break;
        //                case 111:
        //                    if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "")
        //                        LongStr += "Equipment Tagno:" + cat.Equipment.Tagno + sq.Separator;
        //                    break;
        //                case 112:
        //                    if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "")
        //                        LongStr += "Equipment Serialno:" + cat.Equipment.Serialno + sq.Separator;
        //                    break;
        //                case 113:
        //                    if (cat.Referenceno != null && cat.Referenceno != "")
        //                        LongStr += "Referenceno:" + cat.Referenceno + sq.Separator;
        //                    break;
        //                case 114:
        //                    if (cat.Additionalinfo != null && cat.Additionalinfo != "")
        //                        LongStr += "Additional Information:" + cat.Additionalinfo + sq.Separator;
        //                    break;
        //                case 115:
        //                    if (cat.Equipment != null && cat.Equipment.Additionalinfo != null && cat.Equipment.Additionalinfo != "")
        //                        LongStr += "Additional Information(Equipment):" + cat.Equipment.Additionalinfo + sq.Separator;
        //                    break;


        //            }
        //        }
        //    }
        //    LongStr = LongStr.Trim();
        //    int lstIndx = LongStr.Length;
        //    LongStr = LongStr.Remove(lstIndx - 1, 1);
        //    return LongStr.ToUpper();
        //}

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
            var proCat = new Prosol_Datamaster();
            var LstVendors = new List<Vendorsuppliers>();
            if (cat.Vendorsuppliers != null && cat.Vendorsuppliers.Count > 0)
            {
                foreach (Vendorsuppliers LstAtt in cat.Vendorsuppliers)
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


            //Attribute

            var lstCharateristics = new List<Prosol_AttributeList>();
            if (cat.Characteristics != null)
            {

                foreach (Prosol_AttributeList LstAtt in cat.Characteristics)
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






            proCat._id = cat._id;
            proCat.Itemcode = cat.Itemcode;
            proCat.Noun = cat.Noun;
            proCat.Modifier = cat.Modifier;
            proCat.Partno = cat.Partno;
            proCat.Reworkcat = cat.Reworkcat;
            proCat.Characteristics = lstCharateristics;
            proCat.Vendorsuppliers = LstVendors;


            //Equipment

            if (cat.Equipment != null)
            {
                var Equi_mdl = new Equipments();
                Equi_mdl.Name = cat.Equipment.Name;
                Equi_mdl.Manufacturer = cat.Equipment.Manufacturer;
                Equi_mdl.Modelno = cat.Equipment.Modelno;
                Equi_mdl.Tagno = cat.Equipment.Tagno;
                Equi_mdl.Serialno = cat.Equipment.Serialno;
                Equi_mdl.Additionalinfo = cat.Equipment.Additionalinfo;
                Equi_mdl.EMS = cat.Equipment.EMS;
                Equi_mdl.ENS = cat.Equipment.ENS;

                proCat.Equipment = Equi_mdl;

            }

        //gan:
        //    var Qry = Query.And(Query.NE("Itemcode", cat.Itemcode), Query.EQ("Shortdesc", cat.Shortdesc), Query.Or(Query.EQ("Vendorsuppliers", BsonNull.Value), Query.EQ("Vendorsuppliers", new BsonArray())));

        //    var DaList = _DatamasterRepository.FindAll(Qry).ToList();
        //    if (DaList != null && DaList.Count > 0)
        //    {

        //        foreach (Prosol_Datamaster dm in DaList)
        //        {
        //            if (dm.Characteristics != null && proCat.Characteristics != null)
        //            {
        //                foreach (Prosol_AttributeList mdol in dm.Characteristics)
        //                {
        //                    int lnx = 0;
        //                    foreach (Prosol_AttributeList mdl in proCat.Characteristics)
        //                    {
        //                        if (mdl.Characteristic == mdol.Characteristic)
        //                        {
        //                            if (mdl.Value == null)
        //                                mdl.Value = "";
        //                            if (mdol.Value == null)
        //                                mdol.Value = "";
        //                            if (mdl.Value != mdol.Value && !string.IsNullOrEmpty(mdl.Value) && !string.IsNullOrEmpty(mdol.Value))
        //                            {
        //                                if (lnx > 1)
        //                                {
        //                                    for (int dx = lnx; dx <= lnx; dx--)
        //                                    {
        //                                        if (proCat.Characteristics[dx - 1].Value != "" && proCat.Characteristics[dx - 1].Value != null)
        //                                        {
        //                                            proCat.Characteristics[dx - 1].Value = "";
        //                                            break;
        //                                            break;
        //                                        }
        //                                    }
        //                                    cat.Shortdesc = ShortDesc(proCat);
        //                                    goto gan;
        //                                }


        //                            }

        //                        }
        //                        lnx++;
        //                    }
        //                }


        //            }
        //        }

        //    }

            //cat.Shortdesc_ = ShortDesc1(cat);
            cat.Longdesc = LongDesc(cat);
            cat.EnrichedValue = PapulatedValue(cat);
            cat.MissingValue = MissingValue(cat);
            var query = Query.EQ("Itemcode", cat.Itemcode);
            var vn = _DatamasterRepository.FindAll(query).ToList();
            if (vn != null && vn.Count > 0)
            {

                var dupList = checkDuplicate1(cat);
                if (cat.ItemStatus == 11)
                {
                    cat.Rework = "PV";
                    cat.PVuser = vn[0].PVuser;
                }

                if (stus == "Yes" && dupList != null && dupList.Count > 0)
                {
                    if (dupList[0].Duplicates == null || dupList[0].Duplicates == "")
                    {

                        dupList[0].Duplicates = dupList[0].Materialcode;
                        _DatamasterRepository.Add(dupList[0]);


                    }
                    cat.Catalogue = vn[0].Catalogue;

                    if (cat.Review == null)
                    {
                        var review = new Prosol_UpdatedBy();
                        review.UserId = cat.Catalogue.UserId;
                        review.Name = cat.Catalogue.Name;
                        review.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        cat.Review = review;
                    }
                    if (cat.Release == null)
                    {

                        var rels = new Prosol_UpdatedBy();
                        rels.UserId = cat.Catalogue.UserId;
                        rels.Name = cat.Catalogue.Name;
                        rels.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        cat.Release = rels;

                    }

                    cat.ItemStatus = 6;
                    cat.Materialcode = dupList[0].Duplicates;
                    cat.Duplicates = dupList[0].Duplicates;
                    cat.Noun = dupList[0].Noun;
                    cat.Modifier = dupList[0].Modifier;
                    cat.Shortdesc = dupList[0].Shortdesc;
                    cat.Longdesc = dupList[0].Longdesc;
                    cat.UOM = dupList[0].UOM;
                    cat.Catalogue.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                    //cat.Rework = vn[0].Rework;
                    cat.Reworkcat = vn[0].Reworkcat;
                    cat.Junk = vn[0].Junk;

                    cat.Vendorsuppliers = dupList[0].Vendorsuppliers;
                    cat.Equipment = dupList[0].Equipment;
                    cat.CreatedOn = vn[0].CreatedOn;
                    cat.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                    _DatamasterRepository.Add(cat);



                    res = 1;
                }
                else
                {
                    if (vn[0].Duplicates != "" && vn[0].Duplicates != null)
                    {

                        if (vn[0].Itemcode == vn[0].Duplicates)
                        {
                            //Parent remove duplicate
                            var chkQry = Query.EQ("Duplicates", vn[0].Duplicates);
                            var duList = _DatamasterRepository.FindAll(chkQry).ToList();
                            if (duList != null && duList.Count > 0)
                            {
                                foreach (Prosol_Datamaster ob in duList)
                                {
                                    if (ob.Itemcode != vn[0].Itemcode)
                                    {
                                        ob.Duplicates = null;
                                        ob.ItemStatus = 0;
                                        ob.Materialcode = ob.Itemcode;
                                        cat.Vendorsuppliers = null;
                                        cat.Equipment = null;

                                        if (ob.Review == null)
                                        {
                                            var review = new Prosol_UpdatedBy();
                                            review.UserId = ob.Catalogue.UserId;
                                            review.Name = ob.Catalogue.Name;
                                            review.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                                            ob.Review = review;
                                        }
                                        if (ob.Release == null)
                                        {

                                            var rels = new Prosol_UpdatedBy();
                                            rels.UserId = ob.Catalogue.UserId;
                                            rels.Name = ob.Catalogue.Name;
                                            rels.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                                            ob.Release = rels;

                                        }

                                        _DatamasterRepository.Add(ob);
                                    }
                                }

                            }

                        }
                        else
                        {
                            //single child remove duplicate
                            var chkQry = Query.EQ("Duplicates", vn[0].Duplicates);
                            var duList = _DatamasterRepository.FindAll(chkQry).ToList();
                            if (duList != null && duList.Count == 2)
                            {
                                foreach (Prosol_Datamaster ob in duList)
                                {
                                    if (ob.Itemcode == vn[0].Duplicates)
                                    {
                                        ob.Duplicates = null;
                                        _DatamasterRepository.Add(ob);
                                    }
                                }
                            }
                        }
                    }

                    cat.Duplicates = null;
                    //         cat.Materialcode = vn[0].Itemcode;
                    cat.Catalogue = vn[0].Catalogue;
                    cat.Review = vn[0].Review;
                    cat.Release = vn[0].Release;

                    if (cat.Catalogue != null && cat.ItemStatus == 1)
                    {
                        cat.Catalogue.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }
                    if (cat.Review != null && cat.ItemStatus == 3)
                    {
                        cat.Review.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }
                    if (cat.Release != null && cat.ItemStatus == 5)
                    {
                        cat.Release.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }

                    //cat.Rework = vn[0].Rework;
                    cat.Reworkcat = vn[0].Reworkcat;
                    cat.Junk = vn[0].Junk;
                    cat.CreatedOn = vn[0].CreatedOn;
                    cat.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                    _DatamasterRepository.Add(cat);

                }

            }
            else
            {
                //var dupList = checkDuplicate(cat);
                //if (stus == "Yes" && dupList != null && dupList.Count > 0)
                //{
                //    if (dupList[0].Duplicates != "" && dupList[0].Duplicates != null)
                //    {

                //        if (dupList[0].Itemcode == dupList[0].Duplicates)
                //        {
                //            cat.Materialcode = dupList[0].Materialcode;
                //            cat.Duplicates = dupList[0].Materialcode;
                //            cat.ItemStatus = 6;

                //            _DatamasterRepository.Add(cat);
                //        }
                var dupList = checkDuplicate1(cat);

                if (stus == "Yes" && dupList != null && dupList.Count > 0)
                {
                    if (dupList[0].Duplicates == null || dupList[0].Duplicates == "")
                    {

                        dupList[0].Duplicates = dupList[0].Materialcode;
                        _DatamasterRepository.Add(dupList[0]);


                    }

                    // cat.Catalogue = vn[0].Catalogue;

                    if (cat.Review == null)
                    {
                        var review = new Prosol_UpdatedBy();
                        review.UserId = cat.Catalogue.UserId;
                        review.Name = cat.Catalogue.Name;
                        review.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        cat.Review = review;
                    }
                    if (cat.Release == null)
                    {

                        var rels = new Prosol_UpdatedBy();
                        rels.UserId = cat.Catalogue.UserId;
                        rels.Name = cat.Catalogue.Name;
                        rels.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        cat.Release = rels;

                    }

                    cat.ItemStatus = 6;
                    cat.Materialcode = dupList[0].Duplicates;
                    cat.Duplicates = dupList[0].Duplicates;
                    cat.Noun = dupList[0].Noun;
                    cat.Modifier = dupList[0].Modifier;
                    cat.Shortdesc = dupList[0].Shortdesc;
                    cat.Longdesc = dupList[0].Longdesc;
                    cat.UOM = dupList[0].UOM;
                    cat.Catalogue.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                    //cat.Rework = vn[0].Rework;
                    // cat.Reworkcat = vn[0].Reworkcat;
                    //  cat.Junk = vn[0].Junk;

                    cat.Vendorsuppliers = dupList[0].Vendorsuppliers;
                    cat.Equipment = dupList[0].Equipment;
                    //cat.CreatedOn = vn[0].CreatedOn;
                    cat.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                    _DatamasterRepository.Add(cat);



                    res = 1;
                }
                else
                {
                    _DatamasterRepository.Add(cat);

                }


                //  }

                //else
                //{

                //    var dupCheck = checkDuplicate(cat);
                //    if (stus == "Yes" && dupCheck != null && dupCheck.Count > 0)
                //    {
                //        res = 3;
                //    }
                //    else if (dupCheck != null && dupCheck.Count > 0)
                //    {
                //        _DatamasterRepository.Add(cat);
                //    }
                //    else if (dupCheck == null)
                //    {

                //        if (cat.ItemStatus == 1)
                //        {
                //            cat.Catalogue.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //        }
                //        if (cat.ItemStatus == 3)
                //        {
                //            cat.Review.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //        }
                //        if (cat.ItemStatus == 5)
                //        {
                //            cat.Release.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                //        }
                //        _DatamasterRepository.Add(cat);
                //    }


            }
            return res;


        }

        public bool WriteERPInfo(Prosol_ERPInfo erp)
        {
            var erplist = erp as IList;
            var Qrymat = Query.And(Query.EQ("Itemcode", erp.Itemcode), Query.EQ("ItemStatus", 6));
            var resmat = _DatamasterRepository.FindOne(Qrymat);
            if (resmat != null)
            {
                var get = Query.EQ("Itemcode", erp.Itemcode);
                var getexisterp = _erpRepository.FindOne(get);

                var x = AreObjectsEqual(erp, getexisterp, erp.Itemcode, "QA", "Material Master");
                var res = _erplogRepository.Add(x);
            }


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
        public static List<Prosol_ERPLog> AreObjectsEqual(object objectA, object objectB, string itemcode, string page, string module, params string[] ignoreList)
        {
            var res = new List<Prosol_ERPLog>();

            if (objectA != null && objectB != null)
            {
                Type objectType;

                objectType = objectA.GetType();


                foreach (PropertyInfo propertyInfo in objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && !ignoreList.Contains(p.Name)))
                {
                    object valueA;
                    object valueB;

                    valueA = propertyInfo.GetValue(objectA, null);
                    valueB = propertyInfo.GetValue(objectB, null);
                    if (valueA != null && valueB != null && valueA != "" && valueB != "")
                        if (CanDirectlyCompare(propertyInfo.PropertyType))
                        {
                            if (!AreValuesEqual(valueA, valueB))
                            {
                                if (propertyInfo.Name.Contains("_"))
                                {
                                    var x = new Prosol_ERPLog();
                                    x.Itemcode = itemcode;
                                    x.fieldname = propertyInfo.Name;
                                    x.newvalue = valueA.ToString();
                                    x.oldValue = valueB.ToString();
                                    x.page = page;
                                    x.module = module;
                                    x.CreatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                                    res.Add(x);
                                }
                            }
                        }

                }

            }


            return res;
        }

        private static bool CanDirectlyCompare(Type type)
        {
            return typeof(IComparable).IsAssignableFrom(type) || type.IsPrimitive || type.IsValueType;
        }
        private static bool AreValuesEqual(object valueA, object valueB)
        {
            bool result;
            IComparable selfValueComparer;

            selfValueComparer = valueA as IComparable;

            if (valueA == null && valueB != null || valueA != null && valueB == null)
                result = false; // one of the values is null
            else if (selfValueComparer != null && selfValueComparer.CompareTo(valueB) != 0)
                result = false; // the comparison using IComparable failed
            else if (!object.Equals(valueA, valueB))
                result = false; // the comparison using Equals failed
            else
                result = true; // match

            return result;
        }

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


                oneResult.Catalogue = cataloguer;
                oneResult.Review = mdl.Review;
                oneResult.UpdatedOn = mdl.UpdatedOn;
                oneResult.ItemStatus = mdl.ItemStatus;

                res = _DatamasterRepository.Add(oneResult);

                if (res) cont++;
                //var query1 = Query.EQ("itemId", mdl.Itemcode);
                //var rejcde = _ProsolRequest.FindAll(query1).ToList();
                //if (rejcde != null)
                //{
                //    var qryUser = Query.EQ("Userid", rejcde[0].requester);
                //    var UsrRes = _usersRepository.FindOne(qryUser);
                //    string subjectt = "Material code created for Request: " + rejcde[0].itemId;
                //    string body = "<html><body><table><tr><td style='padding-left: 50px;padding-top: 10px;'>Request Id</td><td style='padding-left: 50px;padding-top: 10px;'>" + mdl.Itemcode + "</td></tr><tr><td style='padding-left: 50px;padding-top: 10px;'>Material code</td><td style='padding-left: 50px;padding-top: 10px;'>" + mdl.Materialcode + "</td></tr></table></body></html>";
                //    var x = _Emailservc.sendmail(UsrRes.EmailId, subjectt, body);
                //}
                //  BasicHttpBinding binding = new BasicHttpBinding();
                //  binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                //  binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

                //  EndpointAddress endpoint = new EndpointAddress("http://chqerpqas.chq.nnpcgroup.local:8000/sap/bc/srt/scs/sap/zsf_material_master?sap-client=310");
                //  ZMATERIAL_CREATEEXTEND_RFCClient client = new ZMATERIAL_CREATEEXTEND_RFCClient(binding, endpoint);
                //  client.ClientCredentials.UserName.UserName = "TECH1";
                //  client.ClientCredentials.UserName.Password = "India@123";

                // // var x1 = new ZMATERIAL_CREATEEXTEND_RFC();
                //  ZMATERIAL_INPUT[] mulMaterial = new ZMATERIAL_INPUT[1];
                //  var sapfileds = new ZMATERIAL_INPUT();


                ////  mulMaterial[0] = sapfileds;
                //  x1.ZMATERIALINPUT = sapfileds;
                //  var SapResponse = client.ZMATERIAL_CREATEEXTEND_RFC(x1);

                //  var response = SapResponse.ZRETURN;


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
        public string WriteReleaseDataList(List<Prosol_Datamaster> catList, DataTable tbl)
        {
            try
            {
                int cont = 0; string error = "";
                bool res = false;
                string subjectt = "";

                foreach (Prosol_Datamaster mdl in catList)
                {

                    var Qry = Query.EQ("Itemcode", mdl.Itemcode);
                    var erp = _erpRepository.FindOne(Qry);
                    BasicHttpBinding binding = new BasicHttpBinding();
                    binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                    binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
                    //   EndpointAddress endpoint = new EndpointAddress("http://s4hana1909.training.com:8701/sap/bc/srt/rfc/sap/zzmaterial_master_creation_srv/800/zzmaterial_master/zzmat_com");
                    EndpointAddress endpoint = new EndpointAddress("http://s4hana1909.training.com:8005/sap/bc/srt/rfc/sap/zmaterial_create_rfc_service/800/zmaterial_create_rfc_service_tst/zzmat_com");
                    ZMATERIAL_CREATE_RFC_ServiceClient client = new ZMATERIAL_CREATE_RFC_ServiceClient(binding, endpoint);

                    client.ClientCredentials.UserName.UserName = "TEST4";
                    client.ClientCredentials.UserName.Password = "Vline@123";

                    var x1 = new ZMATERIAL_CREATE_RFC();
                    ZMATERIAL_INPUT[] mulMaterial = new ZMATERIAL_INPUT[1];
                    var inputfileds = new ZMATERIAL_INPUT();
                    var plantfileds = new ZMATPLANT_INPUT();
                    var salesfileds = new ZMATSALES_INPUT();
                    var TaxFields = new BAPI_MLAN();
                    var descfileds = new BAPI_MAKT();
                    var sapdescfileds1 = new BAPI_MAKT[1];
                    var sapdescfileds2 = new BAPI_MLAN[2];
                    var sapfileds2 = new BAPIMATINR[1];

                    inputfileds.MATERIAL = "";
                    inputfileds.IND_SECTOR = erp.Industrysector != null ? erp.Industrysector : ""; //M
                    inputfileds.MATL_TYPE = erp.Materialtype != null ? erp.Materialtype : "";
                    inputfileds.BASIC_VIEW = "X";
                    inputfileds.BASE_UOM = mdl.UOM != null ? mdl.UOM : "";
                    // inputfileds.SIZE_DIM = erp.LOTSize != null ? erp.LOTSize : "";
                    inputfileds.MATL_GROUP = erp.MaterialStrategicGroup != null ? erp.MaterialStrategicGroup : "";
                    inputfileds.DIVISION = erp.Division != null ? erp.Division : "";
                    inputfileds.TRANS_GRP = erp.TransportationGroup != null ? erp.TransportationGroup : "";
                    inputfileds.ZPO_TEXT = mdl.Longdesc != null ? mdl.Longdesc : "";
                    inputfileds.ZSALES_TXT = erp.SalesText != null ? erp.SalesText : "";

                    plantfileds.PLANT = erp.Plant != null ? erp.Plant : "";
                    plantfileds.AVAILCHECK = erp.AvailCheck != null ? erp.AvailCheck : "";
                    plantfileds.LOADINGGRP = erp.LoadingGroup != null ? erp.LoadingGroup : "";
                    plantfileds.PROFIT_CTR = erp.ProfitCenter != null ? erp.ProfitCenter : "";
                    plantfileds.PUR_GROUP = erp.PurchasingGroup != null ? erp.PurchasingGroup : "";
                    // plantfileds.AUTO_P_ORD = "Y";
                    // plantfileds.GR_PR_TIME = Convert.ToDecimal(erp.GoodsReceptprocessingTime_) != null ? Convert.ToDecimal(erp.GoodsReceptprocessingTime_) : 0;
                    plantfileds.GR_PR_TIME = 0;
                    plantfileds.MRP_TYPE = erp.MRPType != null ? erp.MRPType : "";
                    plantfileds.MRP_CTRLER = erp.MRPController != null ? erp.MRPController : "";
                    plantfileds.LOTSIZEKEY = erp.LOTSize != null ? erp.LOTSize : "";
                    plantfileds.PROC_TYPE = erp.ProcurementType != null ? erp.ProcurementType : "";
                    plantfileds.PLND_DELRY = 0;
                    plantfileds.STGE_LOC = erp.StorageLocation != null ? erp.StorageLocation : "";
                    plantfileds.STGE_BIN = erp.StorageBin != null ? erp.StorageBin : "";



                    salesfileds.ITEM_CAT = erp.ItemCategoryGroup != null ? erp.ItemCategoryGroup : "";
                    salesfileds.SALES_ORG = erp.SalesOrganization != null ? erp.SalesOrganization : "";
                    salesfileds.DISTR_CHAN = erp.DistributionChannel != null ? erp.DistributionChannel : "";
                    salesfileds.PRICE_CTRL = erp.PriceControl != null ? erp.PriceControl : "";
                    salesfileds.VAL_CLASS = erp.ValuationClass != null ? erp.ValuationClass : "";
                    salesfileds.DELYG_PLNT = erp.DeliveringPlant != null ? erp.DeliveringPlant : "";
                    //  salesfileds.STD_PRICE = Convert.ToDecimal(erp.StandardPrice_);
                    //  salesfileds.SALES_UNIT = erp.Salesunit != null ? erp.Salesunit : "";

                    TaxFields.DEPCOUNTRY = erp.TaxClassificationDep != null ? erp.TaxClassificationDep : "";
                    TaxFields.TAX_TYPE_1 = erp.TaxClassificationType != null ? erp.TaxClassificationType : "";
                    TaxFields.TAXCLASS_1 = erp.TaxClassificationGroup != null ? erp.TaxClassificationGroup : "";

                    sapdescfileds2[0] = TaxFields;

                    TaxFields = new BAPI_MLAN();

                    TaxFields.DEPCOUNTRY = erp.TaxClassificationDep1 != null ? erp.TaxClassificationDep1 : "";
                    TaxFields.TAX_TYPE_1 = erp.TaxClassificationType1 != null ? erp.TaxClassificationType1 : "";
                    TaxFields.TAXCLASS_1 = erp.TaxClassificationGroup1 != null ? erp.TaxClassificationGroup1 : "";

                    sapdescfileds2[1] = TaxFields;

                    descfileds.LANGU = "E";
                    descfileds.MATL_DESC = mdl.Shortdesc;

                    //inputfileds.MATERIAL = "";
                    //inputfileds.IND_SECTOR = "M";
                    //inputfileds.MATL_TYPE = "FERT";
                    //inputfileds.BASE_UOM = "EA";
                    //inputfileds.MATL_GROUP = "CIVIL";
                    //  inputfileds.DIVISION = "20";

                    //plantfileds.PLANT = "1000";
                    //plantfileds.AVAILCHECK = "01";
                    //plantfileds.LOADINGGRP = "0001";
                    //plantfileds.PROFIT_CTR = "PROD";
                    //plantfileds.PUR_GROUP = "001";
                    //// plantfileds.AUTO_P_ORD = "Y";
                    //plantfileds.GR_PR_TIME = 0;
                    //plantfileds.MRP_TYPE = "ND";
                    //plantfileds.MRP_CTRLER = "001";
                    //plantfileds.LOTSIZEKEY = "DY";
                    //plantfileds.PROC_TYPE = "E";
                    //plantfileds.PLND_DELRY = 0;
                    //plantfileds.STGE_LOC = "0001";
                    //plantfileds.STGE_BIN = "006";

                    //salesfileds.SALES_ORG = "1000";
                    //salesfileds.DISTR_CHAN = "10";
                    //salesfileds.DELYG_PLNT = "1000";
                    //salesfileds.ITEM_CAT = "NORM";
                    ////salesfileds.SALES_UNIT = "BG";
                    //// salesfileds.STD_PRICE = 0;
                    //salesfileds.PRICE_CTRL = "S";
                    //salesfileds.VAL_CLASS = "7920";

                    //TaxFields.DEPCOUNTRY = "IN";
                    //TaxFields.TAX_TYPE_1 = "JTX1";
                    //TaxFields.TAXCLASS_1 = "0";

                    //descfileds.LANGU = "E";
                    //descfileds.MATL_DESC = "Test-O27082020";
                    //sapdescfileds2[0] = TaxFields;


                    sapdescfileds1[0] = descfileds;

                    x1.ZMATERIALINPUT = inputfileds;
                    x1.ZMATPLANTINPUT = plantfileds;
                    x1.ZMATSALESINPUT = salesfileds;
                    x1.TAXCLASSIFICATIONS = sapdescfileds2;
                    x1.ZMATDESC = sapdescfileds1;
                    // x1.ZUOM = sapdescfileds2;
                    // var SapResponse = client.ZMATERIAL_CREATE_RFC(x1);

                    // if (SapResponse.ZMATNR != "" && SapResponse != null)
                    // {



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
                    //  oneResult.Materialcode = SapResponse.ZMATNR;

                    res = _DatamasterRepository.Add(oneResult);
                    var query1 = Query.EQ("itemId", mdl.Itemcode);
                    var rejcde = _ProsolRequest.FindOne(query1);

                    if (rejcde != null)
                    {

                        var qryUser = Query.EQ("Userid", rejcde.requester);
                        var UsrRes = _usersRepository.FindOne(qryUser);
                        subjectt = "Material code created for Request: " + rejcde.itemId;
                        var x = _Emailservc.sendmail(UsrRes.EmailId, subjectt, _Emailservc.getmailbody(tbl));
                        //}
                        //else
                        //{
                        //    error = SapResponse.ZRETURN2.MESSAGE + " " + SapResponse.ZRETURN2.MESSAGE_V1 + " " + SapResponse.ZRETURN2.MESSAGE_V2 + " " + SapResponse.ZRETURN2.MESSAGE_V3;
                        //}


                        //var request = (HttpWebRequest)WebRequest.Create("https://kanaiyazhi.com/API/single_indexing_API/index_api.php");
                        //var postData = "itemcode=" + Uri.EscapeDataString(mdl.Itemcode);
                        //postData += "&materialcode=" + Uri.EscapeDataString(mdl.Materialcode);
                        //postData += "&short_desc=" + Uri.EscapeDataString(mdl.Shortdesc);
                        //postData += "&long_desc=" + Uri.EscapeDataString(mdl.Longdesc);
                        //postData += "&noun=" + Uri.EscapeDataString(mdl.Noun);
                        //postData += "&modifier=" + Uri.EscapeDataString(mdl.Modifier);
                        //var data = Encoding.ASCII.GetBytes(postData);
                        //request.Method = "POST";
                        //request.ContentType = "application/x-www-form-urlencoded";
                        //request.ContentLength = data.Length;
                        //using (var stream = request.GetRequestStream())
                        //{
                        //    stream.Write(data, 0, data.Length);
                        //}
                        //var response1 = (HttpWebResponse)request.GetResponse();
                        //var responseString = new StreamReader(response1.GetResponseStream()).ReadToEnd();
                    }

                }



                if (res) error = "Success";

                return error;
            }
            catch (Exception e)
            {
                return e.Message;
            }
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
            var query = Query.And(Query.EQ("Catalogue.UserId", uId), Query.Or(Query.EQ("ItemStatus", sts), Query.EQ("ItemStatus", 13), Query.EQ("ItemStatus", sts1)));
            // var query = Query.And(Query.EQ("Catalogue.UserId", uId), Query.Or(Query.EQ("ItemStatus", sts),Query.EQ("ItemStatus", sts1)));
            var arrResult = _DatamasterRepository.FindAll(query, sort);
            return arrResult;

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
        public IEnumerable<Prosol_Datamaster> GetDataListpv(int pageNumber, int pageSize, string uId)
        {
            var sort = SortBy.Ascending("PVstatus").Descending("UpdatedOn");
            var query = Query.EQ("PVstatus", "Completed");
            var arrResult = _DatamasterRepository.FindAll(query, sort);
            arrResult = arrResult.Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();
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
            var query = Query.Or(Query.EQ("Itemcode", Itemcode), Query.EQ("Materialcode", Itemcode));
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
        //    else return null;checkDuplicate
        //}

        public List<Prosol_Datamaster> checkPartno(string Partno, string icode, string Flag)
        {
            var q = (Query.EQ("Type", Flag));
            var v = _ReftypeRepository.FindOne(q);
            if (v.Islive == true)
            {
                string result = Partno != null ? Regex.Replace(Partno, @"[^\w\d]", "") : "";
                var query = Query.And(Query.And(Query.EQ("Vendorsuppliers.RefNoDup", result), Query.NE("Vendorsuppliers.RefNo", "")), Query.NE("Itemcode", icode));
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
            else return null;
        }

        public List<Prosol_Datamaster> checkDuplicate(Prosol_Datamaster cat)
        {
            if (cat.Vendorsuppliers != null)
            {
                foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
                {
                    if (vs.RefNo != null && vs.RefNo != "")
                        vs.RefNoDup = Regex.Replace(vs.RefNo, @"[^\w\d]", "");

                    if (vs.Name != null && vs.Name != "")
                        vs.Name = vs.Name.Replace(",", "");

                }
            }
            var nmType = _NounModifiService.Getformat(cat.Noun, cat.Modifier);
            cat.Shortdesc = ShortDesc(cat);
            var proCat = new Prosol_Datamaster();
            var LstVendors = new List<Vendorsuppliers>();
            if (cat.Vendorsuppliers != null && cat.Vendorsuppliers.Count > 0)
            {
                foreach (Vendorsuppliers LstAtt in cat.Vendorsuppliers)
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


            //Attribute

            var lstCharateristics = new List<Prosol_AttributeList>();
            if (cat.Characteristics != null)
            {

                foreach (Prosol_AttributeList LstAtt in cat.Characteristics)
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






            proCat._id = cat._id;
            proCat.Itemcode = cat.Itemcode;
            proCat.Noun = cat.Noun;
            proCat.Modifier = cat.Modifier;
            proCat.Partno = cat.Partno;
            proCat.Characteristics = lstCharateristics;
            proCat.Vendorsuppliers = LstVendors;


            //Equipment

            if (cat.Equipment != null)
            {
                var Equi_mdl = new Equipments();
                Equi_mdl.Name = cat.Equipment.Name;
                Equi_mdl.Manufacturer = cat.Equipment.Manufacturer;
                Equi_mdl.Modelno = cat.Equipment.Modelno;
                Equi_mdl.Tagno = cat.Equipment.Tagno;
                Equi_mdl.Serialno = cat.Equipment.Serialno;
                Equi_mdl.Additionalinfo = cat.Equipment.Additionalinfo;
                Equi_mdl.EMS = cat.Equipment.EMS;
                Equi_mdl.ENS = cat.Equipment.ENS;

                proCat.Equipment = Equi_mdl;

            }
        gan:
            var Qry = Query.And(Query.NE("Itemcode", cat.Itemcode), Query.EQ("Shortdesc", cat.Shortdesc), Query.GT("ItemStatus", 0), Query.Or(Query.EQ("Vendorsuppliers", BsonNull.Value), Query.EQ("Vendorsuppliers", new BsonArray())));

            var DaList = _DatamasterRepository.FindAll(Qry).ToList();
            if (DaList != null && DaList.Count > 0)
            {

                foreach (Prosol_Datamaster dm in DaList)
                {
                    if (dm.Characteristics != null && proCat.Characteristics != null)
                    {
                        foreach (Prosol_AttributeList mdol in dm.Characteristics)
                        {
                            int lnx = 0;
                            foreach (Prosol_AttributeList mdl in proCat.Characteristics)
                            {
                                if (mdl.Characteristic == mdol.Characteristic)
                                {
                                    if (mdl.Value == null)
                                        mdl.Value = "";
                                    if (mdol.Value == null)
                                        mdol.Value = "";
                                    if (mdl.Value != mdol.Value && !string.IsNullOrEmpty(mdl.Value) && !string.IsNullOrEmpty(mdol.Value))
                                    {
                                        if (lnx > 1)
                                        {

                                            for (int dx = lnx; dx <= lnx; dx--)
                                            {
                                                if (proCat.Characteristics[dx - 1].Value != "" && proCat.Characteristics[dx - 1].Value != null)
                                                {
                                                    proCat.Characteristics[dx - 1].Value = "";

                                                    break;
                                                }

                                            }

                                            cat.Shortdesc = ShortDesc(proCat);
                                            goto gan;

                                        }


                                    }

                                }
                                lnx++;
                            }
                        }


                    }
                }

            }
            int flg = 0;
            var dupList = new List<Prosol_Datamaster>();
            //12.2.19 block
            if (cat.Vendorsuppliers != null)
            {
                foreach (Vendorsuppliers vsup in cat.Vendorsuppliers)
                {
                    if (vsup.Refflag != null && vsup.Refflag != "" && vsup.Refflag != "REFERENCE NUMBER" && vsup.Refflag != "DRAWING NUMBER" && vsup.Refflag != "POSITION NUMBER" && vsup.s != 0)
                    {

                        if (vsup.RefNo != null && vsup.RefNo != "" && vsup.Refflag != null && vsup.RefNoDup != null && vsup.Refflag != "")
                        {
                            flg = 1;
                            //  var query = Query.And(Query.EQ("Vendorsuppliers.Refflag", vsup.Refflag), Query.EQ("Vendorsuppliers.RefNo", vsup.RefNo));
                            // var query = Query.And(Query.ElemMatch("Vendorsuppliers", Query.And(Query.EQ("Refflag", vsup.Refflag), Query.EQ("RefNo", vsup.RefNo))));


                            // var query = Query.Matches("Vendorsuppliers.RefNo", BsonRegularExpression.Create(new Regex(stringFormat("^{0}", temp), RegexOptions.IgnoreCase)));
                            //if (nmType != null && nmType[0].Formatted > 0)
                            //{
                            var query = Query.And(Query.EQ("Vendorsuppliers.RefNoDup", vsup.RefNoDup), Query.GT("ItemStatus", 0), Query.NE("Itemcode", cat.Itemcode));
                            var vn = _DatamasterRepository.FindAll(query).ToList();
                            if (vn != null && vn.Count > 0)
                            {
                                foreach (Prosol_Datamaster dm in vn)
                                {
                                    if (dm.Itemcode != cat.Itemcode && -1 == dupList.FindIndex(f => f.Itemcode.Equals(dm.Itemcode)))
                                    {
                                        dm.batch = "100";
                                        dupList.Add(dm);

                                    }


                                }

                            }
                            //}else
                            //{
                            //    var query = Query.And(Query.EQ("ItemStatus", 6),Query.EQ("Vendorsuppliers.RefNoDup", vsup.RefNoDup), Query.NE("Itemcode", cat.Itemcode));
                            //    var vn = _DatamasterRepository.FindAll(query).ToList();
                            //    if (vn != null && vn.Count > 0)
                            //    {
                            //        foreach (Prosol_Datamaster dm in vn)
                            //        {
                            //            if (dm.Itemcode != cat.Itemcode && -1 == dupList.FindIndex(f => f.Itemcode.Equals(dm.Itemcode)))
                            //            {
                            //                dupList.Add(dm);

                            //            }


                            //        }

                            //    }

                            //}
                        }
                    }
                }
            }
            //blockend
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
                                var Qry2 = Query.And(Query.NE("Itemcode", cat.Itemcode), Query.GT("ItemStatus", 0), Query.EQ("Vendorsuppliers.Refflag", "POSITION NUMBER"), Query.EQ("Vendorsuppliers.RefNoDup", vsup.RefNoDup), Query.EQ("Equipment.Modelno", cat.Equipment.Modelno));
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
                    if (Lst != null && Lst.Count > 0)
                    {
                        foreach (Prosol_Datamaster dm in Lst)
                        {

                            if (dm.Itemcode != cat.Itemcode && -1 == dupList.FindIndex(f => f.Itemcode.Equals(dm.Itemcode)))
                            {
                                dm.batch = "100";
                                dupList.Add(dm);

                            }


                        }
                    }
                }


                // var query = Query.And(Query.NE("Itemcode", cat.Itemcode),Query.EQ("Shortdesc", cat.Shortdesc), Query.EQ("Noun", cat.Noun));
                // var query = Query.And(Query.NE("Itemcode", cat.Itemcode), Query.EQ("Shortdesc", BsonRegularExpression.Create(new Regex(string.Format("^{0}", cat.Shortdesc), RegexOptions.IgnoreCase))), Query.EQ("Noun", cat.Noun));
                //    var query = Query.And(Query.NE("Itemcode", cat.Itemcode), Query.GT("ItemStatus", 0), Query.EQ("Shortdesc", cat.Shortdesc));
                var query = Query.And(Query.NE("Itemcode", cat.Itemcode), Query.GT("ItemStatus", 0), Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
                // var query = Query.And(Query.NE("Itemcode", cat.Itemcode), Query.All("Shortdesc", new BsonArray(arr)), Query.EQ("Noun", cat.Noun));

                var vn = _DatamasterRepository.FindAll(query).ToList();
                if (vn != null && vn.Count > 0)
                {

                    foreach (Prosol_Datamaster dm in vn)
                    {
                        if (dm.Characteristics != null && cat.Characteristics != null)
                        {
                            int ind = 0; double cout = 0, chrcount = 0;

                            //character checking
                            foreach (Prosol_AttributeList mdol in dm.Characteristics)
                            {
                                foreach (Prosol_AttributeList mdl in cat.Characteristics)
                                {

                                    if (mdl.Characteristic == mdol.Characteristic && ((mdl.Value != null && mdl.Value != "") || (mdol.Value != null && mdol.Value != "")))
                                    {
                                        if (mdl.Value != mdol.Value)
                                        {
                                            ind = 1;

                                        }
                                        else
                                        {
                                            //if ((mdl.Value != null && mdl.Value != "") && (mdol.Value != null && mdol.Value != ""))
                                            //{
                                            //    if (mdl.Value == mdol.Value)
                                            //    {
                                            //       

                                            //    }

                                            //}
                                            cout++;

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
                                            else
                                            {
                                                if (vsup.RefNoDup != null && vsup.RefNoDup != "" && mdl.RefNoDup != null && mdl.RefNoDup == vsup.RefNoDup)
                                                {
                                                    if (vsup.Refflag == "POSITION NUMBER" && vsup.RefNo != null && vsup.RefNo != "" && dm.Equipment != null && dm.Equipment.Modelno != null && dm.Equipment.Modelno != "" && cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "" && dm.Equipment.Modelno != cat.Equipment.Modelno)
                                                    {
                                                        ind = 1;
                                                    }
                                                    else if (mdl.Refflag == "POSITION NUMBER" && mdl.RefNo != null && mdl.RefNo != "" && cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "" && (dm.Equipment == null || dm.Equipment.Modelno == null || dm.Equipment.Modelno == ""))
                                                    {
                                                        ind = 1;
                                                    }
                                                    else if (vsup.Refflag == "POSITION NUMBER" && vsup.RefNo != null && vsup.RefNo != "" && dm.Equipment != null && dm.Equipment.Modelno != null && dm.Equipment.Modelno != "" && (cat.Equipment == null || cat.Equipment.Modelno == null || cat.Equipment.Modelno == ""))
                                                    {
                                                        ind = 1;
                                                    }
                                                }
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


                            if (ind == 0)
                            {
                                if (dm.Itemcode != cat.Itemcode && -1 == dupList.FindIndex(f => f.Itemcode.Equals(dm.Itemcode)))
                                {
                                    dm.batch = "100";
                                    dupList.Add(dm);

                                }

                            }
                            else
                            {

                                if (cout > 0)
                                {
                                    foreach (Prosol_AttributeList mdl in cat.Characteristics)
                                    {
                                        if (mdl.Value != "" && mdl.Value != null)
                                        {
                                            chrcount++;
                                        }
                                    }

                                    // double chrcunt = cat.Characteristics.Count;
                                    var res = ((cout / chrcount) * 100);
                                    Double perc = Math.Round(res);

                                    if (perc > 50)
                                    {
                                        dm.batch = Convert.ToString(perc);
                                        dupList.Add(dm);

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
        }

        public List<Prosol_Datamaster> checkDuplicate1(Prosol_Datamaster cat)
        {
            if (cat.Vendorsuppliers != null)
            {
                foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
                {
                    if (vs.RefNo != null && vs.RefNo != "")
                        vs.RefNoDup = Regex.Replace(vs.RefNo, @"[^\w\d]", "");

                    if (vs.Name != null && vs.Name != "")
                        vs.Name = vs.Name.Replace(",", "");
                }
            }
            var nmType = _NounModifiService.Getformat(cat.Noun, cat.Modifier);
            cat.Shortdesc = ShortDesc(cat);
            var proCat = new Prosol_Datamaster();
            var LstVendors = new List<Vendorsuppliers>();
            if (cat.Vendorsuppliers != null && cat.Vendorsuppliers.Count > 0)
            {
                foreach (Vendorsuppliers LstAtt in cat.Vendorsuppliers)
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


            //Attribute

            var lstCharateristics = new List<Prosol_AttributeList>();
            if (cat.Characteristics != null)
            {

                foreach (Prosol_AttributeList LstAtt in cat.Characteristics)
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






            proCat._id = cat._id;
            proCat.Itemcode = cat.Itemcode;
            proCat.Noun = cat.Noun;
            proCat.Modifier = cat.Modifier;
            proCat.Partno = cat.Partno;
            proCat.Characteristics = lstCharateristics;
            proCat.Vendorsuppliers = LstVendors;


            //Equipment

            if (cat.Equipment != null)
            {
                var Equi_mdl = new Equipments();
                Equi_mdl.Name = cat.Equipment.Name;
                Equi_mdl.Manufacturer = cat.Equipment.Manufacturer;
                Equi_mdl.Modelno = cat.Equipment.Modelno;
                Equi_mdl.Tagno = cat.Equipment.Tagno;
                Equi_mdl.Serialno = cat.Equipment.Serialno;
                Equi_mdl.Additionalinfo = cat.Equipment.Additionalinfo;
                Equi_mdl.EMS = cat.Equipment.EMS;
                Equi_mdl.ENS = cat.Equipment.ENS;

                proCat.Equipment = Equi_mdl;

            }
        //gan:
        //    var Qry = Query.And(Query.NE("Itemcode", cat.Itemcode), Query.EQ("Shortdesc", cat.Shortdesc), Query.GT("ItemStatus", 0), Query.Or(Query.EQ("Vendorsuppliers", BsonNull.Value), Query.EQ("Vendorsuppliers", new BsonArray())));

        //    var DaList = _DatamasterRepository.FindAll(Qry).ToList();
        //    if (DaList != null && DaList.Count > 0)
        //    {

        //        foreach (Prosol_Datamaster dm in DaList)
        //        {
        //            if (dm.Characteristics != null && proCat.Characteristics != null)
        //            {
        //                foreach (Prosol_AttributeList mdol in dm.Characteristics)
        //                {
        //                    int lnx = 0;
        //                    foreach (Prosol_AttributeList mdl in proCat.Characteristics)
        //                    {
        //                        if (mdl.Characteristic == mdol.Characteristic)
        //                        {
        //                            if (mdl.Value == null)
        //                                mdl.Value = "";
        //                            if (mdol.Value == null)
        //                                mdol.Value = "";
        //                            if (mdl.Value != mdol.Value && !string.IsNullOrEmpty(mdl.Value) && !string.IsNullOrEmpty(mdol.Value))
        //                            {
        //                                if (lnx > 1)
        //                                {

        //                                    for (int dx = lnx; dx <= lnx; dx--)
        //                                    {
        //                                        if (proCat.Characteristics[dx - 1].Value != "" && proCat.Characteristics[dx - 1].Value != null)
        //                                        {
        //                                            proCat.Characteristics[dx - 1].Value = "";

        //                                            break;
        //                                        }

        //                                    }

        //                                    cat.Shortdesc = ShortDesc(proCat);
        //                                    goto gan;

        //                                }


        //                            }

        //                        }
        //                        lnx++;
        //                    }
        //                }


        //            }
        //        }

        //    }
            int flg = 0;
            var dupList = new List<Prosol_Datamaster>();
            //12.2.19 block
            if (cat.Vendorsuppliers != null)
            {
                foreach (Vendorsuppliers vsup in cat.Vendorsuppliers)
                {
                    if (vsup.Refflag != null && vsup.Refflag != "" && vsup.Refflag != "REFERENCE NUMBER" && vsup.Refflag != "DRAWING NUMBER" && vsup.Refflag != "POSITION NUMBER" && vsup.s != 0)
                    {

                        if (vsup.RefNo != null && vsup.RefNo != "" && vsup.Refflag != null && vsup.RefNoDup != null && vsup.Refflag != "")
                        {
                            flg = 1;
                            //  var query = Query.And(Query.EQ("Vendorsuppliers.Refflag", vsup.Refflag), Query.EQ("Vendorsuppliers.RefNo", vsup.RefNo));
                            // var query = Query.And(Query.ElemMatch("Vendorsuppliers", Query.And(Query.EQ("Refflag", vsup.Refflag), Query.EQ("RefNo", vsup.RefNo))));


                            // var query = Query.Matches("Vendorsuppliers.RefNo", BsonRegularExpression.Create(new Regex(stringFormat("^{0}", temp), RegexOptions.IgnoreCase)));
                            //if (nmType != null && nmType[0].Formatted > 0)
                            //{
                            var query = Query.And(Query.EQ("Vendorsuppliers.RefNoDup", vsup.RefNoDup), Query.GT("ItemStatus", 0), Query.NE("Itemcode", cat.Itemcode));
                            var vn = _DatamasterRepository.FindAll(query).ToList();
                            if (vn != null && vn.Count > 0)
                            {
                                foreach (Prosol_Datamaster dm in vn)
                                {
                                    if (dm.Itemcode != cat.Itemcode && -1 == dupList.FindIndex(f => f.Itemcode.Equals(dm.Itemcode)))
                                    {
                                        dupList.Add(dm);

                                    }


                                }

                            }
                            //}else
                            //{
                            //    var query = Query.And(Query.EQ("ItemStatus", 6),Query.EQ("Vendorsuppliers.RefNoDup", vsup.RefNoDup), Query.NE("Itemcode", cat.Itemcode));
                            //    var vn = _DatamasterRepository.FindAll(query).ToList();
                            //    if (vn != null && vn.Count > 0)
                            //    {
                            //        foreach (Prosol_Datamaster dm in vn)
                            //        {
                            //            if (dm.Itemcode != cat.Itemcode && -1 == dupList.FindIndex(f => f.Itemcode.Equals(dm.Itemcode)))
                            //            {
                            //                dupList.Add(dm);

                            //            }


                            //        }

                            //    }

                            //}
                        }
                    }
                }
            }
            //blockend
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
                                var Qry2 = Query.And(Query.NE("Itemcode", cat.Itemcode), Query.GT("ItemStatus", 0), Query.EQ("Vendorsuppliers.Refflag", "POSITION NUMBER"), Query.EQ("Vendorsuppliers.RefNoDup", vsup.RefNoDup), Query.EQ("Equipment.Modelno", cat.Equipment.Modelno));
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
                    if (Lst != null && Lst.Count > 0)
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


                // var query = Query.And(Query.NE("Itemcode", cat.Itemcode),Query.EQ("Shortdesc", cat.Shortdesc), Query.EQ("Noun", cat.Noun));
                // var query = Query.And(Query.NE("Itemcode", cat.Itemcode), Query.EQ("Shortdesc", BsonRegularExpression.Create(new Regex(string.Format("^{0}", cat.Shortdesc), RegexOptions.IgnoreCase))), Query.EQ("Noun", cat.Noun));
                var query = Query.And(Query.NE("Itemcode", cat.Itemcode), Query.GT("ItemStatus", 0), Query.EQ("Shortdesc", cat.Shortdesc));
                // var query = Query.And(Query.NE("Itemcode", cat.Itemcode), Query.All("Shortdesc", new BsonArray(arr)), Query.EQ("Noun", cat.Noun));

                var vn = _DatamasterRepository.FindAll(query).ToList();
                if (vn != null && vn.Count > 0)
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
                                            break;
                                        }

                                    }

                                }
                                if (ind == 1)
                                    break;
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
                                            else
                                            {
                                                if (vsup.RefNoDup != null && vsup.RefNoDup != "" && mdl.RefNoDup != null && mdl.RefNoDup == vsup.RefNoDup)
                                                {
                                                    if (vsup.Refflag == "POSITION NUMBER" && vsup.RefNo != null && vsup.RefNo != "" && dm.Equipment != null && dm.Equipment.Modelno != null && dm.Equipment.Modelno != "" && cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "" && dm.Equipment.Modelno != cat.Equipment.Modelno)
                                                    {
                                                        ind = 1;
                                                    }
                                                    else if (mdl.Refflag == "POSITION NUMBER" && mdl.RefNo != null && mdl.RefNo != "" && cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "" && (dm.Equipment == null || dm.Equipment.Modelno == null || dm.Equipment.Modelno == ""))
                                                    {
                                                        ind = 1;
                                                    }
                                                    else if (vsup.Refflag == "POSITION NUMBER" && vsup.RefNo != null && vsup.RefNo != "" && dm.Equipment != null && dm.Equipment.Modelno != null && dm.Equipment.Modelno != "" && (cat.Equipment == null || cat.Equipment.Modelno == null || cat.Equipment.Modelno == ""))
                                                    {
                                                        ind = 1;
                                                    }
                                                }
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

        //    if (NMList != null && NMList.Count > 0 && NMList.Formatted == 1)
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


        //        if (NMList != null && NMList.Count > 0 && NMList.Formatted == 0)
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

        public bool Reworkreview(string itemcode, string RevRemarks)
        {
            var res = false;
            var query = Query.EQ("Itemcode", itemcode);
            var vn = _DatamasterRepository.FindOne(query);
            if (vn != null)
            {
                vn.ItemStatus = 0;
                vn.Reworkcat = "rev";
                //vn.RevRemarks = RevRemarks;
                vn.RelRemarks = RevRemarks;
                // vn.Review = null;               
                res = _DatamasterRepository.Add(vn);

            }
            return res;

        }
        public bool Reworkrelease(string itemcode, string RelRemarks)
        {
            var res = false;
            var query = Query.EQ("Itemcode", itemcode);
            var vn = _DatamasterRepository.FindOne(query);
            if (vn != null)
            {
                vn.ItemStatus = 2;
                vn.Rework = "rel";
                vn.RelRemarks = RelRemarks;
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

            //var Qry = Query.EQ("Attribute", Attribute);
            //var AttributeList = _attributeRepository.FindOne(Qry);

            //if (AttributeList != null && AttributeList.ValueList != null)
            //{
            //    var Lst = new List<ObjectId>();
            //    string[] strArr = { "Value" };
            //    var fields = Fields.Include(strArr).Exclude("_id");
            //    foreach (string str in AttributeList.ValueList)
            //    {
            //        Lst.Add(new ObjectId(str));
            //    }
            //    var query = Query.In("_id", new BsonArray(Lst));
            //    var arrResult = _abbreivateRepository.FindAll(fields, query);
            //    foreach (Prosol_Abbrevate mdl in arrResult)
            //    {
            //        Lst1.Add(mdl.Value);

            //    }
            //}
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
                        if(str.StartsWith("6"))
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
            var condit = ""; string pattern = " X ", pattern1 = " x ";
            if (Value.Contains(','))
            {
                var Qry = Query.EQ("Value", Value);
                var mobj = _abbreivateRepository.FindOne(Qry);
                if (mobj != null && !string.IsNullOrEmpty(mobj.Abbrevated))
                {
                    condit = condit + mobj.Abbrevated;
                }
                else
                {
                    string[] val = Value.Split(',');

                    foreach (string str in val)
                    {
                        var mCol = Regex.Matches(str, pattern);
                        if (mCol.Count == 0)
                            mCol = Regex.Matches(str, pattern1);

                        if (mCol.Count == 0 && str.Contains(' '))
                        {
                            var query = Query.EQ("Value", str);
                            var obj = _abbreivateRepository.FindOne(query);
                            if (obj != null && !string.IsNullOrEmpty(obj.Abbrevated))
                            {
                                condit = condit + obj.Abbrevated;
                            }
                            else
                            {
                                string[] valsp = str.Split(' ');
                                foreach (string strsp in valsp)
                                {
                                    query = Query.EQ("Value", strsp);
                                    obj = _abbreivateRepository.FindOne(query);
                                    if (obj != null)
                                        condit = obj != null && !string.IsNullOrEmpty(obj.Abbrevated) ? condit + obj.Abbrevated + " " : condit + obj.Value + " ";
                                }
                                condit = condit.TrimEnd(' ');
                            }


                        }
                        else
                        {
                            var query = Query.EQ("Value", str);
                            var obj = _abbreivateRepository.FindOne(query);
                            if (obj != null && !string.IsNullOrEmpty(obj.Abbrevated))
                            {
                                condit = condit + obj.Abbrevated;
                            }
                            //else
                            //{
                            //    condit = condit + obj.Value;
                            //}
                        }
                        condit = condit + ',';
                    }
                    condit = condit.TrimEnd(',');
                }
            }
            else if (Value.Contains(' '))
            {
                var mCol = Regex.Matches(Value, pattern);
                if (mCol.Count == 0)
                    mCol = Regex.Matches(Value, pattern1);
                if (mCol.Count == 0)
                {
                    var query = Query.EQ("Value", Value);
                    var obj = _abbreivateRepository.FindOne(query);
                    if (obj != null && !string.IsNullOrEmpty(obj.Abbrevated))
                    {
                        condit = condit + obj.Abbrevated;
                    }
                    else
                    {
                        string[] valsp = Value.Split(' ');
                        foreach (string strsp in valsp)
                        {
                            query = Query.EQ("Value", strsp);
                            obj = _abbreivateRepository.FindOne(query);
                            if (obj != null)
                                condit = !string.IsNullOrEmpty(obj.Abbrevated) ? condit + obj.Abbrevated + " " : condit + obj.Value + " ";
                        }
                        condit = condit.TrimEnd(' ');
                    }
                }
                else
                {
                    string B = "false";
                    var query = Query.EQ("Value", Value);
                    var obj = _abbreivateRepository.FindOne(query);
                    if (obj != null)
                    {

                        return obj.Abbrevated;

                    }
                    return B;
                }

            }
            else
            {
                string B = "false";
                var query = Query.EQ("Value", Value);
                var obj = _abbreivateRepository.FindOne(query);
                if (obj != null)
                {

                    return obj.Abbrevated;

                }
                return B;
            }

            return condit;
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
        public string valValidate(string Noun, string Modifier, string Attribute, string Value)
        {
            var condit = ""; string pattern = " X ", pattern1 = " x ";
            if (Value.Contains(','))
            {
                var Qry = Query.EQ("Value", Value);
                var mobj = _abbreivateRepository.FindOne(Qry);
                if (mobj != null && !string.IsNullOrEmpty(mobj.Abbrevated))
                {
                    condit = "";
                }
                else
                {
                    string[] val = Value.Split(',');

                    foreach (string str in val)
                    {
                        var mCol = Regex.Matches(str, pattern);
                        if (mCol.Count == 0)
                            mCol = Regex.Matches(str, pattern1);

                        if (mCol.Count == 0 && str.Contains(' '))
                        {
                            var query = Query.EQ("Value", str);
                            var obj = _abbreivateRepository.FindOne(query);
                            if (obj != null && !string.IsNullOrEmpty(obj.Abbrevated))
                            {
                                condit = "";
                            }
                            else
                            {
                                string[] valsp = str.Split(' ');
                                foreach (string strsp in valsp)
                                {
                                    query = Query.EQ("Value", strsp);
                                    obj = _abbreivateRepository.FindOne(query);
                                    if (obj != null)
                                        condit = "";
                                    else
                                    {
                                        condit = "false";
                                        break;
                                    }
                                }
                                if (condit == "false")
                                    break;
                            }


                        }
                        else
                        {
                            var query = Query.EQ("Value", str);
                            var obj = _abbreivateRepository.FindOne(query);
                            if (obj != null && !string.IsNullOrEmpty(obj.Abbrevated))
                                condit = "";
                            else
                            {
                                condit = "false";
                                break;
                            }

                        }

                    }
                }

            }
            else if (Value.Contains(' '))
            {
                var mCol = Regex.Matches(Value, pattern);
                if (mCol.Count == 0)
                    mCol = Regex.Matches(Value, pattern1);
                if (mCol.Count == 0)
                {
                    var query = Query.EQ("Value", Value);
                    var obj = _abbreivateRepository.FindOne(query);
                    if (obj != null && !string.IsNullOrEmpty(obj.Abbrevated))
                    {
                        condit = "";
                    }
                    else
                    {
                        string[] valsp = Value.Split(' ');
                        foreach (string strsp in valsp)
                        {
                            query = Query.EQ("Value", strsp);
                            obj = _abbreivateRepository.FindOne(query);
                            if (obj != null)
                                condit = "";
                            else
                            {
                                condit = "false";
                                break;
                            }
                        }

                    }
                }
                else
                {

                    var query = Query.EQ("Value", Value);
                    var obj = _abbreivateRepository.FindOne(query);
                    if (obj != null)
                        condit = "";
                    else
                    {
                        condit = "false";

                    }

                }

            }
            else
            {

                var query = Query.EQ("Value", Value);
                var obj = _abbreivateRepository.FindOne(query);
                if (obj != null)
                    condit = "";
                else
                {
                    condit = "false";

                }

            }

            return condit;
        }

        public string CheckValApprove(string Value)
        {
            var condit = "";
            //var condit = ""; string pattern = " X ", pattern1 = " x ";
            //if (Value.Contains(','))
            //{
            //    var Qry = Query.EQ("Value", Value);
            //    var mobj = _abbreivateRepository.FindOne(Qry);
            //    if (mobj != null && !string.IsNullOrEmpty(mobj.Abbrevated))
            //    {
            //        if (mobj.Approved == "Yes")
            //            condit = mobj.Approved;
            //        else
            //        {
            //            condit = "false";

            //        }
            //    }
            //    else
            //    {
            //        string[] val = Value.Split(',');

            //        foreach (string str in val)
            //        {
            //            var mCol = Regex.Matches(str, pattern);
            //            if (mCol.Count == 0)
            //                mCol = Regex.Matches(str, pattern1);

            //            if (mCol.Count == 0 && str.Contains(' '))
            //            {
            //                var query = Query.EQ("Value", str);
            //                var obj = _abbreivateRepository.FindOne(query);
            //                if (obj != null)
            //                {
            //                    if (obj.Approved == "Yes")
            //                        condit = obj.Approved;
            //                    else
            //                    {
            //                        condit = "false";
            //                        break;
            //                    }
            //                }
            //                else
            //                {
            //                    string[] valsp = str.Split(' ');
            //                    foreach (string strsp in valsp)
            //                    {
            //                        query = Query.EQ("Value", strsp);
            //                        obj = _abbreivateRepository.FindOne(query);
            //                        if (obj != null)
            //                            condit = obj.Value;
            //                        else
            //                        {
            //                            condit = "false";
            //                            break;
            //                        }
            //                    }
            //                    if (condit == "false")
            //                        break;
            //                }


            //            }
            //            else
            //            {
            //                var query = Query.EQ("Value", str);
            //                var obj = _abbreivateRepository.FindOne(query);
            //                if (obj != null)
            //                {
            //                    if (obj.Approved == "Yes")
            //                        condit = obj.Approved;
            //                    else
            //                    {
            //                        condit = "false";
            //                        break;
            //                    }
            //                }
            //                else condit = "";

            //            }

            //        }
            //    }

            //}
            //else if (Value.Contains(' '))
            //{
            //    var mCol = Regex.Matches(Value, pattern);
            //    if (mCol.Count == 0)
            //        mCol = Regex.Matches(Value, pattern1);
            //    if (mCol.Count == 0)
            //    {
            //        var query = Query.EQ("Value", Value);
            //        var obj = _abbreivateRepository.FindOne(query);
            //        if (obj != null)
            //        {
            //            if (obj.Approved == "Yes")
            //                condit = obj.Approved;
            //            else
            //            {
            //                condit = "false";

            //            }
            //        }
            //        else
            //        {

            //            string[] valsp = Value.Split(' ');
            //            foreach (string strsp in valsp)
            //            {
            //                query = Query.EQ("Value", strsp);
            //                obj = _abbreivateRepository.FindOne(query);
            //                if (obj != null)
            //                {
            //                    if (obj.Approved == "Yes")
            //                        condit = obj.Approved;
            //                    else
            //                    {
            //                        condit = "false";
            //                        break;
            //                    }
            //                }
            //                else condit = "";
            //            }
            //        }
            //    }
            //    else
            //    {

            //        var query = Query.EQ("Value", Value);
            //        var obj = _abbreivateRepository.FindOne(query);
            //        if (obj != null)
            //        {
            //            if (obj.Approved == "Yes")
            //                condit = obj.Approved;
            //            else
            //            {
            //                condit = "false";
            //            }
            //        }
            //        else condit = "";

            //    }

            //}
            //else
            //{

            var query = Query.EQ("Value", Value);
            var obj = _abbreivateRepository.FindOne(query);
            if (obj != null)
            {
                if (obj.Approved == "Yes")
                    condit = obj.Approved;
                else
                {
                    condit = "false";
                }
            }
            else condit = "";

            // }

            return condit;


        }
        public string CheckValue1(string Noun, string Modifier, string Attribute, string Value)
        {

            string B = "false";
            var query = Query.EQ("Value", Value);
            var obj = _abbreivateRepository.FindOne(query);
            if (obj != null)
            {
                int flg = 0;
                string[] sAr = { obj._id.ToString() };
                var Qry1 = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.EQ("Characteristic", Attribute), Query.In("Values", new BsonArray(sAr)));
                var TemplateMaster = _CharacteristicRepository.FindOne(Qry1);
                if (TemplateMaster != null)
                {
                    return "true";
                }
                else
                    return "false";

            }
            else return B;


        }
        public bool AddValue(string Noun, string Modifier, string Attribute, string Value, string abb, string user, string role)
        {
            // var query = Query.And(Query.EQ("Value", Value), Query.EQ("Abbrevated", abb));
            var query = Query.EQ("Value", Value);
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
                if (role == "Yes")
                {
                    newobj.Approved = role;
                }
                else
                {
                    newobj.Approved = "No";
                }

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
        public bool ApproveVal(string Noun, string Modifier, string Attribute, string Value, string abb, string user)
        {
            var query = Query.And(Query.EQ("Value", Value), Query.EQ("Abbrevated", abb));
            var obj = _abbreivateRepository.FindOne(query);
            if (obj != null)
            {
                obj.Approved = "Yes";
                obj.ApprovedBy = user;
                obj.ApprovedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                _abbreivateRepository.Add(obj);


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
                newobj.Approved = "Yes";
                newobj.User = user;
                newobj.ApprovedBy = user;
                newobj.ApprovedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                newobj.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

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

        public IEnumerable<Prosol_UNSPSC> getCOMMCOMMList(string sKey)
        {

            //string[] str = { "Class", "ClassTitle", "Commodity", "CommodityTitle" };
            //var fields = Fields.Include(str);
            var qry1 = Query.Or(Query.Matches("CommodityTitle", BsonRegularExpression.Create(new Regex(sKey.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))), Query.EQ("Commodity", sKey),
                 Query.Matches("ClassTitle", BsonRegularExpression.Create(new Regex(sKey.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))), Query.EQ("Class", sKey),
                 Query.Matches("FamilyTitle", BsonRegularExpression.Create(new Regex(sKey.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))), Query.EQ("Family", sKey),
                 Query.Matches("SegmentTitle", BsonRegularExpression.Create(new Regex(sKey.TrimStart().TrimEnd(), RegexOptions.IgnoreCase))), Query.EQ("Segment", sKey));

            //var Result = _unspsclistRepository.FindAll(fields, qry1).ToList();
            var Result = _unspsclistRepository.FindAll(qry1).ToList();
            return Result;
        }

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
            var sort = SortBy.Descending("_id");
            var query = Query.Matches("Materialcode", BsonRegularExpression.Create(new Regex(LogicCode)));
            var Materialcode = _DatamasterRepository.FindAll(query, sort).ToList();
            if (Materialcode != null && Materialcode.Count > 0)
            {
                code = Materialcode[0].Materialcode;
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

        public bool GetCodeForRejectedItems(Prosol_Datamaster cat, string userid, string username, DataTable tbl)
        {
            var query = Query.EQ("Itemcode", cat.Itemcode);
            var vn = _DatamasterRepository.FindAll(query).ToList();

            Prosol_UpdatedBy userobj = new Prosol_UpdatedBy();
            Prosol_Datamaster pd = vn[0];
            pd.ItemStatus = -1;
            pd.Remarks = cat.Remarks;
            pd.RevRemarks = cat.RevRemarks;
            pd.Noun = cat.Noun;
            pd.Modifier = cat.Modifier;
            pd.Characteristics = cat.Characteristics;
            pd.Vendorsuppliers = cat.Vendorsuppliers;
            pd.Additionalinfo = cat.Additionalinfo;
            pd.Equipment = cat.Equipment;
            userobj.UserId = userid;
            userobj.Name = username;
            userobj.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            pd.Catalogue = userobj;
            pd.Review = null;
            pd.Release = null;
            //pd.Materialcode = null;

            //Prosol_UpdatedBy pu = new Prosol_UpdatedBy();
            //pu.Name = username;
            //pu.UserId = userid;
            //pu.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //pd.RejectedBy = pu;

            _DatamasterRepository.Add(pd);

            //var q = Query.EQ("itemId", vn[0].Itemcode);
            //var up = Update.Set("reason_rejection", vn[0].RevRemarks).Set("itemStatus", "clarification").Set("rejectedOn", DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));
            //var flg = UpdateFlags.Upsert;
            //_ProsolRequest.Update(q, up, flg);

            //var query1 = Query.EQ("itemId", vn[0].Itemcode);
            //var rejcde = _ProsolRequest.FindAll(query1).ToList();
            //var qryUser = Query.EQ("Userid", rejcde[0].requester);
            //var UsrRes = _usersRepository.FindOne(qryUser);
            //string subjectt = "Item Need Clarification";

            //string body = "<html><body><table><tr><td style='padding-left: 50px;padding-top: 10px;'>Request Id</td><td style='padding-left: 50px;padding-top: 10px;'>" + vn[0].Itemcode + "</td></tr><tr><td style='padding-left: 50px;padding-top: 10px;'>Clarification remarks</td><td style='padding-left: 50px;padding-top: 10px;'>" + vn[0].Remarks + "</td></tr></table></body></html>";
            //    string body = vn[0].Itemcode + " requested by you has been Need Clarification";
            //var x = _Emailservc.sendmail(UsrRes.EmailId, subjectt, _Emailservc.getmailbody(tbl));

            //var query1 = Query.EQ("itemId", Itemcode);

            //var rejcde = _ProsolRequest.FindAll(query1).ToList();

            //if (rejcde != null && rejcde.Count > 0)
            //{
            //    //string str = DateTime.Parse(rejcde[0].rejectedOn.ToString()).ToString("dd/MM/yyyy");
            //    rejcde[0].rejectedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //    rejcde[0].itemStatus = "rejected";
            //    _ProsolRequest.Add(rejcde[0]);

            // //   var qryUser = Query.EQ("Userid", rejcde[0].requester);
            // //   var UsrRes = _usersRepository.FindOne(qryUser);

            //  //  qryUser = Query.EQ("Userid", rejcde[0].approver);
            // //   var Usrapp = _usersRepository.FindOne(qryUser);

            //  ///  _Emailservc.sendmail(UsrRes.EmailId, "Item rejection", Itemcode + " requested by you has been rejected");


            // //   _Emailservc.sendmail(Usrapp.EmailId, "Item rejection", Itemcode + " approved by you has been rejected");


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
            Prosol_UpdatedBy pu = new Prosol_UpdatedBy();
            pu.Name = username;
            pu.UserId = userid;
            pu.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            pd.RejectedBy = pu;
            pd.Materialcode = null;

            _DatamasterRepository.Add(pd);
            var tbl = new DataTable();
            tbl.Columns.Add("Item Code");
            tbl.Columns.Add("Legacy");
            tbl.Columns.Add("Clarification Remarks");
            var row = tbl.NewRow();
            row["Item Code"] = Itemcode;


            row["Legacy"] = pd.Legacy;
            row["Clarification Remarks"] = pd.RevRemarks;
            tbl.Rows.Add(row);

            //var q = Query.EQ("itemId", vn[0].Itemcode);
            //var up = Update.Set("reason_rejection", vn[0].RevRemarks).Set("itemStatus", "clarification").Set("rejectedOn", DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));
            //var flg = UpdateFlags.Upsert;
            //_ProsolRequest.Update(q, up, flg);
            //var query1 = Query.EQ("itemId", vn[0].Itemcode);
            //var rejcde = _ProsolRequest.FindAll(query1).ToList();
            var qryUser = Query.EQ("Userid", userid);
            var UsrRes = _usersRepository.FindOne(qryUser);
            string subjectt = "Item Need Clarification";
            var x = _Emailservc.sendmail(UsrRes.EmailId, subjectt, _Emailservc.getmailbody(tbl));
            return true;
        }
        public bool GetCodeForRejectedItems2(string Itemcode, string RelRemarks, string userid, string username)
        {
            var query = Query.EQ("Itemcode", Itemcode);
            var vn = _DatamasterRepository.FindAll(query).ToList();
            Prosol_Datamaster pd = vn[0];
            pd.ItemStatus = -1;
            pd.RelRemarks = RelRemarks;
            Prosol_UpdatedBy pu = new Prosol_UpdatedBy();
            pu.Name = username;
            pu.UserId = userid;
            pu.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            pd.RejectedBy = pu;

            _DatamasterRepository.Add(pd);
            var tbl = new DataTable();
            tbl.Columns.Add("Item Code");
            tbl.Columns.Add("Legacy");
            tbl.Columns.Add("Clarification Remarks");
            var row = tbl.NewRow();
            row["Item Code"] = Itemcode;


            row["Legacy"] = pd.Legacy;
            row["Clarification Remarks"] = pd.RelRemarks;
            tbl.Rows.Add(row);
            var q = Query.EQ("itemId", vn[0].Itemcode);
            var query1 = Query.EQ("itemId", vn[0].Itemcode);
            var rejcde = _ProsolRequest.FindAll(query1).ToList();
            var qryUser = Query.EQ("Userid", rejcde[0].requester);
            string subjectt = "Item Need Clarification";
            var UsrRes = _usersRepository.FindOne(qryUser);
            var up = Update.Set("reason_rejection", vn[0].RelRemarks).Set("itemStatus", "clarification").Set("rejectedOn", DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));
            var flg = UpdateFlags.Upsert;
            _ProsolRequest.Update(q, up, flg);
            var x = _Emailservc.sendmail(UsrRes.EmailId, subjectt, _Emailservc.getmailbody(tbl));
            return true;
        }

        public List<Prosol_Datamaster> searchMaster(string sCode, string sSource, string sShort, string sLong, string sNoun, string sModifier, string sUser, string sType, string sStatus)
        {
            string[] strArr = { "Itemcode", "Materialcode", "Legacy", "Noun", "Modifier", "Shortdesc", "Longdesc", "ItemStatus", "Catalogue", "Review", "Release", "Rework", "Reworkcat", "Junk", "Stock_Status", "PVuser" };
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
                                        if (mdl.SearchColumn == "Materialcode")
                                        {
                                            if (flgg == 0)
                                            {
                                                query1 = (from x in newResultList where x.Materialcode.Contains(str) select x).ToList();
                                            }
                                            if (flgg == 1)
                                            {
                                                query1 = query1.Where(x => x.Materialcode.Contains(str)||x.Itemcode.Contains(str)).ToList();

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
                                            newResultList = newResultList.Where(x => x.Materialcode.ToLower().StartsWith(mdl.SearchKey) || x.Itemcode.ToLower().StartsWith(mdl.SearchKey)).ToList();
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
                                            newResultList = newResultList.Where(x => x.Materialcode.ToLower().StartsWith(mdl.SearchKey) || x.Itemcode.ToLower().StartsWith(mdl.SearchKey)).ToList();
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
                                    newResultList = newResultList.Where(x => x.Materialcode.ToLower().StartsWith(mdl.SearchKey) || x.Itemcode.ToLower().StartsWith(mdl.SearchKey)).ToList();
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
                                    else if (sStatus == "PV Completed")
                                    {
                                        sta = 13; sta1 = 13;
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

        //public List<Prosol_Datamaster> searchMaster(string sCode, string sSource, string sShort, string sLong, string sNoun, string sModifier, string sUser, string sType, string sStatus)
        //{
        //    string[] strArr = { "Itemcode", "Materialcode", "Legacy", "Noun", "Modifier", "Shortdesc", "Longdesc", "ItemStatus", "Catalogue", "Review", "Release", "Rework", "Reworkcat", "Junk" };
        //    List<searchObj> LstObj = new List<searchObj>();
        //    searchObj sObj = new searchObj();
        //    sObj.SearchColumn = "Itemcode";
        //    sObj.SearchKey = sCode;
        //    LstObj.Add(sObj);
        //    sObj.SearchColumn = "Materialcode";
        //    sObj.SearchKey = sCode;
        //    LstObj.Add(sObj);
        //    sObj = new searchObj();
        //    sObj.SearchColumn = "Legacy";
        //    sObj.SearchKey = sSource;
        //    LstObj.Add(sObj);
        //    sObj = new searchObj();
        //    sObj.SearchColumn = "Shortdesc";
        //    sObj.SearchKey = sShort;
        //    LstObj.Add(sObj);
        //    sObj = new searchObj();
        //    sObj.SearchColumn = "Longdesc";
        //    sObj.SearchKey = sLong;
        //    LstObj.Add(sObj);

        //    sObj = new searchObj();
        //    sObj.SearchColumn = "Noun";
        //    sObj.SearchKey = sNoun;
        //    LstObj.Add(sObj);

        //    sObj = new searchObj();
        //    sObj.SearchColumn = "Modifier";
        //    sObj.SearchKey = sModifier;
        //    LstObj.Add(sObj);

        //    sObj = new searchObj();
        //    sObj.SearchColumn = "sUser";
        //    sObj.SearchKey = sUser;
        //    LstObj.Add(sObj);

        //    sObj = new searchObj();
        //    sObj.SearchColumn = "Junk";
        //    sObj.SearchKey = sType;

        //    LstObj.Add(sObj);
        //    sObj = new searchObj();
        //    sObj.SearchColumn = "sStatus";
        //    sObj.SearchKey = sStatus;
        //    LstObj.Add(sObj);

        //    List<Prosol_Datamaster> newResultList = new List<Prosol_Datamaster>();
        //    //  List<Prosol_Datamaster> RemoveResultList = new List<Prosol_Datamaster>();
        //    int flg = 0;
        //    foreach (searchObj mdl in LstObj)
        //    {
        //        if (mdl.SearchKey != "" && mdl.SearchKey != null && mdl.SearchKey != "null" && mdl.SearchKey != "undefined")
        //        {
        //            if (flg == 0)
        //            {
        //                List<Prosol_Datamaster> sResult = new List<Prosol_Datamaster>();
        //                if (mdl.SearchColumn == "sUser")
        //                {
        //                    sResult = UserSearch(strArr, mdl.SearchKey);

        //                }
        //                else if (mdl.SearchColumn == "sStatus")
        //                {
        //                    sResult = StatusSearch(strArr, mdl.SearchKey);
        //                }
        //                else
        //                {
        //                    sResult = SearchFn(strArr, mdl.SearchColumn, mdl.SearchKey);
        //                }
        //                if (sResult != null && sResult.Count > 0)
        //                {

        //                    foreach (Prosol_Datamaster pmdl in sResult)
        //                    {
        //                        newResultList.Add(pmdl);
        //                        flg = 1;
        //                        //  RemoveResultList.Add(pmdl);
        //                    }

        //                }
        //                else break;
        //            }
        //            else
        //            {
        //                //  var sResult = SearchFn(strArr, mdl.SearchColumn, mdl.SearchKey);
        //                if (mdl.SearchKey.Contains("*"))
        //                {
        //                    string[] splt = mdl.SearchKey.Split('*');
        //                    if (splt.Length > 2)
        //                    {
        //                        //contains                              
        //                        foreach (string str in splt)
        //                        {
        //                            if (str != "")
        //                            {
        //                                int flgg = 0;
        //                                var query1 = new List<Prosol_Datamaster>();
        //                                if (mdl.SearchColumn == "Itemcode")
        //                                {
        //                                    if (flgg == 0)
        //                                    {
        //                                        query1 = (from x in newResultList where x.Materialcode.Contains(str) select x).ToList();
        //                                    }
        //                                    if (flgg == 1)
        //                                    {
        //                                        query1 = query1.Where(x => x.Materialcode.Contains(str)).ToList();

        //                                    }
        //                                }
        //                                if (mdl.SearchColumn == "Legacy")
        //                                {
        //                                    if (flgg == 0)
        //                                    {
        //                                        query1 = (from x in newResultList where x.Legacy.Contains(str) select x).ToList();
        //                                    }
        //                                    if (flgg == 1)
        //                                    {
        //                                        query1 = query1.Where(x => x.Legacy.Contains(str)).ToList();

        //                                    }

        //                                    // newResultList = (from x in newResultList where x.Legacy.Contains(str) select x).ToList();
        //                                }
        //                                if (mdl.SearchColumn == "Shortdesc")
        //                                {
        //                                    if (flgg == 0)
        //                                    {
        //                                        query1 = (from x in newResultList where x.Shortdesc.Contains(str) select x).ToList();
        //                                    }
        //                                    if (flgg == 1)
        //                                    {
        //                                        query1 = query1.Where(x => x.Shortdesc.Contains(str)).ToList();

        //                                    }
        //                                    // newResultList = (from x in newResultList where x.Shortdesc.Contains(str) select x).ToList();
        //                                }
        //                                if (mdl.SearchColumn == "Longdesc")
        //                                {
        //                                    if (flgg == 0)
        //                                    {
        //                                        query1 = (from x in newResultList where x.Longdesc.Contains(str) select x).ToList();
        //                                    }
        //                                    if (flgg == 1)
        //                                    {
        //                                        query1 = query1.Where(x => x.Longdesc.Contains(str)).ToList();

        //                                    }
        //                                    // newResultList = (from x in newResultList where x.Longdesc.Contains(str) select x).ToList();
        //                                }
        //                                newResultList = query1;
        //                                flgg = 1;
        //                            }
        //                        }

        //                    }
        //                    else
        //                    {
        //                        if (mdl.SearchKey != "" && mdl.SearchKey != null && mdl.SearchKey != "null" && mdl.SearchKey != "undefined")
        //                        {
        //                            if (sCode.IndexOf('*') > 0)
        //                            {

        //                                //Start with

        //                                if (mdl.SearchColumn == "Materialcode")
        //                                    newResultList = (from x in newResultList where x.Materialcode.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();
        //                                if (mdl.SearchColumn == "Legacy")
        //                                    newResultList = (from x in newResultList where x.Legacy.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();
        //                                if (mdl.SearchColumn == "Shortdesc")
        //                                    newResultList = (from x in newResultList where x.Shortdesc.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();
        //                                if (mdl.SearchColumn == "Longdesc")
        //                                    newResultList = (from x in newResultList where x.Longdesc.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();

        //                            }
        //                            else
        //                            {
        //                                //End with

        //                                if (mdl.SearchColumn == "Materialcode")
        //                                    newResultList = (from x in newResultList where x.Materialcode.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();
        //                                if (mdl.SearchColumn == "Legacy")
        //                                    newResultList = (from x in newResultList where x.Legacy.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();
        //                                if (mdl.SearchColumn == "Shortdesc")
        //                                    newResultList = (from x in newResultList where x.Shortdesc.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();
        //                                if (mdl.SearchColumn == "Longdesc")
        //                                    newResultList = (from x in newResultList where x.Longdesc.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();


        //                            }
        //                        }

        //                    }

        //                }
        //                else
        //                {
        //                    if (mdl.SearchKey != "" && mdl.SearchKey != null && mdl.SearchKey != "null" && mdl.SearchKey != "undefined")
        //                    {
        //                        if (mdl.SearchColumn == "Materialcode")
        //                            newResultList = (from x in newResultList where x.Materialcode != null && x.Itemcode.Equals(mdl.SearchKey) select x).ToList();
        //                        if (mdl.SearchColumn == "Legacy")
        //                            newResultList = (from x in newResultList where x.Legacy != null && x.Legacy.Equals(mdl.SearchKey) select x).ToList();
        //                        if (mdl.SearchColumn == "Shortdesc")
        //                            newResultList = (from x in newResultList where x.Shortdesc != null && x.Shortdesc.Equals(mdl.SearchKey) select x).ToList();
        //                        if (mdl.SearchColumn == "Longdesc")
        //                            newResultList = (from x in newResultList where x.Longdesc != null && x.Longdesc.Equals(mdl.SearchKey) select x).ToList();
        //                        if (mdl.SearchColumn == "Noun")
        //                            newResultList = (from x in newResultList where x.Noun != null && x.Noun.Equals(mdl.SearchKey) select x).ToList();
        //                        if (mdl.SearchColumn == "Modifier")
        //                            newResultList = (from x in newResultList where x.Modifier != null && x.Modifier.Equals(mdl.SearchKey) select x).ToList();
        //                        if (mdl.SearchColumn == "sUser")
        //                            newResultList = (from x in newResultList where x.Catalogue != null && (x.Catalogue.Name.Equals(mdl.SearchKey) || (x.Review != null && x.Review.Name.Equals(mdl.SearchKey)) || (x.Release != null && x.Release.Name.Equals(mdl.SearchKey))) select x).ToList();
        //                        if (mdl.SearchColumn == "Junk")
        //                            newResultList = (from x in newResultList where x.Junk != null && x.Junk.Equals(mdl.SearchKey) select x).ToList();
        //                        if (mdl.SearchColumn == "sStatus")
        //                        {
        //                            int sta = 0, sta1 = 1;
        //                            if (sStatus == "Catalogue")
        //                            {
        //                                sta = 0; sta1 = 1;
        //                            }
        //                            else if (sStatus == "Clarification")
        //                            {
        //                                sta = -1; sta1 = -1;
        //                            }
        //                            else if (sStatus == "QC")
        //                            {
        //                                sta = 2; sta1 = 3;
        //                            }
        //                            else if (sStatus == "QA")
        //                            {
        //                                sta = 4; sta1 = 5;
        //                            }
        //                            else
        //                            {
        //                                sta = 6; sta1 = 6;
        //                            }
        //                            if (sStatus == "Catalogue Rework")
        //                            {
        //                                newResultList = (from x in newResultList where (x.Reworkcat != null && (x.ItemStatus == 0 || x.ItemStatus == 1)) select x).ToList();
        //                            }
        //                            else if (sStatus == "QC Rework")
        //                            {
        //                                newResultList = (from x in newResultList where (x.Rework != null && (x.ItemStatus == 2 || x.ItemStatus == 3)) select x).ToList();
        //                            }
        //                            else
        //                            {
        //                                newResultList = (from x in newResultList where (x.ItemStatus == sta || x.ItemStatus == sta1) select x).ToList();
        //                            }

        //                        }
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    return newResultList;
        //}
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

        //private List<Prosol_Datamaster> SearchFn(string[] strArr, string ColumnName, string sCode)
        //{
        //    var fields = Fields.Include(strArr);
        //    if (sCode.Contains('*'))
        //    {


        //        var QryLst = new List<IMongoQuery>();
        //        var QryLst1 = new List<IMongoQuery>();
        //        string[] sepArr = sCode.Split('*');
        //        if (sepArr.Length > 2)
        //        {
        //            foreach (string str in sepArr)
        //            {
        //                if (str != "")
        //                {
        //                    var Qry1 = Query.Matches(ColumnName, BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd(), RegexOptions.IgnoreCase)));
        //                    QryLst.Add(Qry1);

        //                }
        //            }
        //        }
        //        else
        //        {
        //            foreach (string str in sepArr)
        //            {
        //                if (str != "")
        //                {
        //                    if (sCode.IndexOf('*') > 0)
        //                    {
        //                        //Start with
        //                        var Qry1 = Query.Matches(ColumnName, BsonRegularExpression.Create(new Regex(string.Format("^{0}", str.TrimStart().TrimEnd()), RegexOptions.IgnoreCase)));
        //                        QryLst.Add(Qry1);

        //                    }
        //                    else
        //                    {
        //                        //End with

        //                        var Qry1 = Query.Matches(ColumnName, BsonRegularExpression.Create(new Regex(str.TrimStart().TrimEnd() + "$", RegexOptions.IgnoreCase)));
        //                        QryLst.Add(Qry1);


        //                    }

        //                }
        //            }


        //        }
        //        var query = Query.And(QryLst);
        //        var arrResult = _DatamasterRepository.FindAll(fields, query).ToList();
        //        return arrResult;
        //    }
        //    else
        //    {
        //        var Qry1 = Query.EQ(ColumnName,sCode.TrimStart().TrimEnd());
        //        var arrResult = _DatamasterRepository.FindAll(fields, Qry1).ToList();
        //        return arrResult;

        //    }

        //}

        private List<Prosol_Datamaster> SearchFn(string[] strArr, string ColumnName, string sCode)
        {
            var fields = Fields.Include(strArr);

            if (sCode.Contains('*'))
            {
                var QryLst = new List<IMongoQuery>();
                string[] sepArr = sCode.Split('*');

                if (sepArr.Length > 2)
                {
                    foreach (string str in sepArr)
                    {
                        if (!string.IsNullOrWhiteSpace(str))
                        {
                            var regex = BsonRegularExpression.Create(new Regex(str.Trim(), RegexOptions.IgnoreCase));

                            if (ColumnName == "Materialcode")
                            {
                                QryLst.Add(Query.Or(
                                    Query.Matches("Materialcode", regex),
                                    Query.Matches("Itemcode", regex)
                                ));
                            }
                            else
                            {
                                QryLst.Add(Query.Matches(ColumnName, regex));
                            }
                        }
                    }
                }
                else
                {
                    foreach (string str in sepArr)
                    {
                        if (!string.IsNullOrWhiteSpace(str))
                        {
                            IMongoQuery qry;

                            if (sCode.IndexOf('*') > 0)
                            {
                                // Starts with
                                var regex = BsonRegularExpression.Create(new Regex("^" + str.Trim(), RegexOptions.IgnoreCase));

                                if (ColumnName == "Materialcode")
                                {
                                    qry = Query.Or(
                                        Query.Matches("Materialcode", regex),
                                        Query.Matches("Itemcode", regex)
                                    );
                                }
                                else
                                {
                                    qry = Query.Matches(ColumnName, regex);
                                }
                            }
                            else
                            {
                                // Ends with
                                var regex = BsonRegularExpression.Create(new Regex(str.Trim() + "$", RegexOptions.IgnoreCase));

                                if (ColumnName == "Materialcode")
                                {
                                    qry = Query.Or(
                                        Query.Matches("Materialcode", regex),
                                        Query.Matches("Itemcode", regex)
                                    );
                                }
                                else
                                {
                                    qry = Query.Matches(ColumnName, regex);
                                }
                            }

                            QryLst.Add(qry);
                        }
                    }
                }

                var query = Query.Or(QryLst);
                return _DatamasterRepository.FindAll(fields, query).ToList();
            }
            else
            {
                if (sCode.Contains("/"))
                {
                    string[] splt = sCode.Split('/');
                    sCode = splt[1] + "/" + splt[0] + "/" + splt[2];
                    var date = DateTime.Parse(sCode, new CultureInfo("en-US", true));
                    date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                    var date1 = date.AddDays(1);

                    var Qry1 = Query.And(
                        Query.GTE("CreatedOn", BsonDateTime.Create(date)),
                        Query.LT("CreatedOn", BsonDateTime.Create(date1))
                    );

                    return _DatamasterRepository.FindAll(fields, Qry1).ToList();
                }
                else
                {
                    IMongoQuery qry;

                    if (ColumnName == "Materialcode")
                    {
                        qry = Query.Or(
                            Query.EQ("Materialcode", sCode.Trim()),
                            Query.EQ("Itemcode", sCode.Trim())
                        );
                    }
                    else
                    {
                        qry = Query.EQ(ColumnName, sCode.Trim());
                    }

                    return _DatamasterRepository.FindAll(fields, qry).ToList();
                }
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
            else if (sStatus == "PV Completed")
            {
                sta = 13; sta1 = 13;
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
        public bool insertImages(Prosol_Datamaster imgAsset)
        {
            var reslt = _DatamasterRepository.Add(imgAsset);
            return reslt;

        }
        public string getRefType(string refType)
        {
            var qry = Query.EQ("Type", refType);
            var res = _ReftypeRepository.FindOne(qry);
            return res.Code;
        }
        static Dictionary<string, string> MissingParse(string description)
        {
            var keyValuePairs = new Dictionary<string, string>();

            string[] parts = description.Replace(";", ",").Split(',');

            foreach (var part in parts)
            {
                if (part.Contains(":"))
                {
                    string[] keyValue = part.Split(':');
                    string key = keyValue[0].Trim().ToUpper(); 
                    string value = keyValue[1].Trim();
                    keyValuePairs[key] = value;
                }
            }

            return keyValuePairs;
        }
        static string ConvertMFRToSequential(string input, out int mfrCount)
        {
            string pattern = @"MFR:([A-Za-z0-9]+)";
            MatchCollection matches = Regex.Matches(input, pattern);
            Dictionary<string, int> mfrOccurrenceCount = new Dictionary<string, int>();
            foreach (Match match in matches)
            {
                string mfrValue = match.Groups[1].Value; 
                if (!mfrOccurrenceCount.ContainsKey(mfrValue))
                {
                    mfrOccurrenceCount[mfrValue] = mfrOccurrenceCount.Count + 1; 
                }
                string replacement = $"MFR{mfrOccurrenceCount[mfrValue]}:{mfrValue}";
                input = input.Replace(match.Value, replacement);
            }
            mfrCount = mfrOccurrenceCount.Count;

            return input;
        }
        public virtual int BulkNM(HttpPostedFileBase file)
        {
            int cunt = 0;
            string error = "";
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
                var query1 = Query.Or(Query.EQ("Itemcode", drw[0].ToString()), Query.EQ("Materialcode", drw[0].ToString()));
                var mdl1 = _DatamasterRepository.FindOne(query1);
                if (mdl1 != null)
                {
                    mdl1.Noun = drw[1].ToString();
                    mdl1.Modifier = drw[2].ToString();
                    mdl1.Unspsc = drw[3].ToString();
                    mdl1.Additionalinfo = drw[4].ToString();
                    mdl1.Specification = drw[5].ToString();
                    _DatamasterRepository.Add(mdl1);
                    cunt++;
                }

            }

            return cunt;
        }
    }

        public class searchObj
        {
            public string SearchKey { set; get; }
            public string SearchColumn { set; get; }
        }
        public class chaAbv
        {
            public string Characteristic { set; get; }
            public string Abbrivation { set; get; }
        }
    
}
