using Banks.Entities;
using Banks.Exceptions;

namespace Banks.Accounts;

public class CreditAccount : IBankAccount
{
    private readonly decimal _limit;
    private readonly decimal _limitation;
    private readonly decimal _comission;
    private decimal _monthlyComission;
    private bool _isDoubtful;

    public CreditAccount(Client client, Bank bank, Guid id)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(bank);

        if (client.Address is null || client.Passport is null)
        {
            _limitation = bank.Limitation;
            _isDoubtful = true;
        }

        _limit = bank.Limit;
        Client = client;
        Bank = bank;
        Balance = decimal.Zero;
        Id = id;
        _comission = bank.Commission;
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

        if (Balance - money < _limit)
        {
            throw BankAccountException.ExceededLimit(money);
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
        if (Balance > decimal.Zero) return;

        _monthlyComission += _comission;
    }

    public void UpdateBalance()
    {
        Balance -= _monthlyComission;
        _monthlyComission = 0;
    }
}