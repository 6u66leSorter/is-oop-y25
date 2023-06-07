namespace Backups.Exceptions;

public class RepositoryException : Exception
{
    private RepositoryException(string message)
        : base(message) { }
    public static RepositoryException InvalidDirectoryPath(string path)
        => new RepositoryException($"This is not directory : {path}");
    public static RepositoryException InvalidPath(string path)
        => new RepositoryException($"Can't find path : {path} ");
}