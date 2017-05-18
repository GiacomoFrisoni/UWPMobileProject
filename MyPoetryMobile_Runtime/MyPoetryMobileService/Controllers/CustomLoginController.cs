using MyPoetryMobileService.DataObjects;
using MyPoetryMobileService.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyPoetryMobileService.Controllers
{
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
                .Where(a => a.Email == loginRequest.Email).SingleOrDefault();

            if (account != null)
            {
                byte[] incoming = CustomLoginProviderUtils
                    .Hash(loginRequest.Password, account.Salt);

                if (CustomLoginProviderUtils.SlowEquals(incoming, account.SaltedAndHashedPassword))
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            return this.Request.CreateResponse(HttpStatusCode.Unauthorized,
                "Invalid username or password");
        }
    }
}