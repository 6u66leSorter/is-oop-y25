namespace Banks.Transactions;

public interface ITransaction
{
    Guid Id { get; }
    void Execute();
    void Undo();
}