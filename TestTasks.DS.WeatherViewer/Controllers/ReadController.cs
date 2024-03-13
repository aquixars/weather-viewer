using Microsoft.AspNetCore.Mvc;
using TestTasks.DS.WeatherViewer.Pages;
using TestTasks.DS.WeatherViewer.Repositories;

namespace TestTasks.DS.WeatherViewer.Controllers
{
    [Route("[controller]/[action]")]
    public class ReadController : Controller
    {
        private WeatherArchiveRecordsRepository _recordsRepository;
        private const int pageSize = 7;

        public ReadController(WeatherArchiveRecordsRepository recordsRepository)
        {
            _recordsRepository = recordsRepository;
        }

        [HttpGet("{page}")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var records = await _recordsRepository.GetAllByPageAsync(pageSize, page);
            var count = await _recordsRepository.GetCountAsync();

            var tableModel = new TableViewModel
            {
                Records = records,
                PageInfo = new PageInfo(count, page, pageSize)
            };

            return View("~/Pages/Table.cshtml", tableModel);
        }
    }
}
