using System;
using System.Windows;
using System.Windows.Controls;
using CryptograhpyTools.Properties;

namespace CryptograhpyTools
{
    /// <summary>
    /// Interaction logic for EncryptDecryptPageConfPage.xaml
    /// </summary>
    public partial class EncryptDecryptPageConfPage : Page
    {
        public EncryptDecryptPageConfPage()
        {
            InitializeComponent();
            populateComboBoxItems();
            LoadSetting();
            TextBlockSize.Text = SliderMaxSize.Value.ToString() + " MB";
            ((App)Application.Current).TitlePage = "Crypto Configuration";
            ((App)Application.Current).ConfButton = false;
        }

        private void LoadSetting()
        {
            ComboBoxCipherMode.SelectedIndex = Int32.Parse(Settings.Default["Cipher_Mode"].ToString());
            ComboBoxFormatFiles.SelectedIndex = Int32.Parse(Settings.Default["Format_Files"].ToString());
            ComboBoxPaddingMode.SelectedIndex = Int32.Parse(Settings.Default["Padding_Mode"].ToString());
            SliderMaxSize.Value = Int32.Parse(Settings.Default["Maximal_Size"].ToString());
        }

        private void populateComboBoxItems()
        {
            ComboBoxCipherMode.Items.Add("ECB (Electronic Codebook)");
            ComboBoxCipherMode.Items.Add("CFB (Chipertext Feedback)");
            ComboBoxCipherMode.Items.Add("CBC (Cipher Blok Chaining)");

            ComboBoxPaddingMode.Items.Add("PKCS7");
            ComboBoxPaddingMode.Items.Add("ANSIX923");
            ComboBoxPaddingMode.Items.Add("ISO10126");
            ComboBoxPaddingMode.Items.Add("Zeros");

            ComboBoxFormatFiles.Items.Add("All Format");
            ComboBoxFormatFiles.Items.Add("Pictrue Format");
            ComboBoxFormatFiles.Items.Add("Music Format");
            ComboBoxFormatFiles.Items.Add("Video Format");
            ComboBoxFormatFiles.Items.Add("Document Format");


            ComboBoxCipherMode.SelectedIndex = 0;
            ComboBoxFormatFiles.SelectedIndex = 0;
            ComboBoxPaddingMode.SelectedIndex = 0;
        }

        private void SliderMaxSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                TextBlockSize.Text = SliderMaxSize.Value.ToString() + " MB";
            }
            catch (Exception)
            {
                // Hold Error
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dlg = MessageBox.Show("Sure to Save? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dlg == MessageBoxResult.Yes)
            {
                Settings.Default["Cipher_Mode"] = ComboBoxCipherMode.SelectedIndex;
                Settings.Default["Padding_Mode"] = ComboBoxPaddingMode.SelectedIndex;
                Settings.Default["Format_Files"] = ComboBoxFormatFiles.SelectedIndex;
                Settings.Default["Maximal_Size"] = SliderMaxSize.Value.ToString();
                Settings.Default.Save();
            }
            else
                return;
        }
    }
}