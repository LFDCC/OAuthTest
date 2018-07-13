using OAuthTest.Filter;
using OAuthTest.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace OAuthTest.Controllers
{
    [MvcAuth]
    public class OAuth2Controller : Controller
    {
        public ActionResult Authorize()
        {
            if (Request.HttpMethod == "POST")
            {
                var identity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, CurUser.UserInfo.username),
                    new Claim(ClaimTypes.Role, "测试用户")
                }, "Bearer");
                var authentication = HttpContext.GetOwinContext().Authentication;
                authentication.SignIn(identity);
            }
            return View();
        }
    }
}