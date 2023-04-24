namespace Banks.Exceptions;

public class TransactionException : Exception
{
    private TransactionException(string message)
        : base(message) { }

    public static TransactionException NegativeMoney(decimal money)
        => new TransactionException($"Money can't be negative : {money}");
}