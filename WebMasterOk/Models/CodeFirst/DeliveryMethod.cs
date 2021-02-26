using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Models.CodeFirst
{
    public class DeliveryMethod
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Способ доставки")]
        [Required(ErrorMessage = "Способ доставки")]
        public string TitleDeliveryMethod { get; set; }
    }
}
