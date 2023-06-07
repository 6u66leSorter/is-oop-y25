using Backups.Algorithms;
using Backups.Archivers;
using Backups.Backup;
using Backups.BackupObjects;
using Backups.Exceptions;
using Backups.Extra.Algorithms;
using Backups.Extra.Logging;
using Backups.Extra.Models;
using Backups.Models;
using Backups.Repositories;
using Backups.Storages;

namespace Backups.Extra.Services;

public class BackupExtraTask
{
    private readonly List<IBackupObject> _backupObjects;

    public BackupExtraTask(ILogger logger, string taskName, IRepository repository, IStorageAlgorithm algorithm, Backup.Backup backup, QuantitySelector qSelection, DateSelector dSelector, SelectionAlgorithmCondition condition)
    {
        TaskName = taskName;
        Repository = repository;
        Algorithm = algorithm;
        Backup = backup;
        _backupObjects = new List<IBackupObject>();
        QuantitySelector = qSelection;
        DateSelector = dSelector;
        Condition = condition;
        Logger = logger;
    }

    public ILogger Logger { get; }
    public string TaskName { get; }

    public IRepository Repository { get; }

    public IStorageAlgorithm Algorithm { get; }

    public QuantitySelector QuantitySelector { get; }

    public DateSelector DateSelector { get; }

    public Backup.Backup Backup { get; }

    public SelectionAlgorithmCondition Condition { get; set; }

    public IReadOnlyCollection<IBackupObject> BackupObjects => _backupObjects;

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

    public void CleanBackup()
    {
        var restorePoints = new List<RestorePoint>();
        IReadOnlyCollection<RestorePoint> dateRestorePoints;
        IReadOnlyCollection<RestorePoint> quantityRestorePoints;

        switch (Condition)
        {
            case SelectionAlgorithmCondition.Data:
                restorePoints = DateSelector.SelectRestorePoints(Backup.RestorePoints) as List<RestorePoint>;
                break;
            case SelectionAlgorithmCondition.Quantity:
                restorePoints = QuantitySelector.SelectRestorePoints(Backup.RestorePoints) as List<RestorePoint>;
                break;
            case SelectionAlgorithmCondition.Intersection:
                dateRestorePoints = DateSelector.SelectRestorePoints(Backup.RestorePoints);
                quantityRestorePoints = QuantitySelector.SelectRestorePoints(Backup.RestorePoints);
                restorePoints = dateRestorePoints.Intersect(quantityRestorePoints) as List<RestorePoint>;
                break;
            case SelectionAlgorithmCondition.Union:
                dateRestorePoints = DateSelector.SelectRestorePoints(Backup.RestorePoints);
                quantityRestorePoints = QuantitySelector.SelectRestorePoints(Backup.RestorePoints);
                restorePoints = dateRestorePoints.Union(quantityRestorePoints) as List<RestorePoint>;
                break;
            case SelectionAlgorithmCondition.Non:
                break;
        }

        if (restorePoints is null) return;
        foreach (RestorePoint restorePoint in restorePoints)
        {
            Backup.RemoveRestorePoint(restorePoint);
        }
    }

    public void Execute()
    {
        var restorePoint = new RestorePoint(Guid.NewGuid(), _backupObjects, DateTime.Now);
        Backup.AddRestorePoint(restorePoint);
        Logger.Log("Restore Point was created", DateTime.Now);
        CleanBackup();
        IReadOnlyCollection<SingleStorage> storages = Algorithm.MakeDataPackage(_backupObjects);
        string archivePath = Repository.MakeArchivePath(restorePoint.DateTime, TaskName);
        var archiver = new ZipArchiver();
        archiver.Archive(storages, Repository, archivePath);
        Logger.Log("Archive was created");
    }
}