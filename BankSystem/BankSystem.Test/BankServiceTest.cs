using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BankSystem.Test
{
    public class BankServiceTest
    {
        [Fact]
        public void FindPerson()
        {
            //Arrange
            var bankSystem = new Services.BankService();
            bankSystem.clientsFilePath = "D:/DexPractic/DexPractic/BankSystem/BankSystem/Resources/ListOfClients.txt";
            var client = new Models.Client() { Name = "Oleg client", Age = 34, PassportID = 999, Status = "Good" };

            //Act
            bankSystem.AddPerson(client);
            var result = bankSystem.FindClient(999);


            //Assert
            Assert.Equal(client, result);
        }
    }
}
