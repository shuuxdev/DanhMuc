using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Models;
using Backend.Models.Context;
using Dapper;

namespace Backend.Services
{
    public class QuanLyRepository : IQuanLyRepository
    {
        private DapperContext _ctx;
        public QuanLyRepository(DapperContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<SanPham> Add(SanPham sanPham)
        {
            var procedure_name = "Insert_SanPham";

            var parameters = new
            {
                tenSanPham = sanPham.TenSanPham,
                giaSanPham = sanPham.GiaSanPham,
                danhMucId = sanPham.DanhMucId,
                danhGia = sanPham.DanhGia,
                trangThai = sanPham.TrangThai,
                soLuong = sanPham.SoLuong
            };
            using (var connection = _ctx.CreateConnection())
            {
                try
                {
                    var result = await connection.QuerySingleAsync<SanPham>(procedure_name, commandType: CommandType.StoredProcedure, param: parameters);
                    return result;
                }
                catch (Exception e)
                {

                    throw new InvalidOperationException(e.Message);
                }
            }
        }

        public async Task<int> Edit(SanPham sanPham)
        {
            var procedure_name = "Update_SanPham";

            var parameters = new
            {
                Id = sanPham.id,
                maSanPham = sanPham.MaSanPham,
                tenSanPham = sanPham.TenSanPham,
                giaSanPham = sanPham.GiaSanPham,
                danhMucId = sanPham.DanhMucId,
                danhGia = sanPham.DanhGia,
                trangThai = sanPham.TrangThai,
                soLuong = sanPham.SoLuong
            };
            using (var connection = _ctx.CreateConnection())
            {
                var resultCode = await connection.ExecuteAsync(procedure_name, commandType: CommandType.StoredProcedure, param: parameters);
                return resultCode;
            }
        }

        public async Task<IEnumerable<SanPham>> GetAll()
        {
            var procedure_name = "GetAll_SanPham";
            using (var connection = _ctx.CreateConnection())
            {
                var result = await connection.QueryAsync<SanPham>(procedure_name, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<int> Remove(int id)
        {
            var parameters = new { Id = id };
            var procedure_name = "Delete_SanPham";
            using (var connection = _ctx.CreateConnection())
            {
                var resultCode = await connection.ExecuteAsync(procedure_name, commandType: CommandType.StoredProcedure, param: parameters);
                return resultCode;
            }
        }

        public async Task<int> RemoveList(List<DeleteItem> deleteList)
        {

            string procedure_name = "Delete_ListSanPham";
            var dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Rows.Add();
            foreach (var item in deleteList)
            {
                dt.Rows.Add(item.id);
            }
            var parameters = new DynamicParameters();
            parameters.Add("@List", dt.AsTableValuedParameter("[dbo].[ListDeleteItem]"));
            using (var connection = _ctx.CreateConnection())
            {
                var deleted_row = await connection.ExecuteScalarAsync<int>(procedure_name, commandType: CommandType.StoredProcedure, param: parameters);
                return deleted_row;
            }
        }
    }
}
