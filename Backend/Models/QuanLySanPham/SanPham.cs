namespace Backend.Models
{
    public class SanPham
    {
        public int id { get; set; }
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public double GiaSanPham { get; set; }
        public int DanhMucId { get; set; }
        public int DanhGia { get; set; }
        public string TrangThai { get; set; }
        public int SoLuong { get; set; }

    }
}
