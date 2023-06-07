namespace Backups.Exceptions;

public class BackupTaskException : Exception
{
    private BackupTaskException(string message)
        : base(message) { }
    public static BackupTaskException NoSuchBackupObject()
        => new BackupTaskException($"Object does not exist");
}