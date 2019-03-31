using System.Linq;
using System.Web.Http;
using OAuthTest.Filter;
using OAuthTest.Models;

namespace OAuthTest.Controllers
{

    public class TicketController : ApiController
    {
        /// <summary>
        /// 获取票据信息
        /// </summary>
        /// <returns>获取票据信息r</returns>
        [Route("api/ticket/user")]
        [HttpGet]
        [ApiAuth]
        public User UserInfo()
        {
            var result = Repository.users.Where(u => u.username == User.Identity.Name).FirstOrDefault();
            return result;
        }
    }
}