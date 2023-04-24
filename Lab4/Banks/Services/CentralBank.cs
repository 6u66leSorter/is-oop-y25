using Banks.Accounts;
using Banks.Entities;
using Banks.Exceptions;
using Banks.Models;
using Banks.Transactions;

namespace Banks.Services;

public class CentralBank : ICentralBank
{
    private readonly List<Bank> _banks;
    private readonly List<Client> _clients;
    private readonly List<ITransaction> _transactions;
    private int _days;

    public CentralBank()
    {
        _banks = new List<Bank>();
        _clients = new List<Client>();
        _transactions = new List<ITransaction>();
        _days = 0;
    }

    public Bank RegisterBank(decimal limitation, decimal limit, decimal interest, decimal minPercent, decimal midPercent, decimal maxPercent, string name, decimal commission)
    {
        var bank = new Bank(Guid.NewGuid(), limitation, limit, interest, minPercent, midPercent, maxPercent, name, commission);
        _banks.Add(bank);
        return bank;
    }

    public Client RegisterClient(string firstName, string lastName, Address? address, Passport? passport, Bank bank)
    {
        Client client = new ClientBuilder()
            .SetFirstName(firstName)
            .SetLastName(lastName)
            .SetAddress(address)
            .SetPassport(passport)
            .Build();

        _clients.Add(client);
        bank.AddClient(client);

        return client;
    }

    public void RewindTime(int days)
    {
        for (int i = _days + 1; i <= days + _days; ++i)
        {
            foreach (IBankAccount account in _banks.SelectMany(bank => bank.BankAccounts))
            {
                account.CalculateChange();
                if (i % 30 == 0)
                {
                    account.UpdateBalance();
                }
            }
        }
    }

    public CreditAccount CreateCreditAccount(Client client, Bank bank)
    {
        var account = new CreditAccount(client, bank, Guid.NewGuid());
        bank.AddBankAccount(account);

        return account;
    }

    public DebitAccount CreateDebitAccount(Client client, Bank bank)
    {
        var account = new DebitAccount(client, bank, Guid.NewGuid());
        bank.AddBankAccount(account);

        return account;
    }

    public DepositAccount CreateDepositAccount(Client client, Bank bank, decimal money, decimal validity)
    {
        var account = new DepositAccount(client, bank, Guid.NewGuid(), money, validity);
        bank.AddBankAccount(account);

        return account;
    }

    public Transfer CreateTransfer(IBankAccount accountFrom, IBankAccount accountTo, decimal money)
    {
        var transfer = new Transfer(accountFrom, accountTo, money, Guid.NewGuid());
        _transactions.Add(transfer);

        return transfer;
    }

    public Replenishment CreateReplenishment(IBankAccount accountTo, decimal money)
    {
        var replenishment = new Replenishment(accountTo, money, Guid.NewGuid());
        _transactions.Add(replenishment);

        return replenishment;
    }

    public Withdrawal CreateWithdrawal(IBankAccount accountFrom, decimal money)
    {
        var withdrawal = new Withdrawal(accountFrom, money, Guid.NewGuid());
        _transactions.Add(withdrawal);

        return withdrawal;
    }

    public void ExecuteTransaction(Guid id)
    {
        ITransaction? transaction = _transactions.FirstOrDefault(transaction => transaction.Id.Equals(id));

        if (transaction is null)
        {
            throw CentralBankException.NoSuchTransaction(id);
        }

        transaction.Execute();
    }

    public void UndoTransaction(Guid id)
    {
        ITransaction? transaction = _transactions.FirstOrDefault(transaction => transaction.Id.Equals(id));

        if (transaction is null)
        {
            throw CentralBankException.NoSuchTransaction(id);
        }

        transaction.Undo();
    }

    public Bank GetBank(Guid id)
    {
        Bank? bank = _banks.FirstOrDefault(bank => bank.Id.Equals(id));

        if (bank is null) throw CentralBankException.NoSuchBank(id);

        return bank;
    }

    public Client GetClient(string firstName, string lastName)
    {
        Client? client = _clients.FirstOrDefault(client =>
            client.FirstName.Equals(firstName) && client.LastName.Equals(lastName));

        if (client is null) throw CentralBankException.NoSuchClient(firstName, lastName);

        return client;
    }

    public IBankAccount GetBankAccount(Guid id)
    {
        IBankAccount? bankAccount = _banks
            .SelectMany(b => b.BankAccounts)
            .FirstOrDefault(a => a.Id.Equals(id));

        if (bankAccount is null)
        {
            throw CentralBankException.NoSuchBankAccount(id);
        }

        return bankAccount;
    }
}
