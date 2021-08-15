using BankSystem.Exceptions;
using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BankSystem.Services
{
    class BankService
    {
        List<Client> _listOfClient = new List<Client>();

        List<Employee> _listOfEmployee = new List<Employee>();

        Dictionary<Client, List<Account>> clientInfo = new Dictionary<Client, List<Account>>();


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
                string[] linesArray = File.ReadAllLines(clientsPath);
                foreach (var line in linesArray)
                {
                    string[] parts = line.Split("\t");
                    _listOfClient.Add(new Client() { Name = parts[0], Age = int.Parse(parts[1]), PassportID = int.Parse(parts[2]), Status = parts[3] });
                }
            }
            if (File.Exists(employeersPath))
            {
                string[] linesArray = File.ReadAllLines(employeersPath);
                foreach (var line in linesArray)
                {
                    string[] parts = line.Split("\t");
                    _listOfEmployee.Add(new Employee() { Name = parts[0], Age = int.Parse(parts[1]), PassportID = int.Parse(parts[2]), Position = parts[3] });
                }
            }
        }
        private void ReadToDictionary()
        {
            if (File.Exists(dictionaryOfClientsPath))
            {
                string allText;
                string[] linesRead;

                using (StreamReader streamReader = new StreamReader(dictionaryOfClientsPath))
                {
                    allText = streamReader.ReadToEnd();
                    linesRead = allText.Split("]\n");
                }

                foreach (var line in linesRead)
                {
                    var clientText = line.Substring(0, line.IndexOf("[")-1);
                    string[] clientInfoParts = clientText.Split("\t");

                    Client client = new Client() { 
                        Name = clientInfoParts[1], 
                        Age = int.Parse(clientInfoParts[2]), 
                        PassportID = int.Parse(clientInfoParts[3]), 
                        Status = clientInfoParts[4],
                        ID = clientInfoParts[0]};

                    clientInfo.Add(client, new List<Account>());

                    var accountText = line.Substring(line.IndexOf("[")+2);
                    string[] accountInfoParts = accountText.Split(";\n");

                    foreach (var accountInfoPart in accountInfoParts.Where(x => string.IsNullOrWhiteSpace(x)))
                    {
                        string[] accountInfo = accountInfoPart.TrimStart('\t').Split("\t");
                        Account account = new Account() {
                            AccountBalance = float.Parse(accountInfo[0]), 
                            TypeOfCurrency = new Currency() { 
                                Rate = float.Parse(accountInfo[1]) 
                            } 
                        };

                        clientInfo[client].Add(account);
                    }
                }
            }
        }

        public void AddClientAccount(Client client, Account account)
        {
            if (clientInfo.ContainsKey(client))
            {
                clientInfo[client].Add(account);

                string text;

                using (StreamReader streamReader = new StreamReader(dictionaryOfClientsPath))
                {
                    text = streamReader.ReadToEnd();
                }

                var idIndex = text.IndexOf(client.ID);
                var pasteIndex = text.IndexOf(']', idIndex);
                var newLine = $"\t{account.AccountBalance}\t{account.TypeOfCurrency.Rate};\n";
                text = text.Insert(pasteIndex, newLine);

                using (StreamWriter streamWriter = new StreamWriter(dictionaryOfClientsPath, false))
                {
                    streamWriter.Write(text);
                }
            }
            else
            {
                clientInfo.Add(client, new List<Account>() { account });

                string array = $"{client.ID}\t{client.Name}\t{client.Age}\t{client.PassportID}\t{client.Status}\n[\n\t{account.AccountBalance}\t{account.TypeOfCurrency.Rate};\n]\n";
                StreamWriter streamWriter = new StreamWriter(dictionaryOfClientsPath, true);
                streamWriter.Write(array);
                streamWriter.Close();
                streamWriter.Dispose();

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

                    string array = $"{client.Name}\t{client.Age}\t{client.PassportID}\t{client.Status}";
                    StreamWriter streamWriter = new StreamWriter(clientsPath, true);
                    streamWriter.Write(array);
                    streamWriter.Close();
                    streamWriter.Dispose();
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

                    string array = $"{employee.Name}\t{employee.Age}\t{employee.PassportID}\t{employee.Position}";
                    StreamWriter streamWriter = new StreamWriter(employeersPath, true);
                    streamWriter.Write(array);
                    streamWriter.Close();
                    streamWriter.Dispose();
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
