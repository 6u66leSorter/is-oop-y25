namespace Banks.Exceptions;

public class BankAccountException : Exception
{
    private BankAccountException(string message)
        : base(message) { }

    public static BankAccountException NegativeMoney(decimal money)
        => new BankAccountException($"Money can't be negative : {money}");
    public static BankAccountException ExceededLimit(decimal money)
        => new BankAccountException($"Too much money to withdrawal : {money}");
    public static BankAccountException InsufficientFunds(decimal money)
        => new BankAccountException($"Balance can't be negative. Can't withdrawal {money}");
    public static BankAccountException ExceededLimitation(decimal money, decimal limitation)
        => new BankAccountException($"Can't withdrawal {money} because of limitation ({limitation})");
}