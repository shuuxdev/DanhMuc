using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Models;
namespace Backend.Services
{
    public interface IQuanLyRepository
    {
        Task<SanPham> Add(SanPham sanpham);
        Task<int> Remove(int id);
        Task<IEnumerable<SanPham>> GetAll();

        Task<int> Edit(SanPham sanpham);
        Task<int> RemoveList(List<DeleteItem> deleteList);
    }
}