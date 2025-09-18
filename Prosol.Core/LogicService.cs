using Excel;
using MongoDB.Bson;
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
    public partial class LogicService : ILogic
    {
        private readonly IRepository<Prosol_Attribute> _AttributeRepository;
        private readonly IRepository<Prosol_UOM> _UnitRepository;
        private readonly IRepository<Prosol_Logic> _LogicRepository;
        private readonly IRepository<Prosol_Abbrevate> _valuelist;
        private readonly IRepository<Prosol_NounModifiers> _NounModofiersRepository;
        private readonly IRepository<Prosol_NMAttributeRelationship> _NMattrRel;
        private readonly IRepository<Prosol_Charateristics> _CharateristicsRepository;
        public LogicService(IRepository<Prosol_Attribute> AttributeRepository,
            IRepository<Prosol_UOM> UnitRepository,
            IRepository<Prosol_Logic> LogicRepository,
            IRepository<Prosol_Abbrevate> valuelist,
            IRepository<Prosol_NounModifiers> NounModofiersRepository,
            IRepository<Prosol_NMAttributeRelationship> NMattrRel,
            IRepository<Prosol_Charateristics> CharateristicsRepository)
        {

            this._AttributeRepository = AttributeRepository;
            this._UnitRepository = UnitRepository;
            this._LogicRepository = LogicRepository;
            this._valuelist = valuelist;
            this._NounModofiersRepository = NounModofiersRepository;
            this._NMattrRel = NMattrRel;
            this._CharateristicsRepository = CharateristicsRepository;
        }
        public IEnumerable<Prosol_Attribute> getattributelist()
        {
         
            var result = _AttributeRepository.FindAll().ToList();
            return result;

        }
        public IEnumerable<Prosol_UOM> getunit()
        {

            var result = _UnitRepository.FindAll().ToList();
            return result;

        }
        public bool insertdata(Prosol_Logic data, int update)
        {
            int flg = 0;
            string[] Noun = new string[50], Modifier = new string[50];
            var Qry1 = Query.EQ("Characteristic", data.AttributeName1);
            var res1 = _CharateristicsRepository.FindAll(Qry1).ToList();
            if (res1 != null && res1.Count > 0)
            {
                var Qry2 = Query.EQ("Characteristic", data.AttributeName2);
                var res2 = _CharateristicsRepository.FindAll(Qry2).ToList();
                if (res2 != null && res2.Count > 0)
                {
                    int i = 0;
                    foreach (Prosol_Charateristics mdl in res2)
                    {
                       
                        foreach (Prosol_Charateristics md in res1)
                        {
                            if (md.Noun==mdl.Noun && md.Modifier==mdl.Modifier)
                            {
                                flg = 1;
                                Noun[i] = mdl.Noun; Modifier[i] = mdl.Modifier;
                                i++;
                            }
                        }
                    }
                }
                if (data.AttributeName3 != null && data.AttributeName3 != "")
                {
                    var Qry3 = Query.EQ("Characteristic", data.AttributeName3);
                    var res3 = _CharateristicsRepository.FindAll(Qry3).ToList();
                    if (res3 != null && res3.Count > 0)
                    {
                        flg = 0;
                        foreach (Prosol_Charateristics mdl in res3)
                        {
                            if (Noun.Contains(mdl.Noun) && Modifier.Contains(mdl.Modifier))
                                flg = 1;

                        }

                    }
                }
                if (data.AttributeName4 != null && data.AttributeName4 != "")
                {
                    var Qry4 = Query.EQ("Characteristic", data.AttributeName4);
                    var res4 = _CharateristicsRepository.FindAll(Qry4).ToList();
                    if (res4 != null && res4.Count > 0)
                    {
                        flg = 0;
                        foreach (Prosol_Charateristics mdl in res4)
                        {
                            if (Noun.Contains(mdl.Noun) && Modifier.Contains(mdl.Modifier))
                                flg = 1;

                        }

                    }
                }
            }
            if (flg == 1)
            {
                var res = false;
                int f = 0;
                foreach (string str in Noun)
                {
                    if (str != null)
                    {
                        var newobj = new Prosol_Logic();

                        newobj.Noun = str;
                        newobj.Modifier = Modifier[f];
                        newobj.AttributeName1 = data.AttributeName1;
                        newobj.Value1 = data.Value1;
                        newobj.Unitname1 = data.Unitname1;

                        newobj.AttributeName2 = data.AttributeName2;
                        newobj.Value2 = data.Value2;
                        newobj.Unitname2 = data.Unitname2;

                        newobj.AttributeName3 = data.AttributeName3;
                        newobj.Value3 = data.Value3;
                        newobj.Unitname3 = data.Unitname3;

                        newobj.AttributeName4 = data.AttributeName4;
                        newobj.Value4 = data.Value4;
                        newobj.Unitname4 = data.Unitname4;
                        f++;
                        res = _LogicRepository.Add(newobj);
                    }
                    else break;
                }
                return res;
            }
            else
            {
                return false;
            }
        }
        public IEnumerable<Prosol_Logic> ATTRRELLIST()
        {
        
            var shwusr = _LogicRepository.FindAll().ToList();
            return shwusr;
        }
        public  bool deleteattribute(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _LogicRepository.Delete(query);
            return res;

        }
        public IEnumerable<Prosol_Attribute> getunitname(string AttributeName1)
        {
                   // public List<Prosol_Attribute> GetUnits()
      //  {


            var Qry = Query.EQ("Attribute", AttributeName1);
            var AttributeList = _AttributeRepository.FindAll(Qry).ToList();

            if (AttributeList.Count > 0)
            {
                foreach (Prosol_Attribute md in AttributeList)
                {
                    var Lst1 = new List<string>();
                    var Lst = new List<ObjectId>();
                    string[] strArr = { "Unitname" };
                    var fields = Fields.Include(strArr).Exclude("_id");
                    foreach (string str in md.UOMList)
                    {
                        Lst.Add(new ObjectId(str));
                    }
                    var query = Query.In("_id", new BsonArray(Lst));
                    var arrResult = _UnitRepository.FindAll(fields, query);
                    foreach (Prosol_UOM mdl in arrResult)
                    {
                        Lst1.Add(mdl.Unitname);

                    }
                    md.UOMList = Lst1.ToArray();
                }

            }

            return AttributeList;
        }

        public IEnumerable<Prosol_Abbrevate> GetValue1()
        {
           // var Qry = Query.EQ("Value", value);
            var res = _valuelist.FindAll();
            return res;
        }
        public IEnumerable<Prosol_NounModifiers> getnoun()
        {

            var result = _NounModofiersRepository.FindAll().ToList();
            return result;

        }

        public IEnumerable<Prosol_NounModifiers> getmodifiers(string Noun)
        {
            var query = Query.EQ("Noun", Noun);
            var shwusr = _NounModofiersRepository.FindAll(query).ToList();
            return shwusr;
        }
        public bool savenmattribute(Prosol_NMAttributeRelationship catList, int update)
        {
            bool res = false;
            if (update == 1)
            {
                res = _NMattrRel.Add(catList);
                return res;
            }
            else
            {
                var res1 = _NMattrRel.Add(catList);
                return res1;
            }
        }
        public IEnumerable<Prosol_NMAttributeRelationship> getnmList()
        {
            var shwusr = _NMattrRel.FindAll().ToList();
            return shwusr;
        }


        public bool DeleteNMAttrRel(string id)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var res = _NMattrRel.Delete(query);
            return res;

        }
        public IEnumerable<Prosol_NMAttributeRelationship> GetTableListforkeyatt(string Noun, string Modifier, string KeyAttribute, string KeyValue)
        {
            //var query = Query.And(Query.EQ("Noun", Noun);


            var query = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.EQ("KeyAttribute", KeyAttribute), Query.EQ("KeyValue", KeyValue));
            var shwusr = _NMattrRel.FindAll(query);
            //List<Prosol_KeyAttribute> hhh = new List<Prosol_KeyAttribute>();
            //hhh = shwusr.Characteristics;
            return shwusr;
        }
        public  IEnumerable<Prosol_Logic> GetLogicList(string srchtxt)
        {
            //var fields = Fields.Exclude("Attribute");
            var query = Query.Or(Query.Matches("AttributeName1", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Value1", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Unitname1", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("AttributeName2", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Value2", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Unitname2", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("AttributeName3", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Value3", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Unitname3", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("AttributeName4", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Value4", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Unitname4", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))));
            var LogicList = _LogicRepository.FindAll(query);
            return LogicList;
        }

        public virtual IEnumerable<Prosol_Logic> GetLogicList()
        {
            //var fields = Fields.Exclude("Attribute");
            var LogicList = _LogicRepository.FindAll();
            return LogicList;
        }
        public IEnumerable<Prosol_NMAttributeRelationship> GetNMList(string srchtxt)
        {
            //var fields = Fields.Exclude("Attribute");
            var query = Query.Or(Query.Matches("Noun", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("Modifier", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("KeyAttribute", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))), Query.Matches("KeyValue", BsonRegularExpression.Create(new Regex(srchtxt, RegexOptions.IgnoreCase))));
            var LogicList = _NMattrRel.FindAll(query);
            return LogicList;
        }

        public virtual IEnumerable<Prosol_NMAttributeRelationship> GetNMList()
        {
            //var fields = Fields.Exclude("Attribute");
            var LogicList = _NMattrRel.FindAll();
            return LogicList;
        }

        public IEnumerable<Prosol_NMAttributeRelationship> FetchNMRelation(string Noun, string Modifier)
        {
            var Qry = Query.And(Query.EQ("Noun",Noun), Query.EQ("Modifier",Modifier));
            var LstFetchNMRelation =_NMattrRel.FindAll(Qry);
            return LstFetchNMRelation;
        }
        public virtual int BulkNounModifierAttribute(HttpPostedFileBase file)

        {

            // var Lst=new List<string>();
            // int cunt = 0;
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
            var nmar_list = new List<Prosol_NMAttributeRelationship>();
            int t = 0;
            foreach (DataRow dr in dt.Rows)
            {
                var query = Query.And(Query.EQ("Noun", dr[0].ToString()), Query.EQ("Modifier", dr[1].ToString()), Query.EQ("KeyAttribute", dr[2].ToString()), Query.EQ("KeyValue", dr[3].ToString()));
                var ObjStr = _NMattrRel.FindOne(query);

                if (ObjStr == null)
                {

                    if (dr[0].ToString() != "")
                    {
                        var ListattMdl = new List<Prosol_KeyAttribute>();
                        var attMdl = new Prosol_KeyAttribute();
                        for (int i = 2; i <= dr.ItemArray.Length - 3;)
                        {
                            if (dr[i] != null)
                            {
                                t = 1;
                                attMdl = new Prosol_KeyAttribute();
                                attMdl.Characteristic = dr[i++].ToString();
                                attMdl.Value = dr[i++].ToString();
                                attMdl.UOM = dr[i++].ToString();
                                ListattMdl.Add(attMdl);
                            }
                            else break;

                        }
                        Prosol_NMAttributeRelationship nmr = new Prosol_NMAttributeRelationship();
                        nmr.Noun = dr[0].ToString();
                        nmr.Modifier = dr[1].ToString();
                        nmr.Characteristics = ListattMdl;
                        nmr.KeyAttribute = dr[2].ToString();
                        nmr.KeyValue = dr[3].ToString();

                        nmar_list.Add(nmr);

                    }
                }

            }
            if (t == 1)
            {
                _NMattrRel.Add(nmar_list);
                return nmar_list.Count;
            }
            else
                return 0;

        }

        public IEnumerable<Prosol_Logic> FetchATTRelation(string Noun, string Modifier)
        {
            var Qry = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier));
            var FetchATTRelation = _LogicRepository.FindAll(Qry);
            return FetchATTRelation;

        }

    }

}

    

