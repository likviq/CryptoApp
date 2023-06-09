﻿using CryptoApp.Interfaces;
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
        private readonly ICryptoAPIService _cryptoAPI;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(ICryptoAPIService cryptoAPI) : this()
        {
            _cryptoAPI = cryptoAPI;
            OpenMainPage();
        }

        public void OpenMainPage()
        {
            MainPage mainPage = new MainPage(_cryptoAPI);
            MainFrame_Navigating(mainPage);
        }

        public void OpenDetailsPage(string id)
        {
            DetailsPage detailsPage = new DetailsPage(_cryptoAPI, id);
            MainFrame_Navigating(detailsPage);
        }

        private void HomePageClick(object sender, MouseButtonEventArgs e)
        {
            OpenMainPage();
        }

        private void SearchPageClick(object sender, MouseButtonEventArgs e)
        {
            SearchPage searchPage = new SearchPage(_cryptoAPI);
            MainFrame_Navigating(searchPage);
        }

        private void ConvertPageClick(object sender, MouseButtonEventArgs e)
        {
            ConvertPage convertPage = new ConvertPage(_cryptoAPI);
            MainFrame_Navigating(convertPage);
        }

        private void AboutPageClick(object sender, MouseButtonEventArgs e)
        {
            MainFrame_Navigating(new AboutPage());
        }

        private void MainFrame_Navigating(Page page)
        {
            MainFrame.Navigate(page);
        }

        private void ThemeToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ResourceDictionary lightTheme = new ResourceDictionary();
            lightTheme.Source = new Uri("Resources/Themes/LightTheme.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Add(lightTheme);
        }

        private void ThemeToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ResourceDictionary darkTheme = new ResourceDictionary();
            darkTheme.Source = new Uri("Resources/Themes/DarkTheme.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Add(darkTheme);
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = LanguageComboBox.SelectedItem as ComboBoxItem;
            string languageTag = selectedItem.Tag.ToString();

            string resourceFileName = $"Resources/Localization/MainWindow.{languageTag}.xaml";
            Uri resourceUri = new Uri(resourceFileName, UriKind.Relative);

            ResourceDictionary languageResources = new ResourceDictionary();
            languageResources.Source = resourceUri;

            Resources.MergedDictionaries.Clear();
            Resources.MergedDictionaries.Add(languageResources);

            UpdateLocalization();
        }

        private void UpdateLocalization()
        {
            HomeTextBlock.Text = (string)Resources["Home"];
            SearchTextBlock.Text = (string)Resources["Search"];
            ConvertTextBlock.Text = (string)Resources["Convert"];
            AboutTextBlock.Text = (string)Resources["About"];
            ThemeTextBlock.Text = (string)Resources["Theme change"];
        }
    }
}
