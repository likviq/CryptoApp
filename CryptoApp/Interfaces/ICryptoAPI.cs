using CryptoApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp.Interfaces
{
    public interface ICryptoAPI
    {
        Task<List<string>> GetCurrencyNames();
        Task<List<SearchCryptoCurrenciesViewModel>?> GetCryptoCurrencies(string search);
        Task<List<CryptoCurrenciesViewModel>?> GetCryptoCurrencies();
        Task<CoinDetailsViewModel> GetCoinDetails(string id);
        Task<double?> GetCoinPrice(string id);
    }
}
