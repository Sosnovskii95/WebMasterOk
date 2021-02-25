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

        [Display(Name ="Дата и время покупки")]
        public DateTime DateSale { get; set; }

        public float PercentDiscont { get; set; }

        public ICollection<ProductCheck> ProductChecks { get; set; }

        [Display(Name ="Менеджер")]
        public int? UserId { get; set; }

        public User User { get; set; }

        [Display(Name ="Статус заказа")]
        public string StateOrder { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }
    }
}
