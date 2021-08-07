using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Services
{
    public interface IExchange
    {
        float СurrencyСonversion(Currency originalСurrency, float count, Currency desiredСurrency);
        
    }
}
