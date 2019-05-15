using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tur_Admin
{
    class CorectPares
    {
        class Registration
        {
            public bool Fam { set; get; }
            public bool Nam { set; get; }
            public bool Otch { set; get; }
            public bool Mobil { set; get; }
            public bool Email { set; get; }

            public Registration(bool fm, bool nm, bool ot, bool mb, bool em)
            {
                Fam = fm;
                Nam = nm;
                Otch = ot;
                Mobil = mb;
                Email = em;
            }
        }
        abstract class RegistrationHandler
        {
            public RegistrationHandler Res { set; get; }
            public abstract void Handler(Registration registration);
        }
        class Fam_reg : RegistrationHandler
        {
            public override void Handler(Registration registration)
            {
                if (registration.Fam == true)
                {
                    Res.Handler(registration);
                }
                else
                {
                    Res = null;
                    MessageBox.Show(" Ошибка в поле фамилия");
                }
            }
        }
    }

}
