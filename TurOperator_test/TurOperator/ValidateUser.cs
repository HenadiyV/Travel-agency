using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurOperator
{
    [UserValidation]
    class ValidateUser
    {
        [Required(ErrorMessage = "Поле фамилия пустое.")]
        public string fam { set; get; }
        [Required(ErrorMessage = "Поле имя пустое.")]
        public string name { set; get; }
        [Required(ErrorMessage = "Поле отчество пустое.")]
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
            ValidateUser user = value as ValidateUser;
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
