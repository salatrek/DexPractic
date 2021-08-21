using BankSystem.Models;
using System;

namespace BankSystem.Services
{
    class Exchange : IExchange
    {
        public float СurrencyСonversion<T>(T originalСurrency, float count, T desiredСurrency) where T : Currency
        {
            if (originalСurrency != null && desiredСurrency != null)
            {
                if (originalСurrency.Rate > 0 && desiredСurrency.Rate > 0 && count > 0)
                {
                    var originalCurrencyRate = originalСurrency.Rate;
                    var desiredCurrencyRate = desiredСurrency.Rate;

                    return (count / originalCurrencyRate) * desiredCurrencyRate;
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
