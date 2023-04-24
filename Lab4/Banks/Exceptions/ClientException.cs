namespace Banks.Exceptions;

public class ClientException : Exception
{
    private ClientException(string message)
        : base(message) { }

    public static ClientException InvalidName(string name)
        => new ClientException($"Check name : {name}");
}