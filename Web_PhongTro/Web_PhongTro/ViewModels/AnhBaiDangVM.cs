using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_PhongTro.Models;

namespace Web_PhongTro.ViewModels
{
    public class String
    {
        PhongTroDataContext db = new PhongTroDataContext();

        public BaiDang baiDang { get; set; }
        public int idBD { get; set; }
        public string ddAnh { get; set; }
        public String(int idBD)
        {
            this.idBD = idBD;
            var baidang = db.AnhBaiDangs.Single(x => x.IdBaiDang == idBD);
            this.ddAnh = baidang.DuongDanAnh;
        }
    }
}