using System.Net;
using Backups.BackupObjects;
using Backups.Extra.Exceptions;
using Backups.Models;

namespace Backups.Extra.Algorithms;

public class Merger : IMergeAlgorithm
{
    public RestorePoint Merge(RestorePoint oldRestorePoint, RestorePoint newRestorePoint)
    {
        if (!(oldRestorePoint.DateTime < newRestorePoint.DateTime))
        {
            throw MergeException.InvalidParameters();
        }

        var difference = oldRestorePoint.BackupObjects
            .Except(newRestorePoint.BackupObjects)
            .ToList();

        var same = newRestorePoint.BackupObjects
            .Where(backupObject => oldRestorePoint.BackupObjects.Contains(backupObject))
            .ToList();

        var backupObjects = newRestorePoint.BackupObjects
            .Union(difference)
            .ToList();

        var result = new RestorePoint(Guid.NewGuid(), backupObjects, DateTime.Now);
        return result;
    }
}