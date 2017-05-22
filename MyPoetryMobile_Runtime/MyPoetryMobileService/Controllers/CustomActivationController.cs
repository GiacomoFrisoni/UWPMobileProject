using Microsoft.Azure.Mobile.Server.Config;
using MyPoetryMobileService.DataObjects;
using MyPoetryMobileService.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyPoetryMobileService.Controllers
{
    [MobileAppController]
    public class CustomActivationController : ApiController
    {
        private const string connectionStringName = "Name=MyPoetryMobileContext";
        private const int CODE_LENGTH = 6;

        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
        }

        // POST api/CustomActivation
        public HttpResponseMessage Post(ActivationRequest activationRequest)
        {
            if (string.IsNullOrWhiteSpace(activationRequest.Email) || !activationRequest.Email.Contains("@"))
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "ERR_A_1");
            }
            else if (string.IsNullOrWhiteSpace(activationRequest.Code) || activationRequest.Code.Trim().Length != CODE_LENGTH)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "ERR_A_2");
            }

            // SQL Database version using Entity Framework for data access.
            MyPoetryMobileContext context = new MyPoetryMobileContext(connectionStringName);
            User account = context.User.Where(a => a.Email == activationRequest.Email.Trim().ToLower()).SingleOrDefault();

            if (account != null)
            {
                if (account.ActivationCode.Equals(activationRequest.Code))
                {
                    account.ActivationCode = null;
                    account.IsActivated = true;
                    context.SaveChanges();
                    return this.Request.CreateResponse(HttpStatusCode.OK);
                }
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "ERR_A_3");
            }
            return this.Request.CreateResponse(HttpStatusCode.BadRequest, "ERR_A_4");
        }
    }
}