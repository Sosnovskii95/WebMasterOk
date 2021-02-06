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

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}
