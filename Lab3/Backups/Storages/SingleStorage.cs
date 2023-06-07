using Backups.BackupObjects;

namespace Backups.Storages;

public class SingleStorage : IStorage
{
    private readonly List<IBackupObject> _backupObjects;
    public SingleStorage()
    {
        _backupObjects = new List<IBackupObject>();
    }

    public SingleStorage(List<IBackupObject> backupObjects)
    {
        _backupObjects = backupObjects;
    }

    public IReadOnlyCollection<IBackupObject> BackupObjects => _backupObjects.AsReadOnly();

    public void AddBackupObject(IBackupObject backupObject)
    {
        ArgumentNullException.ThrowIfNull(backupObject);

        _backupObjects.Add(backupObject);
    }

    public void RemoveBackupObject(IBackupObject backupObject)
    {
        ArgumentNullException.ThrowIfNull(backupObject);

        if (!_backupObjects.Remove(backupObject))
        {
            throw new NotImplementedException();
        }
    }
}