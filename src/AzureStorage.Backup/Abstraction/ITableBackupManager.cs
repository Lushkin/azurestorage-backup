namespace AzureStorage.Backup.Abstraction
{
    /// <summary>
    /// The backup manager.
    /// </summary>
    public interface ITableBackupManager
    {
        /// <summary>
        /// Starts the Table backup process.
        /// </summary>
        void Start();
    }
}
