using Microsoft.AspNetCore.Mvc;
using TestTasks.DS.WeatherViewer.Pages;
using TestTasks.DS.WeatherViewer.Repositories;

namespace TestTasks.DS.WeatherViewer.Controllers
{
    [Route("[controller]/[action]")]
    public class ReadController : Controller
    {
        private WeatherArchiveRecordsRepository _recordsRepository;
        private const int pageSize = 10;

        public ReadController(WeatherArchiveRecordsRepository recordsRepository)
        {
            _recordsRepository = recordsRepository;
        }

        [HttpGet("{page}")]
        public IActionResult Index(int page = 1)
        {
            // todo : make it async
            var records = _recordsRepository.GetAll();
            var tableModel = new TableViewModel
            {
                Records = records.Skip((page - 1) * pageSize).Take(pageSize),
                PageInfo = new PageInfo(records.Count, page, pageSize)
            };
            return View("~/Pages/Table.cshtml", tableModel);
        }
    }
}
