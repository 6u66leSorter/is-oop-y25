using Banks.Accounts;
using Banks.Exceptions;

namespace Banks.Transactions;

public class Replenishment : ITransaction
{
    private readonly decimal _money;
    private readonly IBankAccount _to;
    private bool _isCompleted;

    public Replenishment(IBankAccount to, decimal money, Guid id)
    {
        ArgumentNullException.ThrowIfNull(to);

        if (money < decimal.Zero)
        {
            throw TransactionException.NegativeMoney(money);
        }

        Id = id;
        _to = to;
        _money = money;
        _isCompleted = false;
    }

    public Guid Id { get; }

    public void Execute()
    {
        if (_isCompleted) return;

        _to.TopUp(_money);
        _isCompleted = true;
    }

    public void Undo()
    {
        if (!_isCompleted) return;

        _to.Withdrawal(_money);
        _isCompleted = false;
    }
}