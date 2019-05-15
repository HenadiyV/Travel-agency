using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace Tur_Admin
{
    public partial class frmParser : Form
    {
        private static string path = Directory.GetCurrentDirectory() + "\\Temp\\";
        private static string path_parse = "";
        private static string path_notImage = Directory.GetCurrentDirectory() + "\\image\\no_photo.jpg";
        public string path_new { set; get; }
        Dictionary<string, string> hostLinck = new Dictionary<string, string>();
        public frmParser()
        {
            InitializeComponent();
        }
        private void frmParser_Load(object sender, EventArgs e)
        {
            AddHostLinck();
        }
        List<MyParse> dataMyParse = new List<MyParse>();
        public void AddHostLinck()
        {
            hostLinck.Add("Выберрите страну ", "about:blank");
            hostLinck.Add("Австрия", "https://www.tez-tour.com/ru/dp/catalog/austria/hotels.html");
            hostLinck.Add("Андорра", "https://www.tez-tour.com/ru/dp/catalog/andorra/hotels.html");
            hostLinck.Add("Беларусь", "https://www.tez-tour.com/ru/dp/catalog/belarus/hotels.html");
            hostLinck.Add("Болгария", "https://www.tez-tour.com/ru/dp/catalog/bulgaria/hotels.html");
            hostLinck.Add("Венгрия", "https://www.tez-tour.com/ru/dp/catalog/hungary/hotels.html");
            hostLinck.Add("Греция", "https://www.tez-tour.com/ru/dp/catalog/greece/hotels.html");
            hostLinck.Add("Грузия", "https://www.tez-tour.com/ru/dp/catalog/georgia/hotels.html");
            hostLinck.Add("Доминикана", "https://www.tez-tour.com/ru/dp/catalog/dominicana/hotels.html");
            hostLinck.Add("Египет", "https://www.tez-tour.com/ru/dp/catalog/egypt/hotels.html");
            hostLinck.Add("Индонезия", "https://www.tez-tour.com/ru/dp/catalog/indonesia/hotels.html");
            hostLinck.Add("Испания", "https://www.tez-tour.com/ru/dp/catalog/spain/hotels.html");
            hostLinck.Add("Италия", "https://www.tez-tour.com/ru/dp/catalog/italy/hotels.html");
            hostLinck.Add("Кипр", "https://www.tez-tour.com/ru/dp/catalog/cyprus/hotels.html");
            hostLinck.Add("Куба", "https://www.tez-tour.com/ru/dp/catalog/cuba/hotels.html");
            hostLinck.Add("Латвия", "https://www.tez-tour.com/ru/dp/catalog/latvia/hotels.html");
            hostLinck.Add("Литва", "https://www.tez-tour.com/ru/dp/catalog/lithuania/hotels.html");
            hostLinck.Add("Маврикий", "https://www.tez-tour.com/ru/dp/catalog/mauritius/hotels.html");
            hostLinck.Add("Мальдивы", "https://www.tez-tour.com/ru/dp/catalog/maldives/hotels.html");
            hostLinck.Add("Мексика", "https://www.tez-tour.com/ru/dp/catalog/mexico/hotels.html");
            hostLinck.Add("ОАЭ", "https://www.tez-tour.com/ru/dp/catalog/uae/hotels.html");
            hostLinck.Add("Португалия", "https://www.tez-tour.com/ru/dp/catalog/portugal/hotels.html");
            hostLinck.Add("Сейшелы", "https://www.tez-tour.com/ru/dp/catalog/seychelles/hotels.html");
            hostLinck.Add("Таиланд", "https://www.tez-tour.com/ru/dp/catalog/thailand/hotels.html");
            hostLinck.Add("Турция", "https://www.tez-tour.com/ru/dp/catalog/turkey/hotels.html");
            hostLinck.Add("Франция", "https://www.tez-tour.com/ru/dp/catalog/france/hotels.html");
            hostLinck.Add("Чехия", "https://www.tez-tour.com/ru/dp/catalog/czech/hotels.html");
            hostLinck.Add("Шри-Ланка", "https://www.tez-tour.com/ru/dp/catalog/sri-lanka/hotels.html");
            hostLinck.Add("Эстония", "https://www.tez-tour.com/ru/dp/catalog/estonia/hotels.html");
            comboBox1.DataSource = new BindingSource(hostLinck, null);
            comboBox1.DisplayMember = "Key";
            comboBox1.ValueMember = "Value";

        }
        public string CreateNewDirectory(string nameDir)
        {
            string newDir = "";
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                newDir = dir.CreateSubdirectory(nameDir).FullName;
                return newDir;
            }
            catch (ArgumentException)
            {
                MessageBox.Show(" Неудалось создать директорию.");
                return null;
            }
        }
        string key = "";
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            key = ((KeyValuePair<string, string>)comboBox1.SelectedItem).Key;
            string value = ((KeyValuePair<string, string>)comboBox1.SelectedItem).Value;

            textBox1.Text = value;
        }
        public string My(string URL)
        {

            try
            {

                webControl1.Source = new Uri(URL);
                while (webControl1.IsLoading)
                {
                    Application.DoEvents();
                }
                return webControl1.ExecuteJavascriptWithResult("document.documentElement.outerHTML").ToString();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show(" Перегрузите страницу.");
                return null;
            }
        }       
        int a = 0;
       
        byte[] imageByte = null;
        public void pr()
        {
            string s_img = "";
            string contry_ = "";
            string city_ = "";
            string linck_ = "";
            string category_ = "";
            string hotel_ = "";
            string coment_ = "";
            string price_ = "";
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            string _html = richTextBox1.Text;
            doc.LoadHtml(_html);
            path_parse = CreateNewDirectory(comboBox1.Text) + "\\";
            var htmlNodes = doc.DocumentNode.SelectNodes("//table/tbody/tr/td");
            foreach (var h in htmlNodes)
            {
                if (h.OuterHtml.Contains("hotel-list-td1"))
                {
                    s_img = h.OuterHtml;

                    if (s_img == "") MessageBox.Show("No");
                    s_img = s_img.Substring(s_img.IndexOf("(") + 1);
                    s_img = s_img.Remove(s_img.IndexOf(")"));
                    if (s_img.Contains("small") == false)
                        s_img = path_notImage;
                }
                if (h.OuterHtml.Contains("hotel-list-td2"))
                {
                    int one = 0;
                    int one_end = 0;
                    int two = 0;
                    int two_end = 0;
                    int tree = 0;
                    int tree_end = 0;
                    string stroca = h.OuterHtml;
                    one = stroca.IndexOf("<a");
                    one_end = stroca.IndexOf("</a");
                    two = stroca.IndexOf("<a", one_end);
                    two_end = stroca.IndexOf("</a", two);
                    tree = stroca.IndexOf("<a", two_end);
                    tree_end = stroca.IndexOf("</a", tree);
                    string value = textBox1.Text;
                    string tem_value = value.Remove(value.IndexOf("/ru"));
                    string tem_links = stroca.Substring(one, (one_end - one));
                    tem_links = tem_links.Substring(tem_links.IndexOf("href=") + 6);
                    tem_links = tem_links.Remove(tem_links.LastIndexOf("target") - 2);
                    tem_value = tem_value + tem_links;
                    linck_ = tem_value;
                    string hot = stroca.Substring(one, (one_end - one));
                    hot = hot.Substring(hot.IndexOf(">") + 1);
                    hot = hot.Remove(hot.IndexOf("<"));
                    hotel_ = hot;
                    string cat = stroca.Substring(one, (one_end - one));
                    cat = cat.Substring(cat.IndexOf("<i>"));
                    cat = cat.Remove(cat.LastIndexOf("</i>"));
                    cat = cat.Substring(cat.IndexOf(">") + 1);
                    category_ = cat;
                    string cntr = stroca.Substring(two, (two_end - two));
                    cntr = cntr.Substring(cntr.LastIndexOf(">") + 1);
                    contry_ = cntr;
                    string cit = stroca.Substring(tree, (tree_end - tree));
                    cit = cit.Substring(cit.LastIndexOf(">") + 1);
                    city_ = cit;
                }
                if (s_img != "" && hotel_ != "")
                {
                    WebClient wc = new WebClient();
                    try
                    {
                        imageByte = wc.DownloadData(s_img);
                    }
                    catch (Exception)
                    {
                        imageByte = wc.DownloadData(path_notImage);
                    }
                }
                if (h.OuterHtml.Contains("hotel-list-td3"))
                {
                    coment_ = h.InnerText;
                }
                if (h.OuterHtml.Contains("hotel-list-td4"))
                {
                    price_ = h.InnerText;
                }
                if (category_ != "" && city_ != "" && contry_ != "" && linck_ != "" && hotel_ != "" && price_ != "")
                {
                    MyParse pars = new MyParse { category = category_, city = city_, coment = coment_, contry = contry_, linck_hotel = linck_, name = hotel_, price = price_, img = imageByte };
                    dataMyParse.Add(pars);
                    s_img = "";
                    contry_ = "";
                    city_ = "";
                    linck_ = "";
                    category_ = "";
                    hotel_ = "";
                    coment_ = "";
                    price_ = "";
                    imageByte = null;
                }
            }
            MessageBox.Show("Данные получены.");
            btSave.Enabled = true;
        }
        private void SerializationMy(List<MyParse> us, string dir)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(dir, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, us);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
            }
        }
        private void btGo_Click(object sender, EventArgs e)
        {
            timer1.Start();
            richTextBox1.Clear();
            pictureBox1.Visible = true;
            dataMyParse.Clear();
            richTextBox1.Text = My(textBox1.Text);
            if (richTextBox1.Text.Contains("hotel-list-region"))
            {
                pr();
                //Thread.Sleep(500);
                pictureBox1.Visible = false;
              
                btSave.Enabled = true;
                a = 0;
                timer1.Stop();                
            }
            else Thread.Sleep(500);
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            string sav = path + comboBox1.Text + "\\";
            string reg1 = "";
            reg1 = comboBox1.Text;
            string dir1 = sav + reg1 + ".dat";
            SerializationMy(dataMyParse, dir1);
            try { } catch (ArgumentOutOfRangeException) { }
            MessageBox.Show("Данные сохранены.");
            path_parse = dir1;
            Close();
        }
        private void frmParser_FormClosed(object sender, FormClosedEventArgs e)
        {
            path_new = path_parse;
            webControl1.Dispose();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {if (textBox1.Text != "") btGo.Enabled = true;
            else btGo.Enabled = false;
            string dr = path + key;
            DirectoryInfo dir = new DirectoryInfo(dr);
            if (dir.Exists)
            {
                var mbResult = MessageBox.Show("Каталог - " + key + " уже существует. Для продолжения работы он  будет удален. Продолжить?", "Предупреждение.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (mbResult == DialogResult.Yes)
                {
                    try
                    {
                        Directory.Delete(path + key, true);
                    }
                    catch (Exception) { MessageBox.Show("Ошибка удаления каталога.Каталог открыт в другой программе."); }
                }
                else if (mbResult == DialogResult.No)
                {
                    // Close();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            a++;
            if (a == 20) btGo_Click(sender, e);
        }
    }
}
 