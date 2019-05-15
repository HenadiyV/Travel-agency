using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using TurOperator;
using MahApps.Metro.Controls;
namespace TurOperator
{
    /// <summary>
    /// Логика взаимодействия для ViewTur.xaml
    /// </summary>
    public partial class ViewTur : Window
    {
        public static string connect = "";
        byte[] img = null;
        public ViewTur()
        {
            InitializeComponent();
        }
        public ViewTur(int id)
        {
            InitializeComponent();
            ID_user = id;
        }
        int ID_user = 0;       
        public void ViewMyTur()
        {
            using (Model1 db = new Model1(connect))
            {
                var viev = (from contr in db.Contries
                            join cit in db.Cities on contr.Id_contry equals cit.id_contry
                            join hot in db.Hotels on cit.Id_city equals hot.id_city
                            select new myTyr
                            {
                                _id_contry =contr.Id_contry,
                               _id_city = cit.Id_city,
                                _id_hotel = hot.Id_hotel,
                                _contry=contr.contry1,
                                _city=cit.city1,
                                _hotel = hot.hotel1,
                                _category = hot.category,
                                _coment =hot.coment,
                                _price = hot.price,
                                _linck=hot.linck,
                                _img = hot.img
                            }).ToList<myTyr>();            
                dgTur.ItemsSource = viev;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myConnect.Myconnect con = new myConnect.Myconnect();
            connect = con.SearchDataBase(Directory.GetCurrentDirectory());
            using (Model1 db = new Model1(connect))
            {
                var user = db.useries.Where(a => a.Id == ID_user).FirstOrDefault();
                if (user != null)
                { this.Title = "Здравствуйте " + user.fam + " " + user.name + " " + user.surname; }
                var contr = db.Contries.ToList();
                cb_Region.Items.Add("Страна");          
                foreach (var cn in contr)
                cb_Region.Items.Add(cn.contry1);               
            }
            cb_Region.SelectedIndex = 0;
        }          
        private void viewTur_Click(object sender, RoutedEventArgs e)
        {
            ViewMyTur();
        }
        private void cb_Region_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string contry = cb_Region.SelectedItem.ToString();     
            using (Model1 db = new Model1(connect))
            {
                var tyr = (from cn in db.Contries.Where(a => a.contry1 == contry)
                           join ct in db.Cities on cn.Id_contry equals ct.id_contry
                           join ht in db.Hotels on ct.Id_city equals ht.id_city
                           select new myTyr
                           {
                               _id_contry=cn.Id_contry,
                               _contry = cn.contry1,
                               _city = ct.city1,
                               _hotel = ht.hotel1,
                               _category = ht.category,
                               _coment = ht.coment,
                               _price = ht.price,
                               _linck = ht.linck,
                               _img = ht.img
                           }).ToList<myTyr>();               
                int aa = 0;
                foreach (myTyr cnt in tyr)
                { aa = cnt._id_contry; }
                    var cit = db.Cities.Where(a => a.id_contry == aa).ToList();             
                dgTur.ItemsSource =tyr ;

            }
        }
        public static BitmapImage ConvertByteArrayToBitmapImage(Byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }
        private void dgTur_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgTur.SelectedItems.Count > 0)
            {
                for (int i = 0; i < dgTur.SelectedItems.Count; i++)
                {
                    myTyr tyrOrder = dgTur.SelectedItems[i] as myTyr;

                    tbContry.Text= tyrOrder._contry;
                    tbCity.Text = tyrOrder._city;
                    tbHotel.Text = tyrOrder._hotel;
                    tbPrice.Text = tyrOrder._price;
                    img = tyrOrder._img;
                    img_preview.Source =ConvertByteArrayToBitmapImage(tyrOrder._img);
                }
            }
        }
        private void bt_AddTur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var timespan =(dEnd.SelectedDate.Value - dStart.SelectedDate.Value ).TotalDays;
            if (timespan > 0)
            {
            string _start= dStart.SelectedDate.Value.ToShortDateString(); 
           string _end= dEnd.SelectedDate.Value.ToShortDateString();
                    if (ID_user == 0)
                    { 
                Go go = new Go();
                go.Owner = this;
                go.ShowDialog();
                ID_user = go.ID_User;
                    }
                if (ID_user > 0)
                {

                    Report r = new Report(tbContry.Text, tbCity.Text, tbHotel.Text, dStart.Text, dEnd.Text, img, ID_user,tbPrice.Text);
                    // r.Owner = this;
                    r.ShowDialog();
                }
                else
                { MessageBox.Show(" Вам нужно зарегистрироваться."); }
            }
            else MessageBox.Show("Выберите дату.");
           } catch (InvalidOperationException) { MessageBox.Show("Выберите дату."); }
        }
        public static byte[] ConvertBitmapSourceToByteArray(ImageSource imageSource)
        {
            var image = imageSource as BitmapSource;
            byte[] data;
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }
        private void bt_Reg_Click(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration();
            reg.Owner = this;
            reg.ShowDialog();
        }
    }
}
