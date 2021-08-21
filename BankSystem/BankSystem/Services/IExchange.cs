using BankSystem.Models;

namespace BankSystem.Services
{
    public interface IExchange
    {
        float СurrencyСonversion<T>(T originalСurrency, float count, T desiredСurrency) where T : Currency;
        
    }
}
