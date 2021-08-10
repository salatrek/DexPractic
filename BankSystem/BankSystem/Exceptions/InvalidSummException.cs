using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Exceptions
{
    class InvalidSummException : Exception
    {
        public InvalidSummException(string message) : base(message)
        {

        }
    }
}
