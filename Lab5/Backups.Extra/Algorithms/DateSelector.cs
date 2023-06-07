using Backups.Models;

namespace Backups.Extra.Algorithms;

public class DateSelector : ISelectionAlgorithm
{
    public DateSelector(DateTime date)
    {
        DateTime = date;
    }

    public DateTime DateTime { get; }

    public IReadOnlyCollection<RestorePoint> SelectRestorePoints(IReadOnlyCollection<RestorePoint> restorePoints)
    {
        return restorePoints
            .Where(restorePoint => restorePoint.DateTime < DateTime)
            .ToList()
            .AsReadOnly();
    }
}