using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Models.CodeFirst
{
    public class FeedBack
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Вопрос к продавцу")]
        [Required(ErrorMessage = "Укажите что Вас интересует")]
        public string Question { get; set; }

        [Display(Name = "Номер телефона")]
        [Required(ErrorMessage = "Укажите контактный номер телефона")]
        public string NumberFeedBack { get; set; }

        [Display(Name = "Статус")]
        public string StateFeedBack { get; set; }

        [Display(Name = "Пользователь")]
        public int? UserId { get; set; }

        [Display(Name = "Пользователь")]
        public User User { get; set; }
    }
}
