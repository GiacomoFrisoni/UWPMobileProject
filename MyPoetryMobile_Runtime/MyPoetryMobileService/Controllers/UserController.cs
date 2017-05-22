using Microsoft.Azure.Mobile.Server;
using MyPoetryMobileService.DataObjects;
using MyPoetryMobileService.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;

namespace MyPoetryMobileService.Controllers
{
    public class UserController : TableController<UserDto>
    {
        private const string connectionStringName = "Name=MyPoetryMobileContext";

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MyPoetryMobileContext context = new MyPoetryMobileContext(connectionStringName);
            DomainManager = new SimpleMappedEntityDomainManager<UserDto, User>(
                context, Request, user => user.Id);
        }

        // GET tables/User
        public IQueryable<UserDto> GetAllUsers()
        {
            return Query();
        }

        // GET tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<UserDto> GetUser(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<UserDto> PatchUser(string id, Delta<UserDto> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/User
        public async Task<IHttpActionResult> PostUser(UserDto item)
        {
            UserDto current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteUser(string id)
        {
            return DeleteAsync(id);
        }
    }
}