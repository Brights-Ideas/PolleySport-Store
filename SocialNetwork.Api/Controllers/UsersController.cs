using System.Threading.Tasks;
using System.Web.Http;
using PolleySport.Data.Interfaces;

namespace SocialNetwork.Api.Controllers
{
    public class UsersController : ApiController //SocialNetworkApiController
    {
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(string username)
        {
            var user = await userRepository.GetAsync(username);//GetUsernameFromClaims());

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        //[HttpGet]
        //[Route("api/users/friends")]
        //public async Task<IHttpActionResult> FriendsAsync()
        //{
        //    var user = await userRepository.GetAsync(GetUsernameFromClaims());

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var friends = (await userRepository.GetFriendsForAsync(user)).ToArray();

        //    if (!friends.Any())
        //    {
        //        return NotFound();
        //    }
            
        //    return Ok(friends);
        //}

    }
}