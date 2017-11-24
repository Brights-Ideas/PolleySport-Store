using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using PolleySport.Data.Interfaces;

namespace SocialNetwork.Api.Controllers
{
    public class StoreController : ApiController
    {
        private readonly IStoreRepository storeRepository;

        public StoreController(IStoreRepository storeRepository)
        {
            this.storeRepository = storeRepository;
        }

        // GET: Store
        [HttpGet]
        public async Task<IHttpActionResult> GetCategorys()
        {
            var categories = (await storeRepository.GetCategorys()).ToArray();

            if (!categories.Any())
            {
                return NotFound();
            }

            return Ok(categories);
        }
    }
}