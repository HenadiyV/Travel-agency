using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TurOperator
{
    /// <summary>
    /// Логика взаимодействия для MyEmaill.xaml
    /// </summary>
    public partial class MyEmaill : Window
    {
        public static string _file = "";
       static string _Name = "";
        static string _Email = "";
        static string _Path = "";
        string _Tema = "";
        public MyEmaill()
        {
            InitializeComponent();
        }
        public MyEmaill(string _name, string _email,string _tema, string _path)
        {
            InitializeComponent();
            _Name = _name;
            _Email = _email;
            _Tema = _tema;
            _Path = _path;
        }
        public static void SendMail(string smtpServer, string from, string password,
         string mailto, string caption, string message, string attachFile = null)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from,_Name);
                mail.To.Add(new MailAddress(mailto));
                mail.Subject = caption;
                mail.Body = message;
                
                if (!string.IsNullOrEmpty(attachFile))
                    mail.Attachments.Add(new Attachment(attachFile));
                SmtpClient client = new SmtpClient();
                client.Host = smtpServer;
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from.Split('@')[0], password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;//
                client.Send(mail);
                mail.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
        }

        private void btEmaill_Click(object sender, RoutedEventArgs e)
        {
            TextRange range = new TextRange(_message.Document.ContentStart, _message.Document.ContentEnd);
            string mes = range.Text;
          if (!String.IsNullOrEmpty(tbFrom.Text) && !String.IsNullOrEmpty(tbTo.Text))

            SendMail("smtp.gmail.com", "henadiyv@gmail.com", "84726751vge","henadiyvognev@gmail.com" ,"Hello" ,"No" , _file);// работает
          // SendMail(tbFrom.Text, tbName.Text, tbTo.Text, tbTema.Text, mes,false, _file);tbTo.TexttbTema.Textmes
            MessageBox.Show("Письмо отправленно.");
            Close();
        }

        private void btFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = _Path;
            dlg.Filter = "JPEG Files (*.jpeg|*.jpeg|PNG Files (*.png)|*.png|All Files (*.*)|*.*"; // Filter files by extension

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
_file = dlg.FileName;
               

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
             tbFrom.Text = _Email;
            tbName.Text = _Name;
            tbTema.Text = _Tema;
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
