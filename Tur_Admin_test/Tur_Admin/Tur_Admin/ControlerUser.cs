using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Tur_Admin
{
    class ControlerUser
    {
        private string ConvertP(string s)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] checkSum1 = md5.ComputeHash(Encoding.UTF8.GetBytes(s));
            return s = BitConverter.ToString(checkSum1).Replace("-", String.Empty);
        }
        public void AddUser(string _fam,string _name,string _surname,string _tel,string _emaill,string _pass,string connect )
        {
            using (Model1 db = new Model1(connect))
            {
                var us = db.useries.Where(s => s.fam == _fam && s.name == _name && s.surname == _surname && s.tel == _tel).FirstOrDefault();
                if (us == null)
                {
                try
                {
                    string s_p = "";
                    if (_pass!=""|| _pass != null)
                    {  s_p = ConvertP(_pass);}

                        usery _user = new usery { fam = _fam, name = _name, surname = _surname, tel = _tel, emaill = _emaill, pass = s_p };
                    db.useries.Add(_user);
                    db.SaveChanges();                
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                    {
                            //Response.Write("Object: " + validationError.Entry.Entity.ToString());
                            MessageBox.Show("Object: " + validationError.Entry.Entity.ToString());
                        //Response.Write("");
                        foreach (DbValidationError err in validationError.ValidationErrors)
                        {
                                // Response.Write(err.ErrorMessage + "");
                                MessageBox.Show(err.ErrorMessage + "");
                            }
                    }
                }
               }
                else MessageBox.Show("Пользователь уже зарегистрирован в базе."); 
            }
        }
        public List<usery> ViewUserData(string connect)
        {
            List<UserData> usr = new List<UserData>();
            UserData _u = new UserData();
            using (Model1 db = new Model1(connect))
            {
               var us = db.useries.ToList();
                if (us.Count > 0)
                {
                    return us;
            }else return null;
                
            }
        }
        public void UpdateUser(UserData us, string connect)
        {
            using (Model1 db = new Model1(connect))
            {
                var user = db.useries.Where(a => a.Id == us._id_user).FirstOrDefault();
                if(user!=null)
                {
                    user.fam = us._fam;
                    user.name = us._name;
                    user.surname = us._surname;
                    user.tel = us._tel;
                    user.emaill = us._emaill;
                    if (!user.pass.Equals(us._pass))
                        user.pass = ConvertP(us._pass);
                    else
                        user.pass = us._pass;
                    db.SaveChanges();
                }
            }
            }
        public void DeleteUser(int id_user, string connect)
        {
            using (Model1 db = new Model1(connect))
            {
                var user = db.useries.Where(a => a.Id == id_user).FirstOrDefault();
                if (user != null)
                {
                    db.useries.Remove(user);
                    db.SaveChanges();
                }
            }
        }
        public  List<TurUser> UserTur(int id_user,string connect)
        { List<TurUser> tu_list = new List<TurUser>();
            
            using (Model1 db = new Model1(connect))
            {
                TurUser tr_u = new TurUser();
                var us = db.Turs.Where(a => a.id_user == id_user).FirstOrDefault();
                if (us != null)
                {
                    var view = (from usr in db.Turs
                                join contry in db.Contries on usr.id_contry equals contry.Id_contry
                                join cit in db.Cities on usr.id_city equals cit.Id_city
                                join hot in db.Hotels on usr.id_hotel equals hot.Id_hotel
                                select new TurUser
                                {
                                    id_contry = contry.Id_contry,
                                    id_city = cit.Id_city,
                                    id_hotel = hot.Id_hotel,
                                    contry = contry.contry1,
                                    city = cit.city1,
                                    hotel = hot.hotel1,
                                    category = hot.category,
                                    price = hot.price,
                                    img = hot.img,
                                    linck = hot.linck,
                                }).ToList<TurUser>();
                }            
            }            
                return tu_list;
        }
    }
}
