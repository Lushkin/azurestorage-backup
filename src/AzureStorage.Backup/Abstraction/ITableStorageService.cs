namespace AzureStorage.Backup.Abstraction
{
    using Azure.Data.Tables;
    using Azure.Data.Tables.Models;

    /// <summary>
    /// The storage service.
    /// </summary>
    public interface ITableStorageService
    {
        /// <summary>
        /// Gets the tables.
        /// </summary>
        /// <returns>A list of TableItems.</returns>
        List<TableItem> GetTables();

        /// <summary>
        /// Gets the all rows.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>A list of TS.</returns>
        IEnumerable<T> GetAllRows<T>(string tableName) where T : class, ITableEntity, new();

        /// <summary>
        /// Saves the rows.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="tableName">The table name.</param>
        void SaveRows<T>(List<T> entities, string tableName) where T : class, ITableEntity, new();
    }
}
