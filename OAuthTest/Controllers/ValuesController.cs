using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using OAuthTest.Models;

namespace OAuthTest.Controllers
{
    public class ValuesController : ApiController
    {
        [HttpPost]
        [Route("api/login")]
        public JsonResult<JObject> Login(JObject user)//User user
        {
            //dynamic data = user;
            //int id = data.username;
            //string username = data.username;
            return Json(user);
        }
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
