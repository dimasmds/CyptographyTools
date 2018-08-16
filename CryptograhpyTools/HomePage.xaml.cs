using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CryptograhpyTools
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            ((App)Application.Current).TitlePage = "Cryptography Tools and Learn";
            ((App)Application.Current).ConfButton = false;

        }

        private void ButtonTools_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("EncryptDecryptPage.xaml", UriKind.Relative));
        }

        private void ButtonLearns_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("LearnCryptoPage.xaml", UriKind.Relative));
        }

        private void ButtonBugs_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("SendReportBugs.xaml", UriKind.Relative));
        }
    }
}
