using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using Prosol.Common;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using MongoDB.Driver;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Prosol.Core
{
    public partial class UserCreateService : IUserCreate
    {

        private readonly IRepository<Prosol_Users> _UsercreateRepository;
        private readonly IRepository<Prosol_Plants> _UserplantRepository;
        private readonly IRepository<Prosol_Departments> _UserdepartmentRepository;
        private readonly IRepository<Prosol_Rolepage> _UserrolepageRepository;
        private readonly IRepository<Prosol_Datamaster> _DatamasterRepository;
        private readonly IRepository<Prosol_Request> _ItemRequest_Request_Repository;
        private readonly IRepository<Prosol_RequestService> _ItemRequestservice_Repository;
        //private readonly IRepository<Prosol_Pages> _UserpageRepository;

        public UserCreateService(IRepository<Prosol_Users> Usercreateservice,
            IRepository<Prosol_Departments> Userdeptservice,
            IRepository<Prosol_Plants> Userplantervice,
            IRepository<Prosol_Rolepage> Userrolepageservice,
             IRepository<Prosol_Datamaster> datamasterRepository,
              IRepository<Prosol_Request> ItemRequest_Request_Repository,
              IRepository<Prosol_RequestService> ItemRequestservice_Repository)
        {
            this._UsercreateRepository = Usercreateservice;
            this._UserplantRepository = Userplantervice;
            this._UserdepartmentRepository = Userdeptservice;
            this._UserrolepageRepository = Userrolepageservice;
            this._DatamasterRepository = datamasterRepository;
            this._ItemRequest_Request_Repository = ItemRequest_Request_Repository;
            this._ItemRequestservice_Repository = ItemRequestservice_Repository;
            //this._UseraccessRepository = Useraccessservice;
            //this._UserpageRepository = Userpageservice;
        }




        public IEnumerable<Prosol_Users> getuser(string[] plants)
        {
            var query = Query.And(Query.EQ("Islive", "Active"), Query.All("Plantcode",new BsonArray(plants)));
            string[] Userfield = { "Userid", "UserName","Roles" };
            var fields = Fields.Include(Userfield).Exclude("_id");
            var gtuser = _UsercreateRepository.FindAll(query).ToList();
            return gtuser;
        }
        public IEnumerable<Prosol_Users> showall_user()
        {
            string[] Userfield = { "UserName", "EmailId","Plantcode", "Islive", "Userid","Roles" };
            var fields = Fields.Include(Userfield).Exclude("_id");
            var shwusr = _UsercreateRepository.FindAll(fields).ToList();
            return shwusr;
        }
        public IEnumerable<Prosol_Users> showall_user(string[] plant)
        {
          //  var query = Query.And(Query.In("Plantcode", new BsonArray(plant)), Query.EQ("Islive", "Active"));
            var query = Query.In("Plantcode", new BsonArray(plant));
            string[] Userfield = { "UserName", "EmailId", "Plantcode", "Islive", "Userid", "Roles","Departmentname" };
            var fields = Fields.Include(Userfield).Exclude("_id");
            var shwusr = _UsercreateRepository.FindAll(fields, query).ToList();
            return shwusr;
        }
        public IEnumerable<Prosol_Departments> getdepartment()
        {
            var query = Query.EQ("Islive", true);
            var sort = SortBy.Ascending("Departmentname");
            string[] depfield = { "Departmentname" };
            var fields = Fields.Include(depfield).Exclude("_id");
            var gtdepartment = _UserdepartmentRepository.FindAll(fields,query).ToList();
            return gtdepartment;

        }

        public IEnumerable<Prosol_Plants> getplant()
        {

            var sort = SortBy.Ascending("Plantcode");
            string[] plntfield = { "Plantcode", "Plantname" };
            var fields = Fields.Include(plntfield).Exclude("_id");
            var query = Query.EQ("Islive", true);
            var gtplant = _UserplantRepository.FindAll(fields, query).ToList();
            return gtplant;
        }
        public int getmaxid()
        {

            var sort = SortBy.Ascending("_id");
            string[] strArr = { "Userid" };
            // var fields = Fields.Include(strArr).Exclude("_id");
            var fields = Fields.Include(strArr);
            var temp = _UsercreateRepository.FindAll(fields,sort).ToList();
            if (temp.Count > 0)
            {
                var res = temp[temp.Count - 1].Userid;
                Convert.ToInt16(res);
                return Convert.ToInt16(res);
            }
            else
            {
                return 0;
            }
        }
        public bool save(Prosol_Users usrcm)
        {
            var res = _UsercreateRepository.Add(usrcm);
            return res;
        }

        public IEnumerable<Prosol_Users> checkusername_avalibility(string UserName)
        {
            string[] pagefield = { "Userid", "UserName" };
            var fields = Fields.Include(pagefield).Exclude("_id");
            var query = Query.EQ("UserName", UserName);
            var res = _UsercreateRepository.FindAll(fields, query).ToList();

            return res;

        }

        public IEnumerable<Prosol_Users> checkemailid_avalibility(string EmailId)
        {
            string[] pagefield = { "Userid", "EmailId" };
            var fields = Fields.Include(pagefield).Exclude("_id");
            var query =Query.EQ("EmailId", EmailId);
            var res1 = _UsercreateRepository.FindAll(fields, query).ToList();
            return res1;

        }


        public IEnumerable<Prosol_Users> getforupdate(string id)
        {
            string[] pagefield = { "FirstName", "LastName", "EmailId", "Mobile", "UserName", "Password", "Islive", "Departmentname", "Usertype", "Plantcode","Roles","Modules" };
            var fields = Fields.Include(pagefield).Exclude("_id");
            //var fields = Fields.Include(pagefield).Exclude("_id");
            var query =Query.EQ("Userid", id.ToString());
            var mx = _UsercreateRepository.FindAll(query).ToList();
            return mx;
        }


        public IEnumerable<Prosol_Rolepage> getrolepage(string Role, string Module)
        {
            var query = Query.And(Query.EQ("Role", Role.ToString()),Query.EQ("Module", Module));
            var mx = _UserrolepageRepository.FindAll(query).ToList();
            return mx;
        }

        public bool setforupdate(string id, Prosol_Users userup)
        {
         //   var query = Query.EQ("Userid", id.ToString());
         //   var Updae = Update.Set("FirstName", userup.FirstName).Set("LastName", userup.LastName).Set("EmailId", userup.EmailId).Set("Mobile", userup.Mobile).Set
         //("UserName", userup.UserName).Set("Password", userup.Password).Set("Roles",new BsonArray(userup.Roles)).Set("Islive", userup.Islive).Set("Departmentname", userup.Departmentname).Set("Createdon",DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)).Set("Plantcode", userup.Plantcode);
         // var flg = UpdateFlags.Upsert;
         //   var mx = _UsercreateRepository.Update(query, Updae, flg);

            var mx = _UsercreateRepository.Add(userup);

            return true;
        }

        public IEnumerable<Prosol_Users> getReviewerList()
        {
            var query = Query.And(Query.EQ("Islive", "Active"), Query.EQ("Usertype", "Reviewer"));
            var shwusr = _UsercreateRepository.FindAll(query).ToList();
            return shwusr;
        }
        public virtual Prosol_Users getimage(string id)
        {
            var query = Query.And(Query.EQ("Userid", id), Query.EQ("Islive", "Active"));
            var Nm = _UsercreateRepository.FindOne(query);
            if (Nm!=null && Nm.ImageId != null)
            {
                var query1 = Query.EQ("_id", new ObjectId(Nm.ImageId));
                byte[] byt = _UsercreateRepository.GridFsFindOne(query1);
                if (byt != null)
                    Nm.FileData = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(byt));
            }
            return Nm;
        }
        public virtual Prosol_Users getcleansername(string id)
        {
            var query = Query.And(Query.EQ("Userid", id), Query.EQ("Islive", "Active"));
            var Nm = _UsercreateRepository.FindOne(query);
            return Nm;
        }
        public virtual Prosol_Users Getreqinfo(string id)
        {
            var query = Query.And(Query.EQ("Userid", id));
            var Nm = _UsercreateRepository.FindOne(query);
            return Nm;
        }

        public IEnumerable<Prosol_Users> AccountUser(string id)
        {
           

            string[] Userfield = { "FirstName", "LastName", "EmailId", "Mobile", "UserName", "Password", "Islive", "Departmentname", "Usertype", "Plantcode" , "Roles" };
            var fields = Fields.Include(Userfield).Exclude("_id");
            var query = Query.And(Query.EQ("Islive", "Active"), Query.EQ("Userid", id));
            var actusr = _UsercreateRepository.FindAll(fields, query).ToList();
            
            return actusr;
        }

        public bool Profilesubmit(string id, Prosol_Users prf, HttpPostedFileBase file)
        {
            var queryfind = Query.And(Query.EQ("Userid", id.ToString()));
            var ObjStr = _UsercreateRepository.FindOne(queryfind);
            //NM.UpdatedOn =DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            //NM._id = ObjStr != null ? ObjStr._id : new ObjectId();
            var ImageId = ObjStr != null ? ObjStr.ImageId : null;

            if (file != null && file.ContentLength > 0 && file.ContentType.Contains("image"))
            {
                if (ImageId != null)
                    _UsercreateRepository.GridFsDel(Query.EQ("_id", new ObjectId(ImageId)));

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
                ImageId = _UsercreateRepository.GridFsUpload(strm, id.ToString());
            }
            var query = Query.EQ("Userid", id.ToString());
           // var Updae = Update.Set("FirstName", prf.FirstName).Set("LastName", prf.LastName).Set("Mobile", prf.Mobile).Set("Createdon",DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);).Set("ImageId", ImageId);
            var Updae = Update.Set("FirstName", prf.FirstName).Set("LastName", prf.LastName).Set("Mobile", prf.Mobile).Set("Createdon",DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));
            var flg = UpdateFlags.Upsert;
            var mx = _UsercreateRepository.Update(query, Updae, flg);
            return true;
        }

        public bool Changepasswordsubmit(string id, string  Password)
        {
            var query = Query.EQ("Userid", id.ToString());
            var Updae = Update.Set("Password", Password).Set("Createdon",DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));
            var flg = UpdateFlags.Upsert;
            var mx = _UsercreateRepository.Update(query, Updae, flg);
            return true;
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
        public List<ItemStatusMap> getItemstatusmap(string Itemcode)
        {


            List<ItemStatusMap> showMapLst = new List<ItemStatusMap>();
            ItemStatusMap mdl = new ItemStatusMap();
            var qry = Query.EQ("itemId", Itemcode);
            var itmreq = _ItemRequest_Request_Repository.FindOne(qry);
            if (itmreq != null)
            {
                var qryUser = Query.EQ("Userid", itmreq.requester);
                var UsrRes = _UsercreateRepository.FindOne(qryUser);

                mdl.RequestId = itmreq.requestId;
                mdl.Itemcode = itmreq.itemId;
                mdl.Date = DateTime.Parse(itmreq.requestedOn.ToString()).ToString("MMM dd yyyy hh:mm tt");
                mdl.UserName = UsrRes.UserName;
                mdl.Role = "Requester";
                if (itmreq.itemStatus == "pending" || itmreq.itemStatus == "approved")
                    mdl.Status = 2;
                else mdl.Status = -1;
                showMapLst.Add(mdl);

                qryUser = Query.EQ("Userid", itmreq.approver);
                UsrRes = _UsercreateRepository.FindOne(qryUser);
                mdl = new ItemStatusMap();

                if (itmreq.itemStatus == "rejected")
                    mdl.Date = itmreq.rejectedOn != null ? DateTime.Parse(itmreq.rejectedOn.ToString()).ToString("MMM dd yyyy hh:mm tt") : "";
                else
                    mdl.Date = itmreq.approvedOn != null ? DateTime.Parse(itmreq.approvedOn.ToString()).ToString("MMM dd yyyy hh:mm tt") : "";

                mdl.UserName = UsrRes.UserName;
                mdl.Role = "Approver";
                if (itmreq.itemStatus == "pending")
                    mdl.Status = 1;
                else if (itmreq.itemStatus == "approved")
                    mdl.Status = 2;
                else mdl.Status = -1;

                showMapLst.Add(mdl);

            }
            else
            {
                qry = Query.EQ("Itemcode", Itemcode);
                var dataMaster1 = _ItemRequest_Request_Repository.FindAll(qry).ToList();
                if (dataMaster1.Count > 0)
                {
                    Prosol_Request datamaster1 = new Prosol_Request();

                    var rr = Query.EQ("Userid", datamaster1.requester);
                    var tt = _UsercreateRepository.FindAll(rr).ToList();


                    mdl = new ItemStatusMap();
                    mdl.Date = "";
                    mdl.UserName = tt[0].UserName;
                    mdl.Role = "Requester";
                    mdl.Status = 0;
                    showMapLst.Add(mdl);



                }
                else
                {
                    mdl = new ItemStatusMap();
                    mdl.Itemcode = Itemcode;
                    mdl.Date = "";
                    mdl.UserName = "";
                    mdl.Role = "Requester";
                    mdl.Status = 0;
                    showMapLst.Add(mdl);

                    mdl = new ItemStatusMap();
                    mdl.Itemcode = Itemcode;
                    mdl.Date = "";
                    mdl.UserName = "";
                    mdl.Role = "Approver";
                    mdl.Status = 0;
                    showMapLst.Add(mdl);
                }
            }

            qry = Query.EQ("Itemcode", Itemcode);
            var dataMaster = _DatamasterRepository.FindOne(qry);


            if (dataMaster != null)
            {
                mdl = new ItemStatusMap();

                mdl.Date = DateTime.Parse(dataMaster.Catalogue.UpdatedOn.ToString()).ToString("MMM dd yyyy hh:mm tt");
                mdl.UserName = dataMaster.Catalogue.Name;
                mdl.Role = "Cataloguer";
                if ((dataMaster.ItemStatus == 0 || dataMaster.ItemStatus == 1) && (dataMaster.Reworkcat == "" || dataMaster.Reworkcat == null))
                    mdl.Status = 1;
                else if ((dataMaster.Reworkcat != "" || dataMaster.Reworkcat != null) && dataMaster.ItemStatus == 0)
                    mdl.Status = -1;
                else mdl.Status = 2;

                showMapLst.Add(mdl);

                if (dataMaster.Review != null && (dataMaster.ItemStatus > 1 || (dataMaster.Reworkcat != "" && dataMaster.Reworkcat != null)))
                {
                    mdl = new ItemStatusMap();
                    mdl.Date = dataMaster.Review.UpdatedOn != null ? DateTime.Parse(dataMaster.Review.UpdatedOn.ToString()).ToString("MMM dd yyyy hh:mm tt") : "";
                    mdl.UserName = dataMaster.Review.Name;
                    mdl.Role = "Reviewer";
                    if ((dataMaster.ItemStatus == 2 || dataMaster.ItemStatus == 3) && (dataMaster.Rework == "" || dataMaster.Rework == null))
                        mdl.Status = 1;
                    else if (((dataMaster.Rework != "" || dataMaster.Rework != null) && (dataMaster.ItemStatus == 2 || dataMaster.ItemStatus == 0)))
                        mdl.Status = -1;
                    // else if (((dataMaster.Reworkcat != "" || dataMaster.Reworkcat != null) && dataMaster.ItemStatus == 0))
                    //  mdl.Status = -2;                   
                    else mdl.Status = 2;
                    showMapLst.Add(mdl);

                }
                else
                {
                    qry = Query.EQ("Itemcode", Itemcode);
                    var dataMaster1 = _DatamasterRepository.FindOne(qry);
                    Prosol_Users datamaster1 = new Prosol_Users();
                    var qryUser1 = Query.EQ("UserName", dataMaster1.Catalogue.Name);
                    var mx = _UsercreateRepository.FindAll(qryUser1).ToList();

                    var _dbLstRole1 = new List<TargetExn>();
                    foreach (TargetExn ext in mx[0].Roles)
                    {
                        var _dbMdlRole1 = new TargetExn();
                        _dbMdlRole1.Name = ext.Name;
                        _dbMdlRole1.TargetId = ext.TargetId;
                        _dbLstRole1.Add(_dbMdlRole1);

                        if (_dbMdlRole1.Name == "Cataloguer")
                        {
                            var rr = Query.EQ("Userid", _dbMdlRole1.TargetId);
                            var tt = _UsercreateRepository.FindAll(rr).ToList();


                            mdl = new ItemStatusMap();
                            mdl.Date = "EST:-" + DateTime.Parse(dataMaster.Catalogue.UpdatedOn.ToString()).AddHours(1).ToString("MMM dd yyyy hh:mm tt");
                            mdl.UserName = tt[0].UserName;
                            //  mdl.Role = "Cataloguer";
                            mdl.Status = 0;
                            showMapLst.Add(mdl);


                            var qryUser2 = Query.EQ("UserName", tt[0].UserName);
                            var mx1 = _UsercreateRepository.FindAll(qryUser2).ToList();
                            var _dbLstRole2 = new List<TargetExn>();
                            foreach (TargetExn ext1 in mx1[0].Roles)
                            {
                                var _dbMdlRole2 = new TargetExn();
                                _dbMdlRole2.Name = ext1.Name;
                                _dbMdlRole2.TargetId = ext1.TargetId;
                                _dbLstRole2.Add(_dbMdlRole2);

                                if (_dbMdlRole2.Name == "Reviewer")
                                {
                                    var rr1 = Query.EQ("Userid", _dbMdlRole2.TargetId);
                                    var tt1 = _UsercreateRepository.FindAll(rr1).ToList();


                                    mdl = new ItemStatusMap();
                                    mdl.Date = "EST-" + DateTime.Parse(dataMaster.Catalogue.UpdatedOn.ToString()).AddHours(2).ToString("MMM dd yyyy hh:mm tt");
                                    mdl.UserName = tt1[0].UserName;
                                    //  mdl.Role = "Cataloguer";
                                    mdl.Status = 0;
                                    showMapLst.Add(mdl);
                                }
                            }
                        }

                    }

                }
                if (dataMaster.Release != null && (dataMaster.ItemStatus > 3 || (dataMaster.Rework != "" && dataMaster.Rework != null)))
                {
                    mdl = new ItemStatusMap();
                    mdl.Date = dataMaster.Release.UpdatedOn != null ? DateTime.Parse(dataMaster.Release.UpdatedOn.ToString()).ToString("MMM dd yyyy hh:mm tt") : "";
                    mdl.UserName = dataMaster.Release.Name;
                    mdl.Role = "Releaser";
                    if ((dataMaster.ItemStatus == 4 || dataMaster.ItemStatus == 5))
                        mdl.Status = 1;
                    else if ((dataMaster.Rework != "" && dataMaster.Rework != null && dataMaster.ItemStatus == 2) || dataMaster.Reworkcat != "" && dataMaster.Reworkcat != null && dataMaster.ItemStatus == 0)
                        mdl.Status = -1;
                    else mdl.Status = 2;
                    showMapLst.Add(mdl);

                }
                //else
                //{

                //}
                else
                {
                    if (showMapLst.Count != 5)
                    {
                        qry = Query.EQ("Itemcode", Itemcode);
                        var dataMaster5 = _DatamasterRepository.FindOne(qry);
                        // Prosol_Users datamaster5 = new Prosol_Users();
                        if (dataMaster5 != null && dataMaster5.Review!=null)
                        {
                            var qryUser5 = Query.EQ("UserName", dataMaster5.Review.Name);
                            var xx = _UsercreateRepository.FindAll(qryUser5).ToList();

                            var _dbLstRole5 = new List<TargetExn>();
                            foreach (TargetExn ext5 in xx[0].Roles)
                            {
                                var _dbMdlRole5 = new TargetExn();
                                _dbMdlRole5.Name = ext5.Name;
                                _dbMdlRole5.TargetId = ext5.TargetId;
                                _dbLstRole5.Add(_dbMdlRole5);

                                if (_dbMdlRole5.Name == "Reviewer")
                                {
                                    var rr5 = Query.EQ("Userid", _dbMdlRole5.TargetId);
                                    var tt5 = _UsercreateRepository.FindAll(rr5).ToList();


                                    mdl = new ItemStatusMap();
                                    mdl.Date = "EST-" + DateTime.Parse(dataMaster.Review.UpdatedOn.ToString()).AddHours(1).ToString("MMM dd yyyy hh:mm tt");
                                    mdl.UserName = tt5[0].UserName;
                                    //  mdl.Role = "Cataloguer";
                                    mdl.Status = 0;
                                    showMapLst.Add(mdl);

                                }

                            }
                        }

                    }

                }
            }
            else
            {
                {

                    qry = Query.EQ("itemId", Itemcode);
                    var dataMaster1 = _ItemRequest_Request_Repository.FindOne(qry);
                    Prosol_Users datamaster1 = new Prosol_Users();
                    var qryUser1 = Query.EQ("Userid", dataMaster1.approver);
                    var mx = _UsercreateRepository.FindAll(qryUser1).ToList();

                    var _dbLstRole1 = new List<TargetExn>();
                    foreach (TargetExn ext in mx[0].Roles)
                    {
                        var _dbMdlRole1 = new TargetExn();
                        _dbMdlRole1.Name = ext.Name;
                        _dbMdlRole1.TargetId = ext.TargetId;
                        _dbLstRole1.Add(_dbMdlRole1);

                        if (_dbMdlRole1.Name == "Approver")
                        {
                            var rr = Query.EQ("Userid", _dbMdlRole1.TargetId);
                            var tt = _UsercreateRepository.FindAll(rr).ToList();


                            mdl = new ItemStatusMap();
                            mdl.Date = "EST - " + DateTime.Parse(dataMaster1.requestedOn.ToString()).AddHours(1).ToString("MMM dd yyyy hh:mm tt");
                            mdl.UserName = tt[0].UserName;
                            //  mdl.Role = "Cataloguer";
                            mdl.Status = 0;
                            showMapLst.Add(mdl);


                            var qryUser2 = Query.EQ("UserName", tt[0].UserName);
                            var mx1 = _UsercreateRepository.FindAll(qryUser2).ToList();
                            var _dbLstRole2 = new List<TargetExn>();
                            foreach (TargetExn ext1 in mx1[0].Roles)
                            {
                                var _dbMdlRole2 = new TargetExn();
                                _dbMdlRole2.Name = ext1.Name;
                                _dbMdlRole2.TargetId = ext1.TargetId;
                                _dbLstRole2.Add(_dbMdlRole2);

                                if (_dbMdlRole2.Name == "Cataloguer")
                                {
                                    var rr1 = Query.EQ("Userid", _dbMdlRole2.TargetId);
                                    var tt1 = _UsercreateRepository.FindAll(rr1).ToList();


                                    mdl = new ItemStatusMap();
                                    mdl.Date = "EST - " + DateTime.Parse(dataMaster1.requestedOn.ToString()).AddHours(2).ToString("MMM dd yyyy hh:mm tt");
                                    mdl.UserName = tt1[0].UserName;
                                    //  mdl.Role = "Cataloguer";
                                    mdl.Status = 0;
                                    showMapLst.Add(mdl);
                                    var qryUser3 = Query.EQ("UserName", tt1[0].UserName);
                                    var mx3 = _UsercreateRepository.FindAll(qryUser3).ToList();
                                    var _dbLstRole4 = new List<TargetExn>();
                                    foreach (TargetExn ext4 in mx3[0].Roles)
                                    {
                                        var _dbMdlRole4 = new TargetExn();
                                        _dbMdlRole4.Name = ext4.Name;
                                        _dbMdlRole4.TargetId = ext4.TargetId;
                                        _dbLstRole4.Add(_dbMdlRole4);

                                        if (_dbMdlRole4.Name == "Reviewer")
                                        {
                                            var rr2 = Query.EQ("Userid", _dbMdlRole4.TargetId);
                                            var tt2 = _UsercreateRepository.FindAll(rr2).ToList();


                                            mdl = new ItemStatusMap();
                                            mdl.Date = "EST - " + DateTime.Parse(dataMaster1.requestedOn.ToString()).AddHours(3).ToString("MMM dd yyyy hh:mm tt");
                                            mdl.UserName = tt2[0].UserName;
                                            //  mdl.Role = "Cataloguer";
                                            mdl.Status = 0;
                                            showMapLst.Add(mdl);
                                        }
                                    }
                                }
                            }
                        }

                    }

                }
                //mdl = new ItemStatusMap();
                //mdl.Date = "";
                //mdl.UserName = "";
                //mdl.Role = "Cataloguer";
                //mdl.Status = 0;
                //showMapLst.Add(mdl);

                //mdl = new ItemStatusMap();
                //mdl.Date = "";
                //mdl.UserName = "";
                //mdl.Role = "Releaser";
                //mdl.Status = 0;
                //showMapLst.Add(mdl);

                //mdl = new ItemStatusMap();
                //mdl.Date = "";
                //mdl.UserName = "";
                //mdl.Role = "Releaser";
                //mdl.Status = 0;
                //showMapLst.Add(mdl);

            }

            return showMapLst;

        }
        //public List<ItemStatusMap> getItemstatusmap(string Itemcode)
        //{
        //    List<ItemStatusMap> showMapLst = new List<ItemStatusMap>();
        //    ItemStatusMap mdl = new ItemStatusMap();
        //    var qry = Query.EQ("itemId", Itemcode);
        //    var itmreq = _ItemRequest_Request_Repository.FindOne(qry);
        //    if (itmreq != null)
        //    {
        //        var qryUser = Query.EQ("Userid", itmreq.requester);
        //        var UsrRes = _UsercreateRepository.FindOne(qryUser);

        //        mdl.RequestId = itmreq.requestId;
        //        mdl.Itemcode = itmreq.itemId;
        //        mdl.Date = DateTime.Parse(itmreq.requestedOn.ToString()).ToString("MMM dd yyyy hh:mm tt");
        //        mdl.UserName = UsrRes.UserName;
        //        mdl.Role = "Requester";
        //        if (itmreq.itemStatus == "pending" || itmreq.itemStatus == "approved")
        //            mdl.Status = 2;
        //        else mdl.Status = -1;
        //        showMapLst.Add(mdl);

        //        qryUser = Query.EQ("Userid", itmreq.approver);
        //        UsrRes = _UsercreateRepository.FindOne(qryUser);
        //        mdl = new ItemStatusMap();

        //        if (itmreq.itemStatus == "rejected")
        //            mdl.Date = itmreq.rejectedOn != null ? DateTime.Parse(itmreq.rejectedOn.ToString()).ToString("MMM dd yyyy hh:mm tt") : "";
        //        else
        //            mdl.Date = itmreq.approvedOn != null ? DateTime.Parse(itmreq.approvedOn.ToString()).ToString("MMM dd yyyy hh:mm tt") : "";

        //        mdl.UserName = UsrRes.UserName;
        //        mdl.Role = "Approver";
        //        if (itmreq.itemStatus == "pending")
        //            mdl.Status = 1;
        //        else if (itmreq.itemStatus == "approved")
        //            mdl.Status = 2;
        //        else mdl.Status = -1;

        //        showMapLst.Add(mdl);

        //    }else
        //    {


        //        mdl = new ItemStatusMap();
        //        mdl.Itemcode = Itemcode;
        //        mdl.Date = "";
        //        mdl.UserName = "";
        //        mdl.Role = "Requester";
        //        mdl.Status = 0;
        //        showMapLst.Add(mdl);

        //        mdl = new ItemStatusMap();
        //        mdl.Date = "";
        //        mdl.UserName = "";
        //        mdl.Role = "Approver";
        //        mdl.Status = 0;
        //        showMapLst.Add(mdl);

        //    }
        //    qry = Query.EQ("Itemcode", Itemcode);
        //    var dataMaster = _DatamasterRepository.FindOne(qry);
        //    if (dataMaster != null)
        //    {
        //        mdl = new ItemStatusMap();

        //        mdl.Date = DateTime.Parse(dataMaster.Catalogue.UpdatedOn.ToString()).ToString("MMM dd yyyy hh:mm tt");
        //        mdl.UserName = dataMaster.Catalogue.Name;
        //        mdl.Role = "Cataloguer";
        //        if ((dataMaster.ItemStatus == 0 || dataMaster.ItemStatus == 1) && (dataMaster.Reworkcat == "" || dataMaster.Reworkcat == null))
        //            mdl.Status = 1;
        //        else if ((dataMaster.Reworkcat != "" || dataMaster.Reworkcat != null) && dataMaster.ItemStatus == 0)
        //            mdl.Status = -1;
        //        else mdl.Status = 2;

        //        showMapLst.Add(mdl);

        //        if (dataMaster.Review != null && (dataMaster.ItemStatus > 1 || (dataMaster.Reworkcat != "" && dataMaster.Reworkcat != null)))
        //        {
        //            mdl = new ItemStatusMap();
        //            mdl.Date = dataMaster.Review.UpdatedOn != null ? DateTime.Parse(dataMaster.Review.UpdatedOn.ToString()).ToString("MMM dd yyyy hh:mm tt") : "";
        //            mdl.UserName = dataMaster.Review.Name;
        //            mdl.Role = "Reviewer";
        //            if ((dataMaster.ItemStatus == 2 || dataMaster.ItemStatus == 3) && (dataMaster.Rework == "" || dataMaster.Rework == null))
        //                mdl.Status = 1;
        //            else if (((dataMaster.Rework != "" || dataMaster.Rework != null) && (dataMaster.ItemStatus == 2 || dataMaster.ItemStatus == 0)))
        //                mdl.Status = -1;
        //           // else if (((dataMaster.Reworkcat != "" || dataMaster.Reworkcat != null) && dataMaster.ItemStatus == 0))
        //              //  mdl.Status = -2;                   
        //            else mdl.Status = 2;
        //            showMapLst.Add(mdl);

        //        }
        //        else
        //        {
        //            mdl = new ItemStatusMap();
        //            mdl.Date = "";
        //            mdl.UserName = "";
        //            mdl.Role = "Reviewer";
        //            mdl.Status = 0;
        //            showMapLst.Add(mdl);


        //        }
        //        if (dataMaster.Release != null && (dataMaster.ItemStatus > 3 || (dataMaster.Rework != "" && dataMaster.Rework != null)))
        //        {
        //            mdl = new ItemStatusMap();
        //            mdl.Date = dataMaster.Release.UpdatedOn != null ? DateTime.Parse(dataMaster.Release.UpdatedOn.ToString()).ToString("MMM dd yyyy hh:mm tt") : "";
        //            mdl.UserName = dataMaster.Release.Name;
        //            mdl.Role = "Releaser";
        //            if ((dataMaster.ItemStatus == 4 || dataMaster.ItemStatus == 5))
        //                mdl.Status = 1;
        //            else if ((dataMaster.Rework != "" && dataMaster.Rework != null && dataMaster.ItemStatus==2) || dataMaster.Reworkcat != "" && dataMaster.Reworkcat != null && dataMaster.ItemStatus == 0)
        //                mdl.Status = -1;
        //            else mdl.Status = 2;
        //            showMapLst.Add(mdl);

        //        }
        //        else
        //        {
        //            mdl = new ItemStatusMap();
        //            mdl.Date = "";
        //            mdl.UserName = "";
        //            mdl.Role = "Releaser";
        //            mdl.Status = 0;
        //            showMapLst.Add(mdl);


        //        }
        //    }
        //    else
        //    {
        //        mdl = new ItemStatusMap();
        //        mdl.Date = "";
        //        mdl.UserName = "";
        //        mdl.Role = "Cataloguer";
        //        mdl.Status = 0;
        //        showMapLst.Add(mdl);

        //        mdl = new ItemStatusMap();
        //        mdl.Date = "";
        //        mdl.UserName = "";
        //        mdl.Role = "Releaser";
        //        mdl.Status = 0;
        //        showMapLst.Add(mdl);

        //        mdl = new ItemStatusMap();
        //        mdl.Date = "";
        //        mdl.UserName = "";
        //        mdl.Role = "Releaser";
        //        mdl.Status = 0;
        //        showMapLst.Add(mdl);

        //    }

        //    return showMapLst;
        //}
    }
}
