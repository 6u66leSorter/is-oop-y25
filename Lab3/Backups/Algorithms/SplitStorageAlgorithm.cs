using Backups.BackupObjects;
using Backups.Entities;
using Backups.Storages;

namespace Backups.Algorithms;

public class SplitStorageAlgorithm : IStorageAlgorithm
{
    public IReadOnlyCollection<SingleStorage> MakeDataPackage(List<IBackupObject> backupObjects)
    {
        var storages = new List<SingleStorage>();

        foreach (IBackupObject backupObject in backupObjects)
        {
            var storage = new SingleStorage();
            storage.AddBackupObject(backupObject);
            storages.Add(storage);
        }

        return storages;
    }
}