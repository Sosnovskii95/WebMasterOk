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

        [Display(Name = "Как к вам обратиться")]
        [Required(ErrorMessage = "Укажите контактный номер телефона")]
        public string NumberFeedBack { get; set; }

        public int? UserId { get; set; }

        public User User { get; set; }
    }
}
