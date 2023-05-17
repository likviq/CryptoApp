using CryptoApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CryptoApp
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private readonly ICryptoAPI _cryptoAPI;
        public MainPage(ICryptoAPI cryptoAPI)
        {
            _cryptoAPI = cryptoAPI;
            GetTopCurrencies();
            InitializeComponent();
        }

        private async void GetTopCurrencies()
        {
            var cryptoCurrencies = await _cryptoAPI.GetCryptoCurrencies();

            cryptoListBox.ItemsSource = cryptoCurrencies;
        }
    }
}
