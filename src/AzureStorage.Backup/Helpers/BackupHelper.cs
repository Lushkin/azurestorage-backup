using AzureStorage.Backup.Models;

namespace AzureStorage.Backup.Helpers
{
    /// <summary>
    /// The backup helper.
    /// </summary>
    public static class BackupHelper
    {
        /// <summary>
        /// Cleans the row.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A string.</returns>
        public static string CleanRow(this string value)
        {
            if (value.EndsWith(","))
            {
                value = value.Remove(value.Length - 1);
            }

            return value;
        }

        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="path">The path.</param>
        /// <returns>A string.</returns>
        public static string GetFilePath(this string fileName, string path) => $"{path}/{fileName}.csv";

        /// <summary>
        /// Gets the file name.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A string.</returns>
        public static string GetFileName(this string path)
        {
            path = path.Replace("\\", "/");
            var index = path.LastIndexOf('/');
            var fileName = path.Remove(0, index + 1);
            return fileName;
        }

        /// <summary>
        /// Gets the file name for blob.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="backupPath">The backup path.</param>
        /// <returns>A string.</returns>
        public static string GetFileNameForBlob(this string path, string backupPath)
        {
            path = path.Replace(backupPath, string.Empty);
            var fileName = path.Replace("\\", "/");
            return fileName;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A string.</returns>
        public static string GetName(this string path) => Path.GetFileName(path).Replace(".csv", string.Empty);

        /// <summary>
        /// Custom transform to the strign.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A string.</returns>
        public static string ToStrign(this Object value) => (value.ToString() ?? string.Empty).Replace(@"""", @"\""");

        /// <summary>
        /// Gets the blob url.
        /// </summary>
        /// <param name="blobName">The blob name.</param>
        /// <param name="storageAccountName">The storage account name.</param>
        /// <param name="container">The container.</param>
        /// <returns>A string.</returns>
        public static string GetBlobUrl(this string blobName, string storageAccountName, string container) =>
            string.Format(Constants.BLOB_URL, storageAccountName, container, blobName);
    }
}
