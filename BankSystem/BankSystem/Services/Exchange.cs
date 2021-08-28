using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;

namespace BankSystem.Services
{
    public class Exchange
    {
        public async Task<float> СurrencyСonversionAsync<T>(T originalСurrency, float count, T desiredСurrency) where T : Currency
        {
            if (originalСurrency != null && desiredСurrency != null)
            {

                var exchangeService = new ExchangeService();
                var currencyRates = await exchangeService.GetRates(originalСurrency.Name, desiredСurrency.Name);
                var type = currencyRates.Rates.GetType();
                var desiredCurrencyProperty = type
                    .GetProperties().Where(x => x.Name.Equals(desiredСurrency.Name)).Single();

                var desiredСurrencyRate = desiredCurrencyProperty.GetValue(currencyRates.Rates);

                if ((float)desiredСurrencyRate > 0 && count > 0)
                {
                    return (float)desiredСurrencyRate * count;
                }
                else
                {
                    throw new ArithmeticException();
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
