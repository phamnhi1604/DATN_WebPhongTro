using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_PhongTro.Areas.NguoiChoThue.ViewModels;
using Web_PhongTro.Models;
using Web_PhongTro.ViewModels;

namespace Web_PhongTro.Areas.NguoiChoThue.Controllers
{
    public class NCTHomeController : Controller
    {
        PhongTroDataContext db = new PhongTroDataContext();
        // GET: NguoiChoThue/NCTHome
        public ActionResult Dashboard(string productSearchType, string sortCol, string sortType, int page = 1)
        {
            string username = User.Identity.Name.ToString();
            IEnumerable<BaiDangVM> query = null;

            query = (from baiDang in db.BaiDangs
                     join pt in db.PhongTros on baiDang.IdPhongTro equals pt.IdPhongTro
                     join dc in db.DiaChis on pt.IdDiaChi equals dc.IdDiaChi
                     join nct in db.NguoiChoThues on baiDang.IdNguoiChoThue equals nct.IdNguoiChoThue
                     join nd in db.NguoiDungs on nct.IdNguoiDung equals nd.IdNguoiDung
                     orderby baiDang.IdBaiDang descending
                     where username == nd.TenTaiKhoan

                     select new BaiDangVM
                     {
                         BaiDang = baiDang,
                         noidungPT = pt,
                         diachiPT = dc

                     });



            // Paging
            int NoOfRecordPerPage = 12;
            int NoOfPages = (int)Math.Ceiling((double)query.Count() / NoOfRecordPerPage);
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.STT = (page - 1) * NoOfRecordPerPage + 1;
            ViewBag.NoOfPages = NoOfPages;
            query = query.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage);



            return View(query);
        }

        public ActionResult Info()
        {
            return View();
        }
        public ActionResult AddPartial()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var lsp in db.DanhMucs.ToList())
            {
                items.Add(new SelectListItem
                {
                    Value = lsp.IdDanhMuc.ToString(),
                    Text = lsp.TenDanhMuc
                });
            }

            ViewBag.SPLoaiCha = items;

            return PartialView();
        }
        public ActionResult ChiTietBaiDang(int id)
        {
            var query = (from baiDang in db.BaiDangs
                         join nd in db.PhongTros on baiDang.IdPhongTro equals nd.IdPhongTro
                         join dc in db.DiaChis on nd.IdDiaChi equals dc.IdDiaChi
                         join ddA in db.AnhBaiDangs on baiDang.IdBaiDang equals ddA.IdBaiDang
                         join thongtinNguoiChoThue in db.NguoiChoThues on baiDang.IdNguoiChoThue equals thongtinNguoiChoThue.IdNguoiChoThue
                         where baiDang.IdBaiDang == id
                         select new BaiDangVM
                         {
                             BaiDang = baiDang,
                             noidungPT = nd,
                             diachiPT = dc,
                             nguoiChoThue = thongtinNguoiChoThue,
                             listDdAnh = db.AnhBaiDangs
                                       .Where(a => a.IdBaiDang == baiDang.IdBaiDang)
                                       .Select(a => a.DuongDanAnh)
                                       .ToList()

                         }).Take(1);
            return View(query);
        }
        [HttpPost]

        public ActionResult EditPartial()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var lsp in db.DanhMucs.ToList())
            {
                items.Add(new SelectListItem
                {
                    Value = lsp.IdDanhMuc.ToString(),
                    Text = lsp.TenDanhMuc
                });
            }

            ViewBag.DanhMuc = items;
            return PartialView();
        }

        public ActionResult DanhSachPhongTro(string sortCol, string sortType, int page = 1)
        {
            // Lấy tên người dùng đăng nhập
            string username = User.Identity.Name.ToString();

            // Lấy danh sách phòng trọ của người cho thuê đã đăng nhập
            var query = (from pt in db.PhongTros
                         join dc in db.DiaChis on pt.IdDiaChi equals dc.IdDiaChi
                         join nct in db.NguoiChoThues on pt.IdNguoiChoThue equals nct.IdNguoiChoThue
                         join nd in db.NguoiDungs on nct.IdNguoiDung equals nd.IdNguoiDung
                         where nd.TenTaiKhoan == username
                         select new PhongTroVM
                         {
                             PhongTro = pt,
                             DiaChi = dc,
                             NguoiChoThue = nct
                         });

            // Sắp xếp
            //if (!string.IsNullOrEmpty(sortCol) && !string.IsNullOrEmpty(sortType))
            //{
            //    query = sortType.ToLower() == "asc"
            //        ? query.OrderBy(x => EF.Property<object>(x.PhongTro, sortCol))
            //        : query.OrderByDescending(x => EF.Property<object>(x.PhongTro, sortCol));
            //}

            // Phân trang
            int NoOfRecordPerPage = 12;
            int NoOfPages = (int)Math.Ceiling((double)query.Count() / NoOfRecordPerPage);
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;

            ViewBag.Page = page;
            ViewBag.STT = (page - 1) * NoOfRecordPerPage + 1;
            ViewBag.NoOfPages = NoOfPages;

            query = query.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage);

            return View(query.ToList());
        }
        //public JsonResult ThemPhongTro(PhongTroViewModel model)
        //{
        //    var res = new { success = false, message = "Thêm phòng trọ không thành công" };

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            // Lấy thông tin người dùng hiện tại
        //            string username = User.Identity.Name;

        //            // Lấy IdNguoiChoThue từ tài khoản đăng nhập
        //            var idNguoiChoThue = (from nguoiDung in db.NguoiDungs
        //                                  join nct in db.NguoiChoThues on nguoiDung.IdNguoiDung equals nct.IdNguoiDung
        //                                  where nguoiDung.TenTaiKhoan == username
        //                                  select nct.IdNguoiChoThue).FirstOrDefault();

        //            if (idNguoiChoThue == null)
        //            {
        //                return Json(new { success = false, message = "Người cho thuê không tồn tại." });
        //            }

        //            // Kiểm tra xem danh mục có tồn tại hay không
        //        }
        //    }
        //}
    }
}