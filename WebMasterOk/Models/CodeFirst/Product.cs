using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMasterOk.Models.CodeFirst
{
    public class Product
    {
        public int Id { get; set; }

        public int TitleProduct { get; set; }

        public int DescriptionProduct { get; set; }

        public float Price { get; set; }

        public int Warranty { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
