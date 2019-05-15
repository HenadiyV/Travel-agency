using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для GoWindow.xaml
    /// </summary>
    public partial class GoWindow : Window
    {
        public string connect = "";
        public GoWindow()
        {
            InitializeComponent();
        }
        public int ID_USER { set; get; }
        private string ConvertP(string s)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] checkSum1 = md5.ComputeHash(Encoding.UTF8.GetBytes(s));
            return s = BitConverter.ToString(checkSum1).Replace("-", String.Empty);
        }
        private void btGo_Click(object sender, RoutedEventArgs e)
        {
            string _pass = ConvertP(passwordBox1.Password);
            using (Model1 db = new Model1(connect))
            {
                var Usr = db.useries.Where(a => a.pass == _pass).FirstOrDefault();
                if (Usr != null)
                { 
                    ID_USER = Usr.Id;
                    ViewTur vt = new ViewTur(ID_USER);
                    vt.Show();
                    Close();
                }
                else
                {
                    var mbResult = MessageBox.Show( "Хотите зарегистрироваться ?","Вы ввели неверный пароль.", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                    if (mbResult == MessageBoxResult.Yes)

                    {
                        Registration reg = new Registration();
                        reg.ShowDialog();
                    }

                    else if (mbResult == MessageBoxResult.Cancel)

                    {

                    }

                    else if (mbResult == MessageBoxResult.No)

                    {
                        Close();

                    }


                }
            }
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myConnect.Myconnect con = new myConnect.Myconnect();
            connect = con.SearchDataBase(Directory.GetCurrentDirectory());
        }
    }
}
