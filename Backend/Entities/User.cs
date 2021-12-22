using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#nullable disable

namespace Backend.Entities
{
    public partial class User
    {
        public User()
        {
            UserAddresses = new HashSet<UserAddress>();
        }

        public string Username { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public int? Telephone { get; set; }
        public DateTime Modified_At { get; set; }
        public DateTime Created_At { get; set; }
        public virtual ICollection<UserAddress> UserAddresses { get; set; }
        [JsonIgnore]
        public Guid User_Id { get; set; }
    }
}
