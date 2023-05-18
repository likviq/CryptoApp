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
        public Dictionary<string, string> Description { get; set; }
        //public Dictionary<string, List<string>> Links { get; set; }
        public string Community_Score { get; set; }
        public Dictionary<string, string> Community_Data { get; set; }
    }
}
