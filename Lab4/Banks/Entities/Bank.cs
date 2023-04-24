using Banks.Accounts;
using Banks.Exceptions;

namespace Banks.Entities;

public class Bank
{
    private readonly List<Client> _clients;
    private readonly List<IBankAccount> _accounts;

    public Bank(Guid id, decimal limitation, decimal limit, decimal interest, decimal minPercent, decimal midPercent, decimal maxPercent, string name, decimal commission)
    {
        if (minPercent is >= 1 or <= 0 || midPercent is >= 1 or <= 0 || maxPercent is >= 1 or <= 0 || interest is >= 1 or <= 0)
        {
            throw BankException.InvalidPercentOrInterest();
        }

        if (limit > 0)
        {
            throw BankException.InvalidLimit(limit);
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw BankException.InvalidName(name);
        }

        if (limitation < 0)
        {
            throw BankException.InvalidLimitation(limitation);
        }

        Id = id;
        Commission = commission;
        Limitation = limitation;
        Limit = limit;
        Name = name;
        Interest = interest;
        MinPercent = minPercent;
        MidPercent = midPercent;
        MaxPercent = maxPercent;
        _clients = new List<Client>();
        _accounts = new List<IBankAccount>();
    }

    public decimal Commission { get; }
    public decimal Limitation { get; }
    public decimal Limit { get; }
    public decimal MinPercent { get; }
    public decimal MidPercent { get; }
    public decimal MaxPercent { get; }
    public decimal Interest { get; }
    public string Name { get; }
    public Guid Id { get; }

    public IReadOnlyCollection<Client> Clients => _clients.AsReadOnly();
    public IReadOnlyCollection<IBankAccount> BankAccounts => _accounts.AsReadOnly();

    public void AddClient(Client client)
    {
        ArgumentNullException.ThrowIfNull(client);

        _clients.Add(client);
    }

    public void AddBankAccount(IBankAccount account)
    {
        ArgumentNullException.ThrowIfNull(account);

        _accounts.Add(account);
    }

    public void RemoveClient(Client client)
    {
        ArgumentNullException.ThrowIfNull(client);

        if (!_clients.Remove(client))
        {
            throw BankException.NoSuchClient(client.FirstName, client.LastName);
        }
    }

    public void RemoveBankAccount(IBankAccount account)
    {
        ArgumentNullException.ThrowIfNull(account);

        if (!_accounts.Remove(account))
        {
            throw BankException.NoSuchBankAccount(account.Id);
        }
    }
}