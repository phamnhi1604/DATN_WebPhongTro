using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_PhongTro.Models;

namespace Web_PhongTro.ViewModels
{
    public class BaiDangVM
    {
        public BaiDang BaiDang { get; set; }
        public List<string> listDdAnh { get; set; }

        public BaiDangVM() { }
    }
}