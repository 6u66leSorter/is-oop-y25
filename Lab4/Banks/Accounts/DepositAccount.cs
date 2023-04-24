using Banks.Entities;
using Banks.Exceptions;

namespace Banks.Accounts;

public class DepositAccount : IBankAccount
{
    private readonly decimal _interest;
    private readonly decimal _limitation;
    private decimal _monthlySavings;
    private decimal _validity;
    private bool _isDoubtful;

    public DepositAccount(Client client, Bank bank, Guid id, decimal money, decimal validity)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(bank);

        if (client.Address is null || client.Passport is null)
        {
            _limitation = bank.Limitation;
            _isDoubtful = true;
        }

        Client = client;
        Bank = bank;
        Balance = money;
        Id = id;
        _validity = validity;
        _interest = money switch
        {
            > 0 and < 50000 => bank.MinPercent,
            >= 50000 and < 100000 => bank.MidPercent,
            >= 100000 => bank.MaxPercent,
            _ => throw BankAccountException.NegativeMoney(money)
        };
        _isDoubtful = false;
    }

    public decimal Balance { get; private set; }
    public Client Client { get; }
    public Bank Bank { get; }
    public Guid Id { get; }

    public void Withdrawal(decimal money)
    {
        if (_validity > 0)
        {
            _validity -= 1;
            return;
        }

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
        if (_validity > 0)
        {
            _validity -= 1;
            return;
        }

        if (money < decimal.Zero)
        {
            throw BankAccountException.NegativeMoney(money);
        }

        Balance += money;
    }

    public void CalculateChange()
    {
        if (_validity > 0)
        {
            _validity -= 1;
            return;
        }

        _monthlySavings += Balance * (_interest / 365 * 100);
    }

    public void UpdateBalance()
    {
        Balance += _monthlySavings;
        _monthlySavings = 0;
    }
}