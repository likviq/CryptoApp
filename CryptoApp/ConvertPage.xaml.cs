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
        private string currencyFrom;
        private string currencyTo;
        public ConvertPage()
        {
            InitializeComponent();
        }

        private void ChangeCurrencyFrom(object sender, RoutedEventArgs e)
        {
            string currencyFrom = ((ComboBoxItem)ConvertFrom.SelectedItem).Content.ToString();
        }

        private void ChangeCurrencyTo(object sender, RoutedEventArgs e)
        {
            string currencyTo = ((ComboBoxItem)ConvertTo.SelectedItem).Content.ToString();
        }

        private void ConvertButtonClick(object sender, RoutedEventArgs e)
        {
            string convertFromValue = convertFromTextBox.Text;
            float fromValue;

            bool success = float.TryParse(convertFromValue, out fromValue);

            if (!success)
            {
                MessageBox.Show("incorrect values");
                return;
            }

            convertToTextBox.Text = convertFromValue;
        }
    }
}
