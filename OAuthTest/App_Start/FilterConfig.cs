using OAuthTest.Filter;
using System.Web;
using System.Web.Mvc;

namespace OAuthTest
{
    /// <summary>
    /// MVC 筛选器
    /// </summary>
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new MvcAuthAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
