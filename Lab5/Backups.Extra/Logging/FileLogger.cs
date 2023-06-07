using System.Text;
using Backups.Repositories;

namespace Backups.Extra.Logging;

public class FileLogger : ILogger
{
    private readonly IRepository _repository;
    public FileLogger(IRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public void Log(string message)
    {
        string filePath = @$"{_repository.GetPath()}\logs.txt";

        using Stream stream = _repository.GetFileStream(filePath);
        var writer = new StreamWriter(stream);
        writer.WriteLine(message);
    }

    public void Log(string message, DateTime dateTime)
    {
        string time = $"{dateTime.Hour}-{dateTime.Minute}-{dateTime.Second}";
        string filePath = @$"{_repository.GetPath()}\logs.txt";

        using Stream stream = _repository.GetFileStream(filePath);
        string contents = $"{time} {message}";
        var encoding = new UnicodeEncoding();
        stream.Write(encoding.GetBytes(contents));
    }
}