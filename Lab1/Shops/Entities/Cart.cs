using Shops.Models;

namespace Shops.Entities;

public class Cart
{
    private readonly List<ProductQuantity> _productQuantities;

    public Cart()
    {
        _productQuantities = new List<ProductQuantity>();
    }

    public IReadOnlyCollection<ProductQuantity> ProductQuantities => _productQuantities;

    public void AddItem(ProductQuantity productQuantity)
    {
        ArgumentNullException.ThrowIfNull(productQuantity);

        _productQuantities.Add(productQuantity);
    }
}