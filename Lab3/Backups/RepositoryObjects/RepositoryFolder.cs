using Backups.Exceptions;

namespace Backups.RepositoryObjects;

public class RepositoryFolder : IRepositoryFolder
{
    private readonly string _path;
    public RepositoryFolder(string path)
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
        return names.Last(x => x != string.Empty);
    }

    public string GetPath()
    {
        return _path;
    }
}