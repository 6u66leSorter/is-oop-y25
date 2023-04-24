using System.Runtime.InteropServices;
using Shops.Entities;
using Shops.Exceptions;

namespace Shops.Models;

public class ProductQuantity
{
    public ProductQuantity(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (quantity < 0) throw ProductException.NegativeQuantity(quantity);

        Product = product;
        Quantity = quantity;
    }

    public Product Product { get; }

    public int Quantity { get; }
}