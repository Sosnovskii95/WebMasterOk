using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Models.CodeFirst
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Логин")]
        public string LoginClient { get; set; }

        [Display(Name = "Пароль")]
        public string PasswordClient { get; set; }

        [Display(Name = "Электронная почта")]
        public string EmailClient { get; set; }

        [Display(Name = "ФИО")]
        public string FullNameClient { get; set; }

        [Display(Name = "Номер телефона")]
        public string NumberTelephone { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }


    }
}
