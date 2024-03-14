using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
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
            if (uploadedFiles is null || uploadedFiles.Count == 0)
            {
                return View("~/Pages/Load.cshtml", new LoadModel() { ResultMessage = "Некорректно переданы файлы" });
            }

            List<WeatherArchiveRecord> recordsToInsert = new();

            foreach (var uploadedFile in uploadedFiles)
            {
                if (uploadedFile is null)
                {
                    continue;
                }

                try
                {
                    var file = await SaveUploadedFile(uploadedFile);

                    if (file is null)
                    {
                        continue;
                    }

                    GetRowsFromFile(file, ref recordsToInsert);
                }
                catch (Exception ex)
                {
                    // log ex
                    // file failed, continuing
                    continue;
                }
            }

            if (recordsToInsert.Count != 0)
            {
                var amount = (await _recordsRepository.InsertRangeAsync(recordsToInsert)).Count();
                return View("~/Pages/Load.cshtml", new LoadModel() { ResultMessage = $"Успешно! Строк было загружено: {amount}" });
            }

            return View("~/Pages/Load.cshtml", new LoadModel() { ResultMessage = "В загруженных файлах не найдены корректные данные" });
        }

        private async Task<FileInfo> SaveUploadedFile(IFormFile file)
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

        private void GetRowsFromFile(FileInfo file, ref List<WeatherArchiveRecord> rowsStorage)
        {
            const int headerRowsAmount = 4;

            IWorkbook workbook = CreateWorkbookFromFile(file);

            for (int sheetNumber = 0; sheetNumber != workbook.NumberOfSheets; sheetNumber++)
            {
                try
                {
                    ISheet sheet = workbook.GetSheetAt(sheetNumber);

                    for (int rowNumber = sheet.FirstRowNum + headerRowsAmount; rowNumber != sheet.LastRowNum + 1; rowNumber++)
                    {
                        try
                        {
                            IRow row = sheet.GetRow(rowNumber);
                            var parsedRow = ParseService.ParseRow(row);
                            rowsStorage.Add(parsedRow);
                        }
                        catch (Exception ex)
                        {
                            // log ex
                            // row failed, continuing
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // log ex
                    // sheet failed, continuing
                    continue;
                }
            }
        }

        private IWorkbook CreateWorkbookFromFile(FileInfo file)
        {
            using (FileStream fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            {
                if (file.Extension.ToLowerInvariant() == ".xls")
                    return new HSSFWorkbook(fileStream); // excel 97-2003
                else
                    return new XSSFWorkbook(fileStream); // other
            }
        }
    }
}
