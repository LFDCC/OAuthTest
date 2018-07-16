using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace OAuthTest.Filter
{
    /// <summary>
    /// 身份验证
    /// </summary>
    public class ApiAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext filterContext)
        {

            bool flag = filterContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() || filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
            if (!flag && filterContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                var r = Roles;
                var u = Users;
                //filterContext.RequestContext.Principal.IsInRole("auth_login")
                var identity = filterContext.RequestContext.Principal.Identity as ClaimsIdentity;

                var scopes = identity.Claims.Where(t => t.Type == "urn:oauth:scope").Select(t => t.Value).ToList();
                if (scopes.Contains(Roles))
                {

                }
                else
                {

                    //填充response 阻止controller或者action继续执行
                    var response = filterContext.Response = filterContext.Response ?? new HttpResponseMessage();
                    response.StatusCode = HttpStatusCode.Unauthorized;
                    var content = new
                    {
                        success = false,
                        error = "您没有权限访问"
                    };
                    response.Content = new StringContent(Json.Encode(content), Encoding.UTF8, "application/json");
                }
            }
            base.OnAuthorization(filterContext);//可以执行后续的 IsAuthorized HandleUnauthorizedRequest两个方法
        }

        protected override bool IsAuthorized(HttpActionContext filterContext)
        {
            return base.IsAuthorized(filterContext);
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            var response = filterContext.Response = filterContext.Response ?? new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Forbidden;
            var content = new
            {
                success = false,
                error = "服务器拒绝您的访问"
            };
            response.Content = new StringContent(Json.Encode(content), Encoding.UTF8, "application/json");

            base.HandleUnauthorizedRequest(filterContext);
        }

    }
}