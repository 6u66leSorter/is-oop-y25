using System.IO.Compression;
using Backups.Algorithms;
using Backups.Archivers;
using Backups.BackupObjects;
using Backups.Entities;
using Backups.Repositories;
using Zio;
using Zio.FileSystems;

namespace Backups;

public static class Program
{
    public static void Main()
    {
        var backup = new Backup.Backup();
        var archiver = new ZipArchiver();
        var repo = new FileSystemRepository(@"C:\Users\Aleinikov Ivan\Desktop\workspace\asd");
        var bo1 = new BackupObject(@"C:\Users\Aleinikov Ivan\Desktop\workspace\asd\file.txt", repo);
        var bo2 = new BackupObject(@"C:\Users\Aleinikov Ivan\Desktop\workspace\asd\file2.txt", repo);
        var split = new SplitStorageAlgorithm();
        var single = new SingleStorageAlgorithm();
        var bTask = new BackupTask("testTask", repo, split, backup);
        bTask.AddBackupObject(bo1);
        bTask.AddBackupObject(bo2);
        bTask.Execute();
    }
}