using BankSystem.Exceptions;
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

        Dictionary<Client, List<Account>> clientInfo = new Dictionary<Client, List<Account>>();

        public void AddClientAccount(Client client, Account account)
        {
            if (clientInfo.ContainsKey(client))
            {
                clientInfo[client].Add(account);
            }
            else
            {
                clientInfo.Add(client, new List<Account>() { account });
            }
        }

        public void MoneyTransfer(float summ, Account sourceAccount, Account targetAccount, Func<Currency,float,Currency,float> moneyTransferHandle)
        {

            if (sourceAccount == null) throw new ArgumentNullException(nameof(sourceAccount));
            if (targetAccount == null) throw new ArgumentNullException(nameof(sourceAccount));
            if (moneyTransferHandle == null) throw new ArgumentNullException(nameof(sourceAccount));
            if (summ <= 0) throw new InvalidSummException("Указана некорректная сумма.");
            if (summ > sourceAccount.AccountBalance) throw new NotEnoughMoneyException("Недостаточно средств на счете.");

            var targetSumm = moneyTransferHandle(sourceAccount.TypeOfCurrency, summ, targetAccount.TypeOfCurrency);

            sourceAccount.AccountBalance = sourceAccount.AccountBalance - summ;
            targetAccount.AccountBalance = targetAccount.AccountBalance + targetSumm;



        }

        public void AddPerson<T>(T person) where T : IPerson
        {
            if (person.Age < 18) throw new YoungPersonException("Пользователь не достиг совершеннолетия (18 лет)");

            var client = person as Client;

            if (client != null)
            {
                if (!_listOfClient.Contains(client))
                {
                    _listOfClient.Add(client);
                }
                else
                {
                    Console.WriteLine("Такой клиент уже есть в списке.");
                }
            }

            var employee = person as Employee;

            if (employee != null)
            {
                if (!_listOfEmployee.Contains(employee))
                {
                    _listOfEmployee.Add(employee);
                }
                else
                {
                    Console.WriteLine("Такой работник уже есть в списке.");
                }
            }
        }

        public Employee FindEmployee(int passportID)
        {
            return Find<Employee>(passportID);
        }

        public Client FindClient(int passportID)
        {
            return Find<Client>(passportID);
        }

        private T Find<T>(int passportID) where T : IPerson
        {
            if (typeof(T) == typeof(Employee))
            {
                IPerson employee = _listOfEmployee.Single(x => x.PassportID == passportID);
                return (T)employee;
            }

            else if (typeof(T) == typeof(Client))
            {
                IPerson client = _listOfClient.Single(x => x.PassportID == passportID);
                return (T)client;
            }
            throw new InvalidOperationException();
        }
    }
}
