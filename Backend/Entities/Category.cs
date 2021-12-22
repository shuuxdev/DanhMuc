using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Entities
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        public int Category_Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
