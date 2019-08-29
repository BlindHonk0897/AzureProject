using AzureProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace AzureProject.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult SendEmail()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SendEmailTo()
        {

            try
            {
                //Configuring webMail class to send emails  
                //gmail smtp server  
                //  WebMail.SmtpServer = "smtp.gmail.com";
                //gmail port to send emails  
                // WebMail.SmtpPort = 587;
                // WebMail.SmtpUseDefaultCredentials = true;
                //sending emails with secure protocol  
                // WebMail.EnableSsl = true;
                //EmailId used to send emails from application  
                //  WebMail.UserName = "alingasadan@gmail.com";
                // WebMail.Password = "diehardwarrior30";

                //Sender email address.  
                // WebMail.From = "alingasadan@gmail.com";

                //Send email  
                //  WebMail.Send(to: obj.ToEmail, subject: obj.EmailSubject, body: obj.EMailBody, cc: obj.EmailCC, bcc: obj.EmailBCC, isBodyHtml: true);

                var sender = "alingasadan@gmail.com";
                var password = "diehardwarrior30";

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential(sender, password, "domain.com");

                MailMessage mail = new MailMessage(sender, "blindhonk123@gmail.com", "WebMail", "<p> <h1>Hi</h1><br />This is a sample message!</p>");
                mail.IsBodyHtml = true;
                mail.BodyEncoding = UTF8Encoding.UTF8;

                client.Send(mail);


                ViewBag.Status = "Email Sent Successfully.";
                return Content("Email Sent Successfully.");
            }
            catch (Exception ex)
            {
                ViewBag.Status = "Problem while sending email, Please check details.";
                return Content(ex.Message);
            }
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendEmail(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("dalingasa@student.passerellesnumeriques.org")); //replace with valid value
                message.Subject = "Your email subject";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient("domain-com.mail.protection.outlook.com"))
                {
                    await smtp.SendMailAsync(message);
                    return Content("Sent");
                }
            }
            return View(model);
        }

        public ActionResult SendGmail()
        {
            try
            {
                GMailer.GmailUsername = "alingasadan@gmail.com";
                GMailer.GmailPassword = "diehardwarrior30";

                GMailer mailer = new GMailer();
                mailer.ToEmail = "blindhonk123@gmail.com";
                mailer.Subject = "Experiment";
                mailer.Body = "Thanks for Registering your account.<br> please verify your email id by clicking the link <br> <a href='youraccount.com/verifycode=12323232'>verify</a>";
                mailer.IsHtml = true;
                mailer.Send();
                return Content("SENT man guru");
            }
            catch(Exception ex)
            {
                return Content(ex.InnerException.ToString());
            }
           
          
        }
    }
}