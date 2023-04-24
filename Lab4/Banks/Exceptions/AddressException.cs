namespace Banks.Exceptions;

public class AddressException : Exception
{
    private AddressException(string message)
        : base(message) { }

    public static AddressException InvalidCityName(string name)
        => new AddressException($"Check name : {name}");
    public static AddressException InvalidStreetName(string name)
        => new AddressException($"Check name : {name}");
    public static AddressException NegativeNumber()
        => new AddressException($"Number can't be negative");
}