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

        public string LoginUser { get; set; }

        public string PasswordUser { get; set; }

        public bool Valid { get; set; }

        public Staff Staff { get; set; }

        public int StaffId { get; set; }

        public Role Role { get; set; }

        public int RoleId { get; set; }
    }
}
