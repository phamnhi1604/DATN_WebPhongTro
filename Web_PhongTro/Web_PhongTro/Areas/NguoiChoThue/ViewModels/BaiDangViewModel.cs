using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_PhongTro.ViewModels;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Web_PhongTro.Areas.NguoiChoThue.ViewModels
{
    public class BaiDangViewModel
    {
        public string ThanhPho { get; set; }
        public string Quan { get; set; }
        public string Phuong { get; set; }
        public string Duong { get; set; }

        // Fields for DanhMuc
        public int LoaiChuyenMuc { get; set; }

        // Fields for BaiDang
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }

        // Fields for PhongTro
        public int IdPhongTro { get; set; }
        public decimal Gia { get; set; }
        public string DonVi { get; set; }
        public decimal DienTich { get; set; }
        public string DoiTuong { get; set; }

        // Name of the contact person
        public string TenLienHe { get; set; }  
        public string Phone { get; set; }
        public int TrangThaiBaiDang { get; set; }
        public int IdNguoiChoThue { get; set; }
        public string AnhBaiDang { get; set; }
        public List<string> DanhSachAnh { get; set; }
    }
}