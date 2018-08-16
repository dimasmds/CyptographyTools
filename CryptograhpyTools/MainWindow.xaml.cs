using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CryptograhpyTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyFrame.Navigate(new Uri("HomePage.xaml", UriKind.Relative));
            this.BackButton.Visibility = Visibility.Collapsed;
            this.ButtonConfig.Visibility = Visibility.Collapsed;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.GoBack();
        }

        private void MyFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (MyFrame.CanGoBack)
                BackButton.Visibility = Visibility.Visible;
            else
                BackButton.Visibility = Visibility.Collapsed;

            if (((App)Application.Current).ConfButton)
                ButtonConfig.Visibility = Visibility.Visible;
            else
                ButtonConfig.Visibility = Visibility.Collapsed;

            TextBlockTitle.Text = ((App)Application.Current).TitlePage;
        }

        private void ButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            EncryptDecryptPageConfPage nextPage = new EncryptDecryptPageConfPage();
            MyFrame.Navigate(nextPage);
        }
    }
}
