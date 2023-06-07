using Backups.Models;
using Backups.Repositories;

namespace Backups.Extra.Restore;

public interface IRestoration
{
    void Restore(RestorePoint restorePoint, IRepository repository, IRepository? destinationRepository, string taskName);
}