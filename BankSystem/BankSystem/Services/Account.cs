using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Services
{
    public class Account
    {
        public Currency TypeOfCurrency { get; set; }
        public float AccountBalance { get; set; }
    }
}
