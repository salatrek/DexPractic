using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Services
{
    class BankService
    {
        List<Client> _listOfClient = new List<Client>();

        List<Employee> _listOfEmployee = new List<Employee>();

        public void AddPerson<T>(T person) where T : IPerson
        {
            var client = person as Client;

            if (person != null)
            {
                if (!_listOfClient.Contains(client))
                {
                    _listOfClient.Add(client);
                }
                else
                {
                    Console.WriteLine("Такой клиент уже существует");
                }
            }
            else
            {
                var employee = person as Employee;

                if (person != null)
                {
                    if (!_listOfEmployee.Contains(employee))
                    {
                        _listOfEmployee.Add(employee);
                    }
                    else
                    {
                        Console.WriteLine("Такой работник уже существует");
                    }
                }

            }
        }
        public Employee FindEmployee(int passID)
        {
            return Find<Employee>(passID);
        }

        /*public void FindClient<()
        {

        }*/

        private T Find<T>(int passID) where T : IPerson
        {
            if (typeof(T) == typeof(Employee))
            {
                Employee employee = _listOfEmployee.Single(x => x.PassportID == passID);
                return employee;
            }

            return default(T);
        }
    }
}
