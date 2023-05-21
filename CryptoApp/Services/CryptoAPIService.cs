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
    public class CryptoAPIService : ICryptoAPIService
    {
        private readonly HttpClient client;
        private readonly int numberOfTopCurrencies = 10;
        private readonly string coinGeskoUrl = "https://api.coingecko.com/api/v3";
        private readonly string searchCoinsEndpoint = "/coins/list";
        private readonly string coinCapUrl = "https://api.coincap.io/v2";
        private readonly string assetsEndpoint = "/assets";
        private readonly string marketChartEndpoint = "/market_chart?vs_currency=usd&days=30";

        public CryptoAPIService()
        {
            client = new HttpClient();
        }

        public async Task<List<string>> GetCurrencyNames()
        {
            string url = coinCapUrl + assetsEndpoint;
            var responseBody = await RequestPerform(url);
            
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);
            var dataString = data["data"].ToString();
            var currenciesViewModel = JsonConvert.DeserializeObject<List<CurrencyPriceViewModel>>(dataString);

            var currencyNamesList = currenciesViewModel.Select(currency => currency.Id).ToList();

            return currencyNamesList;
        }

        public async Task<List<SearchCryptoCurrenciesViewModel>?> GetCryptoCurrencies(string search)
        {
            string url = $"{coinGeskoUrl + searchCoinsEndpoint}";
            var responseBody = await RequestPerform(url);

            var data = JsonConvert.DeserializeObject<List<SearchCryptoCurrenciesViewModel>>(responseBody);

            var currencies = data
                .Where(currency => currency.Name.ToLower().Contains(search.ToLower()))
                .Take(numberOfTopCurrencies).ToList();

            return currencies;
        }

        public async Task<List<CryptoCurrenciesViewModel>?> GetCryptoCurrencies()
        {
            string url = $"{coinCapUrl + assetsEndpoint}";
            var responseBody = await RequestPerform(url);

            if (responseBody == null)
            {
                return null;
            }

            var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);
            var currenciesList = jsonDictionary["data"].ToString();

            var currencies = JsonConvert.DeserializeObject<List<CryptoCurrenciesViewModel>>(currenciesList);
            var currenciesTopRank = currencies
                .OrderByDescending(currency => currency.VolumeUsd24Hr)
                .Take(numberOfTopCurrencies).ToList();

            return currenciesTopRank;
        }

        public async Task<CoinDetailsViewModel> GetCoinDetails(string id)
        {
            string url = $"{coinGeskoUrl}/coins/{id}";
            var responseBody = await RequestPerform(url);

            if (responseBody == null)
            {
                return null;
            }

            var coinDetails = await CoinJsonInfo(responseBody);
            return coinDetails;
        }

        public async Task<double?> GetCoinPrice(string id)
        {
            string url = $"{coinCapUrl}{assetsEndpoint}/{id}";
            var responseBody = await RequestPerform(url);

            if (responseBody == null)
            {
                return null;
            }

            var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);
            var currenciesList = jsonDictionary["data"].ToString();

            var currency = JsonConvert.DeserializeObject<CurrencyPriceViewModel>(currenciesList);

            var currencyPrice = currency.priceUsd;
            var price = Convert.ToDouble(currencyPrice, new CultureInfo("en-US"));

            return price;
        }

        public async Task<(List<float>, List<float>)?> GetCoinPriceHistory(string id)
        {
            string url = $"{coinGeskoUrl}/coins/{id}{marketChartEndpoint}";
            var responseBody = await RequestPerform(url);

            if (responseBody == null)
            {
                return null;
            }

            var jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<List<float>>>>(responseBody);

            List<float> timestamps = jsonData["prices"].Select(x => x[0]).ToList();
            List<float> values = jsonData["prices"].Select(x => x[1]).ToList();

            return (timestamps, values);
        }

        private async Task<string?> RequestPerform(string url)
        {
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return "";
            }

            var content = await response.Content.ReadAsStringAsync();
            return content;
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
