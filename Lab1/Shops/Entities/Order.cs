using Shops.Models;

namespace Shops.Entities;

public class Order
{
    private readonly List<Container> _consigment;

    private Order(Buyer buyer, Shop shop, decimal sum)
    {
        _consigment = new List<Container>();
        Buyer = buyer;
        Shop = shop;
        Sum = sum;
    }

    public decimal Sum { get; }

    public Buyer Buyer { get; }
    public Shop Shop { get; }

    public IReadOnlyCollection<Container> Consigment => _consigment;

    public static bool TryCreate(Shop shop, Buyer buyer, decimal sum, out Order? order)
    {
        order = null;
        ArgumentNullException.ThrowIfNull(shop);
        ArgumentNullException.ThrowIfNull(buyer);

        order = new Order(buyer, shop, sum);
        return true;
    }

    public void AddItem(Container container)
    {
        ArgumentNullException.ThrowIfNull(container);
        _consigment.Add(container);
    }
}