using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;
using Web_PhongTro.Models;

namespace Web_PhongTro.ViewModels
{
    public class NguoiThueVM
    {
        public long IdNguoiThue { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }

        // Dữ liệu từ bảng NguoiDung
        public long IdNguoiDung { get; set; }
        public string TenTaiKhoan { get; set; }
        public string Password { get; set; } // Mật khẩu mới (nếu có)
        public string ConfirmPassword { get; set; } // Xác nhận mật khẩu mới (nếu có)

        public NguoiThueVM() { }

        public NguoiThueVM(NguoiThue nguoiThue, NguoiDung nguoiDung)
        {
            IdNguoiThue = nguoiThue.IdNguoiThue;
            TenKhachHang = nguoiThue.TenKhachHang;
            SoDienThoai = nguoiThue.SoDienThoai;
            Email = nguoiThue.Email;
            DiaChi = nguoiThue.DiaChi;

            IdNguoiDung = nguoiDung.IdNguoiDung;
            TenTaiKhoan = nguoiDung.TenTaiKhoan;
        }

        public void UpdateNguoiThue(NguoiThue nguoiThue)
        {
            nguoiThue.TenKhachHang = this.TenKhachHang;
            nguoiThue.SoDienThoai = this.SoDienThoai;
            nguoiThue.Email = this.Email;
            nguoiThue.DiaChi = this.DiaChi;
        }

    }
}