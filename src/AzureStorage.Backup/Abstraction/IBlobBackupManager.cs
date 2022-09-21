namespace AzureStorage.Backup.Abstraction
{
    /// <summary>
    /// The blob backup manager.
    /// </summary>
    public interface IBlobBackupManager
    {
        /// <summary>
        /// Starts the Blob backup process.
        /// </summary>
        void Start();
    }
}
