# Azure Storage Backup

![Watchers](https://badgen.net/github/license/Lushkin/azurestorage-backup)
![Watchers](https://badgen.net/github/stars/Lushkin/azurestorage-backup)
![Watchers](https://badgen.net/github/watchers/Lushkin/azurestorage-backup)
![Watchers](https://badgen.net/github/forks/Lushkin/azurestorage-backup)
![Watchers](https://badgen.net/github/commits/Lushkin/azurestorage-backup/main)


Console application that backups the Azure Storage account Tables and Blobs.

Uses .Net 6

Supports `UserSecrets`.

No entity definition required to backup `Tables`, uses `dynamic` conversion of the data.


## Backup
- Backups tables in separate `.csv` files.
- Backups blobs : download files to the choosen location on your computer.


## Restore
- Restore tables from `.csv` files to choosen `Azure Storage Account`.
- Backup blobs : create containers and upload files as `blob` to choosen `Azure Storage Account`


## How to use
Fill the `appsettings.json` file (or `secrets.json` file if you are using the User Secrets) with your Azure Storage Account information.

```json
{
  "Core": {
    "TableManagerEnabled": "true",
    "BlobManagerEnabled": "false",
    "ActionType": "Restore" // Backup or Restore
  },
  "Storage": {
    "Backup": {
      "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=<YourAccountName>;AccountKey=IaaaaaaaaaaaaaRQ//0mlaaaaaaaaaaaaaA4/8aaaaaaaaaaaaa6/JaaaaaaaaaaA==;EndpointSuffix=core.windows.net",
      "AccountName": "<YourAccountName>"
    },
    "Restore": {
      "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=<YourAccountName>;AccountKey=IaaaaaaaaaaaaaRQ//0mlaaaaaaaaaaaaaA4/8aaaaaaaaaaaaa6/JaaaaaaaaaaA==;EndpointSuffix=core.windows.net",
      "AccountName": "<YourAccountName>"
    }
  },
  "Backup": {
    "TablesPath": "C:\\Your\\Backup\\Location\\backup\\tables",
    "BlobPath": "C:\\Your\\Backup\\Location\\backup\\blobs"
  }
}
```

### Storage section
If you want to transfer data from one account to another, you can specify two different accounts in the `Storage` section, one where you would get your data from (`Backup`), and the other where you want your data to be saved to (`Restore`)

### Core section
Here you can define whether you want to `Backup` or `Restore` your data.
> In order to prevent unwanted data loss, the `Backup` and `Restore` operations have to be executed separately.

You can also chose the `Manager` to enable (if you want to backup/restore Tables only, Blobs only or both).

### Backup section
Here you can define the location where the Backuped files (.csv and blobs) would be saved.
> The same location is used for `Backup` and `Restore` operations, but you can define different locations for Tables and Blobs.

--------------------
## Dependencies

- [Azure Tables client library for .NET](https://github.com/Azure/azure-sdk-for-net/blob/Azure.Data.Tables_12.6.1/sdk/tables/Azure.Data.Tables/README.md) (12.6.1)

- [Azure Storage Blobs client library for .NET](https://github.com/Azure/azure-sdk-for-net/blob/Azure.Storage.Blobs_12.13.1/sdk/storage/Azure.Storage.Blobs/README.md) (12.6.1)

- [CSV Helper](https://joshclose.github.io/CsvHelper/) (28.0.1)