using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace PolleySport.Store.Api.Controllers
{
    //[Route("Identity")]
    //[Authorize]
    public class ValuesController : ApiController
    {
        // GET api/<controller>
        public IHttpActionResult Get()
        {
            var user = User as ClaimsPrincipal;
            var claims = from c in user.Claims
                select new
                {
                    type = c.Type,
                    value = c.Value
                };

            return Json(claims);
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
