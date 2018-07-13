using OAuthTest.Filter;
using System.Web;
using System.Web.Mvc;

namespace OAuthTest
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new MvcAuthAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
