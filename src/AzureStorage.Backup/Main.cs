namespace AzureStorage.Backup
{
    using AzureStorage.Backup.Abstraction;
    using AzureStorage.Backup.Configuration;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The main.
    /// </summary>
    public class Main
    {
        private readonly ITableBackupManager _tableBackupManager;
        private readonly IBlobBackupManager _blobBackupManager;
        private readonly MainConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        /// <param name="tableBackupManager">The table backup manager.</param>
        /// <param name="blobBackupManager">The blob backup manager.</param>
        /// <param name="configuration">The configuration.</param>
        public Main(ITableBackupManager tableBackupManager, IBlobBackupManager blobBackupManager, IOptions<MainConfiguration> configuration)
        {
            _tableBackupManager = tableBackupManager;
            _blobBackupManager = blobBackupManager;
            _configuration = configuration.Value;

            Execute();
        }

        /// <summary>
        /// Starts managers.
        /// </summary>
        private void Execute()
        {
            if (_configuration.TableManagerEnabled)
            {
                _tableBackupManager.Start();
            }

            if (_configuration.BlobManagerEnabled)
            {
                _blobBackupManager.Start();
            }
        }
    }
}
