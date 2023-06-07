using System.IO.Compression;
using System.Text.Json;
using Backups.Algorithms;
using Backups.Archivers;
using Backups.BackupObjects;
using Backups.Entities;
using Backups.Extra.Algorithms;
using Backups.Extra.Configuration;
using Backups.Extra.Logging;
using Backups.Extra.Models;
using Backups.Extra.Restore;
using Backups.Extra.Services;
using Backups.Models;
using Backups.Repositories;
using Zio;
using Zio.FileSystems;

namespace Backups.Extra;

public class Test
{
    public Test(int a, string b, IRepository repo)
    {
        Repository = repo;
        A = a;
        B = b ?? throw new ArgumentNullException(nameof(b));
    }

    public IRepository Repository { get; }

    public int A { get; }

    public string B { get; }
}

public static class Program
{
    public static void Main()
    {
        var backup = new Backup.Backup();
        var split = new SplitStorageAlgorithm();
        var qSelector = new QuantitySelector(5);
        var dSelector = new DateSelector(new DateTime(2003, 07, 07));
        var repo = new FileSystemRepository(@"C:\Users\Aleinikov Ivan\Desktop\workspace\asd");
        var repo2 = new FileSystemRepository(@"C:\Users\Aleinikov Ivan\Desktop\workspace\Restore");
        var bo1 = new BackupObject(@"C:\Users\Aleinikov Ivan\Desktop\workspace\asd\file.txt", repo);
        var bo2 = new BackupObject(@"C:\Users\Aleinikov Ivan\Desktop\workspace\asd\file2.txt", repo);
        var logger = new FileLogger(repo);
        var backupExtraTask = new BackupExtraTask(logger, "EXTRA", repo, split, backup, qSelector, dSelector, SelectionAlgorithmCondition.Union);
        backupExtraTask.AddBackupObject(bo1);
        backupExtraTask.AddBackupObject(bo2);
        backupExtraTask.Execute();
        var splitR = new SplitRestoration();
        splitR.Restore(backupExtraTask.Backup.RestorePoints.Last(), repo, repo2, backupExtraTask.TaskName);
        var config = new Configurator(backupExtraTask, repo);
        config.Save();
    }
}