using Backups.Repositories;

namespace Backups.Extra.Logging;

public interface ILogger
{
    void Log(string message);
    void Log(string message, DateTime dateTime);
}