using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CryptograhpyTools.Properties;
using System.Security.Cryptography;

namespace CryptograhpyTools
{
    /// <summary>
    /// Interaction logic for EncryptDecryptTextPage.xaml
    /// </summary>
    public partial class EncryptDecryptTextPage : Page
    {
        public EncryptDecryptTextPage()
        {
            InitializeComponent();
            ((App)Application.Current).TitlePage = "Encrypt & Decrypt Texts";
            ((App)Application.Current).ConfButton = false;
        }

        private void TextBoxChiperText_GotFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(TextBoxChiperText.Text) || TextBoxChiperText.Text == "Chiper text here !")
            {
                TextBoxChiperText.Text = "";
                TextBoxChiperText.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TextBoxChiperText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(TextBoxChiperText.Text))
            {
                TextBoxChiperText.Text = "Chiper text here !";
                TextBoxChiperText.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
        }

        private void TextBoxPlainText_GotFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(TextBoxPlainText.Text) || TextBoxPlainText.Text == "Plain text here !")
            {
                TextBoxPlainText.Text = "";
                TextBoxPlainText.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TextBoxPlainText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(TextBoxPlainText.Text))
            {
                TextBoxPlainText.Text = "Plain text here !";
                TextBoxPlainText.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
        }

        private void ButtonEncrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBoxChiperText.Text = EncryptText(TextBoxPlainText.Text, PasswordBoxKey.Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            TextBoxChiperText.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void ButtonDecrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBoxPlainText.Text = DecryptText(TextBoxChiperText.Text, PasswordBoxKey.Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            TextBoxPlainText.Foreground = new SolidColorBrush(Colors.Black);
        }

        private string EncryptText(string text, string password)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(text);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(password));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider()
                {
                    Key = keys,
                })
                {
                    switch ((Settings.Default.Cipher_Mode))
                    {
                        case 0:
                            tripDes.Mode = CipherMode.ECB;
                            break;

                        case 1:
                            tripDes.Mode = CipherMode.CFB;
                            break;

                        case 2:
                            tripDes.Mode = CipherMode.CBC;
                            break;

                        case 3:
                            tripDes.Mode = CipherMode.CTS;
                            break;

                        default:
                            tripDes.Mode = CipherMode.OFB;
                            break;
                    }
                    switch (Settings.Default.Padding_Mode)
                    {
                        case 0:
                            tripDes.Padding = PaddingMode.PKCS7;
                            break;
                        case 1:
                            tripDes.Padding = PaddingMode.ANSIX923;
                            break;
                        case 2:
                            tripDes.Padding = PaddingMode.ISO10126;
                            break;
                        default:
                            tripDes.Padding = PaddingMode.Zeros;
                            break;
                    }
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(results);
                }
            }
        }

        private string DecryptText(string value, string password)
        {
            byte[] data = Convert.FromBase64String(value);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(password));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider()
                {
                    Key = keys,
                })
                {
                    switch ((Settings.Default.Cipher_Mode))
                    {
                        case 0:
                            tripDes.Mode = CipherMode.ECB;
                            break;

                        case 1:
                            tripDes.Mode = CipherMode.CFB;
                            break;

                        case 2:
                            tripDes.Mode = CipherMode.CBC;
                            break;

                        case 3:
                            tripDes.Mode = CipherMode.CTS;
                            break;

                        default:
                            tripDes.Mode = CipherMode.OFB;
                            break;
                    }
                    switch (Settings.Default.Padding_Mode)
                    {
                        case 0:
                            tripDes.Padding = PaddingMode.PKCS7;
                            break;
                        case 1:
                            tripDes.Padding = PaddingMode.ANSIX923;
                            break;
                        case 2:
                            tripDes.Padding = PaddingMode.ISO10126;
                            break;
                        default:
                            tripDes.Padding = PaddingMode.Zeros;
                            break;
                    }
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    return UTF8Encoding.UTF8.GetString(results);
                }
            }
        }
    }
}