using System;
using System.Data;

namespace PolleySport.Data.Repositories
{
    public abstract class Repository
    {
        protected readonly Func<IDbConnection> OpenConnection;

        protected Repository(Func<IDbConnection> openConnection)
        {
            OpenConnection = openConnection;
        }
    }
}
