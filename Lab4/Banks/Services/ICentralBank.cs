using Banks.Accounts;
using Banks.Entities;
using Banks.Models;
using Banks.Transactions;

namespace Banks.Services;

public interface ICentralBank
{
    Bank RegisterBank(decimal limitation, decimal limit, decimal interest, decimal minPercent, decimal midPercent, decimal maxPercent, string name, decimal commission);
    Client RegisterClient(string firstName, string lastName, Address? address, Passport? passport, Bank bank);
    void RewindTime(int days);
    CreditAccount CreateCreditAccount(Client client, Bank bank);
    DebitAccount CreateDebitAccount(Client client, Bank bank);
    DepositAccount CreateDepositAccount(Client client, Bank bank, decimal money, decimal validity);
    Transfer CreateTransfer(IBankAccount accountFrom, IBankAccount accountTo, decimal money);
    Replenishment CreateReplenishment(IBankAccount accountTo, decimal money);
    Withdrawal CreateWithdrawal(IBankAccount accountFrom, decimal money);
    void ExecuteTransaction(Guid id);
    void UndoTransaction(Guid id);
    Bank GetBank(Guid id);
    Client GetClient(string firstName, string lastName);
    IBankAccount GetBankAccount(Guid id);
}