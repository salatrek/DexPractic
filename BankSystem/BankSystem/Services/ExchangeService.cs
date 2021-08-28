using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Models;
using Newtonsoft.Json;

namespace BankSystem.Services
{
    public class ExchangeService
    {
        public async Task<CurrencyRates> GetRates(string baseCurrencyName, string targetCurrencyName)
        {
            HttpResponseMessage responseMessage;
            CurrencyRates currencyRatesResponce;

            using (HttpClient client = new HttpClient())
            {
                string URL = $"https://api.exchangerate.host/latest?base={baseCurrencyName}&symbols={targetCurrencyName}";
                responseMessage = await client.GetAsync(URL);

                string serializedMessage = await responseMessage.Content.ReadAsStringAsync();
                currencyRatesResponce = JsonConvert.DeserializeObject<CurrencyRates>(serializedMessage);
            }

            return currencyRatesResponce;
        }
    }
}
