using Shops.Entities;

namespace Shops.Exceptions;

public class OrderException : Exception
{
    private OrderException(string message)
        : base(message) { }

    public static OrderException InvalidData(Shop shop, Buyer buyer)
        => new OrderException($"No such shop or buyer");
}