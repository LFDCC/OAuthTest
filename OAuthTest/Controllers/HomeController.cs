using OAuthTest.Filter;
using System.Web.Mvc;

namespace OAuthTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
