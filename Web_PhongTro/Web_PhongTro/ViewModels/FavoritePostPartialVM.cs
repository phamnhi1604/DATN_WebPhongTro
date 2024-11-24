using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_PhongTro.ViewModels;
using Web_PhongTro.Models;

namespace Web_PhongTro.ViewModels
{
    public class FavoritePostPartialVM
    {
        PhongTroDataContext db = new PhongTroDataContext();

        public int IdBaiDang { get; set; }
        public string TieuDe { get; set; }
        public string AnhBD { get; set; }
        public FavoritePostPartialVM(int IdBaiDang)
        {
            this.IdBaiDang = IdBaiDang;
            var post = db.BaiDangs.Single(x => x.IdBaiDang == IdBaiDang);
            this.TieuDe = post.TieuDe;
            this.AnhBD = post.AnhBaiDang;
        }
    }
}