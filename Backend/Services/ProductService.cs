

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Entities;
using Backend.Models.Context;
using Backend.Models.Request;
using Backend.Models.Response;
using Dapper;

namespace Backend.Services
{
    public class ProductService
    {
        public ProductService(DapperContext ctx)
        {
            _ctx = ctx;
        }

        private DapperContext _ctx { get; }
        string sql = @"SELECT product_id,
        name,
        category_id,
        price,image, sold FROM Product.Product p
        ORDER BY p.product_id
        OFFSET @pageSize * (@pageIndex - 1) ROWS
        FETCH NEXT @pageSize ROWS ONLY;
        ";
        public IEnumerable<Product> GetProducts(Page setting)
        {
            var parameters = _ctx.CreateParameters<Page>(setting);
            using (var connection = _ctx.CreateBanHangConnection())
            {
                var query_result = connection.Query<Product>(sql, parameters);

                return (query_result);
            }
        }
        public async Task<IEnumerable<Product>> GetProductsAsync(Page setting)
        {
            var parameters = _ctx.CreateParameters<Page>(setting);
            using (var connection = _ctx.CreateBanHangConnection())
            {
                var query_result = await connection.QueryAsync<Product>(sql, parameters);

                return (query_result);
            }
        }
        public ProductDetail GetProduct(int itemid)
        {

            var parameter = new DynamicParameters();
            parameter.Add("product_id", itemid);
            using (var connection = _ctx.CreateBanHangConnection())
            {
                using (var query_result = connection.QueryMultiple("[Product].GetByID", param: parameter, commandType: System.Data.CommandType.StoredProcedure))
                {

                    var product = query_result.ReadSingle<ProductDetail>();
                    var type = query_result.Read<DetailType>();
                    var options = query_result.Read<TypeOption>();

                    if (type.Count() != 0 && options.Count() != 0)
                    {
                        var types = type.ToDictionary(type => type.type_id);
                        foreach (var option in options)
                        {
                            DetailType _type;
                            types.TryGetValue(option.type_id, out _type);
                            if (_type == null)
                            {

                                throw new System.Exception("Type không tồn tại");
                            }
                            _type.options.Add(option);
                        }
                        product.types.AddRange(types.Select(type => type.Value));
                    }
                    return (product);
                }
            }
        }
        public object SearchProduct(string keyword)
        {
            var param = new DynamicParameters();
            param.Add("@keyword", keyword);
            using (var connection = _ctx.CreateBanHangConnection())
            {
                object query_result = connection.Query<object>("[Product].Search", commandType: System.Data.CommandType.StoredProcedure, param: param);
                return query_result;
            }
        }
    }
}