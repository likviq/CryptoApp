using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp.Models.ViewModels
{
    public class CryptoCurrenciesViewModel
    {
        public int Rank { get; set; }
        public string VolumeUsd24Hr { get; set; }
        public string Name { get; set; }
        public string priceUsd { get; set; }
    }
}
