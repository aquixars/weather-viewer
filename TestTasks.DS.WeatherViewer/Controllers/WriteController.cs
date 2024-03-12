using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace TestTasks.DS.WeatherViewer.Controllers
{
    [Route("[controller]/[action]")]
    public class WriteController : Controller
    {
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

                Console.WriteLine(SecretsReader.ReadSection<string>("dbConnectionString"));

                // Получение листа
                ISheet sheet = workbook.GetSheetAt(0);

                // Чтение данных из ячейки
                IRow row = sheet.GetRow(0);
                string cellValue = row.GetCell(0).StringCellValue;

                // Вывод данных ячейки
                Console.WriteLine(cellValue);

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
