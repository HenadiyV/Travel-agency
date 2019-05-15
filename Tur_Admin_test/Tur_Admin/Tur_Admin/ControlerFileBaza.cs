using System;
using System.Collections.Generic;
using System.Linq;
using myConnect;
using System.IO;
using System.Windows.Forms;
using System.Data.Entity.Validation;

namespace Tur_Admin
{
    class ControlerFileBaza
    {  
        Contry contry = new Contry();
        City city = new City();
        Hotel hotel = new Hotel();
        public static string myPath = Directory.GetCurrentDirectory() + "\\Temp\\";      
        public static string connect = "";
        //добавление списка в базу
        public void AddFile(List<MyParse> dataOpenFile, string connect)
        {
            string _contry = dataOpenFile[0].contry;
            string _city = dataOpenFile[0].city;
            int id_contry = 0;
            int id_cite = 0;
            foreach (MyParse pr in dataOpenFile)
            {
                try {
                  
                    using (Model1 db = new Model1(connect))
                    {
                        var cnt = db.Contries.Where(s => s.contry1 == pr.contry).FirstOrDefault();
                        if (cnt == null)
                        {
                            Contry contry = new Contry();
                            contry.contry1 = pr.contry;                          
                            db.Contries.Add(contry);
                            db.SaveChanges();
                        }
                        else id_contry = cnt.Id_contry;
                        
                        if (id_contry == 0)
                        {
                            var cont = db.Contries.Where(s => s.contry1 == pr.contry).FirstOrDefault();
                            id_contry = cont.Id_contry;
                        }

                        var cite = db.Cities.Where(s => s.city1 == pr.city).FirstOrDefault();
                        if (cite == null)
                        {

                            City cit = new City();
                            cit.id_contry = id_contry;
                            cit.city1 = pr.city;
                            db.Cities.Add(cit);
                            db.SaveChanges();
                        }
                        else id_cite = cite.Id_city;
                        try
                        { }
                        catch (Exception) { MessageBox.Show("Ошибка добавления. Проверте исходящий файл. "); }

                        if (id_cite == 0)
                            {
                                var ct = db.Cities.Where(a => a.city1 == pr.city).FirstOrDefault();
                                id_cite = ct.Id_city;
                            }
                            var hotel = db.Hotels.Where(s => s.hotel1 == pr.name && s.linck == pr.linck_hotel && s.category == pr.category).FirstOrDefault();
                            if (hotel == null)
                            {
                                Hotel hot = new Hotel();
                                hot.id_contry = id_contry;
                                hot.id_city = id_cite;
                                hot.hotel1 = pr.name;
                                hot.category = pr.category;
                                hot.coment = pr.coment;
                                hot.price = pr.price;
                                hot.img = pr.img;
                                hot.linck = pr.linck_hotel;
                              
                                db.Hotels.Add(hot);
                                db.SaveChanges();
                            }
                            else
                            { MessageBox.Show("Такая запись уже есть в базе."); }
                            
                       
                    }
               }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                    {
                        MessageBox.Show("Object: " + validationError.Entry.Entity.ToString()); 
                        foreach (DbValidationError err in validationError.ValidationErrors)
                        {
                            MessageBox.Show(err.ErrorMessage + "");
                        }
                    }
                }
            }
            MessageBox.Show("Операция завершена");
        }                
        //добавление одиночной записи
        public void AddBase
            (string _contry,string _city,string _hotel,string _category,string _coment,string _price,byte[] _img,string _linck, string connect)
        {
            MyParse parse = new MyParse { };
            int id_contry=0;
            int id_city = 0;          
            using (Model1 db = new Model1(connect))
            {
                var hotel = db.Hotels.Where(s => s.hotel1 == _hotel).FirstOrDefault();
                if (hotel == null)
                {
                    Hotel _hot = new Hotel();
                    var contry = db.Contries.Where(s => s.contry1 == _contry).FirstOrDefault();
                    if (contry == null)
                    {
                        Contry c = new Contry();
                        c.contry1 = _contry;
                       db.Contries.Add (c);
                       db.SaveChanges();
                        var contry1 = db.Contries.Where(s => s.contry1 == _contry).FirstOrDefault();
                        id_contry = contry1.Id_contry;
                    }
                    else id_contry = contry.Id_contry;

                    var city = db.Cities.Where(s => s.city1 == _city).FirstOrDefault();
                    if (city == null)
                    {
                        City ct = new City();
                        ct.city1 = _city;
                        ct.id_contry = id_contry;
                        db.Cities.Add(ct);
                        db.SaveChanges();
                    }
                    else id_city = city.Id_city;

                    _hot.hotel1 = _hotel;
                    _hot.category = _category;
                    _hot.coment = _coment;
                    _hot.price = _price;
                    _hot.linck = _linck;
                    _hot.img = _img;
                    if(id_city ==0)
                    {
                        var cit = db.Cities.Where(s => s.city1 == _city).FirstOrDefault();
                        id_city = cit.Id_city;
                    }
                    _hot.id_city = id_city;
                    if (id_contry == 0)
                    {
                        var cnt = db.Contries.Where(s => s.contry1 == _contry).FirstOrDefault();
                        id_contry = cnt.Id_contry;
                    }
                    _hot.id_contry = id_contry;
                    db.Hotels.Add(_hot);
                    db.SaveChanges();
                }
                else MessageBox.Show("Такая запись уже есть в базе.");
            }
        }
        public List<myData> ViewBaza(string connect)
       {  
            using (Model1 db = new Model1(connect))
            {
                var view = (from contr in db.Contries
                            join cit in db.Cities on contr.Id_contry equals cit.id_contry
                            join hot in db.Hotels on cit.Id_city equals hot.id_city
                            select new myData
                            {
                                _id_contry= contr.Id_contry,
                                _id_city=cit.Id_city,
                                _id_hotel=hot.Id_hotel,
                                _contry = contr.contry1,
                                _city = cit.city1,
                                _hotel = hot.hotel1,
                                _category = hot.category,
                                _coment = hot.coment,
                                _price = hot.price,
                                _img = hot.img,
                                _linck = hot.linck,                                            
                 }).ToList<myData>();
 return view;
            }               
       }    
        public void UpdateBaza(myData m_data, string connect)
        {
            using (Model1 db = new Model1(connect))
            {
                var contr = db.Contries.Where(a => a.Id_contry == m_data._id_contry).FirstOrDefault();           
                var cit = db.Cities.Where(a => a.Id_city == m_data._id_city).FirstOrDefault();              
                var hot = db.Hotels.Where(a => a.Id_hotel == m_data._id_hotel).FirstOrDefault();
                hot.hotel1 = m_data._hotel;
                hot.category = m_data._category;
                hot.id_city = m_data._id_city;
                hot.id_contry = m_data._id_contry;
                hot.price = m_data._price;
                hot.linck = m_data._linck;
                hot.img = m_data._img;               
                db.SaveChanges();
            }
            }
        public void DeleteBazaHotel(myData m_data, string connect)
        {           
            using (Model1 db = new Model1(connect))
            {               
                var hot = db.Hotels.Where(a => a.Id_hotel == m_data._id_hotel&&a.id_city==m_data._id_city&&a.id_contry==m_data._id_contry).FirstOrDefault();
                db.Hotels.Remove(hot);
                db.SaveChanges();
            }
            }        
    }
    }