using System.Globalization;
using Banks.Accounts;
using Banks.Entities;
using Banks.Models;
using Banks.Services;
using Banks.Transactions;

namespace Banks.Console;

public static class Program
{
    private static readonly CentralBank CentralBank = new CentralBank();

    public static void Main()
    {
        DrawMenu();
        while (true)
        {
            string option = System.Console.ReadLine() ?? string.Empty;
            switch (option)
            {
                case "1":
                {
                    System.Console.WriteLine(
                        "Input : limitation, limit, interest, minPercent, midPercent, maxPercent, name, commission");
                    string arguments = System.Console.ReadLine() ?? string.Empty;
                    string[] args = arguments.Split(",");
                    decimal limitation = Convert.ToDecimal(args[0], CultureInfo.InvariantCulture);
                    decimal limit = Convert.ToDecimal(args[1], CultureInfo.InvariantCulture);
                    decimal interest = Convert.ToDecimal(args[2], CultureInfo.InvariantCulture);
                    decimal minPercent = Convert.ToDecimal(args[3], CultureInfo.InvariantCulture);
                    decimal midPercent = Convert.ToDecimal(args[4], CultureInfo.InvariantCulture);
                    decimal maxPercent = Convert.ToDecimal(args[5], CultureInfo.InvariantCulture);
                    string name = args[6];
                    decimal commission = Convert.ToDecimal(args[7], CultureInfo.InvariantCulture);

                    Bank bank = CentralBank.RegisterBank(limitation, limit, interest, minPercent, midPercent, maxPercent, name, commission);
                    System.Console.WriteLine($"bank id is : {bank.Id}");
                    break;
                }

                case "2":
                {
                    System.Console.WriteLine(
                        "Input : first name, last name, city, street, house, flat, passport series, passport number, bank id");
                    string arguments = System.Console.ReadLine() ?? string.Empty;
                    string[] args = arguments.Split(",");
                    var address = new Address(args[2], args[3], Convert.ToInt32(args[4]), Convert.ToInt32(args[5]));
                    var passport = new Passport(Convert.ToInt32(args[6]), Convert.ToInt32(args[7]));
                    var id = Guid.Parse(args[8]);
                    Bank bank = CentralBank.GetBank(id);
                    CentralBank.RegisterClient(args[0], args[1], address, passport, bank);
                    break;
                }

                case "3":
                {
                    System.Console.WriteLine("Input : first name, last name, bank id");
                    string arguments = System.Console.ReadLine() ?? string.Empty;
                    string[] args = arguments.Split(",");
                    var id = Guid.Parse(args[2]);
                    Bank bank = CentralBank.GetBank(id);
                    Client client = CentralBank.GetClient(args[0], args[1]);
                    DebitAccount account = CentralBank.CreateDebitAccount(client, bank);
                    System.Console.WriteLine($"account id is : {account.Id}");
                    break;
                }

                case "4":
                {
                    System.Console.WriteLine("Input : first name, last name, bank id, money, validity");
                    string arguments = System.Console.ReadLine() ?? string.Empty;
                    string[] args = arguments.Split(",");
                    var id = Guid.Parse(args[2]);
                    Bank bank = CentralBank.GetBank(id);
                    Client client = CentralBank.GetClient(args[0], args[1]);
                    decimal money = Convert.ToDecimal(args[3], CultureInfo.InvariantCulture);
                    int validity = Convert.ToInt32(args[4]);
                    DepositAccount account = CentralBank.CreateDepositAccount(client, bank, money, validity);
                    System.Console.WriteLine($"account id is : {account.Id}");
                    break;
                }

                case "5":
                {
                    System.Console.WriteLine("Input : first name, last name, bank id");
                    string arguments = System.Console.ReadLine() ?? string.Empty;
                    string[] args = arguments.Split(",");
                    var id = Guid.Parse(args[2]);
                    Bank bank = CentralBank.GetBank(id);
                    Client client = CentralBank.GetClient(args[0], args[1]);
                    CreditAccount account = CentralBank.CreateCreditAccount(client, bank);
                    System.Console.WriteLine($"account id is : {account.Id}");
                    break;
                }

                case "6":
                {
                    System.Console.WriteLine("Input : account id, money");
                    string arguments = System.Console.ReadLine() ?? string.Empty;
                    string[] args = arguments.Split(",");
                    var id = Guid.Parse(args[0]);
                    IBankAccount bankAccount = CentralBank.GetBankAccount(id);
                    Replenishment replenishment = CentralBank.CreateReplenishment(bankAccount, Convert.ToDecimal(args[1], CultureInfo.InvariantCulture));
                    System.Console.WriteLine($"Top up money to account : {bankAccount.Id}");
                    CentralBank.ExecuteTransaction(replenishment.Id);
                    break;
                }

                case "7":
                {
                    System.Console.WriteLine("Input : account id, money");
                    string arguments = System.Console.ReadLine() ?? string.Empty;
                    string[] args = arguments.Split(",");
                    var id = Guid.Parse(args[0]);
                    IBankAccount bankAccount = CentralBank.GetBankAccount(id);
                    Withdrawal withdrawal = CentralBank.CreateWithdrawal(bankAccount, Convert.ToDecimal(args[1], CultureInfo.InvariantCulture));
                    System.Console.WriteLine($"Withdrawal money from account : {bankAccount.Id}");
                    CentralBank.ExecuteTransaction(withdrawal.Id);
                    break;
                }

                case "8":
                {
                    System.Console.WriteLine("Input : accountTo id, accountFrom id, money");
                    string arguments = System.Console.ReadLine() ?? string.Empty;
                    string[] args = arguments.Split(",");
                    var toId = Guid.Parse(args[0]);
                    var fromId = Guid.Parse(args[1]);
                    IBankAccount toAccount = CentralBank.GetBankAccount(toId);
                    IBankAccount fromAccount = CentralBank.GetBankAccount(fromId);
                    Transfer transfer = CentralBank.CreateTransfer(fromAccount, toAccount, Convert.ToDecimal(args[1], CultureInfo.InvariantCulture));
                    CentralBank.ExecuteTransaction(transfer.Id);
                    break;
                }

                case "exit":
                {
                    return;
                }
            }
        }
    }

    private static void DrawMenu()
    {
        System.Console.WriteLine("Choose option:");
        System.Console.WriteLine("1 - Add bank");
        System.Console.WriteLine("2 - Add client");
        System.Console.WriteLine("3 - Register debit account");
        System.Console.WriteLine("4 - Register deposit account");
        System.Console.WriteLine("5 - Register credit account");
        System.Console.WriteLine("6 - Top up account");
        System.Console.WriteLine("7 - Withdrawal account");
        System.Console.WriteLine("8 - Transfer money");
    }
}