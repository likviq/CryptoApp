using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp.Models.ViewModels
{
    public class CoinDetailsViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public float Volume { get; set; }
        public float PriceChange24h { get; set; }
        public List<MarketDetails> Markets { get; set; }
    }
    public class MarketDetails
    {
        public string Name { get; set; }
        public string CoinPrice { get; set; }
        public string Url { get; set; }
    }
}
