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

        public List<WeatherArchiveRecord> GetAll()
        {
            return _context.WeatherArchiveRecords.ToList();
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
