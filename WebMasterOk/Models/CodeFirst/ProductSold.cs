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

        [Display(Name = "Процент скидки")]
        public float PercentDiscont { get; set; }

        public ICollection<ProductCheck> ProductChecks { get; set; }

        [Display(Name ="Менеджер")]
        public int? UserId { get; set; }

        [Display(Name = "Менеджер")]
        public User User { get; set; }

        [Display(Name ="Статус заказа")]
        public string StateOrder { get; set; }

        [Display(Name = "Клиент")]
        public int ClientId { get; set; }

        [Display(Name = "Клиент")]
        public Client Client { get; set; }
    }
}
