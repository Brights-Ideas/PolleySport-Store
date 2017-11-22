using PolleySport.Data.Interfaces;
using PolleySport.Data.Repositories;
using System.Threading.Tasks;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;

namespace PolleySport.Store.Api.Controllers
{
    //[Route("Identity")]
    //[Authorize]
    [ScopeAuthorize("read")]
    public class ValuesController : PolleySportApiController
    {
        private readonly IUserRepository _userRepository;
        //private readonly BrightsIdeasUserService _brightsIdeasUserService;
        public ValuesController(){}

        public ValuesController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
            //_brightsIdeasUserService = brightsIdeasUserService;
        }

        // GET api/<controller>
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(string username)
        {
            //var user = User as ClaimsPrincipal;
            //var username = user?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            //var claims = from c in user.Claims
            //             select new
            //             {
            //                 type = c.Type,
            //                 value = c.Value
            //             };
            var user = await _userRepository.GetAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
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
