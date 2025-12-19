using Excel;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Prosol.Core
{
    public partial class AssetService : I_Asset 
    {
        private readonly IRepository<Prosol_AssetMaster> _assetRepository;
        private readonly IRepository<Prosol_Users> _UsercreateRepository;
        private readonly IRepository<Prosol_AssetBOM> _AssetBOMRepository;
        private readonly IRepository<Prosol_AssetAttributes> _AssetattriRepository;
        private readonly IRepository<Prosol_FARRepository> _FARMasterrep;
        private readonly IRepository<Prosol_SiteMaster> _SiteMaster;
        private readonly IRepository<Prosol_AssetTypeMaster> _AssetTypeMaster;
        private readonly IRepository<Prosol_FARMaster> _FARMaster;
        private readonly IRepository<Prosol_Business> _BusiMasterrep;
        private readonly IRepository<Prosol_MajorClass> _Majorep;
        private readonly IRepository<Prosol_MinorClass> _Minorep;
        private readonly IRepository<Prosol_Area> _Arearep;
        private readonly IRepository<Prosol_SubArea> _SubArearep;
        private readonly IRepository<Prosol_Location> _Locrep;
        private readonly IRepository<Prosol_Region> _Regionrep;
        private readonly IRepository<Prosol_EquipmentClass> _EquipClassrep;
        private readonly IRepository<Prosol_EquipmentType> _EquipTyperep;
        private readonly IRepository<Prosol_Identifier> _Identifierep;
        private readonly IRepository<Prosol_City> _Cityrep;
        private readonly IRepository<Prosol_Charateristics> _CharateristicRepository;
        private readonly IRepository<prosol_FARdashboard> _FardashboardRepository;
        private readonly IRepository<Prosol_GridSettings> _Gridrep;
        private readonly IRepository<Prosol_Sequence> _SequenceRepository;
        private readonly IRepository<Prosol_UOMSettings> _UOMRepository;
        private readonly IRepository<Prosol_NounModifiers> _nounModifierRepository;
        private readonly IRepository<Prosol_Abbrevate> _abbreivateRepository;
        private readonly IRepository<Prosol_AssetAbbrevate> _assetAbbreivateRepository;
        private readonly IRepository<Prosol_Attachment> _attchmentRepository;
        private readonly IRepository<Prosol_Attribute> _attributeRepository;
        private readonly IRepository<Prosol_Dashboard> _dashRepository;
        private readonly IRepository<Prosol_Master> _MasterRepository;
        private readonly IRepository<Prosol_RequestRunning> _ReqRunningRepository;
        private readonly IRepository<Prosol_Vendor> _VendorRepository;
        private readonly IRepository<Prosol_Funloc> _FuncLocRepository;
        public AssetService(IRepository<Prosol_AssetMaster> assetRepository,
            IRepository<Prosol_Users> UsercreateRepository,
            IRepository<Prosol_AssetBOM> AssetBOMRepository,
            IRepository<Prosol_Sequence> seqRepository,
            IRepository<Prosol_UOMSettings> UOMRepository,
            IRepository<Prosol_Attachment> attchmentRepository,
            IRepository<Prosol_NounModifiers> nounModifierRepository,
            IRepository<Prosol_AssetAttributes> AssetattriRepository,
            IRepository<Prosol_FARRepository> FARMasterrep,
            IRepository<Prosol_SiteMaster> SiteMaster,
            IRepository<Prosol_AssetTypeMaster> AssetTypeMaster,
            IRepository<Prosol_FARMaster> FARMaster,
            IRepository<Prosol_Business> BusiMasterrep,
            IRepository<Prosol_MajorClass> Majorep,
            IRepository<Prosol_MinorClass> Minorep,
            IRepository<Prosol_Area> Arearep,
            IRepository<Prosol_SubArea> SubArearep,
            IRepository<Prosol_Location> Locrep,
            IRepository<Prosol_Region> Regionrep,
            IRepository<Prosol_EquipmentClass> EquipClassrep,
            IRepository<Prosol_EquipmentType> EquipTyperep,
            IRepository<Prosol_Identifier> Identifierep,
            IRepository<Prosol_City> Cityrep,
             IRepository<Prosol_Charateristics> Characteristics,
               IRepository<prosol_FARdashboard> Fardashboard,
         IRepository<Prosol_GridSettings> Gridrep,
            IRepository<Prosol_Abbrevate> abbreivateRepository,
            IRepository<Prosol_AssetAbbrevate> assetAbbreivateRepository,
            IRepository<Prosol_Dashboard> dashRepository,
            IRepository<Prosol_Master> MasterRepository,
            IRepository<Prosol_RequestRunning> ReqRunningRepository,
            IRepository<Prosol_Vendor> VendorRepository,
            IRepository<Prosol_Funloc> FuncLocRepository,
            IRepository<Prosol_Attribute> attributeRepository)
        {
            this._assetRepository = assetRepository;
            this._UsercreateRepository = UsercreateRepository;
            this._AssetBOMRepository = AssetBOMRepository;
            this._AssetattriRepository = AssetattriRepository;
            this._FARMasterrep = FARMasterrep;
            this._SiteMaster = SiteMaster;
            this._AssetTypeMaster = AssetTypeMaster;
            this._FARMaster = FARMaster;
            this._BusiMasterrep = BusiMasterrep;
            this._Majorep = Majorep;
            this._Minorep = Minorep;
            this._Arearep = Arearep;
            this._SubArearep = SubArearep;
            this._Locrep = Locrep;
            this._Regionrep = Regionrep;
            this._EquipClassrep = EquipClassrep;
            this._EquipTyperep = EquipTyperep;
            this._Identifierep = Identifierep;
            this._Cityrep = Cityrep;
            this._CharateristicRepository = Characteristics;
            this._FardashboardRepository = Fardashboard;
            this._Gridrep = Gridrep;
            this._SequenceRepository = seqRepository;
            this._UOMRepository = UOMRepository;
            this._nounModifierRepository = nounModifierRepository;
            this._abbreivateRepository = abbreivateRepository;
            this._assetAbbreivateRepository = assetAbbreivateRepository;
            this._attchmentRepository = attchmentRepository;
            this._attributeRepository = attributeRepository;
            this._dashRepository = dashRepository;
            this._MasterRepository = MasterRepository;
            this._ReqRunningRepository = ReqRunningRepository;
            this._VendorRepository = VendorRepository;
            this._FuncLocRepository = FuncLocRepository;
        }

        public List<prosol_FARdashboard> GetFARInfo()
        {
            var res = _FardashboardRepository.FindAll().ToList();
            return res;

        }

        public bool Insertasset(Prosol_AssetMaster assetvalues, Prosol_AssetAttributes assetattri, HttpFileCollectionBase files)
        {

            var lstAtt = new List<Prosol_Attachment>();
            //assetvalues.Equipment_Short = generateShort(assetattri);
            //assetvalues.Equipment_Long = generateLong(assetattri, assetvalues.AdditionalInfo);
            //var Flc = generateFloc(assetvalues);
            //assetvalues.FLOC_Code = Flc[0];
            //assetvalues.FuncLocDesc = Flc[1];


            var res = _assetRepository.Add(assetvalues);

            var query = Query.EQ("UniqueId", assetattri.UniqueId);
            var result1 = _AssetattriRepository.FindOne(query);
            if (result1 != null)
            {
                result1.Noun = assetattri.Noun;
                result1.Modifier = assetattri.Modifier;
                result1.Characterisitics = assetattri.Characterisitics;
                var resul = _AssetattriRepository.Add(result1);
                if (files != null)
                {
                    if (!string.IsNullOrEmpty(assetvalues.Attachment))
                    {
                        for (int g = 0; g < files.Count; g++)
                        {
                            if (assetvalues.Attachment.Contains(files[g].FileName))
                            {
                                if (files[g] != null && files[g].ContentLength > 0)
                                {
                                    var atmnt = new Prosol_Attachment();
                                    atmnt.Itemcode = assetvalues.UniqueId;
                                    atmnt.FileName = files[g].FileName;
                                    atmnt.ContentType = files[g].ContentType;
                                    atmnt.FileSize = GetFileSize(Convert.ToDouble(files[g].ContentLength));

                                    var query1 = Query.And(Query.EQ("filename", files[g].FileName), Query.EQ("length", files[g].ContentLength));
                                    string fileId = _attchmentRepository.GetGridFsFileId(query1);
                                    if (string.IsNullOrEmpty(fileId))
                                        atmnt.FileId = _attchmentRepository.GridFsUpload(files[g].InputStream, atmnt.FileName);
                                    else atmnt.FileId = fileId;
                                    lstAtt.Add(atmnt);
                                }
                            }
                        }
                    }
                }
                if (lstAtt.Count > 0)
                    _attchmentRepository.Add(lstAtt);

            }
            else
            {
                if (files != null)
                {
                    if (!string.IsNullOrEmpty(assetvalues.Attachment))
                    {
                        for (int g = 0; g < files.Count; g++)
                        {
                            if (assetvalues.Attachment.Contains(files[g].FileName))
                            {
                                if (files[g] != null && files[g].ContentLength > 0)
                                {
                                    var atmnt = new Prosol_Attachment();
                                    atmnt.Itemcode = assetvalues.UniqueId;
                                    atmnt.FileName = files[g].FileName;
                                    atmnt.ContentType = files[g].ContentType;
                                    atmnt.FileSize = GetFileSize(Convert.ToDouble(files[g].ContentLength));

                                    var query1 = Query.And(Query.EQ("filename", files[g].FileName), Query.EQ("length", files[g].ContentLength));
                                    string fileId = _attchmentRepository.GetGridFsFileId(query1);
                                    if (string.IsNullOrEmpty(fileId))
                                        atmnt.FileId = _attchmentRepository.GridFsUpload(files[g].InputStream, atmnt.FileName);
                                    else atmnt.FileId = fileId;
                                    lstAtt.Add(atmnt);
                                }
                            }
                        }
                    }
                }
                if (lstAtt.Count > 0)
                    _attchmentRepository.Add(lstAtt);
                var result = _AssetattriRepository.Add(assetattri);
            }



            return res;
        }
        private string generateShort(Prosol_AssetAttributes assetattri)
        {
            string sd = "";
            if (assetattri != null && assetattri.Characterisitics != null && assetattri.Characterisitics.Count > 0)
            {
                sd = sd + assetattri.Noun;
                foreach (var att in assetattri.Characterisitics.OrderBy(x => x.ShortSquence))
                {
                    if (!string.IsNullOrEmpty(att.Value))
                    {
                        if (string.IsNullOrEmpty(att.UOM))
                            sd = sd + "," + att.Value;
                        else sd = sd + "," + att.Value + att.UOM;
                    }
                }
                return sd;

            }
            else return sd;
        }
        private string generateLong(Prosol_AssetAttributes assetattri, string additional)
        {
            string ld = "";
            if (assetattri != null && assetattri.Characterisitics != null && assetattri.Characterisitics.Count > 0)
            {
                ld = ld + assetattri.Noun;
                foreach (var att in assetattri.Characterisitics.OrderBy(x => x.ShortSquence))
                {
                    if (!string.IsNullOrEmpty(att.Value))
                    {
                        if (string.IsNullOrEmpty(att.UOM))
                            ld = ld + "," + att.Characteristic + ":" + att.Value;
                        else ld = ld + "," + att.Characteristic + ":" + att.Value + " " + att.UOM;
                    }
                }
                if (!string.IsNullOrEmpty(additional))
                {
                    ld = ld + ",ADDITIONAL INFORMATION:" + additional.ToUpper();
                }
                return ld;

            }
            else return ld;
        }

        private string[] generateFloc(Prosol_AssetMaster assetvalues)
        {
            string Floc = "", FuncLocDesc = "";
            var AllMaster = getAllCommonMaster();

            //var regname = AllMaster.Regions.Where(x => x._id.ToString() == assetvalues.Region).ToList();
            //if (regname.Count > 0)
            //{
            //    var regin = regname.Count > 0 ? regname[0].RegionCode : "";
            //    var regintxt = regname.Count > 0 ? regname[0].Region : "";
            //    Floc = Floc + regin;
            //    FuncLocDesc = FuncLocDesc + regintxt;
            //}


            //var Cityname = AllMaster.Cities.Where(x => x._id.ToString() == assetvalues.City).ToList();
            //if (Cityname.Count > 0)
            //{
            //    var City = Cityname.Count > 0 ? Cityname[0].CityCode : "";
            //    var Citytxt = Cityname.Count > 0 ? Cityname[0].City : "";
            //    Floc = Floc + "-" + City;
            //    FuncLocDesc = FuncLocDesc + "-" + Citytxt;

            //}


            //var Areaname = AllMaster.Areas.Where(x => x._id.ToString() == assetvalues.Area).ToList();
            //if (Areaname.Count > 0)
            //{
            //    var Area = Areaname.Count > 0 ? Areaname[0].AreaCode : "";
            //    var Areatxt = Areaname.Count > 0 ? Areaname[0].Area : "";
            //    Floc = Floc + "-" + Area;
            //    FuncLocDesc = FuncLocDesc + "-" + Areatxt;
            //}



            //var SubAreaname = AllMaster.SubAreas.Where(x => x._id.ToString() == assetvalues.SubArea).ToList();
            //if (SubAreaname.Count > 0)
            //{
            //    var SubArea = SubAreaname.Count > 0 ? SubAreaname[0].SubAreaCode : "";
            //    var SubAreatxt = SubAreaname.Count > 0 ? SubAreaname[0].SubArea : "";
            //    Floc = Floc + "-" + SubArea;
            //    FuncLocDesc = FuncLocDesc + "-" + SubAreatxt;

            //}

            //var Funcname = AllMaster.MinorClasses.Where(x => x._id.ToString() == assetvalues.MinorClass).ToList();
            //if (Funcname.Count > 0)
            //{
            //    var MinorClass = Funcname.Count > 0 ? Funcname[0].MinorCode : "";
            //    var MinorClasstxt = Funcname.Count > 0 ? Funcname[0].MinorClass : "";
            //    Floc = Floc + "-" + MinorClass;
            //    FuncLocDesc = FuncLocDesc + "-" + MinorClasstxt;
            //}


            //if (!string.IsNullOrEmpty(assetvalues.Identifier))
            //{
            //    var Identiname = AllMaster.Identifiers.Where(x => x._id.ToString() == assetvalues.Identifier).ToList();
            //    if (Identiname.Count > 0)
            //    {

            //        var Identifier = Identiname.Count > 0 ? Identiname[0].IdentifierCode : "";
            //        var Identifiertxt = Identiname.Count > 0 ? Identiname[0].Identifier : "";
            //        Floc = Floc + "-" + Identifier;
            //        FuncLocDesc = FuncLocDesc + "-" + Identifiertxt;
            //    }
            //}
            string[] strArr = new string[] { Floc, FuncLocDesc };
            return strArr;
        }
        public bool AssetRework(string UniqueId, string RevRemarks, string CondRemarks, string ImageRemarks, string AddRemarks, int sts)
        {
            var res = false;
            var query = Query.EQ("UniqueId", UniqueId);
            var vn = _assetRepository.FindOne(query);
            if (vn != null)
            {
                vn.ItemStatus = sts;
                vn.Rework = "Cat";
                vn.Rework_Remarks = RevRemarks;
                vn.assetConditionRemarks = CondRemarks;
                vn.Remarks = ImageRemarks;
                vn.AdditionalNotes = AddRemarks;
                if (sts == 1)
                {
                    vn.Rework = "PV";
                    vn.Catalogue = null;
                    vn.Review = null;
                    vn.PVstatus = null;
                }
                // vn.Review = null;               
                res = _assetRepository.Add(vn);

            }
            return res;

        }
        public bool insertImages(Prosol_AssetMaster imgAsset)
        {
            var reslt = _assetRepository.Add(imgAsset);
            return reslt;

        }

        public bool insertBomImages(Prosol_AssetBOM imgBom)
        {
            var reslt = _AssetBOMRepository.Add(imgBom);
            return reslt;

        }
        public Prosol_AssetAttributes GetAttributeInfo(string UniqueId)
        {
            var query = Query.EQ("UniqueId", UniqueId);
            var genRes = _AssetattriRepository.FindOne(query);
            return genRes;

        }
        public Prosol_AssetMaster getasset(string UniqueId)
        {
            var query = Query.EQ("UniqueId", UniqueId);
            var res = _assetRepository.FindOne(query);
            return res;
        }

        public IEnumerable<Prosol_AssetMaster> GetAssetDataList(string uId)
        {
            var sort = SortBy.Ascending("ItemStatus");

            var query = Query.Or(
                Query.And(
                Query.EQ("Catalogue.UserId", uId), Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3))
                ),
                Query.And(
                Query.EQ("Review.UserId", uId), Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 5))
                )
                //Query.And(
                //Query.EQ("Release.UserId", uId), Query.Or(Query.EQ("ItemStatus", 6), Query.EQ("ItemStatus", 7), Query.EQ("ItemStatus", 9))
                //)
                );  

            var arrResult = _assetRepository.FindAll(query, sort).ToList();
            //var arrResult = _assetRepository.FindAll().ToList();
            return arrResult;

        }
        public bool InserGridflds(List<Prosol_GridSettings> flds)
        {
            var res = _Gridrep.Add(flds);
            return true;
        }
        public int ApproveFAR(List<Prosol_AssetMaster> flds)
        {
            int cnt = 0;
            foreach (var un in flds)
            {
                //var query = Query.EQ("UniqueId", un.UniqueId);
                //var appdata = _assetRepository.FindOne(query);

                //appdata.Release = un.Release;
                //appdata.ItemStatus = un.ItemStatus;
                //if (un.CLF_Remarks != null)
                {
                    //if (appdata.CLF_Remarks != null && appdata.CLF_Remarks.Count > 0)
                    //{

                    //    foreach (var dl in un.CLF_Remarks)
                    //    {
                    //        appdata.CLF_Remarks.Add(dl);
                    //    }

                    //}
                    //else
                    //{
                    //    appdata.CLF_Remarks = un.CLF_Remarks;

                    //}
                }
                //var res = _assetRepository.Add(appdata);
                cnt++;
            }

            return cnt;
        }
        public List<Prosol_GridSettings> getAllFields(string UserId)
        {
            var Qry = Query.EQ("UserId", UserId);

            var Lst = _Gridrep.FindAll(Qry).ToList();

            return Lst;
        }

        public Prosol_AssetMaster GetAssetInfo(string UniqueId)
        {
            var query = Query.Or(Query.EQ("AssetNo", UniqueId), Query.EQ("UniqueId", UniqueId));
            var genRes = _assetRepository.FindOne(query);
            return genRes;

        }
        public int submitasset(List<Prosol_AssetMaster> AssetList)
        {
            int cont = 0;
            bool res = false;

            foreach (Prosol_AssetMaster mdl in AssetList)
            {
                var query = Query.And(Query.EQ("UniqueId", mdl.UniqueId));
                var oneResult = _assetRepository.FindOne(query);

                if (oneResult.Catalogue == null)
                {
                    var cataloguer = new Prosol_UpdatedBy();
                    cataloguer.UserId = oneResult.Catalogue.UserId;
                    cataloguer.Name = oneResult.Catalogue.Name;
                    cataloguer.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    oneResult.Catalogue = cataloguer;
                }

                var reviewer = new Prosol_UpdatedBy();
                if (mdl.ItemStatus == 4)
                {
                    var UseQry = Query.And(Query.EQ("Userid", oneResult.Catalogue.UserId), Query.EQ("Roles.Name", "Cataloguer"));
                    var usrObj = _UsercreateRepository.FindOne(UseQry);

                    if (usrObj != null)
                    {
                        foreach (var trg in usrObj.Roles)
                        {
                            if (trg.Name == "Cataloguer")
                            {
                                UseQry = Query.And(Query.EQ("Userid", trg.TargetId), Query.EQ("Roles.Name", "Reviewer"));
                                usrObj = _UsercreateRepository.FindOne(UseQry);
                                reviewer.UserId = usrObj.Userid;
                                reviewer.Name = usrObj.UserName;
                                reviewer.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                                oneResult.Review = reviewer;
                                break;
                            }
                        }

                    }

                }
                var Releaser = new Prosol_UpdatedBy();
                if (mdl.ItemStatus == 6)
                {
                    var UseQry = Query.And(Query.EQ("Userid", oneResult.Review.UserId), Query.EQ("Roles.Name", "Reviewer"));
                    var usrObj = _UsercreateRepository.FindOne(UseQry);

                    if (usrObj != null)
                    {
                        foreach (var trg in usrObj.Roles)
                        {
                            if (trg.Name == "Reviewer")
                            {
                                UseQry = Query.And(Query.EQ("Userid", trg.TargetId), Query.EQ("Roles.Name", "Releaser"));
                                usrObj = _UsercreateRepository.FindOne(UseQry);

                                Releaser.UserId = usrObj.Userid;
                                Releaser.Name = usrObj.UserName;
                                Releaser.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                                oneResult.Release = Releaser;
                                break;
                            }
                        }

                    }

                }






                //oneResult.UpdatedOn = mdl.UpdatedOn;
                oneResult.ItemStatus = mdl.ItemStatus;

                res = _assetRepository.Add(oneResult);

                if (res) cont++;



            }
            return cont;
        }
        public IEnumerable<Prosol_Users> getuserAsset(string role)
        {
            //string[] userflds = { "Cataloguer", "Userid" };
            //var fields = Fields.Include(userflds).Exclude("_id");
            var query = Query.And(Query.EQ("Roles.Name", role));
            var mxplnt = _UsercreateRepository.FindAll(query).ToList();
            return mxplnt;
        }
        public IEnumerable<Prosol_Users> getpvuser()
        {
            var query = Query.And(Query.EQ("Roles.Name", "PV User"), Query.EQ("Islive", "Active"));
            var mxplnt = _UsercreateRepository.FindAll(query).ToList();
            return mxplnt;
        }
        // Get all Asset data
        //public IEnumerable<Prosol_AssetMaster> getallassetdata(string FARId, string Role, string User, string Status, string fromdate, string todate)
        //{
        //    IMongoQuery qury ,qury1=null, qury2, qury3 ;
        //    if (Role == "PV User")
        //    {
        //        Role = "PVuser";
        //        Status = "PV";
        //    }
        //    else if (Role == "Cataloguer")
        //    {
        //        Role = "Catalogue";
        //        Status = "Catalogue";
        //    }
        //    else if (Role == "Reviewer")
        //    {
        //        Role = "Review";
        //        Status = "QC";
        //    }

        //    if (Role != null && Role != "undefined"&& Role != "")
        //    {
        //        if (User != null)
        //        {
        //            qury1 = Query.EQ(Role + ".Name", User);
        //        }
        //    }
        //    if (Status != null && Status != "")
        //    {
        //       if(qury1 != null)
        //        {
        //            if (Status == "PV")
        //            {
        //                qury1 = Query.And(qury1, Query.Or(Query.EQ("ItemStatus", 1)));
        //            }
        //            else if (Status == "Catalogue")
        //            {
        //                qury1 = Query.And(qury1, Query.And(Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3))));
        //            }
        //            else if (Status == "QC")
        //            {
        //                qury1 = Query.And(qury1, Query.And(Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 5))));
        //            }
        //            else if (Status == "Released")
        //            {
        //                qury1 = Query.And(qury1, Query.And(Query.EQ("ItemStatus", 6)));
        //            }
        //            else if (Status == "All")
        //            {
        //                qury1 = Query.And(qury1, Query.GTE("ItemStatus", 0));
        //            }
        //        }
        //        else
        //        {
        //            if (Status == "PV")
        //            {
        //                qury1 = Query.And( Query.Or(Query.EQ("ItemStatus", 1)));
        //            }
        //            else if (Status == "Catalogue")
        //            {
        //                qury1 = Query.And( Query.And(Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3))));
        //            }
        //            else if (Status == "QC")
        //            {
        //                qury1 = Query.And( Query.And(Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 5))));
        //            }
        //            else if (Status == "Released")
        //            {
        //                qury1 = Query.And( Query.And(Query.EQ("ItemStatus", 6)));
        //            }
        //            else if (Status == "All")
        //            {
        //                qury1 = Query.And( Query.GTE("ItemStatus", 0));
        //            }
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
        //    {
        //        var date = DateTime.Parse(fromdate, new CultureInfo("en-US", true));
        //        date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        //        var date1 = DateTime.Parse(todate, new CultureInfo("en-US", true));
        //        date1 = date1.AddDays(1);
        //        date1 = DateTime.SpecifyKind(date1, DateTimeKind.Utc);
        //        if (Status == "PV")
        //        {
        //            qury1 = Query.And(qury1, Query.GTE("PVuser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("PVuser.UpdatedOn", BsonDateTime.Create(date1)));
        //        }
        //        else if (Status == "Catalogue")
        //        {
        //            qury1 = Query.And(qury1, Query.GTE("Catalogue.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Catalogue.UpdatedOn", BsonDateTime.Create(date1)));
        //        }
        //        else if (Status == "QC")
        //        {
        //            qury1 = Query.And(qury1, Query.GTE("Review.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Review.UpdatedOn", BsonDateTime.Create(date1)));
        //        }
        //        else if (Status == "Released")
        //        {
        //            qury1 = Query.And(qury1, Query.GTE("Release.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Release.UpdatedOn", BsonDateTime.Create(date1)));
        //        }
        //        else
        //        {
        //            qury1 = Query.And(qury1, Query.Or(Query.And(Query.GTE("PVuser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("PVuser.UpdatedOn", BsonDateTime.Create(date1))),
        //                Query.And(Query.GTE("Catalogue.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Catalogue.UpdatedOn", BsonDateTime.Create(date1))),
        //                Query.And(Query.GTE("Review.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Review.UpdatedOn", BsonDateTime.Create(date1))),
        //                Query.And(Query.GTE("Release.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Release.UpdatedOn", BsonDateTime.Create(date1)))));
        //        }
        //    }
        //    else if (!string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
        //    {
        //        var date = DateTime.Parse(fromdate, new CultureInfo("en-US", true));
        //        date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

        //        if (Status == "PV")
        //        {
        //            qury1 = Query.And(qury1, Query.GTE("PVuser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("PVuser.UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
        //        }
        //        else if (Status == "Catalogue")
        //        {
        //            qury1 = Query.And(qury1, Query.GTE("Catalogue.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Catalogue.UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
        //        }
        //        else if (Status == "QC")
        //        {
        //            qury1 = Query.And(qury1, Query.GTE("Review.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Review.UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
        //        }
        //        else if (Status == "Released")
        //        {
        //            qury1 = Query.And(qury1, Query.GTE("Release.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Release.UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))));
        //        }
        //        else
        //        {
        //            qury1 = Query.And(qury1, Query.Or(Query.And(Query.GTE("PVuser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("PVuser.UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)))),
        //                Query.And(Query.GTE("Catalogue.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Catalogue.UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)))),
        //                Query.And(Query.GTE("Review.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Review.UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)))),
        //                Query.And(Query.GTE("Release.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Release.UpdatedOn", BsonDateTime.Create(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))))));
        //        }
        //    }

        //    //if (FARId != null && FARId != "undefined" && Role != null && User != null)
        //    //{
        //    //    qury1 = Query.And(Query.EQ("FixedAssetNo", FARId), Query.EQ(Role + ".Name", User));

        //    //    if (FARId == null)
        //    //    {
        //    //        qury1 = Query.EQ(Role + ".Name", User);
        //    //    }
        //    //    else if (Role == null || User == null || Role == "undefined" && User == "")
        //    //    {
        //    //        qury1 = Query.EQ("FixedAssetNo", FARId); 
        //    //    }
        //    //}
        //    //else if (FARId == null || FARId == "undefined" && Role != null  && Role != "undefined" && User != null && User != "")
        //    //{
        //    //    qury1 = Query.EQ(Role + ".Name", User);
        //    //}
        //    //else
        //    //{
        //    //    qury1 = Query.LTE("ItemStatus", 6);
        //    //    if (Status != null && Status != "")
        //    //    {
        //    //        if(Status == "PV")
        //    //        {
        //    //            qury1 = Query.EQ("ItemStatus", 1);
        //    //        }
        //    //        else if(Status == "Catalogue")
        //    //        {
        //    //            qury1 = Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3));
        //    //        }
        //    //        else if(Status == "QC")
        //    //        {
        //    //            qury1 = Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 5));
        //    //        }
        //    //        else if(Status == "Released")
        //    //        {
        //    //            qury1 = Query.EQ("ItemStatus", 6);
        //    //        }
        //    //    }
        //    //}
        //    var srt = SortBy.Ascending("UniqueId");
        //    var res = _assetRepository.FindAll(qury1, srt).ToList();
        //    return res;

        //}
        private List<string> getallassetcodes(string FARId, string Role, string User, string Status, string fromdate, string todate)
        {
            IMongoQuery query = null;

            if (Role == "PV User")
            {
                Role = "PVuser";
                Status = "PV";
            }
            else if (Role == "Cataloguer")
            {
                Role = "Catalogue";
                Status = "Catalogue";
            }
            else if (Role == "Reviewer")
            {
                Role = "Review";
                Status = "QC";
            }

            if (!string.IsNullOrEmpty(Role) && Role != "undefined")
            {
                if (!string.IsNullOrEmpty(User))
                {
                    query = Query.EQ(Role + ".Name", User);
                }
            }

            if (!string.IsNullOrEmpty(Status))
            {
                query = BuildStatusQuery(query, Status);
            }

            if (!string.IsNullOrEmpty(fromdate))
            {
                query = ApplyDateFilter(query, Status, fromdate, todate);
            }

            var sort = SortBy.Ascending("UniqueId");
            var Assetlist = _assetRepository.FindAll(query).ToList();
            var codeLst = Assetlist.GroupBy(i => i.UniqueId).Select(g => g.Key).ToList();

            return codeLst;
        }

        //public List<Dictionary<string, object>> getallassetdata(string FARId, string Role, string User, string Status, string fromdate, string todate)
        //{
        //    IMongoQuery query = null;
        //    var lockObject = new object();

        //    if (Role == "PV User")
        //    {
        //        Role = "PVuser";
        //        Status = "PV";
        //    }
        //    else if (Role == "Cataloguer")
        //    {
        //        Role = "Catalogue";
        //        Status = "Catalogue";
        //    }
        //    else if (Role == "Reviewer")
        //    {
        //        Role = "Review";
        //        Status = "QC";
        //    }

        //    if (!string.IsNullOrEmpty(Role) && Role != "undefined")
        //    {
        //        if (!string.IsNullOrEmpty(User))
        //        {
        //            query = Query.EQ(Role + ".Name", User);
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(Status))
        //    {
        //        query = BuildStatusQuery(query, Status);
        //    }

        //    if (!string.IsNullOrEmpty(fromdate))
        //    {
        //        query = ApplyDateFilter(query, Status, fromdate, todate);
        //    }

        //    var sort = SortBy.Ascending("UniqueId");
        //    string[] str = { "UniqueId", "FixedAssetNo", "SiteId", "AssetNo", "pvAssetNo", "BarcodeNumber", "OldTagNo", "AssetQRCode", "Description", "Parent", "Location", "LocationHierarchy", "ClassificationHierarchyDesc", "AggregatedClassSpecAttr", "Status", "AssetType", "FailureCode", "Priority", "MaintaineBy", "WarrentyExpDate", "Name", "SerialNo", "ModelNo", "ModelYear", "AssCondition", "OwnedBySite", "Manufacturer", "ReportGroup", "Vendor", "PurchasePrice", "InstallDate", "PO_Contract", "LoadCertExpDate", "CalCertExpDate", "CertExpDate", "TrafficCertExpDate", "Ownedby", "Maintainer", "PresentLocation", "Operatedby", "AdditionalNotes", "GIS", "AssetCondition", "AssetImages", "Equipment_Short", "Equipment_Long", "PVuser", "PVstatus", "Catalogue", "Review", "Soureurl", "Remarks", "Rework_Remarks", "ItemStatus" };
        //    var fields = Fields.Include(str);
        //    var Assetlist = _assetRepository.FindAll(fields, query).ToList();

        //    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //    //Dictionary<string, object> row;
        //    var allAttr = _AssetattriRepository.FindAll().ToList();


        //    System.Threading.Tasks.Parallel.ForEach(Assetlist, cde =>
        //    {
        //        var row = new Dictionary<string, object>();

        //        var assetNo = cde.AssetNo ?? "";
        //        var pvAssetNo = (cde.pvAssetNo != null && cde.pvAssetNo != assetNo) ? cde.pvAssetNo : "";

        //        row.Add("Unique ID", cde.UniqueId);
        //        row.Add("Asset Number", assetNo);

        //        string maximoCount = "";
        //        if (cde.AssetImages != null)
        //        {
        //            if (cde.AssetImages.MatImgs != null && cde.AssetImages.MatImgs.Length > 0)
        //            {
        //                row.Add("Maximo Status", "Image Available");
        //                maximoCount = Convert.ToString(cde.AssetImages.MatImgs.Length);
        //                row.Add("Maximo Image Count", maximoCount);
        //            }
        //            else
        //            {
        //                row.Add("Maximo Status", "Image Not Available");
        //                row.Add("Maximo Image Count", "0");
        //            }
        //        }
        //        else
        //        {
        //            row.Add("Maximo Status", "Image Not Available");
        //            row.Add("Maximo Image Count", "0");
        //        }
        //        string status;
        //        switch (cde.ItemStatus)
        //        {
        //            case 0:
        //                status = "PV Not Assigned";
        //                break;
        //            case 1:
        //                status = "PV Pending";
        //                break;
        //            case 2:
        //                status = "Catalogue Pending";
        //                break;
        //            case 3:
        //                status = "Catalogue Saved";
        //                break;
        //            case 4:
        //                status = "QC Pending";
        //                break;
        //            case 5:
        //                status = "QC Saved";
        //                break;
        //            default:
        //                status = "Released";
        //                break;
        //        }
        //        row.Add("Status", status);

        //        lock (lockObject)
        //        {
        //            rows.Add(row);
        //        }
        //    });


        //    return rows;
        //}

        public List<Dictionary<string, object>> getallassetdata(string FARId, string Role, string User, string Status, string fromdate, string todate)
        {
            IMongoQuery query = null;
            var lockObject = new object();

            if (Role == "PV User")
            {
                Role = "PVuser";
                Status = "PV";
            }
            else if (Role == "Cataloguer")
            {
                Role = "Catalogue";
                Status = "Catalogue";
            }
            else if (Role == "Reviewer")
            {
                Role = "Review";
                Status = "QC";
            }

            if (!string.IsNullOrEmpty(Role) && Role != "undefined")
            {
                if (!string.IsNullOrEmpty(User))
                {
                    query = Query.EQ(Role + ".Name", User);
                }
            }

            if (!string.IsNullOrEmpty(Status))
            {
                query = BuildStatusQuery(query, Status);
            }

            if (!string.IsNullOrEmpty(fromdate))
            {
                query = ApplyDateFilter(query, Status, fromdate, todate);
            }

            var sort = SortBy.Ascending("UniqueId");
            string[] str = { "UniqueId", "FixedAssetNo", "SiteId", "AssetNo", "pvAssetNo", "BarcodeNumber", "OldTagNo", "AssetQRCode", "Description", "Parent", "Location", "LocationHierarchy", "ClassificationHierarchyDesc", "AggregatedClassSpecAttr", "Status", "AssetType", "FailureCode", "Priority", "MaintaineBy", "WarrentyExpDate", "Name", "SerialNo", "ModelNo", "ModelYear", "AssCondition", "OwnedBySite", "Manufacturer", "ReportGroup", "Vendor", "PurchasePrice", "InstallDate", "PO_Contract", "LoadCertExpDate", "CalCertExpDate", "CertExpDate", "TrafficCertExpDate", "Ownedby", "Maintainer", "PresentLocation", "Operatedby", "AdditionalNotes", "GIS", "AssetCondition", "AssetImages", "Equipment_Short", "Equipment_Long", "PVuser", "PVstatus", "Catalogue", "Review", "Soureurl", "Remarks", "Rework_Remarks", "ItemStatus" };
            var fields = Fields.Include(str);
            var Assetlist = _assetRepository.FindAll(fields, query).ToList();

            //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            //Dictionary<string, object> row;
            var allAttr = _AssetattriRepository.FindAll().ToList();


            var rows = new List<Dictionary<string, object>>();

            //System.Threading.Tasks.Parallel.ForEach(Assetlist, cde =>
            //{

            //    var row = new Dictionary<string, object>();

            //    var assetNo = cde.AssetNo ?? "";
            //    var pvAssetNo = (cde.pvAssetNo != null && cde.pvAssetNo != assetNo) ? cde.pvAssetNo : "";

            //    row.Add("SITEID", cde.UniqueId);
            //    row.Add("ASSETNUM", assetNo);
            //    row.Add("ASSETDESCRIPTION", cde.Description ?? "");
            //    row.Add("ASSETLONGDESCRIPTION", cde.Description_ ?? "");
            //    row.Add("STATUS", cde.Plant ?? "");
            //    row.Add("ASSETTYPE", cde.Func_Location ?? "");
            //    row.Add("SOURCESYSID", cde.SourceId ?? "");
            //    row.Add("PARENT", cde.Parent ?? "");
            //    row.Add("LOCATION", cde.Location ?? "");
            //    row.Add("LOCATION_DESCRIPTION", cde.LocationHierarchy ?? "");
            //    row.Add("LOCATION_HIERARCHY", cde.LocationHierarchy ?? "");
            //    row.Add("FAILURECODE", cde.FailureCode ?? "");
            //    row.Add("MAINTAINEDBY", cde.MaintaineBy ?? "");
            //    row.Add("OPERATEDBY", cde.Operatedby ?? "");
            //    row.Add("BINNUM", cde.BinNo ?? "");
            //    row.Add("ITEMNUM", cde.ItemNo ?? "");
            //    row.Add("CONDITIONCODE", cde.ConditionCode ?? "");
            //    row.Add("ITEMTYPE", cde.ObjType ?? "");
            //    row.Add("TOOLRATE", cde.ToolRate ?? "");
            //    row.Add("ASSETTAG", cde.AssetQRCode ?? "");
            //    row.Add("ASSCOND", (cde.AssCondition != null && cde.AssCondition != "Excellent") ? cde.AssCondition : "");
            //    row.Add("PMNUM", "");
            //    row.Add("MAINTENANCE_EXCEPTION", cde.Maintain_Exception);
            //    row.Add("DEFAULTREPFACSITEID", cde.DefaultRepSideId);
            //    row.Add("COMMISION DATE", cde.InstallDate);
            //    row.Add("CALENDAR", cde.Calendar);
            //    row.Add("SHIFT", cde.Shift);
            //    row.Add("FAILURE_CONSEQUENCE", cde.Failure_Consequence);
            //    row.Add("FAILURE_LIKELIHOOD", cde.Failure_Hood);
            //    row.Add("Unique ID", cde.UniqueId);
            //    row.Add("Fixedassetnum(FAR ID)", cde.FixedAssetNo ?? "");
            //    row.Add("PV Asset Number", pvAssetNo);
            //    row.Add("Existing BarCode(Non-Readable)", cde.BarcodeNumber ?? "");
            //    row.Add("Old Asset Barcode(Readable)", cde.OldTagNo ?? "");
            //    row.Add("Classificationhierarchy", cde.ClassificationHierarchyDesc ?? "");
            //    row.Add("Aggregatedclassspecattributes", cde.AggregatedClassSpecAttr ?? "");
            //    row.Add("Asset Status", cde.Status ?? "");
            //    row.Add("Priority", cde.Priority ?? "");
            //    row.Add("Warrantyexpdate", cde.WarrentyExpDate ?? "");
            //    row.Add("Name", cde.Name ?? "");
            //    row.Add("Serial No", cde.SerialNo ?? "");
            //    row.Add("Model No", cde.ModelNo ?? "");
            //    row.Add("Model Year", cde.ModelYear ?? "");
            //    row.Add("OwnedBySite", cde.OwnedBySite ?? "");
            //    row.Add("Manufacturer", cde.Manufacturer ?? "");
            //    row.Add("ReportGroup", cde.ReportGroup ?? "");
            //    row.Add("Vendor", cde.Vendor ?? "");
            //    row.Add("PurchasePrice", cde.PurchasePrice ?? "");
            //    row.Add("InstallDate", cde.InstallDate ?? "");
            //    row.Add("PO Contract", cde.PO_Contract ?? "");
            //    row.Add("Loadcertexpdate", cde.LoadCertExpDate ?? "");
            //    row.Add("Calcertexpdate", cde.CalCertExpDate ?? "");
            //    row.Add("Certexpdate", cde.CertExpDate ?? "");
            //    row.Add("Trafficcertexpdate", cde.TrafficCertExpDate ?? "");
            //    row.Add("Ownedby", cde.Ownedby ?? "");
            //    row.Add("Maintaineby", cde.Maintainer ?? "");
            //    row.Add("Present Location", cde.PresentLocation ?? "");
            //    row.Add("Additional Notes", cde.AdditionalNotes ?? "");

            //    if (cde.GIS != null && !string.IsNullOrEmpty(cde.GIS.LongitudeStart))
            //    {
            //        cde.GIS.LongitudeStart = cde.GIS.LongitudeStart.Replace('W', 'E');
            //        row.Add("Location Coordinates", cde.GIS.LongitudeStart + "," + (cde.GIS.LattitudeStart ?? ""));
            //    }
            //    else
            //    {
            //        row.Add("Location Coordinates", "");
            //    }

            //    // Simplified asset condition logic
            //    string pvCond = "";
            //    if (cde.AssetCondition != null)
            //    {
            //        if (cde.AssetCondition.Rank == "7") pvCond = "Excellent";
            //        else if (cde.AssetCondition.Rank == "6") pvCond = "Very Good";
            //        else if (cde.AssetCondition.Rank == "4" || cde.AssetCondition.Rank == "5") pvCond = "Good";
            //        else if (cde.AssetCondition.Rank == "2" || cde.AssetCondition.Rank == "3") pvCond = "Poor";
            //        else if (cde.AssetCondition.Rank == "1") pvCond = "Very Poor";

            //        if (cde.AssetCondition.Leakage == "Medium") pvCond = "Good";
            //        if (cde.AssetCondition.Damage == "High" && cde.AssetCondition.Corrosion == "High") pvCond = "Very Poor";
            //        if (cde.AssetCondition.Corrosion == "No" && cde.AssetCondition.Leakage == "No" &&
            //            cde.AssetCondition.Vibration == "No" && cde.AssetCondition.Temparature == "No" &&
            //            cde.AssetCondition.Damage == "No" && cde.AssetCondition.Smell == "No" &&
            //            cde.AssetCondition.Noise == "No") pvCond = "Excellent";
            //    }

            //    row.Add("PV Asset Condition", pvCond);

            //    row.Add("Nameplate Text", cde.AssetImages != null && cde.AssetImages.NamePlateText != null && cde.AssetImages.NamePlateText.Length >= 1 ? cde.AssetImages.NamePlateText[0] : "");

            //    //var classification = GetAttributeInfo(cde.UniqueId);
            //    var classification = allAttr.Find(i => i.UniqueId == cde.UniqueId);

            //    if (classification != null)
            //    {
            //        string nm = "";
            //        if (!string.IsNullOrEmpty(classification.Noun) && !string.IsNullOrEmpty(classification.Modifier))
            //        {
            //            nm = classification.Noun + "," + classification.Modifier;
            //        }
            //        else if (!string.IsNullOrEmpty(classification.Noun) && (string.IsNullOrEmpty(classification.Modifier) || classification.Modifier == "--"))
            //        {
            //            nm = classification.Noun;
            //        }
            //        row.Add("Classificationid", nm);
            //    }
            //    else
            //    {
            //        row.Add("Classificationid", "");
            //    }

            //    row.Add("Short Desc", cde.Equipment_Short ?? "");
            //    row.Add("Long Desc", cde.Equipment_Long ?? "");
            //    row.Add("PV", cde.PVuser != null ? cde.PVuser.Name : "");

            //    if (cde.PVstatus == "Completed" && cde.PVuser != null && cde.PVuser.UpdatedOn != null)
            //    {
            //        DateTime date1 = DateTime.Parse(cde.PVuser.UpdatedOn.ToString());
            //        row.Add("PV Completed Date", date1.ToString("dd/MM/yyyy hh:mm:ss tt"));
            //    }
            //    else
            //    {
            //        row.Add("PV Completed Date", "");
            //    }

            //    row.Add("Catalogue", cde.Catalogue != null ? cde.Catalogue.Name : "");

            //    if (cde.ItemStatus > 3 && cde.Catalogue != null && cde.Catalogue.UpdatedOn != null)
            //    {
            //        DateTime date2 = DateTime.Parse(cde.Catalogue.UpdatedOn.ToString());
            //        row.Add("Catalogue Completed Date", date2.ToString("dd/MM/yyyy hh:mm:ss tt"));
            //    }
            //    else
            //    {
            //        row.Add("Catalogue Completed Date", "");
            //    }

            //    row.Add("QC", cde.Review != null ? cde.Review.Name : "");

            //    if (cde.ItemStatus > 5 && cde.Review != null && cde.Review.UpdatedOn != null)
            //    {
            //        DateTime date3 = DateTime.Parse(cde.Review.UpdatedOn.ToString());
            //        row.Add("QC Completed Date", date3.ToString("dd/MM/yyyy hh:mm:ss tt"));
            //    }
            //    else
            //    {
            //        row.Add("QC Completed Date", "");
            //    }

            //    row.Add("URL", cde.Soureurl ?? "");
            //    row.Add("PV Remarks", cde.Remarks ?? "");
            //    row.Add("Rework Remarks", cde.Rework_Remarks ?? "");

            //    // Determine status based on ItemStatus
            //    string status;
            //    switch (cde.ItemStatus)
            //    {
            //        case 0:
            //            status = "PV Not Assigned";
            //            break;
            //        case 1:
            //            status = "PV Pending";
            //            break;
            //        case 2:
            //            status = "Catalogue Pending";
            //            break;
            //        case 3:
            //            status = "Catalogue Saved";
            //            break;
            //        case 4:
            //            status = "QC Pending";
            //            break;
            //        case 5:
            //            status = "QC Saved";
            //            break;
            //        default:
            //            status = "Released";
            //            break;
            //    }
            //    row.Add("Status", status);

            //    lock (lockObject)
            //    {
            //        rows.Add(row);
            //    }
            //});

            return rows;
        }

        //public List<Dictionary<string, object>> getallassetdata(string FARId, string Role, string User, string Status, string fromdate, string todate)
        //{
        //    IMongoQuery query = null;
        //    var lockObject = new object();

        //    if (Role == "PV User")
        //    {
        //        Role = "PVuser";
        //        Status = "PV";
        //    }
        //    else if (Role == "Cataloguer")
        //    {
        //        Role = "Catalogue";
        //        Status = "Catalogue";
        //    }
        //    else if (Role == "Reviewer")
        //    {
        //        Role = "Review";
        //        Status = "QC";
        //    }

        //    if (!string.IsNullOrEmpty(Role) && Role != "undefined")
        //    {
        //        if (!string.IsNullOrEmpty(User))
        //        {
        //            query = Query.EQ(Role + ".Name", User);
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(Status))
        //    {
        //        query = BuildStatusQuery(query, Status);
        //    }

        //    if (!string.IsNullOrEmpty(fromdate))
        //    {
        //        query = ApplyDateFilter(query, Status, fromdate, todate);
        //    }

        //    var sort = SortBy.Ascending("UniqueId");
        //    string[] str = { "UniqueId", "FixedAssetNo", "SiteId", "AssetNo", "pvAssetNo", "BarcodeNumber", "OldTagNo", "AssetQRCode", "Description", "Parent", "Location", "LocationHierarchy", "ClassificationHierarchyDesc", "AggregatedClassSpecAttr", "Status", "AssetType", "FailureCode", "Priority", "MaintaineBy", "WarrentyExpDate", "Name", "SerialNo", "ModelNo", "ModelYear", "AssCondition", "OwnedBySite", "Manufacturer", "ReportGroup", "Vendor", "PurchasePrice", "InstallDate", "PO_Contract", "LoadCertExpDate", "CalCertExpDate", "CertExpDate", "TrafficCertExpDate", "Ownedby", "Maintainer", "PresentLocation", "Operatedby", "AdditionalNotes", "GIS", "AssetCondition", "AssetImages", "Equipment_Short", "Equipment_Long", "PVuser", "PVstatus", "Catalogue", "Review", "Soureurl", "Remarks", "Rework_Remarks", "ItemStatus" };
        //    var fields = Fields.Include(str);
        //    var Assetlist = _assetRepository.FindAll(fields, query).ToList();

        //    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //    //Dictionary<string, object> row;
        //    var allAttr = _AssetattriRepository.FindAll().ToList();

        //    System.Threading.Tasks.Parallel.ForEach(Assetlist, cde =>
        //    {

        //        var row = new Dictionary<string, object>();

        //        var assetNo = cde.AssetNo ?? "";
        //        var pvAssetNo = (cde.pvAssetNo != null && cde.pvAssetNo != assetNo) ? cde.pvAssetNo : "";

        //        row.Add("Unique ID", cde.UniqueId);
        //        row.Add("Fixedassetnum(FAR ID)", cde.FixedAssetNo ?? "");
        //        row.Add("SiteId", cde.SiteId);
        //        row.Add("Asset Number", assetNo);
        //        row.Add("PV Asset Number", pvAssetNo);
        //        row.Add("Existing BarCode(Non-Readable)", cde.BarcodeNumber ?? "");
        //        row.Add("Old Asset Barcode(Readable)", cde.OldTagNo ?? "");
        //        row.Add("Asset QR Code", cde.AssetQRCode ?? "");
        //        row.Add("Asset Description", cde.Description ?? "");
        //        row.Add("Parent", cde.Parent ?? "");
        //        row.Add("Asset Location", cde.Location ?? "");
        //        row.Add("Location Hierarchy", cde.LocationHierarchy ?? "");
        //        row.Add("Classificationhierarchy", cde.ClassificationHierarchyDesc ?? "");
        //        row.Add("Aggregatedclassspecattributes", cde.AggregatedClassSpecAttr ?? "");
        //        row.Add("Asset Status", cde.Status ?? "");
        //        row.Add("Assettype", cde.AssetType ?? "");
        //        row.Add("FailureCode", cde.FailureCode ?? "");
        //        row.Add("Priority", cde.Priority ?? "");
        //        row.Add("Maintained By", cde.MaintaineBy ?? "");
        //        row.Add("Warrantyexpdate", cde.WarrentyExpDate ?? "");
        //        row.Add("Name", cde.Name ?? "");
        //        row.Add("Serial No", cde.SerialNo ?? "");
        //        row.Add("Model No", cde.ModelNo ?? "");
        //        row.Add("Model Year", cde.ModelYear ?? "");
        //        row.Add("Asset Condition", (cde.AssCondition != null && cde.AssCondition != "Excellent") ? cde.AssCondition : "");
        //        row.Add("OwnedBySite", cde.OwnedBySite ?? "");
        //        row.Add("Manufacturer", cde.Manufacturer ?? "");
        //        row.Add("ReportGroup", cde.ReportGroup ?? "");
        //        row.Add("Vendor", cde.Vendor ?? "");
        //        row.Add("PurchasePrice", cde.PurchasePrice ?? "");
        //        row.Add("InstallDate", cde.InstallDate ?? "");
        //        row.Add("PO Contract", cde.PO_Contract ?? "");
        //        row.Add("Loadcertexpdate", cde.LoadCertExpDate ?? "");
        //        row.Add("Calcertexpdate", cde.CalCertExpDate ?? "");
        //        row.Add("Certexpdate", cde.CertExpDate ?? "");
        //        row.Add("Trafficcertexpdate", cde.TrafficCertExpDate ?? "");
        //        row.Add("Ownedby", cde.Ownedby ?? "");
        //        row.Add("Maintaineby", cde.Maintainer ?? "");
        //        row.Add("Present Location", cde.PresentLocation ?? "");
        //        row.Add("Operatedby", cde.Operatedby ?? "");
        //        row.Add("Additional Notes", cde.AdditionalNotes ?? "");

        //        if (cde.GIS != null && !string.IsNullOrEmpty(cde.GIS.LongitudeStart))
        //        {
        //            cde.GIS.LongitudeStart = cde.GIS.LongitudeStart.Replace('W', 'E');
        //            row.Add("Location Coordinates", cde.GIS.LongitudeStart + "," + (cde.GIS.LattitudeStart ?? ""));
        //        }
        //        else
        //        {
        //            row.Add("Location Coordinates", "");
        //        }

        //        // Simplified asset condition logic
        //        string pvCond = "";
        //        if (cde.AssetCondition != null)
        //        {
        //            if (cde.AssetCondition.Rank == "7") pvCond = "Excellent";
        //            else if (cde.AssetCondition.Rank == "6") pvCond = "Very Good";
        //            else if (cde.AssetCondition.Rank == "4" || cde.AssetCondition.Rank == "5") pvCond = "Good";
        //            else if (cde.AssetCondition.Rank == "2" || cde.AssetCondition.Rank == "3") pvCond = "Poor";
        //            else if (cde.AssetCondition.Rank == "1") pvCond = "Very Poor";

        //            if (cde.AssetCondition.Leakage == "Medium") pvCond = "Good";
        //            if (cde.AssetCondition.Damage == "High" && cde.AssetCondition.Corrosion == "High") pvCond = "Very Poor";
        //            if (cde.AssetCondition.Corrosion == "No" && cde.AssetCondition.Leakage == "No" &&
        //                cde.AssetCondition.Vibration == "No" && cde.AssetCondition.Temparature == "No" &&
        //                cde.AssetCondition.Damage == "No" && cde.AssetCondition.Smell == "No" &&
        //                cde.AssetCondition.Noise == "No") pvCond = "Excellent";
        //        }

        //        row.Add("PV Asset Condition", pvCond);

        //        row.Add("Nameplate Text", cde.AssetImages != null && cde.AssetImages.NamePlateText != null && cde.AssetImages.NamePlateText.Length >= 1 ? cde.AssetImages.NamePlateText[0] : "");

        //        //var classification = GetAttributeInfo(cde.UniqueId);
        //        var classification = allAttr.Find(i => i.UniqueId == cde.UniqueId);

        //        if (classification != null)
        //        {
        //            string nm = "";
        //            if (!string.IsNullOrEmpty(classification.Noun) && !string.IsNullOrEmpty(classification.Modifier))
        //            {
        //                nm = classification.Noun + "," + classification.Modifier;
        //            }
        //            else if (!string.IsNullOrEmpty(classification.Noun) && (string.IsNullOrEmpty(classification.Modifier) || classification.Modifier == "--"))
        //            {
        //                nm = classification.Noun;
        //            }
        //            row.Add("Classificationid", nm);
        //        }
        //        else
        //        {
        //            row.Add("Classificationid", "");
        //        }

        //        row.Add("Short Desc", cde.Equipment_Short ?? "");
        //        row.Add("Long Desc", cde.Equipment_Long ?? "");
        //        row.Add("PV", cde.PVuser != null ? cde.PVuser.Name : "");

        //        if (cde.PVstatus == "Completed" && cde.PVuser != null && cde.PVuser.UpdatedOn != null)
        //        {
        //            DateTime date1 = DateTime.Parse(cde.PVuser.UpdatedOn.ToString());
        //            row.Add("PV Completed Date", date1.ToString("dd/MM/yyyy hh:mm:ss tt"));
        //        }
        //        else
        //        {
        //            row.Add("PV Completed Date", "");
        //        }

        //        row.Add("Catalogue", cde.Catalogue != null ? cde.Catalogue.Name : "");

        //        if (cde.ItemStatus > 3 && cde.Catalogue != null && cde.Catalogue.UpdatedOn != null)
        //        {
        //            DateTime date2 = DateTime.Parse(cde.Catalogue.UpdatedOn.ToString());
        //            row.Add("Catalogue Completed Date", date2.ToString("dd/MM/yyyy hh:mm:ss tt"));
        //        }
        //        else
        //        {
        //            row.Add("Catalogue Completed Date", "");
        //        }

        //        row.Add("QC", cde.Review != null ? cde.Review.Name : "");

        //        if (cde.ItemStatus > 5 && cde.Review != null && cde.Review.UpdatedOn != null)
        //        {
        //            DateTime date3 = DateTime.Parse(cde.Review.UpdatedOn.ToString());
        //            row.Add("QC Completed Date", date3.ToString("dd/MM/yyyy hh:mm:ss tt"));
        //        }
        //        else
        //        {
        //            row.Add("QC Completed Date", "");
        //        }

        //        row.Add("URL", cde.Soureurl ?? "");
        //        row.Add("PV Remarks", cde.Remarks ?? "");
        //        row.Add("Rework Remarks", cde.Rework_Remarks ?? "");

        //        // Determine status based on ItemStatus
        //        string status;
        //        switch (cde.ItemStatus)
        //        {
        //            case 0:
        //                status = "PV Not Assigned";
        //                break;
        //            case 1:
        //                status = "PV Pending";
        //                break;
        //            case 2:
        //                status = "Catalogue Pending";
        //                break;
        //            case 3:
        //                status = "Catalogue Saved";
        //                break;
        //            case 4:
        //                status = "QC Pending";
        //                break;
        //            case 5:
        //                status = "QC Saved";
        //                break;
        //            default:
        //                status = "Released";
        //                break;
        //        }
        //        row.Add("Status", status);

        //        lock (lockObject)
        //        {
        //            rows.Add(row);
        //        }
        //    });


        //    return rows;
        //}

        private IMongoQuery BuildStatusQuery(IMongoQuery query, string Status)
        {
            query = query ?? Query.GTE("ItemStatus", 0);

            if (Status == "PV")
            {
                query = Query.And(query, Query.EQ("ItemStatus", 1));
            }
            else if (Status == "PVRework")
            {
                query = Query.And(query, Query.EQ("ItemStatus", 1), Query.EQ("Rework", "PV"));
            }
            else if (Status == "Catalogue")
            {
                query = Query.And(query, Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)));
            }
            else if (Status == "CatalogueRework")
            {
                query = Query.And(query, Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)), Query.EQ("Rework", "Cat"));
            }
            else if (Status == "QC")
            {
                query = Query.And(query, Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 5)));
            }
            else if (Status == "Released")
            {
                query = Query.And(query, Query.EQ("ItemStatus", 6));
            }
            else if (Status == "All")
            {
                query = Query.And(query, Query.GTE("ItemStatus", 0));
            }

            return query;
        }

        private IMongoQuery ApplyDateFilter(IMongoQuery query, string Status, string fromdate, string todate)
        {
            var cultureInfo = new CultureInfo("en-US", true);
            var fromDateParsed = DateTime.Parse(fromdate, cultureInfo);
            fromDateParsed = DateTime.SpecifyKind(fromDateParsed, DateTimeKind.Utc);

            DateTime toDateParsed;
            if (!string.IsNullOrEmpty(todate))
            {
                toDateParsed = DateTime.Parse(todate, cultureInfo).AddDays(1);
            }
            else
            {
                toDateParsed = DateTime.UtcNow;
            }

            toDateParsed = DateTime.SpecifyKind(toDateParsed, DateTimeKind.Utc);

            string field = GetUpdatedOnField(Status);

            if (!string.IsNullOrEmpty(field))
            {
                query = Query.And(query, Query.GTE(field, BsonDateTime.Create(fromDateParsed)),
                                         Query.LTE(field, BsonDateTime.Create(toDateParsed)));
            }
            else
            {
                query = Query.And(query,
                    Query.Or(
                        Query.And(Query.GTE("PVuser.UpdatedOn", BsonDateTime.Create(fromDateParsed)), Query.LTE("PVuser.UpdatedOn", BsonDateTime.Create(toDateParsed))),
                        Query.And(Query.GTE("Catalogue.UpdatedOn", BsonDateTime.Create(fromDateParsed)), Query.LTE("Catalogue.UpdatedOn", BsonDateTime.Create(toDateParsed))),
                        Query.And(Query.GTE("Review.UpdatedOn", BsonDateTime.Create(fromDateParsed)), Query.LTE("Review.UpdatedOn", BsonDateTime.Create(toDateParsed))),
                        Query.And(Query.GTE("Release.UpdatedOn", BsonDateTime.Create(fromDateParsed)), Query.LTE("Release.UpdatedOn", BsonDateTime.Create(toDateParsed)))
                    )
                );
            }

            return query;
        }

        private string GetUpdatedOnField(string Status)
        {
            switch (Status)
            {
                case "PV": return "PVuser.UpdatedOn";
                case "Catalogue": return "Catalogue.UpdatedOn";
                case "QC": return "Review.UpdatedOn";
                case "Released": return "Release.UpdatedOn";
                default: return string.Empty;
            }
        }


        //public IEnumerable<Prosol_AssetAttributes> getallattrdata(List<string> lst)
        //{
        //    var atrrLst = new List<Prosol_AssetAttributes>();
        //    foreach (var code in lst)
        //    {
        //        var qry = Query.EQ("UniqueId", code);
        //        var attr = _AssetattriRepository.FindOne(qry);
        //        atrrLst.Add(attr);
        //    }
        //    return atrrLst;
        //}

        public List<Dictionary<string, object>> getallattrdata(string FARId, string Role, string User, string Status, string fromdate, string todate)
        {

            var lockObject = new object();
            List<string> lst = getallassetcodes(FARId, Role, User, Status, fromdate, todate);

            var attrList = new List<Prosol_AssetAttributes>();

            //if (lst == null || lst.Count == 0)
            //{
            //    return attrList;
            //}

            //foreach (var code in lst)
            //{
            //    var qry = Query.EQ("UniqueId", code);

            //    var attr = _AssetattriRepository.FindOne(qry);

            //    if (attr != null)
            //    {
            //        attrList.Add(attr);
            //    }
            //}
            var Qry = Query.In("UniqueId", new BsonArray(lst));
            attrList = _AssetattriRepository.FindAll(Qry).ToList();

            List<Dictionary<string, object>> attRows = new List<Dictionary<string, object>>();
            //Dictionary<string, object> row;

            if (attrList != null)
            {
                System.Threading.Tasks.Parallel.ForEach(attrList, mrg =>
                {
                    if (mrg != null)
                    {
                        var row = new Dictionary<string, object>();
                        row.Add("Unique Id", mrg.UniqueId);
                        row.Add("Noun", mrg.Noun ?? "");
                        row.Add("Modifier", mrg.Modifier ?? "");

                        var characteristicsCount = mrg.Characterisitics != null ? mrg.Characterisitics.Count : 0;

                        for (int i = 1; i <= 30; i++)
                        {
                            if (i <= characteristicsCount)
                            {
                                var attr = mrg.Characterisitics[i - 1];
                                row.Add("ATTRIBUTE NAME " + i, attr.Characteristic ?? "");
                                row.Add("ATTRIBUTE VALUE " + i, attr.Value ?? "");
                            }
                            else
                            {
                                row.Add("ATTRIBUTE NAME " + i, "");
                                row.Add("ATTRIBUTE VALUE " + i, "");
                            }
                        }

                        lock (lockObject)
                        {
                            attRows.Add(row);
                        }
                    }
                });

            }

            return attRows;
        }


        //public IEnumerable<Prosol_AssetMaster> getallassetdata(string FARId, string Role, string User, string Status, string fromdate, string todate)
        //{
        //    if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
        //    {
        //        var date = DateTime.Parse(fromdate, new CultureInfo("en-US", true));
        //        date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        //        var date1 = DateTime.Parse(todate, new CultureInfo("en-US", true));
        //        date1 = date1.AddDays(1);
        //        date1 = DateTime.SpecifyKind(date1, DateTimeKind.Utc);

        //        var SAQry = Query.EQ("FARId", FARId);
        //        //var SAQry = Query.And(Query.EQ("MajorClass", Major),Query.EQ("SubArea", SubArea));
        //        //if (!string.IsNullOrEmpty(FARId))
        //        //{
        //        //    SAQry = Query.And(Query.EQ("MajorClass", Major), Query.EQ("MinorClass", Minor), Query.EQ("SubArea", SubArea));
        //        //}
        //        var QryLst = new List<IMongoQuery>();

        //        QryLst.Add(SAQry);

        //        if (Role == "PV User" && Status == "Pending")
        //        {

        //            var Qry = Query.And(Query.GTE("ItemStatus", 1), Query.EQ("PVuser.Name", User), Query.GTE("PVuser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("PVuser.UpdatedOn", BsonDateTime.Create(date1)));
        //            if (User == "")
        //                Qry = Query.And(Query.EQ("ItemStatus", 1),  Query.GTE("PVuser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("PVuser.UpdatedOn", BsonDateTime.Create(date1)));
        //            QryLst.Add(Qry);
        //        }
        //        else if (Role == "PV User" && Status == "Completed")
        //        {
        //            var Qry = Query.And(Query.GTE("ItemStatus", 2), Query.EQ("PVuser.Name", User), Query.GTE("PVuser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("PVuser.UpdatedOn", BsonDateTime.Create(date1)));
        //            if (User == "")
        //                Qry = Query.And(Query.GTE("ItemStatus", 2), Query.GTE("PVuser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("PVuser.UpdatedOn", BsonDateTime.Create(date1)));
        //            QryLst.Add(Qry);
        //        }
        //        else if (Role == "PV User" && User == "")
        //        {
        //            var Qry = Query.And(Query.GTE("ItemStatus", 1), /*Query.EQ("Business", "62bec579e7a1c617d830ebb2"),*/ Query.GTE("PVuser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("PVuser.UpdatedOn", BsonDateTime.Create(date1)));
        //            QryLst.Add(Qry);

        //        }
        //        else if (Role == "PV User" && Status == "")
        //        {
        //            // var Qry = Query.And(Query.GTE("ItemStatus", 0), Query.EQ("Business", "62bec30af552cf37e82d64ba"), Query.Or(Query.EQ("MajorClass", "62bec741e7a1c617d830ebba"), Query.EQ("MajorClass", "62bec779e7a1c617d830ebbc")), Query.NE("PVuser", BsonNull.Value), Query.EQ("PVuser.Name", "Kalai"), Query.GTE("PVuser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("PVuser.UpdatedOn", BsonDateTime.Create(date1)));
        //           // var Qry = Query.And(Query.GTE("ItemStatus", 0), Query.EQ("Business", "62bec579e7a1c617d830ebb2"), Query.GTE("PVuser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("PVuser.UpdatedOn", BsonDateTime.Create(date1)));
        //            var Qry = Query.And(Query.GTE("ItemStatus", 1), Query.EQ("PVuser.Name", User), Query.GTE("PVuser.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("PVuser.UpdatedOn", BsonDateTime.Create(date1)));
        //            QryLst.Add(Qry);

        //        }

        //        //cataloguer

        //        if (Role == "Cataloguer" && Status == "Pending")
        //        {
        //            var Qry = Query.And(Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)), Query.EQ("Catalogue.Name", User), Query.GTE("Catalogue.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Catalogue.UpdatedOn", BsonDateTime.Create(date1)));
        //            if (User == "")
        //                Qry = Query.And(Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)),  Query.GTE("Catalogue.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Catalogue.UpdatedOn", BsonDateTime.Create(date1)));
        //            QryLst.Add(Qry);
        //        }
        //        else if (Role == "Cataloguer" && Status == "Completed")
        //        {
        //            var Qry = Query.And(Query.GTE("ItemStatus", 4), Query.EQ("Catalogue.Name", User), Query.GTE("Catalogue.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Catalogue.UpdatedOn", BsonDateTime.Create(date1)));
        //            if (User == "")
        //                Qry = Query.And(Query.GTE("ItemStatus", 4), Query.GTE("Catalogue.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Catalogue.UpdatedOn", BsonDateTime.Create(date1)));
        //            QryLst.Add(Qry);
        //        }
        //        else if (Role == "Cataloguer" && User == "")
        //        {
        //            var Qry = Query.And(Query.GTE("ItemStatus", 2), Query.GTE("Catalogue.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Catalogue.UpdatedOn", BsonDateTime.Create(date1)));
        //            QryLst.Add(Qry);

        //        }
        //        else if (Role == "Cataloguer" && Status == "")
        //        {
        //            var Qry = Query.And(Query.GTE("ItemStatus", 2), Query.EQ("Catalogue.Name", User), Query.GTE("Catalogue.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Catalogue.UpdatedOn", BsonDateTime.Create(date1)));
        //            QryLst.Add(Qry);

        //        }

        //        //Reviewer
        //        if (Role == "Reviewer" && Status == "Pending")
        //        {
        //            var Qry = Query.And(Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 4)), Query.EQ("Review.Name", User), Query.GTE("Review.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Review.UpdatedOn", BsonDateTime.Create(date1)));
        //            QryLst.Add(Qry);
        //        }
        //        else if (Role == "Reviewer" && Status == "Completed")
        //        {
        //            var Qry = Query.And(Query.GTE("ItemStatus", 6), Query.EQ("Review.Name", User), Query.GTE("Review.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Review.UpdatedOn", BsonDateTime.Create(date1)));
        //            QryLst.Add(Qry);
        //        }
        //        else if (Role == "Reviewer" && User == "")
        //        {
        //            var Qry = Query.And(Query.GTE("ItemStatus", 4), Query.GTE("Review.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Review.UpdatedOn", BsonDateTime.Create(date1)));
        //            QryLst.Add(Qry);

        //        }
        //        else if (Role == "Reviewer" && Status == "")
        //        {
        //            var Qry = Query.And(Query.GTE("ItemStatus", 4), Query.EQ("Review.Name", User), Query.GTE("Review.UpdatedOn", BsonDateTime.Create(date)), Query.LTE("Review.UpdatedOn", BsonDateTime.Create(date1)));
        //            QryLst.Add(Qry);

        //        }

        //        var ExcuQry = Query.And(QryLst);
        //        var srt = SortBy.Ascending("AssetNo");
        //        var res = _assetRepository.FindAll(ExcuQry, srt).ToList();
        //        return res;
        //    }
        //    else
        //    {
        //        var SAQry = Query.EQ("FARId", FARId);
        //        //if (!string.IsNullOrEmpty(Minor))
        //        //{
        //        //    SAQry = Query.And(Query.EQ("MajorClass", Major), Query.EQ("MinorClass", Minor), Query.EQ("SubArea", SubArea));
        //        //}
        //        var QryLst = new List<IMongoQuery>();
        //         QryLst.Add(SAQry);
        //        if (Role == "PV User" && Status == "Pending")
        //        {
        //            var Qry = Query.And(Query.EQ("ItemStatus", 1), Query.EQ("PVuser.Name", User));
        //            if (User == "")
        //                Qry = Query.EQ("ItemStatus", 1);
        //            QryLst.Add(Qry);
        //        }
        //        else if (Role == "PV User" && Status == "Completed")
        //        {
        //            var Qry = Query.And(Query.GTE("ItemStatus", 2), Query.EQ("PVuser.Name", User));
        //            if (User == "")
        //                Qry = Query.And(Query.EQ("ItemStatus", 2),Query.EQ("Business", "62bec579e7a1c617d830ebb2"));
        //            QryLst.Add(Qry);
        //        }
        //        else if (Role == "PV User" && User == "")
        //        {
        //            var Qry = Query.GTE("ItemStatus", 1);
        //            QryLst.Add(Qry);

        //        }
        //        else if (Role == "PV User" && Status == "")
        //        {
        //            var Qry = Query.And(Query.GTE("ItemStatus", 1), Query.EQ("PVuser.Name", User));
        //            QryLst.Add(Qry);

        //        }

        //        //cataloguer

        //        if (Role == "Cataloguer" && Status == "Pending")
        //        {
        //            var Qry = Query.And(Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)), Query.EQ("Catalogue.Name", User));
        //            if (User == "")
        //                Qry = Query.And(Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)));
        //            QryLst.Add(Qry);
        //        }
        //        else if (Role == "Cataloguer" && Status == "Completed")
        //        {
        //            var Qry = Query.And(Query.GTE("ItemStatus", 4), Query.EQ("Catalogue.Name", User));
        //            if (User == "")
        //                Qry = Query.GTE("ItemStatus", 4);
        //            QryLst.Add(Qry);
        //        }
        //        else if (Role == "Cataloguer" && User == "")
        //        {
        //            //water distribution & Transmission
        //          //  var Qry = Query.And(Query.GTE("ItemStatus", 0), Query.EQ("Business", "62bec30af552cf37e82d64ba"), Query.Or(Query.EQ("MajorClass", "62bec741e7a1c617d830ebba"), Query.EQ("MajorClass", "62bec779e7a1c617d830ebbc"), Query.NE("PVuser", BsonNull.Value)));
        //           //Electricity Distributeion
        //            //var Qry = Query.And(Query.GTE("ItemStatus", 0), Query.EQ("Business", "62bec579e7a1c617d830ebb2"),Query.EQ("MajorClass", "62bec7ece7a1c617d830ebc2"),Query.NE("PVuser", BsonNull.Value));
        //          //  var Qry = Query.And(Query.GTE("ItemStatus", 0), Query.EQ("Business", "62bec579e7a1c617d830ebb2"), Query.NE("PVuser", BsonNull.Value));
        //            //water production
        //            // var Qry = Query.And(Query.GTE("ItemStatus",4), Query.EQ("Business", "62bec30af552cf37e82d64ba"), Query.EQ("MajorClass", "62bec671e7a1c617d830ebb6"));

        //           var Qry = Query.GTE("ItemStatus",2);
        //            QryLst.Add(Qry);

        //        }
        //        else if (Role == "Cataloguer" && Status == "")
        //        {
        //            var Qry = Query.And(Query.GTE("ItemStatus", 2), Query.EQ("Catalogue.Name", User));
        //            QryLst.Add(Qry);

        //        }

        //        //Reviewer
        //        if (Role == "Reviewer" && Status == "Pending")
        //        {
        //            var Qry = Query.And(Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 5)), Query.EQ("Review.Name", User));
        //            QryLst.Add(Qry);
        //        }
        //        else if (Role == "Reviewer" && Status == "Completed")
        //        {
        //            var Qry = Query.And(Query.GTE("ItemStatus", 6), Query.EQ("Review.Name", User));
        //            QryLst.Add(Qry);
        //        }
        //        else if (Role == "Reviewer" && User == "")
        //        {
        //            var Qry = Query.GTE("ItemStatus", 4);
        //            QryLst.Add(Qry);

        //        }
        //        else if (Role == "Reviewer" && Status == "")
        //        {
        //            var Qry = Query.And(Query.GTE("ItemStatus", 4), Query.EQ("Review.Name", User));
        //            QryLst.Add(Qry);

        //        }

        //        var ExcuQry = Query.And(QryLst);
        //        var srt = SortBy.Ascending("AssetNo");
        //        var res = _assetRepository.FindAll(ExcuQry, srt).ToList();
        //        return res;

        //    }
        //}
        // Get All Data
        public IEnumerable<Prosol_AssetMaster> getall(string PV, string Region, string fromdate, string todate)
        {
            var date = DateTime.Parse(fromdate, new CultureInfo("en-US", true));
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            var date1 = DateTime.Parse(todate, new CultureInfo("en-US", true));
            date1 = date1.AddDays(1);
            date1 = DateTime.SpecifyKind(date1, DateTimeKind.Utc);
            var query = Query.And(Query.EQ("ItemStatus", 4), Query.EQ("Region", Region), Query.EQ("PVuser.Name", PV), Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));

            var get = _assetRepository.FindAll(query).ToList();
            return get;

        }

        //Load Asset data
        public List<Dictionary<string, object>> loadAssetdata1(string PV, string Region, string Fromdate, string Todate)
        {

            var datalist = getall(PV, Region, Fromdate, Todate);

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();

            bool goo;
            var mergelist = (dynamic)null;

            foreach (var cde in datalist)
            {
                row = new Dictionary<string, object>();
                //row = new Dictionary<string, object>();

                //row.Add("Unique_Id", cde.UniqueId);
                //row.Add("Barcode", cde.Barcode);
                //row.Add("NewTagNo", cde.NewTagNo);
                //row.Add("SiteId", cde.SiteId);
                //row.Add("SiteName", cde.SiteName);
                //row.Add("SiteType", cde.SiteType);
                //row.Add("CompanyCode", cde.CompanyCode);
                //row.Add("SAP_Equipment", cde.SAP_Equipment);
                //row.Add("OldTagNo", cde.OldTagNo);
                //row.Add("AssetNo", cde.AssetNo);
                //row.Add("AssetSubNo", cde.AssetSubNo);
                //row.Add("ObjectId", cde.ObjectId);
                //row.Add("ObjectType", cde.ObjectType);
                //row.Add("FuncLocDesc", cde.FuncLocDesc);
                //row.Add("Location", cde.Location);
                //row.Add("Business", cde.Business);
                //row.Add("MajorClass", cde.MajorClass);
                //row.Add("MinorClass", cde.MinorClass);

                //row.Add("EquipmentClass", cde.EquipmentClass);
                //row.Add("EquipmentType", cde.EquipmentType);


                //row.Add("Region", cde.Region);
                //row.Add("Area", cde.Area);
                //row.Add("SubArea", cde.SubArea);
                //row.Add("Function", cde.Function);
                //row.Add("Identifier", cde.Identifier);
                //row.Add("OldFunLoc", cde.OldFunLoc);
                //row.Add("BOM", cde.BOM);
                //row.Add("FLOC_Code", cde.FLOC_Code);
                //row.Add("EquipmentDesc", cde.EquipmentDesc);
                //row.Add("Equipment_Short", cde.Equipment_Short);
                //row.Add("Equipment_Long", cde.Equipment_Long);
                //row.Add("Quantity", cde.Quantity);
                //row.Add("UOM", cde.UOM);
                //row.Add("YearOfInstall", cde.YearOfInstall);
                //row.Add("YearOfMfr", cde.YearOfMfr);
                //row.Add("Idle_Operational", cde.Idle_Operational);
                //row.Add("LattitudeStart", cde.LattitudeStart);
                //row.Add("LongitudeEnd", cde.LongitudeEnd);
                //row.Add("LattitudeEnd", cde.LattitudeEnd);
                //row.Add("LongitudeStart", cde.LongitudeStart);
                //row.Add("VerificateionDate", cde.VerificateionDate);
                //row.Add("VerifiedBy", cde.VerifiedBy);
                //row.Add("BuildingId", cde.BuildingId);
                //row.Add("BuildingName", cde.BuildingName);
                //row.Add("Location", cde.Location);
                //row.Add("Length", cde.Length);
                //row.Add("Width", cde.Width);
                //row.Add("Height", cde.Height);
                //row.Add("BuildingImage", cde.BuildingImage);

                row.Add("Catalogue", cde.Catalogue.Name);
                row.Add("PVuser", cde.PVuser.Name);
                row.Add("ItemStatus", cde.ItemStatus);
                //row.Add("Plant", cde.Plant);
                //row.Add("Remarks", cde.Remarks);
                //row.Add("NamePlateImge", cde.NamePlateImge);
                //row.Add("AssetImage", cde.AssetImage);
                //row.Add("NewTagImage", cde.NewTagImage);
                //row.Add("OldTagImage", cde.OldTagImage);
                //row.Add("NamePlateText", cde.NamePlateText);
                //row.Add("Corrosion", cde.Corrosion);
                //row.Add("Damage", cde.Damage);
                //row.Add("Leakage", cde.Leakage);
                //row.Add("Vibration", cde.Vibration);
                //row.Add("Temparature", cde.Temparature);
                //row.Add("Smell", cde.Smell);
                //row.Add("Noise", cde.Noise);
                //row.Add("Rank", cde.Rank);
                //row.Add("AssetCondition", cde.AssetCondition);
                //row.Add("CorrosionImage", cde.CorrosionImage);
                //row.Add("DamageImage", cde.DamageImage);
                //row.Add("LeakageImage", cde.LeakageImage);
                //row.Add("CreatedOn", cde.CreatedOn.ToString());
                //row.Add("UpdatedOn", cde.UpdatedOn.ToString());
                rows.Add(row);
            }

            return rows;
        }

        public IEnumerable<Prosol_AssetMaster> trackAssetcodelist(string User, string Region)
        {
            //  string[] search_field = { "Itemcode", "Legacy", "Noun", "Modifier", "Legacy2" };
            // var fields = Fields.Include(search_field).Exclude("_id");
            var query = Query.And(Query.EQ("PVuser", User), (Query.EQ("Region", Region)));
            var getdata = _assetRepository.FindAll(query).ToList();
            return getdata;
        }
        public IEnumerable<Prosol_AssetMaster> getallregion(string Region, string fromdate, string todate)
        {
            var date = DateTime.Parse(fromdate, new CultureInfo("en-US", true));
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            var date1 = DateTime.Parse(todate, new CultureInfo("en-US", true));
            date1 = date1.AddDays(1);
            date1 = DateTime.SpecifyKind(date1, DateTimeKind.Utc);
            var query = Query.And(Query.EQ("ItemStatus", 2), Query.EQ("Region", Region), Query.GTE("UpdatedOn", BsonDateTime.Create(date)), Query.LTE("UpdatedOn", BsonDateTime.Create(date1)));

            var get = _assetRepository.FindAll(query).ToList();
            return get;

        }
        //PVUSER
        public IEnumerable<Prosol_Users> getuserpv(string role)
        {
            string[] userflds = { "UserName", "Userid" };
            var fields = Fields.Include(userflds).Exclude("_id");
            var query = Query.And(Query.EQ("Roles.Name", role), Query.EQ("Islive", "Active"));
            var mxplnt = _UsercreateRepository.FindAll(fields, query).ToList();
            return mxplnt;
        }
        public IEnumerable<Prosol_AssetMaster> reloaddata1(string Region, string City, string Area, string SubArea)
        {
            string[] search_field = { "UniqueId", "UserName", "Region", "Area", "SubArea", "EquipmentClass", "EquipmentType", "SAP_Equipment", "OldTagNo", "Equipment", "City", "Business", "MajorClass", "EquipmentDesc" };
            var fields = Fields.Include(search_field).Exclude("_id");

            var query = Query.And(Query.EQ("ItemStatus", 0), Query.EQ("Region", Region), Query.EQ("Area", Area), Query.EQ("SubArea", SubArea), Query.EQ("City", City));


            var getdata = _assetRepository.FindAll(fields, query).ToList();


            return getdata;

        }
        public IEnumerable<Prosol_AssetMaster> reloaddataa(string farId, string user)
        {

            //var query = Query.And(Query.EQ("ItemStatus", 0), Query.EQ("FixedAssetNo", farId));
            var query = Query.Or(Query.And(Query.EQ("PVuser", BsonNull.Value), Query.EQ("ItemStatus", 0), Query.EQ("User.UserId", user)), Query.EQ("ItemStatus", 6));


            var getdata = _assetRepository.FindAll(query).ToList();


            return getdata;

        }
        public bool reassign_submit1(string item, string role, Prosol_AssetMaster reasgn)
        {
            bool res = false;
            var query = Query.EQ("UniqueId", item);
            var vn = _assetRepository.FindOne(query);


            if (vn != null)
            {
                vn.PVuser = reasgn.PVuser;
                vn.ItemStatus = 1;
                vn.Catalogue = null;
                vn.Review = null;

                //vn[0].Plant = reasgn.Plant;
                //vn[0].UpdatedOn = reasgn.UpdatedOn;
                //vn[0].Catalogue = reasgn.Catalogue;
                res = _assetRepository.Add(vn);
            }

            return res;
        }
        public Prosol_Users GetCatId(string name)
        {
            var query = Query.EQ("UserName", name);
            var getId = _UsercreateRepository.FindOne(query);
            return getId;
        }
        //cat assign
        public IEnumerable<Prosol_AssetMaster> catloaddata1()
        {
            string[] search_field = { "UniqueId", "PVuser", "Business", "MajorClass", "Region", "Area", "SubArea", "EquipmentDesc", "EquipmentClass", "EquipmentType", "SAP_Equipment", "OldTagNo", "Equipment" };

            var fields = Fields.Include(search_field).Exclude("_id");

            var query = Query.And(Query.EQ("ItemStatus", 2), Query.EQ("Catalogue", BsonNull.Value));

            var getdata = _assetRepository.FindAll(fields, query).ToList();


            return getdata;
        }
        public IEnumerable<Prosol_AssetMaster> catreloaddata1(string Business, string Major, string Region, string City, string Area, string SubArea)
        {
            string[] search_field = { "UniqueId", "PVuser", "Business", "MajorClass", "Region", "Area", "SubArea", "EquipmentDesc", "EquipmentClass", "EquipmentType", "SAP_Equipment", "OldTagNo", "Equipment", "Catalogue" };

            var fields = Fields.Include(search_field).Exclude("_id");

            var query = Query.And(Query.EQ("ItemStatus", 2), Query.EQ("Catalogue", BsonNull.Value), Query.EQ("Business", Business), Query.EQ("MajorClass", Major), Query.EQ("Region", Region), Query.EQ("City", City), Query.EQ("Area", Area), Query.EQ("SubArea", SubArea));

            var getdata = _assetRepository.FindAll(query).ToList();


            return getdata;

        }
        public IEnumerable<Prosol_AssetMaster> catreloaddataa(string farId)
        {


            //var query = Query.And(Query.EQ("ItemStatus", 2), Query.EQ("Catalogue", BsonNull.Value), Query.EQ("FixedAssetNo", farId));
            var query = Query.And(Query.EQ("ItemStatus", 2), Query.EQ("Catalogue", BsonNull.Value));

            var getdata = _assetRepository.FindAll(query).ToList();

            return getdata;

        }
        public IEnumerable<Prosol_Users> getcatuser()
        {
            string[] userflds = { "UserName", "Userid" };
            var fields = Fields.Include(userflds).Exclude("_id");
            var query = Query.And(Query.EQ("Roles.Name", "Cataloguer"));
            var mxplnt = _UsercreateRepository.FindAll(fields, query).ToList();
            return mxplnt;
        }
        public IEnumerable<Prosol_Users> getuser()
        {
            string[] userflds = { "UserName", "Userid" };
            var fields = Fields.Include(userflds).Exclude("_id");
            var query = Query.And(Query.EQ("Roles.Name", "PV User"));
            var mxplnt = _UsercreateRepository.FindAll(fields, query).ToList();
            return mxplnt;
        }

        public bool catreassign_submit(string item, string role, Prosol_AssetMaster reasgn)
        {
            var query = Query.EQ("UniqueId", item);
            var vn = _assetRepository.FindAll(query).ToList();


            if (vn.Count > 0)
            {
                vn[0].Catalogue = reasgn.Catalogue;
                if (vn[0].ItemStatus == 0)
                    vn[0].ItemStatus = 2;

                var res = _assetRepository.Add(vn[0]);
            }

            return true;
        }
        //bulk user assign
        public virtual string AssetCatBulk_Upload(HttpPostedFileBase file)
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
            DataTable dt = res.Tables[0];

            string[] columns = { "UniqueId", "UserName" };


            if (dt.Rows.Count > 0)
            {

                foreach (DataRow drw in dt.Rows)
                {
                    var objlist = new Prosol_AssetMaster();
                    if (!string.IsNullOrEmpty(drw["UniqueId"].ToString()))
                    {
                        // var qry1 = Query.And(Query.EQ("UniqueId", drw["UniqueId"].ToString()), Query.EQ("ItemStatus", 2));
                        var qry1 = Query.Or(Query.EQ("AssetNo", drw["UniqueId"].ToString()), Query.EQ("UniqueId", drw["UniqueId"].ToString()));
                        objlist = _assetRepository.FindOne(qry1);
                        if (objlist != null)
                        {
                            if (objlist.ItemStatus == 0 || objlist.ItemStatus == 1)
                            {
                                var checkusername = Query.And(Query.EQ("UserName", drw["UserName"].ToString()), Query.EQ("Roles.Name", "PV User"));
                                var username = _UsercreateRepository.FindOne(checkusername);

                                if (username != null)
                                {

                                    var catassign = new Prosol_UpdatedBy();
                                    catassign.Name = username.UserName;
                                    catassign.UserId = username.Userid;
                                    catassign.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                                    objlist.PVuser = catassign;
                                    objlist.ItemStatus = 1;
                                    _assetRepository.Add(objlist);
                                    cunt++;
                                }
                                else
                                    return cunt + " Items uploaded, Row:" + lineId + ", " + drw["UserName"].ToString() + " not exist,provide correct Cataloguer's UserName";
                            }
                            else
                            {
                                return cunt + " Items uploaded, Row:" + (cunt + 1) + ", " + drw["UniqueId"].ToString() + " not exist in the PV";
                            }
                        }
                        else
                            return cunt + " Items uploaded, Row:" + lineId + ", " + drw["UniqueId"].ToString() + " not exist in the DB";
                    }
                    lineId++;
                }
            }


            return cunt + " Items assigned successfully";


        }
        public int UpdateAssetBom(List<Prosol_AssetBOM> ListBOM)
        {
            int cunt = 0;
            foreach (var mdl in ListBOM)
            {
                if (mdl.UniqueId != null)
                {
                    var Qry = Query.EQ("UniqueId", mdl.UniqueId);
                    var obj = _AssetBOMRepository.FindOne(Qry);
                    if (obj != null)
                    {
                        obj.BOMName = mdl.BOMName;
                        obj.Description = mdl.Description;
                        obj.Qty = mdl.Qty;
                        obj.UOM = mdl.UOM;
                        _AssetBOMRepository.Add(obj);
                        cunt++;
                    }

                }

            }

            return cunt;
        }
        public List<Prosol_AssetBOM> getAllAssetBom(string EquipmentId)
        {
            var Qry = Query.EQ("EquipmentId", EquipmentId);

            var Lst = _AssetBOMRepository.FindAll(Qry).ToList();

            return Lst;
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
            if (dt.Columns[0].ColumnName == "Unique No" && dt.Columns[1].ColumnName == "Barcode Unique No" && dt.Columns[2].ColumnName == "Site ID" && dt.Columns[3].ColumnName == "Site Name")
            {
                return dt;
            }
            else
            {

                return null;
            }


        }
        public List<Prosol_FARRepository> GetFarMaster()
        {
            var query = Query.EQ("Islive", true);
            var rs = _FARMasterrep.FindAll(query).ToList();
            return rs;
        }
        public List<Prosol_SiteMaster> GetSiteMaster()
        {
            var query = Query.EQ("Islive", true);
            var rs = _SiteMaster.FindAll(query).ToList();
            return rs;
        }
        public List<Prosol_AssetTypeMaster> GetAssetTypeMaster()
        {
            var query = Query.EQ("Islive", true);
            var rs = _AssetTypeMaster.FindAll(query).ToList();
            return rs;
        }
        public List<Prosol_Location> GetLocationMaster()
        {
            var query = Query.EQ("Islive", true);
            var rs = _Locrep.FindAll(query).ToList();
            return rs;
        }
        public List<Prosol_FARMaster> GetTarMaster()
        {
            var query = Query.EQ("Islive", true);
            var rs = _FARMaster.FindAll(query).ToList();
            return rs;
        }
        public Prosol_Business getbusinessname(string bus)
        {
            var query = Query.EQ("BusinessName", bus);
            var rs = _BusiMasterrep.FindOne(query);
            return rs;
        }
        public Prosol_MajorClass Majorid(string maj, string busi_id)
        {
            var query = Query.And(Query.EQ("Business_id", busi_id), Query.EQ("MajorClass", maj));
            var rs = _Majorep.FindOne(query);
            return rs;
        }
        public Prosol_MinorClass getMinorid(string min, string Major_id)
        {
            var query = Query.And(Query.EQ("Major_id", Major_id), Query.EQ("MinorClass", min));
            var rs = _Minorep.FindOne(query);
            return rs;
        }
        public Prosol_MinorClass getMinorid(string min)
        {
            var query = Query.EQ("MinorClass", min);
            var rs = _Minorep.FindOne(query);
            return rs;
        }
        public Prosol_Region getregionid(string reg)
        {
            var query = Query.EQ("Region", reg);
            var rs = _Regionrep.FindOne(query);
            return rs;
        }
        public Prosol_City getCityid(string city, string Region_id)
        {
            var query = Query.And(Query.EQ("Region_Id", Region_id), Query.EQ("City", city));
            var rs = _Cityrep.FindOne(query);
            return rs;
        }
        public Prosol_Area getAreaid(string area, string City_id)
        {
            var query = Query.And(Query.EQ("City_Id", City_id), Query.EQ("Area", area));
            var rs = _Arearep.FindOne(query);
            return rs;
        }
        public Prosol_SubArea getSubAreaid(string sub, string Area_id)
        {
            var query = Query.And(Query.EQ("Area_Id", Area_id), Query.EQ("SubArea", sub));
            var rs = _SubArearep.FindOne(query);
            return rs;
        }
        public Prosol_Identifier getIdentId(string Ident, string Minor_id)
        {
            var query = Query.And(Query.EQ("fClass_Id", Minor_id), Query.EQ("Identifier", Ident));
            var rs = _Identifierep.FindOne(query);
            return rs;
        }
        public List<Prosol_Identifier> getIdentIdByMinorID(string Minor_id)
        {
            var query = Query.EQ("fClass_Id", Minor_id);
            var rs = _Identifierep.FindAll(query).ToList();
            return rs;
        }
        public Prosol_Identifier getIdentId(string Ident)
        {
            var query = Query.EQ("Identifier", Ident);
            var rs = _Identifierep.FindOne(query);
            return rs;
        }
        public Prosol_EquipmentClass getEquipClassId(string eqC)
        {
            var query = Query.EQ("EquipmentClass", eqC);
            var rs = _EquipClassrep.FindOne(query);
            return rs;
        }
        public Prosol_EquipmentType getEquipTypId(string eqT, string Equcls_id)
        {
            var query = Query.And(Query.EQ("EquClass_Id", Equcls_id), Query.EQ("EquipmentType", eqT));
            var rs = _EquipTyperep.FindOne(query);
            return rs;
        }
        public string getUniqueId(string LogicCode)
        {
            string code = "", itmCode = "";
            // var sort = SortBy.Descending("UniqueId");
            var query = Query.Matches("UniqueId", BsonRegularExpression.Create(new Regex(LogicCode)));
            //var UniqueId = _assetRepository.FindAll(query).OrderByDescending(x=>x.UniqueId).ToList();
            //if (UniqueId != null && UniqueId.Count > 0)
            //{
            //    code = UniqueId[0].UniqueId;
            //}

            //if (code != "")
            //{
            //    long serialinr = Convert.ToInt64(code.Substring(4, 5));


            //    serialinr++;
            //    string addincr = "";
            //    switch (serialinr.ToString().Length)
            //    {
            //        case 1:
            //            addincr = "0000" + serialinr;
            //            break;
            //        case 2:
            //            addincr = "000" + serialinr;
            //            break;
            //        case 3:
            //            addincr = "00" + serialinr;
            //            break;
            //        case 4:
            //            addincr = "0" + serialinr;
            //            break;
            //        case 5:
            //            addincr = serialinr.ToString();
            //            break;
            //    }
            //    itmCode = LogicCode + addincr;

            //}
            //else
            //{
            //    itmCode = LogicCode + "00001";
            //}

            return code;
        }
        public string getBOMUniqueId(string LogicCode)
        {
            string code = "", itmCode = "";
            var sort = SortBy.Descending("_id");
            var query = Query.EQ("EquipmentId", LogicCode);
            var UniqueId = _AssetBOMRepository.FindAll(query, sort).ToList();
            if (UniqueId != null && UniqueId.Count > 0)
            {
                code = UniqueId[0].UniqueId;
            }

            //if (code != "")
            //{
            //    //WAWP00005-001
            //    long serialinr = Convert.ToInt64(code.Substring(10, 3));
            //    serialinr++;
            //    string addincr = "";
            //    switch (serialinr.ToString().Length)
            //    {
            //        case 1:
            //            addincr = "00" + serialinr;
            //            break;
            //        case 2:
            //            addincr = "0" + serialinr;
            //            break;
            //        case 3:
            //            addincr = serialinr.ToString();
            //            break;
            //    }
            //    itmCode = LogicCode + "-" + addincr;

            //}
            //else
            //{
            //    itmCode = LogicCode + "-" + "001";
            //}

            return code;
        }

        //New Asset Bulk Upload


        public virtual string AssetProxyBulk_Upload(HttpPostedFileBase file, string user)
        {
            try
            {
                int cunt = 0;
                Stream stream = file.InputStream;
                IExcelDataReader reader = null;
                // List<Prosol_Import> loaddata = new List<Prosol_Import>();
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

                DataTable dt = res.Tables[0];
                reader.Close();
                string dkfl = dt.Columns[0].ToString();
                var data = new List<Prosol_AssetMaster>();
                var master = GetFarMaster();
                var siteMaster = GetSiteMaster();
                var locMaster = GetLocationMaster();
                var atMaster = GetAssetTypeMaster();

                string error = "";

                if (dt.Columns[0].ToString() == "UNIQUE_ID"
                    && dt.Columns[1].ToString() == "SITEID"
                    && dt.Columns[2].ToString() == "ASSETNUM"
                    && dt.Columns[3].ToString() == "ASSETDESCRIPTION"
                    && dt.Columns[4].ToString() == "ASSETLONGDESCRIPTION"
                    && dt.Columns[5].ToString() == "STATUS"
                    && dt.Columns[6].ToString() == "ASSETTYPE"
                    && dt.Columns[7].ToString() == "SOURCESYSID"
                    && dt.Columns[8].ToString() == "PARENT"
                    && dt.Columns[9].ToString() == "LOCATION"
                    && dt.Columns[10].ToString() == "LOCATION_DESCRIPTION"
                    && dt.Columns[11].ToString() == "LOCATION_HIERARCHY"
                    && dt.Columns[12].ToString() == "FAILURECODE"
                    && dt.Columns[13].ToString() == "MAINTAINEDBY"
                    && dt.Columns[14].ToString() == "OPERATEDBY"
                    && dt.Columns[15].ToString() == "BINNUM"
                    && dt.Columns[16].ToString() == "ITEMNUM"
                    && dt.Columns[17].ToString() == "CONDITIONCODE"
                    && dt.Columns[18].ToString() == "ITEMTYPE"
                    && dt.Columns[19].ToString() == "TOOLRATE"
                    && dt.Columns[20].ToString() == "ASSETTAG"
                    && dt.Columns[21].ToString() == "ASSCOND"
                    && dt.Columns[22].ToString() == "PMNUM"
                    && dt.Columns[23].ToString() == "MAINTENANCE_EXCEPTION"
                    && dt.Columns[24].ToString() == "DEFAULTREPFACSITEID"
                    && dt.Columns[25].ToString() == "INSTALLDATE"
                    && dt.Columns[26].ToString() == "CALNUM"
                    && dt.Columns[27].ToString() == "SHIFT"
                    && dt.Columns[28].ToString() == "FAILURE_CONSEQUENCE"
                    && dt.Columns[29].ToString() == "FAILURE_LIKELIHOOD"
                    && dt.Columns[30].ToString() == "CRITICALITYRANGE"
                    && dt.Columns[31].ToString() == "PRIORITY"
                    && dt.Columns[32].ToString() == "REPORTGROUP"
                    && dt.Columns[33].ToString() == "GLACCOUNT"
                    && dt.Columns[34].ToString() == "CAPEXGLACCOUNT"
                    && dt.Columns[35].ToString() == "FIXEDASSETNUM"
                    && dt.Columns[36].ToString() == "SERIALNUM"
                    && dt.Columns[37].ToString() == "MODELNUM"
                    && dt.Columns[38].ToString() == "MODELYEAR"
                    && dt.Columns[39].ToString() == "REGNO"
                    && dt.Columns[40].ToString() == "EQREPLACED"
                    && dt.Columns[41].ToString() == "LEGACYASSETNUM"
                    && dt.Columns[42].ToString() == "OWNEDBYSITE"
                    && dt.Columns[43].ToString() == "OWNERSHIPTYPE"
                    && dt.Columns[44].ToString() == "LEASED_INFO"
                    && dt.Columns[45].ToString() == "SADDRESSCODE"
                    && dt.Columns[46].ToString() == "SERVICEADDRESS.FORMATTEDADDRESS"
                    && dt.Columns[47].ToString() == "SERVICEADDRESS.CITY"
                    && dt.Columns[48].ToString() == "SERVICEADDRESS.STATEPROVINCE"
                    && dt.Columns[49].ToString() == "VENDOR"
                    && dt.Columns[50].ToString() == "COMPANY.NAME"
                    && dt.Columns[51].ToString() == "MANUFACTURER"
                    && dt.Columns[52].ToString() == "RECEIVEDBY"
                    && dt.Columns[53].ToString() == "RECEIPTDATE"
                    && dt.Columns[54].ToString() == "INSPECTIONDATE"
                    && dt.Columns[55].ToString() == "WARRANTYEXPDATE"
                    && dt.Columns[56].ToString() == "LOADCERTEXPDATE"
                    && dt.Columns[57].ToString() == "TRAFFICCERTEXPDATE"
                    && dt.Columns[58].ToString() == "CALCERTEXPDATE"
                    && dt.Columns[59].ToString() == "EXPECTEDLIFE"
                    && dt.Columns[60].ToString() == "ESTIMATED_EOL"
                    && dt.Columns[61].ToString() == "PURCHASEPRICE"
                    && dt.Columns[62].ToString() == "PURCHASEDATE"
                    && dt.Columns[63].ToString() == "REPLACECOST"
                    && dt.Columns[64].ToString() == "POCONTRACT"
                    && dt.Columns[65].ToString() == "TOTALCOST"
                    && dt.Columns[66].ToString() == "YTDCOST"
                    && dt.Columns[67].ToString() == "BUDGETCOST"
                    && dt.Columns[68].ToString() == "MANAGEBY"
                    && dt.Columns[69].ToString() == "DAILYTARGET"
                    && dt.Columns[70].ToString() == "COMMENTS"
                    && dt.Columns[71].ToString() == "INSURANCECAT"
                    && dt.Columns[72].ToString() == "INSURANCENUM"
                    && dt.Columns[73].ToString() == "INSURANCEPREMRATE"
                    && dt.Columns[74].ToString() == "INSURANCEPREMCOSTYEAR"
                    && dt.Columns[75].ToString() == "INSURANCESTARTDATE"
                    && dt.Columns[76].ToString() == "INSURANCEENDDATE"
                    && dt.Columns[77].ToString() == "STATUSDATE"
                    && dt.Columns[78].ToString() == "TOTDOWNTIME"
                    && dt.Columns[79].ToString() == "CHANGEBY"
                    && dt.Columns[80].ToString() == "CHANGEDATE"
                    && dt.Columns[81].ToString() == "SLA_GROSSRHMOVE"
                    && dt.Columns[82].ToString() == "SLA_MTBF"
                    && dt.Columns[83].ToString() == "SLA_MANMINRHMOVE"
                    && dt.Columns[84].ToString() == "SLA_SRVCOSTRHMOVE"
                    && dt.Columns[85].ToString() == "SLA_MAINTCOST"
                    && dt.Columns[86].ToString() == "SLA_MAINCOSTRHMOVE"
                    && dt.Columns[87].ToString() == "SLA_RELIABILITY"
                    && dt.Columns[88].ToString() == "SLA_KEYGROSSPROD"
                    && dt.Columns[89].ToString() == "SLA_RHMOVE"
                    && dt.Columns[90].ToString() == "SLA_PARTCOSTRHMOVE"
                    && dt.Columns[91].ToString() == "SLA_IND"
                    && dt.Columns[92].ToString() == "CLASSSTRUCTUREID"
                    && dt.Columns[93].ToString() == "PLUSCASSETSTATUS.CREATIONDATE"
                    && dt.Columns[94].ToString() == "DLPEXPDATE"
                    && dt.Columns[95].ToString() == "Present Location"
                    && dt.Columns[96].ToString() == "GIS Coordinates"
                    && dt.Columns[97].ToString() == "Existing Barcode"
                    )
                {
                    int i = 1;
                    foreach (DataRow row in dt.Rows)
                    {

                        var qry = Query.EQ("UniqueId", row["UNIQUE_ID"].ToString());
                        var assMdl = _assetRepository.FindOne(qry);
                        if (assMdl == null)
                        {
                            i++;

                            Prosol_AssetMaster brl1 = new Prosol_AssetMaster();
                            brl1.UniqueId = row["UNIQUE_ID"].ToString();
                            if (string.IsNullOrEmpty(brl1.UniqueId))
                                error = error + "Invalid Reportgroup (Cell A" + i + ")" + '\n';


                            brl1.AssetNo = row["ASSETNUM"].ToString();
                            if (string.IsNullOrEmpty(brl1.AssetNo))
                                error = error + "Invalid Reportgroup (Cell C" + i + ")" + '\n';

                            brl1.Description = row["ASSETDESCRIPTION"].ToString();
                            brl1.Description_ = row["ASSETLONGDESCRIPTION"].ToString();
                            brl1.Status = row["STATUS"].ToString().ToUpper();
                            brl1.Parent = row["PARENT"].ToString().ToUpper();
                            brl1.AssetQRCode = row["ASSETTAG"].ToString();
                            brl1.NewTagNo = row["ASSETTAG"].ToString();
                            brl1.AssCondition = row["ASSCOND"].ToString();
                            brl1.SerialNo = row["SERIALNUM"].ToString();
                            brl1.ModelNo = row["MODELNUM"].ToString();
                            brl1.ModelYear = row["MODELYEAR"].ToString();
                            var ClassificationId = row["CLASSSTRUCTUREID"].ToString();
                            if (ClassificationId != null)
                            {
                                if (ClassificationId.Contains(","))
                                {
                                    var clsSet = string.IsNullOrEmpty(ClassificationId)
                                                    ? new string[] { }
                                                    : ClassificationId.Split(',')
                                                                      .Select(s => s.Trim())
                                                                      .Where(s => !string.IsNullOrEmpty(s))
                                                                      .ToArray();
                                    brl1.Noun = clsSet[0];
                                    brl1.Modifier = clsSet[1];
                                }
                                else
                                {
                                    brl1.Noun = row["CLASSSTRUCTUREID"].ToString();
                                    brl1.Modifier = "--";
                                }
                            }
                            var GisCoordinates = row["GIS Coordinates"].ToString();

                            if (GisCoordinates != null)
                            {
                                if (GisCoordinates.Contains(" "))
                                {
                                    brl1.GIS = new Prosol_AssetGIS();
                                    var coSet = string.IsNullOrEmpty(GisCoordinates)
                                                    ? new string[] { }
                                                    : GisCoordinates.Split(' ')
                                                                      .Select(s => s.Trim())
                                                                      .Where(s => !string.IsNullOrEmpty(s))
                                                                      .ToArray();
                                    brl1.GIS.LattitudeStart = coSet[0];
                                    brl1.GIS.LongitudeStart = coSet[1];
                                }
                            }

                            //var userQry = Query.EQ("Userid", user);
                            //var userDet = _UsercreateRepository.FindOne(userQry);
                            //var userMdl = new Prosol_UpdatedBy();
                            //userMdl.UserId = userDet.Userid;
                            //userMdl.Name = userDet.UserName;
                            //userMdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                            //brl1.User = userMdl;
                            brl1.category = "Equipment";
                            brl1.ItemStatus = 6;
                            //var pvuser = row["PVUser"].ToString();
                            //if (pvuser != "" && pvuser != null)
                            //{
                            //    var pvQry = Query.EQ("UserName", pvuser);
                            //    var pvuserDet = _UsercreateRepository.FindOne(pvQry);
                            //    var pvMdl = new Prosol_UpdatedBy();
                            //    pvMdl.UserId = pvuserDet.Userid;
                            //    pvMdl.Name = pvuserDet.UserName;
                            //    pvMdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                            //    brl1.PVuser = pvMdl;
                            //    brl1.ItemStatus = 1;
                            //}
                            data.Add(brl1);
                            var ress = _assetRepository.Add(brl1);
                            cunt++;
                        }
                    }
                    //  string str = "Test Me\r\nTest Me\nTest Me";
                    // var splitted = error.Split('\n').Select(s => s.Trim()).ToArray();
                    if (!string.IsNullOrEmpty(error))
                    {
                        return error;
                    }
                    var msg = "";
                    if (cunt >= 1)
                    {
                        msg = "Success";
                    }
                    return msg;
                }
                else
                {

                }
            }

            catch (Exception e)
            {
                return e.ToString();
            }

            return null;
        }

        //Old Asset Bulk Upload

        public virtual string AssetBulk_Upload(HttpPostedFileBase file, string user)
        {
            try
            {
                int cunt = 0;
                Stream stream = file.InputStream;
                IExcelDataReader reader = null;
                // List<Prosol_Import> loaddata = new List<Prosol_Import>();
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

                DataTable dt = res.Tables[0];
                reader.Close();
                string dkfl = dt.Columns[0].ToString();
                var data = new List<Prosol_AssetMaster>();
                //var master = GetFarMaster();
                //var siteMaster = GetSiteMaster();
                var locMaster = GetLocationMaster();
                //var atMaster = GetAssetTypeMaster();

                string error = "";
                var allUsers = _UsercreateRepository.FindAll().ToList();

                if (dt.Columns[0].ToString() == "UNIQUE ID" &&
                    dt.Columns[1].ToString() == "EQUIPMENT NO." &&
                    dt.Columns[2].ToString() == "TECHNICAL IDENT. (TAG NO)" &&
                    dt.Columns[3].ToString() == "P&ID NUMBER" &&
                    dt.Columns[4].ToString() == "P&ID NAME" && 
                    dt.Columns[5].ToString() == "SECTION NUMBER" &&
                    dt.Columns[6].ToString() == "SECTION NAME" &&
                    dt.Columns[7].ToString() == "SYSTEM" &&
                    dt.Columns[8].ToString() == "DISCIPLINE" &&
                    dt.Columns[9].ToString() == "SHORT DESCRIPTION" &&
                    dt.Columns[10].ToString() == "EQUIPMENT DESCRIPTION" &&
                    dt.Columns[11].ToString() == "PARENT EQUIPMENT" &&
                    dt.Columns[12].ToString() == "OBJECT TYPE" &&
                    dt.Columns[13].ToString() == "NOUN" &&
                    dt.Columns[14].ToString() == "MODIFIER" &&
                    dt.Columns[15].ToString() == "UNSPSC CODE" &&
                    dt.Columns[16].ToString() == "PLANT" &&
                    dt.Columns[17].ToString() == "ORG. CODE" &&
                    dt.Columns[18].ToString() == "COST CENTER" &&
                    dt.Columns[19].ToString() == "COST CENTER DESCRIPTION" &&
                    dt.Columns[20].ToString() == "ABC INDICATOR" &&
                    dt.Columns[21].ToString() == "EQPRT. CATEGORY" &&
                    dt.Columns[22].ToString() == "MAIN WORK CENTER" &&
                    dt.Columns[23].ToString() == "FUNCTIONAL. LOCATION. NO." &&
                    dt.Columns[24].ToString() == "MANUFACTURER" &&
                    dt.Columns[25].ToString() == "MODEL NO." &&
                    dt.Columns[26].ToString() == "MANUFACTURER PART NUMBER" &&
                    dt.Columns[27].ToString() == "MANUFACTURER SERIAL NUMBER" &&
                    dt.Columns[28].ToString() == "MANUFACTURER COUNTRY" &&
                    dt.Columns[29].ToString() == "YEAR OF MANUFACTURE" &&
                    dt.Columns[30].ToString() == "ADDITIONAL INFORMATION" &&
                    dt.Columns[31].ToString() == "PV USER")
                {
                    int i = 1;
                    foreach (DataRow row in dt.Rows)
                    {

                        var qry = Query.EQ("UniqueId", row["UNIQUE ID"].ToString());
                        var assMdl = _assetRepository.FindOne(qry);
                        if (assMdl == null)
                        {
                            i++;

                            Prosol_AssetMaster brl1 = new Prosol_AssetMaster();

                            //SiteId
                            brl1.AssetNo = row["EQUIPMENT NO."].ToString();
                            brl1.UniqueId = row["UNIQUE ID"].ToString();

                            if (brl1.UniqueId == null)
                            {
                                brl1.UniqueId = getlast_request_R_no();
                            }

                            //AssetNo
                            if (brl1.AssetNo != null)
                                brl1.AssetNo = brl1.AssetNo;
                            else
                                error = error + "AssetNumber Invalid (Cell B" + i + ")" + '\n';

                            brl1.TechIdentNo = row["TECHNICAL IDENT. (TAG NO)"].ToString().ToUpper();
                            brl1.PID_Number = row["P&ID NUMBER"].ToString().ToUpper();
                            brl1.PID_Desc = row["P&ID NAME"].ToString().ToUpper();

                            brl1.Section_Number = row["SECTION NUMBER"].ToString().ToUpper();
                            brl1.Section_Desc = row["SECTION NAME"].ToString().ToUpper();

                            brl1.System = row["SYSTEM"].ToString().ToUpper();
                            brl1.Discipline = row["DISCIPLINE"].ToString().ToUpper();

                            //Description
                            brl1.Description = row["SHORT DESCRIPTION"].ToString();

                            if (string.IsNullOrEmpty(brl1.Description))
                                error = error + "Invalid Description (Cell C" + i + ")" + '\n';

                            brl1.Description_ = row["EQUIPMENT DESCRIPTION"].ToString();
                            brl1.Parent = row["PARENT EQUIPMENT"].ToString();
                            brl1.ObjType = row["OBJECT TYPE"].ToString();

                            brl1.Noun = row["NOUN"].ToString();
                            brl1.Modifier = row["MODIFIER"].ToString();
                            brl1.Unspsc = row["UNSPSC CODE"].ToString();

                            brl1.Plant = row["PLANT"].ToString();
                            brl1.Org_Code = row["ORG. CODE"].ToString();
                            brl1.CostCenter = row["COST CENTER"].ToString();
                            brl1.CostCenter_Desc = row["COST CENTER DESCRIPTION"].ToString();
                            brl1.ABC_Indicator = row["ABC INDICATOR"].ToString();
                            brl1.Equ_Category = row["EQPRT. CATEGORY"].ToString();
                            brl1.MainWorkCenter = row["MAIN WORK CENTER"].ToString();
                            brl1.Func_Location = row["FUNCTIONAL. LOCATION. NO."].ToString();
                            brl1.Manufacturer = row["MANUFACTURER"].ToString();
                            brl1.ModelNo = row["MODEL NO."].ToString();
                            brl1.PartNo = row["MANUFACTURER PART NUMBER"].ToString();
                            brl1.SerialNo = row["MANUFACTURER SERIAL NUMBER"].ToString();
                            brl1.MfrCountry = row["MANUFACTURER COUNTRY"].ToString();
                            brl1.MfrYear = row["YEAR OF MANUFACTURE"].ToString();
                            brl1.AdditionalNotes = row["ADDITIONAL INFORMATION"].ToString();

                            //Asset Attributes Update
                            var nmqry = Query.EQ("UniqueId", brl1.UniqueId);
                            var nmDet = _AssetattriRepository.FindOne(nmqry);
                            if (nmDet == null)
                            {
                                var nm = new Prosol_AssetAttributes();
                                nm.UniqueId = brl1.UniqueId;
                                nm.Noun = brl1.Noun;
                                nm.Modifier = brl1.Modifier;
                                _AssetattriRepository.Add(nm);
                            }
                            else
                            {
                                nmDet.Noun = brl1.Noun;
                                nmDet.Modifier = brl1.Modifier;
                                _AssetattriRepository.Add(nmDet);
                            }

                            var userMdl = new Prosol_UpdatedBy();
                            var userDet = allUsers.Where(u => u.Userid == user).ToList();
                            userMdl.UserId = userDet[0].Userid;
                            userMdl.Name = userDet[0].UserName;
                            userMdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                            brl1.User = userMdl;
                            brl1.category = "Equipment";
                            brl1.PVstatus = "Pending";
                            var pvuser = row["PV USER"].ToString();
                            if (pvuser != "" && pvuser != null)
                            {
                                var pvMdl = new Prosol_UpdatedBy();
                                var pvuserDet = allUsers.Where(u => u.UserName == pvuser).ToList();
                                pvMdl.UserId = pvuserDet[0].Userid;
                                pvMdl.Name = pvuserDet[0].UserName;
                                pvMdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                                brl1.PVuser = pvMdl;
                                brl1.ItemStatus = 1;
                            }
                            //data.Add(brl1);
                            cunt++;
                            var ress = _assetRepository.Add(brl1);
                        }
                    }
                    //  string str = "Test Me\r\nTest Me\nTest Me";
                    // var splitted = error.Split('\n').Select(s => s.Trim()).ToArray();
                    if (!string.IsNullOrEmpty(error))
                    {
                        return error;
                    }
                    var msg = "";
                    if (cunt >= 1)
                    {
                        msg = "Success";
                    }
                    return msg;
                }
            }

            catch (Exception e)
            {
                return e.ToString();
            }

            return null;
        }
        public virtual string BOMBulk_Upload(HttpPostedFileBase file)
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
            DataTable dt = res.Tables[1];

            string[] columns = {  "FUNCTIONAL LOCATION", "TECHNICAL IDENT. (TAG NO)", "BOMID", "ASSEMBLYID", "MATERIALCODE", "ITEMCATEGORY", "SEQUENCE", "QUANTITY", "UOM", "HEADERDESCRIPTION", "HEADERLONGDESCRIPTION", "ASSEMBLYDESCRIPTION", "ASSEMBLYLONGDESCRIPTION", "COMPONENTDESCRIPTION" , "COMPONENTLONGDESCRIPTION" };
            string unqId = "";
            bool colErr = false;

            if (dt.Rows.Count > 0)
            {
                foreach (DataColumn d in dt.Columns)
                {
                    if (columns.Contains(d.ColumnName.ToUpper().Trim()) == false)
                    {
                        colErr = true;
                        return d.ColumnName + " is invalid columns name, please use below columns names:-  FUNCTIONAL LOCATION, TECHNICAL IDENT. (TAG NO), BOMID, ASSEMBLYID, MATERIALCODE, ITEMCATEGORY, SEQUENCE, QUANTITY, UOM, HEADERDESCRIPTION, ASSEMBLYDESCRIPTION, COMPONENTDESCRIPTION";

                    }

                }
                if(!colErr)
                {
                    unqId = "BOMC-" + (GetLastRunning("BOM") + 1).ToString("D6");
                    UpdateRunning("BOM", int.Parse(unqId.Substring(5)));
                }
                foreach (DataRow drw1 in dt.Rows)
                {
                    
                    //var bomquery = Query.EQ("BOMId", drw1[0].ToString());
                    //var mdl = _AssetBOMRepository.FindOne(bomquery);
                    //if (mdl == null)
                    //{
                        var mdl = new Prosol_AssetBOM();
                        mdl.Func_Location = drw1[0].ToString();
                        mdl.TechIdentNo = drw1[1].ToString();
                        mdl.BOMId = drw1[2].ToString();
                        mdl.AssemblyId = drw1[3].ToString();
                        mdl.ComponentId = drw1[4].ToString();
                        mdl.Category = drw1[5].ToString();
                        mdl.Sequence = drw1[6].ToString();
                        mdl.Quantity = drw1[7].ToString();
                        mdl.UOM = drw1[8].ToString();
                        mdl.BOMDesc = drw1[9].ToString();
                        mdl.BOMLongDesc = drw1[10].ToString();
                        mdl.AssemblyDesc = drw1[11].ToString();
                        mdl.AssemblyLongDesc = drw1[12].ToString();
                        mdl.ComponentDesc = drw1[13].ToString();
                        mdl.ComponentLongDesc = drw1[14].ToString();
                        mdl.UniqueId = unqId;
                        _AssetBOMRepository.Add(mdl);
                        cunt++;
                    //}
                }
                foreach (DataRow drw in dt1.Rows)
                {
                    var query = Query.Or(Query.EQ("UniqueId", drw[0].ToString()), Query.EQ("AssetNo", drw[0].ToString()));
                    var ast = _assetRepository.FindOne(query);
                    if (ast != null)
                    {
                        ast.Bom = new BOM();
                        ast.Bom.BOMId = unqId;
                        //ast.Bom.BOMId = drw[1].ToString();
                        _assetRepository.Add(ast);
                    }
                }
            }


            return "Success " + cunt.ToString();


        }
        //public virtual string BOMBulk_Upload(HttpPostedFileBase file)
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
        //    DataTable dt = res.Tables[0];

        //    string[] columns = { "EQUIPMENTUNIQUEID", "SUBSYSTEM/IDENTIFIER", "EQUIPMENTDESCRIPTION", "BOM", "FUNCTION/MINORCLASS", "QTY" };


        //    if (dt.Rows.Count > 0)
        //    {
        //        foreach (DataRow drw1 in dt.Rows)
        //        {
        //            var query2 = Query.EQ("UniqueId", drw1[0].ToString());
        //            var mdl2 = _assetRepository.FindOne(query2);
        //            if (mdl2 != null)
        //            {

        //                var bom = new BOM();
        //                if (mdl2.Bom != null)
        //                {
        //                    bom = mdl2.Bom;
        //                    bom.BOMId = mdl2.Bom.BOMId;
        //                    bom.BOMDescription = mdl2.Bom.BOMDescription;
        //                }
        //                else
        //                {
        //                    bom.BOMId = drw1[2].ToString();
        //                    bom.BOMDescription = drw1[3].ToString();
        //                }
        //                if (mdl2.Bom != null && mdl2.Bom.Assembly != null)
        //                    bom.Assembly = mdl2.Bom.Assembly;
        //                else
        //                    bom.Assembly = new List<ASSEMBLY>();
        //                if (!string.IsNullOrEmpty(drw1[4].ToString()))
        //                {
        //                    var assmb = new ASSEMBLY();
        //                    assmb.AssemblyId = drw1[4].ToString();
        //                    assmb.AssemblyDescription = drw1[5].ToString();
        //                    bom.Assembly.Add(assmb);
        //                }
        //                if (mdl2.Bom != null && mdl2.Bom.Mat != null)
        //                    bom.Mat = mdl2.Bom.Mat;
        //                else
        //                    bom.Mat = new List<MAT>();
        //                if (!string.IsNullOrEmpty(drw1[6].ToString()))
        //                {
        //                    var mtl = new MAT();
        //                    mtl.Materialcode = drw1[6].ToString();
        //                    mtl.MaterialDescription = drw1[7].ToString();
        //                    mtl.Quantity = drw1[8].ToString();
        //                    mtl.UOM = drw1[9].ToString();
        //                    bom.Mat.Add(mtl);
        //                }
        //                mdl2.Bom = bom;
        //                _assetRepository.Add(mdl2);
        //            }
        //            cunt++;
        //        }
        //    }


        //    return "Success " + cunt.ToString();


        //}

        //OLD
        //public virtual string BOMBulk_Upload(HttpPostedFileBase file)
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
        //    DataTable dt = res.Tables[0];

        //    string[] columns = { "EQUIPMENTUNIQUEID", "SUBSYSTEM/IDENTIFIER", "EQUIPMENTDESCRIPTION", "BOM", "FUNCTION/MINORCLASS", "QTY" };


        //    if (dt.Rows.Count > 0)
        //    {
        //        foreach (DataColumn d in dt.Columns)
        //        {
        //            if (columns.Contains(d.ColumnName.ToUpper().Trim()) == false)
        //            {

        //                return d.ColumnName + " is invalid columns name, please use below columns names:-  EquipmentUniqueID, SubSystem/Identifier, EquipmentDescription, BOM, Function/MinorClass, Qty";


        //            }

        //        }
        //        string equpID = ""; long serialinr = 0;
        //        var objlist = new Prosol_AssetMaster();
        //        foreach (DataRow drw in dt.Rows)
        //        {

        //            if (!string.IsNullOrEmpty(drw["EquipmentUniqueID"].ToString()))
        //            {
        //                var qry1 = Query.EQ("UniqueId", drw["EquipmentUniqueID"].ToString());
        //                if (string.IsNullOrEmpty(equpID) || equpID != drw["EquipmentUniqueID"].ToString())
        //                    objlist = _assetRepository.FindOne(qry1);

        //                if (objlist != null)
        //                {

        //                    // var gQry = Query.And(Query.EQ("EquipmentId", drw["EquipmentUniqueID"].ToString()), Query.EQ("BOMName", drw["BOM"].ToString().Trim()));
        //                    // var bomMdl1 = _AssetBOMRepository.FindOne(gQry);
        //                    //  if (bomMdl1 == null)
        //                    // {
        //                    var bomMdl = new Prosol_AssetBOM();
        //                    if (string.IsNullOrEmpty(equpID) || equpID != drw["EquipmentUniqueID"].ToString())
        //                    {
        //                        //string code = getBOMUniqueId(objlist.UniqueId);
        //                        //if (code != "")
        //                        //{
        //                        //    //WAWP00005-001
        //                        //     serialinr = Convert.ToInt64(code.Substring(10, 3));
        //                        //    serialinr++;
        //                        //    string addincr = "";
        //                        //    switch (serialinr.ToString().Length)
        //                        //    {
        //                        //        case 1:
        //                        //            addincr = "00" + serialinr;
        //                        //            break;
        //                        //        case 2:
        //                        //            addincr = "0" + serialinr;
        //                        //            break;
        //                        //        case 3:
        //                        //            addincr = serialinr.ToString();
        //                        //            break;
        //                        //    }
        //                        //    //bomMdl.UniqueId = objlist.UniqueId + "-" + addincr;

        //                        //}
        //                        //else
        //                        //{
        //                        //    bomMdl.UniqueId = objlist.UniqueId + "-" + "001";
        //                        //    serialinr = 1;
        //                        //}

        //                    }
        //                    else
        //                    {

        //                        serialinr = serialinr + 1;
        //                        string addincr = "";
        //                        switch (serialinr.ToString().Length)
        //                        {
        //                            case 1:
        //                                addincr = "00" + serialinr;
        //                                break;
        //                            case 2:
        //                                addincr = "0" + serialinr;
        //                                break;
        //                            case 3:
        //                                addincr = serialinr.ToString();
        //                                break;
        //                        }
        //                        //bomMdl.UniqueId = objlist.UniqueId + "-" + addincr;

        //                    }

        //                    //bomMdl.EquipmentId = objlist.UniqueId;
        //                    bomMdl.BOMName = drw["BOM"].ToString();
        //                    bomMdl.Qty = string.IsNullOrEmpty(drw["Qty"].ToString()) ? 0 : Convert.ToInt32(drw["Qty"]);
        //                    _AssetBOMRepository.Add(bomMdl);
        //                    //equpID = objlist.UniqueId;
        //                    cunt++;
        //                    // }
        //                }
        //                else
        //                    return cunt + " Items uploaded, Row:" + lineId + ", " + drw["EquipmentUniqueID"].ToString() + " not exist in the DB";
        //            }
        //            else if (!string.IsNullOrEmpty(drw["EquipmentDescription"].ToString()))
        //            {
        //                var funObj = getMinorid(drw["Function/MinorClass"].ToString());
        //                var idenObj = getIdentId(drw["SubSystem/Identifier"].ToString(), funObj._id.ToString());
        //                var Qry = Query.And(Query.EQ("MinorClass", funObj._id.ToString()), Query.EQ("Identifier", idenObj._id.ToString()), Query.EQ("EquipmentDesc", drw["EquipmentDescription"].ToString()));
        //                if (string.IsNullOrEmpty(equpID) || equpID != drw["EquipmentUniqueID"].ToString())
        //                    objlist = _assetRepository.FindOne(Qry);
        //                if (objlist != null)
        //                {

        //                    //var gQry = Query.And(Query.EQ("EquipmentId", objlist.UniqueId), Query.EQ("BOMName", drw["BOM"].ToString().Trim()));
        //                    //var bomMdl1 = _AssetBOMRepository.FindOne(gQry);
        //                    //if (bomMdl1 == null)
        //                    //{
        //                    //    var bomMdl = new Prosol_AssetBOM();
        //                    //    if (string.IsNullOrEmpty(equpID) || equpID != drw["EquipmentUniqueID"].ToString())
        //                    //    {
        //                    //        //string code = getBOMUniqueId(objlist.UniqueId);
        //                    //        //if (code != "")
        //                    //        //{
        //                    //        //    //WAWP00005-001
        //                    //        //    serialinr = Convert.ToInt64(code.Substring(10, 3));
        //                    //        //    serialinr++;
        //                    //        //    string addincr = "";
        //                    //        //    switch (serialinr.ToString().Length)
        //                    //        //    {
        //                    //        //        case 1:
        //                    //        //            addincr = "00" + serialinr;
        //                    //        //            break;
        //                    //        //        case 2:
        //                    //        //            addincr = "0" + serialinr;
        //                    //        //            break;
        //                    //        //        case 3:
        //                    //        //            addincr = serialinr.ToString();
        //                    //        //            break;
        //                    //        //    }
        //                    //        //    bomMdl.UniqueId = objlist.UniqueId + "-" + addincr;

        //                    //        //}
        //                    //        //else
        //                    //        //{
        //                    //        //    bomMdl.UniqueId = objlist.UniqueId + "-" + "001";
        //                    //        //    serialinr = 1;
        //                    //        //}

        //                    //    }
        //                    //    else
        //                    //    {

        //                    //        serialinr = serialinr + 1;
        //                    //        string addincr = "";
        //                    //        switch (serialinr.ToString().Length)
        //                    //        {
        //                    //            case 1:
        //                    //                addincr = "00" + serialinr;
        //                    //                break;
        //                    //            case 2:
        //                    //                addincr = "0" + serialinr;
        //                    //                break;
        //                    //            case 3:
        //                    //                addincr = serialinr.ToString();
        //                    //                break;
        //                    //        }
        //                    //        //bomMdl.UniqueId = objlist.UniqueId + "-" + addincr;

        //                    //    }
        //                    //    //bomMdl.EquipmentId = objlist.UniqueId;
        //                    //    bomMdl.BOMName = drw["BOM"].ToString();
        //                    //    bomMdl.Qty = string.IsNullOrEmpty(drw["Qty"].ToString()) ? 0 : Convert.ToInt32(drw["Qty"]);
        //                    //    _AssetBOMRepository.Add(bomMdl);
        //                    //    //equpID = objlist.UniqueId;
        //                    //    cunt++;
        //                    //}
        //                }
        //                else return cunt + " Items uploaded, Row:" + lineId + ", " + drw["EquipmentDescription"].ToString() + " not exist in the DB";
        //            }
        //            else
        //            {
        //                return cunt + " Items uploaded, Row:" + lineId + ", Equipment should not be blanck";
        //            }
        //            lineId++;
        //        }
        //    }


        //    return "Success " + cunt.ToString();


        //}
        //public virtual string AttriBulk_Upload(HttpPostedFileBase file)
        //{
        //    //var nQry = Query.And(Query.EQ("Noun", BsonNull.Value), Query.EQ("Modifier", BsonNull.Value));
        //    var nQry = Query.EQ("Modifier", BsonNull.Value);
        //    var qryLst = _AssetattriRepository.FindAll(nQry).ToList();
        //    foreach(var dt in qryLst)
        //    {
        //        dt.Noun = dt.exNoun;
        //        dt.Modifier = dt.exModifier;
        //        _AssetattriRepository.Add(dt);
        //    }
        //    return "success";
        //}

        public virtual string AttriBulk_Upload(HttpPostedFileBase file)
        {
            int cunt = 0, lineId = 0;
            var item = new List<string>();
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

            string[] columns = { "EQUIPMENTUNIQUEID", "NOUN", "MODIFIER", "ATTRIBUTE", "VALUE", "UOM" };

            //var assetAttr = _AssetattriRepository.FindAll().ToList();
            //foreach (var attr in assetAttr)
            //{
            //    attr.exCharacterisitics = null;
            //    _AssetattriRepository.Add(attr);
            //    cunt++;
            //}


            if (dt.Rows.Count > 0)
            {
                foreach (DataColumn d in dt.Columns)
                {
                    if (columns.Contains(d.ColumnName.ToUpper().Trim()) == false)
                    {

                        return d.ColumnName + " is invalid columns name, please use below columns names:-  EquipmentUniqueID, Noun, Modifier, Attribute, Value, UOM";


                    }

                }
                string itemcode = "";
                foreach (DataRow drw in dt.Rows)
                {
                    if (itemcode != drw[0].ToString() && !string.IsNullOrEmpty(drw[0].ToString()) && !item.Contains(drw[0].ToString()))
                    {
                        itemcode = drw[0].ToString();
                        List<String> charlist = new List<String>();
                        if (!string.IsNullOrEmpty(drw["EquipmentUniqueID"].ToString()))
                        {
                            //var qry1 = Query.EQ("UniqueId", drw["EquipmentUniqueID"].ToString());
                            //var objlist = _assetRepository.FindOne(qry1);
                            //if (objlist != null)
                            //{

                            //var gQry = Query.EQ("UniqueId", drw["EquipmentUniqueID"].ToString());
                            //var AttriMdl1 = _AssetattriRepository.FindOne(gQry);
                            //if (AttriMdl1 != null)
                            //{
                            //if (drw["EquipmentUniqueID"].ToString() == "ADE096417")
                            //{

                            //}
                            var quryy = Query.EQ("UniqueId", drw["EquipmentUniqueID"].ToString());
                            var AttriMdl1 = _AssetattriRepository.FindOne(quryy);
                            if (AttriMdl1 == null)
                                AttriMdl1 = new Prosol_AssetAttributes();
                            AttriMdl1.UniqueId = drw["EquipmentUniqueID"].ToString();
                            AttriMdl1.Noun = drw["Noun"].ToString();
                            //AttriMdl1.exNoun = drw["Noun"].ToString();
                            AttriMdl1.Modifier = drw["Modifier"].ToString();
                            //AttriMdl1.exModifier = drw["Modifier"].ToString();
                            if (!string.IsNullOrEmpty(drw["Noun"].ToString()))
                            {
                                var attrList = new List<Asset_AttributeList>();
                                var nQry = Query.And(Query.EQ("Noun", AttriMdl1.Noun), Query.EQ("Modifier", AttriMdl1.Modifier), Query.EQ("Definition", "Equ"));
                                var mdl2 = _CharateristicRepository.FindAll(nQry).ToList();
                                if (mdl2 == null)
                                {
                                    return drw["EquipmentUniqueID"].ToString() + " : Noun: " + drw["Noun"].ToString() + " is not available in the Dictionary";
                                }
                                attrList = (from DataRow drw1 in dt.Rows
                                            where drw1["EquipmentUniqueID"].ToString() == drw[0].ToString()
                                            select new Asset_AttributeList()
                                            {
                                                Characteristic = drw1["Attribute"] != DBNull.Value ? drw1["Attribute"].ToString() : "",
                                                Value = drw1["Value"] != DBNull.Value ? drw1["Value"].ToString() : "",
                                                UOM = drw1["UOM"] != DBNull.Value ? drw1["UOM"].ToString() : "",
                                                // Mandatory = drw1["Mandatory"] != DBNull.Value ? drw1["Mandatory"].ToString() : "",
                                                Squence = (short)0,
                                                ShortSquence = (short)0
                                            })
                                            .OrderBy(x => x.ShortSquence)
                                            .ToList();


                                if (mdl2.Count != attrList.Count)
                                {
                                    return itemcode + " : No. of Characteristics is MisMatch with Dictionary";
                                }

                                foreach (var x in attrList)
                                {
                                    if (string.IsNullOrEmpty(x.Value) && !string.IsNullOrEmpty(x.UOM))
                                    {
                                        x.UOM = "";
                                    }
                                    if (mdl2.Any(g => g.Characteristic == x.Characteristic) == false)
                                    {
                                        return itemcode + " : Characteristic: " + x.Characteristic + " is not available in the Dictionary";
                                    }
                                    //if (x.Value.Length > 40)
                                    //{
                                    //    return itemcode + " : Characteristic: " + x.Characteristic + " Value:" + x.Value + " is exceeded of 40 chars limit";
                                    //}
                                    //if (!String.IsNullOrEmpty(x.UOM) && (mdl3.Any(g => g.Unitname == x.UOM) == false))
                                    //{
                                    //    return itemcode + " : Characteristic: " + x.Characteristic + "; Value:" + x.Value + "; UOM :" + x.UOM + " is not exist in the master, please add it";
                                    //}
                                    if (charlist.Any(str => str.Equals(x.Characteristic)))
                                    {
                                        return itemcode + " : Characteristic: " + x.Characteristic + " is Duplicate";
                                    }

                                    //if (!String.IsNullOrEmpty(x.UOM))
                                    //{
                                    //    var q = mdl3.Where(g => g.Unitname == x.UOM).ToList();
                                    //    if (mdl4.Any(g => g.UOMList.Contains(Convert.ToString(q[0]._id))) == false)
                                    //    {
                                    //        return itemcode + ": UOM :" + x.UOM + " is not linked with  Characteristic: " + x.Characteristic + " in the Attribute master, please add it";
                                    //    }

                                    //}
                                    charlist.Add(x.Characteristic);
                                }
                                AttriMdl1.Characterisitics = attrList;
                                //AttriMdl1.exCharacterisitics = attrList;
                                _AssetattriRepository.Add(AttriMdl1);
                                //}


                                cunt++;
                                item.Add(drw["EquipmentUniqueID"].ToString());
                                // }

                            }
                            else
                                return cunt + " Items uploaded, Row:" + lineId + ", " + drw["EquipmentUniqueID"].ToString() + " not exist in the DB";
                        }
                        else
                        {
                            return cunt + " Items uploaded, Row:" + lineId + ", Equipment should not be blanck";
                        }
                        lineId++;
                    }
                }
            }


            return "Success " + cunt.ToString();


        }
        public virtual string VendorBulk_Upload(HttpPostedFileBase file)
        {
            int cunt = 0, lineId = 0;
            var item = new List<string>();
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

            string[] columns = { "EQUIPMENTUNIQUEID", "NOUN", "MODIFIER", "ATTRIBUTE", "VALUE", "UOM", "SEQUENCE" };

            //var assetAttr = _AssetattriRepository.FindAll().ToList();
            //foreach (var attr in assetAttr)
            //{
            //    attr.exCharacterisitics = null;
            //    _AssetattriRepository.Add(attr);
            //    cunt++;
            //}


            if (dt1.Rows.Count > 0)
            {
                var allUsers = _UsercreateRepository.FindAll().ToList();
                try
                {
                    foreach (DataRow drw1 in dt1.Rows)
                    {
                        var query1 = Query.EQ("UniqueId", drw1[0].ToString());
                        var mdl = _assetRepository.FindOne(query1);
                        if (mdl != null)
                        {
                            mdl.Manufacturer = drw1[1].ToString();
                            mdl.ModelNo = drw1[2].ToString();
                            mdl.SerialNo = drw1[3].ToString();
                            mdl.PartNo = drw1[4].ToString();
                            mdl.MfrYear = drw1[5].ToString();
                            mdl.MfrCountry = drw1[6].ToString();
                            //mdl.RegNo = drw1[5].ToString();
                            //mdl.Soureurl = drw1[5].ToString();
                            //mdl.Rework_Remarks = drw1[6].ToString();
                            //var catuser = drw1[7].ToString();
                            //if (catuser != "" && catuser != null)
                            //{
                            //    var catMdl = new Prosol_UpdatedBy();
                            //    var catuserDet = allUsers.Where(u => u.UserName == catuser).ToList();
                            //    catMdl.UserId = catuserDet[0].Userid;
                            //    catMdl.Name = catuserDet[0].UserName;
                            //    catMdl.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                            //    mdl.Catalogue = catMdl;
                            //}
                            _assetRepository.Add(mdl);
                            cunt++;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }


            return "Success " + cunt.ToString();


        }
        public CommonMaster getAllCommonMaster()
        {
            var ListMaster = new CommonMaster();
            //   var Flds = Fields.Exclude("IsActive");
            var Qry = Query.EQ("IsActive", true);

            var ListBusiness = _BusiMasterrep.FindAll(Qry).ToList();
            if (ListBusiness.Count > 0)
                ListMaster.Businesses = ListBusiness;

            //  var QryMajor = Query.EQ("IsActive", "Yes");
            var ListMajorcls = _Majorep.FindAll(Qry).ToList();
            if (ListMajorcls.Count > 0)
                ListMaster.MajorClasses = ListMajorcls;

            //Function/Minor Class
            //  var QryMinor = Query.EQ("IsActive", "Yes");
            var ListMinorcls = _Minorep.FindAll(Qry).ToList();
            if (ListMinorcls.Count > 0)
                ListMaster.MinorClasses = ListMinorcls;
            //Sub-system/Identifier
            // var QryIden = Query.EQ("IsActive", "Yes");

            var ListIden = _Identifierep.FindAll(Qry).ToList();
            if (ListIden.Count > 0)
                ListMaster.Identifiers = ListIden;

            //Region
            // var QryRegion = Query.EQ("IsActive", "Yes");
            var Listren = _Regionrep.FindAll(Qry).ToList();
            if (Listren.Count > 0)
                ListMaster.Regions = Listren;


            //City
            // var QryCity = Query.EQ("IsActive", "Yes");
            var ListCity = _Cityrep.FindAll(Qry).ToList();
            if (ListCity.Count > 0)
                ListMaster.Cities = ListCity;

            //Area
            // var Qryarea = Query.EQ("IsActive", "Yes");
            var ListArea = _Arearep.FindAll(Qry).ToList();
            if (ListArea.Count > 0)
                ListMaster.Areas = ListArea;

            //SubArea
            //  var QrySubarea = Query.EQ("IsActive", "Yes");
            var ListSubArea = _SubArearep.FindAll(Qry).ToList();
            if (ListSubArea.Count > 0)
                ListMaster.SubAreas = ListSubArea;


            //Location
            // var QryRegion = Query.EQ("IsActive", "Yes");
            var ListLoc = _Locrep.FindAll(Qry).ToList();
            if (ListLoc.Count > 0)
                ListMaster.Locations = ListLoc;

            //Equipment Class
            // var QryRegion = Query.EQ("IsActive", "Yes");
            var ListEquCls = _EquipClassrep.FindAll(Qry).ToList();
            if (ListEquCls.Count > 0)
                ListMaster.EquipmentClasses = ListEquCls;

            //Equipment Type
            // var QryRegion = Query.EQ("IsActive", "Yes");
            var ListEquType = _EquipTyperep.FindAll(Qry).ToList();
            if (ListEquType.Count > 0)
                ListMaster.EquipmentTypes = ListEquType;

            return ListMaster;



        }
        //Asset Master
        public CommonMaster getAllCommonMasterforTools()
        {
            var ListMaster = new CommonMaster();


            var ListBusiness = _BusiMasterrep.FindAll().ToList();
            if (ListBusiness.Count > 0)
                ListMaster.Businesses = ListBusiness;

            //  var QryMajor = Query.EQ("IsActive", "Yes");
            var ListMajorcls = _Majorep.FindAll().ToList();
            if (ListMajorcls.Count > 0)
                ListMaster.MajorClasses = ListMajorcls;

            //Function/Minor Class
            //  var QryMinor = Query.EQ("IsActive", "Yes");
            var ListMinorcls = _Minorep.FindAll().ToList();
            if (ListMinorcls.Count > 0)
                ListMaster.MinorClasses = ListMinorcls;
            //Sub-system/Identifier
            // var QryIden = Query.EQ("IsActive", "Yes");

            var ListIden = _Identifierep.FindAll().ToList();
            if (ListIden.Count > 0)
                ListMaster.Identifiers = ListIden;

            //Region
            // var QryRegion = Query.EQ("IsActive", "Yes");
            var Listren = _Regionrep.FindAll().ToList();
            if (Listren.Count > 0)
                ListMaster.Regions = Listren;


            //City
            // var QryCity = Query.EQ("IsActive", "Yes");
            var ListCity = _Cityrep.FindAll().ToList();
            if (ListCity.Count > 0)
                ListMaster.Cities = ListCity;

            //Area
            // var Qryarea = Query.EQ("IsActive", "Yes");
            var ListArea = _Arearep.FindAll().ToList();
            if (ListArea.Count > 0)
                ListMaster.Areas = ListArea;

            //SubArea
            //  var QrySubarea = Query.EQ("IsActive", "Yes");
            var ListSubArea = _SubArearep.FindAll().ToList();
            if (ListSubArea.Count > 0)
                ListMaster.SubAreas = ListSubArea;


            //Location
            // var QryRegion = Query.EQ("IsActive", "Yes");
            var ListLoc = _Locrep.FindAll().ToList();
            if (ListLoc.Count > 0)
                ListMaster.Locations = ListLoc;

            //Equipment Class
            // var QryRegion = Query.EQ("IsActive", "Yes");
            var ListEquCls = _EquipClassrep.FindAll().ToList();
            if (ListEquCls.Count > 0)
                ListMaster.EquipmentClasses = ListEquCls;

            //Equipment Type
            // var QryRegion = Query.EQ("IsActive", "Yes");
            var ListEquType = _EquipTyperep.FindAll().ToList();
            if (ListEquType.Count > 0)
                ListMaster.EquipmentTypes = ListEquType;

            return ListMaster;



        }
        //region
        public IEnumerable<Prosol_City> getAllCities(string City)
        {
            var qry = Query.And(Query.EQ("", City), Query.EQ("IsActive", true));
            var res = _Cityrep.FindAll(qry).ToList();
            return res;
        }
        public bool InsertDataRegn(Prosol_Region data)
        {
            bool res = false;
            data.IsActive = true;
            var query = Query.Or(Query.EQ("RegionCode", data.RegionCode), Query.EQ("Region", data.Region));
            var vn = _Regionrep.FindAll(query).ToList();
            if (vn.Count == 0 || (vn.Count == 1 && vn[0]._id == data._id))
            {
                res = _Regionrep.Add(data);
            }
            return res;
        }
        public bool InsertDataFL(Prosol_Funloc data)
        {
            bool res = false;
            data.Islive = true;
            var query = Query.And(Query.EQ("FunctLocation", data.FunctLocation), Query.EQ("SectionNo", data.SectionNo));
            var vn = _FuncLocRepository.FindAll(query).ToList();
            if (vn.Count == 0)
            {
                var qry = Query.And(Query.EQ("Sequence", data.Sequence), Query.EQ("SectionNo", data.SectionNo));
                var seq = _FuncLocRepository.FindOne(qry);
                if(seq != null)
                {
                    int sameSeq = Convert.ToInt32(seq.Sequence);
                    var qryy = Query.And(Query.GTE("Sequence", data.Sequence), Query.EQ("SectionNo", data.SectionNo));
                    var seqLst = _FuncLocRepository.FindAll(qryy).ToList();
                    foreach (var sq in seqLst)
                    {
                        sq.Sequence = (sameSeq+1).ToString();
                        _FuncLocRepository.Add(sq);
                        sameSeq++;
                    }
                }
                res = _FuncLocRepository.Add(data);
            }
            return res;
        }
        public bool UpdateDataFL(Prosol_Funloc data)
        {
            bool res = false;
            data.Islive = true;
            var query = Query.And(Query.EQ("FunctLocation", data.FunctLocation), Query.EQ("SectionNo", data.SectionNo));
            var vn = _FuncLocRepository.FindAll(query).ToList();
            if (vn.Count != 0)
            {
                var qry = Query.And(Query.EQ("Sequence", data.Sequence), Query.EQ("SectionNo", data.SectionNo));
                var seq = _FuncLocRepository.FindOne(qry);
                int sameSeq = Convert.ToInt32(seq.Sequence);
                vn[0].Level1 = data.Level1;
                vn[0].Level2 = data.Level2;
                vn[0].Level3 = data.Level3;
                vn[0].Level4 = data.Level4;
                vn[0].Level5 = data.Level5;
                vn[0].Level6 = data.Level6;
                vn[0].Level7 = data.Level7;
                vn[0].Equipment = data.Equipment;
                vn[0].PrimaryEquipment = data.PrimaryEquipment;
                vn[0].SubEquipment1 = data.SubEquipment1;
                vn[0].SubEquipment2 = data.SubEquipment2;
                vn[0].SubEquipment3 = data.SubEquipment3;
                vn[0].FunctLocation = data.FunctLocation;
                vn[0].SuperiorLocation = data.SuperiorLocation;
                vn[0].SectionNo = data.SectionNo;
                vn[0].Sequence = data.Sequence;
                vn[0].UniqueId = data.UniqueId;
                vn[0].Islive = data.Islive;
                res = _FuncLocRepository.Add(vn[0]);
                if (seq != null)
                {
                    var qryy = Query.And(Query.GTE("Sequence", data.Sequence), Query.NE("FunctLocation", data.FunctLocation), Query.EQ("SectionNo", data.SectionNo));
                    var seqLst = _FuncLocRepository.FindAll(qryy).ToList();
                    foreach (var sq in seqLst)
                    {
                        sq.Sequence = (sameSeq+1).ToString();
                        _FuncLocRepository.Add(sq);
                        sameSeq++;
                    }
                }
            }
            else
            {
                var fQry = Query.EQ("FunctLocation", data.FunctLocation);
                var fLst = _FuncLocRepository.FindAll(fQry).ToList();
                if (fLst.Count != 0)
                {
                    fLst[0].Level1 = data.Level1;
                    fLst[0].Level2 = data.Level2;
                    fLst[0].Level3 = data.Level3;
                    fLst[0].Level4 = data.Level4;
                    fLst[0].Level5 = data.Level5;
                    fLst[0].Level6 = data.Level6;
                    fLst[0].Level7 = data.Level7;
                    fLst[0].Equipment = data.Equipment;
                    fLst[0].PrimaryEquipment = data.PrimaryEquipment;
                    fLst[0].SubEquipment1 = data.SubEquipment1;
                    fLst[0].SubEquipment2 = data.SubEquipment2;
                    fLst[0].SubEquipment3 = data.SubEquipment3;
                    fLst[0].FunctLocation = data.FunctLocation;
                    fLst[0].SuperiorLocation = data.SuperiorLocation;
                    fLst[0].SectionNo = data.SectionNo;
                    fLst[0].Sequence = data.Sequence;
                    fLst[0].UniqueId = data.UniqueId;
                    fLst[0].Islive = data.Islive;
                    res = _FuncLocRepository.Add(fLst[0]);
                }
            }
            return res;
        }
        public bool DelDatareg(string id)
        {
            var query = Query.EQ("RegionCode", id);
            var res = _Regionrep.Delete(query);
            return res;
        }
        public bool DisableReg(string id, bool Islive)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("IsActive", Islive);
            var flg = UpdateFlags.Upsert;
            var res = _Regionrep.Update(query, Updae, flg);
            return res;

        }

        public bool InsertDataCity(Prosol_City data)
        {
            bool res = false;
            data.IsActive = true;
            var query = Query.And(Query.EQ("Region_Id", data.Region_Id), Query.And(Query.EQ("CityCode", data.CityCode), Query.EQ("City", data.City)));
            var vn = _Cityrep.FindAll(query).ToList();
            if (vn.Count == 0 || (vn.Count == 1 && vn[0]._id == data._id))
            {
                res = _Cityrep.Add(data);
            }
            return res;
        }
        public bool DisableCity(string id, bool Islive)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("IsActive", Islive);
            var flg = UpdateFlags.Upsert;
            var res = _Cityrep.Update(query, Updae, flg);
            return res;

        }
        public bool InsertDataAre(Prosol_Area data)
        {
            bool res = false;
            data.IsActive = true;
            var query = Query.And(Query.EQ("City_Id", data.City_Id), Query.Or(Query.EQ("AreaCode", data.AreaCode), Query.EQ("Area", data.Area)));
            var vn = _Arearep.FindAll(query).ToList();
            if (vn.Count == 0 || (vn.Count == 1 && vn[0]._id == data._id))
            {
                res = _Arearep.Add(data);
            }
            return res;
        }
        public bool DisableArea(string id, bool Islive)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("IsActive", Islive);
            var flg = UpdateFlags.Upsert;
            var res = _Arearep.Update(query, Updae, flg);
            return res;

        }
        public bool InsertDataSubAre(Prosol_SubArea data)
        {
            bool res = false;
            data.IsActive = true;
            var query = Query.And(Query.EQ("Area_Id", data.Area_Id), Query.Or(Query.EQ("SubAreaCode", data.SubAreaCode), Query.EQ("SubArea", data.SubArea)));
            var vn = _SubArearep.FindAll(query).ToList();
            if (vn.Count == 0 || (vn.Count == 1 && vn[0]._id == data._id))
            {
                res = _SubArearep.Add(data);
            }
            return res;
        }
        public bool DisableSubArea(string id, bool Islive)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("IsActive", Islive);
            var flg = UpdateFlags.Upsert;
            var res = _SubArearep.Update(query, Updae, flg);
            return res;

        }
        public bool InsertDataLoc(Prosol_Location data)
        {
            bool res = false;
            data.IsActive = true;
            var query = Query.And(Query.EQ("Area_Id", data.Area_Id), Query.Or(Query.EQ("LocationCode", data.LocationCode), Query.EQ("Location", data.Location)));
            var vn = _Locrep.FindAll(query).ToList();
            if (vn.Count == 0 || (vn.Count == 1 && vn[0]._id == data._id))
            {
                res = _Locrep.Add(data);
            }
            return res;
        }
        public bool DisableLoc(string id, bool Islive)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("IsActive", Islive);
            var flg = UpdateFlags.Upsert;
            var res = _Locrep.Update(query, Updae, flg);
            return res;

        }
        public bool InsertDataEquipClass(Prosol_EquipmentClass data)
        {
            bool res = false;
            data.IsActive = true;
            var query = Query.Or(Query.EQ("EquipmentCode", data.EquipmentCode), Query.EQ("EquipmentClass", data.EquipmentClass));
            var vn = _EquipClassrep.FindAll(query).ToList();
            if (vn.Count == 0 || (vn.Count == 1 && vn[0]._id == data._id))
            {
                res = _EquipClassrep.Add(data);
            }
            return res;
        }
        public bool DisableEquipClass(string id, bool Islive)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("IsActive", Islive);
            var flg = UpdateFlags.Upsert;
            var res = _EquipClassrep.Update(query, Updae, flg);
            return res;

        }
        public bool InsertDataEquipType(Prosol_EquipmentType data)
        {
            bool res = false;
            data.IsActive = true;
            var query = Query.And(Query.EQ("EquClass_Id", data.EquClass_Id), Query.Or(Query.EQ("EquTypeCode", data.EquTypeCode), Query.EQ("EquipmentType", data.EquipmentType)));
            var vn = _EquipTyperep.FindAll(query).ToList();
            if (vn.Count == 0)
            {
                res = _EquipTyperep.Add(data);
            }
            return res;
        }
        public bool DisableEquipType(string id, bool Islive)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("IsActive", Islive);
            var flg = UpdateFlags.Upsert;
            var res = _EquipTyperep.Update(query, Updae, flg);
            return res;

        }
        public bool InsertDataBusiness(Prosol_FARMaster data)
        {
            bool res = false;
            data.Islive = true;
            var query = Query.EQ("Code", data.Code);
            var vn = _FARMaster.FindAll(query).ToList();
            if (data.Label == "Section")
            {
                res = UpdateDataBusiness(data);
            }
            else {
                if (vn.Count == 0 || (vn.Count == 1 /*&& vn[0]._id == data._id*/))
                {
                    res = _FARMaster.Add(data);
                }
            }
            return res;
        }
        public bool UpdateDataBusiness(Prosol_FARMaster data)
        {
            data.Islive = true;
            bool res = false;

            // Case 1: Find by Label + Title
            var existingByTitle = _FARMaster.FindOne(Query.And(
                Query.EQ("Label", data.Label),
                Query.EQ("Title", data.Title)
            ));

            if (existingByTitle != null)
            {
                // Update its Code
                string shuffleCode = existingByTitle.Code;
                existingByTitle.Code = data.Code;
                existingByTitle.Islive = true;
                _FARMaster.Add(existingByTitle);
                var withSameCode = _FARMaster.FindOne(Query.And(
                    Query.EQ("Label", data.Label),
                    Query.EQ("Code", data.Code)
                ));
                withSameCode.Code = shuffleCode;
                withSameCode.Islive = true;
                _FARMaster.Add(withSameCode);
                _FARMaster.Add(existingByTitle);
                res = true;

                // Shift others
                ShiftCodes(data.Label, data.Title, data.Code);
            }
            else
            {
                // Case 2: Insert new
                _FARMaster.Add(data);
                res = true;

                // If Code already existed, shift others
                var existingByCode = _FARMaster.FindOne(Query.And(
                    Query.EQ("Label", data.Label),
                    Query.EQ("Code", data.Code)
                ));

                if (existingByCode != null)
                {
                    ShiftCodes(data.Label, data.Title, data.Code);
                }
            }

            return res;
        }

        private void ShiftCodes(string label, string title, string code)
        {
            var query = Query.And(
                Query.EQ("Label", label),
                Query.GTE("Code", code),
                Query.NE("Title", title)
            );

            int i = Convert.ToInt32(code);
            foreach (var v in _FARMaster.FindAll(query))
            {
                v.Code = (++i).ToString();
                v.Islive = true;
                _FARMaster.Add(v);
            }
        }


        public bool InsertDataFar(Prosol_FARRepository data)
        {
            bool res = false;
            data.Islive = true;
            var query = Query.EQ("FARId", data.FARId);
            var newData = _FARMasterrep.FindOne(query);
            if (newData != null)
            {
                //var newData = new Prosol_FARRepository();
                newData.Region = data.Region;
                newData.AssetDesc = data.AssetDesc;
                newData.Islive = data.Islive;
                //var Updae = Update.Set("FARId", data.FARId).Set("Region", data.Region).Set("AssetDesc", data.AssetDesc).Set("Islive", data.Islive);
                //var flg = UpdateFlags.Upsert;
                res = _FARMasterrep.Add(newData);
            }
            else
            {
                res = _FARMasterrep.Add(data);
            }
            return res;
        }
        public bool InsertDataSite(Prosol_SiteMaster data)
        {
            bool res = false;
            data.Islive = true;
            var query = Query.EQ("SiteId", data.SiteId);
            var newData = _SiteMaster.FindOne(query);
            if (newData != null)
            {
                newData.Cluster = data.Cluster;
                newData.HighLevelLocation = data.HighLevelLocation;
                newData.Islive = data.Islive;
                res = _SiteMaster.Add(newData);
            }
            else
            {
                res = _SiteMaster.Add(data);
            }
            return res;
        }
        public bool InsertDataLoc1(Prosol_Location data)
        {
            bool res = false;
            data.Islive = true;
            var query = Query.EQ("Location", data.Location);
            var newData = _Locrep.FindOne(query);
            if (newData != null)
            {
                //newData.Location = data.Location;
                newData.LocationHierarchy = data.LocationHierarchy;
                newData.Islive = data.Islive;
                res = _Locrep.Add(newData);
            }
            else
            {
                res = _Locrep.Add(data);
            }
            return res;
        }
        public bool InsertDataAT(Prosol_AssetTypeMaster data)
        {
            bool res = false;
            data.Islive = true;
            var query = Query.EQ("AssetType", data.AssetType);
            var newData = _AssetTypeMaster.FindOne(query);
            if (newData != null)
            {
                newData.ClassificationHierarchyDesc = data.ClassificationHierarchyDesc;
                newData.FailureCode = data.FailureCode;
                newData.Islive = data.Islive;
                res = _AssetTypeMaster.Add(newData);
            }
            else
            {
                res = _AssetTypeMaster.Add(data);
            }
            return res;
        }
        public bool RemoveMfr(string id, bool sts, string flg)
        {
            var query = Query.And(Query.EQ("Label" , flg), Query.EQ("Code", id));
            var res = _FARMaster.Delete(query);
            return res;

        }
        public bool DisableFunLoc(string section, string id, bool sts)
        {
            var res = false;
            var query = Query.And(Query.EQ("SectionNo", section),Query.EQ("FunctLocation", id));
            var data = _FuncLocRepository.FindOne(query);
            data.Islive = sts;
            res = _FuncLocRepository.Add(data);
            return res;

        }
        public bool DisableBus(string id, bool sts)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("IsActive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _BusiMasterrep.Update(query, Updae, flg);
            return res;

        }
        public bool DisableNotes(string id, bool sts)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("Islive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _MasterRepository.Update(query, Updae, flg);
            return res;

        }

        public bool InsertData(Prosol_MajorClass data)
        {
            bool res = false;
            data.IsActive = true;
            var query = Query.And(Query.EQ("Business_id", data.Business_id), Query.Or(Query.EQ("MajorCode", data.MajorCode), Query.EQ("MajorClass", data.MajorClass)));
            var vn = _Majorep.FindAll(query).ToList();
            if (vn.Count == 0 || (vn.Count == 1 && vn[0]._id == data._id))
            {
                res = _Majorep.Add(data);
            }
            return res;

        }
        public bool Disablemjr(string id, bool sts)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("IsActive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _Majorep.Update(query, Updae, flg);
            return res;

        }

        public bool InsertData1(Prosol_MinorClass data)
        {
            bool res = false;
            data.IsActive = true;
            var query = Query.And(Query.EQ("Major_id", data.Major_id), Query.Or(Query.EQ("MinorCode", data.MinorCode), Query.EQ("MinorClass", data.MinorClass)));
            var vn = _Minorep.FindAll(query).ToList();
            if (vn.Count == 0 || (vn.Count == 1 && vn[0]._id == data._id))
            {
                res = _Minorep.Add(data);
            }
            return res;

        }
        public bool Disablemnr(string id, bool sts)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("IsActive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _Minorep.Update(query, Updae, flg);
            return res;

        }
        public bool InsertDataIdent(Prosol_Identifier data)
        {
            bool res = false;
            data.IsActive = true;
            var query = Query.And(Query.EQ("fClass_Id", data.fClass_Id), Query.Or(Query.EQ("IdentifierCode", data.IdentifierCode), Query.EQ("Identifier", data.Identifier)));
            var vn = _Identifierep.FindAll(query).ToList();
            if (vn.Count == 0)
            {
                res = _Identifierep.Add(data);
            }
            return res;

        }
        public bool Disableidnt(string id, bool sts)
        {
            var query = Query.EQ("_id", new ObjectId(id));
            var Updae = Update.Set("IsActive", sts);
            var flg = UpdateFlags.Upsert;
            var res = _Identifierep.Update(query, Updae, flg);
            return res;

        }
        public virtual string IdentifierBulk_Upload(HttpPostedFileBase file)
        {
            int cunt = 0, lineId = 1;

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


                var objlist = new Prosol_Identifier();

                var BusID = getbusinessname(drw[0].ToString());
                var majorID = Majorid(drw[1].ToString(), BusID._id.ToString());
                if (majorID != null)
                {


                    var funcId = getMinorid(drw[2].ToString());
                    if (funcId != null)
                    {
                        var checkexist = Query.And(Query.EQ("fClass_Id", funcId._id.ToString()), (Query.Or(Query.EQ("IdentifierCode", drw[1].ToString()), (Query.EQ("Identifier", drw[2].ToString())))));
                        var exist = _Identifierep.FindAll(checkexist).ToList();
                        if (exist.Count == 0)
                        {
                            objlist.Business_Id = BusID._id.ToString();
                            objlist.MajorClass_Id = majorID._id.ToString();
                            objlist.fClass_Id = funcId._id.ToString();


                            objlist.IdentifierCode = drw[3].ToString();
                            objlist.Identifier = drw[4].ToString();
                            objlist.IsActive = true;
                            _Identifierep.Add(objlist);
                            cunt++;

                        }

                        else return cunt + " Items uploaded, Row:" + lineId + ", " + drw[3].ToString() + " already exist in the sub system/identifier master";
                    }
                    else return cunt + " Items uploaded, Row:" + lineId + ", " + drw[2].ToString() + " not exist in the function/minor class master";
                }
                else return cunt + " Items uploaded, Row:" + lineId + ", " + drw[1].ToString() + " not exist in the Business master";
                lineId++;
            }

            return "Success " + cunt.ToString();


        }
        //search ItemCode
        public List<Prosol_AssetMaster> searchItemCode(string sCode, string sSource, string sUser, string sStatus, string sQR)
        {
            string[] strArr = { "UniqueId", "PVuser", "SiteId", "AssetNo", "TechIdentNo", "FixedAssetNo", "Location", "LocationHierarchy", "EquipmentDesc", "ItemStatus", "Catalogue", "Review", "Release", "Rework", "Description", "AdditionalNotes" };
            List<searchObj> LstObj = new List<searchObj>();
            searchObj sObj = new searchObj();
            sObj.SearchColumn = "UniqueId";
            sObj.SearchKey = sCode;
            LstObj.Add(sObj);
            sObj = new searchObj();
            sObj.SearchColumn = "AssetNo";
            sObj.SearchKey = sSource;
            LstObj.Add(sObj);

            sObj = new searchObj();
            sObj.SearchColumn = "sUser";
            sObj.SearchKey = sUser;
            LstObj.Add(sObj);


            LstObj.Add(sObj);
            sObj = new searchObj();
            sObj.SearchColumn = "sStatus";
            sObj.SearchKey = sStatus;
            LstObj.Add(sObj);

            LstObj.Add(sObj);
            sObj = new searchObj();
            sObj.SearchColumn = "TechIdentNo";
            sObj.SearchKey = sQR;
            LstObj.Add(sObj);

            var newResultList = new List<Prosol_AssetMaster>();
            //  List<Prosol_Datamaster> RemoveResultList = new List<Prosol_Datamaster>();
            int flg = 0;
            foreach (searchObj mdl in LstObj)
            {
                if (mdl.SearchKey != "" && mdl.SearchKey != null && mdl.SearchKey != "null" && mdl.SearchKey != "undefined")
                {
                    if (flg == 0)
                    {
                        List<Prosol_AssetMaster> sResult = new List<Prosol_AssetMaster>();
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

                            foreach (Prosol_AssetMaster pmdl in sResult)
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
                                        var query1 = new List<Prosol_AssetMaster>();
                                        if (mdl.SearchColumn == "UniqueId")
                                        {
                                            if (flgg == 0)
                                            {
                                                //query1 = (from x in newResultList where x.UniqueId.Contains(str) select x).ToList();
                                            }
                                            if (flgg == 1)
                                            {
                                                //query1 = query1.Where(x => x.UniqueId.Contains(str)).ToList();

                                            }
                                        }
                                        if (mdl.SearchColumn == "EquipmentDesc")
                                        {
                                            if (flgg == 0)
                                            {
                                                //query1 = (from x in newResultList where x.EquipmentDesc.Contains(str) select x).ToList();
                                            }
                                            if (flgg == 1)
                                            {
                                                //query1 = query1.Where(x => x.EquipmentDesc.Contains(str)).ToList();

                                            }

                                            // newResultList = (from x in newResultList where x.Legacy.Contains(str) select x).ToList();
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

                                        //if (mdl.SearchColumn == "UniqueId")
                                        //newResultList = (from x in newResultList where x.UniqueId.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();
                                        //if (mdl.SearchColumn == "EquipmentDesc")
                                        //newResultList = (from x in newResultList where x.EquipmentDesc.StartsWith(mdl.SearchKey.TrimEnd('*')) select x).ToList();

                                    }
                                    else
                                    {
                                        //End with

                                        //if (mdl.SearchColumn == "UniqueId")
                                        //    newResultList = (from x in newResultList where x.UniqueId.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();
                                        //if (mdl.SearchColumn == "EquipmentDesc")
                                        //    newResultList = (from x in newResultList where x.EquipmentDesc.EndsWith(mdl.SearchKey.TrimStart('*')) select x).ToList();


                                    }
                                }

                            }

                        }
                        else
                        {
                            if (mdl.SearchKey != "" && mdl.SearchKey != null && mdl.SearchKey != "null" && mdl.SearchKey != "undefined")
                            {
                                if (mdl.SearchColumn == "UniqueId")
                                    //newResultList = (from x in newResultList where x.UniqueId != null && x.UniqueId.Equals(mdl.SearchKey) select x).ToList();
                                    if (mdl.SearchColumn == "EquipmentDesc")
                                        //newResultList = (from x in newResultList where x.EquipmentDesc != null && x.EquipmentDesc.Equals(mdl.SearchKey) select x).ToList();

                                        if (mdl.SearchColumn == "sStatus")
                                        {
                                            int sta = 0, sta1 = 1;
                                            if (sStatus == "Catalogue")
                                            {
                                                sta = 2; sta1 = 3;
                                            }

                                            else if (sStatus == "QC")
                                            {
                                                sta = 4; sta1 = 5;
                                            }
                                            else if (sStatus == "QA")
                                            {
                                                sta = 6; sta1 = 7;
                                            }
                                            else
                                            {
                                                sta = 8; sta1 = 8;
                                            }
                                            if (sStatus == "Catalogue Rework")
                                            {
                                                newResultList = (from x in newResultList where (x.Rework != null && (x.ItemStatus == 2 || x.ItemStatus == 3)) select x).ToList();
                                            }
                                            //else if (sStatus == "QC Rework")
                                            //{
                                            //    newResultList = (from x in newResultList where (x.Rework != null && (x.ItemStatus == 2 || x.ItemStatus == 3)) select x).ToList();
                                            //}
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


        private List<Prosol_AssetMaster> UserSearch(string[] strArr, string sUser)
        {
            var fields = Fields.Include(strArr);
            var Qry1 = Query.Or(Query.EQ("Pvuser.Name", sUser), Query.EQ("Catalogue.Name", sUser), Query.EQ("Review.Name", sUser), Query.EQ("Release.Name", sUser));
            var arrResult = _assetRepository.FindAll(fields, Qry1).ToList();
            return arrResult;
        }
        private List<Prosol_AssetMaster> StatusSearch(string[] strArr, string sStatus)
        {
            int sta = 0, sta1 = 1;
            if (sStatus == "Catalogue")
            {
                sta = 2; sta1 = 3;
            }
            else if (sStatus == "QC")
            {
                sta = 4; sta1 = 5;
            }
            else if (sStatus == "QA")
            {
                sta = 6; sta1 = 7;
            }
            else
            {
                sta = 8; sta1 = 8;
            }
            var fields = Fields.Include(strArr);
            var Qry1 = Query.Or(Query.EQ("ItemStatus", sta), Query.EQ("ItemStatus", sta1));
            if (sStatus == "Catalogue Rework")
            {
                Qry1 = Query.And(Query.NE("Rework", BsonNull.Value), Query.EQ("ItemStatus", 2));
            }
            //else if (sStatus == "QC Rework")
            //{
            //    Qry1 = Query.And(Query.NE("Rework", BsonNull.Value), Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)));
            //}

            var arrResult = _assetRepository.FindAll(fields, Qry1).ToList();
            return arrResult;
        }

        //private List<Prosol_AssetMaster> SearchFn(string[] strArr, string ColumnName, string sCode)
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
        //        var query = Query.Or(QryLst);


        //        var arrResult = _assetRepository.FindAll(fields, query).ToList();
        //        return arrResult;
        //    }
        //    else
        //    {
        //        if (sCode.Contains("/"))
        //        {
        //            string[] splt = sCode.Split('/');
        //            sCode = splt[1] + "/" + splt[0] + "/" + splt[2];
        //            var date = DateTime.Parse(sCode, new CultureInfo("en-US", true));
        //            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        //            var date1 = date.AddDays(1);
        //            var Qry1 = Query.And(Query.GTE("CreatedOn", BsonDateTime.Create(date)), Query.LT("CreatedOn", BsonDateTime.Create(date1)));
        //            var arrResult = _assetRepository.FindAll(fields, Qry1).ToList();
        //            return arrResult;
        //        }
        //        else
        //        {
        //            var Qry1 = Query.EQ(ColumnName, sCode.TrimStart().TrimEnd());
        //            var arrResult = _assetRepository.FindAll(fields, Qry1).ToList();
        //            return arrResult;
        //        }

        //    }

        //}

        private List<Prosol_AssetMaster> SearchFn(string[] strArr, string ColumnName, string sCode)
        {
            var fields = Fields.Include(strArr);

            if (sCode.Contains('*'))
            {
                var QryLst = new List<IMongoQuery>();
                string[] sepArr = sCode.Split('*');

                // Adjusted logic to handle multiple '*' wildcards
                foreach (string str in sepArr)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        var Qry1 = Query.Matches(ColumnName, BsonRegularExpression.Create(new Regex(Regex.Escape(str.Trim()), RegexOptions.IgnoreCase)));
                        QryLst.Add(Qry1);
                    }
                }

                var query = Query.And(QryLst); // Change to Query.And to ensure all parts match
                var arrResult = _assetRepository.FindAll(fields, query).ToList();
                return arrResult;
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
                    var Qry1 = Query.And(Query.GTE("CreatedOn", BsonDateTime.Create(date)), Query.LT("CreatedOn", BsonDateTime.Create(date1)));
                    var arrResult = _assetRepository.FindAll(fields, Qry1).ToList();
                    return arrResult;
                }
                else
                {
                    var Qry1 = Query.EQ(ColumnName, sCode.Trim());
                    var arrResult = _assetRepository.FindAll(fields, Qry1).ToList();
                    return arrResult;
                }
            }
        }

        public class searchObj
        {
            public string SearchKey { set; get; }
            public string SearchColumn { set; get; }
        }
        public IEnumerable<Prosol_Users> getuseronly(string username)
        {


            var query = Query.And(Query.EQ("Roles.Name", username), Query.EQ("Islive", "Active"));
            var mxplnt = _UsercreateRepository.FindAll(query).ToList();
            return mxplnt;
        }


        public IEnumerable<Prosol_AssetMaster> PV_Reassign(string username, string Role)
        {
            IMongoQuery query;
            //string[] search_field = { "UniqueId","AssetNo", "UserName", "Region", "Area", "SubArea", "EquipmentClass", "EquipmentType", "SAP_Equipment", "OldTagNo", "Equipment", "City", "Business", "MajorClass", "EquipmentDesc" };
            //var fields = Fields.Include(search_field).Exclude("_id");
            if (Role == "Cataloguer")
                query = Query.And(Query.EQ("ItemStatus", 2), Query.EQ("Catalogue.Name", username));
            else if (Role == "Reviewer")
                query = Query.And(Query.EQ("ItemStatus", 4), Query.EQ("Review.Name", username));
            else
                query = Query.And(Query.EQ("ItemStatus", 1), Query.EQ("PVuser.Name", username));

            var getdata = _assetRepository.FindAll(query).ToList();
            //var getdata = _assetRepository.FindAll(fields, query).ToList();
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
        public bool PVreassign_submit(string item, string role, Prosol_AssetMaster reasgn)
        {
            var query = Query.EQ("UniqueId", item);
            var vn = _assetRepository.FindAll(query).ToList();
            if (vn != null)
            {
                if (role == "Cataloguer")
                {
                    if (vn.Count == 1)
                    {
                        vn[0].Catalogue = reasgn.Catalogue;
                        var res = _assetRepository.Add(vn[0]);
                    }
                }
                if (role == "PV User")
                {
                    if (vn.Count == 1)
                    {
                        vn[0].PVuser = reasgn.PVuser;
                        var res = _assetRepository.Add(vn[0]);
                    }

                }
                else
                {
                    if (vn.Count == 1)
                    {
                        vn[0].PVuser = reasgn.PVuser;
                        var res = _assetRepository.Add(vn[0]);
                    }

                }
            }
            return true;
        }
        public Prosol_AssetBOM getBomInfo(string Bomid)
        {
            var Qry = Query.EQ("UniqueId", Bomid);
            var BomObj = _AssetBOMRepository.FindOne(Qry);
            return BomObj;
        }

        public IEnumerable<Prosol_AssetMaster> getAssetDataByFun(string Busi, string Major, string FunStr)
        {
            //  string busi_id = getBusinessIdbyName(Busi);
            var sort = SortBy.Descending("ItemStatus");
            string Major_id = getMajorIdbyName(Busi, Major);
            var Manor = getMinorid(FunStr, Major_id);
            var Qry = Query.And(Query.NE("ItemStatus", 8), Query.EQ("Business", Busi), Query.EQ("MajorClass", Major_id), Query.EQ("MinorClass", Manor._id.ToString()));
            var ItemList = _assetRepository.FindAll(Qry, sort);
            return ItemList;
        }
        public IEnumerable<Prosol_AssetMaster> getAssetDataByFun(string Busi, string FunStr)
        {

            var fList = new List<Prosol_AssetMaster>();
            var Qry = Query.EQ("", "");
            if (Busi == "ELECTRICITY")
            {
                string busi_id = getBusinessIdbyName("ELECTRICITY");
                string Major_id1 = getMajorIdbyName(busi_id, "TRANSMISSION");
                string Major_id2 = getMajorIdbyName(busi_id, "DISTRIBUTION");
                string Major_id3 = getMajorIdbyName(busi_id, "SUBSTATION");
                string Major_id4 = getMajorIdbyName(busi_id, "SCADA");
                Qry = Query.And(Query.EQ("Business", busi_id), Query.Or(Query.EQ("MajorClass", Major_id1), Query.EQ("MajorClass", Major_id2), Query.EQ("MajorClass", Major_id3), Query.EQ("MajorClass", Major_id4)));
            }
            if (Busi == "WATER TRANSMISSION AND DISTRIBUTION")
            {
                string busi_id = getBusinessIdbyName("WATER");
                string Major_id1 = getMajorIdbyName(busi_id, "TRANSMISSION");
                string Major_id2 = getMajorIdbyName(busi_id, "DISTRIBUTION");
                Qry = Query.And(Query.EQ("Business", busi_id), Query.Or(Query.EQ("MajorClass", Major_id1), Query.EQ("MajorClass", Major_id2)));
            }
            if (Busi == "WATER PRODUCTION")
            {
                string busi_id = getBusinessIdbyName("WATER");
                string Major_id1 = getMajorIdbyName(busi_id, "PRODUCTION");
                Qry = Query.And(Query.EQ("Business", busi_id), Query.EQ("MajorClass", Major_id1));
            }
            var ItemList = _assetRepository.FindAll(Qry).ToList();
            if (ItemList.Count > 0)
            {
                //var newList = ItemList.Select(p => p.MinorClass).Distinct().ToArray();
                //foreach (var funcls in newList)
                //{
                //    int relcunt = 0,clfcunt=0,WFA = 0;
                //    var reslst = getAssetDataByFun1(funcls).ToList();
                //    if (reslst.Count > 0)
                //    {

                //        foreach (var chk in reslst)
                //        {
                //            if (chk.ItemStatus == 8)
                //            {
                //                relcunt++;
                //            }
                //            if (chk.ItemStatus == 9)
                //            {
                //                clfcunt++;
                //            }
                //            if (chk.ItemStatus == 10)
                //            {
                //                WFA++;
                //            }
                //        }
                //        reslst[0].SerialNo = clfcunt.ToString();
                //        //reslst[0].Make = relcunt.ToString();
                //        //reslst[0].Quantity = reslst.Count.ToString();
                //        reslst[0].ModelNo=WFA.ToString();
                //        fList.Add(reslst[0]);


                //    }

                //}
            }
            return fList;
        }
        private IEnumerable<Prosol_AssetMaster> getAssetDataByFun1(string FunStr)
        {
            var Qry = Query.EQ("MinorClass", FunStr);
            var ItemList = _assetRepository.FindAll(Qry);
            return ItemList;
        }
        public string getBusinessIdbyName(string busiName)
        {
            var qry = Query.EQ("BusinessName", busiName);
            var busi = _BusiMasterrep.FindOne(qry);
            if (busi != null)
            {
                return busi._id.ToString();
            }
            else return "";
        }
        public string getMajorIdbyName(string busi_id, string Major)
        {
            var qry = Query.And(Query.EQ("Business_id", busi_id), Query.EQ("MajorClass", Major));
            var mjor = _Majorep.FindOne(qry);
            if (mjor != null)
            {
                return mjor._id.ToString();
            }
            else return "";
        }

        public string getlast_request_R_no()
        {
            int count = _assetRepository.FindAll().Count();
            string newCode = "ADE" + (count + 1).ToString("D6");
            return newCode;
        }
        public String[] GenerateShortLong(Prosol_AssetMaster cat)
        {

            String[] strArr = { "", "", "", "", "" /*, ""*/ };
            strArr[0] = ShortDesc(cat);
            strArr[1] = LongDesc(cat);
            cat.Equipment_Short = strArr[0];
            cat.Equipment_Long = strArr[1];

            strArr[2] = MissingValue(cat);
            strArr[3] = PapulatedValue(cat);
            //strArr[5] = MissingAttributeValue(cat);

            // cat.Longdesc = strArr[1];
            // strArr[4] = RepeatedValue(cat);

            return strArr;
        }
        private string MissingValue(Prosol_AssetMaster cat)
        {
            // string tmpstr = Regex.Replace(cat.Legacy.Trim(), @"[^\w\d]", " ");
            // string[] legarray = tmpstr.Split(' ');
            //string[] legarray = cat.Legacy.Trim().Split(' ',',',':',';');
            string Desc = cat.Description + "," + cat.Description_;
            string[] legarray = Desc.Split(new char[] { ' ', ',', ':', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(s => s.Trim())
                                .ToArray();

            //  string tmpstr1 = Regex.Replace(cat.Longdesc.Trim(), @"[^\w\d]", " ");
            // string[] longarray = tmpstr1.Split(' ');
            string[] longarray = cat.Equipment_Long.Trim().Split(' ', ',', ':', ';');
            string[] shortarray = cat.Equipment_Short.Trim().Split(' ', ',', ':', ';');
            string str = "";

            foreach (string arry in legarray)
            {

                if (!longarray.Contains(arry) && !shortarray.Contains(arry))
                {
                    if (arry != "EQUIPMENT")
                    {
                        str = str + arry + " ";
                    }
                    //  str = legarray + " ";

                }

            }
            return str;
        }
        private string MissingAttributeValue(Prosol_AssetMaster cat)
        {
            string str = "";
            var qry = Query.EQ("UniqueId", cat.UniqueId);
            var attr = _AssetattriRepository.FindOne(qry);
            var existingData = new List<Asset_AttributeList>();
            var newData = new List<Asset_AttributeList>();
            if (attr != null)
            {
                existingData = attr.exCharacterisitics;
                newData = attr.Characterisitics;
            }

            var missingValues = existingData
                .Where(e =>
                {
                    var match = newData.FirstOrDefault(n => n.Characteristic == e.Characteristic);
                    return match != null && string.IsNullOrWhiteSpace(match.Value) && !string.IsNullOrWhiteSpace(e.Value);
                })
                .Select(e => new { e.Characteristic, MissingValue = e.Value })
                .ToList();

            foreach (var item in missingValues)
            {
                Console.WriteLine($"Missing Characteristic: {item.Characteristic}, Value: {item.MissingValue}");
            }
            return str;
        }
        private string PapulatedValue(Prosol_AssetMaster cat)
        {
            // string tmpstr = Regex.Replace(cat.Legacy.Trim(), @"[^\w\d]", " ");
            // string[] legarray = tmpstr.Split(' ');
            string[] legarray = cat.Description.Trim().Split(' ', ',', ':', ';');

            //  string tmpstr1 = Regex.Replace(cat.Longdesc.Trim(), @"[^\w\d]", " ");
            // string[] longarray = tmpstr1.Split(' ');
            string[] longarray = cat.Equipment_Long.Trim().Split(' ', ',', ':', ';');
            string[] shortarray = cat.Equipment_Short.Trim().Split(' ', ',', ':', ';');

            string str = "";

            foreach (string arry in longarray)
            {

                if (!legarray.Contains(arry) && !cat.Description.Contains(arry))
                {
                    if (arry != "ADDITIONAL" && arry != "INFORMATION")
                    {

                        str = str + arry + " ";
                    }
                    //  str = legarray + " ";

                }

            }
            return str;
        }

        public string getMfrAbbr(string mfr)
        {
            string result = "";
            var querrry = Query.EQ("Name", mfr);
            var venLst = _VendorRepository.FindOne(querrry);
            result = venLst != null ? venLst.ShortDescName : "";
            return result;
        }

        public string ShortDesc(Prosol_AssetMaster cat)
        {

            string mfrref = "";

            var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier), Query.EQ("RP", "Equ"));
            var NMList = _nounModifierRepository.FindOne(FormattedQuery);
            var sort = SortBy.Ascending("Seq").Ascending("Description");

           // var query = (NMList != null && NMList.Formatted == 1) ? Query.EQ("Description", "Short_Generic") : Query.EQ("Description", "Short_OEM");
            var query = Query.EQ("Description", "Short_OEM");
            string ShortStr = "", strNM = "", dumbVen = "";

            var seqList = _SequenceRepository.FindAll(query, sort).ToList();
            var UOMSet = _UOMRepository.FindOne();

            var AbbrList = _abbreivateRepository.FindAll();

            //int regLen = 0;
            //var reg = "";
            //int venLen = 0;
            //var ven = "";

            //if (!string.IsNullOrEmpty(cat.Manufacturer))
            //{
            //    ven += ",MFR:" + cat.Manufacturer;
            //}
            //if (!string.IsNullOrEmpty(cat.ModelNo))
            //{
            //    ven += ",MODEL:" + cat.ModelNo;
            //}
            //if (!string.IsNullOrEmpty(cat.SerialNo))
            //{
            //    ven += ",SL:" + cat.SerialNo;
            //}
            //venLen = ven.Length;
            //seqList[0].ShortLength = 240 - (regLen + venLen);
            seqList[0].ShortLength = 40;

            var venLst = GetTarMaster().Where(v => v.Label == "Manufacturer").ToList();

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


                                //if (NMList.Nounabv != null && NMList.Nounabv != "")
                                //    ShortStr += NMList.Nounabv + sq.Separator;
                                //else ShortStr += cat.Noun + sq.Separator;
                                if (NMList != null && !string.IsNullOrEmpty(NMList.Nounabv))
                                {
                                    ShortStr += NMList.Nounabv + sq.Separator;
                                }
                                else
                                {
                                    ShortStr += cat.Noun + sq.Separator;
                                    if (cat.Modifier != "--")
                                        ShortStr += cat.Modifier + sq.Separator;
                                }
                                //if (NMList.Modifierabv != null && NMList.Modifierabv != "")
                                //    ShortStr += NMList.Modifierabv + sq.Separator;
                                //else
                                //{
                                //    if (cat.Modifier != "--" && cat.Modifier != "NO MODIFIER")
                                //        ShortStr += cat.Modifier + sq.Separator;
                                //}
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
                                        ShortStr = ShortStr.TrimEnd(',');
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
                            if (cat.Characteristics != null)
                            {
                                int flg = 0;
                                int i = 0;
                                string strVen = ShortStr;
                                string checkVen = "";
                                string shortCheckMfr = "";
                                if (!string.IsNullOrEmpty(cat.Manufacturer))
                                {
                                    var dt = venLst.Find(d => d.Title == cat.Manufacturer);
                                    shortCheckMfr = dt.Code;
                                }
                                else
                                    shortCheckMfr = cat.Manufacturer;
                                //string mfrCheck = !string.IsNullOrEmpty(shortCheckMfr) ? getMfrAbbr(cat.Manufacturer) : (cat.Manufacturer.Length < seqList[0].ShortLength - ShortStr.Length ? cat.Manufacturer : "");
                                string mfrCheck = "";

                                if (!string.IsNullOrEmpty(shortCheckMfr))
                                {
                                    if (cat?.Manufacturer != null)
                                    {
                                        var dt = venLst.Find(d => d.Title == cat.Manufacturer);
                                        mfrCheck = dt.Code;
                                        //mfrCheck = getMfrAbbr(cat.Manufacturer);
                                    }
                                }
                                else
                                {
                                    if (cat?.Manufacturer != null && seqList != null && seqList.Count > 0 && ShortStr != null)
                                    {
                                        int comparisonLength = seqList[0].ShortLength - ShortStr.Length;
                                        if (cat.Manufacturer.Length < comparisonLength)
                                        {
                                            mfrCheck = cat.Manufacturer;
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(cat.Manufacturer))
                                {
                                    checkVen = !string.IsNullOrEmpty(mfrCheck) ? mfrCheck + ",": "";
                                }
                                if (!string.IsNullOrEmpty(cat.ModelNo) || !string.IsNullOrEmpty(cat.PartNo))
                                {
                                    var value = !string.IsNullOrEmpty(cat.ModelNo) ? cat.ModelNo : cat.PartNo;

                                    checkVen += value + ",";
                                }

                                if (checkVen.EndsWith(","))
                                    checkVen = checkVen.TrimEnd(',');
                                int charLen = seqList[0].ShortLength - checkVen.Length;

                                foreach (Asset_AttributeList chM in cat.Characteristics.OrderBy(x => x.Squence))
                                {

                                    //var abbQry = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier), Query.EQ("Characteristic", chM.Characteristic));
                                    //var abb = _CharateristicRepository.FindOne(abbQry);
                                    //if (abb != null)
                                    //{
                                    //    if (abb.Abbrivation != null)
                                    //    {
                                    //        if (abb.Abbrivation.Contains("_"))
                                    //            chM.Abbrevated = abb.Abbrivation.Replace("_", " ");
                                    //        else
                                    //            chM.Abbrevated = abb.Abbrivation;
                                    //    }
                                    //}
                                    //if (chM.Value != null && chM.Value != "")
                                    //{

                                    //    if (UOMSet.Long_space == "with space")
                                    //    {
                                    //        if (chM.UOM != null && chM.UOM != "")
                                    //        {
                                    //            if (chM.Abbrevated == "ADDL INFO")
                                    //            {
                                    //                var Value = chM.Value;
                                    //                string pattern = @"(\w+(\s\w+)*):";
                                    //                MatchCollection matches = Regex.Matches(chM.Value, pattern);
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
                                    //                    var charLst = _CharateristicRepository.FindOne(qry);
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
                                    //                ShortStr += /*chM.Abbrevated + ":" + */Value + sq.Separator;
                                    //            }
                                    //            else
                                    //            {
                                    //                if(chM.Characteristic == "SIZE" && chM.Value.Contains("X"))
                                    //                ShortStr += /*chM.Abbrevated + ":" + */chM.Value + " " + chM.UOM + sq.Separator;
                                    //                else
                                    //                ShortStr += /*chM.Abbrevated + ":" + */chM.Value + chM.UOM + sq.Separator;
                                    //            }
                                    //        }
                                    //        else
                                    //        {
                                    //            if (flg == 0 && chM.Abbrevated == "PART NAME")
                                    //            {
                                    //                ShortStr += chM.Value + sq.Separator;
                                    //            }
                                    //            else
                                    //            {
                                    //                if (chM.Abbrevated == "ADDL INFO")
                                    //                {
                                    //                    var Value = chM.Value;
                                    //                    string pattern = @"(\w+(\s\w+)*):";
                                    //                    MatchCollection matches = Regex.Matches(chM.Value, pattern);
                                    //                    List<string> result = new List<string>();
                                    //                    foreach (Match match in matches)
                                    //                    {
                                    //                        result.Add(match.Groups[1].Value);
                                    //                    }

                                    //                    //var qry = Query.EQ("Characteristic", "MM");
                                    //                    //var charLst = _CharacteristicRepository.FindAll(qry).ToList();
                                    //                    //var unqCharLst = charLst.GroupBy(p => p.Characteristic).Select(g => g.First()).ToList();
                                    //                    foreach (var unq in result)
                                    //                    {
                                    //                        var qry = Query.EQ("Characteristic", unq);
                                    //                        var charLst = _CharateristicRepository.FindOne(qry);
                                    //                        if (charLst != null)
                                    //                        {
                                    //                            if (Value.Contains(charLst.Characteristic))
                                    //                            {
                                    //                                if (charLst.Abbrivation.Contains("_"))
                                    //                                    charLst.Abbrivation = charLst.Abbrivation.Replace("_", " ");
                                    //                                Value = Value.Replace(charLst.Characteristic, charLst.Abbrivation);
                                    //                            }
                                    //                        }
                                    //                    }
                                    //                    ShortStr += /*chM.Abbrevated + ":" + */Value + sq.Separator;
                                    //                }
                                    //                else
                                    //                {
                                    //                    ShortStr += /*chM.Abbrevated + ":" + */chM.Value + sq.Separator;
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        if (chM.UOM != null && chM.UOM != "")
                                    //        {
                                    //            if (chM.Abbrevated == "ADDL INFO")
                                    //            {
                                    //                var Value = chM.Value;
                                    //                string pattern = @"(\w+(\s\w+)*):";
                                    //                MatchCollection matches = Regex.Matches(chM.Value, pattern);
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
                                    //                    var charLst = _CharateristicRepository.FindOne(qry);
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
                                    //                ShortStr += /*chM.Abbrevated + ":" + */Value + sq.Separator;
                                    //            }
                                    //            else
                                    //            {
                                    //                if (chM.Characteristic == "SIZE" && chM.Value.Contains("X"))
                                    //                    ShortStr += /*chM.Abbrevated + ":" + */chM.Value + " " + chM.UOM + sq.Separator;
                                    //                ShortStr += /*chM.Abbrevated + ":" + */chM.Value + chM.UOM + sq.Separator;
                                    //            }
                                    //        }
                                    //        else
                                    //        {
                                    //            if (flg == 0 && chM.Abbrevated == "PART NAME")
                                    //                ShortStr += chM.Value + sq.Separator;
                                    //            else
                                    //            {
                                    //                if (chM.Abbrevated == "ADDL INFO")
                                    //                {
                                    //                    var Value = chM.Value;
                                    //                    string pattern = @"(\w+(\s\w+)*):";
                                    //                    MatchCollection matches = Regex.Matches(chM.Value, pattern);
                                    //                    List<string> result = new List<string>();
                                    //                    foreach (Match match in matches)
                                    //                    {
                                    //                        result.Add(match.Groups[1].Value);
                                    //                    }

                                    //                    //var qry = Query.EQ("Characteristic", "MM");
                                    //                    //var charLst = _CharacteristicRepository.FindAll(qry).ToList();
                                    //                    //var unqCharLst = charLst.GroupBy(p => p.Characteristic).Select(g => g.First()).ToList();
                                    //                    foreach (var unq in result)
                                    //                    {
                                    //                        var qry = Query.EQ("Characteristic", unq);
                                    //                        var charLst = _CharateristicRepository.FindOne(qry);
                                    //                        if (charLst != null)
                                    //                        {
                                    //                            if (Value.Contains(charLst.Characteristic))
                                    //                            {
                                    //                                if (charLst.Abbrivation.Contains("_"))
                                    //                                    charLst.Abbrivation = charLst.Abbrivation.Replace("_", " ");
                                    //                                Value = Value.Replace(charLst.Characteristic, charLst.Abbrivation);
                                    //                            }
                                    //                        }
                                    //                    }
                                    //                    ShortStr += /*chM.Abbrevated + ":" + */Value + sq.Separator;
                                    //                }
                                    //                else
                                    //                {
                                    //                    ShortStr += /*chM.Abbrevated + ":" + */chM.Value + sq.Separator;
                                    //                }
                                    //            }
                                    //        }
                                    //    }

                                    //}
                                    //flg = 1;

                                    //New
                                    string dumbChar = ShortStr;
                                    if (chM.Value != null && chM.Value != "" && chM.Value != "--" && chM.Characteristic != "ADDITIONAL INFORMATION")
                                    {
                                        if (!string.IsNullOrEmpty(chM.UOM))
                                        {
                                            if(chM.Characteristic == "SIZE" && chM.Value.Contains("X"))
                                                dumbChar = ShortStr + chM.Value+" "+chM.UOM;
                                            else
                                                dumbChar = ShortStr + chM.Value + chM.UOM;
                                        }
                                        else
                                            dumbChar = ShortStr + chM.Value;
                                    }


                                    if (NMList.Formatted == 1 || flg == 1 || NMList.Formatted == 2)//&& NMList.Formatted != 0
                                    {

                                        if (chM.Value != null && chM.Value != "")
                                        {
                                            if (dumbChar.Length < charLen)
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
                                                    //if (UOMSet.Short_space == "with space")
                                                    if (chM.Characteristic == "SIZE" && chM.Value.Contains("X"))
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
                                                        //if (UOMSet.Short_space == "with space")
                                                        if (chM.Characteristic == "SIZE" && chM.Value.Contains("X"))
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

                                                        //if (UOMSet.Short_space == "with space")
                                                        if (chM.Characteristic == "SIZE" && chM.Value.Contains("X"))
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

                                                ShortStr = strVen;
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
                                    }
                                    else flg = 1;
                                }
                            }
                            break;
                        case 104:
                            dumbVen = "";
                            string shortMfr = "";
                            if (!string.IsNullOrEmpty(cat.Manufacturer))
                            {
                                var dt1 = venLst.Find(d => d.Title == cat.Manufacturer);
                                shortMfr = dt1.Code;
                                //shortMfr = getMfrAbbr(cat.Manufacturer);
                            }
                            else
                                shortMfr = cat.Manufacturer;
                            string gShortMfr = "";
                            if (!string.IsNullOrEmpty(cat.Manufacturer))
                            {
                                var dt2 = venLst.Find(d => d.Title == cat.Manufacturer);
                                gShortMfr = dt2.Code;
                            }
                            string mfr = !string.IsNullOrEmpty(shortMfr) ? gShortMfr : cat.Manufacturer;
                            //string mfr = !string.IsNullOrEmpty(shortMfr) ? getMfrAbbr(cat.Manufacturer) : cat.Manufacturer;
                            if (!string.IsNullOrEmpty(cat.Manufacturer))
                            {
                                dumbVen = ShortStr + mfr + ",";
                            }
                            if(!string.IsNullOrEmpty(mfr) && dumbVen.TrimEnd(',').Length <= seqList[0].ShortLength)
                                ShortStr += mfr + ",";
                            break;
                        case 105:
                            //if (!string.IsNullOrEmpty(cat.TechIdentNo))
                            //    ShortStr += "," + cat.TechIdentNo;
                            dumbVen = "";
                            if (!string.IsNullOrEmpty(cat.ModelNo) || !string.IsNullOrEmpty(cat.PartNo))
                            {
                                var value = !string.IsNullOrEmpty(cat.ModelNo) ? cat.ModelNo : cat.PartNo;

                                dumbVen = ShortStr + value + ",";

                                if (dumbVen.TrimEnd(',').Length <= seqList[0].ShortLength)
                                {
                                    ShortStr += value + ",";
                                }
                            }

                            //if (!string.IsNullOrEmpty(cat.SerialNo))
                            //    ShortStr += "," + cat.SerialNo;
                            break;
                        //if (cat.Vendor != null)
                        //{
                        //    foreach (Vendorsuppliers vs in cat.Vendor)
                        //    {
                        //        if (vs.s == 1 && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined")
                        //        {
                        //            string prefix = "";
                        //            if (vs.Refflag != null)
                        //            {
                        //                var flag = Query.EQ("Type", vs.Refflag);
                        //                var code = _ReftypeRepository.FindOne(flag);
                        //                if (code != null)
                        //                {
                        //                    prefix = code.Code + ":";
                        //                }
                        //            }
                        //            var frmMdl = new shortFrame();
                        //            frmMdl.position = 100;
                        //            //frmMdl.values = prefix + vs.RefNo.Trim() + sq.Separator;
                        //            frmMdl.values = vs.RefNo.Trim() + sq.Separator;
                        //            lst.Add(frmMdl);
                        //            // ShortStr = strNM;

                                //            //ShortStr += prefix + vs.RefNo.Trim() + sq.Separator;
                                //            ShortStr += vs.RefNo.Trim() + sq.Separator;
                                //            if (!checkLength(ShortStr, seqList[0].ShortLength))
                                //            {
                                //                ShortStr = ShortStr.Trim();
                                //                char[] chr = sq.Separator.ToCharArray();
                                //                ShortStr = ShortStr.TrimEnd(chr[0]);
                                //                while (ShortStr.Length > seqList[0].ShortLength)
                                //                {
                                //                    int lstIndx = ShortStr.LastIndexOf(chr[0]);
                                //                    if (lstIndx > -1)
                                //                    {
                                //                        if (lst.Count > 0)
                                //                        {
                                //                            if (ShortStr.Substring(lstIndx).Length > 1)
                                //                                lst.RemoveAt(lst.Count - 1);
                                //                        }
                                //                        ShortStr = ShortStr.Remove(lstIndx);

                                //                    }
                                //                    else
                                //                    {
                                //                        lstIndx = ShortStr.LastIndexOf(' ');
                                //                        ShortStr = ShortStr.Remove(lstIndx);
                                //                        if (lst.Count > 0)
                                //                        {
                                //                            int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
                                //                            lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
                                //                        }
                                //                    }

                                //                }
                                //                ShortStr = ShortStr + chr[0];
                                //            }
                                //            break;
                                //        }

                                //    }
                                //}
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
                                //break;
                        case 106:
                        //if (cat.Application != null && cat.Application != "")
                        //    ShortStr += cat.Application + sq.Separator;
                        //break;
                        case 107:
                        //if (cat.Drawingno != null && cat.Drawingno != "")
                        //    ShortStr += cat.Drawingno + sq.Separator;


                        //foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
                        //{
                        //    if (vs.Refflag.ToUpper() == "DRAWING & POSITION NUMBER" && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined" && vs.Name == mfrref && vs.Name != "" && vs.Name != null)
                        //    {
                        //        ShortStr += vs.RefNo.Trim() + sq.Separator;
                        //        break;
                        //    }
                        //}
                        //break;
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
                        //if (cat.Referenceno != null && cat.Referenceno != "")
                        //    ShortStr += cat.Referenceno + sq.Separator;
                        //break;
                        case 114:
                        //if (cat.Additionalinfo != null && cat.Additionalinfo != "")
                        //    ShortStr += cat.Additionalinfo + sq.Separator;
                        //break;
                        case 115:
                            if (cat.Equipment != null && cat.Equipment.Additionalinfo != null && cat.Equipment.Additionalinfo != "")
                                ShortStr += cat.Equipment.Additionalinfo + sq.Separator;
                            break;
                            //case 116:
                            //    if (!string.IsNullOrEmpty(cat.Manufacturer))
                            //    {
                            //        ShortStr += "MFR:" + cat.Manufacturer + sq.Separator;
                            //    }
                            //    break;
                            //case 117:
                            //    if (!string.IsNullOrEmpty(cat.ModelNo))
                            //    {
                            //        ShortStr += "MODEL:" + cat.ModelNo + sq.Separator;
                            //    }
                            //    break;
                            //case 118:
                            //    if (!string.IsNullOrEmpty(cat.SerialNo))
                            //    {
                            //        ShortStr += "SL:" + cat.SerialNo + sq.Separator;
                            //    }
                            //    break;


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

            //ShortStr += ven + reg;
            if (ShortStr.Contains(",,,"))
                ShortStr.Replace(",,,", ",");
            if (ShortStr.Contains(",,"))
                ShortStr.Replace(",,", ",");
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
        public string LongDesc(Prosol_AssetMaster cat)
        {

            // var vendortype = _VendortypeRepository.FindAll().ToList();
            var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
            var NMList = _nounModifierRepository.FindOne(FormattedQuery);
            var sort = SortBy.Ascending("Seq").Ascending("Description");
            var query = Query.EQ("Description", "Long_OEM");
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
                            // else LongStr += cat.Noun + " ";
                            break;
                        case 102:
                            if (NMList.Formatted == 1 || NMList.Formatted == 2)
                                if (cat.Modifier != "NO DEFINER" && cat.Modifier != "NO MODIFIER" && cat.Modifier != "--")
                                    LongStr += cat.Modifier + sq.Separator;
                            break;
                        case 103:
                            if (cat.Characteristics != null)
                            {
                                int flg = 0;
                                foreach (Asset_AttributeList chM in cat.Characteristics.OrderBy(x => x.Squence))
                                {

                                    if (chM.Value != null && chM.Value != "")
                                    {

                                        if (UOMSet.Long_space == "with space")
                                        {
                                            if (chM.UOM != null && chM.UOM != "")
                                            {
                                                if (chM.Characteristic == "ADDL INFO")
                                                {
                                                    var Value = chM.Value;
                                                    string pattern = @"(\w+(\s\w+)*):";
                                                    MatchCollection matches = Regex.Matches(chM.Value, pattern);
                                                    List<string> result = new List<string>();
                                                    foreach (Match match in matches)
                                                    {
                                                        result.Add(match.Groups[1].Value);
                                                    }

                                                    //var qry = Query.EQ("Characteristic", "MM");
                                                    //var charLst = _CharacteristicRepository.FindAll(qry).ToList();
                                                    //var unqCharLst = charLst.GroupBy(p => p.Characteristic).Select(g => g.First()).ToList();
                                                    foreach (var unq in result)
                                                    {
                                                        var qry = Query.EQ("Characteristic", unq);
                                                        var charLst = _CharateristicRepository.FindOne(qry);
                                                        if (charLst != null)
                                                        {
                                                            if (Value.Contains(charLst.Characteristic))
                                                            {
                                                                if (charLst.Abbrivation.Contains("_"))
                                                                    charLst.Abbrivation = charLst.Abbrivation.Replace("_", " ");
                                                                Value = Value.Replace(charLst.Characteristic, charLst.Abbrivation);
                                                            }
                                                        }
                                                    }
                                                    LongStr += chM.Characteristic + ":" + Value + sq.Separator;
                                                }
                                                else
                                                    LongStr += chM.Characteristic + ":" + chM.Value + " " + chM.UOM + sq.Separator;
                                            }
                                            else
                                            {
                                                if (flg == 0 && chM.Characteristic == "PART NAME")
                                                {
                                                    LongStr += chM.Value + sq.Separator;
                                                }
                                                else
                                                {
                                                    if (chM.Characteristic == "ADDL INFO")
                                                    {
                                                        var Value = chM.Value;
                                                        string pattern = @"(\w+(\s\w+)*):";
                                                        MatchCollection matches = Regex.Matches(chM.Value, pattern);
                                                        List<string> result = new List<string>();
                                                        foreach (Match match in matches)
                                                        {
                                                            result.Add(match.Groups[1].Value);
                                                        }

                                                        //var qry = Query.EQ("Characteristic", "MM");
                                                        //var charLst = _CharacteristicRepository.FindAll(qry).ToList();
                                                        //var unqCharLst = charLst.GroupBy(p => p.Characteristic).Select(g => g.First()).ToList();
                                                        foreach (var unq in result)
                                                        {
                                                            var qry = Query.EQ("Characteristic", unq);
                                                            var charLst = _CharateristicRepository.FindOne(qry);
                                                            if (charLst != null)
                                                            {
                                                                if (Value.Contains(charLst.Characteristic))
                                                                {
                                                                    if (charLst.Abbrivation.Contains("_"))
                                                                        charLst.Abbrivation = charLst.Abbrivation.Replace("_", " ");
                                                                    Value = Value.Replace(charLst.Characteristic, charLst.Abbrivation);
                                                                }
                                                            }
                                                        }
                                                        LongStr += chM.Characteristic + ":" + Value + sq.Separator;
                                                    }
                                                    else
                                                        LongStr += chM.Characteristic + ":" + chM.Value + sq.Separator;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (chM.UOM != null && chM.UOM != "")
                                            {
                                                if (chM.Characteristic == "ADDL INFO")
                                                {
                                                    var Value = chM.Value;
                                                    string pattern = @"(\w+(\s\w+)*):";
                                                    MatchCollection matches = Regex.Matches(chM.Value, pattern);
                                                    List<string> result = new List<string>();
                                                    foreach (Match match in matches)
                                                    {
                                                        result.Add(match.Groups[1].Value);
                                                    }

                                                    //var qry = Query.EQ("Characteristic", "MM");
                                                    //var charLst = _CharacteristicRepository.FindAll(qry).ToList();
                                                    //var unqCharLst = charLst.GroupBy(p => p.Characteristic).Select(g => g.First()).ToList();
                                                    foreach (var unq in result)
                                                    {
                                                        var qry = Query.EQ("Characteristic", unq);
                                                        var charLst = _CharateristicRepository.FindOne(qry);
                                                        if (charLst != null)
                                                        {
                                                            if (Value.Contains(charLst.Characteristic))
                                                            {
                                                                if (charLst.Abbrivation.Contains("_"))
                                                                    charLst.Abbrivation = charLst.Abbrivation.Replace("_", " ");
                                                                Value = Value.Replace(charLst.Characteristic, charLst.Abbrivation);
                                                            }
                                                        }
                                                    }
                                                    LongStr += chM.Characteristic + ":" + Value + sq.Separator;
                                                }
                                                else
                                                    LongStr += chM.Characteristic + ":" + chM.Value + chM.UOM + sq.Separator;
                                            }
                                            else
                                            {
                                                if (flg == 0 && chM.Characteristic == "PART NAME")
                                                    LongStr += chM.Value + sq.Separator;
                                                else
                                                {
                                                    if (chM.Characteristic == "ADDL INFO")
                                                    {
                                                        var Value = chM.Value;
                                                        string pattern = @"(\w+(\s\w+)*):";
                                                        MatchCollection matches = Regex.Matches(chM.Value, pattern);
                                                        List<string> result = new List<string>();
                                                        foreach (Match match in matches)
                                                        {
                                                            result.Add(match.Groups[1].Value);
                                                        }

                                                        //var qry = Query.EQ("Characteristic", "MM");
                                                        //var charLst = _CharacteristicRepository.FindAll(qry).ToList();
                                                        //var unqCharLst = charLst.GroupBy(p => p.Characteristic).Select(g => g.First()).ToList();
                                                        foreach (var unq in result)
                                                        {
                                                            var qry = Query.EQ("Characteristic", unq);
                                                            var charLst = _CharateristicRepository.FindOne(qry);
                                                            if (charLst != null)
                                                            {
                                                                if (Value.Contains(charLst.Characteristic))
                                                                {
                                                                    if (charLst.Abbrivation.Contains("_"))
                                                                        charLst.Abbrivation = charLst.Abbrivation.Replace("_", " ");
                                                                    Value = Value.Replace(charLst.Characteristic, charLst.Abbrivation);
                                                                }
                                                            }
                                                        }
                                                        LongStr += chM.Characteristic + ":" + Value + sq.Separator;
                                                    }
                                                    else
                                                        LongStr += chM.Characteristic + ":" + chM.Value + sq.Separator;
                                                }
                                            }
                                        }

                                    }
                                    flg = 1;
                                }
                            }
                            break;

                        case 104:
                            if (!string.IsNullOrEmpty(cat.TechIdentNo))
                                LongStr += "TAG NUMBER:" + cat.TechIdentNo + ",";
                            if (!string.IsNullOrEmpty(cat.Manufacturer))
                                LongStr += "MANUFACTURER:" +cat.Manufacturer + ",";
                            break;
                        //if (cat.Manufacturer != null && cat.Manufacturer != "")
                        //    LongStr += "Manufacturer:" + cat.Manufacturer + sq.Separator;
                        //break;
                        //if (cat.Vendorsuppliers != null)
                        //{

                        //    int g = 0;
                        //    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
                        //    {

                        //        if (vs.l == 1 && vs.Name != null && vs.Name != "")
                        //        {
                        //            LongStr += vs.Type + ":" + vs.Name + sq.Separator;

                        //        }
                        //        if (vs.l == 1 && vs.RefNo != null && vs.RefNo != "")
                        //        {
                        //            LongStr += vs.Refflag + ":" + vs.RefNo + sq.Separator;
                        //        }
                        //        //else
                        //        //{
                        //        //    if (vs.l == 1 && vs.RefNo != null && vs.RefNo != "")
                        //        //    {
                        //        //        LongStr += vs.Refflag + ":" + vs.RefNo + sq.Separator;
                        //        //    }
                        //        //}
                        //    }


                        //    //string[] mfrnames = new string[cat.Vendorsuppliers.Count];
                        //    //int iii = 0;
                        //    //foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
                        //    //{
                        //    //    if (!mfrnames.Contains(vs.Name))
                        //    //    {
                        //    //        mfrnames[iii] = vs.Name;
                        //    //        iii++;
                        //    //    }
                        //    //}
                        //    //foreach (string names in mfrnames)
                        //    //{
                        //    //    int g = 0;
                        //    //    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
                        //    //    {

                        //    //        if (vs.l == 1 && g == 0 && vs.Name == names && vs.Name != null && vs.Name != "")
                        //    //        {
                        //    //            LongStr += vs.Type + ":" + vs.Name + sq.Separator;
                        //    //            g = 1;
                        //    //        }
                        //    //        if (vs.l == 1 && vs.Name == names && vs.RefNo != null && vs.RefNo != "")
                        //    //        {
                        //    //            LongStr += vs.Refflag + ":" + vs.RefNo + sq.Separator;
                        //    //        }
                        //    //    }

                        //}
                        //break;

                        case 105:
                            if (!string.IsNullOrEmpty(cat.ModelNo))
                                LongStr += "MODEL NUMBER:" + cat.ModelNo+ ",";
                            if (!string.IsNullOrEmpty(cat.PartNo))
                                LongStr += "PART NUMBER:" + cat.PartNo+ ",";
                            if (!string.IsNullOrEmpty(cat.SerialNo))
                                LongStr += "SERIAL NUMBER:" + cat.SerialNo+ ",";
                            if (!string.IsNullOrEmpty(cat.MfrCountry))
                                LongStr += "MANUFACTURER COUNTRY:" + cat.MfrCountry+ ",";
                            if (!string.IsNullOrEmpty(cat.MfrYear))
                                LongStr += "MANUFACTURER YEAR:" + cat.MfrYear+ ",";
                            break;
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
                        //if (cat.Application != null && cat.Application != "")
                        //    LongStr += "APPLICATION:" + cat.Application + sq.Separator;
                        //break;
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
                        //if (cat.Referenceno != null && cat.Referenceno != "")
                        //    LongStr += "POSITION NO." + cat.Referenceno + sq.Separator;
                        //break;
                        case 114:
                            if (cat.AdditionalInfo != null && cat.AdditionalInfo != "")
                                LongStr += "ADDITIONAL INFORMATION:" + cat.AdditionalInfo + sq.Separator;
                            break;
                            //case 115:
                            //    if (cat.Equipment != null && cat.Equipment.Additionalinfo != null && cat.Equipment.Additionalinfo != "")
                            //        LongStr += "ADDITIONAL INFORMATION(EQUIPMENT):" + cat.Equipment.Additionalinfo + sq.Separator;
                            //    break;
                            //case 116:
                            //    if (!string.IsNullOrEmpty(cat.Manufacturer))
                            //    {
                            //        LongStr += "MFR:" + cat.Manufacturer + sq.Separator;
                            //    }
                            //    break;
                            //case 117:
                            //    if (!string.IsNullOrEmpty(cat.ModelNo))
                            //    {
                            //        LongStr += "MODEL:" + cat.ModelNo + sq.Separator;
                            //    }
                            //    break;
                            //case 118:
                            //    if (!string.IsNullOrEmpty(cat.SerialNo))
                            //    {
                            //        LongStr += "SL:" + cat.SerialNo + sq.Separator;
                            //    }
                            //    break;


                    }
                }
            }
            LongStr = LongStr.Trim();
            int lstIndx = LongStr.Length;
            LongStr = LongStr.Remove(lstIndx - 1, 1);
            //int regLen = 0;
            //var reg = "";
            //int venLen = 0;
            //var ven = "";

            //if (!string.IsNullOrEmpty(cat.Manufacturer))
            //{
            //    ven += ",MFR:" + cat.Manufacturer;
            //}
            //if (!string.IsNullOrEmpty(cat.ModelNo))
            //{
            //    ven += ",MODEL:" + cat.ModelNo;
            //}
            //if (!string.IsNullOrEmpty(cat.SerialNo))
            //{
            //    ven += ",SL:" + cat.SerialNo;
            //}
            //if (!string.IsNullOrEmpty(cat.ModelYear))
            //{
            //    ven += ",YEAR:" + cat.ModelYear;
            //}
            //venLen = ven.Length;
            //LongStr += ven + reg;
            return LongStr;
        }

        private string GetFileSize(double byteCount)
        {
            string size = "0 Bytes";
            if (byteCount >= 1073741824.0)
                size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";
            else if (byteCount >= 1048576.0)
                size = String.Format("{0:##.##}", byteCount / 1048576.0) + " MB";
            else if (byteCount >= 1024.0)
                size = String.Format("{0:##.##}", byteCount / 1024.0) + " KB";
            else if (byteCount > 0 && byteCount < 1024.0)
                size = byteCount.ToString() + " Bytes";

            return size;
        }


        //Dashboard

        public List<Prosol_AssetMaster> getMasterItems()
        {
            //var query = Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", -1), Query.EQ("ItemStatus", 0)/*, Query.EQ("ItemStatus", 0)*/);
            var query = Query.GTE("ItemStatus", -3);
            var itmsLst = _assetRepository.FindAll(query).ToList();
            return itmsLst;
        }

        public List<Prosol_AssetMaster> getSKUItems()
        {
            var query = Query.GTE("ItemStatus", 0);
            var itmsLst = _assetRepository.FindAll(query).ToList();
            return itmsLst;
        }
        public List<Prosol_AssetMaster> getReqItems()
        {
            var itmsLst = _assetRepository.FindAll().ToList();
            return itmsLst;
        }
        //public List<Prosol_AssetMaster> getMasterItems()
        //{
        //    //var query = Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", -1), Query.EQ("ItemStatus", 0)/*, Query.EQ("ItemStatus", 0)*/);
        //    var query = Query.GTE("ItemStatus", -3);
        //    var itmsLst = _assetRepository.FindAll(query).ToList();
        //    return itmsLst;
        //}
        public List<Prosol_AssetMaster> getPendItems()
        {
            var query = Query.EQ("ItemStatus", 2);
            var itmsLst = _assetRepository.FindAll(query).ToList();
            return itmsLst;
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


                var query1 = Query.And(Query.EQ("UniqueId", drw[0].ToString()));
                var mdl1 = _assetRepository.FindOne(query1);
                var mdl2 = _AssetattriRepository.FindOne(query1);
                if (mdl2 != null)
                {

                    var ListCharas = mdl2.Characterisitics;

                    var lstCharateristics = new List<Asset_AttributeList>();
                    if (ListCharas != null && ListCharas.Count > 0)
                    {

                        foreach (Asset_AttributeList LstAtt in ListCharas)
                        {
                            var AttrMdl = new Asset_AttributeList();
                            AttrMdl.Characteristic = LstAtt.Characteristic;
                            AttrMdl.Value = LstAtt.Value;
                            AttrMdl.UOM = LstAtt.UOM;

                            var d = Query.And(Query.EQ("Noun", mdl2.Noun), Query.EQ("Modifier", mdl2.Modifier), Query.EQ("Characteristic", AttrMdl.Characteristic));
                            var m1 = _CharateristicRepository.FindOne(d);
                            if (m1 != null)
                            {
                                AttrMdl.ShortSquence = m1.ShortSquence;
                                AttrMdl.Squence = m1.Squence;
                                AttrMdl.Source = LstAtt.Source;
                                lstCharateristics.Add(AttrMdl);
                            }

                        }
                    }
                    if (mdl1 != null)
                    {
                        mdl1.Characteristics = lstCharateristics;
                        mdl1.Noun = mdl2.Noun;
                        mdl1.Modifier = mdl2.Modifier;
                        mdl1.Equipment_Short = ShortDesc(mdl1);
                        mdl1.Equipment_Long = LongDesc(mdl1);
                        mdl1.MissingValue = MissingValue(mdl1);
                        mdl1.EnrichedValue = PapulatedValue(mdl1);
                        mdl1.Characteristics = null;
                        //var qc = new Prosol_UpdatedBy();
                        //qc.Name = "SACHIN";
                        //qc.UserId = "33";
                        //qc.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        //mdl1.Review = qc;
                        //mdl1.ItemStatus = 5;
                        _assetRepository.Add(mdl1);
                        cunt++;
                    }
                }

            }


            return cunt;
        }
        public virtual int BulkAutoShortLong()
        {
            int cunt = 0;
            var qry = Query.And(Query.NE("Noun", BsonNull.Value),Query.NE("Modifier", BsonNull.Value),Query.NE("Characterisitics", BsonNull.Value));
            var lst = _AssetattriRepository.FindAll(qry).ToList();
            foreach (var drw in lst)
            {
                var query1 = Query.And(Query.EQ("UniqueId", drw.UniqueId));
                var mdl1 = _assetRepository.FindOne(query1);
                var mdl2 = _AssetattriRepository.FindOne(query1);
                if (mdl2 != null)
                {
                    var ListCharas = mdl2.Characterisitics;

                    var lstCharateristics = new List<Asset_AttributeList>();
                    if (ListCharas != null && ListCharas.Count > 0)
                    {

                        foreach (Asset_AttributeList LstAtt in ListCharas)
                        {
                            var AttrMdl = new Asset_AttributeList();
                            AttrMdl.Characteristic = LstAtt.Characteristic;
                            AttrMdl.Value = LstAtt.Value;
                            AttrMdl.UOM = LstAtt.UOM;

                            var d = Query.And(Query.EQ("Noun", mdl2.Noun), Query.EQ("Modifier", mdl2.Modifier), Query.EQ("Characteristic", AttrMdl.Characteristic));
                            var m1 = _CharateristicRepository.FindOne(d);
                            if (m1 != null)
                            {
                                AttrMdl.ShortSquence = m1.ShortSquence;
                                AttrMdl.Squence = m1.Squence;
                                AttrMdl.Source = LstAtt.Source;
                                lstCharateristics.Add(AttrMdl);
                            }

                        }
                    }
                    if (mdl1 != null)
                    {
                        mdl1.Characteristics = lstCharateristics;
                        mdl1.Noun = mdl2.Noun;
                        mdl1.Modifier = mdl2.Modifier;
                        mdl1.Equipment_Short = ShortDesc(mdl1);
                        mdl1.Equipment_Long = LongDesc(mdl1);
                        mdl1.MissingValue = MissingValue(mdl1);
                        mdl1.EnrichedValue = PapulatedValue(mdl1);
                        mdl1.Characteristics = null;
                        //var qc = new Prosol_UpdatedBy();
                        //qc.Name = "SACHIN";
                        //qc.UserId = "33";
                        //qc.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        //mdl1.Review = qc;
                        //mdl1.ItemStatus = 5;
                        _assetRepository.Add(mdl1);
                        cunt++;
                    }
                }

            }


            return cunt;
        }

        public List<Dictionary<string, object>> Downloaddata(string username, string status)
        {
            IMongoQuery query;
            var datalist = new List<Prosol_AssetMaster>();
            if (status == "cat")
            {
                query = Query.Or(
                Query.And(
                Query.EQ("Catalogue.Name", username), Query.Or(/*Query.EQ("ItemStatus", 2),*/ Query.EQ("ItemStatus", 3))
                ),
                Query.And(
                Query.EQ("Review.Name", username), Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 5))
                )
                );
            }
            else if (status == "qc")
            {
                query = Query.And(Query.EQ("Review.Name", username), Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 5)));
                if (username == "Ramkumar")
                {
                    query = Query.And(Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 5)));
                }
            }
            else if (status == "All")
            {
                query = Query.Or(Query.NE("Catalogue", BsonNull.Value), Query.GT("ItemStatus", 0));
            }
            else if (status == "QA")
            {
                query = Query.And(Query.EQ("Release.Name", username), Query.GTE("ItemStatus", 4), Query.EQ("category", BsonNull.Value));

            }
            else
            {
                query = Query.GTE("ItemStatus", 8);
            }
            datalist = _assetRepository.FindAll(query).ToList();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
            Dictionary<string, object> rowh;
            foreach (var code in datalist)
            {
                var chaQry = Query.EQ("UniqueId", code.UniqueId);
                var nmChar = _AssetattriRepository.FindOne(chaQry);
                var Charateristics = code.Characteristics;
                if (status != "All")
                {
                    if (nmChar != null)
                    {
                        if (!string.IsNullOrEmpty(nmChar.Noun) && !string.IsNullOrEmpty(nmChar.Modifier))
                        {
                            var QR = Query.And(Query.EQ("Noun", nmChar.Noun), Query.EQ("Modifier", nmChar.Modifier), Query.EQ("Definition", "Equ"));
                            var mm = _CharateristicRepository.FindAll(QR).ToList();
                            var nm = mm.OrderBy(x => x.Squence).ToList();
                            if (nm.Count > 0)
                            {
                                code.Characteristics = nmChar.Characterisitics;
                                code.Noun = nmChar.Noun;
                                code.Modifier = nmChar.Modifier;
                                Charateristics = code.Characteristics;
                                var exCharateristics = new List<Asset_AttributeList>();
                                if (nmChar.exCharacterisitics != null)
                                    exCharateristics = nmChar.exCharacterisitics;
                                foreach (var nm1 in nm)
                                {
                                    int chaCnt = 0;
                                    if (Charateristics != null)
                                        chaCnt = Charateristics.Count();
                                    int filledCha = Charateristics?.Count(g => g.Value != "") ?? 0;
                                    double chaFilledRatePer = (double)filledCha / chaCnt * 100;
                                    string chaFilledRate = chaFilledRatePer.ToString("F2") + " %";

                                    int exchaCnt = 0;
                                    if (exCharateristics != null)
                                        exchaCnt = exCharateristics.Count();
                                    int filledexCha = exCharateristics.Count(g => g.Value != "");
                                    double exchaFilledRatePer = (double)filledexCha / exchaCnt * 100;
                                    string exchaFilledRate = exchaFilledRatePer.ToString("F2") + " %";
                                    var mandCharc = new List<Asset_AttributeList>();
                                    if (Charateristics != null && code != null && _CharateristicRepository != null)
                                    {
                                        if (mandCharc == null) mandCharc = new List<Asset_AttributeList>();

                                        foreach (var cha in Charateristics)
                                        {
                                            if (cha == null || string.IsNullOrEmpty(cha.Characteristic)) continue;

                                            var mand = new Asset_AttributeList();
                                            var queryyy = Query.And(
                                                Query.EQ("Noun", code.Noun ?? ""),
                                                Query.EQ("Modifier", code.Modifier ?? ""),
                                                Query.EQ("Characteristic", cha.Characteristic)
                                            );

                                            var result = _CharateristicRepository.FindOne(queryyy);
                                            if (result != null)
                                            {
                                                mand.Characteristic = cha.Characteristic;
                                                mand.Value = cha.Value;
                                                mand.Abbrevated = cha.Abbrevated;
                                                mand.UomMandatory = result.Mandatory;
                                            }
                                            mandCharc.Add(mand);
                                        }
                                    }

                                    int mandCnt = mandCharc.Count();
                                    int MandCha = mandCharc.Count(g => g.UomMandatory == "Yes");
                                    int filledMandCha = mandCharc.Count(g => g.UomMandatory == "Yes" && g.Value != "");
                                    double mandFilledRatePer = (double)filledMandCha / MandCha * 100;
                                    string mandFilledRate = mandFilledRatePer.ToString("F2") + " %";
                                    if (exchaFilledRate == "NaN %")
                                    {
                                        exchaFilledRate = "0.00 %";
                                    }
                                    if (chaFilledRate == "NaN %")
                                    {
                                        chaFilledRate = "0.00 %";
                                    }
                                    if (mandFilledRate == "NaN %")
                                    {
                                        mandFilledRate = "0.00 %";
                                    }
                                    if (Charateristics != null && Charateristics.Count > 0)
                                    {
                                        if (nm1.Noun == code.Noun && nm1.Modifier == code.Modifier && !string.IsNullOrEmpty(nm1.Characteristic))
                                        {
                                            var at = code.Characteristics.Where(x => x.Characteristic == nm1.Characteristic).ToList();
                                            if (at.Count > 0)
                                            {
                                                row = new Dictionary<string, object>();
                                                row.Add("Unique ID", code.UniqueId);
                                                row.Add("Asset NO", code.AssetNo);
                                                row.Add("Equipment Description", code.Description);
                                                row.Add("Noun", nm1.Noun);
                                                row.Add("Modifier", nm1.Modifier);
                                                row.Add("Shortdesc", code.Equipment_Short);
                                                row.Add("Longdesc", code.Equipment_Long);
                                                row.Add("PDesc", nm1.PDesc);
                                                row.Add("Attribute ID", nm1.Abbrivation);
                                                row.Add("Attribute", nm1.Characteristic);
                                                row.Add("Value", at[0].Value);
                                                row.Add("Mandatory", nm1.Mandatory == "Yes" ? "M" : "O");
                                                row.Add("Sequence", nm1.Squence);
                                                if (!string.IsNullOrEmpty(exchaFilledRate))
                                                    row.Add("Existing Filled Rate", exchaFilledRate);
                                                else
                                                    row.Add("Existing Filled Rate", "");
                                                if (!string.IsNullOrEmpty(chaFilledRate))
                                                    row.Add("Mandatory Filled Rate", mandFilledRate);
                                                else
                                                    row.Add("Mandatory Filled Rate", "");
                                                if (!string.IsNullOrEmpty(chaFilledRate))
                                                    row.Add("New Filled Rate", chaFilledRate);
                                                else
                                                    row.Add("New Filled Rate", "");
                                                if (code.AssetImages != null)
                                                {
                                                    if (code.AssetImages.NamePlateText != null)
                                                    {
                                                        row.Add("Nameplate Text", code.AssetImages.NamePlateText[0]);
                                                    }
                                                    else
                                                    {
                                                        row.Add("Nameplate Text", "");
                                                    }
                                                }
                                                else
                                                {
                                                    row.Add("Nameplate Text", "");
                                                }

                                                row.Add("Soureurl", code.Soureurl);
                                                row.Add("PV Remarks", code.Remarks);
                                                row.Add("Rework Remarks", code.Rework_Remarks);
                                                row.Add("MissingValue", code.MissingValue);
                                                row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                                                row.Add("QC", code.Review != null ? code.Review.Name : "");
                                                if (code.ItemStatus == -1)
                                                {
                                                    row.Add("ItemStatus", "Clarification");
                                                }
                                                else if (code.ItemStatus == 0)
                                                {
                                                    row.Add("ItemStatus", "PV Not Assigned");
                                                }
                                                else if (code.ItemStatus == 1)
                                                {
                                                    row.Add("ItemStatus", "PV Pending");
                                                }
                                                else if (code.ItemStatus == 2)
                                                {
                                                    row.Add("ItemStatus", "PV Completed");
                                                }
                                                else if (code.ItemStatus == 3)
                                                {
                                                    row.Add("ItemStatus", "Catalogue Saved");
                                                }
                                                else if (code.ItemStatus == 4)
                                                {
                                                    row.Add("ItemStatus", "Catalogue Submit");
                                                }
                                                else if (code.ItemStatus == 5)
                                                {
                                                    row.Add("ItemStatus", "QC Saved");
                                                }
                                                else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                                                {
                                                    row.Add("ItemStatus", "Released-Delivered");
                                                }
                                                else
                                                {
                                                    row.Add("ItemStatus", "Released");
                                                }
                                                rows.Add(row);
                                            }
                                            else
                                            {
                                                row = new Dictionary<string, object>();
                                                row.Add("Unique ID", code.UniqueId);
                                                row.Add("Asset NO", code.AssetNo);
                                                row.Add("Equipment Description", code.Description);
                                                row.Add("Noun", code.Noun);
                                                row.Add("Modifier", code.Modifier);
                                                row.Add("Shortdesc", code.Equipment_Short);
                                                row.Add("Longdesc", code.Equipment_Long);
                                                row.Add("PDesc", nm1.PDesc);
                                                row.Add("Attribute ID", nm1.Abbrivation);
                                                row.Add("Attribute", nm1.Characteristic);
                                                row.Add("Value", "");
                                                row.Add("UOM", "");
                                                row.Add("Mandatory", nm1.Mandatory == "Yes" ? "M" : "O");
                                                row.Add("Sequence", nm1.Squence);
                                                if (!string.IsNullOrEmpty(exchaFilledRate))
                                                    row.Add("Existing Filled Rate", exchaFilledRate);
                                                else
                                                    row.Add("Existing Filled Rate", "");
                                                if (!string.IsNullOrEmpty(chaFilledRate))
                                                    row.Add("Mandatory Filled Rate", mandFilledRate);
                                                else
                                                    row.Add("Mandatory Filled Rate", "");
                                                if (!string.IsNullOrEmpty(chaFilledRate))
                                                    row.Add("New Filled Rate", chaFilledRate);
                                                else
                                                    row.Add("New Filled Rate", "");
                                                row.Add("Soureurl", code.Soureurl);
                                                row.Add("PV Remarks", code.Remarks);
                                                row.Add("Rework Remarks", code.Rework_Remarks);
                                                row.Add("MissingValue", code.MissingValue);
                                                row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                                                row.Add("QC", code.Review != null ? code.Review.Name : "");
                                                if (code.ItemStatus == -1)
                                                {
                                                    row.Add("ItemStatus", "Clarification");
                                                }
                                                else if (code.ItemStatus == 0)
                                                {
                                                    row.Add("ItemStatus", "PV Not Assigned");
                                                }
                                                else if (code.ItemStatus == 1)
                                                {
                                                    row.Add("ItemStatus", "PV Pending");
                                                }
                                                else if (code.ItemStatus == 2)
                                                {
                                                    row.Add("ItemStatus", "PV Completed");
                                                }
                                                else if (code.ItemStatus == 3)
                                                {
                                                    row.Add("ItemStatus", "Catalogue Saved");
                                                }
                                                else if (code.ItemStatus == 4)
                                                {
                                                    row.Add("ItemStatus", "Catalogue Submit");
                                                }
                                                else if (code.ItemStatus == 5)
                                                {
                                                    row.Add("ItemStatus", "QC Saved");
                                                }
                                                else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                                                {
                                                    row.Add("ItemStatus", "Released-Delivered");
                                                }
                                                else
                                                {
                                                    row.Add("ItemStatus", "Released");
                                                }
                                                rows.Add(row);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        row = new Dictionary<string, object>();
                                        row.Add("Unique ID", code.UniqueId);
                                        row.Add("Asset NO", code.AssetNo);
                                        row.Add("Equipment Description", code.Description);
                                        row.Add("Noun", code.Noun);
                                        row.Add("Modifier", code.Modifier);
                                        row.Add("Shortdesc", code.Equipment_Short);
                                        row.Add("Longdesc", code.Equipment_Long);
                                        row.Add("PDesc", nm1.PDesc);
                                        row.Add("Attribute ID", nm1.Abbrivation);
                                        row.Add("Attribute", nm1.Characteristic);
                                        row.Add("Value", "");
                                        row.Add("Mandatory", nm1.Mandatory == "Yes" ? "M" : "O");
                                        row.Add("Sequence", nm1.Squence);
                                        if (!string.IsNullOrEmpty(exchaFilledRate))
                                            row.Add("Existing Filled Rate", exchaFilledRate);
                                        else
                                            row.Add("Existing Filled Rate", "");
                                        if (!string.IsNullOrEmpty(chaFilledRate))
                                            row.Add("Mandatory Filled Rate", mandFilledRate);
                                        else
                                            row.Add("Mandatory Filled Rate", "");
                                        if (!string.IsNullOrEmpty(chaFilledRate))
                                            row.Add("New Filled Rate", chaFilledRate);
                                        else
                                            row.Add("New Filled Rate", "");
                                        if (code.AssetImages != null)
                                        {
                                            if (code.AssetImages.NamePlateText != null)
                                            {
                                                row.Add("Nameplate Text", code.AssetImages.NamePlateText[0]);
                                            }
                                            else
                                            {
                                                row.Add("Nameplate Text", "");
                                            }
                                        }
                                        else
                                        {
                                            row.Add("Nameplate Text", "");
                                        }
                                        row.Add("Soureurl", code.Soureurl);
                                        row.Add("Remarks", code.Remarks);
                                        row.Add("MissingValue", code.MissingValue);
                                        row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                                        row.Add("QC", code.Review != null ? code.Review.Name : "");
                                        if (code.ItemStatus == -1)
                                        {
                                            row.Add("ItemStatus", "Clarification");
                                        }
                                        else if (code.ItemStatus == 0)
                                        {
                                            row.Add("ItemStatus", "PV Not Assigned");
                                        }
                                        else if (code.ItemStatus == 1)
                                        {
                                            row.Add("ItemStatus", "PV Pending");
                                        }
                                        else if (code.ItemStatus == 2)
                                        {
                                            row.Add("ItemStatus", "PV Completed");
                                        }
                                        else if (code.ItemStatus == 3)
                                        {
                                            row.Add("ItemStatus", "Catalogue Saved");
                                        }
                                        else if (code.ItemStatus == 4)
                                        {
                                            row.Add("ItemStatus", "Catalogue Submit");
                                        }
                                        else if (code.ItemStatus == 5)
                                        {
                                            row.Add("ItemStatus", "QC Saved");
                                        }
                                        else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                                        {
                                            row.Add("ItemStatus", "Released-Delivered");
                                        }
                                        else
                                        {
                                            row.Add("ItemStatus", "Released");
                                        }
                                        rows.Add(row);
                                    }

                                }

                            }
                            else
                            {
                                row = new Dictionary<string, object>();
                                row.Add("Unique ID", code.UniqueId);
                                row.Add("Asset NO", code.AssetNo);
                                row.Add("Equipment Description", code.Description);
                                row.Add("Noun", "");
                                row.Add("Modifier", "");
                                row.Add("Shortdesc", "");
                                row.Add("Longdesc", "");
                                row.Add("PDesc", "");
                                row.Add("Attribute ID", "");
                                row.Add("Attribute", "");
                                row.Add("Value", "");
                                row.Add("Mandatory", "");
                                row.Add("Sequence", "");
                                row.Add("Existing Filled Rate", "");
                                row.Add("Mandatory Filled Rate", "");
                                row.Add("New Filled Rate", "");
                                row.Add("Unspsc", "");
                                if (code.AssetImages != null)
                                {
                                    if (code.AssetImages.NamePlateText != null)
                                    {
                                        row.Add("Nameplate Text", code.AssetImages.NamePlateText[0]);
                                    }
                                    else
                                    {
                                        row.Add("Nameplate Text", "");
                                    }
                                }
                                else
                                {
                                    row.Add("Nameplate Text", "");
                                }
                                row.Add("Soureurl", code.Soureurl);
                                row.Add("PV Remarks", code.Remarks);
                                row.Add("Rework Remarks", code.Rework_Remarks);
                                row.Add("MissingValue", code.MissingValue);
                                row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                                row.Add("QC", code.Review != null ? code.Review.Name : "");
                                if (code.ItemStatus == -1)
                                {
                                    row.Add("ItemStatus", "Clarification");
                                }
                                else if (code.ItemStatus == 0)
                                {
                                    row.Add("ItemStatus", "PV Not Assigned");
                                }
                                else if (code.ItemStatus == 1)
                                {
                                    row.Add("ItemStatus", "PV Pending");
                                }
                                else if (code.ItemStatus == 2)
                                {
                                    row.Add("ItemStatus", "PV Completed");
                                }
                                else if (code.ItemStatus == 3)
                                {
                                    row.Add("ItemStatus", "Catalogue Saved");
                                }
                                else if (code.ItemStatus == 4)
                                {
                                    row.Add("ItemStatus", "Catalogue Submit");
                                }
                                else if (code.ItemStatus == 5)
                                {
                                    row.Add("ItemStatus", "QC Saved");
                                }
                                else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                                {
                                    row.Add("ItemStatus", "Released-Delivered");
                                }
                                else
                                {
                                    row.Add("ItemStatus", "Released");
                                }
                                rows.Add(row);
                            }
                        }
                        else
                        {
                            row = new Dictionary<string, object>();
                            row.Add("Unique ID", code.UniqueId);
                            row.Add("Asset NO", code.AssetNo);
                            row.Add("Equipment Description", code.Description);
                            row.Add("Noun", "");
                            row.Add("Modifier", "");
                            row.Add("Shortdesc", "");
                            row.Add("Longdesc", "");
                            row.Add("PDesc", "");
                            row.Add("Attribute ID", "");
                            row.Add("Attribute", "");
                            row.Add("Value", "");
                            row.Add("Mandatory", "");
                            row.Add("Sequence", "");
                            row.Add("Existing Filled Rate", "");
                            row.Add("Mandatory Filled Rate", "");
                            row.Add("New Filled Rate", "");
                            if (code.AssetImages != null)
                            {
                                if (code.AssetImages.NamePlateText != null)
                                {
                                    row.Add("Nameplate Text", code.AssetImages.NamePlateText[0]);
                                }
                                else
                                {
                                    row.Add("Nameplate Text", "");
                                }
                            }
                            else
                            {
                                row.Add("Nameplate Text", "");
                            }
                            row.Add("Soureurl", code.Soureurl);
                            row.Add("PV Remarks", code.Remarks);
                            row.Add("Rework Remarks", code.Rework_Remarks);
                            row.Add("MissingValue", code.MissingValue);
                            row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                            row.Add("QC", code.Review != null ? code.Review.Name : "");
                            row.Add("ItemStatus", code.ItemStatus == 0 ?
                                                  (code.ItemStatus == 1 ?
                                                  (code.ItemStatus == 2 ?
                                                  (code.ItemStatus == 5 ?
                                                  (code.ItemStatus == 3 ?
                                                  (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category)) ?
                                                  "Released-Delivered"
                                                  : "QC Submit"
                                                  : "QC Saved")
                                                  : "QA Saved")
                                                  : "Catalogue Submit")
                                                  : "Saved")
                                                  : "Not Saved");
                            rows.Add(row);
                        }
                    }
                    else
                    {
                        row = new Dictionary<string, object>();
                        row.Add("Unique ID", code.UniqueId);
                        row.Add("Asset NO", code.AssetNo);
                        row.Add("Equipment Description", code.Description);
                        row.Add("Noun", "");
                        row.Add("Modifier", "");
                        row.Add("Shortdesc", "");
                        row.Add("Longdesc", "");
                        row.Add("PDesc", "");
                        row.Add("Attribute ID", "");
                        row.Add("Attribute", "");
                        row.Add("Value", "");
                        row.Add("Mandatory", "");
                        row.Add("Sequence", "");
                        row.Add("Existing Filled Rate", "");
                        row.Add("Mandatory Filled Rate", "");
                        row.Add("New Filled Rate", "");
                        if (code.AssetImages != null)
                        {
                            if (code.AssetImages.NamePlateText != null)
                            {
                                row.Add("Nameplate Text", code.AssetImages.NamePlateText[0]);
                            }
                            else
                            {
                                row.Add("Nameplate Text", "");
                            }
                        }
                        else
                        {
                            row.Add("Nameplate Text", "");
                        }
                        row.Add("Soureurl", code.Soureurl);
                        row.Add("PV Remarks", code.Remarks);
                        row.Add("Rework Remarks", code.Rework_Remarks);
                        row.Add("MissingValue", code.MissingValue);
                        row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                        row.Add("QC", code.Review != null ? code.Review.Name : "");
                        row.Add("ItemStatus", code.ItemStatus == 0 ?
                                              (code.ItemStatus == 1 ?
                                              (code.ItemStatus == 2 ?
                                              (code.ItemStatus == 5 ?
                                              (code.ItemStatus == 3 ?
                                              (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category)) ?
                                              "Released-Delivered"
                                              : "QC Submit"
                                              : "QC Saved")
                                              : "QA Saved")
                                              : "Catalogue Submit")
                                              : "Saved")
                                              : "Not Saved");
                        rows.Add(row);
                    }
                }
                else
                {
                    row = new Dictionary<string, object>();
                    row.Add("Unique ID", code.UniqueId);
                    row.Add("Asset NO", code.AssetNo);
                    row.Add("Noun", string.IsNullOrEmpty(code.Noun) ? "" : code.Noun);
                    row.Add("Modifier", string.IsNullOrEmpty(code.Modifier) ? "" : code.Modifier);
                    row.Add("Existing Filled Rate", "");
                    row.Add("Mandatory Filled Rate", "");
                    row.Add("New Filled Rate", "");
                    row.Add("PV Remarks", code.Remarks);
                    row.Add("Rework Remarks", code.Rework_Remarks);
                    row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                    row.Add("QC", code.Review != null ? code.Review.Name : "");
                    if (code.ItemStatus == -1)
                    {
                        row.Add("ItemStatus", "Clarification");
                    }
                    else if (code.ItemStatus == 0)
                    {
                        row.Add("ItemStatus", "PV Not Assigned");
                    }
                    else if (code.ItemStatus == 1)
                    {
                        row.Add("ItemStatus", "PV Pending");
                    }
                    else if (code.ItemStatus == 2)
                    {
                        row.Add("ItemStatus", "PV Completed");
                    }
                    else if (code.ItemStatus == 3)
                    {
                        row.Add("ItemStatus", "Catalogue Saved");
                    }
                    else if (code.ItemStatus == 4)
                    {
                        row.Add("ItemStatus", "Catalogue Submit");
                    }
                    else if (code.ItemStatus == 5)
                    {
                        row.Add("ItemStatus", "QC Saved");
                    }
                    else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                    {
                        row.Add("ItemStatus", "Released-Delivered");
                    }
                    else
                    {
                        row.Add("ItemStatus", "Released");
                    }

                    row.Add("QC-UpdatedOn", code.Review != null ? code.Review.UpdatedOn.ToString() : "");
                    rows.Add(row);

                }
            }
            return rows ?? new List<Dictionary<string, object>>();
        }
        public IEnumerable<Prosol_AssetMaster> trackmulticodelist(string codestr)
        {
            //  string[] search_field = { "Itemcode", "Legacy", "Noun", "Modifier", "Legacy2" };
            // var fields = Fields.Include(search_field).Exclude("_id");
            var query = Query.Or(Query.EQ("UniqueId", codestr), (Query.EQ("AssetNo", codestr)));
            var getdata = _assetRepository.FindAll(query).ToList();
            return getdata;
        }
        public List<Dictionary<string, object>> Downloaddatacodes(string[] code_split)
        {
            try
            {
                var datalist = new List<Prosol_AssetMaster>();
                //foreach (var code in code_split)
                //{
                //    var data = new Prosol_AssetMaster();
                //    string[] str = { "UniqueId", "AssetNo", "Description", "Noun", "Modifier", "Equipment_Short", "Equipment_Long", "Characteristics", "AssetImages", "Soureurl", "Remarks", "Rework_Remarks", "MissingValue", "Catalogue", "Review", "ItemStatus" };
                //    var fields = Fields.Include(str);
                //    var query = Query.Or(Query.EQ("UniqueId", code), Query.EQ("AssetNo", code));
                //    data = _assetRepository.FindOne(query);
                //    datalist.Add(data);
                //}
                string[] str = { "UniqueId", "AssetNo", "Description", "Noun", "Modifier", "Equipment_Short", "Equipment_Long", "Characteristics", "AssetImages", "Soureurl", "Remarks", "Rework_Remarks", "MissingValue", "Catalogue", "Review", "ItemStatus" };
                var fields = Fields.Include(str);
                var Qry = Query.Or(Query.In("UniqueId", new BsonArray(code_split)), Query.In("AssetNo", new BsonArray(code_split)));
                datalist = _assetRepository.FindAll(Qry).ToList();

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
                Dictionary<string, object> rowh;

                foreach (var code in datalist)
                {
                    if (code != null)
                    {
                        //    var ven = code.Vendorsuppliers.Where(x => x.s == 1).ToList();
                        //     var Charateristics = code.Characteristics.OrderBy(x => x.Squence).ToList();
                        var chaQry = Query.EQ("UniqueId", code.UniqueId);
                        var nmChar = _AssetattriRepository.FindOne(chaQry);
                        var Charateristics = code.Characteristics;
                        if (nmChar != null)
                        {
                            if (!string.IsNullOrEmpty(nmChar.Noun) && !string.IsNullOrEmpty(nmChar.Modifier))
                            {
                                var QR = Query.And(Query.EQ("Noun", nmChar.Noun), Query.EQ("Modifier", nmChar.Modifier), Query.EQ("Definition", "Equ"));
                                var mm = _CharateristicRepository.FindAll(QR).ToList();
                                var nm = mm.OrderBy(x => x.Squence).ToList();
                                if (nm.Count > 0)
                                {
                                    //string NMUnspsc = "";
                                    //var uns = Query.And(Query.EQ("Noun", code.Noun), (Query.EQ("Modifier", code.Modifier)));
                                    //var unspc = _unspsclistRepository.FindOne(uns);
                                    //if (unspc != null)
                                    //{
                                    //    if (!String.IsNullOrEmpty(unspc.Commodity))
                                    //        NMUnspsc = unspc.Commodity;
                                    //    else NMUnspsc = unspc.Class;
                                    //}
                                    //if(nmChar.exCharacterisitics != null)
                                    //    code.Characteristics = nmChar.exCharacterisitics;
                                    //else
                                    //    code.Characteristics = new List<Asset_AttributeList>();
                                    code.Characteristics = nmChar.Characterisitics;
                                    code.Noun = nmChar.Noun;
                                    code.Modifier = nmChar.Modifier;
                                    Charateristics = code.Characteristics;
                                    var exCharateristics = new List<Asset_AttributeList>();
                                    if (nmChar.exCharacterisitics != null)
                                        exCharateristics = nmChar.exCharacterisitics;


                                    foreach (var nm1 in nm)
                                    {
                                        //if (code.UniqueId == "ADE096552")
                                        //{

                                        //}
                                        int chaCnt = 0;
                                        if (Charateristics != null)
                                            chaCnt = Charateristics.Count();
                                        int filledCha = Charateristics.Count(g => g.Value != "");
                                        double chaFilledRatePer = (double)filledCha / chaCnt * 100;
                                        string chaFilledRate = chaFilledRatePer.ToString("F2") + " %";

                                        int exchaCnt = 0;
                                        if (exCharateristics != null)
                                            exchaCnt = exCharateristics.Count();
                                        int filledexCha = exCharateristics.Count(g => g.Value != "");
                                        double exchaFilledRatePer = (double)filledexCha / exchaCnt * 100;
                                        string exchaFilledRate = exchaFilledRatePer.ToString("F2") + " %";


                                        var mandCharc = new List<Asset_AttributeList>();
                                        foreach (var cha in Charateristics)
                                        {
                                            var mand = new Asset_AttributeList();
                                            //var dic = _CharateristicService.GetCharacteristicvalues(cha.Characteristic, classification.Noun, classification.Modifier);
                                            var queryyy = Query.And(Query.EQ("Noun", code.Noun), Query.EQ("Modifier", code.Modifier), Query.EQ("Characteristic", cha.Characteristic));
                                            var result = _CharateristicRepository.FindOne(queryyy);
                                            if (result != null)
                                            {
                                                mand.Characteristic = cha.Characteristic;
                                                mand.Value = cha.Value;
                                                mand.Abbrevated = cha.Abbrevated;
                                                mand.UomMandatory = result.Mandatory;
                                            }
                                            mandCharc.Add(mand);
                                        }

                                        int mandCnt = mandCharc.Count();
                                        int MandCha = mandCharc.Count(g => g.UomMandatory == "Yes");
                                        int filledMandCha = mandCharc.Count(g => g.UomMandatory == "Yes" && g.Value != "");
                                        double mandFilledRatePer = (double)filledMandCha / MandCha * 100;
                                        string mandFilledRate = mandFilledRatePer.ToString("F2") + " %";


                                        if (exchaFilledRate == "NaN %")
                                        {
                                            exchaFilledRate = "0.00 %";
                                        }
                                        if (chaFilledRate == "NaN %")
                                        {
                                            chaFilledRate = "0.00 %";
                                        }
                                        if (mandFilledRate == "NaN %")
                                        {
                                            mandFilledRate = "0.00 %";
                                        }

                                        if (Charateristics != null && Charateristics.Count > 0)
                                        {

                                            // foreach (var at in Charateristics)
                                            // {

                                            if (nm1.Noun == code.Noun && nm1.Modifier == code.Modifier && !string.IsNullOrEmpty(nm1.Characteristic))
                                            {
                                                var at = code.Characteristics.Where(x => x.Characteristic == nm1.Characteristic).ToList();
                                                if (at.Count > 0)
                                                {
                                                    row = new Dictionary<string, object>();

                                                    //row.Add("Created On", code.CreatedOn);
                                                    row.Add("Unique ID", code.UniqueId);
                                                    row.Add("Asset NO", code.AssetNo);
                                                    row.Add("Equipment Description", code.Description);
                                                    //row.Add("L-Legacy", code.Legacy2);
                                                    //   row.Add("ML-Missingvalue", code.Legacy2);
                                                    row.Add("Noun", nm1.Noun);
                                                    row.Add("Modifier", nm1.Modifier);
                                                    //row.Add("Labelshort", code.Shortdesc);
                                                    row.Add("Shortdesc", code.Equipment_Short);
                                                    row.Add("Longdesc", code.Equipment_Long);
                                                    row.Add("Attribute", nm1.Characteristic);
                                                    row.Add("Value", at[0].Value);
                                                    //row.Add("UOM", at[0].UOM);
                                                    row.Add("Mandatory", nm1.Mandatory == "Yes" ? "M" : "O");
                                                    row.Add("Sequence", nm1.Squence);


                                                    if (!string.IsNullOrEmpty(exchaFilledRate))
                                                        row.Add("Existing Filled Rate", exchaFilledRate);
                                                    else
                                                        row.Add("Existing Filled Rate", "");
                                                    if (!string.IsNullOrEmpty(chaFilledRate))
                                                        row.Add("Mandatory Filled Rate", mandFilledRate);
                                                    else
                                                        row.Add("Mandatory Filled Rate", "");
                                                    if (!string.IsNullOrEmpty(chaFilledRate))
                                                        row.Add("New Filled Rate", chaFilledRate);
                                                    else
                                                        row.Add("New Filled Rate", "");
                                                    //row.Add("Additionalinfo", code.Additionalinfo);
                                                    //if (!String.IsNullOrEmpty(code.Unspsc))
                                                    //{
                                                    //    row.Add("Unspsc", code.Unspsc);
                                                    //}
                                                    //else row.Add("Unspsc", NMUnspsc);

                                                    if (code.AssetImages != null)
                                                    {
                                                        if (code.AssetImages.NamePlateText != null)
                                                        {
                                                            row.Add("Nameplate Text", code.AssetImages.NamePlateText[0]);
                                                        }
                                                        else
                                                        {
                                                            row.Add("Nameplate Text", "");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        row.Add("Nameplate Text", "");
                                                    }

                                                    row.Add("Soureurl", code.Soureurl);
                                                    //if (code.Equipment != null)
                                                    //{
                                                    //    row.Add("EQ_Name", code.Equipment.Name);
                                                    //    row.Add("EQ_Name_s", code.Equipment.ENS);
                                                    //    row.Add("EQ_Manf", code.Equipment.Manufacturer);
                                                    //    row.Add("EQ_MNo", code.Equipment.Modelno);
                                                    //    row.Add("EQ_MNo_s", code.Equipment.ENS);
                                                    //    row.Add("EQ_TNo", code.Equipment.Tagno);
                                                    //    row.Add("EQ_SNo", code.Equipment.Serialno);
                                                    //    row.Add("EQ_Add", code.Equipment.Additionalinfo);
                                                    //}
                                                    //else
                                                    //{
                                                    //    row.Add("EQ_Name", "");
                                                    //    row.Add("EQ_Name_s", "");
                                                    //    row.Add("EQ_Manf", "");
                                                    //    row.Add("EQ_MNo", "");
                                                    //    row.Add("EQ_MNo_s", "");
                                                    //    row.Add("EQ_TNo", "");
                                                    //    row.Add("EQ_SNo", "");
                                                    //    row.Add("EQ_Add", "");
                                                    //}
                                                    row.Add("PV Remarks", code.Remarks);
                                                    row.Add("Rework Remarks", code.Rework_Remarks);
                                                    //row.Add("QC-Remarks", code.RevRemarks);
                                                    row.Add("MissingValue", code.MissingValue);
                                                    row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                                                    row.Add("QC", code.Review != null ? code.Review.Name : "");
                                                    if (code.ItemStatus == -1)
                                                    {
                                                        row.Add("ItemStatus", "Clarification");
                                                    }
                                                    else if (code.ItemStatus == 0)
                                                    {
                                                        row.Add("ItemStatus", "PV Not Assigned");
                                                    }
                                                    else if (code.ItemStatus == 1)
                                                    {
                                                        row.Add("ItemStatus", "PV Pending");
                                                    }
                                                    else if (code.ItemStatus == 2)
                                                    {
                                                        row.Add("ItemStatus", "PV Completed");
                                                    }
                                                    else if (code.ItemStatus == 3)
                                                    {
                                                        row.Add("ItemStatus", "Catalogue Saved");
                                                    }
                                                    else if (code.ItemStatus == 4)
                                                    {
                                                        row.Add("ItemStatus", "Catalogue Submit");
                                                    }
                                                    else if (code.ItemStatus == 5)
                                                    {
                                                        row.Add("ItemStatus", "QC Saved");
                                                    }
                                                    else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                                                    {
                                                        row.Add("ItemStatus", "Released-Delivered");
                                                    }
                                                    else
                                                    {
                                                        row.Add("ItemStatus", "Released");
                                                    }
                                                    rows.Add(row);
                                                    //break;
                                                }
                                                else
                                                {
                                                    row = new Dictionary<string, object>();
                                                    //row.Add("Created On", code.CreatedOn);
                                                    row.Add("Unique ID", code.UniqueId);
                                                    row.Add("Asset NO", code.AssetNo);
                                                    row.Add("Equipment Description", code.Description);
                                                    //row.Add("L-Legacy", code.Legacy2);
                                                    //   row.Add("ML-Missingvalue", code.Legacy2);
                                                    row.Add("Noun", code.Noun);
                                                    row.Add("Modifier", code.Modifier);
                                                    //row.Add("Labelshort", code.Shortdesc);
                                                    row.Add("Shortdesc", code.Equipment_Short);
                                                    row.Add("Longdesc", code.Equipment_Long);
                                                    row.Add("Attribute", nm1.Characteristic);
                                                    row.Add("Value", "");
                                                    row.Add("UOM", "");
                                                    row.Add("Mandatory", nm1.Mandatory == "Yes" ? "M" : "O");
                                                    row.Add("Sequence", nm1.Squence);

                                                    if (!string.IsNullOrEmpty(exchaFilledRate))
                                                        row.Add("Existing Filled Rate", exchaFilledRate);
                                                    else
                                                        row.Add("Existing Filled Rate", "");
                                                    if (!string.IsNullOrEmpty(chaFilledRate))
                                                        row.Add("Mandatory Filled Rate", mandFilledRate);
                                                    else
                                                        row.Add("Mandatory Filled Rate", "");
                                                    if (!string.IsNullOrEmpty(chaFilledRate))
                                                        row.Add("New Filled Rate", chaFilledRate);
                                                    else
                                                        row.Add("New Filled Rate", "");
                                                    //row.Add("Additionalinfo", code.Additionalinfo);
                                                    //if (!String.IsNullOrEmpty(code.Unspsc))
                                                    //{
                                                    //    row.Add("Unspsc", code.Unspsc);
                                                    //}
                                                    //else row.Add("Unspsc", NMUnspsc);

                                                    row.Add("Soureurl", code.Soureurl);
                                                    //if (code.Equipment != null)
                                                    //{
                                                    //    row.Add("EQ_Name", code.Equipment.Name);
                                                    //    row.Add("EQ_Name_s", code.Equipment.ENS);
                                                    //    row.Add("EQ_Manf", code.Equipment.Manufacturer);
                                                    //    row.Add("EQ_MNo", code.Equipment.Modelno);
                                                    //    row.Add("EQ_MNo_s", code.Equipment.ENS);
                                                    //    row.Add("EQ_TNo", code.Equipment.Tagno);
                                                    //    row.Add("EQ_SNo", code.Equipment.Serialno);
                                                    //    row.Add("EQ_Add", code.Equipment.Additionalinfo);
                                                    //}
                                                    //else
                                                    //{
                                                    //    row.Add("EQ_Name", "");
                                                    //    row.Add("EQ_Name_s", "");
                                                    //    row.Add("EQ_Manf", "");
                                                    //    row.Add("EQ_MNo", "");
                                                    //    row.Add("EQ_MNo_s", "");
                                                    //    row.Add("EQ_TNo", "");
                                                    //    row.Add("EQ_SNo", "");
                                                    //    row.Add("EQ_Add", "");
                                                    //}
                                                    row.Add("PV Remarks", code.Remarks);
                                                    row.Add("Rework Remarks", code.Rework_Remarks);
                                                    //row.Add("QC-Remarks", code.RevRemarks);
                                                    row.Add("MissingValue", code.MissingValue);
                                                    row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                                                    row.Add("QC", code.Review != null ? code.Review.Name : "");
                                                    if (code.ItemStatus == -1)
                                                    {
                                                        row.Add("ItemStatus", "Clarification");
                                                    }
                                                    else if (code.ItemStatus == 0)
                                                    {
                                                        row.Add("ItemStatus", "PV Not Assigned");
                                                    }
                                                    else if (code.ItemStatus == 1)
                                                    {
                                                        row.Add("ItemStatus", "PV Pending");
                                                    }
                                                    else if (code.ItemStatus == 2)
                                                    {
                                                        row.Add("ItemStatus", "PV Completed");
                                                    }
                                                    else if (code.ItemStatus == 3)
                                                    {
                                                        row.Add("ItemStatus", "Catalogue Saved");
                                                    }
                                                    else if (code.ItemStatus == 4)
                                                    {
                                                        row.Add("ItemStatus", "Catalogue Submit");
                                                    }
                                                    else if (code.ItemStatus == 5)
                                                    {
                                                        row.Add("ItemStatus", "QC Saved");
                                                    }
                                                    else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                                                    {
                                                        row.Add("ItemStatus", "Released-Delivered");
                                                    }
                                                    else
                                                    {
                                                        row.Add("ItemStatus", "Released");
                                                    }
                                                    rows.Add(row);
                                                }
                                            }

                                            // }
                                        }
                                        else
                                        {

                                            row = new Dictionary<string, object>();
                                            //row.Add("Created On", code.CreatedOn);
                                            row.Add("Unique ID", code.UniqueId);
                                            row.Add("Asset NO", code.AssetNo);
                                            row.Add("Equipment Description", code.Description);
                                            //row.Add("L-Legacy", code.Legacy2);
                                            //   row.Add("ML-Missingvalue", code.Legacy2);
                                            row.Add("Noun", code.Noun);
                                            row.Add("Modifier", code.Modifier);
                                            //row.Add("Labelshort", code.Shortdesc);
                                            row.Add("Shortdesc", code.Equipment_Short);
                                            row.Add("Longdesc", code.Equipment_Long);
                                            row.Add("Attribute", nm1.Characteristic);
                                            row.Add("Value", "");
                                            //row.Add("UOM", "");
                                            row.Add("Mandatory", nm1.Mandatory == "Yes" ? "M" : "O");
                                            row.Add("Sequence", nm1.Squence);
                                            if (!string.IsNullOrEmpty(exchaFilledRate))
                                                row.Add("Existing Filled Rate", exchaFilledRate);
                                            else
                                                row.Add("Existing Filled Rate", "");
                                            if (!string.IsNullOrEmpty(chaFilledRate))
                                                row.Add("Mandatory Filled Rate", mandFilledRate);
                                            else
                                                row.Add("Mandatory Filled Rate", "");
                                            if (!string.IsNullOrEmpty(chaFilledRate))
                                                row.Add("New Filled Rate", chaFilledRate);
                                            else
                                                row.Add("New Filled Rate", "");
                                            //row.Add("Additionalinfo", code.Additionalinfo);

                                            //row.Add("Unspsc", NMUnspsc);


                                            if (code.AssetImages != null)
                                            {
                                                if (code.AssetImages.NamePlateText != null)
                                                {
                                                    row.Add("Nameplate Text", code.AssetImages.NamePlateText[0]);
                                                }
                                                else
                                                {
                                                    row.Add("Nameplate Text", "");
                                                }
                                            }
                                            else
                                            {
                                                row.Add("Nameplate Text", "");
                                            }

                                            row.Add("Soureurl", code.Soureurl);
                                            //if (code.Equipment != null)
                                            //{
                                            //    row.Add("EQ_Name", code.Equipment.Name);
                                            //    row.Add("EQ_Name_s", code.Equipment.ENS);
                                            //    row.Add("EQ_Manf", code.Equipment.Manufacturer);
                                            //    row.Add("EQ_MNo", code.Equipment.Modelno);
                                            //    row.Add("EQ_MNo_s", code.Equipment.ENS);
                                            //    row.Add("EQ_TNo", code.Equipment.Tagno);
                                            //    row.Add("EQ_SNo", code.Equipment.Serialno);
                                            //    row.Add("EQ_Add", code.Equipment.Additionalinfo);
                                            //}
                                            //row.Add("Alt_Unspsc", code.Referenceno);
                                            //row.Add("Status", code.Application);
                                            //row.Add("Soureurl", code.Soureurl);
                                            //row.Add("MFR_Name", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].Name) ? ven[0].Name : "");
                                            //row.Add("MFR_PartNo", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].RefNo) ? ven[0].RefNo : "");
                                            //row.Add("Batch", code.batch);
                                            row.Add("Remarks", code.Remarks);
                                            //row.Add("QC-Remarks", code.RevRemarks);
                                            row.Add("MissingValue", code.MissingValue);
                                            row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                                            row.Add("QC", code.Review != null ? code.Review.Name : "");
                                            if (code.ItemStatus == -1)
                                            {
                                                row.Add("ItemStatus", "Clarification");
                                            }
                                            else if (code.ItemStatus == 0)
                                            {
                                                row.Add("ItemStatus", "PV Not Assigned");
                                            }
                                            else if (code.ItemStatus == 1)
                                            {
                                                row.Add("ItemStatus", "PV Pending");
                                            }
                                            else if (code.ItemStatus == 2)
                                            {
                                                row.Add("ItemStatus", "PV Completed");
                                            }
                                            else if (code.ItemStatus == 3)
                                            {
                                                row.Add("ItemStatus", "Catalogue Saved");
                                            }
                                            else if (code.ItemStatus == 4)
                                            {
                                                row.Add("ItemStatus", "Catalogue Submit");
                                            }
                                            else if (code.ItemStatus == 5)
                                            {
                                                row.Add("ItemStatus", "QC Saved");
                                            }
                                            else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                                            {
                                                row.Add("ItemStatus", "Released-Delivered");
                                            }
                                            else
                                            {
                                                row.Add("ItemStatus", "Released");
                                            }
                                            rows.Add(row);
                                        }

                                    }

                                }
                                else
                                {
                                    row = new Dictionary<string, object>();
                                    //row.Add("Created On", code.CreatedOn);
                                    row.Add("Unique ID", code.UniqueId);
                                    row.Add("Asset NO", code.AssetNo);
                                    row.Add("Equipment Description", code.Description);
                                    //row.Add("L-Legacy", code.Legacy2);
                                    //   row.Add("ML-Missingvalue", code.Legacy2);
                                    //  row.Add("ML-Missingvalue", code.Legacy2);
                                    row.Add("Noun", "");
                                    row.Add("Modifier", "");
                                    //row.Add("Labelshort", "");
                                    row.Add("Shortdesc", "");
                                    row.Add("Longdesc", "");
                                    row.Add("Attribute", "");
                                    row.Add("Value", "");
                                    //row.Add("UOM", "");
                                    row.Add("Mandatory", "");
                                    row.Add("Sequence", "");
                                    row.Add("Existing Filled Rate", "");
                                    row.Add("Mandatory Filled Rate", "");
                                    row.Add("New Filled Rate", "");
                                    //row.Add("Additionalinfo", code.Additionalinfo);

                                    row.Add("Unspsc", "");

                                    if (code.AssetImages != null)
                                    {
                                        if (code.AssetImages.NamePlateText != null)
                                        {
                                            row.Add("Nameplate Text", code.AssetImages.NamePlateText[0]);
                                        }
                                        else
                                        {
                                            row.Add("Nameplate Text", "");
                                        }
                                    }
                                    else
                                    {
                                        row.Add("Nameplate Text", "");
                                    }
                                    row.Add("Soureurl", code.Soureurl);
                                    //if (code.Equipment != null)
                                    //{
                                    //    row.Add("EQ_Name", code.Equipment.Name);
                                    //    row.Add("EQ_Name_s", code.Equipment.ENS);
                                    //    row.Add("EQ_Manf", code.Equipment.Manufacturer);
                                    //    row.Add("EQ_MNo", code.Equipment.Modelno);
                                    //    row.Add("EQ_MNo_s", code.Equipment.ENS);
                                    //    row.Add("EQ_TNo", code.Equipment.Tagno);
                                    //    row.Add("EQ_SNo", code.Equipment.Serialno);
                                    //    row.Add("EQ_Add", code.Equipment.Additionalinfo);
                                    //}
                                    //row.Add("Alt_Unspsc", code.Referenceno);
                                    //row.Add("Status", code.Application);
                                    //row.Add("Soureurl", code.Soureurl);
                                    //row.Add("MFR_Name", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].Name) ? ven[0].Name : "");
                                    //row.Add("MFR_PartNo", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].RefNo) ? ven[0].RefNo : "");
                                    //row.Add("Batch", code.batch);
                                    row.Add("PV Remarks", code.Remarks);
                                    row.Add("Rework Remarks", code.Rework_Remarks);
                                    //row.Add("QC-Remarks", code.RevRemarks);
                                    row.Add("MissingValue", code.MissingValue);
                                    row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                                    row.Add("QC", code.Review != null ? code.Review.Name : "");
                                    if (code.ItemStatus == -1)
                                    {
                                        row.Add("ItemStatus", "Clarification");
                                    }
                                    else if (code.ItemStatus == 0)
                                    {
                                        row.Add("ItemStatus", "PV Not Assigned");
                                    }
                                    else if (code.ItemStatus == 1)
                                    {
                                        row.Add("ItemStatus", "PV Pending");
                                    }
                                    else if (code.ItemStatus == 2)
                                    {
                                        row.Add("ItemStatus", "PV Completed");
                                    }
                                    else if (code.ItemStatus == 3)
                                    {
                                        row.Add("ItemStatus", "Catalogue Saved");
                                    }
                                    else if (code.ItemStatus == 4)
                                    {
                                        row.Add("ItemStatus", "Catalogue Submit");
                                    }
                                    else if (code.ItemStatus == 5)
                                    {
                                        row.Add("ItemStatus", "QC Saved");
                                    }
                                    else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                                    {
                                        row.Add("ItemStatus", "Released-Delivered");
                                    }
                                    else
                                    {
                                        row.Add("ItemStatus", "Released");
                                    }
                                    rows.Add(row);
                                }
                            }
                            else
                            {
                                row = new Dictionary<string, object>();
                                //row.Add("Created On", code.CreatedOn);
                                row.Add("Unique ID", code.UniqueId);
                                row.Add("Asset NO", code.AssetNo);
                                row.Add("Equipment Description", code.Description);
                                //  row.Add("ML-Missingvalue", code.Legacy2);
                                row.Add("Noun", "");
                                row.Add("Modifier", "");
                                //row.Add("Labelshort", "");
                                row.Add("Shortdesc", "");
                                row.Add("Longdesc", "");
                                row.Add("Attribute", "");
                                row.Add("Value", "");
                                //row.Add("UOM", "");
                                row.Add("Mandatory", "");
                                row.Add("Sequence", "");
                                row.Add("Existing Filled Rate", "");
                                row.Add("Mandatory Filled Rate", "");
                                row.Add("New Filled Rate", "");
                                //row.Add("Additionalinfo", code.Additionalinfo);

                                //row.Add("Unspsc", "");

                                if (code.AssetImages != null)
                                {
                                    if (code.AssetImages.NamePlateText != null)
                                    {
                                        row.Add("Nameplate Text", code.AssetImages.NamePlateText[0]);
                                    }
                                    else
                                    {
                                        row.Add("Nameplate Text", "");
                                    }
                                }
                                else
                                {
                                    row.Add("Nameplate Text", "");
                                }
                                row.Add("Soureurl", code.Soureurl);
                                //if (code.Equipment != null)
                                //{
                                //    row.Add("EQ_Name", code.Equipment.Name);
                                //    row.Add("EQ_Name_s", code.Equipment.ENS);
                                //    row.Add("EQ_Manf", code.Equipment.Manufacturer);
                                //    row.Add("EQ_MNo", code.Equipment.Modelno);
                                //    row.Add("EQ_MNo_s", code.Equipment.ENS);
                                //    row.Add("EQ_TNo", code.Equipment.Tagno);
                                //    row.Add("EQ_SNo", code.Equipment.Serialno);
                                //    row.Add("EQ_Add", code.Equipment.Additionalinfo);
                                //}
                                //row.Add("Alt_Unspsc", code.Referenceno);
                                //row.Add("Status", code.Application);
                                //row.Add("Soureurl", code.Soureurl);
                                //row.Add("MFR_Name", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].Name) ? ven[0].Name : "");
                                //row.Add("MFR_PartNo", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].RefNo) ? ven[0].RefNo : "");
                                //row.Add("Batch", code.batch);
                                row.Add("PV Remarks", code.Remarks);
                                row.Add("Rework Remarks", code.Rework_Remarks);
                                //row.Add("QC-Remarks", code.RevRemarks);
                                row.Add("MissingValue", code.MissingValue);
                                row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                                row.Add("QC", code.Review != null ? code.Review.Name : "");
                                if (code.ItemStatus == -1)
                                {
                                    row.Add("ItemStatus", "Clarification");
                                }
                                else if (code.ItemStatus == 0)
                                {
                                    row.Add("ItemStatus", "PV Not Assigned");
                                }
                                else if (code.ItemStatus == 1)
                                {
                                    row.Add("ItemStatus", "PV Pending");
                                }
                                else if (code.ItemStatus == 2)
                                {
                                    row.Add("ItemStatus", "PV Completed");
                                }
                                else if (code.ItemStatus == 3)
                                {
                                    row.Add("ItemStatus", "Catalogue Saved");
                                }
                                else if (code.ItemStatus == 4)
                                {
                                    row.Add("ItemStatus", "Catalogue Submit");
                                }
                                else if (code.ItemStatus == 5)
                                {
                                    row.Add("ItemStatus", "QC Saved");
                                }
                                else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                                {
                                    row.Add("ItemStatus", "Released-Delivered");
                                }
                                else
                                {
                                    row.Add("ItemStatus", "Released");
                                }
                                rows.Add(row);
                            }
                        }
                        else
                        {
                            row = new Dictionary<string, object>();
                            //row.Add("Created On", code.CreatedOn);
                            row.Add("Unique ID", code.UniqueId);
                            row.Add("Asset NO", code.AssetNo);
                            row.Add("Equipment Description", code.Description);
                            //  row.Add("ML-Missingvalue", code.Legacy2);
                            row.Add("Noun", "");
                            row.Add("Modifier", "");
                            //row.Add("Labelshort", "");
                            row.Add("Shortdesc", "");
                            row.Add("Longdesc", "");
                            row.Add("Attribute", "");
                            row.Add("Value", "");
                            //row.Add("UOM", "");
                            row.Add("Mandatory", "");
                            row.Add("Sequence", "");
                            row.Add("Existing Filled Rate", "");
                            row.Add("Mandatory Filled Rate", "");
                            row.Add("New Filled Rate", "");
                            //row.Add("Additionalinfo", code.Additionalinfo);

                            //row.Add("Unspsc", "");

                            if (code.AssetImages != null)
                            {
                                if (code.AssetImages.NamePlateText != null)
                                {
                                    row.Add("Nameplate Text", code.AssetImages.NamePlateText[0]);
                                }
                                else
                                {
                                    row.Add("Nameplate Text", "");
                                }
                            }
                            else
                            {
                                row.Add("Nameplate Text", "");
                            }
                            row.Add("Soureurl", code.Soureurl);
                            row.Add("PV Remarks", code.Remarks);
                            row.Add("Rework Remarks", code.Rework_Remarks);
                            //row.Add("QC-Remarks", code.RevRemarks);
                            row.Add("MissingValue", code.MissingValue);
                            row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
                            row.Add("QC", code.Review != null ? code.Review.Name : "");
                            if (code.ItemStatus == -1)
                            {
                                row.Add("ItemStatus", "Clarification");
                            }
                            else if (code.ItemStatus == 0)
                            {
                                row.Add("ItemStatus", "PV Not Assigned");
                            }
                            else if (code.ItemStatus == 1)
                            {
                                row.Add("ItemStatus", "PV Pending");
                            }
                            else if (code.ItemStatus == 2)
                            {
                                row.Add("ItemStatus", "PV Completed");
                            }
                            else if (code.ItemStatus == 3)
                            {
                                row.Add("ItemStatus", "Catalogue Saved");
                            }
                            else if (code.ItemStatus == 4)
                            {
                                row.Add("ItemStatus", "Catalogue Submit");
                            }
                            else if (code.ItemStatus == 5)
                            {
                                row.Add("ItemStatus", "QC Saved");
                            }
                            else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
                            {
                                row.Add("ItemStatus", "Released-Delivered");
                            }
                            else
                            {
                                row.Add("ItemStatus", "Released");
                            }
                            rows.Add(row);
                        }
                    }
                }
                return rows;
            }
            catch (Exception e)
            {
                return null;
            }


        }

        ////Ex Attributes

        //public List<Dictionary<string, object>> Downloaddata(string username, string status)
        //{
        //    try
        //    {
        //        IMongoQuery query;
        //        var datalist = new List<Prosol_AssetMaster>();
        //        if (status == "cat")
        //        {
        //            //query = Query.And(Query.Or(Query.EQ("Catalogue.Name", username), Query.And(Query.EQ("Review.Name", username))), Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)));
        //            query = Query.Or(
        //            Query.And(
        //            Query.EQ("Catalogue.Name", username), Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3))
        //            ),
        //            Query.And(
        //            Query.EQ("Review.Name", username), Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 5))
        //            )
        //            //Query.And(
        //            //Query.EQ("Release.UserId", uId), Query.Or(Query.EQ("ItemStatus", 6), Query.EQ("ItemStatus", 7), Query.EQ("ItemStatus", 9))
        //            //)
        //            );
        //        }
        //        else if (status == "qc")
        //        {
        //            query = Query.And(Query.EQ("Review.Name", username), Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 5)));
        //            if (username == "Ramkumar")
        //            {
        //                query = Query.And(Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 5)));
        //            }

        //        }
        //        else if (status == "All")
        //        {
        //            query = Query.Or(Query.NE("Catalogue", BsonNull.Value), Query.GT("ItemStatus", 0));
        //        }
        //        else if (status == "QA")
        //        {
        //            query = Query.And(Query.EQ("Release.Name", username), Query.GTE("ItemStatus", 4), Query.EQ("category", BsonNull.Value));

        //        }
        //        else
        //        {
        //            //query = Query.And(Query.EQ("Review.Name", username), Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)));
        //            query = Query.GTE("ItemStatus", 8);
        //        }
        //        datalist = _assetRepository.FindAll(query).ToList();


        //        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //        Dictionary<string, object> row;
        //        List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
        //        Dictionary<string, object> rowh;

        //        foreach (var code in datalist)
        //        {
        //            //    var ven = code.Vendorsuppliers.Where(x => x.s == 1).ToList();
        //            //     var Charateristics = code.Characteristics.OrderBy(x => x.Squence).ToList();
        //            var chaQry = Query.EQ("UniqueId", code.UniqueId);
        //            var nmChar = _AssetattriRepository.FindOne(chaQry);
        //            var Charateristics = code.Characteristics;
        //            if (status != "All")
        //            {

        //                if (!string.IsNullOrEmpty(nmChar.Noun) && !string.IsNullOrEmpty(nmChar.Modifier))
        //                {
        //                    var QR = Query.And(Query.EQ("Noun", nmChar.Noun), Query.EQ("Modifier", nmChar.Modifier), Query.EQ("Definition", "Equ"));
        //                    var mm = _CharateristicRepository.FindAll(QR).ToList();
        //                    var nm = mm.OrderBy(x => x.Squence).ToList();
        //                    if (nm.Count > 0)
        //                    {
        //                        //string NMUnspsc = "";
        //                        //var uns = Query.And(Query.EQ("Noun", code.Noun), (Query.EQ("Modifier", code.Modifier)));
        //                        //var unspc = _unspsclistRepository.FindOne(uns);
        //                        //if (unspc != null)
        //                        //{
        //                        //    if (!String.IsNullOrEmpty(unspc.Commodity))
        //                        //        NMUnspsc = unspc.Commodity;
        //                        //    else NMUnspsc = unspc.Class;
        //                        //}
        //                        if (nmChar.exCharacterisitics != null)
        //                            code.Characteristics = nmChar.exCharacterisitics;
        //                        else
        //                            code.Characteristics = new List<Asset_AttributeList>();
        //                        //code.Characteristics = nmChar.Characterisitics;
        //                        code.Noun = nmChar.exNoun;
        //                        code.Modifier = nmChar.exModifier;
        //                        Charateristics = code.Characteristics;
        //                        var exCharateristics = new List<Asset_AttributeList>();
        //                        if (nmChar.exCharacterisitics != null)
        //                            exCharateristics = nmChar.exCharacterisitics;


        //                        foreach (var nm1 in Charateristics)
        //                        {
        //                            int chaCnt = 0;
        //                            if (Charateristics != null)
        //                                chaCnt = Charateristics.Count();
        //                            int filledCha = Charateristics.Count(g => g.Value != "");
        //                            double chaFilledRatePer = (double)filledCha / chaCnt * 100;
        //                            string chaFilledRate = chaFilledRatePer.ToString("F2") + " %";

        //                            int exchaCnt = 0;
        //                            if (exCharateristics != null)
        //                                exchaCnt = exCharateristics.Count();
        //                            int filledexCha = exCharateristics.Count(g => g.Value != "");
        //                            double exchaFilledRatePer = (double)filledexCha / exchaCnt * 100;
        //                            string exchaFilledRate = exchaFilledRatePer.ToString("F2") + " %";

        //                            var mandCharc = new List<Asset_AttributeList>();
        //                            foreach (var cha in Charateristics)
        //                            {
        //                                var mand = new Asset_AttributeList();
        //                                //var dic = _CharateristicService.GetCharacteristicvalues(cha.Characteristic, classification.Noun, classification.Modifier);
        //                                var queryyy = Query.And(Query.EQ("Noun", code.Noun), Query.EQ("Modifier", code.Modifier), Query.EQ("Characteristic", cha.Characteristic));
        //                                var result = _CharateristicRepository.FindOne(queryyy);
        //                                if (result != null)
        //                                {
        //                                    mand.Characteristic = cha.Characteristic;
        //                                    mand.Value = cha.Value;
        //                                    mand.Abbrevated = cha.Abbrevated;
        //                                    mand.UomMandatory = result.Mandatory;
        //                                }
        //                                mandCharc.Add(mand);
        //                            }

        //                            int mandCnt = mandCharc.Count();
        //                            int MandCha = mandCharc.Count(g => g.UomMandatory == "Yes");
        //                            int filledMandCha = mandCharc.Count(g => g.UomMandatory == "Yes" && g.Value != "");
        //                            double mandFilledRatePer = (double)filledMandCha / MandCha * 100;
        //                            string mandFilledRate = mandFilledRatePer.ToString("F2") + " %";


        //                            if (exchaFilledRate == "NaN %")
        //                            {
        //                                exchaFilledRate = "0.00 %";
        //                            }
        //                            if (chaFilledRate == "NaN %")
        //                            {
        //                                chaFilledRate = "0.00 %";
        //                            }
        //                            if (mandFilledRate == "NaN %")
        //                            {
        //                                mandFilledRate = "0.00 %";
        //                            }
        //                            if (Charateristics != null && Charateristics.Count > 0)
        //                            {

        //                                // foreach (var at in Charateristics)
        //                                // {

        //                                if (nmChar.exNoun == code.Noun && nmChar.exModifier == code.Modifier && !string.IsNullOrEmpty(nm1.Characteristic))
        //                                {
        //                                    var at = code.Characteristics.Where(x => x.Characteristic == nm1.Characteristic).ToList();
        //                                    if (at.Count > 0)
        //                                    {
        //                                        row = new Dictionary<string, object>();

        //                                        //row.Add("Created On", code.CreatedOn);
        //                                        row.Add("Unique ID", code.UniqueId);
        //                                        row.Add("Asset NO", code.AssetNo);
        //                                        row.Add("Equipment Description", code.Description);
        //                                        //row.Add("L-Legacy", code.Legacy2);
        //                                        //   row.Add("ML-Missingvalue", code.Legacy2);
        //                                        row.Add("Noun", nmChar.exNoun);
        //                                        row.Add("Modifier", nmChar.exModifier);
        //                                        //row.Add("Labelshort", code.Shortdesc);
        //                                        row.Add("Shortdesc", code.Equipment_Short);
        //                                        row.Add("Longdesc", code.Equipment_Long);
        //                                        row.Add("Attribute", nm1.Characteristic);
        //                                        row.Add("Value", at[0].Value);
        //                                        //row.Add("UOM", at[0].UOM);
        //                                        row.Add("Mandatory", "");
        //                                        row.Add("Sequence", nm1.Squence);


        //                                        if (!string.IsNullOrEmpty(exchaFilledRate))
        //                                            row.Add("Existing Filled Rate", exchaFilledRate);
        //                                        else
        //                                            row.Add("Existing Filled Rate", "");
        //                                        if (!string.IsNullOrEmpty(chaFilledRate))
        //                                            row.Add("Mandatory Filled Rate", mandFilledRate);
        //                                        else
        //                                            row.Add("Mandatory Filled Rate", "");
        //                                        if (!string.IsNullOrEmpty(chaFilledRate))
        //                                            row.Add("New Filled Rate", chaFilledRate);
        //                                        else
        //                                            row.Add("New Filled Rate", "");
        //                                        //row.Add("Additionalinfo", code.Additionalinfo);
        //                                        //if (!String.IsNullOrEmpty(code.Unspsc))
        //                                        //{
        //                                        //    row.Add("Unspsc", code.Unspsc);
        //                                        //}
        //                                        //else row.Add("Unspsc", NMUnspsc);

        //                                        if (code.AssetImages != null)
        //                                        {
        //                                            if (code.AssetImages.NamePlateText != null)
        //                                            {
        //                                                row.Add("Nameplate Text", code.AssetImages.NamePlateText[0]);
        //                                            }
        //                                            else
        //                                            {
        //                                                row.Add("Nameplate Text", "");
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            row.Add("Nameplate Text", "");
        //                                        }

        //                                        row.Add("Soureurl", code.Soureurl);
        //                                        //if (code.Equipment != null)
        //                                        //{
        //                                        //    row.Add("EQ_Name", code.Equipment.Name);
        //                                        //    row.Add("EQ_Name_s", code.Equipment.ENS);
        //                                        //    row.Add("EQ_Manf", code.Equipment.Manufacturer);
        //                                        //    row.Add("EQ_MNo", code.Equipment.Modelno);
        //                                        //    row.Add("EQ_MNo_s", code.Equipment.ENS);
        //                                        //    row.Add("EQ_TNo", code.Equipment.Tagno);
        //                                        //    row.Add("EQ_SNo", code.Equipment.Serialno);
        //                                        //    row.Add("EQ_Add", code.Equipment.Additionalinfo);
        //                                        //}
        //                                        //else
        //                                        //{
        //                                        //    row.Add("EQ_Name", "");
        //                                        //    row.Add("EQ_Name_s", "");
        //                                        //    row.Add("EQ_Manf", "");
        //                                        //    row.Add("EQ_MNo", "");
        //                                        //    row.Add("EQ_MNo_s", "");
        //                                        //    row.Add("EQ_TNo", "");
        //                                        //    row.Add("EQ_SNo", "");
        //                                        //    row.Add("EQ_Add", "");
        //                                        //}
        //                                        row.Add("PV Remarks", code.Remarks);
        //                                        row.Add("Rework Remarks", code.Rework_Remarks);
        //                                        //row.Add("QC-Remarks", code.RevRemarks);
        //                                        row.Add("MissingValue", code.MissingValue);
        //                                        row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
        //                                        row.Add("QC", code.Review != null ? code.Review.Name : "");
        //                                        if (code.ItemStatus == -1)
        //                                        {
        //                                            row.Add("ItemStatus", "Clarification");
        //                                        }
        //                                        else if (code.ItemStatus == 0)
        //                                        {
        //                                            row.Add("ItemStatus", "PV Not Assigned");
        //                                        }
        //                                        else if (code.ItemStatus == 1)
        //                                        {
        //                                            row.Add("ItemStatus", "PV Pending");
        //                                        }
        //                                        else if (code.ItemStatus == 2)
        //                                        {
        //                                            row.Add("ItemStatus", "PV Completed");
        //                                        }
        //                                        else if (code.ItemStatus == 3)
        //                                        {
        //                                            row.Add("ItemStatus", "Catalogue Saved");
        //                                        }
        //                                        else if (code.ItemStatus == 4)
        //                                        {
        //                                            row.Add("ItemStatus", "Catalogue Submit");
        //                                        }
        //                                        else if (code.ItemStatus == 5)
        //                                        {
        //                                            row.Add("ItemStatus", "QC Saved");
        //                                        }
        //                                        else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
        //                                        {
        //                                            row.Add("ItemStatus", "Released-Delivered");
        //                                        }
        //                                        else
        //                                        {
        //                                            row.Add("ItemStatus", "Released");
        //                                        }
        //                                        rows.Add(row);
        //                                        //break;
        //                                    }
        //                                    else
        //                                    {
        //                                        row = new Dictionary<string, object>();
        //                                        //row.Add("Created On", code.CreatedOn);
        //                                        row.Add("Unique ID", code.UniqueId);
        //                                        row.Add("Asset NO", code.AssetNo);
        //                                        row.Add("Equipment Description", code.Description);
        //                                        //row.Add("L-Legacy", code.Legacy2);
        //                                        //   row.Add("ML-Missingvalue", code.Legacy2);
        //                                        row.Add("Noun", code.Noun);
        //                                        row.Add("Modifier", code.Modifier);
        //                                        //row.Add("Labelshort", code.Shortdesc);
        //                                        row.Add("Shortdesc", code.Equipment_Short);
        //                                        row.Add("Longdesc", code.Equipment_Long);
        //                                        row.Add("Attribute", nm1.Characteristic);
        //                                        row.Add("Value", "");
        //                                        row.Add("UOM", "");
        //                                        row.Add("Mandatory", "");
        //                                        row.Add("Sequence", nm1.Squence);

        //                                        if (!string.IsNullOrEmpty(exchaFilledRate))
        //                                            row.Add("Existing Filled Rate", exchaFilledRate);
        //                                        else
        //                                            row.Add("Existing Filled Rate", "");
        //                                        if (!string.IsNullOrEmpty(chaFilledRate))
        //                                            row.Add("New Filled Rate", chaFilledRate);
        //                                        else
        //                                            row.Add("New Filled Rate", "");
        //                                        //row.Add("Additionalinfo", code.Additionalinfo);
        //                                        //if (!String.IsNullOrEmpty(code.Unspsc))
        //                                        //{
        //                                        //    row.Add("Unspsc", code.Unspsc);
        //                                        //}
        //                                        //else row.Add("Unspsc", NMUnspsc);

        //                                        row.Add("Soureurl", code.Soureurl);
        //                                        //if (code.Equipment != null)
        //                                        //{
        //                                        //    row.Add("EQ_Name", code.Equipment.Name);
        //                                        //    row.Add("EQ_Name_s", code.Equipment.ENS);
        //                                        //    row.Add("EQ_Manf", code.Equipment.Manufacturer);
        //                                        //    row.Add("EQ_MNo", code.Equipment.Modelno);
        //                                        //    row.Add("EQ_MNo_s", code.Equipment.ENS);
        //                                        //    row.Add("EQ_TNo", code.Equipment.Tagno);
        //                                        //    row.Add("EQ_SNo", code.Equipment.Serialno);
        //                                        //    row.Add("EQ_Add", code.Equipment.Additionalinfo);
        //                                        //}
        //                                        //else
        //                                        //{
        //                                        //    row.Add("EQ_Name", "");
        //                                        //    row.Add("EQ_Name_s", "");
        //                                        //    row.Add("EQ_Manf", "");
        //                                        //    row.Add("EQ_MNo", "");
        //                                        //    row.Add("EQ_MNo_s", "");
        //                                        //    row.Add("EQ_TNo", "");
        //                                        //    row.Add("EQ_SNo", "");
        //                                        //    row.Add("EQ_Add", "");
        //                                        //}
        //                                        row.Add("PV Remarks", code.Remarks);
        //                                        row.Add("Rework Remarks", code.Rework_Remarks);
        //                                        //row.Add("QC-Remarks", code.RevRemarks);
        //                                        row.Add("MissingValue", code.MissingValue);
        //                                        row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
        //                                        row.Add("QC", code.Review != null ? code.Review.Name : "");
        //                                        if (code.ItemStatus == -1)
        //                                        {
        //                                            row.Add("ItemStatus", "Clarification");
        //                                        }
        //                                        else if (code.ItemStatus == 0)
        //                                        {
        //                                            row.Add("ItemStatus", "PV Not Assigned");
        //                                        }
        //                                        else if (code.ItemStatus == 1)
        //                                        {
        //                                            row.Add("ItemStatus", "PV Pending");
        //                                        }
        //                                        else if (code.ItemStatus == 2)
        //                                        {
        //                                            row.Add("ItemStatus", "PV Completed");
        //                                        }
        //                                        else if (code.ItemStatus == 3)
        //                                        {
        //                                            row.Add("ItemStatus", "Catalogue Saved");
        //                                        }
        //                                        else if (code.ItemStatus == 4)
        //                                        {
        //                                            row.Add("ItemStatus", "Catalogue Submit");
        //                                        }
        //                                        else if (code.ItemStatus == 5)
        //                                        {
        //                                            row.Add("ItemStatus", "QC Saved");
        //                                        }
        //                                        else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
        //                                        {
        //                                            row.Add("ItemStatus", "Released-Delivered");
        //                                        }
        //                                        else
        //                                        {
        //                                            row.Add("ItemStatus", "Released");
        //                                        }
        //                                        rows.Add(row);
        //                                    }
        //                                }

        //                                // }
        //                            }
        //                            else
        //                            {

        //                                row = new Dictionary<string, object>();
        //                                //row.Add("Created On", code.CreatedOn);
        //                                row.Add("Unique ID", code.UniqueId);
        //                                row.Add("Asset NO", code.AssetNo);
        //                                row.Add("Equipment Description", code.Description);
        //                                //row.Add("L-Legacy", code.Legacy2);
        //                                //   row.Add("ML-Missingvalue", code.Legacy2);
        //                                row.Add("Noun", code.Noun);
        //                                row.Add("Modifier", code.Modifier);
        //                                //row.Add("Labelshort", code.Shortdesc);
        //                                row.Add("Shortdesc", code.Equipment_Short);
        //                                row.Add("Longdesc", code.Equipment_Long);
        //                                row.Add("Attribute", nm1.Characteristic);
        //                                row.Add("Value", "");
        //                                //row.Add("UOM", "");
        //                                row.Add("Mandatory", "");
        //                                row.Add("Sequence", nm1.Squence);
        //                                if (!string.IsNullOrEmpty(exchaFilledRate))
        //                                    row.Add("Existing Filled Rate", exchaFilledRate);
        //                                else
        //                                    row.Add("Existing Filled Rate", "");
        //                                if (!string.IsNullOrEmpty(chaFilledRate))
        //                                    row.Add("Mandatory Filled Rate", mandFilledRate);
        //                                else
        //                                    row.Add("Mandatory Filled Rate", "");
        //                                if (!string.IsNullOrEmpty(chaFilledRate))
        //                                    row.Add("New Filled Rate", chaFilledRate);
        //                                else
        //                                    row.Add("New Filled Rate", "");
        //                                //row.Add("Additionalinfo", code.Additionalinfo);

        //                                //row.Add("Unspsc", NMUnspsc);


        //                                if (code.AssetImages != null)
        //                                {
        //                                    if (code.AssetImages.NamePlateText != null)
        //                                    {
        //                                        row.Add("Nameplate Text", code.AssetImages.NamePlateText[0]);
        //                                    }
        //                                    else
        //                                    {
        //                                        row.Add("Nameplate Text", "");
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    row.Add("Nameplate Text", "");
        //                                }

        //                                row.Add("Soureurl", code.Soureurl);
        //                                //if (code.Equipment != null)
        //                                //{
        //                                //    row.Add("EQ_Name", code.Equipment.Name);
        //                                //    row.Add("EQ_Name_s", code.Equipment.ENS);
        //                                //    row.Add("EQ_Manf", code.Equipment.Manufacturer);
        //                                //    row.Add("EQ_MNo", code.Equipment.Modelno);
        //                                //    row.Add("EQ_MNo_s", code.Equipment.ENS);
        //                                //    row.Add("EQ_TNo", code.Equipment.Tagno);
        //                                //    row.Add("EQ_SNo", code.Equipment.Serialno);
        //                                //    row.Add("EQ_Add", code.Equipment.Additionalinfo);
        //                                //}
        //                                //row.Add("Alt_Unspsc", code.Referenceno);
        //                                //row.Add("Status", code.Application);
        //                                //row.Add("Soureurl", code.Soureurl);
        //                                //row.Add("MFR_Name", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].Name) ? ven[0].Name : "");
        //                                //row.Add("MFR_PartNo", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].RefNo) ? ven[0].RefNo : "");
        //                                //row.Add("Batch", code.batch);
        //                                row.Add("Remarks", code.Remarks);
        //                                //row.Add("QC-Remarks", code.RevRemarks);
        //                                row.Add("MissingValue", code.MissingValue);
        //                                row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
        //                                row.Add("QC", code.Review != null ? code.Review.Name : "");
        //                                if (code.ItemStatus == -1)
        //                                {
        //                                    row.Add("ItemStatus", "Clarification");
        //                                }
        //                                else if (code.ItemStatus == 0)
        //                                {
        //                                    row.Add("ItemStatus", "PV Not Assigned");
        //                                }
        //                                else if (code.ItemStatus == 1)
        //                                {
        //                                    row.Add("ItemStatus", "PV Pending");
        //                                }
        //                                else if (code.ItemStatus == 2)
        //                                {
        //                                    row.Add("ItemStatus", "PV Completed");
        //                                }
        //                                else if (code.ItemStatus == 3)
        //                                {
        //                                    row.Add("ItemStatus", "Catalogue Saved");
        //                                }
        //                                else if (code.ItemStatus == 4)
        //                                {
        //                                    row.Add("ItemStatus", "Catalogue Submit");
        //                                }
        //                                else if (code.ItemStatus == 5)
        //                                {
        //                                    row.Add("ItemStatus", "QC Saved");
        //                                }
        //                                else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
        //                                {
        //                                    row.Add("ItemStatus", "Released-Delivered");
        //                                }
        //                                else
        //                                {
        //                                    row.Add("ItemStatus", "Released");
        //                                }
        //                                rows.Add(row);
        //                            }

        //                        }

        //                    }
        //                    else
        //                    {
        //                        row = new Dictionary<string, object>();
        //                        //row.Add("Created On", code.CreatedOn);
        //                        row.Add("Unique ID", code.UniqueId);
        //                        row.Add("Asset NO", code.AssetNo);
        //                        row.Add("Equipment Description", code.Description);
        //                        //row.Add("L-Legacy", code.Legacy2);
        //                        //   row.Add("ML-Missingvalue", code.Legacy2);
        //                        //  row.Add("ML-Missingvalue", code.Legacy2);
        //                        row.Add("Noun", "");
        //                        row.Add("Modifier", "");
        //                        //row.Add("Labelshort", "");
        //                        row.Add("Shortdesc", "");
        //                        row.Add("Longdesc", "");
        //                        row.Add("Attribute", "");
        //                        row.Add("Value", "");
        //                        //row.Add("UOM", "");
        //                        row.Add("Mandatory", "");
        //                        row.Add("Sequence", "");
        //                        row.Add("Existing Filled Rate", "");
        //                        row.Add("Mandatory Filled Rate", "");
        //                        row.Add("New Filled Rate", "");
        //                        //row.Add("Additionalinfo", code.Additionalinfo);

        //                        row.Add("Unspsc", "");

        //                        if (code.AssetImages != null)
        //                        {
        //                            if (code.AssetImages.NamePlateText != null)
        //                            {
        //                                row.Add("Nameplate Text", code.AssetImages.NamePlateText[0]);
        //                            }
        //                            else
        //                            {
        //                                row.Add("Nameplate Text", "");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            row.Add("Nameplate Text", "");
        //                        }
        //                        row.Add("Soureurl", code.Soureurl);
        //                        //if (code.Equipment != null)
        //                        //{
        //                        //    row.Add("EQ_Name", code.Equipment.Name);
        //                        //    row.Add("EQ_Name_s", code.Equipment.ENS);
        //                        //    row.Add("EQ_Manf", code.Equipment.Manufacturer);
        //                        //    row.Add("EQ_MNo", code.Equipment.Modelno);
        //                        //    row.Add("EQ_MNo_s", code.Equipment.ENS);
        //                        //    row.Add("EQ_TNo", code.Equipment.Tagno);
        //                        //    row.Add("EQ_SNo", code.Equipment.Serialno);
        //                        //    row.Add("EQ_Add", code.Equipment.Additionalinfo);
        //                        //}
        //                        //row.Add("Alt_Unspsc", code.Referenceno);
        //                        //row.Add("Status", code.Application);
        //                        //row.Add("Soureurl", code.Soureurl);
        //                        //row.Add("MFR_Name", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].Name) ? ven[0].Name : "");
        //                        //row.Add("MFR_PartNo", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].RefNo) ? ven[0].RefNo : "");
        //                        //row.Add("Batch", code.batch);
        //                        row.Add("PV Remarks", code.Remarks);
        //                        row.Add("Rework Remarks", code.Rework_Remarks);
        //                        //row.Add("QC-Remarks", code.RevRemarks);
        //                        row.Add("MissingValue", code.MissingValue);
        //                        row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
        //                        row.Add("QC", code.Review != null ? code.Review.Name : "");
        //                        if (code.ItemStatus == -1)
        //                        {
        //                            row.Add("ItemStatus", "Clarification");
        //                        }
        //                        else if (code.ItemStatus == 0)
        //                        {
        //                            row.Add("ItemStatus", "PV Not Assigned");
        //                        }
        //                        else if (code.ItemStatus == 1)
        //                        {
        //                            row.Add("ItemStatus", "PV Pending");
        //                        }
        //                        else if (code.ItemStatus == 2)
        //                        {
        //                            row.Add("ItemStatus", "PV Completed");
        //                        }
        //                        else if (code.ItemStatus == 3)
        //                        {
        //                            row.Add("ItemStatus", "Catalogue Saved");
        //                        }
        //                        else if (code.ItemStatus == 4)
        //                        {
        //                            row.Add("ItemStatus", "Catalogue Submit");
        //                        }
        //                        else if (code.ItemStatus == 5)
        //                        {
        //                            row.Add("ItemStatus", "QC Saved");
        //                        }
        //                        else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
        //                        {
        //                            row.Add("ItemStatus", "Released-Delivered");
        //                        }
        //                        else
        //                        {
        //                            row.Add("ItemStatus", "Released");
        //                        }
        //                        rows.Add(row);
        //                    }
        //                }
        //                else
        //                {
        //                    row = new Dictionary<string, object>();
        //                    //row.Add("Created On", code.CreatedOn);
        //                    row.Add("Unique ID", code.UniqueId);
        //                    row.Add("Asset NO", code.AssetNo);
        //                    row.Add("Equipment Description", code.Description);
        //                    //  row.Add("ML-Missingvalue", code.Legacy2);
        //                    row.Add("Noun", "");
        //                    row.Add("Modifier", "");
        //                    //row.Add("Labelshort", "");
        //                    row.Add("Shortdesc", "");
        //                    row.Add("Longdesc", "");
        //                    row.Add("Attribute", "");
        //                    row.Add("Value", "");
        //                    //row.Add("UOM", "");
        //                    row.Add("Mandatory", "");
        //                    row.Add("Sequence", "");
        //                    row.Add("Existing Filled Rate", "");
        //                    row.Add("Mandatory Filled Rate", "");
        //                    row.Add("New Filled Rate", "");
        //                    //row.Add("Additionalinfo", code.Additionalinfo);

        //                    //row.Add("Unspsc", "");

        //                    if (code.AssetImages != null)
        //                    {
        //                        if (code.AssetImages.NamePlateText != null)
        //                        {
        //                            row.Add("Nameplate Text", code.AssetImages.NamePlateText[0]);
        //                        }
        //                        else
        //                        {
        //                            row.Add("Nameplate Text", "");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        row.Add("Nameplate Text", "");
        //                    }
        //                    row.Add("Soureurl", code.Soureurl);
        //                    //if (code.Equipment != null)
        //                    //{
        //                    //    row.Add("EQ_Name", code.Equipment.Name);
        //                    //    row.Add("EQ_Name_s", code.Equipment.ENS);
        //                    //    row.Add("EQ_Manf", code.Equipment.Manufacturer);
        //                    //    row.Add("EQ_MNo", code.Equipment.Modelno);
        //                    //    row.Add("EQ_MNo_s", code.Equipment.ENS);
        //                    //    row.Add("EQ_TNo", code.Equipment.Tagno);
        //                    //    row.Add("EQ_SNo", code.Equipment.Serialno);
        //                    //    row.Add("EQ_Add", code.Equipment.Additionalinfo);
        //                    //}
        //                    //row.Add("Alt_Unspsc", code.Referenceno);
        //                    //row.Add("Status", code.Application);
        //                    //row.Add("Soureurl", code.Soureurl);
        //                    //row.Add("MFR_Name", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].Name) ? ven[0].Name : "");
        //                    //row.Add("MFR_PartNo", ven.Count > 0 && !string.IsNullOrEmpty(ven[0].RefNo) ? ven[0].RefNo : "");
        //                    //row.Add("Batch", code.batch);
        //                    row.Add("PV Remarks", code.Remarks);
        //                    row.Add("Rework Remarks", code.Rework_Remarks);
        //                    //row.Add("QC-Remarks", code.RevRemarks);
        //                    row.Add("MissingValue", code.MissingValue);
        //                    row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
        //                    row.Add("QC", code.Review != null ? code.Review.Name : "");
        //                    row.Add("ItemStatus", code.ItemStatus == 0 ?
        //                                          (code.ItemStatus == 1 ?
        //                                          (code.ItemStatus == 2 ?
        //                                          (code.ItemStatus == 5 ?
        //                                          (code.ItemStatus == 3 ?
        //                                          (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category)) ?
        //                                          "Released-Delivered"
        //                                          : "QC Submit"
        //                                          : "QC Saved")
        //                                          : "QA Saved")
        //                                          : "Catalogue Submit")
        //                                          : "Saved")
        //                                          : "Not Saved");
        //                    rows.Add(row);
        //                }
        //            }
        //            else
        //            {
        //                row = new Dictionary<string, object>();
        //                //row.Add("Created On", code.CreatedOn);
        //                row.Add("Unique ID", code.UniqueId);
        //                row.Add("Asset NO", code.AssetNo);
        //                //   row.Add("Legacy", code.Legacy);
        //                //  row.Add("ML-Missingvalue", code.Legacy2);
        //                row.Add("Noun", string.IsNullOrEmpty(code.Noun) ? "" : code.Noun);
        //                row.Add("Modifier", string.IsNullOrEmpty(code.Modifier) ? "" : code.Modifier);
        //                //   row.Add("Attribute", "");
        //                //   row.Add("Value", "");
        //                // row.Add("UOM", "");
        //                //  row.Add("Mandatory", "");
        //                // row.Add("Sequence", "");
        //                //  row.Add("Additionalinfo", code.Additionalinfo);
        //                //  row.Add("Unspsc", code.Unspsc);
        //                //  row.Add("MissingValue", code.MissingValue);
        //                //      row.Add("Status", code.Application);
        //                //  row.Add("Soureurl", code.Soureurl);
        //                //  row.Add("Batch", code.batch);
        //                // row.Add("Remarks", code.Remarks);
        //                row.Add("Existing Filled Rate", "");
        //                row.Add("Mandatory Filled Rate", "");
        //                row.Add("New Filled Rate", "");
        //                row.Add("PV Remarks", code.Remarks);
        //                row.Add("Rework Remarks", code.Rework_Remarks);
        //                row.Add("Cataloguer", code.Catalogue != null ? code.Catalogue.Name : "");
        //                row.Add("QC", code.Review != null ? code.Review.Name : "");
        //                if (code.ItemStatus == -1)
        //                {
        //                    row.Add("ItemStatus", "Clarification");
        //                }
        //                else if (code.ItemStatus == 0)
        //                {
        //                    row.Add("ItemStatus", "PV Not Assigned");
        //                }
        //                else if (code.ItemStatus == 1)
        //                {
        //                    row.Add("ItemStatus", "PV Pending");
        //                }
        //                else if (code.ItemStatus == 2)
        //                {
        //                    row.Add("ItemStatus", "PV Completed");
        //                }
        //                else if (code.ItemStatus == 3)
        //                {
        //                    row.Add("ItemStatus", "Catalogue Saved");
        //                }
        //                else if (code.ItemStatus == 4)
        //                {
        //                    row.Add("ItemStatus", "Catalogue Submit");
        //                }
        //                else if (code.ItemStatus == 5)
        //                {
        //                    row.Add("ItemStatus", "QC Saved");
        //                }
        //                else if (code.ItemStatus == 6 && !string.IsNullOrEmpty(code.category))
        //                {
        //                    row.Add("ItemStatus", "Released-Delivered");
        //                }
        //                else
        //                {
        //                    row.Add("ItemStatus", "Released");
        //                }

        //                row.Add("QC-UpdatedOn", code.Review != null ? code.Review.UpdatedOn.ToString() : "");
        //                rows.Add(row);

        //            }
        //        }
        //        return rows;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }


        //}
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
                    var query2 = Query.Or(Query.EQ("UniqueId", drw1[0].ToString()), Query.EQ("AssetNo", drw1[0].ToString()));
                    var mdl2 = _assetRepository.FindOne(query2);
                    if (mdl2 != null)
                    {

                        if (mdl2.PVstatus == "Completed" || (mdl2.AssetImages != null && mdl2.AssetImages.AssetImgs != null))
                        {
                            mdl2.ItemStatus = 2;
                            var usrQry = Query.EQ("UserName", drw1[1].ToString());
                            //var usrQry = Query.Matches("UserName", BsonRegularExpression.Create(new Regex(drw1[1].ToString().TrimStart().TrimEnd(), RegexOptions.IgnoreCase)));
                            var usrInfo = _UsercreateRepository.FindOne(usrQry);
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
                            _assetRepository.Add(mdl2);
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
                foreach (DataRow drw1 in dt1.Rows)
                {
                    var query2 = Query.Or(Query.EQ("UniqueId", drw1[0].ToString()), Query.EQ("AssetNo", drw1[0].ToString()));
                    var mdl2 = _assetRepository.FindOne(query2);
                    if (mdl2 != null)
                    {
                        if (mdl2.ItemStatus > 2)
                        {
                            mdl2.ItemStatus = 4;
                            var usrQry = Query.EQ("UserName", drw1[1].ToString());
                            //var usrQry = Query.Matches("UserName", BsonRegularExpression.Create(new Regex(drw1[1].ToString().TrimStart().TrimEnd(), RegexOptions.IgnoreCase)));
                            var usrInfo = _UsercreateRepository.FindOne(usrQry);
                            var Qc = new Prosol_UpdatedBy();
                            Qc.UserId = usrInfo.Userid;
                            Qc.Name = usrInfo.UserName;
                            Qc.UpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                            mdl2.Review = Qc;
                            _assetRepository.Add(mdl2);
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


        public virtual string AssetBulkData(HttpPostedFileBase file)
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

            }

            return cunt + " Items assigned successfully";

        }

        public bool AddValue(string Noun, string Modifier, string Attribute, string Value, string abb, string user, string role)
        {
            // var query = Query.And(Query.EQ("Value", Value), Query.EQ("Abbrevated", abb));
            var query = Query.EQ("Value", Value);
            var obj = _assetAbbreivateRepository.FindOne(query);
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
                var Qry1 = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.EQ("Characteristic", Attribute), Query.EQ("Definition", "Equ"));
                var TemplateMaster = _CharateristicRepository.FindOne(Qry1);
                if (TemplateMaster != null && TemplateMaster.Values != null)
                {
                    var lstobj = TemplateMaster.Values.ToList();
                    lstobj.Add(obj._id.ToString());
                    TemplateMaster.Values = lstobj.ToArray();
                    _CharateristicRepository.Add(TemplateMaster);
                }
                else
                {
                    var str = new string[1];
                    str[0] = obj._id.ToString();

                    TemplateMaster.Values = str;
                    _CharateristicRepository.Add(TemplateMaster);


                }

            }
            else
            {
                var newobj = new Prosol_AssetAbbrevate();
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

                _assetAbbreivateRepository.Add(newobj);
                query = Query.And(Query.EQ("Value", Value), Query.EQ("Abbrevated", abb));
                obj = _assetAbbreivateRepository.FindOne(query);
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
                    var Qry1 = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.EQ("Characteristic", Attribute), Query.EQ("Definition", "Equ"));
                    var TemplateMaster = _CharateristicRepository.FindOne(Qry1);
                    if (TemplateMaster != null && TemplateMaster.Values != null)
                    {
                        var lstobj = TemplateMaster.Values.ToList();
                        lstobj.Add(obj._id.ToString());
                        TemplateMaster.Values = lstobj.ToArray();
                        _CharateristicRepository.Add(TemplateMaster);
                    }
                    else
                    {
                        var str = new string[1];
                        str[0] = obj._id.ToString();
                        TemplateMaster.Values = str;
                        _CharateristicRepository.Add(TemplateMaster);

                    }
                }

            }
            var x = CheckValue(Noun, Modifier, Attribute, Value);
            return true;


        }

        public string CheckValue(string Noun, string Modifier, string Attribute, string Value)
        {
            var condit = ""; string pattern = " X ", pattern1 = " x ";
            if (Value.Contains(','))
            {
                var Qry = Query.EQ("Value", Value);
                var mobj = _assetAbbreivateRepository.FindOne(Qry);
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
                            var obj = _assetAbbreivateRepository.FindOne(query);
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
                                    obj = _assetAbbreivateRepository.FindOne(query);
                                    if (obj != null)
                                        condit = obj != null && !string.IsNullOrEmpty(obj.Abbrevated) ? condit + obj.Abbrevated + " " : condit + obj.Value + " ";
                                }
                                condit = condit.TrimEnd(' ');
                            }


                        }
                        else
                        {
                            var query = Query.EQ("Value", str);
                            var obj = _assetAbbreivateRepository.FindOne(query);
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
                    var obj = _assetAbbreivateRepository.FindOne(query);
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
                            obj = _assetAbbreivateRepository.FindOne(query);
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
                    var obj = _assetAbbreivateRepository.FindOne(query);
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
                var obj = _assetAbbreivateRepository.FindOne(query);
                if (obj != null)
                {

                    return obj.Abbrevated;

                }
                return B;
            }

            return condit;
        }
        public List<Dictionary<string, object>> Downloadvendordata(string username, string status)
        {

            try
            {
                IMongoQuery query;
                var datalist = new List<Prosol_AssetMaster>();
                if (status == "cat")
                {
                    //query = Query.And(Query.Or(Query.EQ("Catalogue.Name", username), Query.And(Query.EQ("Review.Name", username))), Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)));
                    query = Query.Or(
                    Query.And(
                    Query.EQ("Catalogue.Name", username), Query.Or(/*Query.EQ("ItemStatus", 2),*/ Query.EQ("ItemStatus", 3))
                    ),
                    Query.And(
                    Query.EQ("Review.Name", username), Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 5))
                    )
                    //Query.And(
                    //Query.EQ("Release.UserId", uId), Query.Or(Query.EQ("ItemStatus", 6), Query.EQ("ItemStatus", 7), Query.EQ("ItemStatus", 9))
                    //)
                    );
                }
                else if (status == "qc")
                {
                    query = Query.And(Query.EQ("Review.Name", username), Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 5)));
                    if (username == "Ramkumar")
                    {
                        query = Query.And(Query.Or(Query.EQ("ItemStatus", 4), Query.EQ("ItemStatus", 5)));
                    }

                }
                else if (status == "All")
                {
                    query = Query.Or(Query.NE("Catalogue", BsonNull.Value), Query.GT("ItemStatus", 0));
                }
                else if (status == "QA")
                {
                    query = Query.And(Query.EQ("Release.Name", username), Query.GTE("ItemStatus", 4), Query.EQ("category", BsonNull.Value));

                }
                else
                {
                    //query = Query.And(Query.EQ("Review.Name", username), Query.Or(Query.EQ("ItemStatus", 2), Query.EQ("ItemStatus", 3)));
                    query = Query.GTE("ItemStatus", 8);
                }
                string[] strArr = { "UniqueId", "AssetNo", "Manufacture", "ModelNo", "SerialNo", "ModelYear" };
                datalist = _assetRepository.FindAll(query).ToList();


                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
                Dictionary<string, object> rowh;

                foreach (var code in datalist)
                {
                    if (status != "All")
                    {
                        row = new Dictionary<string, object>();
                        row.Add("UniqueId", code.UniqueId);
                        row.Add("AssetNo", code.AssetNo);
                        row.Add("Manufacture", code.Manufacturer);
                        row.Add("ModelNo", code.ModelNo);
                        row.Add("SerialNo", code.SerialNo);
                        row.Add("ModelYear", code.ModelYear);
                        if(code.Doc_Availability == true)
                            row.Add("Documents", "Available");
                        else
                            row.Add("Documents", "Not Available");
                        rows.Add(row);
                    }
                    else
                    {
                        row = new Dictionary<string, object>();
                        row.Add("UniqueId", code.UniqueId);
                        row.Add("AssetNo", code.AssetNo);
                        row.Add("Manufacture", code.Manufacturer);
                        row.Add("ModelNo", code.ModelNo);
                        row.Add("SerialNo", code.SerialNo);
                        row.Add("ModelYear", code.ModelYear);
                        if (code.Doc_Availability == true)
                            row.Add("Documents", "Available");
                        else
                            row.Add("Documents", "Not Available");
                        rows.Add(row);

                    }
                }
                return rows;
            }
            catch (Exception e)
            {
                return null;
            }


        }
        public List<Dictionary<string, object>> Downloadvendordatacodes(string[] code_split)
        {

            try
            {
                var datalist = new List<Prosol_AssetMaster>();
                //foreach (var code in code_split)
                //{
                //    var data = new Prosol_AssetMaster();
                //    var query = Query.Or(Query.EQ("UniqueId", code), Query.EQ("AssetNo", code));
                //    data = _assetRepository.FindOne(query);
                //    datalist.Add(data);
                //}
                var Qry = Query.Or(Query.In("UniqueId", new BsonArray(code_split)), Query.In("AssetNo", new BsonArray(code_split)));
                datalist = _assetRepository.FindAll(Qry).ToList();

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                List<Dictionary<string, object>> rowsheader = new List<Dictionary<string, object>>();
                Dictionary<string, object> rowh;

                foreach (var code in datalist)
                {
                    if (code != null)
                    {
                        row = new Dictionary<string, object>();
                        row.Add("UniqueId", code.UniqueId);
                        row.Add("AssetNo", code.AssetNo);
                        row.Add("Manufacture", code.Manufacturer);
                        row.Add("ModelNo", code.ModelNo);
                        row.Add("SerialNo", code.SerialNo);
                        row.Add("ModelYear", code.ModelYear);
                        rows.Add(row);
                    }
                }
                return rows;
            }
            catch (Exception e)
            {
                return null;
            }


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
            var usrLst = _UsercreateRepository.FindAll().ToList();
            foreach (DataRow drw in dt.Rows)
            {
                var query1 = Query.Or(Query.EQ("AssetNo", drw[0].ToString()), Query.EQ("UniqueId", drw[0].ToString()));
                var mdl1 = _assetRepository.FindOne(query1);
                if (mdl1 != null)
                {
                    mdl1.ItemStatus = drw[1].ToString() == "PV" ? 1 : drw[1].ToString() == "Cat" ? 2 : mdl1.ItemStatus;
                    mdl1.Rework = drw[1].ToString();
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
                    mdl1.Rework_Remarks = drw[3].ToString();
                    _assetRepository.Add(mdl1);
                    cunt++;
                }

            }


            return cunt;
        }
        public virtual int BulkDashboard(HttpPostedFileBase file)
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
            var objLst = new List<Prosol_Dashboard>();
            foreach (DataRow drw in dt.Rows)
            {
                _dashRepository.DeleteAll();
                var mdl = new Prosol_Dashboard();
                mdl.Cluster = drw[0].ToString();
                mdl.Location = drw[1].ToString();
                mdl.ReportGroup = drw[2].ToString();
                if (!string.IsNullOrEmpty(drw[3].ToString()))
                    mdl.AssetCount = Convert.ToInt32(drw[3].ToString());
                else
                    mdl.AssetCount = 0;
                if (!string.IsNullOrEmpty(drw[4].ToString()))

                    mdl.AssetCompleted = Convert.ToInt32(drw[4].ToString());
                else
                    mdl.AssetCompleted = 0;
                if (!string.IsNullOrEmpty(drw[5].ToString()))
                    mdl.NewAsset = Convert.ToInt32(drw[5].ToString());
                else
                    mdl.NewAsset = 0;
                if (!string.IsNullOrEmpty(drw[6].ToString()))
                    mdl.Furniture = Convert.ToInt32(drw[6].ToString());
                else
                    mdl.Furniture = 0;

                if (!string.IsNullOrEmpty(drw[7].ToString()))
                    mdl.AssetPending = Convert.ToInt32(drw[7].ToString());
                else
                    mdl.AssetPending = 0;
                mdl.Remarks = drw[8].ToString();
                objLst.Add(mdl);
                cunt++;
            }

            _dashRepository.Add(objLst);

            return cunt;
        }
        public virtual int BulkSwap(HttpPostedFileBase file)
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
            foreach (DataRow drw1 in dt.Rows)
            {
                var query1 = Query.Or(Query.EQ("UniqueId", drw1[0].ToString()), Query.EQ("AssetNo", drw1[0].ToString()));
                var assetvalues = _assetRepository.FindOne(query1);
                var query2 = Query.Or(Query.EQ("UniqueId", drw1[1].ToString()), Query.EQ("AssetNo", drw1[1].ToString()));
                var catasset = _assetRepository.FindOne(query2);
                if (assetvalues != null)
                {
                    if (assetvalues.AssetImages != null)
                    {
                        var Asset_imgs = new AssetImage();

                        if (assetvalues.AssetImages.AssetImgs != null && assetvalues.AssetImages.AssetImgs.Length > 0)
                            Asset_imgs.AssetImgs = assetvalues.AssetImages.AssetImgs;

                        if (assetvalues.AssetImages.MatImgs != null && assetvalues.AssetImages.MatImgs.Length > 0)
                            Asset_imgs.MatImgs = assetvalues.AssetImages.MatImgs;

                        if (assetvalues.AssetImages.NamePlateImge != null && assetvalues.AssetImages.NamePlateImge.Length > 0)
                            Asset_imgs.NamePlateImge = assetvalues.AssetImages.NamePlateImge;

                        if (assetvalues.AssetImages.NamePlateImgeTwo != null && assetvalues.AssetImages.NamePlateImgeTwo.Length > 0)
                            Asset_imgs.NamePlateImgeTwo = assetvalues.AssetImages.NamePlateImgeTwo;

                        if (assetvalues.AssetImages.NamePlateText != null && assetvalues.AssetImages.NamePlateText.Length > 0)
                            Asset_imgs.NamePlateText = assetvalues.AssetImages.NamePlateText;

                        if (assetvalues.AssetImages.NamePlateTextTwo != null && assetvalues.AssetImages.NamePlateTextTwo.Length > 0)
                            Asset_imgs.NamePlateTextTwo = assetvalues.AssetImages.NamePlateTextTwo;

                        if (assetvalues.AssetImages.NameplateImgs != null && assetvalues.AssetImages.NameplateImgs.Length > 0)
                            Asset_imgs.NameplateImgs = assetvalues.AssetImages.NameplateImgs;

                        if (assetvalues.AssetImages.NewTagImage != null && assetvalues.AssetImages.NewTagImage.Length > 0)
                            Asset_imgs.NewTagImage = assetvalues.AssetImages.NewTagImage;

                        if (assetvalues.AssetImages.OldTagImage != null && assetvalues.AssetImages.OldTagImage.Length > 0)
                            Asset_imgs.OldTagImage = assetvalues.AssetImages.OldTagImage;

                        catasset.AssetImages = Asset_imgs;
                    }

                    if (assetvalues.GIS != null)
                    {
                        var gis = new Prosol_AssetGIS();
                        gis.LattitudeStart = assetvalues.GIS.LattitudeStart;
                        gis.LattitudeEnd = assetvalues.GIS.LattitudeEnd;
                        gis.LongitudeStart = assetvalues.GIS.LongitudeStart;
                        gis.LongitudeEnd = assetvalues.GIS.LongitudeEnd;
                        gis.Lat_LongLength = assetvalues.GIS.Lat_LongLength;
                        catasset.GIS = gis;
                    }
                    if (assetvalues.AssetCondition != null)
                    {
                        var Asset_Cndt = new Prosol_AssetCondition();

                        Asset_Cndt.Corrosion = assetvalues.AssetCondition.Corrosion;
                        Asset_Cndt.Damage = assetvalues.AssetCondition.Damage;
                        Asset_Cndt.Leakage = assetvalues.AssetCondition.Leakage;
                        Asset_Cndt.Vibration = assetvalues.AssetCondition.Vibration;
                        Asset_Cndt.Temparature = assetvalues.AssetCondition.Temparature;
                        Asset_Cndt.Smell = assetvalues.AssetCondition.Smell;
                        Asset_Cndt.Noise = assetvalues.AssetCondition.Noise;
                        Asset_Cndt.Rank = assetvalues.AssetCondition.Rank;
                        Asset_Cndt.Asset_Condition = assetvalues.AssetCondition.Asset_Condition;
                        Asset_Cndt.CorrosionImage = assetvalues.AssetCondition.CorrosionImage;
                        Asset_Cndt.DamageImage = assetvalues.AssetCondition.DamageImage;
                        Asset_Cndt.LeakageImage = assetvalues.AssetCondition.LeakageImage;
                        catasset.AssetCondition = Asset_Cndt;
                    }
                    catasset.AssetQRCode = assetvalues.AssetQRCode;
                    catasset.OldTagNo = assetvalues.OldTagNo;
                    catasset.NewTagNo = assetvalues.NewTagNo;
                    catasset.AdditionalNotes = assetvalues.AdditionalNotes;
                    catasset.assetConditionRemarks = assetvalues.assetConditionRemarks;
                    catasset.Remarks = assetvalues.Remarks;
                    catasset.Rework_Remarks = assetvalues.Rework_Remarks;
                    catasset.AdditionalNotes = assetvalues.AdditionalNotes;
                    catasset.Manufacturer = assetvalues.Manufacturer;
                    catasset.SerialNo = assetvalues.SerialNo;
                    catasset.ModelNo = assetvalues.ModelNo;
                    catasset.ModelYear = assetvalues.ModelYear;
                    catasset.PVuser = assetvalues.PVuser;
                    catasset.Catalogue = assetvalues.Catalogue;
                    catasset.ItemStatus = 2;
                    catasset.PVstatus = assetvalues.PVstatus;

                    _assetRepository.Add(catasset);
                }
                cunt++;
            }

            return cunt;
        }
        //public virtual int BulkAdditional(HttpPostedFileBase file)
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

        //    foreach (DataRow drw1 in dt.Rows)
        //    {
        //        var query2 = Query.Or(Query.EQ("UniqueId", drw1[0].ToString()), Query.EQ("AssetNo", drw1[0].ToString())); 
        //        var mdl2 = _assetRepository.FindOne(query2);
        //        if (mdl2 != null)
        //        {
        //            mdl2.AdditionalNotes = drw1[1].ToString();
        //            _assetRepository.Add(mdl2);
        //        }
        //        cunt++;
        //    }

        //    return cunt;
        //}
        public virtual int MfrBulkUpload(HttpPostedFileBase file)
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

            var lst = new List<Prosol_FARMaster>();
            foreach (DataRow drw1 in dt.Rows)
            {
                var mdl2 = new Prosol_FARMaster();
                mdl2.Label = "Manufacturer";
                mdl2.Code = drw1[0].ToString();
                mdl2.Islive = drw1[1].ToString() == "Approved"?true:false;
                lst.Add(mdl2);
                cunt++;
            }
            _FARMaster.Add(lst);
            return cunt;
        }
        //public virtual int BulkLocation(HttpPostedFileBase file)
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

        //    foreach (DataRow drw1 in dt.Rows)
        //    {
        //        var query2 = Query.Or(Query.EQ("UniqueId", drw1[0].ToString()), Query.EQ("AssetNo", drw1[0].ToString())); 
        //        var mdl2 = _assetRepository.FindOne(query2);
        //        if (mdl2 != null)
        //        {
        //            mdl2.PresentLocation = drw1[1].ToString();
        //            _assetRepository.Add(mdl2);
        //        }
        //        cunt++;
        //    }

        //    return cunt;
        //}

        public virtual int BulkParent(HttpPostedFileBase file)
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

            foreach (DataRow drw1 in dt.Rows)
            {
                var query2 = Query.Or(Query.EQ("UniqueId", drw1[0].ToString()), Query.EQ("AssetNo", drw1[0].ToString()));
                var mdl2 = _assetRepository.FindOne(query2);
                if (mdl2 != null)
                {
                    mdl2.Parent = drw1[1].ToString();
                    mdl2.ParentName = drw1[2].ToString();
                    _assetRepository.Add(mdl2);
                }
                cunt++;
            }

            return cunt;
        }
        public virtual int BulkObject(HttpPostedFileBase file)
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

            foreach (DataRow drw1 in dt.Rows)
            {
                var query2 = Query.Or(Query.EQ("UniqueId", drw1[0].ToString()), Query.EQ("AssetNo", drw1[0].ToString()));
                var mdl2 = _assetRepository.FindOne(query2);
                if (mdl2 != null)
                {
                    mdl2.ObjType = drw1[1].ToString();
                    _assetRepository.Add(mdl2);
                }
                cunt++;
            }

            return cunt;
        }
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

            foreach (DataRow drw1 in dt.Rows)
            {
                var query2 = Query.Or(Query.EQ("UniqueId", drw1[0].ToString()), Query.EQ("AssetNo", drw1[0].ToString()));
                var mdl2 = _assetRepository.FindOne(query2);
                if (mdl2 != null)
                {
                    mdl2.Unspsc = drw1[1].ToString();
                    _assetRepository.Add(mdl2);
                }
                cunt++;
            }

            return cunt;
        }
        public virtual int BulkAssetNo(HttpPostedFileBase file)
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

            foreach (DataRow drw1 in dt.Rows)
            {
                var query2 = Query.Or(Query.EQ("UniqueId", drw1[0].ToString()), Query.EQ("AssetNo", drw1[0].ToString()));
                var mdl2 = _assetRepository.FindOne(query2);
                if (mdl2 != null)
                {
                    mdl2.EquipmentNo = drw1[1].ToString();
                    _assetRepository.Add(mdl2);
                }
                cunt++;
            }

            return cunt;
        }
        public virtual int BulkCost(HttpPostedFileBase file)
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

            foreach (DataRow drw1 in dt.Rows)
            {
                var query2 = Query.Or(Query.EQ("UniqueId", drw1[0].ToString()), Query.EQ("AssetNo", drw1[0].ToString()));
                var mdl2 = _assetRepository.FindOne(query2);
                if (mdl2 != null)
                {
                    mdl2.CostCenter = drw1[1].ToString();
                    mdl2.CostCenter_Desc = drw1[2].ToString();
                    _assetRepository.Add(mdl2);
                }
                cunt++;
            }

            return cunt;
        }
        public virtual int BulkDiscipline(HttpPostedFileBase file)
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

            foreach (DataRow drw1 in dt.Rows)
            {
                var query2 = Query.Or(Query.EQ("UniqueId", drw1[0].ToString()), Query.EQ("AssetNo", drw1[0].ToString()));
                var mdl2 = _assetRepository.FindOne(query2);
                if (mdl2 != null)
                {
                    mdl2.Discipline = drw1[1].ToString();
                    _assetRepository.Add(mdl2);
                }
                cunt++;
            }

            return cunt;
        }
        public virtual int BulkWorkC(HttpPostedFileBase file)
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

            foreach (DataRow drw1 in dt.Rows)
            {
                var query2 = Query.Or(Query.EQ("UniqueId", drw1[0].ToString()), Query.EQ("AssetNo", drw1[0].ToString()));
                var mdl2 = _assetRepository.FindOne(query2);
                if (mdl2 != null)
                {
                    mdl2.MainWorkCenter = drw1[1].ToString();
                    _assetRepository.Add(mdl2);
                }
                cunt++;
            }

            return cunt;
        }
        public virtual int BulkAdditional(HttpPostedFileBase file)
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

            foreach (DataRow drw1 in dt.Rows)
            {
                var query2 = Query.Or(Query.EQ("UniqueId", drw1[0].ToString()), Query.EQ("AssetNo", drw1[0].ToString()));
                var mdl2 = _assetRepository.FindOne(query2);
                if (mdl2 != null)
                {
                    mdl2.AdditionalInfo = drw1[1].ToString();
                    _assetRepository.Add(mdl2);
                }
                cunt++;
            }

            return cunt;
        }
        public virtual int BulkURL(HttpPostedFileBase file)
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

            foreach (DataRow drw1 in dt.Rows)
            {
                var query2 = Query.Or(Query.EQ("UniqueId", drw1[0].ToString()), Query.EQ("AssetNo", drw1[0].ToString()));
                var mdl2 = _assetRepository.FindOne(query2);
                if (mdl2 != null)
                {
                    mdl2.Soureurl = drw1[1].ToString();
                    _assetRepository.Add(mdl2);
                }
                cunt++;
            }

            return cunt;
        }
        public virtual int BulkTag(HttpPostedFileBase file)
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

            foreach (DataRow drw1 in dt.Rows)
            {
                var query2 = Query.Or(Query.EQ("UniqueId", drw1[0].ToString()), Query.EQ("AssetNo", drw1[0].ToString()));
                var mdl2 = _assetRepository.FindOne(query2);
                if (mdl2 != null)
                {
                    mdl2.TechIdentNo = drw1[1].ToString();
                    _assetRepository.Add(mdl2);
                }
                cunt++;
            }

            return cunt;
        }
        public virtual int BulkLegacy(HttpPostedFileBase file)
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

            foreach (DataRow drw1 in dt.Rows)
            {
                var query2 = Query.Or(Query.EQ("UniqueId", drw1[0].ToString()), Query.EQ("AssetNo", drw1[0].ToString()));
                var mdl2 = _assetRepository.FindOne(query2);
                if (mdl2 != null)
                {
                    mdl2.Description = drw1[1].ToString();
                    _assetRepository.Add(mdl2);
                }
                cunt++;
            }

            return cunt;
        }

        public IEnumerable<Prosol_Dashboard> getDashboard()
        {
            var objLst = _dashRepository.FindAll();
            return objLst;
        }
        public IEnumerable<QRObj> AssetQRCheck(HttpPostedFileBase file)
        {
            var objLst = new List<QRObj>();
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
            var assetLst = _assetRepository.FindAll().ToList();
            foreach (DataRow drw in dt.Rows)
            {
                var obj = new QRObj();
                var matchValue = drw[0].ToString();
                var data = assetLst.Find(i => i.AssetNo == matchValue || i.UniqueId == matchValue);
                if (data != null)
                {
                    obj.AssetNo = data.AssetNo;
                    //obj.TagNo = drw[1].ToString();
                    objLst.Add(obj);
                }

            }
            return objLst;
        }
        public List<Prosol_FARMaster> GetMfrMaster()
        {
            var rs = _FARMaster.FindAll().ToList();
            return rs;
        }
        public List<Prosol_Funloc> GetFuncLoc()
        {
            var rs = _FuncLocRepository.FindAll().ToList();
            return rs;
        }
        public string InsertMfr(Prosol_FARMaster item)
        {
            var qry = Query.And(
                Query.EQ("Label", item.Label),
                Query.EQ("Code", item.Code)
            );  

            var existing = _FARMaster.FindOne(qry);

            if (existing == null)
            {
                var newMfr = new Prosol_FARMaster
                {
                    Label = item.Label,
                    Code = item.Code,
                    Islive = false
                };

                _FARMaster.Add(newMfr);
                return "Data added successfully";
            }
            else
            {
                if (!existing.Islive)
                {
                    existing.Islive = true;
                    _FARMaster.Add(existing); 
                    return "Data approved successfully";
                }

                return "Data already exists and is active";
            }
        }
        public IEnumerable<Prosol_FARMaster> GetDataList(string Label)
        {
            var query = Query.And(Query.EQ("Label", Label)/*,Query.EQ("Islive", true)*/);
            var lst = _FARMaster.FindAll(query);
            return lst;
        }
        public IEnumerable<Prosol_FARMaster> GetDataList()
        {
            var lst = _FARMaster.FindAll();
            return lst;
        }

        public List<Dictionary<string, object>> DownloadMFR(string id)
        {
            var datalist = new List<Prosol_FARMaster>();
            var query = Query.EQ("Label", id);
            datalist = _FARMaster.FindAll(query).ToList();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (var code in datalist)
            {
                row = new Dictionary<string, object>(); 
                row.Add("Manufacturer Abbrevation", code.Code);
                row.Add("Manufacturer Name", code.Title);
                row.Add("Status", code.Islive == true ? "Approved" : "Pending");
                rows.Add(row);
            }
            return rows;
        }

        public int GetLastRunning(string label)
        {
            int res = -1;
            var query = Query.EQ("Sn", label);
            var runLst = _ReqRunningRepository.FindOne(query);
            res = runLst != null ? runLst.RunningNo : res;
            return res;
        }

        public bool UpdateRunning(string label,int runNo)
        {
            bool res = false;
            var query = Query.EQ("Sn", label);
            var runLst = _ReqRunningRepository.FindOne(query);
            runLst.RunningNo = runLst != null ? runNo : runLst.RunningNo;
            res = _ReqRunningRepository.Add(runLst);
            return res;
        }

        public string GenerateQRCode(QRRequestModel model, out string tagNo)
        {
            tagNo = "3" + (GetLastRunning("VT") + 1).ToString("D7");

            if (string.IsNullOrEmpty(model.fileName))
            {
                model.fileName = model.AssetNo + "_VT_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".png";
            }

            string folderPath = HttpContext.Current.Server.MapPath("~/QRImages/");
            string fullPath = Path.Combine(folderPath, model.fileName);

            var asset = GetAssetInfo(model.AssetNo);
            if (asset.AssetImages != null)
            {
                asset.AssetImages.VirtualTagImage = new[] { model.fileName };
            }
            asset.VirtualTagNo = tagNo;
                insertImages(asset);
                UpdateRunning("VT", int.Parse(tagNo.Substring(1)));

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Generate QR Image
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(tagNo, QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new QRCode(qrCodeData))
            using (var qrImage = qrCode.GetGraphic(20))
            {
                string logoPath = HttpContext.Current.Server.MapPath("~/Images/Ad-logo.png");
                Bitmap logo = new Bitmap(logoPath);

                int finalSize = 400;
                int qrSize = 250;
                Bitmap finalImage = new Bitmap(finalSize, finalSize);

                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    g.Clear(Color.White);
                    g.DrawImage(logo, 0, 0, finalSize, 80);
                    g.DrawImage(qrImage, (finalSize - qrSize) / 2, 90, qrSize, qrSize);

                    using (Font font = new Font("Arial", 36, FontStyle.Bold))
                    using (Brush brush = new SolidBrush(Color.Black))
                    {
                        RectangleF footerRect = new RectangleF(0, 330, finalSize, 50);
                        StringFormat format = new StringFormat { Alignment = StringAlignment.Center };
                        g.DrawString(tagNo, font, brush, footerRect, format);
                    }
                }

                finalImage.Save(fullPath, ImageFormat.Png);
            }

            return model.fileName;
        }

        public List<Prosol_AssetBOM> GetBOM(string id)
        {
            var bomLst = new List<Prosol_AssetBOM>();
            var query = Query.EQ("UniqueId", id);
            bomLst = _AssetBOMRepository.FindAll(query).ToList();
            return bomLst;
        }

        public List<Prosol_AssetBOM> getFLBom(string id)
        {
            var flBomLst = new List<Prosol_AssetBOM>();
            var query = Query.Matches("Func_Location", new BsonRegularExpression("^" + id, "i"));
            flBomLst = _AssetBOMRepository.FindAll(query).ToList();
            return flBomLst;
        }

        public List<Prosol_Funloc> GetFL(string id)
        {
            var flLst = new List<Prosol_Funloc>();
            //var query = Query.Matches("FunctLocation", id);
            flLst = _FuncLocRepository.FindAll().ToList();
            return flLst;
        }
        public List<string> GetAssetValues(string Noun, string Modifier, string Attribute)
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

                var query = Query.And(Query.EQ("Noun", Noun), Query.EQ("Modifier", Modifier), Query.EQ("Characteristic", Attribute), Query.EQ("Definition" , "Equ"));
                var Lstobj = _CharateristicRepository.FindAll(fields, query).ToList();

                if (Lstobj != null && Lstobj.Count > 0 && Lstobj[0].Values != null)
                {

                    var Lst = new List<ObjectId>();
                    foreach (string str in Lstobj[0].Values)
                    {
                        if (str.StartsWith("6"))
                            Lst.Add(new ObjectId(str));
                    }
                    query = Query.In("_id", new BsonArray(Lst));
                    var arrResult = _assetAbbreivateRepository.FindAll(query);
                    foreach (var mdl in arrResult)
                    {
                        Lst1.Add(mdl.Value);

                    }
                }
            }

            return Lst1;
        }

        public List<Prosol_AssetMaster> BindAll()
        {
            string[] strArr = { "Catalogue", "Review", "PVuser", "ItemStatus", };
            var fields = Fields.Include(strArr);
            var lst = new List<Prosol_AssetMaster>();
            lst = _assetRepository.FindAll(fields).ToList();
            return lst;
        }
        public IEnumerable<Prosol_Users> BindUsersByRole(string role)
        {
            var query = Query.And(Query.EQ("Roles.Name", role), Query.EQ("Islive", "Active"), Query.In("Modules", new BsonArray { "Asset" }));
            var gtuser = _UsercreateRepository.FindAll(query).ToList();
            return gtuser;
        }

        public bool Deletefile (string uniqueId, string fileName)
        {
            bool res = false;
            var qry = Query.EQ("UniqueId", uniqueId);
            var lst = _assetRepository.FindOne(qry);
            if(lst != null)
            {
                if (!string.IsNullOrEmpty(lst.Attachment))
                {
                    var aStr = lst.Attachment.Split(',');
                    var nAttch = new List<string>();
                    string attachment = "";
                    foreach (var a in aStr)
                    {
                        if(a != fileName)
                        {
                            nAttch.Add(a);
                        }
                    }
                    attachment = string.Join(",", nAttch);
                    lst.Attachment = attachment;
                    res = _assetRepository.Add(lst);
                } 
            }
            return res;
        }

        //public bool AddAssetValue(string Value, string user)
        //{
        //    bool res = false;

        //    var mdl = new Prosol_AssetAbbrevate();
        //    mdl.Value = Value;
        //    mdl.Abbrevated = Value;
        //    mdl.Value = user;
        //    mdl.Approved = "No";

        //    res = _assetAbbreivateRepository.Add(mdl);

        //    return res;
        //}

    }
    
}
