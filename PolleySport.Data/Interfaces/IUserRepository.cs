using PolleySport.Data.Models;
using System.Threading.Tasks;

namespace PolleySport.Data.Interfaces
{
    public interface IUserRepository
    {
        //Task<IEnumerable<FriendRelation>> GetFriendsForAsync(User user);
        Task<User> GetAsync(string username, string password);
        Task<User> GetAsync(string username);
    }
}
