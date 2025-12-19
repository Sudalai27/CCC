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
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using Excel;
using MongoDB.Driver;

namespace Prosol.Core
{
    public partial class NounModifierService : INounModifier
    {
        private readonly IRepository<Prosol_NounModifiers> _nounModifierRepository;
        private readonly IRepository<Prosol_CodeLogic> _CodeLogicRepository;
        private readonly IRepository<Prosol_UOMMODEL> _UOMMODELRepository;
        private readonly IRepository<Prosol_Nounscrub> _NounscrubRepository;
        private readonly IRepository<Prosol_AssetAbbrevate> _assetAbbrevateRepository;
        public NounModifierService(IRepository<Prosol_NounModifiers> nounModifierRepository,
            IRepository<Prosol_CodeLogic> CodeLogicRepository,
            IRepository<Prosol_UOMMODEL> UOMMODELRepository,
            IRepository<Prosol_AssetAbbrevate> assetAbbrevateRepository,
             IRepository<Prosol_Nounscrub> NounscrubRepository)
        {
            this._nounModifierRepository = nounModifierRepository;
            this._CodeLogicRepository = CodeLogicRepository;
            this._UOMMODELRepository = UOMMODELRepository;
            this._NounscrubRepository = NounscrubRepository;
            this._assetAbbrevateRepository = assetAbbrevateRepository;
        }

        //savecode
        public bool codesave(Prosol_CodeLogic data)
        {
            bool res = false;


            var res1 = _CodeLogicRepository.FindAll().ToList();
            if (res1.Count() > 0)
            {
                data._id = res1[0]._id;
                res = _CodeLogicRepository.Add(data);
            }
            else
            {
                var query = Query.EQ("CODELOGIC", data.CODELOGIC);
                var vn = _CodeLogicRepository.FindAll(query).ToList();
                if (vn.Count == 0)
                {
                    res = _CodeLogicRepository.Add(data);
                }
            }
            return res;
        }


        public Prosol_CodeLogic showcode()
        {
            //var sort = SortBy.Descending("UpdatedOn");
            var shwusr = _CodeLogicRepository.FindOne();
            return shwusr;
        }



        //public virtual void DeleteNounModifier(Prosol_NounModifiers NM)
        //{
        //    try
        //    {
        //        _nounModifierRepository.Delete(NM);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.ToString());         

        //    }          

