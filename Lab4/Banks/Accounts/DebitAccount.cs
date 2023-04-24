using Banks.Entities;
using Banks.Exceptions;

namespace Banks.Accounts;

public class DebitAccount : IBankAccount
{
    private readonly decimal _interest;
    private readonly decimal _limitation;
    private decimal _monthlySavings;
    private bool _isDoubtful;

    public DebitAccount(Client client, Bank bank, Guid id)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(bank);

        if (client.Address is null || client.Passport is null)
        {
            _limitation = bank.Limitation;
            _isDoubtful = true;
        }

        _interest = bank.Interest;
        Balance = decimal.Zero;
        Client = client;
        Bank = bank;
        Id = id;
        _isDoubtful = false;
    }

    public decimal Balance { get; private set; }
    public Client Client { get; }
    public Bank Bank { get; }
    public Guid Id { get; }

    public void Withdrawal(decimal money)
    {
        if (money < decimal.Zero)
        {
            throw BankAccountException.NegativeMoney(money);
        }

        if (Balance - money < decimal.Zero)
        {
            throw BankAccountException.InsufficientFunds(money);
        }

        if (Client.Address is not null && Client.Passport is not null)
        {
            _isDoubtful = false;
        }

        if (Client.Address is null && Client.Passport is null)
        {
            _isDoubtful = true;
        }

        if (_isDoubtful)
        {
            if (money > _limitation)
            {
                throw BankAccountException.ExceededLimitation(money, _limitation);
            }
        }

        Balance -= money;
    }

    public void TopUp(decimal money)
    {
        if (money < decimal.Zero)
        {
            throw BankAccountException.NegativeMoney(money);
        }

        Balance += money;
    }

    public void CalculateChange()
    {
        _monthlySavings += Balance * (_interest / 365 * 100);
    }

    public void UpdateBalance()
    {
        Balance += _monthlySavings;
        _monthlySavings = 0;
    }
}