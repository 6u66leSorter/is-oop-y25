using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class Shop
{
    private const int MinQuantity = 1;
    private readonly List<Container> _stock;
    private decimal _balance;

    private Shop(Guid id, string name, string address)
    {
        Id = id;
        Name = name;
        Address = address;
        _stock = new List<Container>();
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Address { get; }

    public decimal Balance
    {
        get => _balance;
        set => _balance = value;
    }

    public IReadOnlyCollection<Container> Stock => _stock;

    public static bool TryCreate(Guid id, string name, string address, out Shop? shop)
    {
        shop = null;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address))
        {
            return false;
        }

        shop = new Shop(id, name, address);
        return true;
    }

    public void AddContainer(Container container)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        Container? box = FindContainer(container.Product);
        if (box is not null)
        {
            box.Add(container);
            return;
        }

        _stock.Add(container);
    }

    public void ChangeProductPrice(Product product, decimal newPrice)
    {
        Container? container = FindContainer(product);
        if (container is null)
        {
            throw ShopException.NoSuchProductInStock(product);
        }

        container.ChangePrice(newPrice);
    }

    public Container? FindContainerWithEnoughQuantity(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (quantity < MinQuantity)
        {
            throw ShopException.InavalidQuantity(quantity);
        }

        return _stock.FirstOrDefault(c => c.Product.Equals(product) && c.Quantity >= quantity);
    }

    public void AddMoney(decimal sum)
    {
        _balance += sum;
    }

    private Container? FindContainer(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        return _stock.FindLast(c => c.Product.Name == product.Name);
    }
}