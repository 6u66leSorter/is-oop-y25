namespace Shops.Exceptions;

public class ProductException : Exception
{
    private ProductException(string message)
        : base(message) { }

    public static ProductException InvalidData(string name)
        => new ProductException($"invalid name of product : {name}");

    public static ProductException NegativeQuantity(int quantity)
        => new ProductException($"Quantity of product can't be less then zero : {quantity}");
}