using Backups.Extra.Services;

namespace Backups.Extra.Configuration;

public interface IConfiguration
{
    string Save();
    BackupExtraTask Load(string json);
}