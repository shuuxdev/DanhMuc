
using System;
using System.Threading.Tasks;
using Backend.Attributes;
using Backend.Models.Request;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api")]
    public class UserController : Controller
    {
        private UserService _userRepository { get; set; }
        public UserController(UserService userRepository)
        {
            _userRepository = userRepository;
        }
        [Authorize]
        [HttpGet("user/account")]
        public async Task<IActionResult> GetUserInformation()
        {
            var userDetail = await _userRepository.GetUserDetail();
            return Json(userDetail);
        }
        [HttpPost("user/login")]
        public IActionResult Login([FromBody] Login user)
        {
            string token = _userRepository.Authenticate(user);
            if (token == null)
            {
                return BadRequest("username or password is incorrect");
            }
            HttpContext.Response.Cookies.Append("token", token, new Microsoft.AspNetCore.Http.CookieOptions()
            {
                Expires = DateTime.Now.AddDays(7),
                HttpOnly = true,

            });
            return Ok(token);
        }
        [HttpPost("user/signup")]
        public IActionResult Register([FromBody] SignUp user)
        {
            bool isCreated = _userRepository.Register(user);
            if (isCreated)
            {
                return Ok();
            }
            return BadRequest();
        }

    }

}