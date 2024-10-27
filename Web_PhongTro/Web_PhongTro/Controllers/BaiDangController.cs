using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_PhongTro.Models;
using Web_PhongTro.ViewModels;

namespace Web_PhongTro.Controllers
{
    public class BaiDangController : Controller
    {
        PhongTroDataContext db = new PhongTroDataContext();
        // GET: BaiDang
        public ActionResult BaiDang()
        {
            return View();
        }

        public ActionResult BaiDangNoiBat(int page = 1)
        {

            return View();
        }

        public ActionResult GetBaiDangById( int? id)
        {
            if(id == 0 || id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var query = (from baiDang in db.BaiDangs
                         join ddA in db.AnhBaiDangs on baiDang.IdBaiDang equals ddA.IdBaiDang
                         where baiDang.IdBaiDang == id
                         select new BaiDangVM
                         {
                             BaiDang = baiDang,
                             listDdAnh = db.AnhBaiDangs
                                       .Where(a => a.IdBaiDang == baiDang.IdBaiDang)
                                       .Select(a => a.DuongDanAnh)
                                       .ToList()
                         }).Take(1);
            return View(query);
        }
    }
}