using System.Collections.Generic;
using System.Threading.Tasks;
using PolleySport.Data.Models;

namespace PolleySport.Data.Interfaces
{
    public interface IStoreRepository
    {
        Task<IEnumerable<Category>> GetCategorys();
    }
}