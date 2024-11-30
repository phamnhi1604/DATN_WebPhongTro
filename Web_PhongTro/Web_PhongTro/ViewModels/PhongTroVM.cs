using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_PhongTro.Models; 

namespace Web_PhongTro.ViewModels
{
    public class PhongTroVM
    {
        public PhongTro PhongTro { get; set; } // Thông tin phòng trọ
        public DiaChi DiaChi { get; set; } // Thông tin địa chỉ
        public NguoiChoThue NguoiChoThue { get; set; } // Thông tin người cho thuê

        
    }
}