using Backups.Repositories;

namespace Backups.BackupObjects;

public class BackupObject : IBackupObject
{
    private readonly string _path;

    public BackupObject(string path, IRepository repository)
    {
        _path = path;
        Repository = repository;
    }

    public IRepository Repository { get; }

    public string GetPath()
    {
        return _path;
    }
}