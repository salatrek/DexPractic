using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    class Employee : IPerson
    {
        public string Name { get; set; }
        public string Age { get; set; }
        public int PassportID { get; set; }
        public string Position { get; set; }
    }
}
