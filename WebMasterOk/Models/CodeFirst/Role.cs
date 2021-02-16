using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Models.CodeFirst
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Название роли")]
        public string TitleRole { get; set; }

        [Display(Name ="Описание роли")]
        public string DescriptionRole { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
