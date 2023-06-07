using Backups.Models;

namespace Backups.Extra.Algorithms;

public interface IMergeAlgorithm
{
    RestorePoint Merge(RestorePoint oldRestorePoint, RestorePoint newRestorePoint);
}