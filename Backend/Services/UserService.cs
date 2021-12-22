using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backend.Entities;
using Backend.Models.Context;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using Backend.Models.Request;
using System.Reflection;

namespace Backend.Services
{

    public class UserService
    {
        private DapperContext _ctx;

        public UserService(DapperContext ctx)
        {
            _ctx = ctx;
        }


        public string GetById(string id)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@user_id", value: new Guid(id), dbType: DbType.Guid);
            using (var connection = _ctx.CreateBanHangConnection())
            {
                var userId = connection.ExecuteScalar("[User].GetByID", param: parameters, commandType: CommandType.StoredProcedure).ToString();
                return userId;
            }
        }

        public async Task<User> GetUserDetail()
        {

            var paramDictionary = new Dictionary<string, object>(){
                 {"@user_id",new Guid("312fd2dd-93a0-496b-ae76-88dc459a4ebd")}
            };
            var parameters = new DynamicParameters(paramDictionary);
            using (var connection = _ctx.CreateBanHangConnection())
            {
                var queryResult = await connection.QueryMultipleAsync("[User].Detail", param: parameters);

                var user = await queryResult.ReadSingleAsync<User>();
                var user_address = await queryResult.ReadAsync<UserAddress>();
                foreach (var address in user_address)
                {
                    user.UserAddresses.Add(address);
                }
                return (user);
            }
        }

        public string Authenticate(Login user)
        {
            var parameters = _ctx.CreateParameters<Login>(user);
            using (var connection = _ctx.CreateBanHangConnection())
            {
                string userId = connection.ExecuteScalar("[User].SignIn", commandType: CommandType.StoredProcedure, param: parameters).ToString();
                if (userId == null)
                {
                    return null;
                }
                return generateJwtToken(userId);
            }
        }
        public bool Register(SignUp user)
        {

            string store_procedure = "[User].SignUp";
            var parameters = _ctx.CreateParameters<SignUp>(user);
            using (var connection = _ctx.CreateBanHangConnection())
            {
                var query_status = connection.ExecuteScalar(store_procedure, commandType: CommandType.StoredProcedure, param: parameters).ToString();
                switch (query_status)
                {
                    case "failed":
                        return false;
                    case "success":
                        return true;
                    default:
                        return false;
                }
            }
        }


        //Tạo token
        //return token để biết token có tạo chưa
        private string generateJwtToken(string user_id)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("shuudeptraicomotkhonghai");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("user_id", user_id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}