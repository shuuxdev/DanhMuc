using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Backend.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private UserService _userService { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            _userService = context.HttpContext.RequestServices.GetService(typeof(UserService)) as UserService;


            // Validate token, pass thông tin user cho HttpContext.Items["User"],
            var token = context.HttpContext.Request.Cookies["token"];
            if (token != null)
            {
                attachUserToContext(context, _userService, token);
            }
            //Xác thực thất bại nên k cho user truy cập
            if (context.HttpContext.Items["User"] == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
        private void attachUserToContext(AuthorizationFilterContext context, UserService userService, string token)
        {
            string secret = "shuudeptraicomotkhonghai";
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var user_id = jwtToken.Claims.First(x => x.Type == "user_id").Value.ToString();
                // attach user to context on successful jwt validation
                context.HttpContext.Items["User"] = userService.GetById(user_id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}