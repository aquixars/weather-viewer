using TestTasks.DS.WeatherViewer.Models.DBEntities;

namespace TestTasks.DS.WeatherViewer.Repositories
{
    public class WeatherArchiveRecordsRepository
    {
        private readonly WeatherViewerDbContext _context;

        public WeatherArchiveRecordsRepository(WeatherViewerDbContext context)
        {
            _context = context;
        }

        public List<WeatherArchiveRecord> GetAll()
        {
            using (_context)
            {
                return _context.WeatherArchiveRecords.ToList();
            }
        }

        public long Add(WeatherArchiveRecord record)
        {
            using (_context)
            {
                _context.WeatherArchiveRecords.Add(record);
                _context.SaveChanges();
                return record.Id;
            }
        }

        public IEnumerable<long> AddRange(IEnumerable<WeatherArchiveRecord> records)
        {
            using (_context)
            {
                _context.WeatherArchiveRecords.AddRange(records);
                _context.SaveChanges();
                return records.Select(r => r.Id);
            }
        }
    }
}
