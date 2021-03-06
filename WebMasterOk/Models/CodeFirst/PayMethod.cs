﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Models.CodeFirst
{
    public class PayMethod
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Тип оплаты")]
        [Required(ErrorMessage = "Тип оплаты")]
        public string TitlePayMethod { get; set; }
    }
}
