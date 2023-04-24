namespace Shops.Entities;

public class Product
{
    private Product(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; }
    public string Name { get; }

    public static bool TryCreate(Guid id, string name, out Product? product)
    {
        product = null;
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        product = new Product(id, name);
        return true;
    }
}