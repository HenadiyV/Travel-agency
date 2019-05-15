using System;
using System.Collections.Generic;
using System.IO;
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
using System.Net.Http;
using System.Xml;

using System.Data.SqlClient;


using System.Windows.Threading;

namespace TurOperator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DoubleAnimation imageAnimation;
        FileInfo[] files;
        int currentIndexImage;
        bool stop_animation = false;
        public static string connect = "";

        DirectoryInfo dir = new DirectoryInfo("Kurorti_World");//temp
        public delegate bool MyOpenBaza ();
        int id_user = 0;
        public MainWindow()
        {
            InitializeComponent();
            imageAnimation = new DoubleAnimation(1.0, new Duration(TimeSpan.FromSeconds(1)));
            imageAnimation.AutoReverse = true;
           imageAnimation.Completed += new EventHandler(imageAnimation_Completed);
            
        }
        #region Animation
        void imageAnimation_Completed(object sender, EventArgs e)
        {if (stop_animation == false)
            {
            if ((++currentIndexImage) < files.Length)
            {
                showImage.Source = new BitmapImage(new Uri(files[currentIndexImage].FullName));
                showImage.BeginAnimation(OpacityProperty, imageAnimation);

            }
            
                if (currentIndexImage + 1 == files.Length) currentIndexImage = -1;
            }
        }
        public void MySlaidAnimation(string _folder, int pos)
        {
            if (_folder !=null)
            {
                currentIndexImage = pos;
                DirectoryInfo dir = new DirectoryInfo(_folder);
                files = dir.GetFiles("*.jpg");
                if (files.Length > 0 && pos <= files.Length)
                {
                    showImage.Source = new BitmapImage(new Uri(files[currentIndexImage].FullName));
                    showImage.BeginAnimation(OpacityProperty, imageAnimation);
                }
                else pos = 0;
                
            }
        }
        private void showImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (stop_animation)
            {
                myPanel.Width = 0;
                myPanel.Visibility = Visibility.Hidden;
                stop_animation = false;
                imageAnimation.Completed += new EventHandler(imageAnimation_Completed);
                MySlaidAnimation(dir.FullName, currentIndexImage);
            }
            else
            {
                stop_animation = true;
                myPanel.Visibility = Visibility.Visible;
                myPanel.Width = 100;
            }
        }
        #endregion
        int a = 0;
        int b= 0;
        bool a1 = true;
        bool a2 = false;
        public void AnimText()
        { string stroka = "Подождите идет загрузка базы . . . ";
            if (a1) { 
                if(a>=stroka.Length)
                { a2 = true;
                    a1 = false;
                    b = 0;
                }else
            if (a < stroka.Length)
            {
                tbAnim.Text += stroka[a].ToString();
                a++;
            }
            }
           if(a2)
            {
                if(b >= stroka.Length)
                {
                    a1 = true;
                    a2 = false;
                    a = 0;
                }else
                if(b<= stroka.Length) {
                    string s = stroka.Remove(0, b);
                    b++; 
                tbAnim.Text =s;
                }
            }
        }       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer tm = new DispatcherTimer();
            tm.Interval = new TimeSpan(0, 0, 0, 0, 70);
            tm.Tick += (s, ea) => {AnimText();  };//вместо этого отсылаем там куда что нужно
            tm.Start();
            myConnect.Myconnect con = new myConnect.Myconnect();
         connect = con.SearchDataBase(Directory.GetCurrentDirectory());
            btOpen.Visibility = Visibility.Hidden;
            myPanel.Width = 0;
            // DisplayFolder(dir.FullName);
            MySlaidAnimation(dir.FullName,0);
          // inet= CheckForInternetConnection();
            panelGo.Visibility = Visibility.Visible;
           btOpenVisible();
           
        }
     async   public void btOpenVisible()
        {
            if ( await   MyCount())
            {         
            btOpen.Visibility = Visibility.Visible;
            panelGo.Visibility = Visibility.Hidden;
            }
            else {
                panelGo.Visibility = Visibility.Hidden;
                System.Windows.MessageBox.Show("База не подключена."); }
        }
     public static Task< bool> MyCount()
        {
            return Task.Run(() => {
                try
                {
                    using (Model1 db = new Model1(connect))
                    {
                        var contry = db.Contries.Count();

                        return  true;

                    }
                }
                catch (SqlException)
                {
                   // MessageBox.Show("База не подключена.");
                   return false;
                }
            });
        }
       
        private void btOpen_Click(object sender, RoutedEventArgs e)
        { 
            stop_animation = true;
            if (id_user != 0)
            {
                ViewTur vt = new ViewTur(id_user);
                vt.Owner = this;
                vt.ShowDialog();
            }
            else { 
            ViewTur vt1 = new ViewTur();
            vt1.Owner = this;
            vt1.ShowDialog();
            }
        }

        private void btGo_Click(object sender, RoutedEventArgs e)
        {
            GoWindow gw = new GoWindow();
            gw.Owner = this;
            gw.ShowDialog();
            id_user = gw.ID_USER;
        }

        private void btGo1_Click(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration();
           reg.Owner = this;
            reg.ShowDialog();
        }
    }
}
