using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Models.CodeFirst
{
    public class Position
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Название должности")]
        [Required(ErrorMessage = "Название должности")]
        public string TitlePosition { get; set; }

        public ICollection<Staff> Staffs { get; set; }
    }
}
