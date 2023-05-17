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
    /// Interaction logic for SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        private readonly ICryptoAPI _cryptoAPI;
        public SearchPage(ICryptoAPI cryptoAPI)
        {
            _cryptoAPI = cryptoAPI;
            InitializeComponent();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = searchTextBox.Text;

            var cryptoCurrencies = await _cryptoAPI.GetCryptoCurrencies(searchText);

            cryptoListBox.ItemsSource = cryptoCurrencies;
        }
    }
}
