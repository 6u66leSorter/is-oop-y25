using System.IO.Compression;
using System.Reflection.PortableExecutable;
using Backups.BackupObjects;
using Backups.Models;
using Backups.Repositories;
using Backups.RepositoryObjects;

namespace Backups.Extra.Restore;

public class SingleRestorer : ISingleRestoration
{
    public void Restore(RestorePoint restorePoint, IRepository repository, IRepository? destinationRepository, string taskName)
    {
        var repositoryObjects = restorePoint.BackupObjects
            .Select(repository.ConvertToIRepositoryObject)
            .ToList();

        string path = Path.Combine(repository.MakeArchivePath(restorePoint.DateTime, taskName), "archive0.zip");
        var archive = new ZipArchive(repository.OpenRead(path), ZipArchiveMode.Read);

        foreach (ZipArchiveEntry entry in archive.Entries)
        {
            IRepositoryObject repositoryObject = repositoryObjects.First(x => x.GetName().Equals(entry.Name));

            if (destinationRepository is not null)
            {
                string destination = Path.Combine(destinationRepository.GetPath(), repositoryObject.GetName());

                using Stream fStream = repository.GetFileStream(destination);
                using Stream eStream = entry.Open();
                eStream.CopyTo(fStream);
            }
            else
            {
                using Stream fileStream = repository.GetFileStream(repositoryObject.GetPath());
                using Stream entryStream = entry.Open();
                entryStream.CopyTo(fileStream);
            }
        }
    }
}