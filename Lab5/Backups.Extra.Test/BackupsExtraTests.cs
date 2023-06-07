using Backups.Algorithms;
using Backups.BackupObjects;
using Backups.Entities;
using Backups.Extra.Logging;
using Backups.Extra.Restore;
using Backups.Models;
using Backups.Repositories;
using Xunit;
using Zio;
using Zio.FileSystems;

namespace Backups.Extra.Test;

public class BackupsExtraTests
{
    [Fact]
    public void RestoreBackupObjectToRepo_BackupObjectRestored()
    {
        var memoryFileSystem = new MemoryFileSystem();
        var memoryFileSystem2 = new MemoryFileSystem();
        memoryFileSystem.WriteAllText(@"/file1.txt", "qwe");
        memoryFileSystem.WriteAllText(@"/file2.txt", "asd");
        var backup = new Backup.Backup();
        var repo = new MemoryRepository(memoryFileSystem);
        var repo2 = new MemoryRepository(memoryFileSystem2);
        var bo1 = new BackupObject(@"/file1.txt", repo);
        var bo2 = new BackupObject(@"/file2.txt", repo);
        var split = new SplitStorageAlgorithm();
        const string taskName = "testTask";
        var bTask = new BackupTask(taskName, repo, split, backup);
        bTask.AddBackupObject(bo1);
        bTask.AddBackupObject(bo2);
        bTask.Execute();

        var splitR = new MfsSplitRestorer();
        splitR.Restore(bTask.RestorePoints.Last(), repo, repo2, bTask.TaskName);

        Assert.True(memoryFileSystem2.FileExists(@"/file1.txt"));
    }

    [Fact]
    public void RestoreBackupObjectToOriginalLocation_BackupObjectRestored()
    {
        var memoryFileSystem = new MemoryFileSystem();
        memoryFileSystem.WriteAllText(@"/file1.txt", "qwe");
        memoryFileSystem.WriteAllText(@"/file2.txt", "asd");

        var backup = new Backup.Backup();
        var repo = new MemoryRepository(memoryFileSystem);
        var bo1 = new BackupObject(@"/file1.txt", repo);
        var bo2 = new BackupObject(@"/file2.txt", repo);

        DateTime dateTime = DateTime.Now;
        Thread.Sleep(1000);

        var split = new SplitStorageAlgorithm();
        const string taskName = "testTask";
        var bTask = new BackupTask(taskName, repo, split, backup);
        bTask.AddBackupObject(bo1);
        bTask.AddBackupObject(bo2);
        bTask.Execute();

        var splitR = new MfsSplitRestorer();
        splitR.Restore(bTask.RestorePoints.Last(), repo, repo, bTask.TaskName);

        Assert.True(memoryFileSystem.GetLastWriteTime(@"/file1.txt") > dateTime);
        Assert.True(memoryFileSystem.FileExists(@"/file1.txt"));
    }

    [Fact]
    public void LogIntoFile_LogExists()
    {
        var memoryFileSystem = new MemoryFileSystem();
        var repo = new MemoryRepository(memoryFileSystem);
        var fileLogger = new FileLogger(repo);

        fileLogger.Log("testtestetstesttesttesttesttest");

        Assert.True(memoryFileSystem.FileExists(new UPath("/logs.txt")));
    }
}