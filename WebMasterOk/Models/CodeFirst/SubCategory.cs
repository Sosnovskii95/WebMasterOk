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

        public string TitleSubCategory { get; set; }

        public string DescriptionSubCategory { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
