using Backups.Exceptions;

namespace Backups.RepositoryObjects;

public class RepositoryFile : IRepositoryFile
{
    private readonly string _path;
    public RepositoryFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw RepositoryObjectException.InvalidPath(path);
        }

        _path = path;
    }

    public string GetName()
    {
        var names = _path.Split(Path.DirectorySeparatorChar).ToList();
        return names.Last();
    }

    public string GetPath()
    {
        return _path;
    }
}