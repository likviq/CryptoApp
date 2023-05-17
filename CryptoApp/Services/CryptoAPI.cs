using CryptoApp.Interfaces;
using CryptoApp.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private readonly int numberOfTopCurrencies = 10;
        private readonly string coinGeskoUrl = "https://api.coingecko.com/api/v3";
        private readonly string searchCoinsEndpoint = "/coins/list";
        private readonly string coinCapUrl = "https://api.coincap.io/v2";
        private readonly string assetsEndpoint = "/assets";

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
                .Take(numberOfTopCurrencies).ToList();

            return currencies;
        }

        public async Task<List<CryptoCurrenciesViewModel>?> GetCryptoCurrencies()
        {
            HttpClient client = new HttpClient();

            string url = $"{coinCapUrl + assetsEndpoint}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string jsonString = await response.Content.ReadAsStringAsync();

            var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
            var currenciesList = jsonDictionary["data"].ToString();

            var currencies = JsonConvert.DeserializeObject<List<CryptoCurrenciesViewModel>>(currenciesList);
            var currenciesTopRank = currencies
                .OrderByDescending(currency => currency.VolumeUsd24Hr)
                .Take(numberOfTopCurrencies).ToList();

            return currenciesTopRank;
        }
    }
}
