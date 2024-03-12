using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TestTasks.DS.WeatherViewer.Pages
{
    public class LoadModel : PageModel
    {
        private readonly ILogger<LoadModel> _logger;

        public LoadModel(ILogger<LoadModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
