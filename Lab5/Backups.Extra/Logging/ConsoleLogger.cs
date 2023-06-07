namespace Backups.Extra.Logging;

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }

    public void Log(string message, DateTime dateTime)
    {
        string time = $"{dateTime.Hour}-{dateTime.Minute}-{dateTime.Second}";
        Console.WriteLine($"{time} {message}");
    }
}