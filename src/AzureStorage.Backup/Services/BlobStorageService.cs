namespace AzureStorage.Backup.Services
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using AzureStorage.Backup.Abstraction;
    using AzureStorage.Backup.Helpers;
    using AzureStorage.Backup.Models;
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// The blob storage service.
    /// </summary>
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _storageAccountName;
        private readonly string _backupPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageService"/> class.
        /// </summary>
        /// <param name="blobServiceClient">The blob service client.</param>
        /// <param name="storageAccountName">The storage account name.</param>
        /// <param name="backupPath">The backup path.</param>
        public BlobStorageService(BlobServiceClient blobServiceClient, string storageAccountName, string backupPath)
        {
            _blobServiceClient = blobServiceClient;
            _storageAccountName = storageAccountName;
            _backupPath = backupPath;
        }

        /// <inheritdoc/>
        public List<string> GetContainers()
        {
            return _blobServiceClient.GetBlobContainers().Select(m => m.Name).ToList();
        }

        /// <inheritdoc/>
        public List<BlobItem> GetBlobs(string container)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(container);
            return containerClient.GetBlobs().ToList();
        }

        /// <inheritdoc/>
        public void DownloadBlobs(List<BlobItem> blobs, string container)
        {
            foreach (var blob in blobs)
            {
                PrepareBlobFolders(blob, container);
                var blobClient = new BlobClient(new Uri(blob.Name.GetBlobUrl(_storageAccountName, container)));
                blobClient.DownloadTo($"{_backupPath}/{container}/{blob.Name}");
            }
        }

        /// <inheritdoc/>
        public List<string> GetContainerFolders()
        {
            var containers = Directory.GetDirectories(_backupPath);
            return containers.Select(f => f.GetFileName()).ToList();
        }

        /// <inheritdoc/>
        public List<BlobFileModel> GetFiles(string container)
        {
            var blobFiles = new List<BlobFileModel>();
            var containerPath = $"{_backupPath}\\{container}";
            GetFiles(blobFiles, containerPath, containerPath);
            return blobFiles;
        }

        /// <inheritdoc/>
        public void UploadBlobs(List<BlobFileModel> files, string container)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(container);
            containerClient.CreateIfNotExists(PublicAccessType.Blob);

            foreach (var file in files)
            {
                BlobClient blob = containerClient.GetBlobClient(file.Name);
                blob.Upload(file.Path);
            }
        }

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="blobFiles">The blob files.</param>
        /// <param name="path">The path.</param>
        /// <param name="containerPath">The container path.</param>
        private void GetFiles(List<BlobFileModel> blobFiles, string path, string containerPath)
        {
            var files = Directory.GetFiles(path);
            if (files.Length > 0)
            {
                blobFiles.AddRange(files.Select(f => new BlobFileModel { Name = f.GetFileNameForBlob(containerPath), Path = f }));
            }

            var folders = Directory.GetDirectories(path);
            if (folders.Length > 0)
            {
                foreach (var folder in folders)
                {
                    GetFiles(blobFiles, folder, containerPath);
                }
            }
        }

        /// <summary>
        /// Prepares the blob folders.
        /// </summary>
        /// <param name="blob">The blob.</param>
        /// <param name="container">The container.</param>
        private void PrepareBlobFolders(BlobItem blob, string container)
        {
            if (blob.Name.Contains('/'))
            {
                var index = blob.Name.LastIndexOf("/");
                var foldersPath = blob.Name.Remove(index);
                if (!Directory.Exists($"{_backupPath}/{foldersPath}"))
                {
                    Directory.CreateDirectory($"{_backupPath}/{container}/{foldersPath}");
                }
            }
        }
    }
}
