using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using builder = MongoDB.Driver.Builders;
using System.IO;
using Prosol.Core.Model;
using System.Text.RegularExpressions;
using System.Data;
using System.Globalization;

namespace Prosol.WebApi
{

    public class UserRepository : IUserRepository
    {

        private static string _connectionString = ConfigurationManager.ConnectionStrings["ProsolConnect"].ConnectionString;

        private static MongoServer _server = new MongoClient(_connectionString).GetServer();
        // private string _collectionName;
        private MongoDatabase _db;
        public UserRepository()
        {
            _db = _server.GetDatabase(MongoUrl.Create(_connectionString).DatabaseName);

        }
        public string AuthenUr(string login)
        {

            if (login.Contains(';'))
            {
                string[] loginstr = login.Split(';');
                var qry = Query.And(Query.EQ("EmailId", loginstr[0]), Query.EQ("Password", loginstr[1]));
                var sts = _db.GetCollection<Prosol_Users>("Prosol_Users").FindOne(qry);
                if (sts != null)
                    return "Valid";
                else return "Invalid";
            }
            else return "Invalid";

        }
    
        public IEnumerable<ApiModel> Getstock(string uNme)
        {
            //dbConn conn = new dbConn();
            //List<User> mList = new List<User>();
            //SqlCommand cmdGetMgr = new SqlCommand("sp_Stock", conn.OpenConn());
            //cmdGetMgr.CommandType = CommandType.StoredProcedure;
            //cmdGetMgr.Parameters.AddWithValue("@EmailId", uNme);
            //cmdGetMgr.Parameters.AddWithValue("@Mode", "USERSTOCK_MOB");
            //SqlDataReader sdr = cmdGetMgr.ExecuteReader();
            //while (sdr.Read())
            //{

            //    User mModel = new User();
            //    mModel.Id = Convert.ToInt32(sdr["Id"]);
            //    mModel.UId = Convert.ToInt16(sdr["uId"]);
            //    mModel.Sapcode = Convert.ToString(sdr["Sapcode"]);
            //    mModel.Sysbal = Convert.ToString(sdr["Sysbal"]);
            //    mModel.Storagebin = Convert.ToString(sdr["Storagebin"]);
            //    mModel.QtyasonDate = Convert.ToString(sdr["QtyasonDate"]);
            //    mModel.GRDate = Convert.ToString(sdr["GRDate"]);
            //    mModel.UOM = Convert.ToString(sdr["UOM"]);
            //    mModel.MaterialDesc = Convert.ToString(sdr["MaterialDesc"]);

            //    mModel.Pysbal = Convert.ToString(sdr["Pysbal"]);
            //    mModel.PysicalObservation = Convert.ToString(sdr["PysicalObservation"]);
            //    mModel.PysicalObservationQty = Convert.ToString(sdr["PysicalObservationQty"]);
            //    mModel.SelfLife = Convert.ToString(sdr["SelfLife"]);
            //    mModel.ExpiredDate = Convert.ToString(sdr["ExpiredDate"]);
            //    mModel.ExpiredQty = Convert.ToString(sdr["ExpiredQty"]);
            //    mModel.BinUpdation = Convert.ToString(sdr["BinUpdation"]);

            //    mModel.Model = Convert.ToString(sdr["Model"]);
            //    mModel.Make = Convert.ToString(sdr["Make"]);
            //    mModel.DataCollection = Convert.ToString(sdr["DataCollection"]);
            //    mModel.Additioninfo = Convert.ToString(sdr["Additioninfo"]);
            //    mModel.status = Convert.ToString(sdr["status"]);
            //    mModel.CreatedOn = Convert.ToString(sdr["dte"]);
            //    // mModel.Appimage1 = Convert.ToString(sdr["Appimage1"]);
            //    // mModel.Appimage2 = Convert.ToString(sdr["Appimage2"]);

            //    mList.Add(mModel);


            //}
         //   var vendor = _db.GetCollection<Vendorsuppliers>("Vendorsuppliers").FindOne();


            var mList = new List<ApiModel>();
            try
            {
                var qry = Query.EQ("EmailId", uNme);
                var usrObj = _db.GetCollection<Prosol_Users>("Prosol_Users").FindOne(qry);
                if (usrObj != null)
                {
                    var qury = Query.And(Query.EQ("PVuser.UserId", usrObj.Userid), Query.EQ("PVstatus", "Pending"));
                    var DatamasList = _db.GetCollection<Prosol_Datamaster>("Prosol_Datamaster").Find(qury).ToList();
                    if (DatamasList != null && DatamasList.Count > 0)
                    {
                        foreach (Prosol_Datamaster mdl in DatamasList)
                        {
                            var apiMdl = new ApiModel();
                            apiMdl.BinUpdation = mdl.Bin_Updation_Miss_Placed != null ? mdl.Bin_Updation_Miss_Placed : "";
                            apiMdl.UserId = Convert.ToInt16(usrObj.Userid);
                            apiMdl.Id = Convert.ToInt32(mdl.Itemcode);
                            apiMdl.EmailId = uNme != null ? uNme : "";
                            apiMdl.Sapcode = mdl.Materialcode != null ? mdl.Materialcode : "";
                            apiMdl.Sysbal = mdl.System_Balance != null ? mdl.System_Balance : "";
                            apiMdl.Storagebin = mdl.Storage_Bin1 != null ? mdl.Storage_Bin1 : "";
                            apiMdl.MaterialDesc = mdl.Longdesc != null ? mdl.Longdesc : "";
                            apiMdl.DataCollection = mdl.Specification != null ? mdl.Specification : "";

                            apiMdl.Additioninfo = mdl.Additionalinfo != null ? mdl.Additionalinfo : "";

                            apiMdl.QtyasonDate = mdl.Quantity_as_on_Date;
                            //string DATE1 = DateTime.Parse(mdl.Quantity_as_on_Date.ToString()).ToString("MM-dd -yyyy hh:mm tt");
                            //apiMdl.QtyasonDate = DATE1;

                            apiMdl.UOM = mdl.UOM != null ? mdl.UOM : "";

                            apiMdl.GRDate = mdl.GR_Date;
                            //string DATE2 = DateTime.Parse(mdl.Expired_Date.ToString()).ToString("MM -dd- yyyy hh:mm tt");
                            //apiMdl.ExpiredDate = DATE2;

                            apiMdl.PysicalObservation = mdl.Physical_Observation != null ? mdl.Physical_Observation : "";
                            apiMdl.PysicalObservationQty = mdl.No_of_Item_Aginst_PV_Obs != null ? mdl.No_of_Item_Aginst_PV_Obs : "";
                            apiMdl.SelfLife = mdl.Shelf_Life != null ? mdl.Shelf_Life : "";
                            apiMdl.ExpiredQty = mdl.No_of_Items_Expired != null ? mdl.No_of_Items_Expired : "";
                            apiMdl.ExpiredDate = mdl.Expired_Date;
                            //string DATE3 = DateTime.Parse(mdl.GR_Date.ToString()).ToString("MM dd yyyy hh:mm tt");
                            //apiMdl.GRDate = DATE3;
                            apiMdl.DataCollection = mdl.Specification != null ? mdl.Specification : "";
                            apiMdl.Pysbal = mdl.Stock_Quantity != null ? mdl.Stock_Quantity : "";
                            apiMdl.status = mdl.PVstatus != null ? mdl.PVstatus : "";
                            // var Equi_mdl = new Equipments();
                            if (mdl.Vendorsuppliers != null && mdl.Vendorsuppliers.Count > 0)
                            {

                                apiMdl.Make = mdl.Vendorsuppliers[0].Name != null ? mdl.Vendorsuppliers[0].Name : "";
                                apiMdl.Model = mdl.Vendorsuppliers[0].RefNo != null ? mdl.Vendorsuppliers[0].RefNo : "";

                            }



                            mList.Add(apiMdl);
                        }
                        return mList;
                    }
                    else return mList;

                }
                else return mList;
            }catch(Exception e)
            {
                var mdl = new ApiModel();

                mdl.MaterialDesc = e.ToString();
                mList.Add(mdl);
                return mList;
            }
        }

