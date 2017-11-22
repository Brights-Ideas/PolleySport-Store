using System.Security.Claims;
using System.Web.Http;

namespace PolleySport.Store.Api.Controllers
{
    [Authorize]
    public abstract class PolleySportApiController : ApiController
    {
        // GET: PolleySportApi
        public string GetUsernameFromClaims()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            return claimsPrincipal?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
        }
    }
}