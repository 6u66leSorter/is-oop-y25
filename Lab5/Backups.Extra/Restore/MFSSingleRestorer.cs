using System.IO.Compression;
using Backups.Models;
using Backups.Repositories;
using Backups.RepositoryObjects;
using Zio;

namespace Backups.Extra.Restore;

public class MfsSingleRestorer : ISingleRestoration
{
    public void Restore(RestorePoint restorePoint, IRepository repository, IRepository? destinationRepository, string taskName)
    {
        var repositoryObjects = restorePoint.BackupObjects
            .Select(repository.ConvertToIRepositoryObject)
            .ToList();

        string path = UPath.Combine(repository.MakeArchivePath(restorePoint.DateTime, taskName), "archive0.zip").ToString();
        var archive = new ZipArchive(repository.OpenRead(path), ZipArchiveMode.Read);

        foreach (ZipArchiveEntry entry in archive.Entries)
        {
            IRepositoryObject repositoryObject = repositoryObjects.First(x => x.GetName().Equals(entry.Name));
            if (destinationRepository is not null)
            {
                string destination = $"{destinationRepository.GetPath()}/{repositoryObject.GetName()}";

                using Stream fStream = destinationRepository.GetFileStream(destination);
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