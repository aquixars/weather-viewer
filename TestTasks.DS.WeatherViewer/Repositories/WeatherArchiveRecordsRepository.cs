using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.Record;
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

        // todo: speed it up
        public async Task<int> GetCountAsync()
        {
            var result = await _context.WeatherArchiveRecords
                .CountAsync();

            return result;
        }

        public long Insert(WeatherArchiveRecord record)
        {
            _context.WeatherArchiveRecords.Add(record);
            _context.SaveChanges();
            return record.Id;
        }

        public IEnumerable<long> InsertRange(IEnumerable<WeatherArchiveRecord> records)
        {
            _context.WeatherArchiveRecords.AddRange(records);
            _context.SaveChanges();
            return records.Select(r => r.Id);
        }
    }
}
