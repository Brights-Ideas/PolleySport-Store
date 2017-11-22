using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using PolleySport.Data.Interfaces;
using System.Threading.Tasks;
using PolleySport.Data.Models;
using IdentityServer3.Core.Services;

namespace Identity.Web
{
    public class BrightsIdeasUserService : UserServiceBase
    {
        private readonly IUserRepository userRepository;

        public BrightsIdeasUserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public override async Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            var user = await userRepository.GetAsync(context.UserName,
                HashHelper.Sha512(context.Password + context.UserName));

            if (user == null)
            {
                context.AuthenticateResult
                    = new AuthenticateResult("Incorrect credentials");
                return;
            }

            context.AuthenticateResult =
                new AuthenticateResult(context.UserName, context.UserName);
        }

        public async Task<User> GetAsync(string username, string password)
        {
            var user = await userRepository.GetAsync(username,
                HashHelper.Sha512(password + username));

            return user;
        }

        public async Task<User> GetApiUserByName(string username)
        {
            var user = await userRepository.GetAsync(username);

            return user;
            //throw new System.NotImplementedException();
        }

    }
}