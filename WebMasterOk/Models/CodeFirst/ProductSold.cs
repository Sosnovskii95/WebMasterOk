using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Models.CodeFirst
{
    public class ProductSold
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateSale { get; set; }

        public float PercentDiscont { get; set; }

        public int ProductCheckId { get; set; }

        public ProductCheck ProductCheck { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }
    }
}
