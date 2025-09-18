using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;

namespace Prosol.Core
{
    public class EmailSettingService : IEmailSettings
    {
        private readonly IRepository<Prosol_EmailSettings> _EmailRepository;      

        public EmailSettingService(IRepository<Prosol_EmailSettings> EmailRepository)
        {
            this._EmailRepository = EmailRepository;
        }
        public string getmailbody( DataTable tbl)           
        {

            if (tbl.Rows.Count > 0)
            {
                string str1 = "<html><body><table style='border: 1px solid black;border-collapse:collapse'><tr><th style='border:1px solid black'>S.No</th>";
                foreach (DataColumn dcl in tbl.Columns)
                {
                    str1 += "<th style='border:1px solid black'>" + dcl.ColumnName + "</th>";

                }
                str1 += "</tr>";
                int inc = 1;
                foreach (DataRow drw in tbl.Rows)
                {
                    str1 += "<tr><td style='border:1px solid black'>" + inc++ + "</td>";
                    foreach (DataColumn dcl in tbl.Columns)
                    {
                        str1 += "<td style='border:1px solid black'>" + drw[dcl.ColumnName] + "</td>";

                    }
                    str1 += "</tr>";

                }
                str1 += "</table>";

                str1 += "<br/><br/> Warm Regards,<br/> MDM Application Team";
                str1 += "<br/><br/><br/>------------------------------------------------------------------------------------------------<br/>";
                str1 += "This is an automatically generated email, please do not reply directly to it.";
                str1 += " </body></html>";
                return str1;
            }
            else return "";

            //DataTable dt = new DataTable(); // Runtime Datatable  
            //dt.Columns.Add("Values", typeof(string)); // Adding dynamic column  
            //if (objects != null && objects.Count > 0)
            //{
            //    for (int i = 0; i < objects.Count; i++)
            //    {
            //        DataRow dr = dt.NewRow(); // Adding values to Datatable  
            //        dr["Values"] = objects[i];
            //        dt.Rows.Add(dr);
            //    }

            //    return dt.ToString(); // Converted Dynamic list to Datatable  
            //}
            // return null;
        }

        public bool insertemaildata(Prosol_EmailSettings data, int update)
        {
            var res = _EmailRepository.FindOne();
            if (res != null)
            {
                res.FromId = data.FromId;
                res.Password = data.Password;
                res.ConformPassword = data.ConformPassword;
                res.PortNo = data.PortNo;
                res.SMTPServerName = data.SMTPServerName;
                res.CCC = data.CCC;
                res.BCCC = data.BCCC;
                res.EnableSSL = data.EnableSSL;
                return _EmailRepository.Add(res);

            }
            else
            {
                return _EmailRepository.Add(data);
            }

        }
        public string emailTest(string to_mail, string subjectt, string body)
        {
            try
            {
               
                var data = _EmailRepository.FindAll().ToList();

                using (MailMessage mail = new MailMessage(data[0].FromId, to_mail))
                {
                    mail.Subject = subjectt;
                    mail.Body = body;



               

                    // mail.IsBodyHtml = email.IsBodyHtml;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    // smtp.Host = email.host;
                    //smtp.Host = "smtp.gmail.com";
                    smtp.Host = data[0].SMTPServerName;
                    smtp.EnableSsl = data[0].EnableSSL;
                    // smtp.EnableSsl = true;
                    NetworkCredential networkCredential = new NetworkCredential(data[0].FromId, data[0].Password);
                    //smtp.UseDefaultCredentials = email.UseDefaultCredentials;

                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCredential;
                    smtp.Port = data[0].PortNo;
                    // smtp.Port = 587;
                    smtp.Send(mail);
                }

                return "true";
            }
            catch (Exception e)
            {
                return e.ToString();
            }

        }
        public bool sendmail(string to_mail, string subjectt, string body)
        {
            try
            {
                // var data = new Prosol_EmailSettings();
                var data = _EmailRepository.FindAll().ToList();

                using (MailMessage mail = new MailMessage(data[0].FromId, to_mail))
                {
                    mail.Subject = subjectt;
                    mail.Body = body;



                    foreach (string str in data[0].CCC)
                    {
                        if (str != null)
                        {
                            if (Regex.IsMatch(str, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                                mail.CC.Add(new MailAddress(str.ToString()));
                        }
                    }
                    foreach (string str in data[0].BCCC)
                    {
                        if (str != null)
                        {
                            if (Regex.IsMatch(str, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                                mail.Bcc.Add(new MailAddress(str.ToString()));
                        }
                    }


                    // mail.IsBodyHtml = email.IsBodyHtml;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    // smtp.Host = email.host;
                    //smtp.Host = "smtp.gmail.com";
                    smtp.Host = data[0].SMTPServerName;
                    smtp.EnableSsl = data[0].EnableSSL;
                    // smtp.EnableSsl = true;
                    NetworkCredential networkCredential = new NetworkCredential(data[0].FromId, data[0].Password);
                    //smtp.UseDefaultCredentials = email.UseDefaultCredentials;

                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCredential;
                    smtp.Port = data[0].PortNo;
                    // smtp.Port = 587;
                    smtp.Send(mail);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
     
        public Prosol_EmailSettings ShowEmaildata()
        {
            //var sort = SortBy.Descending("UpdatedOn");
            var shwusr = _EmailRepository.FindOne();
            return shwusr;
        }

        public virtual string[] AutoCompleteEmailId(string term)
        {

            var query = Query.Matches("FromId", BsonRegularExpression.Create(new Regex(term, RegexOptions.IgnoreCase)));
            var arrResult = _EmailRepository.AutoSearch(query, "FromId");
            return arrResult;
            //  new BsonRegularExpression(string.Format("^{0}", term));
            // var result = Mongodb.GetCollection("Prosol_Charateristics").Find(query).SetFields("Noun").ToArray();
            //var result = await col.Find(new BsonDocument("Email", model.Email)).ToListAsync();



        }

       
    }
}
