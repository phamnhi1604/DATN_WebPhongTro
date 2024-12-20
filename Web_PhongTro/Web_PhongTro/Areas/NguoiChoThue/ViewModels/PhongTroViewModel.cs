using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_PhongTro.Models;

namespace Web_PhongTro.Areas.NguoiChoThue.ViewModels
{
    public class PhongTroViewModel
    {
        public string ThanhPho { get; set; }
        public string Quan { get; set; }
        public string Phuong { get; set; }
        public string Duong { get; set; }
        public int LoaiChuyenMuc { get; set; }

        public string DiaChi { get; set; }
        public string NoiDung { get; set; }
        public List<long> IdPhong { get; set; }
        public List<PhongTro> MotaPT { get; set; }
        public decimal GiaPhong { get; set; }
        public string DonVi { get; set; }
        public decimal DienTich { get; set; }
        public int IdNguoiChoThue { get; set; }
        public int SoPhongTrong { get; set; }
        public string MoTa { get; set; }
    }
}