using System.IO.Compression;
using Backups.BackupObjects;
using Backups.Entities;
using Backups.Repositories;
using Backups.RepositoryObjects;
using Backups.Storages;

namespace Backups.Archivers;

public class ZipArchiver : IArchiver
{
    public void Archive(IReadOnlyCollection<SingleStorage> storages, IRepository repository, string archivePath)
    {
        int i = 0;

        foreach (SingleStorage storage in storages)
        {
            const string name = "archive";

            var repositoryObjects = storage.BackupObjects
                .Select(repository.ConvertToIRepositoryObject)
                .ToList();

            string fullPath = repository.CombinePath(archivePath, $"{name}{i}.zip");
            using Stream fileStream = repository.GetFileStream(fullPath);
            using var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Update);

            foreach (IRepositoryObject repositoryObject in repositoryObjects)
            {
                if (repository.IsDirectory(repositoryObject.GetPath()))
                {
                    ZipFile.CreateFromDirectory(repositoryObject.GetPath(), fullPath);
                }

                if (!repository.IsFile(repositoryObject.GetPath())) continue;
                using Stream source = repository.OpenRead(repositoryObject.GetPath());
                ZipArchiveEntry zae = zipArchive.CreateEntry(@repositoryObject.GetName());
                source.CopyTo(zae.Open());
            }

            i++;
        }
    }
}