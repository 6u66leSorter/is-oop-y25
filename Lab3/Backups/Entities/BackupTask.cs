using Backups.Algorithms;
using Backups.Archivers;
using Backups.Backup;
using Backups.BackupObjects;
using Backups.Exceptions;
using Backups.Models;
using Backups.Repositories;
using Backups.Storages;

namespace Backups.Entities;

public class BackupTask
{
    private readonly List<IBackupObject> _backupObjects;
    private readonly IRepository _repository;
    private readonly IStorageAlgorithm _algorithm;
    private readonly Backup.Backup _backup;

    public BackupTask(string taskName, IRepository repository, IStorageAlgorithm algorithm, Backup.Backup backup)
    {
        TaskName = taskName;
        _repository = repository;
        _algorithm = algorithm;
        _backup = backup;
        _backupObjects = new List<IBackupObject>();
    }

    public string TaskName { get; }
    public IReadOnlyCollection<IBackupObject> BackupObjects => _backupObjects;
    public IReadOnlyCollection<RestorePoint> RestorePoints => _backup.RestorePoints;

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
            throw BackupTaskException.NoSuchBackupObject();
        }
    }

    public void Execute()
    {
        var restorePoint = new RestorePoint(Guid.NewGuid(), _backupObjects, DateTime.Now);
        _backup.AddRestorePoint(restorePoint);
        IReadOnlyCollection<SingleStorage> storages = _algorithm.MakeDataPackage(_backupObjects);
        string archivePath = _repository.MakeArchivePath(restorePoint.DateTime, TaskName);
        var archiver = new ZipArchiver();
        archiver.Archive(storages, _repository, archivePath);
    }
}