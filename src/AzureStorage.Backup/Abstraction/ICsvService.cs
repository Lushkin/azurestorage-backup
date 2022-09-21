namespace AzureStorage.Backup.Abstraction
{
    using Azure.Data.Tables;

    /// <summary>
    /// The csv service.
    /// </summary>
    public interface ICsvService
    {
        /// <summary>
        /// Saves the to file.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns>A Task.</returns>
        void SaveToFile(IEnumerable<TableEntity> entities, string fileName);

        /// <summary>
        /// Gets the from file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>A list of IDictionary.</returns>
        List<IDictionary<string, object>> GetFromFile(string filePath);

        /// <summary>
        /// Gets the backup files.
        /// </summary>
        /// <returns>A list of string.</returns>
        List<string> GetBackupFiles();
    }
}
