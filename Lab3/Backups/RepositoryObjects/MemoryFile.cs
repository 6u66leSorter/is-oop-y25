using Backups.Exceptions;
using Zio;

namespace Backups.RepositoryObjects;

public class MemoryFile : IMemoryFile
{
    private readonly string _path;

    public MemoryFile(string path)
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
        return names.Last();
    }

    public string GetPath()
    {
        return _path;
    }
}