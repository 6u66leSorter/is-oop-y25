using Banks.Accounts;
using Banks.Exceptions;

namespace Banks.Transactions;

public class Transfer : ITransaction
{
    private readonly decimal _money;
    private readonly IBankAccount _from;
    private readonly IBankAccount _to;
    private bool _isCompleted;

    public Transfer(IBankAccount from, IBankAccount to, decimal money, Guid id)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        if (money < decimal.Zero)
        {
            throw TransactionException.NegativeMoney(money);
        }

        Id = id;
        _from = from;
        _to = to;
        _money = money;
        _isCompleted = false;
    }

    public Guid Id { get; }

    public void Execute()
    {
        if (_isCompleted) return;

        _from.Withdrawal(_money);
        _to.TopUp(_money);
        _isCompleted = true;
    }

    public void Undo()
    {
        if (!_isCompleted) return;

        _to.Withdrawal(_money);
        _from.TopUp(_money);
        _isCompleted = false;
    }
}