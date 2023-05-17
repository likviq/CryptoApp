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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICryptoAPI _cryptoAPI;

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new MainPage());
        }

        public MainWindow(ICryptoAPI cryptoAPI) : this()
        {
            _cryptoAPI = cryptoAPI;
        }

        private void HomePageClick(object sender, MouseButtonEventArgs e)
        {
            MainFrame_Navigating(new MainPage());
        }

        private void SearchPageClick(object sender, MouseButtonEventArgs e)
        {
            SearchPage searchPage = new SearchPage(_cryptoAPI);
            MainFrame_Navigating(searchPage);
        }

        private void ConvertPageClick(object sender, MouseButtonEventArgs e)
        {
            MainFrame_Navigating(new ConvertPage());
        }

        private void AboutPageClick(object sender, MouseButtonEventArgs e)
        {
            MainFrame_Navigating(new AboutPage());
        }

        private void MainFrame_Navigating(Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}
