using Backups.BackupObjects;
using Backups.Entities;
using Backups.RepositoryObjects;

namespace Backups.Repositories;

public interface IRepository
{
    Stream GetFileStream(string archivePath);
    IRepositoryObject ConvertToIRepositoryObject(IBackupObject backupObject);
    string MakeArchivePath(DateTime dateTime, string backupTask);
    string CombinePath(string path, string name);
    bool IsDirectory(string path);
    bool IsFile(string path);
    Stream OpenRead(string path);
    string GetPath();
}