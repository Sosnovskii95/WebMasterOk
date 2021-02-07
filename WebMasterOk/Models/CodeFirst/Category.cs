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

        public string TitleCategory { get; set; }

        public string DescriptionCategory { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
