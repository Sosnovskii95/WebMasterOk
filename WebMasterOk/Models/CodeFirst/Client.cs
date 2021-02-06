using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Models.CodeFirst
{
    public class Client
    {
        public int Id { get; set; }

        public string LoginClient { get; set; }

        public string PasswordClient { get; set; }

        public string EmailClient { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NumberTelephone { get; set; }

        public string Address { get; set; }


    }
}
