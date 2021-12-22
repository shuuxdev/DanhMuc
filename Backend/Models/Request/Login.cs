using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace Backend.Models.Request
{

    public class Login
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }

}
