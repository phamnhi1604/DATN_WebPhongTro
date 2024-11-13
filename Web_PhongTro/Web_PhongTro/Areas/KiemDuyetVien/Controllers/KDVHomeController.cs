using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_PhongTro.ViewModels;
using Web_PhongTro.Models;
namespace Web_PhongTro.Areas.KiemDuyetVien.Controllers
{
    public class KDVHomeController : Controller
    {
        PhongTroDataContext db = new PhongTroDataContext();
        // GET: KiemDuyetVien/KDVHome
        public ActionResult DashBoard()
        {
            IEnumerable<BaiDangVM> query = null;
            query = (from baiDang in db.BaiDangs
                     join nd in db.PhongTros on baiDang.IdPhongTro equals nd.IdPhongTro
                     join dc in db.DiaChis on nd.IdDiaChi equals dc.IdDiaChi
                     orderby baiDang.IdBaiDang descending
                     select new BaiDangVM
                     {
                         BaiDang = baiDang,
                         noidungPT = nd,
                         diachiPT = dc
                         //Gia = db.func_GiaBaiDang(BaiDang.IdBaiDang)
                     });
            return View(query);
        }

    }
}