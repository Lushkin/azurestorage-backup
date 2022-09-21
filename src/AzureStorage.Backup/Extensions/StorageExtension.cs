namespace AzureStorage.Backup.Extensions
{
    using Azure.Data.Tables;
    using Azure.Storage.Blobs;
    using AzureStorage.Backup.Abstraction;
    using AzureStorage.Backup.Configuration;
    using AzureStorage.Backup.Models;
    using AzureStorage.Backup.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The storage extension.
    /// </summary>
    public static class StorageExtension
    {
        /// <summary>
        /// Adds the storage.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="config">The config.</param>
        public static void AddStorage(this IServiceCollection services, IConfiguration config)
        {
            services.AddTables(config);
            services.AddBlobs(config);
        }

        /// <summary>
        /// Adds the tables.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="config">The config.</param>
        private static void AddTables(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<ITableStorageService, TableStorageService>(opt =>
            {
                var actionType = config.GetSection("Core:ActionType").Value;
                string connectionString;
                if (actionType.Equals(Constants.BACKUP))
                {
                    connectionString = config.GetSection("Storage:Backup:ConnectionString").Value;
                }
                else if (actionType.Equals(Constants.RESTORE))
                {
                    connectionString = config.GetSection("Storage:Restore:ConnectionString").Value;
                }
                else
                {
                    connectionString = string.Empty;
                }

                var tableServiceClient = new TableServiceClient(connectionString);
                return new TableStorageService(tableServiceClient);
            });
        }

        /// <summary>
        /// Adds the blobs.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="config">The config.</param>
        private static void AddBlobs(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IBlobStorageService, BlobStorageService>(opt =>
            {
                var actionType = config.GetSection("Core:ActionType").Value;
                string connectionString;
                string storageAccountName;
                if (actionType.Equals(Constants.BACKUP))
                {
                    connectionString = config.GetSection("Storage:Backup:ConnectionString").Value;
                    storageAccountName = config.GetSection("Storage:Backup:AccountName").Value;
                }
                else if (actionType.Equals(Constants.RESTORE))
                {
                    connectionString = config.GetSection("Storage:Restore:ConnectionString").Value;
                    storageAccountName = config.GetSection("Storage:Restore:AccountName").Value;
                }
                else
                {
                    connectionString = string.Empty;
                    storageAccountName = string.Empty;
                }

                var backupPath = config.GetSection("Backup:BlobPath").Value;

                var blobServiceClient = new BlobServiceClient(connectionString);
                return new BlobStorageService(blobServiceClient, storageAccountName, backupPath);
            });
        }
    }
}
