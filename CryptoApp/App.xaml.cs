using CryptoApp.Interfaces;
using CryptoApp.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CryptoApp
{
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection();

            services.AddSingleton<ICryptoAPIService, CryptoAPIService>();

            services.AddSingleton<MainWindow>(serviceProvider =>
            {
                var cryptoAPI = serviceProvider.GetRequiredService<ICryptoAPIService>();
                return new MainWindow(cryptoAPI);
            });

            var serviceProvider = services.BuildServiceProvider();

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            Application.Current.MainWindow = mainWindow;

            mainWindow.Show();
        }
    }
}

