using Backups.BackupObjects;
using Backups.Entities;
using Backups.Exceptions;
using Backups.RepositoryObjects;
using Backups.Storages;
using Zio;
using Zio.FileSystems;

namespace Backups.Repositories;

public class MemoryRepository : IRepository
{
    private readonly MemoryFileSystem _mfs;

    public MemoryRepository(MemoryFileSystem mfs)
    {
        _mfs = mfs;
    }

    public Stream GetFileStream(string archivePath)
    {
        return _mfs.OpenFile($"{UPath.DirectorySeparator}{new UPath(archivePath)}", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
    }

    public bool IsDirectory(string path)
    {
        return _mfs.DirectoryExists(new UPath(path));
    }

    public bool IsFile(string path)
    {
        return _mfs.FileExists(new UPath(path));
    }

    public IRepositoryObject ConvertToIRepositoryObject(IBackupObject backupObject)
    {
        string path = backupObject.GetPath();

        if (_mfs.DirectoryExists(path))
        {
            var repositoryFolder = new MemoryFolder(path);
            return repositoryFolder;
        }

        if (!_mfs.FileExists(path)) throw RepositoryException.InvalidPath(path);
        var repositoryFile = new MemoryFile(path);
        return repositoryFile;
    }

    public string MakeArchivePath(DateTime dateTime, string backupTask)
    {
        _mfs.CreateDirectory($"{UPath.DirectorySeparator}{new UPath(backupTask)}");
        _mfs.CreateDirectory(UPath.Combine($"{UPath.DirectorySeparator}{backupTask}", $"{dateTime.Date:yy-MM-dd}_{dateTime.Hour:D2}_{dateTime.Minute:D2}_{dateTime.Second:D2}"));

        return UPath.Combine(backupTask, $"{dateTime.Date:yy-MM-dd}_{dateTime.Hour:D2}_{dateTime.Minute:D2}_{dateTime.Second:D2}").ToString();
    }

    public string CombinePath(string path, string name)
    {
        return UPath.Combine(new UPath(path), new UPath(name)).ToString();
    }

    public Stream OpenRead(string path)
    {
        return _mfs.OpenFile($"{UPath.DirectorySeparator}{path}", FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    public string GetPath()
    {
        return string.Empty;
    }
}