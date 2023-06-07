namespace Backups.Exceptions;

public class BackupException : Exception
{
    private BackupException(string message)
        : base(message) { }
    public static BackupException NoSuchBackupRestorePoint(Guid id)
        => new BackupException($"Restore point with id : {id} does not exist");
}