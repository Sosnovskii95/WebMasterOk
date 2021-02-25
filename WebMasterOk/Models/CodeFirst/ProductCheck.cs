using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Models.CodeFirst
{
    public class ProductCheck
    {
        [Key]
        public int Id { get; set; }

        public int QuantitySale { get; set; }

        public float SumSale { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int ProductSoldId { get; set; }

        public ProductSold ProductSold { get; set; }

        public int PayMethodId { get; set; }

        public PayMethod PayMethod { get; set; }

        public int DeliveryMethodId { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; }
    }
}
