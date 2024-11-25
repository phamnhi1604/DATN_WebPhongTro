using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_PhongTro.ViewModels;
namespace Web_PhongTro.Areas.NguoiChoThue.ViewModal
{
    public class PostVM
    {
        
        public long IdBaiDang;

        public string TenBaiDang;

        public int IdDanhMuc;

        public string AnhSP;

        public List<string> listDdAnh { get; set; }


        public Nullable<long> GiaBan;

        public string NoiDungSanPham;

        public string DanhGiaSanPham;

        public string ThanhToanVanChuyen;

        public string TonTai;

        public long? Gia; 
        public PostVM(BaiDangVM sp)
        {
            IdBaiDang = sp.BaiDang.IdBaiDang;
            TenBaiDang = sp.BaiDang.TieuDe;
            IdDanhMuc = sp.noidungPT.IdDanhMuc;
            AnhSP = sp.BaiDang.AnhBaiDang;
            listDdAnh = sp.listDdAnh;
            GiaBan = (long?)sp.noidungPT.GiaPhong;
            NoiDungSanPham = sp.BaiDang.NoiDung;
            TonTai = sp.BaiDang.TrangThai;
        }
    }


}