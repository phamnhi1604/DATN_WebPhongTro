using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_PhongTro.Models;
using Web_PhongTro.ViewModels;

namespace Web_PhongTro.Controllers
{
    public class DanhMucController : Controller
    {
        PhongTroDataContext db = new PhongTroDataContext();
        // GET: DanhMuc
        public ActionResult DanhMucPartial()
        {
            var lstDanhMuc = db.DanhMucs.ToList();
            //ViewData[lstDanhMuc] = GetDanhMuc();
            
            return PartialView(lstDanhMuc);
        }
        public List<DanhMuc> GetDanhMuc()
        {
            var query = ( from l in db.DanhMucs select l);
            return query.ToList();
        }

        public ActionResult ListDiaChiPartial()
        {
            var query = db.DiaChis
                          .Select(diachi => diachi.Quan)
                          .Distinct()
                          .ToList();
            return PartialView(query);
        }
        public ActionResult ListGiaPartial()
        {
            var query = db.DiaChis
                          .Select(diachi => diachi.Quan)
                          .Distinct()
                          .ToList();
            return PartialView(query);
        }
        public ActionResult ListDienTichPartial()
        {
            var query = from l in db.DanhMucs select l;
            return PartialView(query);
        }
    }
}