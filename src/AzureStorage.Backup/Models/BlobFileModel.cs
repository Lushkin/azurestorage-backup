namespace AzureStorage.Backup.Models
{
    /// <summary>
    /// The blob file model.
    /// </summary>
    public class BlobFileModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = default!;
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; } = default!;
    }
}
