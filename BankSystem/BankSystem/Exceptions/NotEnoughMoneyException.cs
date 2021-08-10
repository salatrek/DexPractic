using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Exceptions
{
    class NotEnoughMoneyException : Exception
    {
        public NotEnoughMoneyException(string message) : base(message)
        {

        }
    }
}
