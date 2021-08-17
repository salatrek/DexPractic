using BankSystem.Models;
using BankSystem.Services;
using System;
using static BankSystem.Services.BankService;

namespace BankSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new BankService();

            service.AddPerson(new Client() {Name = "Alex client",Age = 25, PassportID = 580, Status = "Good" });
            service.AddPerson(new Employee() { Name = "John employee", Age = 34, PassportID = 789, Position = "Clerk" });

            Currency Rubles = new Currency() { Rate = 73.13f };
            Currency Euro = new Currency() { Rate = 0.84f };
            Currency Hryvnia = new Currency() { Rate = 26.82f };

            Client Anton = new Client() { Name = "Anton client", Age = 34, PassportID = 547, Status = "Good"};
            Client Alex = new Client() { Name = "Alex client", Age = 25, PassportID = 965, Status = "Good"};
            Client Anna = new Client() { Name = "Ana client", Age = 26, PassportID = 124, Status = "Good"};


            Account account = new Account() {TypeOfCurrency = Rubles, AccountBalance = 900};
            Account account2 = new Account() { TypeOfCurrency = Euro, AccountBalance = 500 };
            Account account3 = new Account() { TypeOfCurrency = Hryvnia, AccountBalance = 850 };
            Account account4 = new Account() { TypeOfCurrency = Rubles, AccountBalance = 120 };
            Account account5 = new Account() { TypeOfCurrency = Euro, AccountBalance = 1500 };

            try
            {
                service.FindClient(789);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }

            

            Exchange exchange = new Exchange();
            var balance = exchange.СurrencyСonversion(Rubles, 800, Euro);

            service.AddClientAccount(Anton.PassportID, account);
            service.AddClientAccount(Anton.PassportID, account2);
            service.AddClientAccount(Anton.PassportID, account3);
            service.AddClientAccount(Alex.PassportID, account4);
            service.AddClientAccount(Anna.PassportID, account5);

        }
    }
}
