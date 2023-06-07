using Backups.Entities;
using Backups.Exceptions;
using Backups.Models;

namespace Backups.Backup;

public class Backup : IBackup
{
    private readonly List<RestorePoint> _restorePoints;

    public Backup()
    {
        _restorePoints = new List<RestorePoint>();
    }

    public IReadOnlyCollection<RestorePoint> RestorePoints => _restorePoints.AsReadOnly();

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        ArgumentNullException.ThrowIfNull(restorePoint);

        _restorePoints.Add(restorePoint);
    }

    public void RemoveRestorePoint(RestorePoint restorePoint)
    {
        ArgumentNullException.ThrowIfNull(restorePoint);

        if (!_restorePoints.Contains(restorePoint))
        {
            throw BackupException.NoSuchBackupRestorePoint(restorePoint.Id);
        }

        _restorePoints.Remove(restorePoint);
    }
}