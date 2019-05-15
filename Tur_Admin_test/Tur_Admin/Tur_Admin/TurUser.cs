using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tur_Admin
{
   public class TurUser
    {
       
            public int id_contry { set; get; }
            public int id_city { set; get; }
            public int id_hotel { set; get; }
            public int id_user { set; get; }
            //public string fam_user { set; get; }
            //public string name_user { set; get; }
            //public string surname_user { set; get; }
            public string contry { set; get; }
            public string city { set; get; }
            public string hotel { set; get; }
            public string category { set; get; }
            public string price { set; get; }
            public string linck { set; get; }
        public  DateTime dStart { set; get; }
        public DateTime dEnd { set; get; }
        public DateTime dReg{ set; get; }
        public byte[] img { set; get; }

    }
}
