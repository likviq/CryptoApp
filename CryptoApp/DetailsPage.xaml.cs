using CryptoApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for DetailsPage.xaml
    /// </summary>
    public partial class DetailsPage : Page
    {
        private readonly ICryptoAPI _cryptoAPI;
        private readonly string _id;
        public DetailsPage(ICryptoAPI cryptoAPI, string id)
        {
            _cryptoAPI = cryptoAPI;
            _id = id;
            GetCoinDetails(id);
            InitializeComponent();
        }

        private async void GetCoinDetails(string id)
        {
            var coinDetails = await _cryptoAPI.GetCoinDetails(id);
            DataContext = coinDetails;
        }

        private void OpenLinkInBrowser(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                // Обробка помилки, якщо не вдалося відкрити посилання
                Console.WriteLine($"Помилка при відкритті посилання: {ex.Message}");
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            string url = e.Uri.ToString();
            OpenLinkInBrowser(url);
            e.Handled = true;
        }
    }
}
