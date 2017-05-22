using Microsoft.Azure.Mobile.Server.Config;
using MyPoetryMobileService.DataObjects;
using MyPoetryMobileService.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace MyPoetryMobileService.Controllers
{
    [MobileAppController]
    public class CustomPasswordRecoverController : ApiController
    {
        private const string connectionStringName = "Name=MyPoetryMobileContext";

        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
        }

        // POST api/CustomPasswordRecover
        public HttpResponseMessage Post(PwRecoverRequest pwRecoverRequest)
        {
            if (string.IsNullOrWhiteSpace(pwRecoverRequest.Email) || !pwRecoverRequest.Email.Contains("@"))
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "ERR_P_1");
            }

            // SQL Database version using Entity Framework for data access.
            MyPoetryMobileContext context = new MyPoetryMobileContext(connectionStringName);
            User account = context.User.Where(a => a.Email == pwRecoverRequest.Email.Trim().ToLower()).SingleOrDefault();

            if (account != null)
            {
                // Generates and sets a new random password
                string newPassword = Membership.GeneratePassword(12, 1);
                byte[] salt = CustomLoginProviderUtils.GenerateSalt();
                account.Salt = salt;
                account.SaltedAndHashedPassword = CustomLoginProviderUtils.Hash(newPassword, salt);
                context.SaveChanges();

                // Sends welcome email
                EmailHandler.SendEmail(
                    account.Email,
                    account.Name + " " + account.Surname,
                    "Password recovery - MyPoetry",
                    "<h1><span style=\"color: #20B2AA;\"><strong>MyPoetry</strong></span></h1>" +
                    "<h2>Password recovery</h2>" +
                    "<hr/>" +
                    "<p>&nbsp;</p>" +
                    "<p>Hi " + account.Name + " " + account.Surname + ",</p>" +
                    "<p>this is your new password: <strong>" + newPassword + "</strong>.</p>" +
                    "<p>You can now access the application again!</p>" +
                    "<p>&nbsp;</p>" +
                    "<p><em>This is an email generated automatically by MyPoetry. Do not respond to this email address.</em></p>",
                    true);

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            return this.Request.CreateResponse(HttpStatusCode.Unauthorized, "ERR_P_2");
        }
    }
}