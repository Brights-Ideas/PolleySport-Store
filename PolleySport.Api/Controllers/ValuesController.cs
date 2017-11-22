using PolleySport.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PolleySport.Api.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly IUserRepository _userRepository;
        
        //public ValuesController() { }

        public ValuesController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        // GET api/values
        public async Task<IHttpActionResult> GetAsync(string username)//IEnumerable<string> Get()
        {
            var user = await _userRepository.GetAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
            //return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
