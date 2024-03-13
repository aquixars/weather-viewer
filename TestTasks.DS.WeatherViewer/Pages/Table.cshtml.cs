using Microsoft.AspNetCore.Mvc.RazorPages;
using TestTasks.DS.WeatherViewer.Models.DBEntities;

namespace TestTasks.DS.WeatherViewer.Pages
{
    public class TableModel : PageModel
    {
        public List<WeatherArchiveRecord> records { get; set; }
    }
}
