using Banks.Accounts;
using Banks.Entities;
using Banks.Exceptions;
using Banks.Services;
using Banks.Transactions;
using Xunit;

namespace Banks.Test;

public class CentralBankTests
{
    private readonly CentralBank _centralBank;

    public CentralBankTests()
    {
        _centralBank = new CentralBank();
    }

    [Fact]
    public void CreateAccounts_AccountsCreated()
    {
        Bank bank = _centralBank.RegisterBank(100000, -1000000, 0.05m, 0.03m, 0.035m, 0.04m, "BANK", 100);
        Client client = _centralBank.RegisterClient("Иван", "Алейников", null, null, bank);
        CreditAccount creditAccount = _centralBank.CreateCreditAccount(client, bank);
        DebitAccount debitAccount = _centralBank.CreateDebitAccount(client, bank);
        DepositAccount depositAccount = _centralBank.CreateDepositAccount(client, bank, 100000, 10);

        Assert.Contains(creditAccount, bank.BankAccounts);
        Assert.Contains(debitAccount, bank.BankAccounts);
        Assert.Contains(depositAccount, bank.BankAccounts);
    }

    [Fact]
    public void ExecuteAndUndoTransaction_MoneyLeftAndMoneyGone()
    {
        Bank bank = _centralBank.RegisterBank(100000, -1000000, 0.05m, 0.03m, 0.035m, 0.04m, "BANK", 100);
        Client client = _centralBank.RegisterClient("Иван", "Алейников", null, null, bank);
        CreditAccount creditAccount = _centralBank.CreateCreditAccount(client, bank);
        DebitAccount debitAccount = _centralBank.CreateDebitAccount(client, bank);
        Transfer transfer = _centralBank.CreateTransfer(creditAccount, debitAccount, 10000);
        creditAccount.TopUp(10000);

        _centralBank.ExecuteTransaction(transfer.Id);

        Assert.Equal(0, creditAccount.Balance);
        Assert.Equal(10000, debitAccount.Balance);

        _centralBank.UndoTransaction(transfer.Id);

        Assert.Equal(10000, creditAccount.Balance);
        Assert.Equal(0, debitAccount.Balance);
    }

    [Fact]
    public void ExecuteTransaction_ThrowBankAccountExceptionExceededLimitation()
    {
        Bank bank = _centralBank.RegisterBank(100000, -1000000, 0.0365m, 0.03m, 0.035m, 0.04m, "BANK", 100);
        Client client = _centralBank.RegisterClient("Иван", "Алейников", null, null, bank);
        CreditAccount creditAccount = _centralBank.CreateCreditAccount(client, bank);
        DebitAccount debitAccount = _centralBank.CreateDebitAccount(client, bank);
        Transfer transfer = _centralBank.CreateTransfer(debitAccount, creditAccount, 1000000);
        creditAccount.TopUp(1000000);

        Assert.Throws<BankAccountException>(() => _centralBank.ExecuteTransaction(transfer.Id));
    }

    [Fact]
    public void RewindTime_BalanceUpdated()
    {
        Bank bank = _centralBank.RegisterBank(100000, -1000000, 0.0365m, 0.03m, 0.035m, 0.04m, "BANK", 100);
        Client client = _centralBank.RegisterClient("Иван", "Алейников", null, null, bank);
        DebitAccount debitAccount = _centralBank.CreateDebitAccount(client, bank);
        debitAccount.TopUp(1000000m);

        _centralBank.RewindTime(60);

        Assert.Equal(1690000m, debitAccount.Balance);
    }
}