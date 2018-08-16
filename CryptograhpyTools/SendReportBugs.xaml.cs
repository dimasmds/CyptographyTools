using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Net.Mail;

namespace CryptograhpyTools
{
    /// <summary>
    /// Interaction logic for SendReportBugs.xaml
    /// </summary>
    public partial class SendReportBugs : Page
    {
        private string attachmentPath = null;

        public SendReportBugs()
        {
            InitializeComponent();
            ((App)Application.Current).TitlePage = "Send Report Bugs";
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder SB = new StringBuilder();
                SB.Append("\n");
                SB.Append("\n");
                SB.Append("-----------Device Information------------------");
                SB.Append("\n");
                SB.Append("Operating System: " + DeviceProperties.getOSName());
                SB.Append("\n");
                SB.Append("Processor Name: " + DeviceProperties.getProcName());
                SB.Append("\n");
                SB.Append("Total Phsycal Memory: " + DeviceProperties.getTotalRAM());
                SB.Append("\n");
                SB.Append("Available Phsycal Memory: " + DeviceProperties.getAvailableRAM());
                SB.Append("\n");
                SB.Append("Graphic Card: " + DeviceProperties.getGraphicCardName());
                SB.Append("\n");
                SB.Append("Resolution: " + DeviceProperties.getCurrentResolution());
                MailMessage mail = new MailMessage();
                
                mail.From = new MailAddress(@"dimasnotmastah@gmail.com", "Dimas Saputra");
                mail.To.Add(@"dimas.maulanads@gmail.com");
                mail.Subject = "[Report Bugs] Cryptotools " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                mail.Body = TextBoxDescription.Text + SB.ToString();

                if (!string.IsNullOrWhiteSpace(attachmentPath))
                {
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(attachmentPath);
                    mail.Attachments.Add(attachment);
                }
                

                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(@"dimasnotmastah@gmail.com", "fieldofinnocence");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                MessageBox.Show("Report Sent !");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ButtonAttachment_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "Picture Files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp";
            OFD.InitialDirectory = @"C:\";
            OFD.Title = "Select Image Attachment";

            if (OFD.ShowDialog() == true)
            {
                TextBoxAttachment.Text = OFD.FileName;
                attachmentPath = OFD.FileName;
            }
            else
            {
                return;
            }
        }
    }
}
