using System;
using System.Collections.Generic;

#nullable disable

namespace Backend.Entities
{
    public partial class OrderDetail
    {
        public int Order_Detail_Id { get; set; }
        public int Order_Id { get; set; }
        public Guid User_Id { get; set; }
        public int Address_Id { get; set; }
        public int Product_Id { get; set; }
        public DateTime Created_At { get; set; }

    }
}
