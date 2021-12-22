using System;
using System.Data;
using System.Linq;
using System.Reflection;
using Backend.Entities;
using Backend.Models.Request;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace Backend.Models.Context
{
    public class DapperContext
    {
        private readonly IConfiguration Configuration;
        private readonly string danhMucConnectionString;
        private readonly string examConnectionString;

        private readonly string BanHangConnectionString;

        private string MockingConnectionString { get; }

        public DapperContext(IConfiguration configuration)
        {
            Configuration = configuration;
            danhMucConnectionString = configuration.GetConnectionString("DanhMuc");
            examConnectionString = configuration.GetConnectionString("Exam");
            BanHangConnectionString = configuration.GetConnectionString("BanHang");
            MockingConnectionString = configuration.GetConnectionString("Mocking");
        }
        public IDbConnection CreateConnection() => new SqlConnection(danhMucConnectionString);
        public IDbConnection CreateExamConnection() => new SqlConnection(examConnectionString);
        public IDbConnection CreateBanHangConnection() => new SqlConnection(BanHangConnectionString);

        public IDbConnection CreateMockingConnection() => new SqlConnection(MockingConnectionString);


        public DynamicParameters CreateParameters<T>(T obj)
        {

            var parameters = new DynamicParameters();
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (var prop in properties)
            {
                parameters.Add("@" + prop.Name.ToLower(), dbType: GetDbType(prop.PropertyType), value: prop.GetValue(obj, null));
            }
            return parameters;
        }

        public DynamicParameters CreateParameters<T>(T[] obj, string paramName, string paramType)
        {
            var dt = obj.ToDataTable<T>();
            var parameters = new DynamicParameters();
            parameters.Add(paramName, dt.AsTableValuedParameter(paramType));
            return parameters;
        }

        public DynamicParameters CreateSingleParameter<T>(T obj, string objName)
        {

            var parameter = new DynamicParameters();

            parameter.Add("@" + objName, dbType: GetDbType(obj.GetType()), value: obj);
            return parameter;
        }
        public DbType GetDbType(Type type)
        {
            switch (type.Name.ToLower())
            {
                case "string":
                    return DbType.String;
                case "int32":
                    return DbType.Int32;
                case "double":
                    return DbType.Double;
                case "guid":
                    return DbType.Guid;
                default:
                    return DbType.String;
            }
        }
    }
    public static class Util
    {
        //Convert 1 list object thành datatable
        //Nếu là list value thì datatable chỉ có 1 column mặc định là id
        public static DataTable ToDataTable<T>(this T[] obj)
        {
            if (obj.Length < 1) throw new Exception("Empty List");
            DataTable table = new DataTable();
            Type type = obj[0].GetType();
            if (type.IsValueType)
            {
                table.Columns.Add("id");
                for (int i = 0; i < obj.Length; ++i)
                {
                    table.Rows.Add(obj[i]);
                }
            }
            else
            {
                var typeProperties = type.GetProperties();
                table.Columns.AddRange(typeProperties.Select(f => new DataColumn(f.Name, f.PropertyType)).ToArray());
                foreach (var item in obj)
                {
                    var row = table.NewRow();

                    foreach (PropertyInfo prop in typeProperties)
                    {
                        row[prop.Name] = prop.GetValue(item, null);
                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }
    }
}
