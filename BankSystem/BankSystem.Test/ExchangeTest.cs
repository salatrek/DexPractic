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
        public async Task СurrencyСonversion_Rubles_800_Euro_Eq_9Async()
        {
            //Arrange
            var exchange = new Exchange();

            //Act
            var result = await exchange.СurrencyСonversionAsync(
                new Currency(Currencies.RUB) ,
                800,
                new Currency(Currencies.EUR));

            var expectedResult = 800 * 0.011474;

            Assert.True(Math.Abs(expectedResult - result) <= 0.000001f);
        }

        [Fact]
        public async Task СurrencyСonversion_Null_Negative()
        {
            var exchange = new Exchange();

            await Assert.ThrowsAsync<ArgumentNullException>(
                () => exchange.СurrencyСonversionAsync<Currency>(new Currency(Currencies.RUB), 800, null));
        }
    } 
}



