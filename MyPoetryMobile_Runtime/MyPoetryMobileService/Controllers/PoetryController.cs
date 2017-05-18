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
    public class PoetryController : TableController<PoetryDto>
    {
        private const string connectionStringName = "Name=MyPoetryMobileContext";

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MyPoetryMobileContext context = new MyPoetryMobileContext(connectionStringName);
            DomainManager = new SimpleMappedEntityDomainManager<PoetryDto, Poetry>(
                context, Request, poetry => poetry.Id);
        }

        // GET tables/Poetry
        public IQueryable<PoetryDto> GetAllPoetries()
        {
            return Query();
        }

        // GET tables/Poetry/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<PoetryDto> GetPoetry(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Poetry/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<PoetryDto> PatchPoetry(string id, Delta<PoetryDto> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Poetry
        public async Task<IHttpActionResult> PostPoetry(PoetryDto item)
        {
            PoetryDto current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Poetry/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePoetry(string id)
        {
            return DeleteAsync(id);
        }
    }
}