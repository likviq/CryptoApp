using CryptoApp.Interfaces;
using CryptoApp.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp.Services
{
    public class CryptoAPI : ICryptoAPI
    {
        private readonly HttpClient client;
        private readonly int numberOfTopCurrencies = 10;
        private readonly string coinGeskoUrl = "https://api.coingecko.com/api/v3";
        private readonly string searchCoinsEndpoint = "/coins/list";
        private readonly string coinCapUrl = "https://api.coincap.io/v2";
        private readonly string assetsEndpoint = "/assets";

        public CryptoAPI()
        {
            client = new HttpClient();
        }

        public async Task<List<string>> GetCurrencyNames()
        {
            string url = coinCapUrl + assetsEndpoint;
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
            var dataString = data["data"].ToString();

            var currenciesViewModel = JsonConvert.DeserializeObject<List<CurrencyPriceViewModel>>(dataString);

            var currencyNamesList = currenciesViewModel.Select(currency => currency.Id).ToList();

            return currencyNamesList;
        }

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

        public async Task<CoinDetailsViewModel> GetCoinDetails(string id)
        {
            HttpClient client = new HttpClient();

            string url = $"{coinGeskoUrl}/coins/{id}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string responseBody = await response.Content.ReadAsStringAsync();

            var coinDetails = await CoinJsonInfo(responseBody);
            return coinDetails;
        }

        public async Task<double?> GetCoinPrice(string id)
        {
            HttpClient client = new HttpClient();

            string url = $"{coinCapUrl}{assetsEndpoint}/{id}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string jsonString = await response.Content.ReadAsStringAsync();
            
            var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
            var currenciesList = jsonDictionary["data"].ToString();

            var currency = JsonConvert.DeserializeObject<CurrencyPriceViewModel>(currenciesList);

            var currencyPrice = currency.priceUsd;
            var price = Convert.ToDouble(currencyPrice, new CultureInfo("en-US"));

            return price;
        }

        private async Task<CoinDetailsViewModel> CoinJsonInfo(string responseBody)
        {
            dynamic data = JsonConvert.DeserializeObject(responseBody);

            string name = data["localization"]["en"];
            string description = data["description"]["en"];
            float price = data["market_data"]["current_price"]["usd"];
            float volume = data["market_data"]["total_volume"]["usd"];
            float priceChange24h = data["market_data"]["price_change_24h"];
            
            List<MarketDetails> markets = new List<MarketDetails>();
            foreach (var market in data["tickers"])
            {
                string marketName = market["market"]["name"];
                string marketPrice = market["last"];
                string marketUrl = market["trade_url"];
                var marketEntry = new MarketDetails()
                {
                    Name = marketName,
                    CoinPrice = marketPrice,
                    Url = marketUrl
                };
                markets.Add(marketEntry);
            }

            CoinDetailsViewModel viewModel = new CoinDetailsViewModel()
            {
                Name = name,
                Description = description,
                Price = price,
                Volume = volume,
                PriceChange24h = priceChange24h,
                Markets = markets
            };

            return viewModel;
        }
    }
}
