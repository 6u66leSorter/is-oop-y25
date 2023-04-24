using Banks.Entities;

namespace Banks.Accounts;

public interface IBankAccount
{
    decimal Balance { get; }
    Client Client { get; }
    Bank Bank { get; }
    Guid Id { get; }

    void Withdrawal(decimal money);
    void TopUp(decimal money);
    void CalculateChange();
    void UpdateBalance();
}