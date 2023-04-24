namespace Shops.Exceptions;

public class ContainerException : Exception
{
    private ContainerException(string message)
        : base(message) { }

    public static ContainerException InvalidData()
        => new ContainerException($"invalid data : product or quantity or price");

    public static ContainerException InvalidPrice(decimal price)
        => new ContainerException($"Price can't be less then 1 : {price}");

    public static ContainerException NotEnoughQuantity(int quantity)
        => new ContainerException($"There is too big quantity : {quantity}");

    public static ContainerException NegativeQuantity(int quantity)
        => new ContainerException($"Quantity can't be negative : {quantity}");
}