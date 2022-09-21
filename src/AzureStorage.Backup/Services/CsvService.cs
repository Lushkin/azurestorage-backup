namespace AzureStorage.Backup.Services
{
    using Azure.Data.Tables;
    using AzureStorage.Backup.Abstraction;
    using AzureStorage.Backup.Configuration;
    using AzureStorage.Backup.Helpers;
    using CsvHelper;
    using Microsoft.Extensions.Options;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// The csv service.
    /// </summary>
    public class CsvService : ICsvService
    {
        private readonly BackupConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public CsvService(IOptions<BackupConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        /// <inheritdoc/>
        public void SaveToFile(IEnumerable<TableEntity> entities, string fileName)
        {
            var stringBuilder = new StringBuilder();
            var firstEntity = entities.FirstOrDefault();
            if (firstEntity is null)
            {
                return;
            }

            // Append Header
            var headerSB = new StringBuilder();
            foreach (var key in firstEntity.Keys.ToList())
            {
                // Ignore etag
                if (key.Equals("odata.etag"))
                {
                    continue;
                }

                headerSB.Append($"{key},");
            }
            var header = headerSB.ToString();
            stringBuilder.AppendLine(header.CleanRow());

            // Append rows
            foreach (var entity in entities)
            {
                var rowSb = new StringBuilder();
                foreach (var key in entity.Keys)
                {
                    // Ignore etag
                    if (key.Equals("odata.etag"))
                    {
                        continue;
                    }

                    if (entity.TryGetValue(key, out var value))
                    {
                        // Encapsulate the value as string, so it doesn't process comas in text as separators.
                        rowSb.Append($"\"{value.ToStrign()}\",");
                    }
                    else
                    {
                        rowSb.Append($"EMPTY,");
                    }
                }

                var row = rowSb.ToString();
                stringBuilder.AppendLine(row.CleanRow());
            }

            File.WriteAllText(fileName.GetFilePath(_configuration.TablesPath), stringBuilder.ToString(), Encoding.UTF8);
        }

        /// <inheritdoc/>
        public List<string> GetBackupFiles()
        {
            var files = Directory.GetFiles(_configuration.TablesPath);
            return files.ToList();
        }

        /// <inheritdoc/>
        public List<IDictionary<string, object>> GetFromFile(string filePath)
        {
            var result = new List<IDictionary<string, object>>();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>().ToList();

                foreach (var record in records)
                {
                    var r = record as IDictionary<string, object>;
                    if (r != null)
                    {
                        result.Add(r);
                    }
                }
            }

            return result;
        }
    }
}
