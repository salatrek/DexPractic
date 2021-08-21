using System;

namespace BankSystem.Exceptions
{
    class PersonNotFoundException : Exception

    {
        public PersonNotFoundException(string message) : base(message)
        {

        }
    }
}
