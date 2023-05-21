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
        private readonly ICryptoAPIService _cryptoAPI;
        public SearchPage(ICryptoAPIService cryptoAPI)
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

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            string id = (sender as Button)?.CommandParameter?.ToString();

            if (string.IsNullOrEmpty(id))
            {
                
            }

            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            if (mainWindow == null)
            {

            }

            mainWindow.OpenDetailsPage(id);
        }
    }
}
