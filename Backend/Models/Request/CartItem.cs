using System;

namespace Backend.Models.Request
{
    public class CartItem
    {
        public int product_id { get; set; }
        public int? option_id { get; set; }
        public int? type_id { get; set; }
    }

}