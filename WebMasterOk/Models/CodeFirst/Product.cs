using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Models.CodeFirst
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Название товара")]
        public string TitleProduct { get; set; }

        [Display(Name = "Описание товара")]
        public string DescriptionProduct { get; set; }

        [Display(Name = "Стоимость")]
        public int Price { get; set; }

        [Display(Name = "Гарантийный срок")]
        public int Warranty { get; set; }

        public string PathImage { get; set; }

        public int SubCategoryId { get; set; }

        public SubCategory SubCategory { get; set; }
    }
}
