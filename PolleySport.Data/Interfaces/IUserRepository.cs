using PolleySport.Data.Models;
using System.Threading.Tasks;

namespace PolleySport.Data.Interfaces
{
    public interface IUserRepository
    {
        //Task<IEnumerable<FriendRelation>> GetFriendsForAsync(User user);
        Task<User> GetAsync(string username, string password);
        
        Task<User> GetAsync(string username);
        //ApiIdentityResponse GetUserById(int id);
        //bool ValidatePassword(string username, string plainTextPassword);
        //bool SendOtp(ApiUser user);
        //bool ValidateOtp(ApiUser user, string otp);
        //Task<CustomerResponse> GetCompleteCustomerByUserId(int userId);
    }
}
