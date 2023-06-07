using Backups.Models;

namespace Backups.Extra.Algorithms;

public class QuantitySelector : ISelectionAlgorithm
{
    public QuantitySelector(int number)
    {
        Quantity = number;
    }

    public int Quantity { get; }
    public IReadOnlyCollection<RestorePoint> SelectRestorePoints(IReadOnlyCollection<RestorePoint> restorePoints)
    {
        return restorePoints
            .Take(restorePoints.Count - Quantity)
            .ToList()
            .AsReadOnly();
    }
}