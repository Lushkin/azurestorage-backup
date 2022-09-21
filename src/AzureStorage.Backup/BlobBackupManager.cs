namespace AzureStorage.Backup
{
    using AzureStorage.Backup.Abstraction;
    using AzureStorage.Backup.Configuration;
    using AzureStorage.Backup.Models;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The blob backup manager.
    /// </summary>
    public class BlobBackupManager : IBlobBackupManager
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly MainConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobBackupManager"/> class.
        /// </summary>
        /// <param name="blobStorageService">The blob storage service.</param>
        /// <param name="configuration">The configuration.</param>
        public BlobBackupManager(IBlobStorageService blobStorageService, IOptions<MainConfiguration> configuration)
        {
            _blobStorageService = blobStorageService;
            _configuration = configuration.Value;
        }

        /// <inheritdoc/>
        public void Start()
        {
            Console.WriteLine("BlobBackupManager started.");
            if (_configuration.ActionType.Equals(Constants.BACKUP))
            {
                Backup();
            }
            else if (_configuration.ActionType.Equals(Constants.RESTORE))
            {
                Restore();
            }
            else
            {
                Console.WriteLine($"Unsupported action {_configuration.ActionType}");
            }
            Console.WriteLine("BlobBackupManager stopped.");
        }

        /// <summary>
        /// Backups the Azure Storage blobs files.
        /// </summary>
        private void Backup()
        {
            Console.WriteLine($"Start BACKUP process");

            try
            {
                Console.WriteLine($"- Get Data:");
                Console.WriteLine($"-- Retrieve containers");
                var containers = _blobStorageService.GetContainers();
                Console.WriteLine($"--- {containers.Count} containers retrieved");

                Console.WriteLine($"- Process Data:");
                foreach (var container in containers)
                {
                    Console.WriteLine($"-- Start processing {container} container");
                    Console.WriteLine($"--- Retrieve data from Storage Account...");
                    var blobs = _blobStorageService.GetBlobs(container).ToList();
                    Console.WriteLine($"---- {blobs.Count} items retrieved");

                    Console.WriteLine($"--- Download {container} container items...");
                    _blobStorageService.DownloadBlobs(blobs, container);
                    Console.WriteLine($"---- Blobs downloaded with success.");
                    Console.WriteLine($"-- Finish processing {container} container");
                    Console.WriteLine($"-----------------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Backup process failure:");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine($"Finish BACKUP process");
        }

        /// <summary>
        /// Restores the Azure Storage blobs from files.
        /// </summary>
        private void Restore()
        {
            Console.WriteLine($"Start RESTORE process");

            try
            {
                Console.WriteLine($"- Get Data:");
                Console.WriteLine($"-- Retrieve container folders");
                var containers = _blobStorageService.GetContainerFolders();
                Console.WriteLine($"--- {containers.Count} container folders retrieved");


                Console.WriteLine($"- Process Data:");
                foreach (var container in containers)
                {
                    Console.WriteLine($"-- Start processing {container} container folder");
                    Console.WriteLine($"--- Retrieve data from folder...");
                    var files = _blobStorageService.GetFiles(container);
                    Console.WriteLine($"---- {files.Count} files retrieved");

                    Console.WriteLine($"--- Save files as blobs to {container} container...");
                    _blobStorageService.UploadBlobs(files, container);
                    Console.WriteLine($"---- Container populated with success.");
                    Console.WriteLine($"-- Finish processing {container} container");
                    Console.WriteLine($"-----------------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Restore process failure:");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine($"Finish RESTORE process");
        }
    }
}
