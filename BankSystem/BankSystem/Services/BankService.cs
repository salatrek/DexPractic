using BankSystem.Exceptions;
using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace BankSystem.Services
{
    internal class BankService
    {
        List<Client> _clientsList;
        List<Employee> _employeersList;
        Dictionary<int, List<Account>> _clientsInfo;

        private const string clientsFilePath = "../../../Resources/ListOfClients.txt";
        private const string employeersFilePath = "../../../Resources/ListOfEmployeers.txt";
        private const string clientsDictionaryFilePath = "../../../Resources/DictionaryOfClients.txt";

        public BankService()
        {
            _clientsList = new List<Client>();
            _employeersList = new List<Employee>();
            _clientsInfo = new Dictionary<int, List<Account>>();

            FillListFromFile();
            FillDictionaryFromFile();
        }

        private void FillListFromFile()
        {
            if (File.Exists(clientsFilePath))
            {
                string text;
                using (var streamReader = new StreamReader(clientsFilePath))
                {
                    text = streamReader.ReadToEnd();
                }
                _clientsList = JsonConvert.DeserializeObject<List<Client>>(text);

            }
            if (File.Exists(employeersFilePath))
            {
                string text;
                using (var streamReader = new StreamReader(employeersFilePath))
                {
                    text = streamReader.ReadToEnd();
                }
                _employeersList = JsonConvert.DeserializeObject<List<Employee>>(text);
            }
        }
        private void FillDictionaryFromFile()
        {
            if (File.Exists(clientsDictionaryFilePath))
            {
                string allText;

                using (var streamReader = new StreamReader(clientsDictionaryFilePath))
                {
                    allText = streamReader.ReadToEnd();
                }

                _clientsInfo = JsonConvert.DeserializeObject<Dictionary<int, List<Account>>>(allText);
            }
        }

        public void AddAccountToClient(int passportID, Account account)
        {
            if (_clientsInfo.ContainsKey(passportID))
            {
                _clientsInfo[passportID].Add(account);
                UpdateDictionaryDataFile();
            }
            else
            {
                _clientsInfo.Add(passportID, new List<Account>() { account });
                UpdateDictionaryDataFile();
            }
        }

        private void UpdateDictionaryDataFile()
        {
            var serDictionary = JsonConvert.SerializeObject(_clientsInfo);

            using (var streamWriter = new StreamWriter(clientsDictionaryFilePath, false))
            {
                streamWriter.Write(serDictionary);
            }
        }

        public void TransferMoney(
            float summ,
            Account sourceAccount,
            Account targetAccount,
            Func<Currency, float, Currency, float> moneyTransferHandle)
        {
            if (sourceAccount == null) throw new ArgumentNullException(nameof(sourceAccount));
            if (targetAccount == null) throw new ArgumentNullException(nameof(sourceAccount));
            if (moneyTransferHandle == null) throw new ArgumentNullException(nameof(sourceAccount));
            if (summ <= 0) throw new InvalidSummException("Указана некорректная сумма.");
            if (summ > sourceAccount.AccountBalance) throw new NotEnoughMoneyException("Недостаточно средств на счете.");

            var targetSumm = moneyTransferHandle(sourceAccount.CurrencyType, summ, targetAccount.CurrencyType);

            sourceAccount.AccountBalance = sourceAccount.AccountBalance - summ;
            targetAccount.AccountBalance = targetAccount.AccountBalance + targetSumm;
        }

        public void AddPerson<T>(T person) where T : IPerson
        {
            if (person.Age < 18) throw new YoungPersonException("Пользователь не достиг совершеннолетия (18 лет)");

            if (person is Client client)
            {
                if (!_clientsList.Contains(client))
                {
                    _clientsList.Add(client);
                    SerializeList(_clientsList, clientsFilePath);
                }
                else
                {
                    Console.WriteLine("Такой клиент уже есть в списке.");
                }
            }

            if (person is Employee employee)
            {
                if (!_employeersList.Contains(employee))
                {
                    _employeersList.Add(employee);
                    SerializeList(_employeersList, employeersFilePath);
                }
                else
                {
                    Console.WriteLine("Такой сотрудник уже есть в списке.");
                }
            }
        }

        private void SerializeList<T>(List<T> personList, string path) where T : IPerson // serialize list, path to params
        {
            var srPersonsList = JsonConvert.SerializeObject(personList);
            using (var streamWriter = new StreamWriter(path, false))
            {
                streamWriter.Write(srPersonsList);
            }
        }

        public Employee FindEmployee(int passportID)
        {
            if (passportID > 0) return Find<Employee>(passportID);
            throw new ArgumentException($"Введеный номер паспорта: {passportID}, не валиден");
        }

        public Client FindClient(int passportID)
        {
            if (passportID > 0) return Find<Client>(passportID);
            throw new ArgumentException($"Введеный номер паспорта: {passportID}, не валиден");
        }

        private T Find<T>(int passportID) where T : IPerson
        {
            if (typeof(T) == typeof(Employee))
            {
                IPerson employee = _employeersList.Single(x => x.PassportID == passportID);
                if (employee is null) throw new PersonNotFoundException("Сотрудник не найден.");

                return (T)employee;
            }

            if (typeof(T) == typeof(Client))
            {
                IPerson client = _clientsList.Single(x => x.PassportID == passportID);
                if (client is null) throw new PersonNotFoundException("Клиент не найден.");

                return (T)client;
            }

            throw new InvalidOperationException("Искомый объект не найден, или не реализован метод для его поиска.");
        }
    }
}
