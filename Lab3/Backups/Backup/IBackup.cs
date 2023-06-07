using Backups.Entities;
using Backups.Models;

namespace Backups.Backup;

public interface IBackup
{
    void AddRestorePoint(RestorePoint restorePoint);
    void RemoveRestorePoint(RestorePoint restorePoint);
}