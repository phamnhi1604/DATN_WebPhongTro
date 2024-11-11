using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_PhongTro.Models;
using Web_PhongTro.ViewModels;

namespace Web_PhongTro.Controllers
{
    public class HomeController : Controller
    {
        PhongTroDataContext db = new PhongTroDataContext();
        // GET: Home
        public ActionResult Index()
        {
            //List<string> tenDanhMucList = db.DanhMucs.Select(ldm => ldm.TenDanhMuc).ToList();
            //foreach(var tenDanhMuc in tenDanhMucList)
            //{
            //    ViewData[tenDanhMuc] = GetBaiDangPartial(tenDanhMuc);
            //}
            //return View(tenDanhMucList);
            var query = (from danhMuc in db.DanhMucs
                         join phongTro in db.PhongTros on danhMuc.IdDanhMuc equals phongTro.IdDanhMuc
                         join baiDang in db.BaiDangs on phongTro.IdPhongTro equals baiDang.IdPhongTro 
                         orderby baiDang.IdBaiDang descending
                         select new BaiDangVM
                         {
                             noidungPT = phongTro,
                             BaiDang = baiDang 
                         }).Take(4);
            return View(query);
        }

        public List<BaiDangVM> GetBaiDangPartial(string tenDanhMuc)
        {
            var query = ( from danhMuc in db.DanhMucs
                          join phongTro in db.PhongTros on danhMuc.IdDanhMuc equals phongTro.IdDanhMuc
                          join baiDang in db.BaiDangs on phongTro.IdPhongTro equals baiDang.IdPhongTro
                          //join anhBD in db.AnhBaiDangs on baiDang.IdBaiDang equals anhBD.IdBaiDang
                          where danhMuc.TenDanhMuc == tenDanhMuc
                          orderby baiDang.IdBaiDang descending
                          select new BaiDangVM
                          {
                              noidungPT = phongTro,
                              BaiDang  =  baiDang

                          }).Take(4);
            return query.ToList();
        }

        public ActionResult Dashboard()
        {
            return View();
        }
    }
}