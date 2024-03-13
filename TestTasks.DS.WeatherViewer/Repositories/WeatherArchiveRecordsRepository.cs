using Microsoft.EntityFrameworkCore;
using TestTasks.DS.WeatherViewer.Models.DBEntities;
using TestTasks.DS.WeatherViewer.Pages;

namespace TestTasks.DS.WeatherViewer.Repositories
{
    public class WeatherArchiveRecordsRepository : IDisposable
    {
        private readonly WeatherViewerDbContext _context;

        public WeatherArchiveRecordsRepository(WeatherViewerDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public async Task<(List<WeatherArchiveRecord> records, int totalCount)> GetAllByPageModel(int pageNumber, int pageSize, int month, int year)
        {
            IQueryable<WeatherArchiveRecord> query = _context.WeatherArchiveRecords;

            if (year != 0)
            {
                query = query.Where(r => r.Created.HasValue && r.Created.Value.Year == year);
            }

            if (month != 0)
            {
                query = query.Where(r => r.Created.HasValue && r.Created.Value.Month == month);
            }

            var totalCount = await query.CountAsync();

            var result = await query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (result, totalCount);
        }

        public async Task<IEnumerable<long>> InsertRangeAsync(IEnumerable<WeatherArchiveRecord> records)
        {
            await _context.WeatherArchiveRecords.AddRangeAsync(records);
            await _context.SaveChangesAsync();
            return records.Select(r => r.Id);
        }
    }
}
