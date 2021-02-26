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

        [Display(Name = "Название товара")]
        [Required(ErrorMessage = "Название товара")]
        public string TitleProduct { get; set; }

        [Display(Name = "Описание товара")]
        [Required(ErrorMessage = "Описание товара")]
        public string DescriptionProduct { get; set; }

        [Display(Name = "Стоимость")]
        [Required(ErrorMessage = "Стоимость")]
        public int Price { get; set; }

        [Display(Name = "Гарантийный срок")]
        [Required(ErrorMessage = "Гарантийный срок")]
        public int Warranty { get; set; }

        [Display(Name ="Изображение")]
        public ICollection<PathImage> PathImages { get; set; }

        [Display(Name = "Подкатегория товара")]
        public int SubCategoryId { get; set; }

        [Display(Name = "Подкатегория")]
        public SubCategory SubCategory { get; set; }

        public Store Stores { get; set; }
    }
}
