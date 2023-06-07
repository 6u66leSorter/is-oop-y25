using Backups.Models;

namespace Backups.Extra.Algorithms;

public interface ISelectionAlgorithm
{
    IReadOnlyCollection<RestorePoint> SelectRestorePoints(IReadOnlyCollection<RestorePoint> restorePoints);
}