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

        public string TitleProduct { get; set; }

        public string DescriptionProduct { get; set; }

        public int Price { get; set; }

        public int Warranty { get; set; }

        public int SubCategoryId { get; set; }

        public SubCategory SubCategory { get; set; }
    }
}
