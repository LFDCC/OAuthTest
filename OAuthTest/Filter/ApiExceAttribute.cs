using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Filters;

namespace OAuthTest.Filter
{
    /// <summary>
    /// WebApi的异常筛选器
    /// </summary>
    public class ApiExceAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext filterContext)
        {
            string controllerName = filterContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionContext.ActionDescriptor.ActionName;
            string error = "controllerName:" + controllerName + " actionName:" + actionName + " Error:" + filterContext.Exception.Message;

            var response = filterContext.Response = filterContext.Response ?? new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.InternalServerError;
            var content = new
            {
                success = false,
                error = "系统异常"
            };
            response.Content = new StringContent(Json.Encode(content), Encoding.UTF8, "application/json");

            base.OnException(filterContext);
        }
    }
}