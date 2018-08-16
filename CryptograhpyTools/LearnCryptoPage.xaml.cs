using System.Windows;
using System.Windows.Controls;

namespace CryptograhpyTools
{
    /// <summary>
    /// Interaction logic for LearnCryptoPage.xaml
    /// </summary>
    public partial class LearnCryptoPage : Page
    {
        public LearnCryptoPage()
        {
            InitializeComponent();
            ((App)Application.Current).TitlePage = "Ebooks Cyptography";

        }

        private void Lesson1_Click(object sender, RoutedEventArgs e)
        {
            var uc = new PdfUserControl(@"Lesson\Lesson1.pdf");
            this.WFHPdf.Child = uc;
        }

        private void Lesson2_Click(object sender, RoutedEventArgs e)
        {
            var uc = new PdfUserControl(@"Lesson\Lesson2.pdf");
            this.WFHPdf.Child = uc;

        }

        private void Lesson3_Click(object sender, RoutedEventArgs e)
        {
            var uc = new PdfUserControl(@"Lesson\Lesson3.pdf");
            this.WFHPdf.Child = uc;
        }
    }
}
