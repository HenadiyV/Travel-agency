using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Net;
using System.Windows.Controls;

namespace TurOperator
{
    /// <summary>
    /// Логика взаимодействия для Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        public static string connect = "";
        private static string path = Directory.GetCurrentDirectory() + "\\_User";
        private static string path_user = "";
        TurReport tur = new TurReport();

        string id = "";
        int ID = 0;
        DateTime _startData;
        DateTime _endData;
        string name_user = "";
        string email_user = "";
        bool sav = false;
        public Report()
        {
            InitializeComponent();
        }
        public Report(string contry, string city, string hotel, string dataStart, string dataEnd, byte[] img, int IDuser, string price)
        {
            InitializeComponent();

            tbContry.Text = contry;
            tbCity.Text = city;
            tbHotel.Text = hotel;
            tbStartData.Text = dataStart;
            _startData = Convert.ToDateTime(dataStart);
            tbEndData.Text = dataEnd;
            _endData = Convert.ToDateTime(dataEnd);
            myImage.Source = ConvertByteArrayToBitmapImage(img);
            id = IDuser.ToString();
            tbPrice.Text = price;
            ID = IDuser;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myConnect.Myconnect con = new myConnect.Myconnect();
            connect = con.SearchDataBase(Directory.GetCurrentDirectory());
            CreateDir(id);
            using (Model1 db = new Model1(connect))
            {
                var us = db.useries.Where(a => a.Id == ID).FirstOrDefault();
                if (us != null)
                {
                    klient.Text = us.fam + " " + us.name + " " + us.surname;
                    name_user = us.name;
                    email_user = us.emaill;
                }
            }
        }


        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll")]
        static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;
        }
        public void CreateDir(string id_user)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            path_user = dir.CreateSubdirectory(id_user).FullName;

        }
        public void CreateFile(string path_us, string id_us)
        {
            string dat = DateTime.Now.ToShortDateString();
            string time = DateTime.Now.ToLongTimeString();
            dat = dat.Replace('.', '_');
            time = time.Replace(':', '_');
            string dir = path_us + "\\" + id_us + "_" + tbContry.Text + "_" + dat + "_" + time + ".jpeg";
            FileInfo fi = new FileInfo(dir);
            if (fi.Exists) return;
            FileStream fstr = fi.Create();
            fstr.Close();


            path_user = dir;/*path+ "\\" + id_us + "_" + tbContry.Text + "_" + dat + ".png"*/;
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
       
        public void SaveTur()
        {
            byte[] _img = File.ReadAllBytes(path_user);
            using (Model1 db = new Model1(connect))
            {
                var _contry = db.Contries.Where(a => a.contry1 == tbContry.Text).FirstOrDefault();
                var _city = db.Cities.Where(a => a.city1 == tbCity.Text).FirstOrDefault();
                var _hotel = db.Hotels.Where(a => a.hotel1 == tbHotel.Text).FirstOrDefault();
                DateTime dt = DateTime.Now;
                Tur tr = new Tur { id_contry = _contry.Id_contry, id_city = _city.Id_city, id_hotel = _hotel.Id_hotel, id_user = ID, dateStart = _startData, dateEnd = _endData, reg_data = dt, img = _img };
                db.Turs.Add(tr);
                db.SaveChanges();
            }
        }
        public void Scrin()
        {
            Process ActiveProcess = Process.GetCurrentProcess();
            try
            {
                var process = Process.GetProcessesByName(ActiveProcess.ProcessName).FirstOrDefault();

                if (process != null)
                {
                    var hwnd = process.MainWindowHandle;
                    RECT rect;
                    GetWindowRect(hwnd, out rect);
                    using (var image = new Bitmap(rect.Right - rect.Left, rect.Bottom - rect.Top))
                    {
                        using (var graphics = Graphics.FromImage(image))
                        {
                            var hdcBitmap = graphics.GetHdc();
                            PrintWindow(hwnd, hdcBitmap, 0);
                            graphics.ReleaseHdc(hdcBitmap);
                        }
                        image.Save(path_user, ImageFormat.Jpeg);
                    }
                }
                else System.Windows.MessageBox.Show("Картинку неудалось сохранить");
                }
            catch (Exception) { System.Windows.MessageBox.Show("Процес неопределен."); }
        }
        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (sav==false) { 
            CreateFile(path_user, id);
            Scrin();
                SaveTur();
                sav = true;
                
               }
                
        }
        //проверка интернет соединения
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }
        private void btEmaill_Click(object sender, RoutedEventArgs e)
        {
            btSave_Click(sender, e);
            if (CheckForInternetConnection())
            {
                if (path_user != null && path_user != "")
                {
                    string tema = "Hello " + tbContry.Text;
                    MyEmaill me = new MyEmaill(name_user, email_user, tema, path_user);
                    // me.Owner = this;
                    me.ShowDialog();
                }
            }
            else MessageBox.Show(" Нет соединения с интернетом.");
        }
        #region Print
        public void print()
        {
            System.Drawing.Printing.PrintDocument Document = new System.Drawing.Printing.PrintDocument();
            Document.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(Document_PrintPage);
            PrintDialog pr_d = new PrintDialog();
            if (pr_d.ShowDialog() == true)
            {
                Document.Print();
            }
        }
        void Document_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(new Bitmap(path_user), new System.Drawing.Point(0, 0)); //Картинка на печать
        }
        //private void Print()
        //{
        //    PrintDialog dlg = new PrintDialog();
        //    if (dlg.ShowDialog() == true)
        //    {
        //        var document = doc;//CreateFlowDocument();       
        //        IDocumentPaginatorSource dp = document as IDocumentPaginatorSource;
        //        dlg.PrintDocument(dp.DocumentPaginator, "Print output"); //здесь вваливается ошибка
        //    }
        //}
        //private PrintDocument PD = new PrintDocument();
        private void btPrint_Click(object sender, RoutedEventArgs e)
        {
          // Print();
            btSave_Click(sender, e);
            print();
            //var pi = new ProcessStartInfo(path_user);
            //pi.UseShellExecute = true;
            //pi.Verb = "print";
            //PrintDocument printDoc = new PrintDocument();
            //printDoc.PrintPage += PrintPageHandler;
            //PrintDialog printDialog = new PrintDialog();
            //printDialog.Document = printDoc;
            //if (printDialog.ShowDialog() == DialogResult.OK) printDialog.Document.Print();
            //var process = System.Diagnostics.Process.Start(pi);
            //string PrinterName = "Canon MF2540";
            //PrinterSettings PS = new PrinterSettings();
            //string PrinterName =PS.InstalledPrinters
            //PS.PrinterName = PrinterName;
            //PD.PrinterSettings = PS;
            //foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            //{
            //    MessageBox.Show(printer);
            //}
        }
   
    
   
private void btExit_Click(object sender, RoutedEventArgs e)
{
    Close();
}
        #endregion
}
 }
