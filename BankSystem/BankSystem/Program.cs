using BankSystem.Models;
using BankSystem.Services;
using System;
using Bogus;
using Bogus.DataSets;
using System.Reflection;
using System.Threading.Tasks;

namespace BankSystem
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var service = new BankService();

            var Rubles = new Models.Currency(Currencies.RUB);
            var Euro = new Models.Currency(Currencies.EUR);
            var Hryvnia = new Models.Currency(Currencies.UAH);
            var Dollar = new Models.Currency(Currencies.USD);

            Client Anton = new Client() { Name = "Anton client", Age = 34, PassportID = 547, Status = "Good" };
            Client Alex = new Client() { Name = "Alex client", Age = 25, PassportID = 965, Status = "Good" };
            Client Anna = new Client() { Name = "Ana client", Age = 26, PassportID = 124, Status = "Good" };

            Account account = new Account() { CurrencyType = Rubles, AccountBalance = 900 };
            Account account2 = new Account() { CurrencyType = Euro, AccountBalance = 500 };
            Account account3 = new Account() { CurrencyType = Hryvnia, AccountBalance = 850 };
            Account account4 = new Account() { CurrencyType = Rubles, AccountBalance = 120 };
            Account account5 = new Account() { CurrencyType = Euro, AccountBalance = 1500 };

            service.AddAccountToClient(Anton.PassportID, account);
            service.AddAccountToClient(Anton.PassportID, account2);
            service.AddAccountToClient(Anton.PassportID, account3);
            service.AddAccountToClient(Alex.PassportID, account4);
            service.AddAccountToClient(Anna.PassportID, account5);

            Exchange exchange = new Exchange();
            Console.WriteLine($"Обмен валюты: {await exchange.СurrencyСonversionAsync(Dollar, 800, Rubles)}");

            FindClient(511);
            FindEmployee(453);

            FileExportService(Anton);

            //ClientListGenerate();
            //EmployeetListGenerate();

        }

        static void FindClient(int passportID)
        {
            var service = new BankService();
            try
            {
                var client = service.FindClient(passportID);
                var type = client.GetType();

                Console.WriteLine("\nИнформация о найденном клиенте\n");
                PropertyInfo[] properties = type.GetProperties();
                foreach (var item in properties)
                {
                    Console.WriteLine($"{item.Name}\t{item.GetValue(client)}");
                }   
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void FindEmployee(int passportID)
        {
            var service = new BankService();
            try
            {
                var employee = service.FindEmployee(passportID);
                var type = employee.GetType();

                Console.WriteLine("\nИнформация о найденном сотруднике\n");
                PropertyInfo[] properties = type.GetProperties();
                foreach (var item in properties)
                {
                    Console.WriteLine($"{item.Name}\t{item.GetValue(employee)}");
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void FileExportService<T>(T person) where T : IPerson
        {
            FileExportService fileExportServices = new FileExportService();
            fileExportServices.PropertyExport(person);
        }

        static void ClientListGenerate()
        {
            var service = new BankService();

            string[] randomStatus = new string[5];
            randomStatus[0] = "Conscientious";
            randomStatus[1] = "Unscrupulous";
            randomStatus[2] = "Evades payments";
            randomStatus[3] = "Neutral";
            randomStatus[4] = "Bad credit history";

            Random random = new Random();

            var generator = new Faker<Client>("ru").StrictMode(true)
                .RuleFor(x => x.Name, f => f.Name.FirstName())
                .RuleFor(x => x.Age, f => f.Random.Int(16, 85))
                .RuleFor(x => x.PassportID, f => f.Random.Int(1, 999))
                .RuleFor(x => x.Status, f => (randomStatus[random.Next(0, randomStatus.Length)]));

            for (int i = 1; i < 500; i++)
            {
                try
                {
                    service.AddPerson(generator.Generate());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("Done");
        }

        static void EmployeetListGenerate()
        {
            var service = new BankService();

            var generator = new Faker<Employee>("ru").StrictMode(true)
                .RuleFor(x => x.Name, f => f.Name.FirstName())
                .RuleFor(x => x.Age, f => f.Random.Int(16, 85))
                .RuleFor(x => x.PassportID, f => f.Random.Int(1, 999))
                .RuleFor(x => x.Position, f =>f.Name.JobType());

            for (int i = 1; i < 500; i++)
            {
                try
                {
                    service.AddPerson(generator.Generate());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("Done");
        }
    }
}
