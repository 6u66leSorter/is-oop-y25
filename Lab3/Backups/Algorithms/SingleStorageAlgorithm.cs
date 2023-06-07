using Backups.BackupObjects;
using Backups.Entities;
using Backups.Repositories;
using Backups.Storages;

namespace Backups.Algorithms;

public class SingleStorageAlgorithm : IStorageAlgorithm
{
    public IReadOnlyCollection<SingleStorage> MakeDataPackage(List<IBackupObject> backupObjects)
    {
        var storages = new List<SingleStorage>();
        var storage = new SingleStorage();

        foreach (IBackupObject backupObject in backupObjects)
        {
            storage.AddBackupObject(backupObject);
        }

        storages.Add(storage);
        return storages;
    }
}