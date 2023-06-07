using Backups.Algorithms;
using Backups.Archivers;
using Backups.BackupObjects;
using Backups.Entities;
using Backups.Models;
using Backups.Repositories;
using Xunit;
using Zio;
using Zio.FileSystems;

namespace Backups.Test;

public class BackupsTests
{
    [Fact]
    public void Test1()
    {
        var memoryFileSystem = new MemoryFileSystem();
        memoryFileSystem.WriteAllText(@"/file1.txt", "qwe");
        memoryFileSystem.WriteAllText(@"/file2.txt", "asd");

        var backup = new Backup.Backup();
        var repo = new MemoryRepository(memoryFileSystem);
        var bo1 = new BackupObject(@"/file1.txt", repo);
        var bo2 = new BackupObject(@"/file2.txt", repo);

        var split = new SplitStorageAlgorithm();
        const string taskName = "testTask";
        var bTask = new BackupTask(taskName, repo, split, backup);
        bTask.AddBackupObject(bo1);
        bTask.AddBackupObject(bo2);
        bTask.Execute();

        Thread.Sleep(1000);

        RestorePoint restorePoint1 = backup.RestorePoints.Last();
        DateTime dateTime1 = restorePoint1.DateTime;
        string dir1 = UPath.Combine(taskName, $"{dateTime1.Date:yy-MM-dd}_{dateTime1.Hour:D2}_{dateTime1.Minute:D2}_{dateTime1.Second:D2}").ToString();

        bTask.RemoveBackupObject(bo1);
        bTask.Execute();

        RestorePoint restorePoint2 = backup.RestorePoints.Last();
        DateTime dateTime2 = restorePoint2.DateTime;
        string dir2 = UPath.Combine(taskName, $"{dateTime2.Date:yy-MM-dd}_{dateTime2.Hour:D2}_{dateTime2.Minute:D2}_{dateTime2.Second:D2}").ToString();

        Assert.True(memoryFileSystem.DirectoryExists(new UPath($"/{dir1}")));
        Assert.True(memoryFileSystem.DirectoryExists(new UPath($"/{dir2}")));

        Assert.True(memoryFileSystem.FileExists(new UPath($"/{dir1}/archive0.zip")));
        Assert.True(memoryFileSystem.FileExists(new UPath($"/{dir1}/archive1.zip")));
        Assert.True(memoryFileSystem.FileExists(new UPath($"/{dir2}/archive0.zip")));
    }
}