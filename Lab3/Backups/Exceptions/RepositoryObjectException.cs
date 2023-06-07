namespace Backups.Exceptions;

public class RepositoryObjectException : Exception
{
    private RepositoryObjectException(string message)
        : base(message) { }
    public static RepositoryObjectException InvalidDirectoryPath(string path)
        => new RepositoryObjectException($"This is not directory : {path}");
    public static RepositoryObjectException InvalidPath(string path)
        => new RepositoryObjectException($"Can't find path : {path} ");
}