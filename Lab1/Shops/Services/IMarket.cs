using Shops.Entities;

namespace Shops.Services;

public interface IMarket
{
    Shop AddShop(string name, string address);
    Buyer AddBuyer(string name, decimal balance);
    void SupplyProduct(Product product, int quantity, decimal price, Shop shop);
    void ChangePrice(Product product, Shop shop, decimal newPrice);
    Shop? FindShopWithTheBestPrice(Product product, int quantity);
    Order BuyConsignment(Shop shop, Buyer buyer, Cart cart);
}