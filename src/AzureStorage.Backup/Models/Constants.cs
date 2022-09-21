namespace AzureStorage.Backup.Models
{
    /// <summary>
    /// The project constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The backup.
        /// </summary>
        public const string BACKUP = "Backup";

        /// <summary>
        /// The restore.
        /// </summary>
        public const string RESTORE = "Restore";

        /// <summary>
        /// The blob url.
        /// </summary>
        public const string BLOB_URL = "https://{0}.blob.core.windows.net/{1}/{2}";
    }
}
