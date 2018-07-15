using Microsoft.AspNet.Identity;
using OAuthTest.Filter;
using OAuthTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace OAuthTest.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (CurUser.UserInfo != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(User user, string returnUrl = "/Home/Index")
        {
            var model = Repository.users.FirstOrDefault(t => t.username == user.username && t.password == user.password);
            if (model != null)
            {
                Session["UserInfo"] = model;
                
                /*
                ///id验证
                var identity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, model.id.ToString()),
                    new Claim(ClaimTypes.Name, model.username),
                }, DefaultAuthenticationTypes.ApplicationCookie);
                var authentication = HttpContext.GetOwinContext().Authentication;
                authentication.SignIn(identity);
                */

                return RedirectToLocal(returnUrl);
            }
            else
            {
                ViewBag.error = "用户名或密码错误";
                return View();
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}