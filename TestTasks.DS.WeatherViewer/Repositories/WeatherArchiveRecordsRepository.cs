using Microsoft.EntityFrameworkCore;
using TestTasks.DS.WeatherViewer.Models.DBEntities;

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

        public async Task<List<WeatherArchiveRecord>> GetAllByPageAsync(int pageSize, int pageNumber)
        {
            var result = await _context.WeatherArchiveRecords
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return result;
        }

        public async Task<int> GetCountAsync()
        {
            var result = await _context.WeatherArchiveRecords
                .Select(r => r.Id)
                .CountAsync();

            return result;
        }

        public async Task<IEnumerable<long>> InsertRangeAsync(IEnumerable<WeatherArchiveRecord> records)
        {
            await _context.WeatherArchiveRecords.AddRangeAsync(records);
            await _context.SaveChangesAsync();
            return records.Select(r => r.Id);
        }
    }
}
