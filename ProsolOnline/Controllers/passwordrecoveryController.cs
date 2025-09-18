using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using Prosol.Core.Interface;
using Prosol.Core.Model;
using Prosol.Core;

namespace ProsolOnline.Controllers
{
    public class passwordrecoveryController : Controller
    {

        private readonly I_pwRecovery _pwrecovery;
        private readonly IEmailSettings _Emailservc;


        public passwordrecoveryController(I_pwRecovery pwrecovery, IEmailSettings Emailservc)
        {
            _pwrecovery = pwrecovery;
            _Emailservc = Emailservc;
        }
      
        // GET: passwordrecovery
        public ActionResult Index()
        {
            return View("sendemail");
        }
        public ActionResult updatepasswordpage()
        {
            return View("updatepassword");
        }
        public JsonResult sendemail_forPR(string email)
        {
            var Data = _pwrecovery.sendemail_forPR(email).ToList();
            if (Data.Count > 0)
            {
                string to_mail = Data[0].EmailId;
                string userid = Data[0].Userid;
                string username = Data[0].UserName;
                Random rnd = new Random();
                int rndm = rnd.Next(30000);

                bool value = _pwrecovery.saveRandom(userid, rndm);

                if (value != true)
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);


                // email email1 = new email();

                // email1.email_to = to_mail;  
                // email1.email_from = "codasol.madras@gmail.com";    
                string subject = "Password Recovery From PROSOL MDG Application";
                //string body = "Use the below link to reset your prosol password"+ "<html><body> <a href=http://"+ HttpContext.Request.Url.Host+"/passwordrecovery/updatepasswordpage#?num$123=" + userid + "&num$coda=" + rndm+" > Changepassword </a></body></html>";

                string body = @"<div style='background-color:#f5f5f5;'> 
  <table align='center' border='0' cellpadding='0' cellspacing='0' width='700'> 
   <tbody> 
    <tr> 
     <td align='center' style='padding-top:10px;padding-left:40px;padding-right:40px;padding-top:20px;font-family:Arial;font-size:13px;line-height:18px;padding-bottom:20px;font-weight:normal' valign='top'> 
      <table align='center' border='0' cellpadding='0' cellspacing='0' style='background: #fff;    border-collapse: collapse;' width='100%'> 
       <tbody> 
        <tr style='padding-right:0px;padding-bottom:10px;border-bottom:1px dashed #ccc;'> 
         <td style='width: 80%;float: left;'><img alt='PROSOL' src='https://" + HttpContext.Request.Url.Host + @"/Images/logo.png' style='width:250px;' ></td> 
         <td style='width: 20%;float: left;text-align:center'><img alt='PROSOL' src='https://" + HttpContext.Request.Url.Host + @"/Images/sheld.png' style='width:50px;height:50px;margin-top: 10px;' ></td>     
	   </tr> 
        <tr> 
         <td style='padding:3px 0px;padding-top:0px;font-family:Arial;font-size:13px;text-align:justify;line-height:18px'> 
          <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%'> 
           <tbody> 
            <tr> 
             <td style='padding:3px 0px;font-family:Arial;font-size:13px;text-align:justify;line-height:18px;padding-left:75px;padding-right:75px;padding-bottom: 18px;'>&nbsp; 
			 <p><span style='font-family:Arial'><span style='font-size:11px'>Dear " + username + @",</span> </span> </p>
			 <p><span style='font-family:Verdana,Geneva,sans-serif'></span></p>
			 <p style='font-size:11px'>To reset your password for PROSOL MDG application,click the below link</p
			 </td> 
			 
            </tr> 
           <tr><td style='font-family:Arial;font-size:13px;padding-left:75px;padding-right:75px'><p style='margin-top:0;margin-bottom:9px;width:22%;float:left;font-weight:300'>Click Here :&nbsp;</p><p style='margin-top:0;margin-bottom:9px;width:50%;float:left;font-weight:bold'><a href=http://localhost:15505//passwordrecovery/updatepasswordpage#?num$123=" + @" > Changepassword
             </a><p></td>
           </tr>
           
            <tr> 
             <td style='font-family:Arial;font-size:15px;text-align:center;line-height:18px;padding-top:10px;border-top:1px dashed #a32e5e!important'><span><strong>Our Services&nbsp;</strong> </span> </td> 
            </tr>
			<tr> 
             <td style='font-family:Arial;font-size:13px;text-align:justify;line-height:18px;padding-top:10px;padding-bottom:2px'> 
              <table align='center' cellpadding='0' cellspacing='0' style='border-bottom:1px dashed #a32e5e!important;padding-left:15px;padding-right:15px' width='100%'> 
               <tbody> 
                <tr> 
                 <td align='center' style='padding:3px;font-family:Arial;font-size:9px;line-height:18px;border-right:1px dashed black'><img alt='PROSOL' src='https://" + HttpContext.Request.Url.Host + @"/Images/material-management.png' width='40'></td> 
                 <td align='center' style='padding:3px;font-family:Arial;font-size:9px;line-height:18px;border-right:1px dashed black'><img alt='PROSOL' src='https://" + HttpContext.Request.Url.Host + @"/Images/service-master.png' style='display:block' width='40'></td> 
                 <td align='center' style='padding:3px;font-family:Arial;font-size:9px;line-height:18px;border-right:1px dashed black'><img alt='PROSOL' src='https://" + HttpContext.Request.Url.Host + @"/Images/bom.png'  width='40'></td> 
                 <td align='center' style='padding:3px;font-family:Arial;font-size:9px;line-height:18px;border-right:1px dashed black'><img alt='PROSOL' src='https://" + HttpContext.Request.Url.Host + @"/Images/asset-master-icon.png'  width='40'></td> 
                 <td align='center' style='padding:3px;font-family:Arial;font-size:9px;line-height:18px;'><img alt='PROSOL' src='https://" + HttpContext.Request.Url.Host + @"/Images/vendor-master.png' width='40'></td> 
                 
                </tr> 
                <tr> 
                 <td align='center' style='padding:3px;font-family:Arial;font-size:9px;line-height:18px;border-right:1px dashed black'><strong><a href=https://" + HttpContext.Request.Url.Host + @" style='color:black;text-decoration: none;' target='_blank'>MATERIAL MASTER</a> </strong> </td> 
                 <td align='center' style='padding:3px;font-family:Arial;font-size:9px;line-height:18px;border-right:1px dashed black'><strong><a href=https://" + HttpContext.Request.Url.Host + @" style='color:black;text-decoration: none;' target='_blank'>SERVICE MASTER</a> </strong> </td> 
				 <td align='center' style='padding:3px;font-family:Arial;font-size:9px;line-height:18px;border-right:1px dashed black'><strong><a href=https://" + HttpContext.Request.Url.Host + @" style='color:black;text-decoration: none;' target='_blank'>BOM</a> </strong> </td> 
				 <td align='center' style='padding:3px;font-family:Arial;font-size:9px;line-height:18px;border-right:1px dashed black'><strong><a href=https://" + HttpContext.Request.Url.Host + @" style='color:black;text-decoration: none;' target='_blank'>ASSET MASTER</a> </strong> </td> 
				 <td align='center' style='padding:3px;font-family:Arial;font-size:9px;line-height:18px;'><strong><a href=https://" + HttpContext.Request.Url.Host + @" style='color:black;text-decoration: none;' target='_blank'>BUSINESS PARTNER</a> </strong> </td> 
				 </tr> 
               </tbody> 
              </table></td> 
            </tr>
			           <tr> 
             <td style='padding:3px 0px;font-family:Arial;font-size:13px;text-align:justify;line-height:18px;padding-top:5px;padding-left:75px;padding-right:75px'>&nbsp; <p style='text-align:center'><span style='font-size:9px'>Please do not reply to this mail as this is an automated mail service.</span> </p>
</td> 

            </tr>
		   </tbody> 
          </table> 
</td> 
        </tr> 
       </tbody> 
      </table></td> 
    </tr> 
   </tbody> 
  </table>
  </div>";

                //   string body = "Use the below link to reset your prosol password" + "<html><body> <a href=http://localhost:15505//passwordrecovery/updatepasswordpage#?num$123=" + userid + "&num$coda=" + rndm + " > Changepassword </a></body></html>";
                //  email1.IsBodyHtml = true;    
                //  email1.host = "smtp.gmail.com";     
                // email1.enablessl = true;      
                //email1.UseDefaultCredentials = true;     
                // email1.Port = 587;    
                // email1.password = "codasolwestmambalam";

                // EmailSettingService ems = new EmailSettingService();
                bool val = _Emailservc.sendmail(to_mail, subject, body);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult updatepassword(string pswd, string userid,int rndm)
        {
            bool value = _pwrecovery.updatepassword(pswd,userid,rndm);

            if (value == true)
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }



        }
}