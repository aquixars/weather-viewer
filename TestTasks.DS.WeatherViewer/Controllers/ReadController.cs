using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTasks.DS.WeatherViewer.Pages;
using TestTasks.DS.WeatherViewer.Repositories;

namespace TestTasks.DS.WeatherViewer.Controllers
{
    [Route("[controller]/[action]")]
    public class ReadController : Controller
    {
        private WeatherArchiveRecordsRepository _recordsRepository;

        public ReadController(WeatherArchiveRecordsRepository recordsRepository)
        {
            _recordsRepository = recordsRepository;
        }

        public IActionResult Index()
        {
            var tableModel = new TableModel
            {
                records = _recordsRepository.GetAll().Take(100).ToList()
            };
            return View("~/Pages/Table.cshtml", tableModel);
        }
    }
}
