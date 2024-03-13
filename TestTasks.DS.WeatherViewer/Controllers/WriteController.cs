using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using TestTasks.DS.WeatherViewer.Models.DBEntities;
using TestTasks.DS.WeatherViewer.Pages;
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

        public IActionResult Index()
        {
            return View("~/Pages/Load.cshtml", new LoadModel());
        }

        [HttpPost]
        public async Task<IActionResult> Load(IFormFileCollection uploadedFiles)
        {
            List<WeatherArchiveRecord> recordsToInsert = new();
            try
            {
                if (uploadedFiles is null || uploadedFiles.Count == 0)
                {
                    return View("~/Pages/Load.cshtml", new LoadModel() { ResultMessage = "Файлы не были загружены" });
                }

                foreach (var uploadedFile in uploadedFiles)
                {
                    if (uploadedFile is null)
                    {
                        continue;
                    }

                    var file = await SaveFile(uploadedFile);

                    IWorkbook workbook;
                    using (FileStream fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                    {
                        workbook = new XSSFWorkbook(fileStream);
                    }

                    const int headerRowsAmount = 4;
                    for (int sheetNumber = 0; sheetNumber != workbook.NumberOfSheets; sheetNumber++)
                    {
                        ISheet sheet = workbook.GetSheetAt(sheetNumber);

                        for (int rowNumber = sheet.FirstRowNum + headerRowsAmount; rowNumber != sheet.LastRowNum + 1; rowNumber++)
                        {
                            IRow row = sheet.GetRow(rowNumber);
                            var parsedRow = ParseService.ParseRow(row);
                            recordsToInsert.Add(parsedRow);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // log ex
                return View("~/Pages/Load.cshtml", new LoadModel() { ResultMessage = "Ошибка при загрузке данных" });
            }
            finally
            {
                if (recordsToInsert.Count != 0)
                {
                    await _recordsRepository.InsertRangeAsync(recordsToInsert);
                }
            }

            if (recordsToInsert.Count != 0)
            {
                return View("~/Pages/Load.cshtml", new LoadModel() { ResultMessage = "Данные успешно загружены!" });
            }

            return View("~/Pages/Load.cshtml", new LoadModel() { ResultMessage = "Данные не были загружены" });
        }

        private async Task<FileInfo> SaveFile(IFormFile file)
        {
            string path = SecretsReader.ReadSection<string>("uploadedFilesDirectory").TrimEnd(Path.DirectorySeparatorChar);
            var folderName = Path.Combine(path, $"file_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}");
            var folderInfo = new DirectoryInfo(folderName);

            folderInfo.Create();
            string filePath = Path.Combine(folderInfo.FullName, file.FileName);
            using (var saveFileStream = new FileStream(filePath, FileMode.Create))
            using (var formFileStream = file.OpenReadStream())
                await formFileStream.CopyToAsync(saveFileStream);

            var fileInfo = new FileInfo(filePath);

            return fileInfo;
        }
    }
}