        //    }
        public string Updatestock(List<ApiModel> MobileReturnList)
        {

            //dbConn conn = new dbConn();
            try
            {
              

                DateTime ucTme1 = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                DateTime UpdatedOn = TimeZoneInfo.ConvertTimeFromUtc(ucTme1, TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time"));
                string desc, datacollection, addinf;
                string ExpiredDate = "";
                //    DataTable datatbl = (DataTable)JsonConvert.DeserializeObject(strdata, (typeof(DataTable)));            


                foreach (ApiModel mdl in MobileReturnList)
                {
                    var qry = Query.EQ("Itemcode", mdl.Id.ToString());
                    var prodata = _db.GetCollection<Prosol_Datamaster>("Prosol_Datamaster").FindOne(qry);

                    if (prodata != null)
                    {

                        prodata.Bin_Updation_Miss_Placed = mdl.BinUpdation;
                        prodata.Materialcode = mdl.Sapcode;
                        prodata.Storage_Bin1 = mdl.Storagebin;

                        prodata.System_Balance = mdl.Sysbal;

                        prodata.Stock_Quantity = mdl.Pysbal;

                        prodata.Legacy2 = mdl.MaterialDesc;
                        prodata.Specification = mdl.DataCollection;
                        prodata.Additionalinfo = mdl.Additioninfo;
                            prodata.Quantity_as_on_Date = mdl.QtyasonDate;
                       // string DATE1 = DateTime.Parse(mdl.QtyasonDate.ToString()).ToString("MMM dd yyyy hh:mm tt");
                       // prodata.Quantity_as_on_Date = DATE1;
                        //var date = DateTime.Parse(mdl.QtyasonDate, new CultureInfo("en-US", true));
                        //date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                        //prodata.Quantity_as_on_Date = date;

                        prodata.UOM = mdl.UOM;
                          prodata.GR_Date = mdl.GRDate;
                        //string DATE2 = DateTime.Parse(mdl.GRDate.ToString()).ToString("MMM dd yyyy hh:mm tt");
                        //prodata.GR_Date = DATE2;
                        //var date1 = DateTime.Parse(mdl.GRDate, new CultureInfo("en-US", true));
                        //date1 = DateTime.SpecifyKind(date1, DateTimeKind.Utc);
                        //prodata.GR_Date = date1;

                        prodata.Physical_Observation = mdl.PysicalObservation;
                        prodata.No_of_Item_Aginst_PV_Obs = mdl.PysicalObservationQty;
                        prodata.Shelf_Life = mdl.SelfLife;
                        prodata.No_of_Items_Expired = mdl.ExpiredQty;
                        prodata.PVstatus = mdl.status;
                         prodata.Expired_Date = mdl.ExpiredDate;
                        prodata.ItemStatus = 13;
                       prodata.Appimage1 = mdl.Appimage1;
                       prodata.Appimage2 = mdl.Appimage2;

                        //string DATE3 = DateTime.Parse(mdl.ExpiredDate.ToString()).ToString("MMM dd yyyy hh:mm tt");
                        //prodata.Expired_Date = DATE3;
                        //var date3 = DateTime.Parse(mdl.ExpiredDate, new CultureInfo("en-US", true));
                        //date3 = DateTime.SpecifyKind(date3, DateTimeKind.Utc);
                        //prodata.Expired_Date = date3;
                        var Makemodel = new Vendorsuppliers();
                        if (mdl.Make != null)
                        {

                            Makemodel.Name = mdl.Make;

                        }
                        if (mdl.Model != null)
                        {

                            Makemodel.RefNo = mdl.Model;

                        }
                        prodata.Vendorsuppliers.Add(Makemodel);

                        var result = _db.GetCollection<Prosol_Datamaster>("Prosol_Datamaster").Save(prodata);

                    }
                }

                return "true";

                // desc = Convert.ToString(drr["MaterialDesc"]).Replace("**", "\"");
                //        //desc = desc.Replace("*", "''");
                //        //// stckRmrk = Convert.ToString(drr["StockRemark"]).Replace("**", "\"");
                //        //// stckRmrk = stckRmrk.Replace("*", "''");
                //        //datacollection = Convert.ToString(drr["DataCollection"]).Replace("**", "\"");
                //        //datacollection = datacollection.Replace("*", "''");
                //        //addinf = Convert.ToString(drr["Additioninfo"]).Replace("**", "\"");
                //        //addinf = addinf.Replace("*", "''");

                //        if (Convert.ToString(drr["ExpiredDate"]) != null && Convert.ToString(drr["ExpiredDate"]) != "")
                //        {
                //            string[] splt = Convert.ToString(drr["ExpiredDate"]).Split('/');
                //            ExpiredDate = splt[1] + "/" + splt[0] + "/" + splt[2];
                //        }
                //        SqlCommand cmdGetMgr = new SqlCommand(@"update tbl_Stock set [Pysbal]='" + drr["Pysbal"] + "'," +
                //            "[MaterialDesc] = '" + drr["MaterialDesc"] + "',[PysicalObservation] = '" + drr["PysicalObservation"] + "',[PysicalObservationQty] = '" + drr["PysicalObservationQty"] + "'," +
                //            "[SelfLife]='" + drr["ShelfLife"] + "',[ExpiredQty] = '" + drr["ExpiredQty"] + "', [ExpiredDate] ='" + ExpiredDate + "',[BinUpdation] = '" + drr["Bup"] + "'," +
                //            "[Appimage1]='" + drr["stock_image_path_one"] + "',[Appimage2]='" + drr["stock_image_path_two"] + "'," +
                //            "[Make]='" + drr["Make"] + "',[Model] = '" + drr["Model"] + "',[Additioninfo]='" + drr["Additioninfo"] + "',[DataCollection]='" + drr["Datagap"] + "',[status]='" + drr["status"] + "',UpdatedOn='" + UpdatedOn + "' where Id=" + drr["Id"], conn.OpenConn());

                //        cmdGetMgr.ExecuteNonQuery();
                //        cmdGetMgr.Dispose();
                //  }
                //    conn.CloseConn();




            }
            catch (Exception ex)
            {


                //   File.WriteAllText(HttpContext.Current.Server.MapPath("~/log/log.txt"), ex.ToString());

                //  conn.CloseConn();
                return ex.ToString();
            }

        }


        public IEnumerable<RfidModel> Getstockrfid()

        {


            var rList = new List<RfidModel>();

            var eqpilst = _db.GetCollection<Prosol_equipbom>("Prosol_equipbom").FindAll().ToList();
            if(eqpilst.Count!= null && eqpilst.Count>0)
            { 
            foreach(Prosol_equipbom eqi in eqpilst)
                {
                    var rfid = new RfidModel();
                    rfid.Materialcode = eqi.Itemcode;

                    rfid.Shortdesc = eqi.Shortdesc;
                    rfid.FunctLocation = eqi.FunctLocation;
                    rfid.TechIdentNo = eqi.TechIdentNo;
                    rList.Add(rfid);

                }
               
            }

            return rList;
        }



        public string insertIN(List<IOModel> List)
        {
            try
            {


                DateTime Createdon = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                //  var IOList = new List<IOModel>();


                if (List != null && List.Count > 0)
                {
                    foreach (IOModel l in List)
                    {

                        var IO = new IOModel();
                        IO.Materialcode = l.Materialcode;
                        IO.INtime = l.INtime;
                        IO.FunctLocation = l.FunctLocation;
                        IO.TechIdentNo = l.TechIdentNo;
                        IO.Createdon = Createdon;
                    //        IOList.Add(IO);
                        var result = _db.GetCollection<Prosol_IOModel>("Prosol_IOModel").Insert(IO);
                    }

                }

              //  var result = _db.GetCollection<Prosol_IOModel>("Prosol_IOModel").Insert(IOList);
              return "true";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
         
        }
        public string insertOUT(List<IOModel> List1)
        {
            try
            {


                DateTime Createdon = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                //  var IOList1 = new List<IOModel>();


                if (List1 != null && List1.Count > 0)
                {
                    foreach (IOModel l in List1)
                    {
                        var IO = new IOModel();
                        IO.Materialcode = l.Materialcode;
                        IO.OUTtime = l.OUTtime;
                        IO.FunctLocation = l.FunctLocation;
                        IO.TechIdentNo = l.TechIdentNo;
                        IO.Createdon = Createdon;
                        //IOList1.Add(IO);

                        var result = _db.GetCollection<Prosol_IOModel>("Prosol_IOModel").Insert(IO);
                    }

                }

              //  var result = _db.GetCollection<Prosol_IOModel>("Prosol_IOModel").Insert(IOList1);


                return "true";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

    }
}