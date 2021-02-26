using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Models.CodeFirst
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Название подкатегории")]
        [Required(ErrorMessage = "Название подкатегории")]
        public string TitleSubCategory { get; set; }

        [Display(Name = "Описание подкатегории")]
        [Required(ErrorMessage = "Описание подкатегории")]
        public string DescriptionSubCategory { get; set; }

        [Display(Name = "Картинка подкатегории")]
        public ICollection<PathImage> PathImages { get; set; }

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Категория")]
        public int CategoryId { get; set; }

        [Display(Name = "Категория")]
        public Category Category { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
