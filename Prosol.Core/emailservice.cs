using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prosol.Core.Model;
using System.Net.Mail;
using System.Net;
using Prosol.Core.Interface;

namespace Prosol.Core
{

    public partial class emailservice 
    {
        //   private readonly IRepository<Prosol_EmailSettings> _EmailRepository;
        //public emailservice(IRepository<Prosol_EmailSettings> EmailRepository)
        //{
        //    this._EmailRepository = EmailRepository;
        //}
        private readonly IRepository<Prosol_EmailSettings> _EmailRepository;
        public emailservice(IRepository<Prosol_EmailSettings> EmailRepository)
        {
            this._EmailRepository = EmailRepository;
        }
        public bool sendmail(email email)
        {
            try
            {
               // var data = new Prosol_EmailSettings();
               var data = _EmailRepository.FindAll().ToList();

                using (MailMessage mail = new MailMessage(data[0].FromId, email.email_to))
            {
                    mail.Subject = email.subject;
                    mail.Body = email.body;
                   
                    // mail.IsBodyHtml = email.IsBodyHtml;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    // smtp.Host = email.host;
                    //smtp.Host = "smtp.gmail.com";
                    smtp.Host = data[0].SMTPServerName;
                    smtp.EnableSsl = data[0].EnableSSL;
                   // smtp.EnableSsl = true;
                     NetworkCredential networkCredential = new NetworkCredential(data[0].FromId, "codasolwestmambalam");
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
    }
}
