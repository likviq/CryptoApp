﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp.Models.ViewModels
{
    public class CryptoCurrenciesViewModel
    {
        public float Rank { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
