using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestTasks.DS.WeatherViewer.Controllers
{
    [Route("[controller]/[action]")]
    public class ReadController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Pages/Table.cshtml");
        }
    }
}
