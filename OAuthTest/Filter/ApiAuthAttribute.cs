using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace OAuthTest.Filter
{
    /// <summary>
    /// 身份验证
    /// </summary>
    public class ApiAuthAttribute : ActionFilterAttribute
    {
        public string scope { get; set; } = "xj_auth_base";//xj_auth_login
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            bool flag = filterContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() || filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
            if (!flag)
            {
                /*
                var identity = filterContext.RequestContext.Principal.Identity as ClaimsIdentity;
                var scopes = identity.Claims.Where(t => t.Type == "urn:oauth:scope");

                foreach (var scope in scopes)
                {
                    string value = scope.Value;
                }*/
            }
            base.OnActionExecuting(filterContext);
        }
    }
}