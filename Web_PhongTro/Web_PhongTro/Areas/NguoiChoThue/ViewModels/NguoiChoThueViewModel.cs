using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_PhongTro.Areas.NguoiChoThue.ViewModels
{
    public class NguoiChoThueViewModel
    {
        public string TenLienHe { get; set; }
        public string SoDienThoai { get; set; }
        public long IdNguoiChoThue { get; set; }

        [Required]
        public long IdNguoiDung { get; set; } // Liên kết với bảng NguoiDung

        [Required]
        [StringLength(255)]
        public string TenNguoiChoThue { get; set; } // Họ và tên của người cho thuê

        public DateTime? NgaySinh { get; set; } // Ngày sinh (cho phép null)

        [StringLength(5)]
        public string GioiTinh { get; set; } // Giới tính (Nam, Nữ, Khác) 

        [StringLength(30)]
        public string Emai { get; set; }

        [StringLength(255)]
        public string DiaChi { get; set; } // Địa chỉ của người cho thuê


        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } // Email (độc nhất)
        public string MatKhau { get; set; }

        // Navigation property nếu cần (có thể bỏ nếu không cần liên kết tự động)
        //[ForeignKey("IdNguoiDung")]
        //public virtual NguoiDung NguoiDung { get; set; }
    }
}