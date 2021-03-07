using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Models.CodeFirst
{
    public class PathImage
    {
        [Key]
        public int Id { get; set; }

        public string NameImage { get; set; }

        public int? ProductId { get; set; }

        public Product Product { get; set; }

        public int? SubCategoryId { get; set; }

        public SubCategory SubCategory { get; set; }

        public int? CategoryId { get; set; }

        public Category Category { get; set; }

        public bool Slider { get; set; }

        public string TypeImage { get; set; }
    }
}
