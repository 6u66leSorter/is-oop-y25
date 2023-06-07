namespace Backups.Extra.Exceptions;

public class MergeException : Exception
{
    private MergeException(string message)
        : base(message) { }
    public static MergeException InvalidParameters()
        => new MergeException($"Restore Points must be in another order");
}