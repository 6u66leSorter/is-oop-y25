using Shops.Entities;

namespace Shops.Exceptions;

public class ShopException : Exception
{
    private ShopException(string message)
        : base(message) { }

    public static ShopException InavalidData(string name, string address)
        => new ShopException($"Invalid name ({name}) or ({address}), can't create new shop");

    public static ShopException NoSuchProductInStock(Product product)
        => new ShopException($"This container ({product}) was not found");

    public static ShopException NoProductInSuchQuantity(Product product, int quantity)
        => new ShopException($"There is no product : {product} in such quantity {quantity}");

    public static ShopException InavalidQuantity(int quantity)
        => new ShopException($"Quantity can't be less then 1: {quantity}");
}