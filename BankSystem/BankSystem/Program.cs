using BankSystem.Models;
using BankSystem.Services;
using System;

namespace BankSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new BankService();
            service.AddPerson(new Client() {Name = "Alex client",Age = 25, PassportID = 580, Status = "Good" });
            service.AddPerson(new Employee() { Name = "John employee", Age = 34, PassportID = 789, Position = "Clerk" });

            try
            {
                service.FindClient(789);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
