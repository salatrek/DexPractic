using System;

namespace BankSystem.Exceptions
{
    class InvalidSummException : Exception
    {
        public InvalidSummException(string message) : base(message)
        {

        }
    }
}
