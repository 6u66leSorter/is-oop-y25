namespace Banks.Exceptions;

public class CentralBankException : Exception
{
    private CentralBankException(string message)
        : base(message) { }

    public static CentralBankException NoSuchTransaction(Guid id)
        => new CentralBankException($"Can't find transaction with id : {id}");
    public static CentralBankException NoSuchBank(Guid id)
        => new CentralBankException($"Can't find bank with id : {id}");
    public static CentralBankException NoSuchBankAccount(Guid id)
        => new CentralBankException($"Can't find account with id : {id}");
    public static CentralBankException NoSuchClient(string firstname, string lastName)
        => new CentralBankException($"Can't find client {firstname} {lastName}");
}