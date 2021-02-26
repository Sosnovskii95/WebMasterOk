using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Models.CodeFirst
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Логин пользователя")]
        public string LoginUser { get; set; }

        [Display(Name = "Пароль пользователя")]
        public string PasswordUser { get; set; }

        [Display(Name = "Разрешение для входа")]
        public bool Valid { get; set; }

        [Display(Name = "Сотрудник")]
        public Staff Staff { get; set; }

        [Display(Name = "Сотрудник")]
        public int StaffId { get; set; }

        [Display(Name = "Роль")]
        public Role Role { get; set; }

        [Display(Name = "Роль")]
        public int RoleId { get; set; }
    }
}
