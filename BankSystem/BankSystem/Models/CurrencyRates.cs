using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class CurrencyRates
    {
        public bool Success { get; set; }
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public Rates Rates { get; set; }

    }
}
