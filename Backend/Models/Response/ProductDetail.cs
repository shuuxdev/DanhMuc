using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#nullable disable

namespace Backend.Models.Response
{
    public partial class ProductDetail
    {
        public ProductDetail()
        {
            types = new List<DetailType>();
        }

        public int product_id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

        public int Sold { get; set; }


        public string Image { get; set; }

        public List<DetailType> types { get; set; }

    }
    public class DetailType
    {
        public DetailType()
        {
            options = new List<TypeOption>();
        }

        public int type_id { get; set; }
        public string type { get; set; }
        public List<TypeOption> options { get; set; }
    }
    public class TypeOption
    {

        [JsonIgnore]
        public int type_id { get; set; }
        public string option { get; set; }

        public int option_id { get; set; }
    }
}
