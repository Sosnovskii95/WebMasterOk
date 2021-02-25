using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Models.Authoriziation
{
    public class RegisterModel
    {
        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Не указан Логин")]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Не указан пароль")]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Повторите пароль")]
        [Compare("Password", ErrorMessage = "Пароль введен не верно")]
        //[DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Электронная почта")]
        [Required(ErrorMessage = "Не указана эл почта")]
        public string EmailClient { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Требуется поле Фамилия")]
        public string FamClient { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Требуется поле Имя")]
        public string FirstNameClient { get; set; }

        [Display(Name = "Отчество")]
        [Required(ErrorMessage = "Требуется поле Отчество")]
        public string LastNameClient { get; set; }

        [Display(Name = "Номер телефона")]
        [Required(ErrorMessage = "Требуется поле Номер телефона")]
        public string NumberTelephone { get; set; }

        [Display(Name = "Адрес")]
        [Required(ErrorMessage = "Требуется поле Адрес")]
        public string Address { get; set; }
    }
}
