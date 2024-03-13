using TestTasks.DS.WeatherViewer.Models.DBEntities;

namespace TestTasks.DS.WeatherViewer.Pages
{
    public class PageInfo
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }
 
        public PageInfo(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
 
        public bool HasPreviousPage => PageNumber > 1;
 
        public bool HasNextPage => PageNumber < TotalPages;
    }

    public class TableViewModel
    {
        public IEnumerable<WeatherArchiveRecord> Records { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
