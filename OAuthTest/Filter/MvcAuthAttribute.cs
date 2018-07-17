using System.Web;
using System.Web.Mvc;

namespace OAuthTest.Filter
{
    /// <summary>
    /// 身份验证
    /// </summary>
    public class MvcAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool flag = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (!flag)
            {                
                //如果存在身份信息
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    string url = string.Format("{0}?ReturnUrl={1}", "~/Account/Login", HttpUtility.UrlEncode(filterContext.HttpContext.Request.RawUrl));
                    filterContext.Result = new RedirectResult(url);
                }
            }
            base.OnAuthorization(filterContext);
        }
    }
}