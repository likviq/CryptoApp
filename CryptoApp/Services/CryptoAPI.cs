using CryptoApp.Interfaces;
using CryptoApp.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp.Services
{
    public class CryptoAPI : ICryptoAPI
    {
        private readonly string coinGeskoUrl = "https://api.coingecko.com/api/v3";
        private readonly string searchCoinsEndpoint = "/coins/list";
        private readonly string getCoinsEndpoint = "/exchanges";

        public async Task<List<SearchCryptoCurrenciesViewModel>?> GetCryptoCurrencies(string search)
        {
            HttpClient client = new HttpClient();

            string url = $"{coinGeskoUrl + searchCoinsEndpoint}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string jsonString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<SearchCryptoCurrenciesViewModel>>(jsonString);

            var currencies = data
                .Where(currency => currency.Name.ToLower().Contains(search.ToLower()))
                .Take(10).ToList();

            return currencies;
        }

        public async Task<List<CryptoCurrenciesViewModel>?> GetCryptoCurrencies(int amount = 10)
        {
            HttpClient client = new HttpClient();

            string url = $"{coinGeskoUrl + getCoinsEndpoint}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string jsonString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<MyData>(jsonString);

            return new List<CryptoCurrenciesViewModel>();
        }
    }

    public class MyData
    {
        public long Data { get; set; }
    }
}
