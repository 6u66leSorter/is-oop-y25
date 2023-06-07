using Backups.Repositories;
using Backups.Storages;

namespace Backups.Archivers;

public interface IArchiver
{
    void Archive(IReadOnlyCollection<SingleStorage> storages, IRepository repository, string archivePath);
}