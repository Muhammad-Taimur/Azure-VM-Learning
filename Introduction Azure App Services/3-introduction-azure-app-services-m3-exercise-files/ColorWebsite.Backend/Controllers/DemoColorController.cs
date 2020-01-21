using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using ColorWebsite.Data.Entities;
using ColorWebsite.Data;

namespace colorwebsitemobileService.Controllers
{
    public class DemoColorController : TableController<DemoColor>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            DataContext context = new DataContext();
            DomainManager = new EntityDomainManager<DemoColor>(context, Request);
        }

        // GET tables/DemoColor
        public IQueryable<DemoColor> GetAllDemoColors()
        {
            return Query();
        }

        // GET tables/DemoColor/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<DemoColor> GetDemoColor(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/DemoColor/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<DemoColor> PatchDemoColor(string id, Delta<DemoColor> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/DemoColor
        public async Task<IHttpActionResult> PostDemoColor(DemoColor item)
        {
            DemoColor current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/DemoColor/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteDemoColor(string id)
        {
            return DeleteAsync(id);
        }
    }
}