using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Entities
{
    public partial class Product
    {

        public int Product_Id { get; set; }
        public string Name { get; set; }
        public int? Price { get; set; }
        public int? Quantity { get; set; }

        public int? Sold { get; set; }

        public string Image { get; set; }
        public DateTime Modified_At { get; set; }
        public DateTime Deleted_At { get; set; }
    }
}
