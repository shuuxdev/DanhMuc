using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Middlewares
{
    public class JwtMiddleware
    {
        private RequestDelegate _next { get; set; }

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, UserService _userRepository)
        {
            // var token = context.Request.Cookies["token"];
            // if (token != null)
            // {
            //     attachUserToContext(context, _userRepository, token);
            // }
            await _next(context);
        }
    }
}