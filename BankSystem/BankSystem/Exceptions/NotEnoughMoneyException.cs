using System;

namespace BankSystem.Exceptions
{
    class NotEnoughMoneyException : Exception
    {
        public NotEnoughMoneyException(string message) : base(message)
        {

        }
    }
}
