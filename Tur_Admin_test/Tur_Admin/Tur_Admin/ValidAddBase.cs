using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tur_Admin
{
    [BaseValidationAttribute]
  public  class ValidAddBase
    {
        [Required(ErrorMessage = "Страна поле пустое.")]
        public string contry { set; get; }
        [Required(ErrorMessage = "Город поле пустое.")]
        public string city { set; get; }
        [Required(ErrorMessage = "Отель поле пустое.")]
        public string hotel { set; get; }
        [Required(ErrorMessage = "Категория поле пустое.")]
        public string category { set; get; }     
    }

    public class BaseValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            ValidAddBase adr = value as ValidAddBase;           
            try
            {
                if (adr.contry.Length < 3 )
                {
                    this.ErrorMessage = "Название страны  должно быть больше 3-х символов";
                    return false;
                }
                if (adr.city.Length < 2 )
                {
                    this.ErrorMessage = "Название города  должно быть больше 2-х символов";
                    return false;
                }
                if (adr.hotel.Length < 2 )
                {
                    this.ErrorMessage = "Название отеля  должно быть больше 2-х символов";
                    return false;
                }
                if (adr.category.Length < 2)
                {
                    this.ErrorMessage = "Укажите категорию";
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
