using Banks.Accounts;
using Banks.Exceptions;

namespace Banks.Transactions;

public class Withdrawal : ITransaction
{
    private readonly decimal _money;
    private readonly IBankAccount _from;
    private bool _isCompleted;

    public Withdrawal(IBankAccount from, decimal money, Guid id)
    {
        ArgumentNullException.ThrowIfNull(from);

        if (money < decimal.Zero)
        {
            throw TransactionException.NegativeMoney(money);
        }

        Id = id;
        _from = from;
        _money = money;
        _isCompleted = false;
    }

    public Guid Id { get; }

    public void Execute()
    {
        if (_isCompleted) return;

        _from.Withdrawal(_money);
        _isCompleted = true;
    }

    public void Undo()
    {
        if (!_isCompleted) return;

        _from.TopUp(_money);
        _isCompleted = false;
    }
}