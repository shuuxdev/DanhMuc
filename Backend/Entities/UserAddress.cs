using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Entities
{
    public partial class UserAddress
    {
        public int Address_Id { get; set; }
        public string Address_Line { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Telephone { get; set; }

    }
}
