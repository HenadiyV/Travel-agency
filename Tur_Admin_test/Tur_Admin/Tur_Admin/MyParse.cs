using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tur_Admin
{[Serializable]
  public   class MyParse
    {       
        public string contry { set; get; }
        public string city { set; get; }
        public string name { set; get; }
        public string category { set; get; }
        public string coment { set; get; }
        public string price { set; get; }        
        public string linck_hotel { set; get; }
        public byte[] img { set; get; }
    }
}
