namespace AzureStorage.Backup.Abstraction
{
    using Azure.Storage.Blobs.Models;
    using AzureStorage.Backup.Models;

    /// <summary>
    /// The blob storage service.
    /// </summary>
    public interface IBlobStorageService
    {
        /// <summary>
        /// Gets the containers.
        /// </summary>
        /// <returns>A list of string.</returns>
        List<string> GetContainers();

        /// <summary>
        /// Gets the blobs.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>A list of BlobItems.</returns>
        List<BlobItem> GetBlobs(string container);

        /// <summary>
        /// Downloads the blobs.
        /// </summary>
        /// <param name="blobs">The blobs.</param>
        /// <param name="container">The container.</param>
        void DownloadBlobs(List<BlobItem> blobs, string container);

        /// <summary>
        /// Gets the container folders.
        /// </summary>
        /// <returns>A list of string.</returns>
        List<string> GetContainerFolders();

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>A list of BlobFileModels.</returns>
        List<BlobFileModel> GetFiles(string container);

        /// <summary>
        /// Uploads the blobs.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="container">The container.</param>
        void UploadBlobs(List<BlobFileModel> files, string container);
    }
}
