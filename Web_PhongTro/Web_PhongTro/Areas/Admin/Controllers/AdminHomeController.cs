using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web_PhongTro.Areas.KiemDuyetVien;
using Web_PhongTro.Models;
using Web_PhongTro.ViewModels;

namespace Web_PhongTro.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        PhongTroDataContext db = new PhongTroDataContext();
        // GET: Admin/AdminHome
        public ActionResult DashBoard()
        {
            return View();
        }
        public ActionResult BaiDang(string postSearchType, string postSearchInput, string sortCol, string sortType, int page = 1)
        {
            IEnumerable<BaiDangVM> query = null;
            if (!string.IsNullOrEmpty(postSearchType) && !string.IsNullOrEmpty(postSearchInput))
            {
                query = db.BaiDangs
                        .Where(p => p.TieuDe.Contains(postSearchInput))
                        .Select(baiDang => new BaiDangVM
                        {
                            BaiDang = baiDang
                        });
            }
            else
            {
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
            }



            // Paging
            int NoOfRecordPerPage = 12;
            int NoOfPages = (int)Math.Ceiling((double)query.Count() / NoOfRecordPerPage);
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.STT = (page - 1) * NoOfRecordPerPage + 1;
            ViewBag.NoOfPages = NoOfPages;
            query = query.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage);
            if (!string.IsNullOrEmpty(sortCol) && !string.IsNullOrEmpty(sortType))
            {
                //switch (sortCol)
                //{
                //    case "GiaDaGiam":
                //        if (sortType == "ASC")
                //            query = query.OrderBy(x => x.Gia);
                //        else if (sortType == "DESC")
                //            query = query.OrderByDescending(x => x.Gia);
                //        else return HttpNotFound();
                //        break;
                //    case "GiaGoc":
                //        if (sortType == "ASC")
                //            query = query.OrderBy(x => x.BaiDang.GiaBan);
                //        else if (sortType == "DESC")
                //            query = query.OrderByDescending(x => x.BaiDang.GiaBan);
                //        else return HttpNotFound();
                //        break;
                //    case "GiamGia":
                //        if (sortType == "ASC")
                //            query = query.OrderBy(x => x.BaiDang.GiamGia);
                //        else if (sortType == "DESC")
                //            query = query.OrderByDescending(x => x.BaiDang.GiamGia);
                //        else return HttpNotFound();
                //        break;
                //    case "SoLuongDanhGia":
                //        if (sortType == "ASC")
                //            query = query.OrderBy(x => x.BaiDang.SoLuongDanhGia);
                //        else if (sortType == "DESC")
                //            query = query.OrderByDescending(x => x.BaiDang.SoLuongDanhGia);
                //        else return HttpNotFound();
                //        break;
                //    default:
                //        return HttpNotFound();
                //}
            }
            return View(query);
        }
        public ActionResult NhanVien(string NhanVienSearchType, string NhanVienSearchInput, string sortCol, string sortType, int page = 1)
        {
            //IEnumerable<BaiDangVM> query = null;
            var query = (from KiemDuyetVien in db.KiemDuyetViens
                         select KiemDuyetVien);
            int NoOfRecordPerPage = 12;
            int NoOfPages = (int)Math.Ceiling((double)query.Count() / NoOfRecordPerPage);
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.STT = (page - 1) * NoOfRecordPerPage + 1;
            ViewBag.NoOfPages = NoOfPages;
            query = query.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage);
            return View(query);
        }

        public PartialViewResult TaoTKKDVPartial()
        {
            return PartialView();
        }

        public JsonResult addTKKDV(UserVM lg)
        {
            var res = new { success = false, message = "Tạo tài khoản thất bại" };
            if (ModelState.IsValid)
            {
                bool existsUsername = db.NguoiDungs.Any(x => x.TenTaiKhoan == lg.Username);

                if (existsUsername)
                {
                    return Json(new { success = false, message = "Tài khoản đã tồn tại" });
                }

                if (lg.Password != lg.ConfirmPassword)
                {
                    return Json(new { success = false, message = "Mật khẩu và xác nhận mật khẩu không khớp" });
                }
                try
                {
                    // Xác định vai trò
                    //int roleId = lg.AccountType == "Người cho thuê" ? 3 : 4;

                    // Tạo NguoiDung
                    NguoiDung u = new NguoiDung()
                    {
                        TenTaiKhoan = lg.Username,
                        MatKhau = lg.Password,
                        IdVaiTro = 2,
                        TonTai = true
                    };

                    db.NguoiDungs.InsertOnSubmit(u);
                    db.SubmitChanges();

                    // Lấy IdNguoiDung mới được tạo
                    long newUserId = u.IdNguoiDung;

                    // Thêm bản ghi vào bảng tương ứng

                    Web_PhongTro.Models.KiemDuyetVien kdv = new Web_PhongTro.Models.KiemDuyetVien()
                    {
                        IdNguoiDung = newUserId
                    };
                    db.KiemDuyetViens.InsertOnSubmit(kdv);

                    return Json(new
                    {
                        success = true,
                        message = "Đăng ký thành công"
                    });
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Đã xảy ra lỗi: " + ex.Message
                    });
                }
            }

            return Json(res);
        }

        public ActionResult NguoiThue(string NhanVienSearchType, string NhanVienSearchInput, string sortCol, string sortType, int page = 1)
        {
            //IEnumerable<BaiDangVM> query = null;
            var query = (from NguoiThue in db.NguoiThues
                         select NguoiThue);
            int NoOfRecordPerPage = 12;
            int NoOfPages = (int)Math.Ceiling((double)query.Count() / NoOfRecordPerPage);
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.STT = (page - 1) * NoOfRecordPerPage + 1;
            ViewBag.NoOfPages = NoOfPages;
            query = query.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage);
            return View(query);
        }
        public ActionResult NguoiChoThue(string NhanVienSearchType, string NhanVienSearchInput, string sortCol, string sortType, int page = 1)
        {
            //IEnumerable<BaiDangVM> query = null;
            var query = (from NguoiChoThue in db.NguoiChoThues
                         //join NguoiThue in db.NguoiThues on NguoiChoThue.IdNguoiDung equals NguoiThue.IdNguoiDung
                         select NguoiChoThue);
            int NoOfRecordPerPage = 12;
            int NoOfPages = (int)Math.Ceiling((double)query.Count() / NoOfRecordPerPage);
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.STT = (page - 1) * NoOfRecordPerPage + 1;
            ViewBag.NoOfPages = NoOfPages;
            query = query.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage);
            return View(query);
        }
        public ActionResult PhanHoi(string NhanVienSearchType, string NhanVienSearchInput, string sortCol, string sortType, int page = 1)
        {
            var query = (from PhanHoi in db.PhanHois
                         join NguoiThue in db.NguoiThues on PhanHoi.IdNguoiThue equals NguoiThue.IdNguoiThue
                         select PhanHoi);
            int NoOfRecordPerPage = 12;
            int NoOfPages = (int)Math.Ceiling((double)query.Count() / NoOfRecordPerPage);
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.STT = (page - 1) * NoOfRecordPerPage + 1;
            ViewBag.NoOfPages = NoOfPages;
            query = query.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage);
            return View(query);
        }

        public ActionResult TaiKhoan(string postSearchType, string postSearchInput, string sortCol, string sortType, int page = 1)
        {

            var query = (from NguoiDung in db.NguoiDungs
                         select NguoiDung);
            int NoOfRecordPerPage = 12;
            int NoOfPages = (int)Math.Ceiling((double)query.Count() / NoOfRecordPerPage);
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.STT = (page - 1) * NoOfRecordPerPage + 1;
            ViewBag.NoOfPages = NoOfPages;
            query = query.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage);
            return View(query);
        }


        public JsonResult ResetPassWord(long idND)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    var tk = db.NguoiDungs.Where(x => x.IdNguoiDung == idND).FirstOrDefault();

                    if (tk != null)
                    {
                        tk.MatKhau = "12345678";
                        db.SubmitChanges();

                        return Json(new
                        {
                            success = true,
                            message = "Đặt lại mật khẩu thành công! "
                        });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Đã xảy ra lỗi: " + ex.Message
                    });
                }
            }

            // Nếu ModelState không hợp lệ
            return Json(new
            {
                success = false,
                message = "Dữ liệu không hợp lệ!"
            });
        }
    }
}