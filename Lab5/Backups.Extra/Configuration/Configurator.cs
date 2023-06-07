using System.Text;
using System.Text.Json;
using Backups.Algorithms;
using Backups.BackupObjects;
using Backups.Extra.Algorithms;
using Backups.Extra.Logging;
using Backups.Extra.Models;
using Backups.Extra.Services;
using Backups.Repositories;

namespace Backups.Extra.Configuration;

public class Configurator
{
    public Configurator(BackupExtraTask backupExtraTask, IRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        TaskPrototype = new TaskPrototype(backupExtraTask);
        Repository = repository;
    }

    public IRepository Repository { get; }
    public TaskPrototype TaskPrototype { get; }

    public string Save()
    {
        string path = $"{Repository.GetPath()}\\{TaskPrototype.TaskName}.txt";
        string json = JsonSerializer.Serialize(TaskPrototype);
        using Stream stream = Repository.GetFileStream(path);
        var encoding = new UnicodeEncoding();
        stream.Write(encoding.GetBytes(json));
        return json;
    }

    public BackupExtraTask Load(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new ArgumentException("Empty json", nameof(json));
        }

        TaskPrototype? taskPrototype = JsonSerializer.Deserialize<TaskPrototype>(json);
        var backupObjects = taskPrototype!.BackupObjects
            .Select(path => new BackupObject(path, new FileSystemRepository(taskPrototype.Repository)))
            .ToList();

        var repository = new FileSystemRepository(taskPrototype.Repository);

        ILogger logger;

        if (taskPrototype.Logger.Equals("ConsoleLogger"))
        {
            logger = new ConsoleLogger();
        }
        else
        {
            logger = new FileLogger(repository);
        }

        IStorageAlgorithm algorithm;
        if (taskPrototype.Algorithm == "SplitStorageAlgorithm")
        {
            algorithm = new SplitStorageAlgorithm();
        }
        else
        {
            algorithm = new SingleStorageAlgorithm();
        }

        var qSelector = new QuantitySelector(taskPrototype.QuantitySelector);
        var dSelector = new DateSelector(taskPrototype.DateSelector);
        var condition = (SelectionAlgorithmCondition)taskPrototype.Condition;

        var backupExtraTask = new BackupExtraTask(logger, taskPrototype.TaskName, repository, algorithm, taskPrototype.Backup, qSelector, dSelector, condition);

        foreach (BackupObject backupObject in backupObjects)
        {
            backupExtraTask.AddBackupObject(backupObject);
        }

        return backupExtraTask;
    }
}