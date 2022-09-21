namespace AzureStorage.Backup.Configuration
{
    /// <summary>
    /// The main configuration.
    /// </summary>
    public class MainConfiguration
    {
        /// <summary>
        /// Gets or sets the action type.
        /// </summary>
        public string ActionType { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether table manager enabled.
        /// </summary>
        public bool TableManagerEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether blob manager enabled.
        /// </summary>
        public bool BlobManagerEnabled { get; set; }
    }
}
