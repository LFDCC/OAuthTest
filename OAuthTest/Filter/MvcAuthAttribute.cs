using OAuthTest.Models;
using System.Web;
using System.Web.Mvc;

namespace OAuthTest.Filter
{
    /// <summary>
    /// 身份验证
    /// </summary>
    public class MvcAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool flag = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (!flag)
            {
                //如果存在身份信息
                if (CurUser.UserInfo == null)
                {
                    string url = string.Format("{0}?ReturnUrl={1}", "~/Account/Login", HttpUtility.UrlEncode(filterContext.HttpContext.Request.RawUrl));
                    filterContext.Result = new RedirectResult(url);
                }
            }
        }
    }
}