namespace AzureStorage.Backup.Configuration
{
    /// <summary>
    /// The backup configuration.
    /// </summary>
    public class BackupConfiguration
    {
        /// <summary>
        /// Gets or sets the tables path.
        /// </summary>
        public string TablesPath { get; set; } = default!;

        /// <summary>
        /// Gets or sets the blob path.
        /// </summary>
        public string BlobPath { get; set; } = default!;
    }
}
