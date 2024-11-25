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
        //public string Password { get; set; } // Mật khẩu mới (nếu có)
        //public string ConfirmPassword { get; set; } // Xác nhận mật khẩu mới (nếu có)

        //public NguoiThue NguoiThue { get; set; }

        //public NguoiDung nguoiDung { get; set; }


        //public NguoiThueVM() { }



        // Dữ liệu từ bảng NguoiThue
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

        // Constructor để khởi tạo từ các đối tượng cơ sở dữ liệu
        public NguoiThueVM(NguoiThue nguoiThue, NguoiDung nguoiDung)
        {
            // Từ bảng NguoiThue
            IdNguoiThue = nguoiThue.IdNguoiThue;
            TenKhachHang = nguoiThue.TenKhachHang;
            SoDienThoai = nguoiThue.SoDienThoai;
            Email = nguoiThue.Email;
            DiaChi = nguoiThue.DiaChi;

            // Từ bảng NguoiDung
            IdNguoiDung = nguoiDung.IdNguoiDung;
            TenTaiKhoan = nguoiDung.TenTaiKhoan;
        }

        // Method cập nhật thông tin từ ViewModel vào NguoiThue
        public void UpdateNguoiThue(NguoiThue nguoiThue)
        {
            nguoiThue.TenKhachHang = this.TenKhachHang;
            nguoiThue.SoDienThoai = this.SoDienThoai;
            nguoiThue.Email = this.Email;
            nguoiThue.DiaChi = this.DiaChi;
        }

    }
}