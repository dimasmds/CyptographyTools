using CryptograhpyTools.Properties;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CryptograhpyTools
{
    /// <summary>
    /// Interaction logic for EncryptDecryptFilesPage.xaml
    /// </summary>
    public partial class EncryptDecryptFilesPage : Page
    {
        string[] _filesPath;
        Int64 fileSize = 0;
        string[] SizeSuffixes =
                  { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        private BackgroundWorker myBgEncrypt = new BackgroundWorker();
        private BackgroundWorker myBgDecrypt = new BackgroundWorker();

        private int successCount = 0;
        private int failedCount = 0;

        public EncryptDecryptFilesPage()
        {
            InitializeComponent();
            ((App)Application.Current).TitlePage = "Encrypt & Decrypt Files";
            ((App)Application.Current).ConfButton = false;
            EncryptProgressBar.Value = 0;

            // bgWorker encrypt Init
            myBgEncrypt.DoWork += MyBgEncrypt_DoWork;
            myBgEncrypt.ProgressChanged += MyBgEncrypt_ProgressChanged;
            myBgEncrypt.RunWorkerCompleted += MyBgEncrypt_RunWorkerCompleted;
            myBgEncrypt.WorkerReportsProgress = true;

            // bgWorker decrypt Init
            myBgDecrypt.DoWork += MyBgDecrypt_DoWork;
            myBgDecrypt.ProgressChanged += MyBgDecrypt_ProgressChanged;
            myBgDecrypt.RunWorkerCompleted += MyBgDecrypt_RunWorkerCompleted;
            myBgDecrypt.WorkerReportsProgress = true;
        }

       

        private void ButtonFile1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.InitialDirectory = @"C:\";
            switch (Settings.Default.Format_Files)
            {
                case 0:
                    OFD.Filter = "All files(*.*)|*.*";
                    break;
                case 1:
                    OFD.Filter = "Picture Format|*.jpg; *.jpeg; *.png; *.bmp; *.gif";
                    break;
                case 2:
                    OFD.Filter = "Music Format|*.mp3; *.m4a; *.wav; *.amr";
                    break;
                case 3:
                    OFD.Filter = "Video Format|*.avi; *.mp4; *.mkv; *.flv; *.3gp; *.mov";
                    break;
                default:
                    OFD.Filter = "Document Format|*.doc; *.docx; *.ppt; *.pptx; *.xls; *.xlsx; *.rtf; *.txt";
                    break;
            }
            OFD.FilterIndex = 1;
            OFD.RestoreDirectory = true;
            OFD.Multiselect = true;

            if (OFD.ShowDialog() == true)
            {
                _filesPath = OFD.FileNames;
                TextBlockNameFile.Text = "";
                fileSize = 0;
                for (int i = 0; i < _filesPath.Length; i++)
                {
                    System.IO.FileInfo FI = new System.IO.FileInfo(_filesPath[i]);
                    fileSize += FI.Length;
                }
                TextBoxFile1.Text = string.Join("|", _filesPath);
                TextBlockNameFile.Text = (OFD.FileNames.Length > 1) ? "Multiple" : System.IO.Path.GetFileNameWithoutExtension(OFD.FileName);
                TextBlockFormatFile.Text = System.IO.Path.GetExtension(OFD.FileName);
                TextBlockSizeFile.Text = SizeSuffix(fileSize);
            }
        }

        private string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return "0.0 bytes"; }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            int progressNumber = _filesPath.Length;

            if (fileSize > Convert.ToInt64(Convert.ToInt32(Settings.Default["Maximal_Size"].ToString())* 1048576))
            {
                MessageBox.Show("File Terlalu Besar");
                return;
            }
            if (string.IsNullOrWhiteSpace(PasswordBoxKey.Password))
            {
                if (MessageBox.Show("Are You sure to encrypt without key?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    return;
                }
            }
            myBgEncrypt.RunWorkerAsync();
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            myBgDecrypt.RunWorkerAsync();
        }

        private void EncryptFile(string path, string key)
        {
            byte[] plainContent = System.IO.File.ReadAllBytes(path);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider())
                {
                    tripDes.Key = keys;
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
                    using (var memStream = new System.IO.MemoryStream())
                    {
                        CryptoStream cryptoStream = new CryptoStream(memStream, tripDes.CreateEncryptor(), CryptoStreamMode.Write);

                        cryptoStream.Write(plainContent, 0, plainContent.Length);
                        cryptoStream.FlushFinalBlock();
                        System.IO.File.WriteAllBytes(path, memStream.ToArray());
                        //MessageBox.Show("Encrypt Successfully " + path, "Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                        cryptoStream.Dispose();
                        memStream.Dispose();
                        cryptoStream.Close();
                        memStream.Close();
                    }
                }
            }
        }

        private void DecryptFile(string path, string key)
        {
            byte[] encrypted = System.IO.File.ReadAllBytes(path);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                using (var tripDes = new TripleDESCryptoServiceProvider())
                {
                    tripDes.Key = keys;
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

                    using (var memStream = new System.IO.MemoryStream())
                    {
                        CryptoStream cryptoStream = new CryptoStream(memStream, tripDes.CreateDecryptor(), CryptoStreamMode.Write);

                        cryptoStream.Write(encrypted, 0, encrypted.Length);
                        cryptoStream.FlushFinalBlock();
                        System.IO.File.WriteAllBytes(path, memStream.ToArray());
                        //MessageBox.Show("Decrypted Successfully " + path, "Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                        cryptoStream.Dispose();
                        memStream.Dispose();
                        cryptoStream.Close();
                        memStream.Close();
                    }
                }
            }
        }

        //---------------**** Background Worker ***--------------------//

        private void MyBgDecrypt_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Decrypt Complete ! \nSuccess: " + successCount.ToString() + "\nFailed: " + failedCount.ToString());
            failedCount = 0;
            successCount = 0;
        }

        private void MyBgDecrypt_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            EncryptProgressBar.Value = e.ProgressPercentage;
            TextBlockProgress.Text = e.ProgressPercentage.ToString() + "%";
        }

        private void MyBgDecrypt_DoWork(object sender, DoWorkEventArgs e)
        {
                for (int i = 0; i < _filesPath.Length; i++)
                {
                    try
                    {
                        DecryptFile(_filesPath[i], PasswordBoxKey.Password);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + " Check: " + ex.HelpLink);
                        failedCount++;
                    }
                    int percentage = ((i + 1) * 100) / _filesPath.Length;
                    (sender as BackgroundWorker).ReportProgress(percentage, null);
                }
        }

        private void MyBgEncrypt_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            EncryptProgressBar.Value = e.ProgressPercentage;
            TextBlockProgress.Text = e.ProgressPercentage.ToString() + "%";
        }

        private void MyBgEncrypt_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Encrypt Complete ! \nSuccess: " + successCount.ToString() + "\nFailed: " + failedCount.ToString());
            failedCount = 0;
            successCount = 0;
        }

        private void MyBgEncrypt_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < _filesPath.Length; i++)
            {
                try
                {
                    EncryptFile(_filesPath[i], PasswordBoxKey.Password);
                    successCount++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " Check: " + ex.HelpLink);
                    failedCount++;
                }
                int percentage = ((i + 1) * 100) / _filesPath.Length;
                (sender as BackgroundWorker).ReportProgress(percentage, null);
            }
        }

        private void DropPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                TextBoxFile1.Text = "";
                _filesPath = (string[])e.Data.GetData(DataFormats.FileDrop);
                fileSize = 0;
                for (int i = 0; i < _filesPath.Length; i++)
                {
                    System.IO.FileInfo FI = new System.IO.FileInfo(_filesPath[i]);
                    fileSize += FI.Length;
                }
                TextBlockNameFile.Text = (_filesPath.Length > 1) ? "Multiple" : System.IO.Path.GetFileNameWithoutExtension(_filesPath[0]);
                TextBlockFormatFile.Text = System.IO.Path.GetExtension(_filesPath[0]);
                TextBlockSizeFile.Text = SizeSuffix(fileSize);
                TextBoxFile1.Text = string.Join("|", _filesPath);

            }
        }
    }
}
