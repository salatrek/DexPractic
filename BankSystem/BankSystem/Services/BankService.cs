using BankSystem.Exceptions;
using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace BankSystem.Services
{
    class BankService
    {
        List<Client> _listOfClient = new List<Client>();

        List<Employee> _listOfEmployee = new List<Employee>();

        Dictionary<int, List<Account>> _clientInfo = new Dictionary<int, List<Account>>();


        string clientsPath = "../../../Resources/ListOfClients.txt";
        string employeersPath = "../../../Resources/ListOfEmployeers.txt";
        string dictionaryOfClientsPath = "../../../Resources/DictionaryOfClients.txt";

        public BankService()
        {
            ReadToList();
            ReadToDictionary();
        }

        private void ReadToList()
        {
            if (File.Exists(clientsPath))
            {
                string text;
                using (StreamReader streamReader = new StreamReader(clientsPath))
                {
                    text = streamReader.ReadToEnd();
                }
                _listOfClient = JsonConvert.DeserializeObject<List<Client>>(text);

            }
            if (File.Exists(employeersPath))
            {
                string text;
                using (StreamReader streamReader = new StreamReader(employeersPath))
                {
                    text = streamReader.ReadToEnd();
                }
                _listOfEmployee = JsonConvert.DeserializeObject<List<Employee>>(text);
            }
        }
        private void ReadToDictionary()
        {
            if (File.Exists(dictionaryOfClientsPath))
            {
                string allText;

                using (StreamReader streamReader = new StreamReader(dictionaryOfClientsPath))
                {
                    allText = streamReader.ReadToEnd();
                }

                _clientInfo = JsonConvert.DeserializeObject<Dictionary<int, List<Account>>>(allText);
            }
        }

        public void AddClientAccount(int passportID, Account account)
        {
            if (_clientInfo.ContainsKey(passportID))
            {
                _clientInfo[passportID].Add(account);
                UpdateDataFileDictionary();
            }
            else
            {
                _clientInfo.Add(passportID, new List<Account>() { account });
                UpdateDataFileDictionary();
            }
        }

        private void UpdateDataFileDictionary()
        {
            var serDictionary = JsonConvert.SerializeObject(_clientInfo);

            using (StreamWriter streamWriter = new StreamWriter(dictionaryOfClientsPath, false))
            {
                streamWriter.Write(serDictionary);
            }
        }

        public void MoneyTransfer(float summ, Account sourceAccount, Account targetAccount, Func<Currency, float, Currency, float> moneyTransferHandle)
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
                    ListSerializator(_listOfClient);
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
                    ListSerializator(_listOfEmployee);
                }
                else
                {
                    Console.WriteLine("Такой работник уже есть в списке.");
                }
            }
        }

        private void ListSerializator<T>(List<T> personList) where T : IPerson
        {
            if (typeof(T) == typeof(Employee))
            {
                var srEmployee = JsonConvert.SerializeObject(personList);
                using (StreamWriter streamWriter = new StreamWriter(employeersPath, false))
                {
                    streamWriter.Write(srEmployee);
                }
            }
            if (typeof(T) == typeof(Client))
            {
                var srEmployee = JsonConvert.SerializeObject(personList);
                using (StreamWriter streamWriter = new StreamWriter(clientsPath, false))
                {
                    streamWriter.Write(srEmployee);
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
