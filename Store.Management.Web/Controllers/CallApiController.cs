using IdentityModel.Client;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace Store.Management.Web.Controllers
{
    public class CallApiController : Controller
    {
        // GET: CallApi/ClientCredentials
        public async Task<ActionResult> ClientCredentials()
        {
            var response = await GetTokenAsync();
            var result = await CallApi(response.AccessToken);

            ViewBag.Json = result;
            return View("ShowApiResult");
        }

        // GET: CallApi/UserCredentials
        public async Task<ActionResult> UserCredentials()
        {
            var user = User as ClaimsPrincipal;
            var token = user.FindFirst("access_token").Value;
            var result = await CallApi(token);

            ViewBag.Json = result;
            return View("ShowApiResult");
        }

        private async Task<string> CallApi(string token)
        {
            var client = new HttpClient();
            client.SetBearerToken(token);

            var json = await client.GetStringAsync("https://localhost:44384/identity");
            return JArray.Parse(json).ToString();
        }

        private async Task<TokenResponse> GetTokenAsync()
        {
            var client = new TokenClient(
                "https://localhost:44383/identity/connect/token",
                "mvc_service",
                "secret");

            return await client.RequestClientCredentialsAsync("sampleApi");
        }

    }
}
