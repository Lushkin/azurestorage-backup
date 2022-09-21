// See https://aka.ms/new-console-template for more information
using Azure.Data.Tables;
using AzureStorage.Backup;
using AzureStorage.Backup.Abstraction;
using AzureStorage.Backup.Configuration;
using AzureStorage.Backup.Extensions;
using AzureStorage.Backup.Models;
using AzureStorage.Backup.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Azure Storage Backup started!");

var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
                                devEnvironmentVariable.ToLower() == "development";

var configuration = new ConfigurationBuilder()
     .SetBasePath(Directory.GetCurrentDirectory())
     .AddJsonFile($"appsettings.json");

if (isDevelopment)
{
    configuration.AddUserSecrets<Program>();
}

IConfiguration config = configuration.Build();

// Create the service container
var builder = new ConfigurationBuilder();
var services = new ServiceCollection();

// Configuration
services.Configure<BackupConfiguration>(opt => config.Bind("Backup", opt));
services.Configure<MainConfiguration>(opt => config.Bind("Core", opt));

// Services
services.AddStorage(config);
services.AddTransient<ICsvService, CsvService>();
services.AddTransient<ITableBackupManager, TableBackupManager>();
services.AddTransient<IBlobBackupManager, BlobBackupManager>();
services.AddSingleton<Main, Main>();

var serviceProvider = services.BuildServiceProvider();
var app = serviceProvider.GetRequiredService<Main>();