using Microsoft.Azure.Mobile.Server.Config;
using MyPoetryMobileService.DataObjects;
using MyPoetryMobileService.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyPoetryMobileService.Controllers
{
    [MobileAppController]
    public class CustomReSendingActivationController : ApiController
    {
        private const string connectionStringName = "Name=MyPoetryMobileContext";

        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
        }

        public HttpResponseMessage Post(ReSendingActivationRequest reSendingActivationRequest)
        {
            if (string.IsNullOrWhiteSpace(reSendingActivationRequest.Email) || !reSendingActivationRequest.Email.Contains("@"))
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "ERR_RA_1");
            }

            // SQL Database version using Entity Framework for data access.
            MyPoetryMobileContext context = new MyPoetryMobileContext(connectionStringName);
            User account = context.User.Where(a => a.Email == reSendingActivationRequest.Email.Trim().ToLower()).SingleOrDefault();

            if (account != null)
            {
                // Generates random 6 digit number
                Random generator = new Random();
                String activationCode = generator.Next(0, 1000000).ToString("D6");

                // Resets activation code
                account.ActivationCode = activationCode;
                context.SaveChanges();

                // Sends a new email
                EmailHandler.SendEmail(
                    account.Email,
                    account.Name + " " + account.Surname,
                    "Account activation - MyPoetry",
                    "<h1><strong>Account activation&nbsp;<span style=\"color:#20b2aa;\">MyPoetry</span>!</strong></h1>" +
                    "<hr/>" +
                    "<p>Hi, " + account.Name + " " + account.Surname + "!</p>" +
                    "<p>This is your activation&nbsp;code: <strong>" + activationCode + "</strong>.Enter it in&nbsp;the application to start using MyPoetry immediately.</p>" +
                    "<p>Start now to...</p>" +
                    "<ul>" +
                    "<li>Write poetry</li>" +
                    "<li>Use specific writing tools</li>" +
                    "<li>Share your texts</li>" +
                    "<li>Track your progress</li>" +
                    "<li>...And so much more</li>" +
                    "</ul>" +
                    "<p><strong>What are you waiting for?</strong></p>" +
                    "<p>&nbsp;</p>" +
                    "<p>&nbsp;</p>" +
                    "<p><em>This is an email generated automatically by MyPoetry. Do not respond to this email address.</em></p>",
                    true);

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            return this.Request.CreateResponse(HttpStatusCode.BadRequest, "ERR_RA_2");
        }
    }
}