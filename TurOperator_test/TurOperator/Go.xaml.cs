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
    /// Логика взаимодействия для Go.xaml
    /// </summary>
    public partial class Go : Window
    {
        public static string connect = "";
        public Go()
        {
            InitializeComponent();
        }
        public int ID_User { set; get; }
        private string ConvertP(string s)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] checkSum1 = md5.ComputeHash(Encoding.UTF8.GetBytes(s));
            return s = BitConverter.ToString(checkSum1).Replace("-", String.Empty);
        }
        private void bt_Reg_Click(object sender, RoutedEventArgs e)
        {
            string pass = ConvertP(tbPass.Password);
            using (Model1 db = new Model1(connect))
            {
                var user = db.useries.Where(a => a.pass == pass).FirstOrDefault();
                if (user != null)
                { ID_User = user.Id; }
                else ID_User = -1;
            }
            Close();
            }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myConnect.Myconnect con = new myConnect.Myconnect();
            connect = con.SearchDataBase(Directory.GetCurrentDirectory()); 
        }

        private void bt_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
