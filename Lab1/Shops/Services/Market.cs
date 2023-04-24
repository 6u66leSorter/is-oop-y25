using Shops.Entities;
using Shops.Exceptions;
using Shops.Models;

namespace Shops.Services;

public class Market : IMarket
{
    private readonly List<Shop> _shops;
    private readonly List<Buyer> _buyers;
    private readonly List<Product> _products;

    public Market()
    {
        _shops = new List<Shop>();
        _buyers = new List<Buyer>();
        _products = new List<Product>();
    }

    public Shop AddShop(string name, string address)
    {
        if (!Shop.TryCreate(Guid.NewGuid(), name, address, out Shop? shop))
        {
            throw ShopException.InavalidData(name, address);
        }

        _shops.Add(shop!);
        return shop!;
    }

    public Buyer AddBuyer(string name, decimal balance)
    {
        if (!Buyer.TryCreate(Guid.NewGuid(), name, balance, out Buyer? buyer))
        {
            throw BuyerException.InvalidData(name, balance);
        }

        _buyers.Add(buyer!);
        return buyer!;
    }

    public Product AddProduct(string name)
    {
        if (!Product.TryCreate(Guid.NewGuid(), name, out Product? product))
        {
            throw ProductException.InvalidData(name);
        }

        _products.Add(product!);
        return product!;
    }

    public void SupplyProduct(Product product, int quantity, decimal price, Shop shop)
    {
        if (!Container.TryCreate(product, quantity, price, out Container? box))
        {
            throw ContainerException.InvalidData();
        }

        shop.AddContainer(box!);
    }

    public void ChangePrice(Product product, Shop shop, decimal newPrice)
    {
        shop.ChangeProductPrice(product, newPrice);
    }

    public Shop? FindShopWithTheBestPrice(Product product, int quantity)
    {
        return _shops
            .Where(s => s.FindContainerWithEnoughQuantity(product, quantity) is not null)
            .MinBy(s => s.FindContainerWithEnoughQuantity(product, quantity) !.Price);
    }

    public Order BuyConsignment(Shop shop, Buyer buyer, Cart cart)
    {
        decimal sum = 0;
        var consignment = new List<Container>();

        foreach (ProductQuantity elem in cart.ProductQuantities)
        {
            Container? container = shop.FindContainerWithEnoughQuantity(elem.Product, elem.Quantity);
            _ = container ?? throw ShopException.NoProductInSuchQuantity(elem.Product, elem.Quantity);

            sum += container.Price * elem.Quantity;
            consignment.Add(container);
        }

        if (buyer.Balance < sum)
        {
            throw BuyerException.NotEnoughMoney();
        }

        if (!Order.TryCreate(shop, buyer, sum, out Order? order))
        {
            throw OrderException.InvalidData(shop, buyer);
        }

        foreach ((Container StockItem, ProductQuantity CartItem) itemPair in consignment.Zip(cart.ProductQuantities))
        {
            itemPair.StockItem.ReduceQuantity(itemPair.CartItem.Quantity);

            if (!Container.TryCreate(itemPair.CartItem.Product, itemPair.CartItem.Quantity, itemPair.StockItem.Price, out Container? container))
            {
                throw ContainerException.InvalidData();
            }

            order!.AddItem(container!);
            buyer.RemoveMoney(container!.Price * container.Quantity);
            shop.AddMoney(container.Price * container.Quantity);
        }

        return order!;
    }
}