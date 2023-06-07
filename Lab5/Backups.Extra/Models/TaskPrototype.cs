using Backups.Algorithms;
using Backups.BackupObjects;
using Backups.Extra.Algorithms;
using Backups.Extra.Services;
using Backups.Repositories;

namespace Backups.Extra.Models;

public class TaskPrototype
{
    public TaskPrototype(BackupExtraTask backupExtraTask)
    {
        QuantitySelector = backupExtraTask.QuantitySelector.Quantity;
        DateSelector = backupExtraTask.DateSelector.DateTime;
        Repository = backupExtraTask.Repository.GetPath();
        TaskName = backupExtraTask.TaskName;
        Backup = backupExtraTask.Backup;
        Algorithm = backupExtraTask.Algorithm.GetType().Name;
        BackupObjects = new List<string>();
        foreach (IBackupObject backupObject in backupExtraTask.BackupObjects)
        {
            BackupObjects.Add(backupObject.GetPath());
        }

        Condition = (int)backupExtraTask.Condition;
        Logger = backupExtraTask.Logger.GetType().Name;
    }

    public string Logger { get; }
    public int Condition { get; }
    public string Repository { get; }
    public string TaskName { get; }
    public string Algorithm { get; }
    public int QuantitySelector { get; }
    public DateTime DateSelector { get; }
    public Backup.Backup Backup { get; }
    public List<string> BackupObjects { get; }
}