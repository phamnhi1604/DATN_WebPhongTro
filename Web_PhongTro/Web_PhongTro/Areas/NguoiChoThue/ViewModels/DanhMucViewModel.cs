using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Web_PhongTro.Areas.NguoiChoThue.ViewModels
{
    public class DanhMucViewModel
    {
        public int IdDanhMuc { get; set; } // ID của danh mục
        public string TenDanhMuc { get; set; } // Tên của danh mục
        public List<SelectListItem> DanhMucList { get; set; } // Đổi thành List<SelectListItem>
    }
}