        //}      
        public virtual bool Create(Prosol_NounModifiers NM, HttpPostedFileBase file)
        {
            try
            {
                //Noun Modifier DB write
                var query = Query.And(Query.EQ("Noun", NM.Noun), Query.EQ("Modifier", NM.Modifier), Query.EQ("RP", "MM"));
               // var query = Query.EQ("_id", NM._id);

                var ObjStr = _nounModifierRepository.FindOne(query);

                NM.UpdatedOn =DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                NM._id = ObjStr != null ? ObjStr._id : new ObjectId();
                NM.ImageId = ObjStr != null ? ObjStr.ImageId : null;
                NM.RP = "MM";

                if (file != null && file.ContentLength > 0 && file.ContentType.Contains("image"))
                {
                    if (NM.ImageId != null)
                        _nounModifierRepository.GridFsDel(Query.EQ("_id", new ObjectId(NM.ImageId)));

                    Stream strm = file.InputStream;
                    using (var image = Image.FromStream(strm))
                    {
                        var bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppPArgb);
                        bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                        var graphics = Graphics.FromImage(bitmap);
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

                        graphics.Dispose();

                        using (var memoryStream = new MemoryStream())
                        {
                            //loading it to the memory stream                            
                            bitmap.Save(memoryStream, GetImageFormat(file.ContentType));
                            memoryStream.Position = 0;
                            strm = new MemoryStream(memoryStream.ToArray());
                        }
                    }
                    NM.ImageId = _nounModifierRepository.GridFsUpload(strm, NM.Noun + NM.Modifier);
                }
                if (ObjStr != null)
                {
                    query = Query.And(Query.EQ("Noun", ObjStr.Noun), Query.EQ("Modifier", ObjStr.Modifier));
                    NM.Nounabv = NM.Nounabv == null ? "" : NM.Nounabv;
                    NM.NounEqu= NM.NounEqu == null ? "" : NM.NounEqu;
                    NM.NounDefinition = NM.NounDefinition == null ? "" : NM.NounDefinition;
                    var Updte = Update.Set("Noun", NM.Noun).Set("NounDefinition", NM.NounDefinition).Set("Nounabv", NM.Nounabv).Set("NounEqu", NM.NounEqu);
                    var flg = UpdateFlags.Multi;

                    var theResult = _nounModifierRepository.Add(NM);
                    theResult = _nounModifierRepository.Update(query, Updte, flg);

                    // Noun equalent
                    //var Updte1 = Update.Set("NounEqu", NM.NounEqu);
                    //var flg1 = UpdateFlags.Multi;
                    //var Qry = Query.EQ("Noun", ObjStr.Noun);                 
                    //_nounModifierRepository.Update(Qry, Updte1, flg1);


                    return theResult;

                }
                else
                {
                    query = Query.And(Query.EQ("Noun", NM.Noun), Query.EQ("Modifier", NM.Modifier));
                    NM.NounEqu = NM.NounEqu == null ? "" : NM.NounEqu;
                    NM.Nounabv = NM.Nounabv == null ? "" : NM.Nounabv;
                    NM.NounDefinition = NM.NounDefinition == null ? "" : NM.NounDefinition;
                    var Updte = Update.Set("NounDefinition", NM.NounDefinition).Set("Nounabv", NM.Nounabv).Set("NounEqu", NM.NounEqu);
                    var flg = UpdateFlags.Multi;

                    var theResult = _nounModifierRepository.Add(NM);
                    theResult = _nounModifierRepository.Update(query, Updte, flg);

                    // Noun equalent
                    //var Updte1 = Update.Set("NounEqu", NM.NounEqu);
                    //var flg1 = UpdateFlags.Multi;
                    //var Qry = Query.EQ("Noun", ObjStr.Noun);
                    //_nounModifierRepository.Update(Qry, Updte1, flg1);

                    return theResult;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());

            }

        }
        public virtual bool AssetCreate(Prosol_NounModifiers NM, HttpPostedFileBase file)
        {
            try
            {
                //Noun Modifier DB write
                var query = Query.And(Query.EQ("Noun", NM.Noun), Query.EQ("Modifier", NM.Modifier), Query.EQ("RP", "Equ"));
                // var query = Query.EQ("_id", NM._id);

                var ObjStr = _nounModifierRepository.FindOne(query);

                NM.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                NM._id = ObjStr != null ? ObjStr._id : new ObjectId();
                NM.ImageId = ObjStr != null ? ObjStr.ImageId : null;
                NM.RP = "Equ";

                if (file != null && file.ContentLength > 0 && file.ContentType.Contains("image"))
                {
                    if (NM.ImageId != null)
                        _nounModifierRepository.GridFsDel(Query.EQ("_id", new ObjectId(NM.ImageId)));

                    Stream strm = file.InputStream;
                    using (var image = Image.FromStream(strm))
                    {
                        var bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppPArgb);
                        bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                        var graphics = Graphics.FromImage(bitmap);
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

                        graphics.Dispose();

                        using (var memoryStream = new MemoryStream())
                        {
                            //loading it to the memory stream                            
                            bitmap.Save(memoryStream, GetImageFormat(file.ContentType));
                            memoryStream.Position = 0;
                            strm = new MemoryStream(memoryStream.ToArray());
                        }
                    }
                    NM.ImageId = _nounModifierRepository.GridFsUpload(strm, NM.Noun + NM.Modifier);
                }
                if (ObjStr != null)
                {
                    query = Query.And(Query.EQ("Noun", ObjStr.Noun), Query.EQ("Modifier", ObjStr.Modifier));
                    NM.Nounabv = NM.Nounabv == null ? "" : NM.Nounabv;
                    NM.NounEqu = NM.NounEqu == null ? "" : NM.NounEqu;
                    NM.NounDefinition = NM.NounDefinition == null ? "" : NM.NounDefinition;
                    var Updte = Update.Set("Noun", NM.Noun).Set("NounDefinition", NM.NounDefinition).Set("Nounabv", NM.Nounabv).Set("NounEqu", NM.NounEqu);
                    var flg = UpdateFlags.Multi;

                    var theResult = _nounModifierRepository.Add(NM);
                    theResult = _nounModifierRepository.Update(query, Updte, flg);

                    // Noun equalent
                    //var Updte1 = Update.Set("NounEqu", NM.NounEqu);
                    //var flg1 = UpdateFlags.Multi;
                    //var Qry = Query.EQ("Noun", ObjStr.Noun);                 
                    //_nounModifierRepository.Update(Qry, Updte1, flg1);


                    return theResult;

                }
                else
                {
                    query = Query.And(Query.EQ("Noun", NM.Noun), Query.EQ("Modifier", NM.Modifier));
                    NM.NounEqu = NM.NounEqu == null ? "" : NM.NounEqu;
                    NM.Nounabv = NM.Nounabv == null ? "" : NM.Nounabv;
                    NM.NounDefinition = NM.NounDefinition == null ? "" : NM.NounDefinition;
                    var Updte = Update.Set("NounDefinition", NM.NounDefinition).Set("Nounabv", NM.Nounabv).Set("NounEqu", NM.NounEqu);
                    var flg = UpdateFlags.Multi;

                    var theResult = _nounModifierRepository.Add(NM);
                    theResult = _nounModifierRepository.Update(query, Updte, flg);

                    // Noun equalent
                    //var Updte1 = Update.Set("NounEqu", NM.NounEqu);
                    //var flg1 = UpdateFlags.Multi;
                    //var Qry = Query.EQ("Noun", ObjStr.Noun);
                    //_nounModifierRepository.Update(Qry, Updte1, flg1);

                    return theResult;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());

            }

        }
        public virtual string[] AutoSearchNoun(string term)
        {
            var query = Query.And(Query.EQ("RP", "MM"), Query.Or(Query.Matches("ModifierEqu", BsonRegularExpression.Create(new Regex(string.Format("^{0}", term.TrimStart().TrimEnd()), RegexOptions.IgnoreCase))), Query.Matches("ModifierEqu", BsonRegularExpression.Create(new Regex("," + term, RegexOptions.IgnoreCase))), Query.Matches("NounEqu", BsonRegularExpression.Create(new Regex("," + term, RegexOptions.IgnoreCase))), Query.Matches("NounEqu", BsonRegularExpression.Create(new Regex(string.Format("^{0}", term.TrimStart().TrimEnd()), RegexOptions.IgnoreCase))), Query.Matches("Noun", BsonRegularExpression.Create(new Regex(string.Format("^{0}", term.TrimStart().TrimEnd()), RegexOptions.IgnoreCase)))));
            //var query = Query.Or( Query.Matches("ModifierEqu", BsonRegularExpression.Create(new Regex(string.Format("^{0}", term.TrimStart().TrimEnd()), RegexOptions.IgnoreCase))), Query.Matches("ModifierEqu", BsonRegularExpression.Create(new Regex("," + term, RegexOptions.IgnoreCase))), Query.Matches("NounEqu", BsonRegularExpression.Create(new Regex(","+term, RegexOptions.IgnoreCase))),Query.Matches("NounEqu", BsonRegularExpression.Create(new Regex(string.Format("^{0}", term.TrimStart().TrimEnd()), RegexOptions.IgnoreCase))), Query.Matches("Noun", BsonRegularExpression.Create(new Regex(string.Format("^{0}", term.TrimStart().TrimEnd()),  RegexOptions.IgnoreCase))));
            var arrResult = _nounModifierRepository.AutoSearch(query, "Noun");           
            return arrResult;
            //  new BsonRegularExpression(string.Format("^{0}", term));
            // var result = Mongodb.GetCollection("Prosol_Charateristics").Find(query).SetFields("Noun").ToArray();
            //var result = await col.Find(new BsonDocument("Email", model.Email)).ToListAsync();



        }
        public virtual string[] AutoSearchAssetNoun(string term)
        {
            var query = Query.And(Query.EQ("RP", "Equ"), Query.Or(Query.Matches("ModifierEqu", BsonRegularExpression.Create(new Regex(string.Format("^{0}", term.TrimStart().TrimEnd()), RegexOptions.IgnoreCase))), Query.Matches("ModifierEqu", BsonRegularExpression.Create(new Regex("," + term, RegexOptions.IgnoreCase))), Query.Matches("NounEqu", BsonRegularExpression.Create(new Regex("," + term, RegexOptions.IgnoreCase))), Query.Matches("NounEqu", BsonRegularExpression.Create(new Regex(string.Format("^{0}", term.TrimStart().TrimEnd()), RegexOptions.IgnoreCase))), Query.Matches("Noun", BsonRegularExpression.Create(new Regex(string.Format("^{0}", term.TrimStart().TrimEnd()), RegexOptions.IgnoreCase)))));
            var arrResult = _nounModifierRepository.AutoSearch(query, "Noun");           
            return arrResult;
            //  new BsonRegularExpression(string.Format("^{0}", term));
            // var result = Mongodb.GetCollection("Prosol_Charateristics").Find(query).SetFields("Noun").ToArray();
            //var result = await col.Find(new BsonDocument("Email", model.Email)).ToListAsync();



        }
        public virtual string[] AutoSearchModifier(string term,string Noun)
        {

            var query = Query.And(Query.EQ("Noun", Noun), Query.Matches("Modifier", BsonRegularExpression.Create(new Regex(term, RegexOptions.IgnoreCase))));
            var arrResult = _nounModifierRepository.AutoSearch(query, "Modifier");
            return arrResult;
            //  new BsonRegularExpression(string.Format("^{0}", term));
            // var result = Mongodb.GetCollection("Prosol_Charateristics").Find(query).SetFields("Noun").ToArray();
            //var result = await col.Find(new BsonDocument("Email", model.Email)).ToListAsync();
        }
        public virtual Prosol_NounModifiers GetNounModifier(string Noun, string Modifier)
        {
            var sort = SortBy.Ascending("Noun");
            var query = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier));
            var Nm = _nounModifierRepository.FindOne(query);
             if (Nm !=null && Nm.ImageId != null)
            {
                var query1 = Query.EQ("_id", new ObjectId(Nm.ImageId));
                byte[] byt = _nounModifierRepository.GridFsFindOne(query1);
                if(byt != null)
                Nm.FileData = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(byt));
            }
            return Nm;
        }
        public virtual Prosol_NounModifiers GetNounDetail(string Noun)
        {
            var query = Query.EQ("Noun", Noun);
            var Nm = _nounModifierRepository.FindOne(query);           
            return Nm;
        }
        private static ImageFormat GetImageFormat(string imageType)
        {
            ImageFormat imageFormat;
            switch (imageType)
            {
                case "image/jpg":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case "image/jpeg":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case "image/pjpeg":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case "image/gif":
                    imageFormat = ImageFormat.Gif;
                    break;
                case "image/png":
                    imageFormat = ImageFormat.Png;
                    break;
                case "image/x-png":
                    imageFormat = ImageFormat.Png;
                    break;
                default:
                    throw new Exception("Unsupported image type !");
            }

            return imageFormat;
        }

        public virtual int BulkNounModifier(HttpPostedFileBase file)
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
                if (dt.Columns[0].ColumnName == "Noun" && dt.Columns[1].ColumnName == "Modifier" && dt.Columns[2].ColumnName == "NM Abbreviation"
                    && dt.Columns[3].ColumnName == "NM Definition" && dt.Columns[4].ColumnName == "Formatted")
                {
                    var LstNM = new List<Prosol_NounModifiers>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[0].ToString() != "" && dr[1].ToString() != "")
                        {
                            var NM_Mdl = new Prosol_NounModifiers();
                            NM_Mdl.Noun = dr[0].ToString();
                            NM_Mdl.Modifier = dr[1].ToString();
                            NM_Mdl.Nounabv = dr[2].ToString();
                            NM_Mdl.ModifierDefinition = dr[3].ToString();
                            NM_Mdl.RP = "MM";
                            NM_Mdl.Formatted = dr[4].ToString() != null ? Convert.ToInt16(dr[4]) : 1;
                            NM_Mdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                            LstNM.Add(NM_Mdl);
                        }
                    }
                    if (LstNM.Count > 0)
                    {
                        //IEnumerable<Prosol_NounModifiers> filteredList = LstNM.GroupBy(Prosol_NounModifiers => Prosol_NounModifiers.Modifier).Select(group => group.First())
                        //    .GroupBy(Prosol_NounModifiers => Prosol_NounModifiers.Modifier).Select(group => group.First());

                        List<Prosol_NounModifiers> filteredList = LstNM.GroupBy(p => new { p.Noun, p.Modifier }).Select(g => g.First()).ToList();
                        if (filteredList.Count > 0)
                        {
                            var fRes = new List<Prosol_NounModifiers>();
                            foreach (Prosol_NounModifiers nm in filteredList.ToList())
                            {
                                var query = Query.And(Query.EQ("Noun", nm.Noun), Query.EQ("Modifier", nm.Modifier), Query.EQ("RP" , "MM" ));
                                var ObjStr = _nounModifierRepository.FindOne(query);
                                if (ObjStr == null)
                                {
                                    fRes.Add(nm);

                                }
                            }
                            cunt = _nounModifierRepository.Add(fRes);

                        }
                    }
                }
                else return -1;
            }
            else return -1;
            return cunt;

        }

        public virtual IEnumerable<Prosol_NounModifiers> GetNounList()
        {
         
            var sort = SortBy.Ascending("Noun");
            string[] strArr = {"Noun", "NounDefinition","Formatted","RP" };
            var fields = Fields.Include(strArr).Exclude("_id");
            var qry = Query.EQ("RP", "MM");
            var arrResult = _nounModifierRepository.FindAll(fields, sort).ToList();           
            return arrResult;
            //  new BsonRegularExpression(string.Format("^{0}", term));
            // var result = Mongodb.GetCollection("Prosol_Charateristics").Find(query).SetFields("Noun").ToArray();
            //var result = await col.Find(new BsonDocument("Email", model.Email)).ToListAsync();



        }
        public virtual IEnumerable<Prosol_NounModifiers> GetAssetNounList()
        {
         
            var sort = SortBy.Ascending("Noun");
            string[] strArr = {"Noun", "NounDefinition","Formatted","RP" };
            var fields = Fields.Include(strArr).Exclude("_id");
            var qry = Query.EQ("RP","Equ");
            var arrResult = _nounModifierRepository.FindAll(qry).ToList();
            arrResult = arrResult.OrderBy(item => item.Noun).ToList();
            //arrResult = arrResult(fields, sort);           
            return arrResult;
            //  new BsonRegularExpression(string.Format("^{0}", term));
            // var result = Mongodb.GetCollection("Prosol_Charateristics").Find(query).SetFields("Noun").ToArray();
            //var result = await col.Find(new BsonDocument("Email", model.Email)).ToListAsync();



        }

        public virtual IEnumerable<Prosol_NounModifiers> GetNMinfo(string info)
        {
            var sort = SortBy.Ascending("Noun");
            string[] strArr = { "Noun", "Modifier", "Nounabv", "Similaritems" };
            var fields = Fields.Include(strArr).Exclude("_id");
            var arrResult = _nounModifierRepository.FindAll(fields, sort).ToList();         
            return arrResult;
           
            
        }
        public virtual IEnumerable<Prosol_Nounscrub> GetScrubNMinfo()
        {
          
            var scrubLst = _NounscrubRepository.FindAll().ToList();         

            return scrubLst;


        }

        public virtual IEnumerable<Prosol_NounModifiers> GetModifierList(string Noun)
        {         
            var sort = SortBy.Ascending("Modifier");
            var query = Query.EQ("Noun", Noun); 
            var arrResult = _nounModifierRepository.FindAll(query , sort).ToList();
            return arrResult;
            //  new BsonRegularExpression(string.Format("^{0}", term));
            // var result = Mongodb.GetCollection("Prosol_Charateristics").Find(query).SetFields("Noun").ToArray();
            //var result = await col.Find(new BsonDocument("Email", model.Email)).ToListAsync();
       }
        public virtual IEnumerable<Prosol_NounModifiers> GetAssetModifierList(string Noun)
        {         
            var sort = SortBy.Ascending("Modifier");
            var query = Query.And(Query.EQ("Noun", Noun), Query.EQ("RP", "Equ"));
            var arrResult = _nounModifierRepository.FindAll(query , sort).ToList();
            return arrResult;
            //  new BsonRegularExpression(string.Format("^{0}", term));
            // var result = Mongodb.GetCollection("Prosol_Charateristics").Find(query).SetFields("Noun").ToArray();
            //var result = await col.Find(new BsonDocument("Email", model.Email)).ToListAsync();
       }



        public virtual IEnumerable<Prosol_NounModifiers> GetNounModifierList()
        {
            var sort = SortBy.Ascending("Noun");           
            var arrResult = _nounModifierRepository.FindAll(sort).ToList();
            return arrResult;
            //  new BsonRegularExpression(string.Format("^{0}", term));
            // var result = Mongodb.GetCollection("Prosol_Charateristics").Find(query).SetFields("Noun").ToArray();
            //var result = await col.Find(new BsonDocument("Email", model.Email)).ToListAsync();



        }
        public virtual List<Prosol_NounModifiers> Getformat(string Noun, string Modifier)
        {
            var queryyy = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier));
            var arrResult = _nounModifierRepository.FindAll(queryyy).ToList();
            return arrResult;
        }
        public List<Prosol_UOMMODEL> getuomlist(string Noun, string Modifier)
        {
            var arrResult1 = new List<Prosol_UOMMODEL>();
            var query = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier));
            var arrResult = _nounModifierRepository.FindOne(query);
            if (arrResult != null && arrResult.uomlist != null)
            {
                var Lst1 = new List<ObjectId>();
                foreach (string str in arrResult.uomlist)
                {
                    Lst1.Add(new ObjectId(str));
                }
                var query1 = Query.In("_id", new BsonArray(Lst1));
                var arrResult2 = _UOMMODELRepository.FindAll(query1).ToList();
                arrResult1 = arrResult2;
            }

            return arrResult1;
        }

        public virtual string[] AutoSearchAssetValues(string term)
       {
            var query = Query.Matches("Value", BsonRegularExpression.Create(new Regex(string.Format("^{0}", term.TrimStart().TrimEnd()), RegexOptions.IgnoreCase)));
            var arrResult = _assetAbbrevateRepository.AutoSearch(query, "Value");
            return arrResult;
            //  new BsonRegularExpression(string.Format("^{0}", term));
            // var result = Mongodb.GetCollection("Prosol_Charateristics").Find(query).SetFields("Noun").ToArray();
            //var result = await col.Find(new BsonDocument("Email", model.Email)).ToListAsync();



        }

    }
}

