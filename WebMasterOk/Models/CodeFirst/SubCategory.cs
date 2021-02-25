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
        public string TitleSubCategory { get; set; }

        [Display(Name = "Описание подкатегории")]
        public string DescriptionSubCategory { get; set; }

        [Display(Name = "Картинка подкатегории")]
        public ICollection<PathImage> PathImages { get; set; }

        [Display(Name = "Категория")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
