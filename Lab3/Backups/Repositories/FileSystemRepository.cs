using Backups.Archivers;
using Backups.BackupObjects;
using Backups.Exceptions;
using Backups.RepositoryObjects;

namespace Backups.Repositories;

public class FileSystemRepository : IRepository
{
    private readonly string _repositoryPath;

    public FileSystemRepository(string path)
    {
        if (!Directory.Exists(path))
        {
            throw RepositoryException.InvalidDirectoryPath(path);
        }

        _repositoryPath = path;
    }

    public string GetPath()
    {
        return _repositoryPath;
    }

    public IRepositoryObject ConvertToIRepositoryObject(IBackupObject backupObject)
    {
        string path = backupObject.GetPath();

        if (Directory.Exists(path))
        {
            var repositoryFolder = new RepositoryFolder(path);
            return repositoryFolder;
        }

        if (!File.Exists(path)) throw RepositoryException.InvalidPath(path);
        var repositoryFile = new RepositoryFile(path);
        return repositoryFile;
    }

    public Stream GetFileStream(string archivePath)
    {
        return File.Open($"{archivePath}", FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    public string MakeArchivePath(DateTime dateTime, string backupTask)
    {
        Directory.CreateDirectory(Path.Combine(_repositoryPath, backupTask));
        Directory.CreateDirectory(Path.Combine(_repositoryPath, backupTask, $"{dateTime.Date:yy-MM-dd}_{dateTime.Hour:D2}_{dateTime.Minute:D2}_{dateTime.Second:D2}"));

        return Path.Combine(_repositoryPath, backupTask, $"{dateTime.Date:yy-MM-dd}_{dateTime.Hour:D2}_{dateTime.Minute:D2}_{dateTime.Second:D2}");
    }

    public string CombinePath(string path, string name)
    {
        return Path.Combine(path, name);
    }

    public bool IsDirectory(string path)
    {
        return Directory.Exists(path);
    }

    public bool IsFile(string path)
    {
        return File.Exists(path);
    }

    public Stream OpenRead(string path)
    {
        return File.OpenRead(path);
    }
}