using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tur_Admin
{
    [UserValidation]
    public class ValidateUser
    {

        [Required]
        public string fam { set; get; }
        [Required]
        public string name { set; get; }
        [Required]
        public string surname { set; get; }
        [Phone(ErrorMessage = "Введен некоректный номер")]
        public string pfone { set; get; }
        [EmailAddress(ErrorMessage = "Введён некоректный адрес ")]
        public string emaill { set; get; }

    }
    public class UserValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            ValidateUser user = value as ValidateUser ;
            try
            {
                if (user.name.Length < 3)
                {
                    this.ErrorMessage = "Имя не должно меньше 3 символов";
                    return false;
                }

                if (user.fam.Length < 3)
                {
                    this.ErrorMessage = " Фамилия   меньше 3 символов";
                    return false;
                }
                if (user.surname.Length < 3)
                {
                    this.ErrorMessage = " Отчество   меньше 3 символов";
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}
