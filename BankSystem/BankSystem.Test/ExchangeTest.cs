using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using BankSystem.Models;
using BankSystem.Services;

namespace BankSystem.Test
{
    public class ExchangeTest
    {
        [Fact]
        public void СurrencyСonversion_Rubles_800_Euro_Eq_9()
        {
            //Arrange
            var exchange = new Exchange();

            //Act
            var result = exchange.СurrencyСonversion(
                new Currency() { Rate = 73.13f },
                800,
                new Currency() { Rate = 0.84f });

            var expectedResult = 800 / 73.13f * 0.84f;

            Assert.True(Math.Abs(expectedResult - result) <= 0.000001f);
        }
        
        [Fact]
        public void СurrencyСonversion_Null_Negative()
        {
            var exchange = new Exchange();

            Assert.Throws<ArgumentNullException>(
                () =>  exchange.СurrencyСonversion<Currency>(new Currency() { Rate = 73.13f }, 800, null));            
        }
    } 
}



