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
    /// Interaction logic for ConvertPage.xaml
    /// </summary>
    public partial class ConvertPage : Page
    {
        private readonly ICryptoAPIService _cryptoAPI;
        private string currencyFrom;
        private string currencyTo;
        
        public ConvertPage(ICryptoAPIService cryptoAPI)
        {
            _cryptoAPI = cryptoAPI;
            SetCurrenciesDropdown();
            InitializeComponent();
        }

        private async void SetCurrenciesDropdown()
        {
            var currencyNames = await _cryptoAPI.GetCurrencyNames();
            ConvertFrom.ItemsSource = currencyNames;
            ConvertTo.ItemsSource = currencyNames;
        }

        private void ChangeCurrencyFrom(object sender, RoutedEventArgs e)
        {
            currencyFrom = ConvertFrom.SelectedItem?.ToString();
        }

        private void ChangeCurrencyTo(object sender, RoutedEventArgs e)
        {
            currencyTo = ConvertTo.SelectedItem?.ToString();
        }

        private async void ConvertButtonClick(object sender, RoutedEventArgs e)
        {
            string convertFromValue = convertFromTextBox.Text;
            float convertValue;

            bool success = float.TryParse(convertFromValue, out convertValue);

            if (!success)
            {
                MessageBox.Show("incorrect values");
                return;
            }
            var convetFromPrice = await _cryptoAPI.GetCoinPrice(currencyFrom);
            var convetToPrice = await _cryptoAPI.GetCoinPrice(currencyTo);
            convertToTextBox.Text = $"{(convertValue * convetFromPrice) / convetToPrice}";
        }
    }
}
