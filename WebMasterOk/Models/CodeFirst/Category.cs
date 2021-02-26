using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Models.CodeFirst
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Название категории")]
        [Required(ErrorMessage = "Название категории")]
        public string TitleCategory { get; set; }

        [Display(Name = "Описание категории")]
        [Required(ErrorMessage = "Описание категории")]
        public string DescriptionCategory { get; set; }

        [Display(Name = "Изображение категории")]
        public ICollection<PathImage> PathImages { get; set; }

        [Display(Name ="Подкатегории")]
        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
