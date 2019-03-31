using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace OAuthTest.Controllers
{
    [Authorize]
    public class OAuth2Controller : Controller
    {
        public ActionResult Authorize()
        {
            var scopes = (Request.QueryString.Get("scope") ?? "").Split(',');
            if (Request.HttpMethod == "POST")
            {
                var identity = User.Identity as ClaimsIdentity;
                identity = new ClaimsIdentity(identity.Claims, OAuthDefaults.AuthenticationType, identity.NameClaimType, identity.RoleClaimType);

                foreach (var scope in scopes)
                {
                    identity.AddClaim(new Claim("urn:oauth:scope", scope));
                }

                var props = new AuthenticationProperties(new Dictionary<string, string> {
                    { "user_id", User.Identity.Name }
                });

                var ticket = new AuthenticationTicket(identity, props);

                var authentication = HttpContext.GetOwinContext().Authentication;
                authentication.SignIn(props, identity);
            }
            return View();
        }
    }
}