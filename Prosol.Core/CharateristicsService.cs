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
using MongoDB.Bson;

namespace Prosol.Core
{

    public partial class CharateristicsService : ICharateristics
    {

        private readonly IRepository<Prosol_Charateristics> _CharateristicRepository;
        private readonly IRepository<Prosol_NounModifiers> _nounModifierRepository;
        private readonly IRepository<Prosol_Attribute> _attributeRepository;
        private readonly IRepository<Prosol_Abbrevate> _abbreviateRepository;
        private readonly IRepository<Prosol_AssetAbbrevate> _assetAbbrivateRepository;
        private readonly IRepository<Prosol_UOM> _UOMRepository;
        public CharateristicsService(IRepository<Prosol_Charateristics> charateristicRepository,
            IRepository<Prosol_NounModifiers> nounModifierRepository,
            IRepository<Prosol_Attribute> attributeRepository,
            IRepository<Prosol_Abbrevate> AbbreviateRepository,
            IRepository<Prosol_AssetAbbrevate> AssetAbbreviateRepository,
             IRepository<Prosol_UOM> UOMRepository)
        {
            this._CharateristicRepository = charateristicRepository;
            this._nounModifierRepository = nounModifierRepository;
            this._attributeRepository = attributeRepository;
            this._abbreviateRepository = AbbreviateRepository;
            this._assetAbbrivateRepository = AssetAbbreviateRepository;
            this._UOMRepository = UOMRepository;
        }
        public bool Create(List<Prosol_Charateristics> chara)
        {
            var query = Query.And(Query.EQ("Noun", chara[0].Noun), Query.EQ("Modifier", chara[0].Modifier), Query.EQ("Definition", "MM"));
            _CharateristicRepository.Delete(query);

           // string[] strArr = { "Unitname" };
           // var fields = Fields.Include(strArr).Exclude("_id");
            foreach (Prosol_Charateristics pch in chara)
            {
                if (pch.Uom != null && pch.Uom.Length > 0)
                {
                    var Lst1 = new List<string>();
                    var query1 = Query.In("Unitname", new BsonArray(pch.Uom));
                    var arrResult = _UOMRepository.FindAll(query1);
                    foreach (Prosol_UOM mdl in arrResult)
                    {
                        Lst1.Add(mdl._id.ToString());

                    }
                    pch.Uom = Lst1.ToArray();
                }
            }
            _CharateristicRepository.Add(chara);

            return true;


        }
        public bool AssetCreate(List<Prosol_Charateristics> chara)
        {
            var query = Query.And(Query.EQ("Noun", chara[0].Noun), Query.EQ("Modifier", chara[0].Modifier), Query.EQ("Definition", "Equ"));
            _CharateristicRepository.Delete(query);

           // string[] strArr = { "Unitname" };
           // var fields = Fields.Include(strArr).Exclude("_id");
            foreach (Prosol_Charateristics pch in chara)
            {
                if (pch.Uom != null && pch.Uom.Length > 0)
                {
                    var Lst1 = new List<string>();
                    var query1 = Query.In("Unitname", new BsonArray(pch.Uom));
                    var arrResult = _UOMRepository.FindAll(query1);
                    foreach (Prosol_UOM mdl in arrResult)
                    {
                        Lst1.Add(mdl._id.ToString());

                    }
                    pch.Uom = Lst1.ToArray();
                }
            }
            foreach (Prosol_Charateristics pch in chara)
            {
                if (pch.Values != null && pch.Values.Length > 0)
                {
                    var Lst1 = new List<string>();
                    foreach (var value in pch.Values)
                    {
                        var qry = Query.EQ("Value", value);
                        var mdl = _assetAbbrivateRepository.FindOne(qry);
                        if (mdl != null) 
                        {
                            Lst1.Add(mdl._id.ToString());
                        }
                        else
                        {
                            mdl = new Prosol_AssetAbbrevate();
                            mdl.Value = value;
                            mdl.Abbrevated = value;
                            mdl.Approved = "No";
                            _assetAbbrivateRepository.Add(mdl);
                            var query1 = Query.EQ("Value", value);
                            var mdl1 = _assetAbbrivateRepository.FindOne(query1);
                            Lst1.Add(mdl1._id.ToString());
                        }
                    }
                    pch.Values = Lst1.ToArray();
                }
            }
            _CharateristicRepository.Add(chara);

            return true;


        }
        public int BulkCharateristic(HttpPostedFileBase file)
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
            if (res != null)
            {
                DataTable dt = res.Tables[0];
                if (dt.Columns[0].ColumnName == "Noun" && dt.Columns[1].ColumnName == "Modifier" && dt.Columns[2].ColumnName == "Characteristics"
                    && dt.Columns[3].ColumnName == "Abbrevation" && dt.Columns[4].ColumnName == "Long Seq" && dt.Columns[5].ColumnName == "Short Seq"
                    && dt.Columns[6].ColumnName == "Mandatory" && dt.Columns[7].ColumnName == "Characteristic Definition" && dt.Columns[8].ColumnName == "UOM Mandatory")
                {
                    var LstChar = new List<Prosol_Charateristics>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[0].ToString() != "" && dr[1].ToString() != "" && dr[2].ToString() != "")
                        {
                            var NM_Mdl = new Prosol_Charateristics();
                            NM_Mdl.Noun = dr[0].ToString();
                            NM_Mdl.Modifier = dr[1].ToString();
                            NM_Mdl.Characteristic = dr[2].ToString();
                            NM_Mdl.Abbrivation = dr[3].ToString();
                            NM_Mdl.Squence = dr[4] != null ? Convert.ToInt16(dr[4]) : 0;
                            NM_Mdl.ShortSquence = dr[5] != null ? Convert.ToInt16(dr[5]) : 0;
                            NM_Mdl.Mandatory = dr[6].ToString();
                            NM_Mdl.Definition = dr[7].ToString();
                            NM_Mdl.UomMandatory = dr[8].ToString();
                            NM_Mdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                            LstChar.Add(NM_Mdl);
                        }
                    }
                    if (LstChar.Count > 0)
                    {


                        List<Prosol_Charateristics> filteredList = LstChar.GroupBy(p => new { p.Noun, p.Modifier, p.Characteristic }).Select(g => g.First()).ToList();

                        if (filteredList.Count > 0)
                        {
                            var fRes = new List<Prosol_Charateristics>();
                            int flg = 0;
                            foreach (Prosol_Charateristics nm in filteredList.ToList())
                            {
                                var query1 = Query.And(Query.EQ("Noun", nm.Noun), Query.EQ("Modifier", nm.Modifier));
                                var ObjStr1 = _nounModifierRepository.FindOne(query1);
                                if (ObjStr1 != null && ObjStr1.Noun != null)
                                {
                                    flg = 1;
                                    var query = Query.And(Query.EQ("Noun", nm.Noun), Query.EQ("Modifier", nm.Modifier), Query.EQ("Characteristic", nm.Characteristic), Query.EQ("Definition" , "MM" ));
                                    var ObjStr = _CharateristicRepository.FindOne(query);
                                    if (ObjStr == null)
                                    {
                                        fRes.Add(nm);
                                    }
                                }
                            }
                            if (flg == 1 && fRes.Count > 0)
                            {
                                cunt = _CharateristicRepository.Add(fRes);
                            }
                            if (flg == 1 && fRes.Count == 0)
                            {
                                cunt = 0;
                            }
                            else if (flg == 0) cunt = -1;

                        }
                    }
                }
                else return -2;
            }
            else return -2;
            return cunt;


        }
        //bulkvalue
        public int BulkValue(HttpPostedFileBase file)
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
            if (res != null)
            {
                DataTable dt = res.Tables[0];
                if (dt.Columns[0].ColumnName == "NOUN" && dt.Columns[1].ColumnName == "MODIFIER" && dt.Columns[2].ColumnName == "ATTRIBUTE" && dt.Columns[3].ColumnName == "VALUE")
                {
                    DataTable resTbl = dt.AsEnumerable().GroupBy(r => new { Col1 = r["NOUN"], Col2 = r["MODIFIER"], Col3 = r["ATTRIBUTE"] }).Select(x => x.First()).CopyToDataTable();
                    var LstChar = new List<Prosol_Charateristics>();
                    foreach (DataRow dr in resTbl.Rows)
                    {
                        if (dr[0].ToString() != "" && dr[1].ToString() != "" && dr[2].ToString() != "")
                        {

                            DataRow[] result = dt.Select("NOUN = '" + dr[0].ToString() + "' and MODIFIER = '" + dr[1].ToString() + "' and ATTRIBUTE = '" + dr[2].ToString() + "'");
                            List<string> LstStr = new List<string>();
                            // string[] str = new string[result.Length];
                            //  int i = 0;
                            foreach (DataRow row in result)
                            {
                                //str[i]= row["VALUE"].ToString();                      
                                var Qry = Query.EQ("Value", row["VALUE"].ToString());
                                var obj = _abbreviateRepository.FindOne(Qry);
                                if (obj != null)
                                {
                                    // str[i] = obj._id.ToString();
                                    LstStr.Add(obj._id.ToString());
                                    // i++;
                                }

                            }
                            var query = Query.And(Query.EQ("Noun", dr[0].ToString()), Query.EQ("Modifier", dr[1].ToString()), Query.EQ("Characteristic", dr[2].ToString()));
                            var ObjStr = _CharateristicRepository.FindOne(query);
                            if (ObjStr != null)
                            {
                                if (ObjStr.Values != null)
                                {
                                    var Lstobj = ObjStr.Values.ToList();
                                    foreach (string stng in LstStr)
                                    {
                                        if (!ObjStr.Values.Contains(stng))
                                        {
                                            Lstobj.Add(stng);
                                        }
                                    }
                                    ObjStr.Values = Lstobj.ToArray();
                                }
                                else
                                {
                                    ObjStr.Values = LstStr.ToArray();

                                }
                                _CharateristicRepository.Add(ObjStr);
                            }

                            cunt++;
                        }
                    }
                }
                else return -1;
                
            }
            else return -1;
            return cunt;
        }
        public int BulkUom(HttpPostedFileBase file)
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
            if (res != null)
            {
                DataTable dt = res.Tables[0];
                if (dt.Columns[0].ColumnName == "NOUN" && dt.Columns[1].ColumnName == "MODIFIER" && dt.Columns[2].ColumnName == "ATTRIBUTE" && dt.Columns[3].ColumnName == "UOM")
                {
                    DataTable resTbl = dt.AsEnumerable().GroupBy(r => new { Col1 = r["NOUN"], Col2 = r["MODIFIER"], Col3 = r["ATTRIBUTE"] }).Select(x => x.First()).CopyToDataTable();
                    var LstChar = new List<Prosol_Charateristics>();
                    foreach (DataRow dr in resTbl.Rows)
                    {
                        if (dr[0].ToString() != "" && dr[1].ToString() != "" && dr[2].ToString() != "")
                        {

                            DataRow[] result = dt.Select("NOUN = '" + dr[0].ToString() + "' and MODIFIER = '" + dr[1].ToString() + "' and ATTRIBUTE = '" + dr[2].ToString() + "'");
                            List<string> LstStr = new List<string>();
                            // string[] str = new string[result.Length];
                            //  int i = 0;
                            foreach (DataRow row in result)
                            {
                                //str[i]= row["VALUE"].ToString();                      
                                var Qry = Query.EQ("Unitname", row["UOM"].ToString());
                                var obj = _UOMRepository.FindOne(Qry);
                                if (obj != null)
                                {
                                    // str[i] = obj._id.ToString();
                                    LstStr.Add(obj._id.ToString());
                                    // i++;
                                }

                            }
                            var query = Query.And(Query.EQ("Noun", dr[0].ToString()), Query.EQ("Modifier", dr[1].ToString()), Query.EQ("Characteristic", dr[2].ToString()));
                            var ObjStr = _CharateristicRepository.FindOne(query);
                            if (ObjStr != null)
                            {
                                if (ObjStr.Uom != null)
                                {
                                    var Lstobj = ObjStr.Uom.ToList();
                                    foreach (string stng in LstStr)
                                    {
                                        if (!ObjStr.Uom.Contains(stng))
                                        {
                                            Lstobj.Add(stng);
                                        }
                                    }
                                    ObjStr.Uom = Lstobj.ToArray();
                                }
                                else
                                {
                                    ObjStr.Uom = LstStr.ToArray();

                                }
                                _CharateristicRepository.Add(ObjStr);
                            }

                            cunt++;
                        }
                    }
                }
                else return -1;

            }
            else return -1;
            return cunt;
        }
        public IEnumerable<Prosol_Charateristics> GetCharateristic(string Noun, string Modifier)
        {
            // var sort = SortBy.Ascending("ShortSquence");
            var query = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier));
            var sort = SortBy.Ascending("Squence");
            var Nm = _CharateristicRepository.FindAll(query, sort).ToList();
            foreach (Prosol_Charateristics md in Nm)
            {
                var Lst1 = new List<string>();
                var Lst = new List<ObjectId>();
                string[] strArr = { "Unitname" };
                var fields = Fields.Include(strArr).Exclude("_id");
                if (md.Uom != null && md.Uom.Length > 0)
                {
                    foreach (string str in md.Uom)
                    {
                        Lst.Add(new ObjectId(str));
                    }
                }
                else
                {
                    var mQry = Query.EQ("Attribute", md.Characteristic);
                    var AttributeList = _attributeRepository.FindAll(mQry).ToList();

                    if (AttributeList.Count > 0)
                    {
                        Lst1 = new List<string>();
                        Lst = new List<ObjectId>();
                        foreach (Prosol_Attribute mdl in AttributeList)
                        {

                            if (mdl.UOMList != null && mdl.UOMList.Length > 0)
                            {
                                foreach (string str in mdl.UOMList)
                                {
                                    Lst.Add(new ObjectId(str));
                                }
                            }

                        }

                    }
                }
                var query1 = Query.In("_id", new BsonArray(Lst));
                var arrResult = _UOMRepository.FindAll(fields, query1);
                foreach (Prosol_UOM mdl in arrResult)
                {
                    Lst1.Add(mdl.Unitname);

                }
                md.Uom = Lst1.ToArray();
            }
            return Nm;
        }
        public IEnumerable<Prosol_Charateristics> GetCharateristics(string Noun, string Modifier,string flg)
        {
            // var sort = SortBy.Ascending("ShortSquence");
            var query = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.EQ("Definition", flg));
            var sort = SortBy.Ascending("Squence");
            var Nm = _CharateristicRepository.FindAll(query, sort).ToList();
            foreach (Prosol_Charateristics md in Nm)
            {
                var Lst1 = new List<string>();
                var Lst = new List<ObjectId>();
                string[] strArr = { "Unitname" };
                var fields = Fields.Include(strArr).Exclude("_id");
                if (md.Uom != null && md.Uom.Length > 0)
                {
                    foreach (string str in md.Uom)
                    {
                        Lst.Add(new ObjectId(str));
                    }
                }
                else
                {
                    var mQry = Query.EQ("Attribute", md.Characteristic);
                    var AttributeList = _attributeRepository.FindAll(mQry).ToList();

                    if (AttributeList.Count > 0)
                    {
                        Lst1 = new List<string>();
                        Lst = new List<ObjectId>();
                        foreach (Prosol_Attribute mdl in AttributeList)
                        {

                            if (mdl.UOMList != null && mdl.UOMList.Length > 0)
                            {
                                foreach (string str in mdl.UOMList)
                                {
                                    Lst.Add(new ObjectId(str));
                                }
                            }

                        }

                    }
                }
                var query1 = Query.In("_id", new BsonArray(Lst));
                var arrResult = _UOMRepository.FindAll(fields, query1);
                foreach (Prosol_UOM mdl in arrResult)
                {
                    Lst1.Add(mdl.Unitname);
                }
                md.Uom = Lst1.ToArray();
            }
            return Nm;
        }

        public List<Dictionary<string, object>> DownloadNM()
        {

            var sort = SortBy.Ascending("Noun").Ascending("Squence");

            string[] strArr = { "Noun", "Modifier", "Characteristic", "Definition", "Squence", "ShortSquence", "Mandatory","UomMandatory","Values","Uom" };
            var fields = Fields.Include(strArr).Exclude("_id");
            var lstCha = _CharateristicRepository.FindAll(fields, sort).ToList();
            string[] strArr1 = { "Noun", "Modifier", "Nounabv", "ModifierDefinition", "ImageId", "Formatted" };
            fields = Fields.Include(strArr1).Exclude("_id");
            var nmlist = _nounModifierRepository.FindAll(fields, sort);

            var mergelist = (from nm in nmlist join chr in lstCha on nm.Noun
                             equals chr.Noun where nm.Noun == chr.Noun && nm.Modifier == chr.Modifier select new
                             { Noun = nm.Noun, Modifier = nm.Modifier,
                                 nmabv = nm.Nounabv, nmdef = nm.ModifierDefinition,
                                 image = nm.ImageId, cat = nm.Formatted == 0 ? "OEM" :
                                     (nm.Formatted == 1) ? "Generic" : "OPM",
                                 chr = chr.Characteristic, chrdef = chr.Definition,
                                 chrman = chr.Mandatory, uomman = chr.UomMandatory,
                                 sseq = chr.ShortSquence, lseq = chr.Squence, value =(chr.Values!=null && chr.Values.Count()>0)? chr.Values[0] :"",UOM =(chr.Uom!=null && chr.Uom.Count()>0)?chr.Uom[0] :"" }).ToList();


            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;

            foreach (var data in mergelist)
            {
                if (data.chrdef == "MM") {
                    row = new Dictionary<string, object>();
                    row.Add("Noun", data.Noun);
                    row.Add("Modifier", data.Modifier);
                    row.Add("NM Abberivation", data.nmabv);
                    row.Add("NM Definition", data.nmdef);
                    row.Add("Image Id", data.image);
                    row.Add("Category", data.cat);
                    row.Add("Attribute", data.chr);
                    row.Add("Attribute Def", data.chrdef);
                    row.Add("Mandatory", data.chrman);
                    row.Add("UOM Mandatory", data.uomman);
                    row.Add("Short Sequence", data.sseq);
                    row.Add("Long Sequence", data.lseq);
                    row.Add("Value", data.value != "" ? getValue(data.value) : "");
                    row.Add("UOM", data.UOM != "" ? getUom(data.UOM) : "");

                    rows.Add(row);
                }

            }
            return rows;

        }

        public List<Dictionary<string, object>> DownloadAssetNM()
        {

            var sort = SortBy.Ascending("Noun").Ascending("Squence");

            string[] strArr = { "Noun", "Modifier", "Characteristic", "Definition", "Squence", "ShortSquence", "Mandatory","UomMandatory","Values","Uom" };
            var fields = Fields.Include(strArr).Exclude("_id");
            var qry = Query.EQ("Definition","Equ");
            var nmqry = Query.EQ("RP","Equ");
            var lstCha = _CharateristicRepository.FindAll(qry, sort).ToList();
            string[] strArr1 = { "Noun", "Modifier", "Nounabv", "ModifierDefinition", "ImageId", "Formatted" };
            fields = Fields.Include(strArr1).Exclude("_id");
            var nmlist = _nounModifierRepository.FindAll( fields, nmqry, sort);

            var mergelist = (from nm in nmlist join chr in lstCha on nm.Noun
                             equals chr.Noun where nm.Noun == chr.Noun && nm.Modifier == chr.Modifier select new
                             { Noun = nm.Noun, Modifier = nm.Modifier,
                                 nmabv = nm.Nounabv, nmdef = nm.ModifierDefinition,
                                 image = nm.ImageId, cat = nm.Formatted == 0 ? "OEM" :
                                     (nm.Formatted == 1) ? "Generic" : "OPM",
                                 chr = chr.Characteristic, chrdef = chr.Definition,
                                 chrman = chr.Mandatory, uomman = chr.UomMandatory,
                                 HierarchyPath = chr.HierarchyPath,
                                 ClassificationId = chr.ClassificationId,
                                 PDesc = chr.PDesc,
                                 ClassLevel = chr.ClassLevel,
                                 Abbrivation = chr.Abbrivation,
                                 Squence = chr.Squence,
                                 sseq = chr.ShortSquence, lseq = chr.Squence, value =(chr.Values!=null && chr.Values.Count()>0)? chr.Values[0] :"",UOM =(chr.Uom!=null && chr.Uom.Count()>0)?chr.Uom[0] :"" }).ToList();


            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;

            foreach (var data in mergelist)
            {
                if (data.chrdef == "Equ") {
                    row = new Dictionary<string, object>();
                    //string classficationId = "";
                    //if (data.Noun != "--")
                    //    classficationId = data.Noun + "," + data.Modifier;
                    //else
                    //    classficationId = data.Noun;
                    row.Add("NOUN", data.Noun);
                    row.Add("MODIFIER", data.Modifier);
                    row.Add("CHARACTERISTIC", data.chr);
                    row.Add("ABBREVATION", data.Abbrivation);
                    row.Add("SHORT SEQUENCE", data.sseq);
                    row.Add("SEQUENCE", data.Squence);
                    row.Add("MANDATORY", data.chrman == "Yes" ? "M" : "O");
                    //row.Add("UOM MANDATORY", data.uomman);
                    //row.Add("UOM", data.UOM != "" ? getUom(data.UOM) : "");
                    //row.Add("Mandatory", data.chrman);
                    //row.Add("Short Sequence", data.sseq);
                    //row.Add("Long Sequence", data.lseq);
                    //row.Add("Value", data.value != "" ? getValue(data.value) : "");

                    rows.Add(row);
                }

            }
            return rows;

        }
        private string getValue(string valId)
        {
            var qry = Query.EQ("_id", new ObjectId(valId));
            var res = _abbreviateRepository.FindOne(qry);
            if (res != null)
                return res.Value;
            else return "";
           
        }
        private string getUom(string uomId)
        {
            var qry = Query.EQ("_id", new ObjectId(uomId));
            var res = _UOMRepository.FindOne(qry);

            if (res != null)
                return res.Unitname;
            else return "";

        }
        // Attribute Master

        public bool AddAttribute(Prosol_Attribute Attribute)
        {

            var res = false;
            var query = Query.EQ("Attribute", Attribute.Attribute);
            var um = _attributeRepository.FindAll(query).ToList();

            if (um.Count == 0)
            {
                res = _attributeRepository.Add(Attribute);
            }
            else
            {
                Attribute._id = um[0]._id;
                res = _attributeRepository.Add(Attribute);
            }
            return res;
        }

        //public bool AddAttribute(Prosol_Attribute Attribute)
        //{

        //    var res = false;
        //    var query = Query.EQ("Attribute", Attribute.Attribute);
        //    var um = _attributeRepository.FindAll(query).ToList();
        //    if (um.Count == 0 || (um.Count == 1 && um[0]._id == Attribute._id))
        //    {
        //        res = _attributeRepository.Add(Attribute);
        //    }
        //    return res;
        //}
        public IEnumerable<Prosol_Attribute> GetAttributes()
        {
            var res = _attributeRepository.FindAll();
            return res;
        }
        public Prosol_Attribute GetAttributeDetail(string Name)
        {
            var query = Query.EQ("Attribute", Name);
            var res = _attributeRepository.FindOne(query);
            return res;
        }

        public virtual List<Prosol_Attribute> DownloadAttributeMaster()
        {

            // var sort = SortBy.Ascending("Noun").Ascending("Squence");

            string[] strArr = { "Attribute" };
            var fields = Fields.Include(strArr).Exclude("_id");
            var lstCha = _attributeRepository.FindAll(fields).ToList();
            return lstCha;

        }

        public Prosol_Charateristics GetCharacteristicvalues(string Name, string Noun, string Modifier)
        {
            var queryyy = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.EQ("Characteristic", Name));
            var result = _CharateristicRepository.FindOne(queryyy);
            return result;

        }
    }


}
