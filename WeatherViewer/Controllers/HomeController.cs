using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WeatherViewer.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Pages/Index.cshtml");
        }
    }
}
