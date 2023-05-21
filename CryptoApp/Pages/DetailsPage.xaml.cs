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
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace CryptoApp
{
    /// <summary>
    /// Interaction logic for DetailsPage.xaml
    /// </summary>
    public partial class DetailsPage : Page
    {
        private readonly ICryptoAPIService _cryptoAPI;
        private readonly string _id;
        public DetailsPage(ICryptoAPIService cryptoAPI, string id)
        {
            _cryptoAPI = cryptoAPI;
            _id = id;
            GetCoinDetails(id);
            PlotBuild(id);
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
                Console.WriteLine($"Помилка при відкритті посилання: {ex.Message}");
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            string url = e.Uri.ToString();
            OpenLinkInBrowser(url);
            e.Handled = true;
        }

        private async void PlotBuild(string id)
        {
            var chartData = await _cryptoAPI.GetCoinPriceHistory(id);
            var (timestamps, values) = chartData.Value;

            var model = new PlotModel();
            
            var lineSeries = new LineSeries();
            for (int i = 0; i < timestamps.Count; i++)
            {
                double x = DateTimeAxis.ToDouble(new DateTime(1970, 1, 1).AddMilliseconds(timestamps[i]));
                double y = values[i];
                lineSeries.Points.Add(new DataPoint(x, y));
            }

            model.Series.Add(lineSeries);

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            model.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom });

            MyPlotView.Model = model;
        }
    }
}
