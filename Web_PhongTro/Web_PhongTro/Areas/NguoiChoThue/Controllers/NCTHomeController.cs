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
                     //join yt in db.YeuThiches on baiDang.IdBaiDang equals yt.IdBaiDang
                     orderby baiDang.IdBaiDang descending
                     where username == nd.TenTaiKhoan

                     select new BaiDangVM
                     {
                         BaiDang = baiDang,
                         noidungPT = pt,
                         diachiPT = dc,
                         slyt = db.YeuThiches.Count(yt => yt.IdBaiDang == baiDang.IdBaiDang) // Lấy số lượng yêu thích

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
            string username = User.Identity.Name;

            //if (string.IsNullOrEmpty(username))
            //{
            //    return RedirectToAction("Login", "Account"); 
            //}

            var nguoiDung = db.NguoiDungs.FirstOrDefault(nd => nd.TenTaiKhoan == username);

            if (nguoiDung == null)
            {
                return View("Error");
            }
            var nguoiChoThue = db.NguoiChoThues.FirstOrDefault(nct => nct.IdNguoiDung == nguoiDung.IdNguoiDung);

            if (nguoiChoThue == null)
            {
                return View("Error");
            }

            // Tạo ViewModel để đẩy dữ liệu ra View
            var viewModel = new NguoiChoThueViewModel
            {
                IdNguoiChoThue = nguoiChoThue.IdNguoiChoThue,
                TenNguoiChoThue = nguoiChoThue.TenNguoiChoThue,
                SoDienThoai = nguoiChoThue.SoDienThoai,
                Email = nguoiChoThue.Email,
                DiaChi = nguoiChoThue.DiaChi,
                NgaySinh = nguoiChoThue.NgaySinh,
                GioiTinh = nguoiChoThue.GioiTinh,
                MatKhau = nguoiDung.MatKhau
            };

            return View(viewModel); // Truyền ViewModel ra View
        }
        [HttpPost]
        public ActionResult UpdateInfo(NguoiChoThueViewModel model, string currentPassword, string password, string confirmPassword)
        {
            string username = User.Identity.Name;

            // Tìm người dùng hiện tại trong cơ sở dữ liệu dựa trên tên tài khoản
            var user = (from u in db.NguoiDungs
                        where u.TenTaiKhoan == username
                        select u).FirstOrDefault();

            if (user == null)
            {
                ViewBag.ErrorMessage = "Người dùng không tồn tại.";
                return View("Info", model);
            }

            // Kiểm tra mật khẩu hiện tại chỉ khi người dùng muốn thay đổi mật khẩu
            if (!string.IsNullOrEmpty(password) && user.MatKhau != currentPassword)  // So sánh mật khẩu chỉ khi mật khẩu mới được nhập
            {
                ViewBag.ErrorMessage = "Mật khẩu hiện tại không chính xác.";
                return View("Info", model);
            }

            // Kiểm tra mật khẩu mới và xác nhận mật khẩu chỉ khi có mật khẩu mới được nhập
            if (!string.IsNullOrEmpty(password))
            {
                if (password != confirmPassword)
                {
                    ViewBag.ErrorMessage = "Mật khẩu mới và xác nhận mật khẩu không khớp.";
                    return View("Info", model);
                }

                // Cập nhật mật khẩu mới, lưu trực tiếp mật khẩu mà không mã hóa (nếu muốn lưu rõ ràng)
                user.MatKhau = password;
            }

            // Cập nhật thông tin người cho thuê
            var nguoiChoThue = (from n in db.NguoiChoThues
                                where n.IdNguoiDung == user.IdNguoiDung
                                select n).FirstOrDefault();

            if (nguoiChoThue == null)
            {
                ViewBag.ErrorMessage = "Thông tin người cho thuê không tồn tại.";
                return View("Info", model);
            }

            nguoiChoThue.TenNguoiChoThue = model.TenNguoiChoThue;
            nguoiChoThue.SoDienThoai = model.SoDienThoai;
            nguoiChoThue.Email = model.Email;
            nguoiChoThue.DiaChi = model.DiaChi;

            // Lưu thay đổi vào cơ sở dữ liệu
            db.SubmitChanges();

            // Thông báo thành công
            TempData["SuccessMessage"] = "Cập nhật thông tin thành công.";
            return RedirectToAction("Info");
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
        public ActionResult ThongBao(string sortCol, string sortType, int page = 1)
        {
             
            string username = User.Identity.Name.ToString();
            var query = (from kd in db.kiemduyetbaidangs
                         join bd in db.BaiDangs on kd.IdBaiDang equals bd.IdBaiDang
                         join nct in db.NguoiChoThues on bd.IdNguoiChoThue equals nct.IdNguoiChoThue
                         join nd in db.NguoiDungs on nct.IdNguoiDung equals nd.IdNguoiDung
                         where nd.TenTaiKhoan == username
                         select kd);
           
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