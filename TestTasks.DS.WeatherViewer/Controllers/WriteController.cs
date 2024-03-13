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

        public IActionResult Index()
        {
            return View("~/Pages/Load.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Load(IFormFileCollection uploadedFiles)
        {
            try
            {
                if (uploadedFiles is null || uploadedFiles.Count == 0)
                {
                    return View("~/Pages/Load.cshtml");
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
                    // todo: optimize
                    for (int j = 0; j != workbook.NumberOfSheets; j++)
                    {
                        ISheet sheet = workbook.GetSheetAt(j);

                        for (int k = sheet.FirstRowNum + headerRowsAmount; k != sheet.LastRowNum + 1; k++)
                        {
                            IRow row = sheet.GetRow(k);
                            var parsedRow = ParseService.ParseRow(row);
                            _recordsRepository.Insert(parsedRow);
                        }
                    }
                }

                return View("~/Pages/Load.cshtml");
            }
            catch (Exception ex)
            {
                // log ex
                return BadRequest();
            }
        }
        private async Task<FileInfo> SaveFile(IFormFile file)
        {
            if (file == null)
                throw new InvalidOperationException("Отсутствует файл");

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
