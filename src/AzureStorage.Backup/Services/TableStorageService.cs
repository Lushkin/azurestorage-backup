namespace AzureStorage.Backup.Services
{
    using Azure.Data.Tables;
    using Azure.Data.Tables.Models;
    using AzureStorage.Backup.Abstraction;

    /// <summary>
    /// The storage service.
    /// </summary>
    public class TableStorageService : ITableStorageService
    {
        private readonly TableServiceClient _tableServiceClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableStorageService"/> class.
        /// </summary>
        /// <param name="tableServiceClient">The table service client.</param>
        public TableStorageService(TableServiceClient tableServiceClient)
        {
            _tableServiceClient = tableServiceClient;
        }

        /// <inheritdoc/>
        public List<TableItem> GetTables()
        {
            return _tableServiceClient.Query().ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<T> GetAllRows<T>(string tableName) where T : class, ITableEntity, new()
        {
            var tableClient = _tableServiceClient.GetTableClient(tableName);
            return tableClient.Query<T>();
        }

        /// <inheritdoc/>
        public void SaveRows<T>(List<T> entities, string tableName) where T : class, ITableEntity, new()
        {
            var tableClient = _tableServiceClient.GetTableClient(tableName);
            tableClient.CreateIfNotExists();
            
            foreach(var entity in entities)
            {
                tableClient.UpsertEntity(entity);
            }
        }
    }
}
