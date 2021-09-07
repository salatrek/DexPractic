using System.Threading;
using System.Threading.Tasks;
using BankSystem.Models;
using BankSystem.Services;
using Xunit;

namespace BankSystem.Test
{
    public class BankServiceTest
    {
        [Fact]
        public void FindPerson()
        {
            //Arrange
            var bankSystem = new BankService();
            bankSystem.clientsFilePath = "D:/DexPractic/DexPractic/BankSystem/BankSystem/Resources/ListOfClients.txt";
            var client = new Models.Client() { Name = "Oleg client", Age = 34, PassportID = 999, Status = "Good" };

            //Act
            bankSystem.AddPerson(client);
            var result = bankSystem.FindClient(999);


            //Assert
            Assert.Equal(client, result);
        }

        [Fact]
        public async Task ParallelAddMoney()
        {
            //Arrange
            var bankService = new BankService(initializeFromFile: false);
            var newClient = new Client()
            {
                PassportID = 236,
                Name = "Test client",
                Age = 55,
                Status = "Good"
            };
            bankService.AddPerson(newClient);

            var testAccount = new Account()
            {
                AccountBalance = 0,
                CurrencyType = new Currency(Currencies.RUB)
            };
            bankService.AddAccountToClient(newClient.PassportID, testAccount);

            //Act
            await Task.Factory.StartNew(() => bankService.AccrualMoney(testAccount, 200));
            await Task.Factory.StartNew(() => bankService.AccrualMoney(testAccount, 300));
            
            var newBalance = testAccount.AccountBalance;

            //Assert
            Assert.Equal(expected: 500f, actual: newBalance);
        }
    }
}