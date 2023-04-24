namespace Banks.Exceptions;

public class BankException : Exception
{
    private BankException(string message)
        : base(message) { }

    public static BankException InvalidPercentOrInterest()
        => new BankException($"Check bank's arguments");
    public static BankException InvalidLimit(decimal limit)
        => new BankException($"limit can't be positive : {limit}");
    public static BankException InvalidName(string name)
        => new BankException($"Name can't be empty : {name}");
    public static BankException InvalidLimitation(decimal limitation)
        => new BankException($"limitation can't be negative : {limitation}");
    public static BankException NoSuchClient(string firsName, string lastName)
        => new BankException($"{firsName} {lastName} does not exist");
    public static BankException NoSuchBankAccount(Guid id)
        => new BankException($"Account with id ({id}) does not exist");
}