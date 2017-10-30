using Dapper;
using PolleySport.Data.Interfaces;
using PolleySport.Data.Models;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PolleySport.Data.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(Func<IDbConnection> openConnection) : base(openConnection) { }

        public async Task<User> GetAsync(string username, string password)
        {
            using (var connection = OpenConnection())
            {
                var queryResult = await connection.QueryAsync<User>("select * from [Users] where [Username]=@username and [Password]=@password",
                    new { username, password });

                return queryResult.SingleOrDefault();
            }
        }

        public async Task<User> GetAsync(string username)
        {
            using (var connection = OpenConnection())
            {
                var queryResult = await connection.QueryAsync<User>("select * from [Users] where [Username]=@username",
                    new { username });

                return queryResult.SingleOrDefault();
            }
        }
    }
}
