using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using MyPoetryMobileService.Models;
using System.Net.Http;
using MyPoetryMobileService.DataObjects;
using System.Net;
using System.Linq;

namespace MyPoetryMobileService.Controllers
{
    [MobileAppController]
    public class CustomLoginController : ApiController
    {
        private const string connectionStringName = "Name=MyPoetryMobileContext";

        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
        }

        // POST api/CustomLogin
        public HttpResponseMessage Post(LoginRequest loginRequest)
        {
            MyPoetryMobileContext context = new MyPoetryMobileContext(connectionStringName);
            User account = context.User
                .Where(a => a.Email == loginRequest.Email.Trim().ToLower()).SingleOrDefault();

            if (account != null)
            {
                byte[] incoming = CustomLoginProviderUtils
                    .Hash(loginRequest.Password, account.Salt);

                if (CustomLoginProviderUtils.SlowEquals(incoming, account.SaltedAndHashedPassword))
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            return this.Request.CreateResponse(HttpStatusCode.Unauthorized, "ERR_L_1");
        }
    }
}
