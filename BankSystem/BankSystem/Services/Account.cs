using BankSystem.Models;

namespace BankSystem.Services
{
    public class Account
    {
        public Currency CurrencyType { get; set; }
        public float AccountBalance { get; set; }
    }
}
