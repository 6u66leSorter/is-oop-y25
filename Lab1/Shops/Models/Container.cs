using Shops.Entities;
using Shops.Exceptions;

namespace Shops.Models;

public class Container
{
    private Container(Product product, int quantity, decimal price)
    {
        Product = product;
        Quantity = quantity;
        Price = price;
    }

    public Product Product { get; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public static bool TryCreate(Product product, int quantity, decimal price, out Container? box)
    {
        const decimal minPrice = 1;
        const int minQuantity = 1;
        box = null;
        ArgumentNullException.ThrowIfNull(product);

        if (price < minPrice || quantity < minQuantity)
        {
            return false;
        }

        box = new Container(product, quantity, price);
        return true;
    }

    public void ChangePrice(decimal price)
    {
        const decimal minPrice = 1;

        if (price < minPrice)
        {
            throw ContainerException.InvalidPrice(price);
        }

        Price = price;
    }

    public void ReduceQuantity(int quantity)
    {
        if (Quantity - quantity < 0)
        {
            throw ContainerException.NotEnoughQuantity(quantity);
        }

        Quantity -= quantity;
    }

    public void IncreaseQuantity(int quantity)
    {
        if (quantity < 0)
        {
            throw ContainerException.NegativeQuantity(quantity);
        }

        Quantity += quantity;
    }

    public void Add(Container box)
    {
        Quantity += box.Quantity;
    }
}