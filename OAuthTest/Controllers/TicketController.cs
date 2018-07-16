using OAuthTest.Filter;
using OAuthTest.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace OAuthTest.Controllers
{
    [ApiAuth]
    public class TicketController : ApiController
    {
        [Route("api/ticket/user")]
        [HttpGet]
        public User UserInfo()
        {
            var result = Repository.users.Where(u => u.username == User.Identity.Name).FirstOrDefault();
            return result;
        }
        [Route("api/ticket/client")]
        [HttpGet]
        public Client ClientInfo()
        {
            var result = Repository.clients.Where(u => u.clientId == User.Identity.Name).FirstOrDefault();
            return result;
        }
    }
}