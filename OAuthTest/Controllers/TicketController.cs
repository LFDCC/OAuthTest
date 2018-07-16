using OAuthTest.Filter;
using OAuthTest.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace OAuthTest.Controllers
{

    public class TicketController : ApiController
    {
        [Route("api/ticket/user")]
        [HttpGet]
        [ApiAuth(Roles = "auth_login")]
        public User UserInfo()
        {
            var result = Repository.users.Where(u => u.username == User.Identity.Name).FirstOrDefault();
            return result;
        }
    }
}