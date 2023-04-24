using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class Buyer
{
    private Buyer(Guid id, string name, decimal balance)
    {
        Id = id;
        Name = name;
        Balance = balance;
    }

    public Guid Id { get; }

    public string Name { get; }

    public decimal Balance { get; private set; }

    public static bool TryCreate(Guid id, string name, decimal balance, out Buyer? buyer)
    {
        buyer = null;
        ArgumentNullException.ThrowIfNull(name);

        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        buyer = new Buyer(id, name, balance);
        return true;
    }

    public void RemoveMoney(decimal money)
    {
        if (Balance - money >= 0)
        {
            Balance -= money;
        }
        else
        {
            throw BuyerException.NotEnoughMoney();
        }
    }

    public void AddMoney(decimal money)
    {
        if (money > 0)
        {
            Balance += money;
        }
        else
        {
            throw BuyerException.NegativeSum();
        }
    }
}