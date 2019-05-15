using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
namespace Tur_Admin
{
    public partial class FormAdmin : Form
    {
        public static string myPath = Directory.GetCurrentDirectory() + "\\Temp\\";
        public static string myPath_Resourse = "";
        public static string image_path = "";
        public static string image_path_temp = "";
        private static string path_notImage = Directory.GetCurrentDirectory() + "\\image\\no_photo.jpg";         
        public static string connect = "";
        List<MyParse> dataOpenFile = new List<MyParse>();
        Contry contry = new Contry();
        City city = new City();
        Hotel hotel = new Hotel();
        ControlerFileBaza controlerFileBase = new ControlerFileBaza();       
       int pbh =0;
        int pbw = 0;
        int pbX = 0;
        int pbY =0;
        Size StartSize;
        ControlerUser controlUser = new ControlerUser();
        public FormAdmin()
        {
            InitializeComponent();
           pbh = pbTurUserImg.Height;
            pbw = pbTurUserImg.Width;
           pbX = pbTurUserImg.Location.X;
           pbY = pbTurUserImg.Location.Y;
            StartSize = pbTurUserImg.Size;
        }
        private void FormAdmin_Load(object sender, EventArgs e)
        {
            myConnect.Myconnect con = new myConnect.Myconnect();
            
             connect= con.SearchDataBase(Directory.GetCurrentDirectory());
            try
            {
                using (Model1 db = new Model1(connect))
                {
                    var mycount = db.Hotels.Count();
                    MessageBox.Show("В базе " + mycount.ToString() + " отелей");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("База не подключена");
            }
        }       
        private void FormAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ComponentAddData())
            {
                var mbResult = MessageBox.Show("Сохранить изменения ?", "Предупреждение.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (mbResult == DialogResult.Yes)
                {
                    Update_dataOpenFile();
                    SerializationMy(dataOpenFile, OpenDir);
                }
                else if (mbResult == DialogResult.No)
                {
                    // Close();
                }
            }
        }
        #region openfile
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
        // добавить с сайта
        private void btUpdateBaza_Click(object sender, EventArgs e)
        {
            if (CheckForInternetConnection())
            {
                frmParser update = new frmParser();
                update.ShowDialog();
                myPath_Resourse = update.path_new;
                if (myPath_Resourse != "" && myPath_Resourse != null)
                {
                    dataOpenFile = DeMySerializationFoBinary(myPath_Resourse);
                    dataGridTur.DataSource = dataOpenFile;
                }
            }
            else MessageBox.Show(" Нет соединения с интернетом.");
        }
        private List<MyParse> DeMySerializationFoBinary(string dir)
        {
            try {
                List<MyParse> myList = new List<MyParse>();
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(dir, FileMode.Open))
                {
                    List<MyParse> deserilize = (List<MyParse>)formatter.Deserialize(fs);
                    foreach (MyParse p in deserilize)
                    {
                        myList.Add(p);
                    }
                }
                return myList;
            } catch (Exception)
            {
                MessageBox.Show("Невозможно открыть файл. ");
                return null;
            }
        }
        // переход по ссылке file
        private void linckFile_Click(object sender, EventArgs e)
        {
            try { 
            System.Diagnostics.Process.Start(linckFile.Text);
            } catch (Exception) { }
        }
        private void linckFile_Leave(object sender, EventArgs e)
        {
            linckFile.Cursor = Cursors.Hand;
        }
        int index_dataGridTur = -1;
        // file 
        private void dataGridTur_MouseClick(object sender, MouseEventArgs e)
        {
          try {
                tbContry.Text = dataGridTur.SelectedRows[0].Cells[0].Value.ToString();
                tbCity.Text = dataGridTur.SelectedRows[0].Cells[1].Value.ToString();
                tbHotel.Text = dataGridTur.SelectedRows[0].Cells[2].Value.ToString();
                tbCategory.Text = dataGridTur.SelectedRows[0].Cells[3].Value.ToString();
                tbComent.Text = dataGridTur.SelectedRows[0].Cells[4].Value.ToString();
                tbPrice.Text = dataGridTur.SelectedRows[0].Cells[5].Value.ToString();
                linckFile.Text = dataGridTur.SelectedRows[0].Cells[6].Value.ToString();
                tbLinck.Text = dataGridTur.SelectedRows[0].Cells[6].Value.ToString();
                ImageConverter ic = new ImageConverter();
                Image img = (Image)ic.ConvertFrom(dataGridTur.SelectedRows[0].Cells[7].Value);
                pictureBox1.Image = img;
                index_dataGridTur = dataGridTur.CurrentRow.Index;
             }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Выберите всю строку");
            }
        }
        string OpenDir = "";
        private void btOpenFile_Click(object sender, EventArgs e)
        {
            if (ComponentAddData())
            {
                var mbResult = MessageBox.Show("Сохранить изменения ?", "Предупреждение.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (mbResult == DialogResult.Yes)
                {
                    Update_dataOpenFile();
                    SerializationMy(dataOpenFile, OpenDir);
                }
                else if (mbResult == DialogResult.No)
                {
                    // Close();
                }
            }
            ClearComponent();
            OpenFileDialog op = new OpenFileDialog();

            op.InitialDirectory = myPath ;
            op.Filter= " (*.DAT;)| *.dat; | All files(*.*) | *.*";
            DialogResult result = op.ShowDialog();
            if (result == DialogResult.OK)
            {
                OpenDir = op.FileName;
                dataOpenFile = DeMySerializationFoBinary(op.FileName);

                dataGridTur.DataSource = dataOpenFile;
                dataGridFileView(dataGridTur);
                //btAddAllBase.Enabled = true;
            }
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
        // редактирование file
        public void Update_dataOpenFile()
        {
            dataOpenFile[index_dataGridTur].contry = tbContry.Text;
            dataOpenFile[index_dataGridTur].city = tbCity.Text;
            dataOpenFile[index_dataGridTur].name = tbHotel.Text;
            dataOpenFile[index_dataGridTur].category = tbCategory.Text;
            dataOpenFile[index_dataGridTur].coment = tbComent.Text;
            dataOpenFile[index_dataGridTur].price = tbPrice.Text;
            dataOpenFile[index_dataGridTur].linck_hotel = tbLinck.Text;
            if (image_path != "")
            {
                dataOpenFile[index_dataGridTur].img = File.ReadAllBytes(image_path);
            }
        }
        private void btUpdateFileTur_Click(object sender, EventArgs e)
        {     
            if (ComponentAddData()) {
                Update_dataOpenFile();
                dataGridTur.Enabled = false;
                btAddFileTur.Enabled = true;
            }
        }
        public void dataGridFileView(DataGridView dg)
        {
            dataGridTur.Columns[0].HeaderText = "Страна";
            dataGridTur.Columns[1].HeaderText = "Город";
            dataGridTur.Columns[2].HeaderText = "Отель";
            dataGridTur.Columns[3].HeaderText = "Категория";
            dataGridTur.Columns[4].HeaderText = "Описание";
            dataGridTur.Columns[5].HeaderText = "Цена";
            dataGridTur.Columns[6].HeaderText = "Ссылка";
            dataGridTur.Columns[7].HeaderText = "Картинка";
            //dataGridTur.Columns[7].HeaderText = "путь";
            //dataGridTur.Columns[8].HeaderText = "Картинка";
        }          
        public void ClearComponent()
        {
            tbCategory.Clear();
            tbCity.Clear();
            tbComent.Clear();
            tbContry.Clear();
            tbEmaill.Clear();
            tbFam.Clear();
            tbHotel.Clear();
            tbLinck.Clear();
            tbName.Clear();
            tbPass.Clear();
            tbPrice.Clear();
            tbSurname.Clear();
            tbTel.Clear();
            tbSearch.Clear();
            pictureBox1.Image = null;
            rbCategory.Checked = false;
            
            rbCity.Checked = false;
            rbContry.Checked = false;
            rbHotel.Checked = false;
           
            tbBasaCategory.Clear();
            tbBasaCity.Clear();
            tbBasaComent.Clear();
            tbBasaContry.Clear();
            tbBasaHotel.Clear();
            tbBasaLinck.Clear();
            tbBasaPrice.Clear();
            pictureBox2.Image = null;

        }
        public bool ComponentAddData()
        {
            if (!String.IsNullOrEmpty(tbContry.Text)) { 
            if (!dataOpenFile[index_dataGridTur].contry.Equals(tbContry.Text)) return true;
             if (!dataOpenFile[index_dataGridTur].city.Equals(tbCity.Text)) return true;
            if (!dataOpenFile[index_dataGridTur].name.Equals(tbHotel.Text)) return true;
             if (!dataOpenFile[index_dataGridTur].category.Equals(tbCategory.Text)) return true;
             if (!dataOpenFile[index_dataGridTur].coment.Equals(tbComent.Text)) return true;
            if (!dataOpenFile[index_dataGridTur].price.Equals(tbPrice.Text)) return true;
            if (!dataOpenFile[index_dataGridTur].linck_hotel.Equals(tbLinck.Text)) return true;
             if (image_path != "") return true;
             }
           return false;
        }
        List<string> errFile = new List<string>();
        //добавление списка в file
        private void btAddFileTur_Click(object sender, EventArgs e)
        {           
            SerializationMy(dataOpenFile, OpenDir);
            MessageBox.Show(" Изменения в файл добавлены. ");
            btAddFileTur.Enabled = false;
            dataGridTur.Enabled = true;
        }   
        public string CreateNewDirectory(string nameDir)
        {
            string newDir = "";
            DirectoryInfo dir = new DirectoryInfo(myPath);
            newDir = dir.CreateSubdirectory(nameDir).FullName;
            return newDir;
        }
        private void btUpdateImage_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            string _path_image = "";
            if (op.ShowDialog() == DialogResult.OK)
            {            
                _path_image = op.FileName.ToString();
                image_path = _path_image;
                pictureBox1.ImageLocation = _path_image;               
            }
            dataGridTur.Enabled = false;
            btUpdateImage.Enabled = false;
        }
        // удаление из файла
        private void btDeleteFileTur_Click(object sender, EventArgs e)
        {
            if(index_dataGridTur!=-1)
            {
              try {
                  var indx = dataOpenFile.Find
                    (s => s.contry == tbContry.Text && s.city == tbCity.Text
                         &&s.name==tbHotel.Text&&s.linck_hotel==linckFile.Text);
                    var index = dataOpenFile.IndexOf(indx);
                    if (index>=0)
                    {
                        var mbResult = MessageBox.Show("Удалить ?", "Предупреждение.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (mbResult == DialogResult.Yes)
                        {
                            dataOpenFile.RemoveAt(index);
                            SerializationMy(dataOpenFile, OpenDir);
                            dataOpenFile = DeMySerializationFoBinary(OpenDir);
                        }
                        else if (mbResult == DialogResult.No)
                        {
                            // Close();
                        }
                    }
                    ClearComponent();                 
                    dataGridTur.DataSource = null;
                    dataGridTur.DataSource = dataOpenFile;
                    dataGridFileView(dataGridTur);
                }
                catch(Exception)
                { MessageBox.Show(" Ошибка при удалении."); }
            }
        }
        private void btClearComponentFile_Click(object sender, EventArgs e)
        {
            tbCategory.Clear();
            tbCity.Clear();
            tbComent.Clear();
            tbContry.Clear();
            tbHotel.Clear();
            tbLinck.Clear();
            tbPrice.Clear();
            linckFile.Text = "ссылка отсутствует";
            pictureBox1.Image = null;
        }
        private void btAddAllBase_Click(object sender, EventArgs e)
        {
            if (dataOpenFile.Count()>0)
            controlerFileBase.AddFile(dataOpenFile,  connect);

        }
        #endregion file

        #region Base   
        int index_dgBasaView = -1;
        byte[] img_hotelBasa = null;
        List<string> err_bas = new List<string>();
        List<myData> myDataList = new List<myData>();
        static string image_path_Base = "";
        public bool ComponentAddBase()
        {

            if (!String.IsNullOrEmpty(tbBasaContry.Text)
                && !String.IsNullOrEmpty(tbBasaCity.Text)
                && !String.IsNullOrEmpty(tbBasaHotel.Text)
                ) { 
            if(!dgBasaView[3, index_dgBasaView].Value.ToString().Equals(tbBasaContry.Text))   return true;
            if (!dgBasaView[4, index_dgBasaView].Value.ToString().Equals(tbBasaCity.Text)) return true;
            if (!dgBasaView[5, index_dgBasaView].Value.ToString().Equals(tbBasaHotel.Text)) return true;
            if (!dgBasaView[6, index_dgBasaView].Value.ToString().Equals(tbBasaCategory.Text)) return true;
            if (!dgBasaView[7, index_dgBasaView].Value.ToString().Equals(tbBasaPrice.Text)) return true;
            if (!dgBasaView[8, index_dgBasaView].Value.ToString().Equals(tbBasaLinck.Text)) return true;
            if (!dgBasaView[9, index_dgBasaView].Value.ToString().Equals(tbBasaComent.Text)) return true;
                if (image_path_Base != "") return true;
                    }
             return false;
        }
        private void Validate(ValidAddBase bas)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(bas);
            if (!Validator.TryValidateObject(bas, context, results, true))
            {
                foreach (var error in results)
                {
                    err_bas.Add(error.ErrorMessage);
                }
            }
            else
            {
                // MessageBox.Show("");
            }
        }
        public void dataGridBazaView(DataGridView dg)
        {
            dgBasaView.Columns[0].Visible = false;
            dgBasaView.Columns[1].Visible = false;
            dgBasaView.Columns[2].Visible = false;
            dgBasaView.Columns[3].HeaderText = "Страна";
            dgBasaView.Columns[4].HeaderText = "Город";
            dgBasaView.Columns[5].HeaderText = "Отель";
            dgBasaView.Columns[6].HeaderText = "Категория";
            dgBasaView.Columns[7].HeaderText = "Описание";
            dgBasaView.Columns[8].HeaderText = "Цена";
            dgBasaView.Columns[9].HeaderText = "Ссылка";
            dgBasaView.Columns[10].HeaderText = "Картинка";
        }        
        private void btOpenDataBase_Click(object sender, EventArgs e)
        {           
            dgBasaView.DataSource = controlerFileBase.ViewBaza(connect);
            label35.Text = dgBasaView.RowCount.ToString();
            dataGridBazaView(dgBasaView);
        }
        private void btUpdateBase_Click(object sender, EventArgs e)
        {
            if (ComponentAddBase())
            {
                int id_contr = int.Parse(dgBasaView[0, index_dgBasaView].Value.ToString());
                int id_cit = int.Parse(dgBasaView[1, index_dgBasaView].Value.ToString());
                int id_hot = int.Parse(dgBasaView[2, index_dgBasaView].Value.ToString());
                byte[] img1 = null;
                if (image_path_Base != "")
                {
                    img1 = File.ReadAllBytes(image_path_Base);
                }
                else
                { img1 = (byte[])dgBasaView[10, index_dgBasaView].Value; }
                myData dat = new myData
                {
                    _id_contry = id_contr,
                    _id_city = id_cit,
                    _id_hotel = id_hot,
                    _contry = tbBasaContry.Text,
                    _city = tbBasaCity.Text,
                    _hotel = tbBasaHotel.Text,
                    _category = tbBasaCategory.Text,
                    _coment = tbBasaComent.Text,
                    _price = tbBasaPrice.Text,
                    _linck = tbBasaLinck.Text,
                    _img = img1
                };
                controlerFileBase.UpdateBaza(dat, connect);
            }
            dgBasaView.DataSource = controlerFileBase.ViewBaza(connect);
            dataGridBazaView(dgBasaView);
        }
        private void btDeleteBase_Click(object sender, EventArgs e)
        {
            int id_contr = int.Parse(dgBasaView[0, index_dgBasaView].Value.ToString());
            int id_cit = int.Parse(dgBasaView[1, index_dgBasaView].Value.ToString());
            int id_hot = int.Parse(dgBasaView[2, index_dgBasaView].Value.ToString());
            myData dat = new myData
            {
                _id_contry = id_contr,
                _id_city = id_cit,
                _id_hotel = id_hot,
            };
            controlerFileBase.DeleteBazaHotel(dat, connect);
            dgBasaView.DataSource = null;
            ClearComponent();
            dgBasaView.DataSource = controlerFileBase.ViewBaza(connect);
            dataGridBazaView(dgBasaView);
        }                
        public List<myData> SearchBaza(string s, int a, List<myData> mydataList)
        {
            List<myData> mydat1 = new List<myData>();
            string stroka = "";
            
            foreach (myData dt in mydataList)
            {
                if (a == 1) stroka = dt._contry;
                if (a == 2) stroka = dt._city;
                if (a == 3) stroka = dt._hotel;
                if (a == 4) stroka = dt._category; 

                if (stroka.ToUpper().Contains(s.ToString().ToUpper()))
                {
                    myData d = new myData();
                    d._id_contry = dt._id_contry;
                    d._id_city = dt._id_city;
                    d._id_hotel = dt._id_hotel;
                    d._contry = dt._contry;
                    d._city = dt._city;
                    d._hotel = dt._hotel;
                    d._category = dt._category;
                    d._coment = dt._coment;
                    d._price = dt._price;
                    d._linck = dt._linck;
                    d._img = (byte[])dt._img;
                    mydat1.Add(d); 
                }
               
                 }
            return mydat1;
        }       
        string ss = "";
        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back)
            {
                ss += e.KeyChar.ToString();
            }
            else
{
                if (ss.Length>=1)
             ss = ss.Substring(0, ss.Length - 1); }
               //ss = tbSearch.Text;
               
            if (rbContry.Checked)
            {
               // dgBasaView.DataSource = null;
                dgBasaView.DataSource = SearchBaza(ss, 1, myDataList);
                dataGridBazaView(dgBasaView);
            }
            if (rbCity.Checked)
            {              
                dgBasaView.DataSource = SearchBaza(ss, 2, myDataList);
                dataGridBazaView(dgBasaView);
            }
            if (rbHotel.Checked)
            {               
                dgBasaView.DataSource = SearchBaza(ss,3, myDataList);
                dataGridBazaView(dgBasaView);
            }
            if (rbCategory.Checked)
            {                
                dgBasaView.DataSource = SearchBaza(ss, 4, myDataList);
                dataGridBazaView(dgBasaView);
            }
        }
        private void rbContry_CheckedChanged(object sender, EventArgs e)
        {
            myDataList = controlerFileBase.ViewBaza(connect);
        }
        private void rbHotel_CheckedChanged(object sender, EventArgs e)
        {
            myDataList = controlerFileBase.ViewBaza(connect);
        }
        private void rbCity_CheckedChanged(object sender, EventArgs e)
        {
            myDataList = controlerFileBase.ViewBaza(connect);
        }
        private void rbCategory_CheckedChanged(object sender, EventArgs e)
        {
            myDataList = controlerFileBase.ViewBaza(connect);
        }
        private void tbSearch_TextChanged(object sender, EventArgs e)
        {           
           if (tbSearch.Text == "") ss = "";
        }
        private void btSearchTur_Click(object sender, EventArgs e)
        {
            dgBasaView.DataSource = null;
            dgBasaView.DataSource = controlerFileBase.ViewBaza(connect);
            dataGridBazaView(dgBasaView);
            ClearComponent(); 
            ss = "";
        }
        private void dgBasaView_MouseClick(object sender, MouseEventArgs e)
        {
            try {
            tbBasaContry.Text = dgBasaView.SelectedRows[0].Cells[3].Value.ToString();
            tbBasaCity.Text = dgBasaView.SelectedRows[0].Cells[4].Value.ToString();
            tbBasaHotel.Text = dgBasaView.SelectedRows[0].Cells[5].Value.ToString();
            tbBasaCategory.Text = dgBasaView.SelectedRows[0].Cells[6].Value.ToString();
            tbBasaComent.Text = dgBasaView.SelectedRows[0].Cells[7].Value.ToString();
            tbBasaPrice.Text = dgBasaView.SelectedRows[0].Cells[8].Value.ToString();
            tbBasaLinck.Text = dgBasaView.SelectedRows[0].Cells[9].Value.ToString();
            linckBasa.Text = dgBasaView.SelectedRows[0].Cells[9].Value.ToString();
               try { 
                ImageConverter ic = new ImageConverter();
            Image img = (Image)ic.ConvertFrom(dgBasaView.SelectedRows[0].Cells[10].Value);
            pictureBox2.Image = img;
                } catch (NotSupportedException) { }
            index_dgBasaView = dgBasaView.CurrentRow.Index;
             }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Выберите всю строку");
            }
        }       
        //добавление одиночной записи в базу
        private void btAddBase_Click(object sender, EventArgs e)
        {
            err_bas.Clear();
            errorProvider1.Clear();
            ValidAddBase fl = new ValidAddBase();

            fl.contry = tbBasaContry.Text;
            fl.city = tbBasaCity.Text;
            fl.hotel = tbBasaHotel.Text;
            fl.category = tbBasaCategory.Text;
            Validate(fl);
            if (err_bas.Count > 0)
            {
                for (int i = 0; i < err_bas.Count; i++)
                    errorProvider1.SetError(btAddBase, err_bas[i]);
            }
            else
            {
                string s_price = "";
                if (tbBasaPrice.Text == "")
                { s_price = "Цена по запросу."; }
                else { s_price = tbBasaPrice.Text; }
                string s_linck = "";
                if (tbBasaLinck.Text == "")
                { s_linck = "Ссылка отсутствует."; }
                else s_linck = tbBasaLinck.Text;
                string s_coment = "";
                if (tbBasaComent.Text == "")
                { s_coment = "Коментарий отсутствует."; }
                else s_coment = tbBasaComent.Text;
                if (image_path_Base != "")
                {
                    img_hotelBasa = File.ReadAllBytes(image_path_Base);
                    image_path_Base = "";
                }
                else
                {
                    img_hotelBasa = File.ReadAllBytes(path_notImage);
                }
                controlerFileBase.AddBase(tbBasaContry.Text, tbBasaCity.Text, tbBasaHotel.Text, tbBasaCategory.Text, s_coment, s_price, img_hotelBasa, s_linck, connect);
            
            }
        }
        private void btUpImageBasa_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
           
            if (op.ShowDialog() == DialogResult.OK)
            {
                image_path_Base = op.FileName.ToString();
                pictureBox2.Load(image_path_Base);
                img_hotelBasa = File.ReadAllBytes(image_path_Base);
                btUpdateBase_Click(sender, e);
            }
        }
        private void linckBasa_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(linckBasa.Text);
            }
            catch (Exception) { }
        }
        private void linckBasa_Layout(object sender, LayoutEventArgs e)
        {
            linckBasa.Cursor = Cursors.Hand;
        }
        #endregion Base

        #region User
        List<string> errUser = new List<string>();
        private void ValidateUs(ValidateUser us)
        {
            var results = new List<ValidationResult>();
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
        public void NoLeter(KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) == true) e.Handled = true;

            if ((Char)e.KeyChar == (Char)Keys.Back) e.Handled = false;

            if ((Char)e.KeyChar == (Char)Keys.Space) e.Handled = false;

            if (e.KeyChar == '.') e.Handled = false;
        }
        private void txtFam_KeyPress(object sender, KeyPressEventArgs e)
        {
            NoLeter(e);

        }
        private void textName_KeyPress(object sender, KeyPressEventArgs e)
        {
            NoLeter(e);
        }
        private void textSurName_KeyPress(object sender, KeyPressEventArgs e)
        {
            NoLeter(e);
        }     
        private void btAddUser_Click(object sender, EventArgs e)
        {
            errUser.Clear();
            errorProvider1.Clear();
            ValidateUser us = new ValidateUser();

            us.fam = tbFam.Text;
            us.name = tbName.Text;
            us.surname = tbSurname.Text;
            us.pfone = tbTel.Text;
            us.emaill = tbEmaill.Text;
            ValidateUs(us);
            if (errUser.Count > 0)
            {
                for (int i = 0; i < errUser.Count; i++)
                    errorProvider1.SetError(btAddUser, errUser[i]);
            }
            else
            {
                controlUser.AddUser(tbFam.Text, tbName.Text, tbSurname.Text, tbTel.Text, tbEmaill.Text, tbPass.Text,connect);
                ClearComponent();
                dataGridUser.DataSource = null;
                dataGridUser.DataSource = controlUser.ViewUserData(connect);
                myDatGridView();
            }      
        }
        private void tbFam_TextChanged(object sender, EventArgs e)
        {
            if(tbFam.Text!="")
            btClearUserInfo.Enabled = true;
            else
                btClearUserInfo.Enabled = false;
        }
        private void btUserList_Click(object sender, EventArgs e)
        {
            dataGridUser.DataSource = null;
              dataGridUser.DataSource = controlUser.ViewUserData(connect);
            myDatGridView();
            label37.Text = dataGridUser.RowCount.ToString();
            btClearSearchUser.Enabled = true;
            btClearUserInfo.Enabled = true;


        }
        private void btUpdateUser_Click(object sender, EventArgs e)
        {
            UserData user_ = new UserData();
            errUser.Clear();
            errorProvider1.Clear();
            ValidateUser us = new ValidateUser();

            us.fam = tbFam.Text;
            us.name = tbName.Text;
            us.surname = tbSurname.Text;
            us.pfone = tbTel.Text;
            us.emaill = tbEmaill.Text;
            ValidateUs(us);
            if (errUser.Count > 0)
            {
                for (int i = 0; i < errUser.Count; i++)
                    errorProvider1.SetError(btAddUser, errUser[i]);
            }
            else
            {
                user_._id_user = id_user_;
                user_._fam = tbFam.Text;
                user_._name = tbName.Text;
                user_._surname = tbSurname.Text;
                user_._tel = tbTel.Text;
                user_._emaill = tbEmaill.Text;

                user_._pass = tbPass.Text;
                controlUser.UpdateUser(user_,connect);
                ClearComponent();
                dataGridUser.DataSource = null;
                dataGridUser.DataSource = controlUser.ViewUserData(connect);
                myDatGridView();
                btUpdateUser.Enabled = false;
                btDeleteUser.Enabled = false;
            }

        }
        public void myDatGridView()
        {
            dataGridUser.Columns[0].Visible = false;
            dataGridUser.Columns[1].HeaderText = "Фамилия";
            dataGridUser.Columns[2].HeaderText = "Имя";
            dataGridUser.Columns[3].HeaderText = "Отчество";
            dataGridUser.Columns[4].HeaderText = "Телефон";
            dataGridUser.Columns[5].HeaderText ="Email" ;
            dataGridUser.Columns[6].HeaderText = "Пароль";
           
        }
        int id_user_ = -1;
        public void ClearComponentUserTur()
        {
            tbTurUserContry.Clear();
            tbTurUserCity.Clear();
            tbTurUserHotel.Clear();
            tbTurUserCategory.Clear();
            tbDataReg.Clear();
            LinckUserTur.Text= "ссылка отсутствует";
            tbTurUserPrice.Clear();
            tbTurUserLinck.Clear();
            pbTurUserImg.Image = null;
            dataGridUserTur.DataSource = null;
            groupBox13.Text = " Заказаные туры ";
        }
        private void dataGridUser_MouseClick(object sender, MouseEventArgs e)
        { try {
            dataGridUser.Columns[0].Visible = false;
            id_user_= int.Parse(dataGridUser.SelectedRows[0].Cells[0].Value.ToString());
            tbFam.Text=dataGridUser.SelectedRows[0].Cells[1].Value.ToString();
            tbName.Text = dataGridUser.SelectedRows[0].Cells[2].Value.ToString();
            tbSurname.Text = dataGridUser.SelectedRows[0].Cells[3].Value.ToString();
            tbTel.Text = dataGridUser.SelectedRows[0].Cells[4].Value.ToString();
            tbEmaill.Text = dataGridUser.SelectedRows[0].Cells[5].Value.ToString();
            tbPass.Text = dataGridUser.SelectedRows[0].Cells[6].Value.ToString();
            ClearComponentUserTur();
            groupBox13.Text += " " + tbFam.Text + " " + tbName.Text + " " + tbSurname.Text;
                btUpdateUser.Enabled = true;
                btDeleteUser.Enabled = true;
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Выберите всю строку");
            }
        }       
        private void btDeleteUser_Click(object sender, EventArgs e)
        {
            controlUser.DeleteUser(id_user_, connect);
            ClearComponent();
            dataGridUser.DataSource = null;
            dataGridUser.DataSource = controlUser.ViewUserData(connect);
            myDatGridView();
            btUpdateUser.Enabled = false;
            btDeleteUser.Enabled = false;
            id_user_ = 0;

        }       
        List<TurUser> _tur = new List<TurUser>();
        private void btSearchUserTur_Click(object sender, EventArgs e)
        {
            using (Model1 db = new Model1(connect))
            {
                if (rbUserData.Checked && rbDown.Checked)
                { 
                    _tur.Sort(( x,y) => y.dReg.CompareTo(x.dReg));
                    dataGridUserTur.DataSource = null;
                    dataGridUserTur.DataSource = _tur;
                   ViewdataGridUserTurHeader(dataGridUserTur);
                }else if(rbUserData.Checked && rbUp.Checked)
                {
                    _tur.Sort((y, x) => y.dReg.CompareTo(x.dReg));
                    dataGridUserTur.DataSource = null;
                    dataGridUserTur.DataSource = _tur;
                    ViewdataGridUserTurHeader(dataGridUserTur);
                }
                if (rbUserContry.Checked&& rbDown.Checked)
                {
                    _tur.Sort((x, y) => y.contry.CompareTo(x.contry));
                    dataGridUserTur.DataSource = null;
                    dataGridUserTur.DataSource = _tur;
                    ViewdataGridUserTurHeader(dataGridUserTur);
                    

                }else if (rbUserContry.Checked && rbUp.Checked)
                {
                    _tur.Sort((y,x ) => y.contry.CompareTo(x.contry));
                    dataGridUserTur.DataSource = null;
                    dataGridUserTur.DataSource = _tur;
                    ViewdataGridUserTurHeader(dataGridUserTur);                 
                }
                if (rbUserCity.Checked && rbDown.Checked)
                {
                    _tur.Sort((x, y) => y.city.CompareTo(x.city));
                    dataGridUserTur.DataSource = null;
                    dataGridUserTur.DataSource = _tur;
                    ViewdataGridUserTurHeader(dataGridUserTur);
                }
                else
                if(rbUserCity.Checked && rbUp.Checked)
                {
                    _tur.Sort((y,x ) => y.city.CompareTo(x.city));
                    dataGridUserTur.DataSource = null;
                    dataGridUserTur.DataSource = _tur;
                    ViewdataGridUserTurHeader(dataGridUserTur);
                }
                if (rbUserNameHotel.Checked && rbDown.Checked)
                {
                    _tur.Sort((x, y) => y.hotel.CompareTo(x.hotel));
                    dataGridUserTur.DataSource = null;
                    dataGridUserTur.DataSource = _tur;
                    ViewdataGridUserTurHeader(dataGridUserTur);
                }
                else
               if (rbUserNameHotel.Checked && rbUp.Checked)
                {
                    _tur.Sort((y,x ) => y.hotel.CompareTo(x.hotel));
                    dataGridUserTur.DataSource = null;
                    dataGridUserTur.DataSource = _tur;
                    ViewdataGridUserTurHeader(dataGridUserTur);
                }
            }
        }
        private void btClearUserInfo_Click(object sender, EventArgs e)
        {
            ClearComponent();
            ClearComponentUserTur();
            btUpdateUser.Enabled = false;
            btDeleteUser.Enabled = false;
            id_user_ = 0;
        }
        private void btViewUserTur_Click(object sender, EventArgs e)
        {
            _tur.Clear();
            if (id_user_ > 0)
            {
            using (Model1 db = new Model1(connect))
            {
                var usr = db.Turs.Where(a => a.id_user == id_user_).ToList();
                if (usr.Count > 0)
                {
                    foreach (var us in usr)
                    {
                        TurUser t = new TurUser();
                        t.dStart = us.dateStart.Value;
                        t.dEnd = us.dateEnd.Value;
                        var hotel = db.Hotels.Where(a => a.Id_hotel == us.id_hotel).FirstOrDefault();
                        var contry = db.Contries.Where(a => a.Id_contry == hotel.id_contry).FirstOrDefault();
                        var city = db.Cities.Where(a => a.Id_city == hotel.id_city).FirstOrDefault();
                        t.contry = contry.contry1;
                        t.city = city.city1;
                        t.hotel = hotel.hotel1;
                        t.linck = hotel.linck;
                        t.category = hotel.category;
                        t.price = hotel.price;
                        t.img = us.img;
                        t.dReg = us.reg_data.Value;
                        _tur.Add(t);
                    }
                }
            }
           if (_tur.Count > 0)
            {
                dataGridUserTur.DataSource = _tur;
                ViewdataGridUserTurHeader(dataGridUserTur);
            }
            else { MessageBox.Show(" У даного пользователя нет туров."); }
        }else { MessageBox.Show(" Выберите пользователя."); }
      
            }    
            #region turUser
            //List<TurUser> t_us = new List<TurUser>();
            //    using (Model1 db = new Model1(connect))
            //    {
            //        var us = db.Turs.Where(a => a.id_user == id_user_).FirstOrDefault();
            //        if(us!=null)
            //        {
            //            //var contry = db.Contries.Where(a => a.Id_contry == us.id_contry).ToList();
            //            var hotel = db.Hotels.Where(a => a.Id_hotel == us.id_hotel).ToList();
            //           foreach(var h in hotel) { 
            //                var contry = db.Contries.Where(a => a.Id_contry == h.id_contry).FirstOrDefault();
            //                var city = db.Cities.Where(a => a.Id_city == h.id_city).FirstOrDefault();
            //                TurUser t = new TurUser { contry = contry.contry1, city = city.city1, hotel = h.hotel1, category = h.category, img = h.img, linck = h.linck, price = h.price };
            //                t_us.Add(t);
            //            }
            //        }

            /// }

            #endregion turUser      
        public void ViewdataGridUserTurHeader(DataGridView dg)
        {
            dg.Columns[0].Visible = false;
            dg.Columns[1].Visible = false;
            dg.Columns[2].Visible = false;
            dg.Columns[3].Visible = false;
            dg.Columns[4].HeaderText = "Страна";
            dg.Columns[5].HeaderText = "Город";
            dg.Columns[6].HeaderText = "Отель";
            dg.Columns[7].HeaderText = "Категория";
            dg.Columns[8].HeaderText = "Цена";
            dg.Columns[9].Visible = false;
            dg.Columns[10].Visible = false;
            dg.Columns[11].Visible = false;
            dg.Columns[12].HeaderText = "Дата регистрации тура";
            dg.Columns[13].Visible = false;
        }
        private void dataGridUserTur_MouseClick(object sender, MouseEventArgs e)
        {
           try {  
            tbTurUserContry.Text = dataGridUserTur.SelectedRows[0].Cells[4].Value.ToString();
            tbTurUserCity.Text = dataGridUserTur.SelectedRows[0].Cells[5].Value.ToString();
            tbTurUserHotel.Text = dataGridUserTur.SelectedRows[0].Cells[6].Value.ToString();
           tbTurUserCategory.Text = dataGridUserTur.SelectedRows[0].Cells[7].Value.ToString();
            tbTurUserPrice.Text = dataGridUserTur.SelectedRows[0].Cells[8].Value.ToString();
            tbTurUserLinck.Text = dataGridUserTur.SelectedRows[0].Cells[9].Value.ToString();
            LinckUserTur.Text= dataGridUserTur.SelectedRows[0].Cells[9].Value.ToString();
            tbDataReg.Text= dataGridUserTur.SelectedRows[0].Cells[12].Value.ToString();
            ImageConverter ic = new ImageConverter();
            Image img = (Image)ic.ConvertFrom(dataGridUserTur.SelectedRows[0].Cells[13].Value);
            pbTurUserImg.Image = img;
            }catch(ArgumentOutOfRangeException)
            {
                MessageBox.Show("Выберите всю строку");
            }

        }
        private void pbTurUserImg_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            pbTurUserImg.SizeMode = PictureBoxSizeMode.StretchImage;        
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            pbTurUserImg.Width = StartSize.Width * trackBar1.Value;
            pbTurUserImg.Height = StartSize.Height * trackBar1.Value;

            // Размещаем в центре области
            pictureBox1.Left = pbX + (pbw - pbTurUserImg.Width) / 2;
            pictureBox1.Top = pbY + (pbh - pbTurUserImg.Height) / 2;
        }       
        private void LinckUserTur_Layout(object sender, LayoutEventArgs e)
        {
                LinckUserTur.Cursor = Cursors.Hand;
        }
        private void LinckUserTur_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(LinckUserTur.Text);
            }
            catch (Exception) { }
        }
        private void tbContry_TextChanged(object sender, EventArgs e)
        {
            if (tbContry.Text != "")
            { 
                btUpdateFileTur.Enabled = true;
                btDeleteFileTur.Enabled = true;
                btUpdateImage.Enabled = true;

            }
            else
            {
                btUpdateFileTur.Enabled = false;
                btDeleteFileTur.Enabled = false;
                btUpdateImage.Enabled = false;
            }
        }
        public List<usery> SearchUser(string s, int a, List<usery> myUserList)
        {
            List<usery> myUser = new List<usery>();
            string stroka = "";

            foreach (usery dt in myUserList)
            {
                if (a == 1) stroka = dt.fam;
                if (a == 2) stroka = dt.tel;
                if (a == 3) stroka = dt.emaill;
              

                if (stroka.ToUpper().Contains(s.ToString().ToUpper()))
                {
                    usery us = new usery();
                    us.fam = dt.fam;
                    us.name = dt.name;
                    us.surname = dt.surname;
                    us.tel = dt.tel;
                    us.emaill = dt.emaill;
                    myUser.Add(us);
                }

            }
            return myUser;
        }
        List<usery> _usData = new List<usery>();
        private void rbFam_CheckedChanged(object sender, EventArgs e)
        {
            _usData = controlUser.ViewUserData(connect);
            tbSearchUser.ReadOnly = false;
        }
        private void rbTel_CheckedChanged(object sender, EventArgs e)
        {
            _usData = controlUser.ViewUserData(connect);
            tbSearchUser.ReadOnly = false;
        }
        private void rbEmail_CheckedChanged(object sender, EventArgs e)
        {
            _usData = controlUser.ViewUserData(connect);
            tbSearchUser.ReadOnly = false;
        }
        string searchUser = "";
        private void tbSearchUser_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back)
            {
                searchUser += e.KeyChar.ToString();
            }
            else
            {
                if (searchUser.Length >= 1)
                    searchUser = searchUser.Substring(0, searchUser.Length - 1);
            }
            //ss = tbSearch.Text;
            if(searchUser!=""&& searchUser != null) { 
            if (rbFam.Checked)
            {
                // dgBasaView.DataSource = null;
               
                dataGridUser.DataSource = SearchUser(searchUser, 1, _usData);
                myDatGridView();
            }
            if (rbTel.Checked)
            {
                dataGridUser.DataSource = SearchUser(searchUser, 2, _usData);
                myDatGridView();
            }
            if (rbEmail.Checked)
            {
                dataGridUser.DataSource = SearchUser(searchUser, 3, _usData);
                myDatGridView();
            }
            }else
            {
                dataGridUser.DataSource = controlUser.ViewUserData(connect);
                myDatGridView();
            }
        }
        private void btClearSearchUser_Click(object sender, EventArgs e)
        {
            rbFam.Checked = false;
            rbTel.Checked = false;
            rbEmail.Checked=false;
            tbSearchUser.ReadOnly = true;
            dataGridUser.DataSource = null;
            searchUser = "";
            dataGridUser.DataSource = controlUser.ViewUserData(connect);
            myDatGridView();
        } 
        private void tbSearchUser_TextChanged(object sender, EventArgs e)
        {
            if (tbSearchUser.Text == "")
            {
                dataGridUser.DataSource = null;
                searchUser = "";
                dataGridUser.DataSource = controlUser.ViewUserData(connect);
                myDatGridView();
            }
        }

        #endregion User

       
    }
    }
