using System.Windows;
using System.Windows.Controls;


namespace CryptograhpyTools
{
    /// <summary>
    /// Interaction logic for EncryptDecryptPage.xaml
    /// </summary>
    public partial class EncryptDecryptPage : Page
    {
        public EncryptDecryptPage()
        {
            InitializeComponent();
            ((App)Application.Current).TitlePage = "Encryption and Decryption Tools";
            ((App)Application.Current).ConfButton = true;
        }

        private void ButtonText_Click(object sender, RoutedEventArgs e)
        {
            EncryptDecryptTextPage nextPage = new EncryptDecryptTextPage();
            this.NavigationService.Navigate(nextPage);
        }

        private void ButtonFile_Click(object sender, RoutedEventArgs e)
        {
            EncryptDecryptFilesPage nextPage = new EncryptDecryptFilesPage();
            this.NavigationService.Navigate(nextPage);
        }

        private void ButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            EncryptDecryptPageConfPage nextPage = new EncryptDecryptPageConfPage();
            this.NavigationService.Navigate(nextPage);
        }
    }
}
