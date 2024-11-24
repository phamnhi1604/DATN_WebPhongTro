using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_PhongTro.Models;

namespace Web_PhongTro.ViewModels
{
    public class NguoiThueVM
    {
        PhongTroDataContext db = new PhongTroDataContext();
        public NguoiThue NguoiThue { get; set; }

        public NguoiDung nguoiDung { get; set; }

        public NguoiThueVM() { }

    }
}