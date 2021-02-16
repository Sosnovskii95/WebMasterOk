using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Models.CodeFirst
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="ФИО сотрудника")]
        public string FullNameStaff { get; set; }

        [Display(Name = "Возраст")]
        public int Age { get; set; }

        public Position Position { get; set; }

        [Display(Name = "Должность")]
        public int PositionId { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
