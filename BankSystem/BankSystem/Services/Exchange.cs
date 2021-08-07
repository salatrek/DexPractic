using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Services
{
    class Exchange : IExchange
    {
        public float СurrencyСonversion(Currency originalСurrency, float count, Currency desiredСurrency)
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
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
