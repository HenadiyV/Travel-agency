using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public static string connect = "";    
        public Registration()
        {
            InitializeComponent();
        }
        private string ConvertP(string s)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] checkSum1 = md5.ComputeHash(Encoding.UTF8.GetBytes(s));
            return s = BitConverter.ToString(checkSum1).Replace("-", String.Empty);
        }
        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ValidateUs(ValidateUser us)
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var context = new ValidationContext(us);
            if (!Validator.TryValidateObject(us, context, results, true))
            {
                foreach (var error in results)
                {
                    errUser.Add(error.ErrorMessage);
                }
            }
            else
            {
                // MessageBox.Show("");
            }
        }
        List<string> errUser = new List<string>();
        private void btRegistration_Click(object sender, RoutedEventArgs e)
        {
            errUser.Clear();               
            ValidateUser us = new ValidateUser();
            us.fam = tb_Fam.Text;
            us.name = tb_Name.Text;
            us.surname = tb_SurName.Text;
            us.pfone = tb_Tel.Text;
            us.emaill = tb_Emaill.Text;
            ValidateUs(us);
            if (errUser.Count > 0)
            {
                for (int i = 0; i < errUser.Count; i++)
                   MessageBox.Show( errUser[i]);
            }
            else
            {
                try { 
                string _pass = ConvertP(tb_Pass.Password);
                    usery _us = new usery { fam = tb_Fam.Text, name = tb_Name.Text, surname = tb_SurName.Text, tel = tb_Tel.Text, emaill = tb_Emaill.Text, pass = _pass };
                using (Model1 db = new Model1(connect))
                    {

                        db.useries.Add(_us);
                        db.SaveChanges();
                    }
                    MessageBox.Show("Вы зарегистрированны.");
                Close();
                } catch { }
            }            
            }        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myConnect.Myconnect con = new myConnect.Myconnect();
            connect = con.SearchDataBase(Directory.GetCurrentDirectory());
        }
    }
    }
