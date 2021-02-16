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

        public Staff Staff { get; set; }

        public int StaffId { get; set; }

        public Role Role { get; set; }

        public int RoleId { get; set; }
    }
}
