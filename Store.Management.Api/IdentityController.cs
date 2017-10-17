﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace Store.Management.Api
{
    [Route("Identity")]
    [Authorize]
    public class IdentityController : ApiController
    {
        // GET api/<controller>
        public IHttpActionResult Get()
        {
            var user = User as ClaimsPrincipal;
            var claims = from c in user.Claims
                select new
                {
                    type = c.Type,
                    value = c.Value
                };

            return Json(claims);
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}