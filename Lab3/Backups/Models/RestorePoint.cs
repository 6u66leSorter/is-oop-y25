using Backups.BackupObjects;

namespace Backups.Models;

public class RestorePoint
{
    private readonly List<IBackupObject> _backupObjects;

    public RestorePoint(Guid id, List<IBackupObject> backupObjects, DateTime dateTime)
    {
        Id = id;
        _backupObjects = backupObjects;
        DateTime = dateTime;
    }

    public DateTime DateTime { get; set; }
    public Guid Id { get; }

    public IReadOnlyCollection<IBackupObject> BackupObjects => _backupObjects.AsReadOnly();
}