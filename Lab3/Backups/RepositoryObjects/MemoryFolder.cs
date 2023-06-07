using Backups.Exceptions;
using Zio;

namespace Backups.RepositoryObjects;

public class MemoryFolder : IMemoryFolder
{
    private readonly string _path;

    public MemoryFolder(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw RepositoryObjectException.InvalidPath(path);
        }

        _path = path;
    }

    public string GetName()
    {
        var names = _path.Split(UPath.DirectorySeparator).ToList();
        return names.Last(x => x != string.Empty);
    }

    public string GetPath()
    {
        return _path;
    }
}