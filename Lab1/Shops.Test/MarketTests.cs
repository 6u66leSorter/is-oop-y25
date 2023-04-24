using Shops.Entities;
using Shops.Models;
using Shops.Services;
using Xunit;

namespace Shops.Test;

public class MarketTests
{
    private readonly Market _market;

    public MarketTests()
    {
        _market = new Market();
    }

    [Fact]
    public void AddProductToShop_ShopContainsProduct()
    {
        Shop shop1 = _market.AddShop("shop1", "address1");

        Product product1 = _market.AddProduct("product1");

        _market.SupplyProduct(product1, 30, 3000, shop1);

        Assert.Same(product1, shop1.Stock.Select(c => c.Product).First(c => c == product1));
    }

    [Fact]
    public void SetAndChangePrice_PriceSetAndPriceChange()
    {
        const decimal newPrice = 238.5M;
        Shop shop1 = _market.AddShop("shop1", "address1");

        Product product1 = _market.AddProduct("product1");
        Product product2 = _market.AddProduct("product2");

        _market.SupplyProduct(product1, 30, 3000, shop1);
        _market.SupplyProduct(product2, 30, 3000, shop1);

        shop1.ChangeProductPrice(product2, newPrice);

        Assert.Equal(3000, shop1.Stock.Where(c => c.Product == product1).Select(c => c.Price).First());
        Assert.Equal(newPrice, shop1.Stock.Where(c => c.Product == product2).Select(c => c.Price).First());
    }

    [Fact]
    public void FindTheCheapestShop_ShopFound()
    {
        Shop shop1 = _market.AddShop("shop1", "address1");
        Shop shop2 = _market.AddShop("shop2", "address2");

        Product product = _market.AddProduct("product");

        _market.SupplyProduct(product, 30, 9000, shop1);
        _market.SupplyProduct(product, 30, 3000, shop2);

        Shop? shop = _market.FindShopWithTheBestPrice(product, 30);

        Assert.Equal(shop2, shop);
    }

    [Fact]
    public void FindTheCheapestShop_ShopNotFound()
    {
        Shop shop1 = _market.AddShop("shop1", "address1");
        Shop shop2 = _market.AddShop("shop2", "address2");

        Product product = _market.AddProduct("product");
        Product fakeProduct = _market.AddProduct("fakeProduct");

        _market.SupplyProduct(product, 30, 9000, shop1);
        _market.SupplyProduct(product, 30, 3000, shop2);

        Shop? testShop1 = _market.FindShopWithTheBestPrice(product, 300);
        Shop? testShop2 = _market.FindShopWithTheBestPrice(fakeProduct, 300);

        Assert.Null(testShop1);
        Assert.Null(testShop2);
    }

    [Fact]
    public void BuyConsignment_MoneyAndProductsChange()
    {
        Shop shop = _market.AddShop("shop1", "address1");
        Buyer buyer = _market.AddBuyer("buyer", 200000);

        Product product1 = _market.AddProduct("product1");
        Product product2 = _market.AddProduct("product2");

        var productQuantity1 = new ProductQuantity(product1, 30);
        var productQuantity2 = new ProductQuantity(product2, 30);

        var cart = new Cart();

        cart.AddItem(productQuantity1);
        cart.AddItem(productQuantity2);

        _market.SupplyProduct(product1, 30, 3000, shop);
        _market.SupplyProduct(product2, 30, 3000, shop);

        Order order = _market.BuyConsignment(shop, buyer, cart);

        Assert.Equal(20000, buyer.Balance);
        Assert.Equal(180000, shop.Balance);
        Assert.Equal(180000, order.Sum);
    }
}