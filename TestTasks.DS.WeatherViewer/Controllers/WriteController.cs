using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using TestTasks.DS.WeatherViewer.Repositories;
using TestTasks.DS.WeatherViewer.Services;

namespace TestTasks.DS.WeatherViewer.Controllers
{
    [Route("[controller]/[action]")]
    public class WriteController : Controller
    {
        private WeatherArchiveRecordsRepository _recordsRepository;

        public WriteController(WeatherArchiveRecordsRepository recordsRepository)
        {
            _recordsRepository = recordsRepository;
        }

        public IActionResult Load(/* file */)
        {
            try
            {
                var file = new FileInfo("Data/moskva_2010.xlsx");

                // Открытие существующей рабочей книги
                IWorkbook workbook;
                using (FileStream fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                {
                    workbook = new XSSFWorkbook(fileStream);
                }

                const int headerRowsAmount = 4;
                // todo: optimize
                for (int j = 0; j != workbook.NumberOfSheets; j++)
                {
                    // Получение листа
                    ISheet sheet = workbook.GetSheetAt(j);

                    for (int k = sheet.FirstRowNum + headerRowsAmount; k != sheet.LastRowNum + 1; k++)
                    {
                        // Чтение данных из ячейки
                        IRow row = sheet.GetRow(k);
                        var parsedRow = ParseService.ParseRow(row);
                        _recordsRepository.Insert(parsedRow);
                        // Вывод данных ячейки
                        Console.WriteLine($"Вставили строку с данными от {parsedRow.Created}");
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                // log ex
                return BadRequest();
            }
        }
    }
}
