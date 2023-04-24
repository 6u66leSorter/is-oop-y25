namespace Shops.Exceptions;

public class BuyerException : Exception
{
    private BuyerException(string message)
        : base(message) { }
    public static BuyerException NotEnoughMoney()
        => new BuyerException($"not enough money to buy product");
    public static BuyerException InvalidData(string name, decimal balance)
        => new BuyerException($"invalid name ({name}) or balance ({balance}), can't create new buyer");
    public static BuyerException NegativeSum()
        => new BuyerException($"sum must be greater then zero");
}