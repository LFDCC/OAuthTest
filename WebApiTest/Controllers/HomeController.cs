using System.Web.Mvc;
using OAuthTest.Filter;
using OAuthTest.Models;

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
