using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using PolleySport.Data.Interfaces;
using PolleySport.Data.Models;

namespace PolleySport.Data.Repositories
{
    public class StoreRepository : Repository, IStoreRepository
    {
        public StoreRepository(Func<IDbConnection> openConnection) : base(openConnection) { }


        public async Task<IEnumerable<Category>> GetCategorys()
        {
            using (var connection = OpenConnection())
            {
                var queryResult = await connection.QueryAsync<Category>("select * from [Category]");

                return queryResult;
            }
        }
    }
}
