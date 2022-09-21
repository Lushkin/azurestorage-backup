namespace AzureStorage.Backup
{
    using Azure.Data.Tables;
    using AzureStorage.Backup.Abstraction;
    using AzureStorage.Backup.Configuration;
    using AzureStorage.Backup.Helpers;
    using AzureStorage.Backup.Models;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The table backup manager.
    /// </summary>
    public class TableBackupManager : ITableBackupManager
    {
        private readonly ITableStorageService _tableStorageService;
        private readonly ICsvService _csvService;
        private readonly MainConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableBackupManager"/> class.
        /// </summary>
        /// <param name="tableStorageService">The table storage service.</param>
        /// <param name="csvService">The csv service.</param>
        /// <param name="configuration">The configuration.</param>
        public TableBackupManager(ITableStorageService tableStorageService, ICsvService csvService, IOptions<MainConfiguration> configuration)
        {
            _tableStorageService = tableStorageService;
            _csvService = csvService;
            _configuration = configuration.Value;
        }

        /// <inheritdoc/>
        public void Start()
        {
            Console.WriteLine("TableBackupManager started.");
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
            Console.WriteLine("TableBackupManager stopped.");
        }

        /// <summary>
        /// Backups the Azure Storage tables to CSV file.
        /// </summary>
        private void Backup()
        {
            Console.WriteLine($"Start BACKUP process");

            try
            {
                Console.WriteLine($"- Get Data:");
                Console.WriteLine($"-- Retrieve tables");
                var tables = _tableStorageService.GetTables();
                Console.WriteLine($"--- {tables.Count} tables retrieved");


                Console.WriteLine($"- Process Data:");
                foreach (var table in tables)
                {
                    Console.WriteLine($"-- Start processing {table.Name} table");
                    Console.WriteLine($"--- Retrieve data from Storage Account...");
                    var entities = _tableStorageService.GetAllRows<TableEntity>(table.Name).ToList();
                    Console.WriteLine($"---- {entities.Count} items retrieved");

                    Console.WriteLine($"--- Save items to {table.Name}.csv file...");
                    _csvService.SaveToFile(entities, table.Name);
                    Console.WriteLine($"---- File saved with success.");
                    Console.WriteLine($"-- Finish processing {table.Name} table");
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
        /// Restores the Azure Storage tables from CSV file.
        /// </summary>
        private void Restore()
        {
            Console.WriteLine($"Start RESTORE process");

            try
            {
                Console.WriteLine($"- Get Data:");
                Console.WriteLine($"-- Retrieve files");
                var files = _csvService.GetBackupFiles();
                Console.WriteLine($"--- {files.Count} files retrieved");


                Console.WriteLine($"- Process Data:");
                foreach (var file in files)
                {
                    Console.WriteLine($"-- Start processing {file} file");
                    Console.WriteLine($"--- Retrieve data from file...");
                    var rows = _csvService.GetFromFile(file);
                    var entities = new List<TableEntity>();
                    Console.WriteLine($"---- {rows.Count} items retrieved");

                    Console.WriteLine($"--- Transform items to entities...");
                    foreach (var row in rows)
                    {
                        entities.Add(new TableEntity(row));
                    }
                    Console.WriteLine($"---- Obtained {entities.Count} entities");

                    Console.WriteLine($"--- Save items to {file.GetName()} table...");
                    _tableStorageService.SaveRows(entities, file.GetName());
                    Console.WriteLine($"---- Table populated with success.");
                    Console.WriteLine($"-- Finish processing {file} file");
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
