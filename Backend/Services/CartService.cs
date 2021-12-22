using System;
using System.Collections.Generic;
using System.Data;
using Backend.Entities;
using Backend.Models.Context;
using Backend.Models.Request;
using Dapper;
namespace Backend.Services
{
    public class CartService
    {
        private DapperContext _ctx { get; set; }
        public CartService(DapperContext ctx)
        {
            _ctx = ctx;
        }
        public IEnumerable<object> GetCart(string user_id)
        {
            var param = _ctx.CreateSingleParameter(new Guid(user_id), nameof(user_id));
            using (var connection = _ctx.CreateBanHangConnection())
            {
                var products = connection.Query<object>("[Cart].GetItems", param, commandType: CommandType.StoredProcedure);
                return products;
            }
        }
        public string Add(int product_id, string userId)
        {
            var param = _ctx.CreateSingleParameter<int>(product_id, nameof(product_id));
            param.Add("@user_id", new Guid(userId), dbType: DbType.Guid);
            using (var connection = _ctx.CreateBanHangConnection())
            {
                try
                {
                    var query_result = connection.ExecuteScalar<string>("Cart.AddItem", param, commandType: CommandType.StoredProcedure);
                    return query_result;
                }
                //TODO exception handling
                catch (Microsoft.Data.SqlClient.SqlException)
                {
                    return "Sản phẩm đã tồn tại";
                }
            }
        }
        public string Add(CartItem item, string userId)
        {
            var param = _ctx.CreateParameters<CartItem>(item);
            param.Add("@user_id", new Guid(userId), dbType: DbType.Guid);
            using (var connection = _ctx.CreateBanHangConnection())
            {
                try
                {
                    var query_result = connection.ExecuteScalar<string>("Cart.AddItem", param, commandType: CommandType.StoredProcedure);
                    return query_result;
                }
                //TODO exception handling
                catch (Microsoft.Data.SqlClient.SqlException e)
                {
                    return e.Message;
                }
            }
        }

        public int RemoveList(int[] list, string user_id)
        {
            var param = _ctx.CreateParameters<int>(list, "@list", "DeleteList");
            param.Add("@user_id", new Guid(user_id), dbType: DbType.Guid);
            using (var connection = _ctx.CreateBanHangConnection())
            {
                try
                {
                    var query_result = connection.ExecuteScalar<int>("Cart.RemoveListItem", param, commandType: CommandType.StoredProcedure);
                    return query_result;
                }
                catch (Microsoft.Data.SqlClient.SqlException)
                {
                    return 0;
                }
            }
        }
        public int UpdateQuantity(Quantity quantity)
        {
            var param = _ctx.CreateParameters<Quantity>(quantity);
            using (var connection = _ctx.CreateBanHangConnection())
            {
                try
                {
                    var query_result = connection.ExecuteScalar<int>("Cart.UpdateQuantity", param, commandType: CommandType.StoredProcedure);
                    return query_result;
                }
                catch (Microsoft.Data.SqlClient.SqlException)
                {
                    return 0;
                }
            }
        }
        public IEnumerable<object> GetItems(int[] list)
        {

            var param = _ctx.CreateParameters<int>(list, "@list", "ListId");
            using (var connection = _ctx.CreateBanHangConnection())
            {
                var query_result = connection.Query<object>("Cart.GetCheckoutItems", param: param, commandType: CommandType.StoredProcedure);
                return query_result;
            }
        }
    }
}