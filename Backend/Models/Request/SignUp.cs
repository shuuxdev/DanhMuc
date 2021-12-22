using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace Backend.Models.Request
{

    public class SignUp
    {
        [Required(ErrorMessage = "Tên tài khoản không được bỏ trống")]
        public string username { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]

        public string password { get; set; }
        [Required(ErrorMessage = "Email không được bỏ trống")]

        public string email { get; set; }
        [Required(ErrorMessage = "Tên không được bỏ trống")]

        public string first_name { get; set; }
        [Required(ErrorMessage = "Tên không được bỏ trống")]

        public string last_name { get; set; }


    }
}
