using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WaterPreview.Controllers
{
    [RoutePrefix("api/cg")]
    public class CredentialsGrantController : ApiController
    {
        // GET api/<controller>
        [HttpGet, Route("cgs/get")]
        public IEnumerable<string> Get()
        {
            return new string[] { "hello", "world" };
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

        public void Login(string name, string password)
        {

        }
    }
}