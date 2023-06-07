using Backups.BackupObjects;
using Backups.Entities;
using Backups.Storages;

namespace Backups.Algorithms;

public interface IStorageAlgorithm
{
    IReadOnlyCollection<SingleStorage> MakeDataPackage(List<IBackupObject> backupObjects);
}