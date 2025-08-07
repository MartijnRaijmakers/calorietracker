using CsvHelper;
using Persistance.Dto;
using System.Globalization;

namespace Persistance.Repositories
{
    public class CsvRepository<T> : IRepository<T> where T : DtoEntity
    {
        private const int RETRY_DELAY_IN_MS = 500;
        private const int RETRY_COUNT = 5;

        private string Path { get; }
        private List<T> CachedRecords { get; set; }

        public CsvRepository(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path cannot be null or empty", nameof(path));

            Path = path;
            CachedRecords = GetRecordCache();
        }

        public IEnumerable<T> GetAll()
        {
            return CachedRecords;
        }

        public void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

            entity = UpdateIdForEntity(entity);
            CachedRecords = this.GetAll().Append(entity).ToList();
            this.PersistToFile();
        }

        public void Delete(int id)
        {
            var recordToDelete = this.GetAll().FirstOrDefault(r => r.Id == id);

            if (recordToDelete == null)
                throw new KeyNotFoundException($"No record found with ID {id}");

            CachedRecords = this.GetAll().Where(r => r.Id != recordToDelete.Id).ToList();
            this.PersistToFile();
        }


        public T GetById(int id)
        {
            var record = this.GetAll().FirstOrDefault(r => r.Id == id);

            if (record == null)
                throw new KeyNotFoundException($"No record found with ID {id}");

            return record;
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

            if (!entity.Id.HasValue)
                throw new ArgumentException(nameof(entity.Id), "Entity to be updated must have an Id value");

            var existingRecord = this.GetById(entity.Id.Value);

            if (existingRecord == null)
                throw new KeyNotFoundException($"No record found with ID {entity.Id}");

            CachedRecords = this.GetAll().Select(record => record.Id == entity.Id ? entity : record).ToList();
            this.PersistToFile();
        }



        private T UpdateIdForEntity(T entity)
        {
            if (entity.Id.HasValue)
            {
                var maxId = this.GetAll().Max(r => r.Id);
                entity.Id = maxId + 1;
            }

            return entity;
        }

        private List<T> GetRecordCache()
        {
            if (!File.Exists(Path))
            {
                using var file = File.Create(Path);
                file.Dispose();
                return [];
            }

            using var reader = new StreamReader(Path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<T>().ToList();

            return records;
        }

        private void PersistToFile(int retryCount = 0)
        {
            try
            {
                using var writer = new StreamWriter(Path);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(CachedRecords);
            }
            catch (Exception ex)
            {
                if (retryCount < RETRY_COUNT)
                {
                    Console.WriteLine($"Try {retryCount}");
                    Thread.Sleep(RETRY_DELAY_IN_MS);
                    PersistToFile(++retryCount);
                }
                else
                {
                    throw new IOException($"Failed to persist records to file after {retryCount} attempts.", ex);
                }
            }
        }
    }
}
