using WeatherViewer.Models.DBEntities;

namespace WeatherViewer.Pages
{
    public class PageInfo
    {
        public int PageSize { get; private set; }
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }
 
        public PageInfo(int count, int pageNumber, int pageSize)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
 
        public bool HasPreviousPage => PageNumber > 1;
 
        public bool HasNextPage => PageNumber < TotalPages;
    }

    public class FilterInfo
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }

    public class TableViewModel
    {
        public string Message { get; set; }
        public IEnumerable<WeatherArchiveRecord> Records { get; set; }
        public PageInfo PageInfo { get; set; }
        public FilterInfo FilterInfo { get; set; } = new FilterInfo();
    }
}
