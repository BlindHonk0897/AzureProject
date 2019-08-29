using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.TwiML;
using Twilio.AspNet.Mvc;

namespace AzureProject.Controllers
{
    public class SMSController : TwilioController
    {
        // GET: SMS
        public ActionResult SendSMS()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Send()
        {
            var mes = Request.Form["message"];
            string num = Request.Form["phone"];

            var accountSid = "putTwilioAccountSid";
            var authToken = "putTwilioAuthToken";
            TwilioClient.Init(accountSid, authToken);

            var to = new PhoneNumber(num);
            var from = new PhoneNumber("putYourTwilioNumber");

            var message = MessageResource.Create(to: to, from: from,
                body: mes);
            return Content(message.AccountSid);
        }
    }
}