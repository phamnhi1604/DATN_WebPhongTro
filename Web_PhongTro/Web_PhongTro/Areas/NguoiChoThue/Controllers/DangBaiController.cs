using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_PhongTro.Models;
using Web_PhongTro.Areas.NguoiChoThue.ViewModels;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Web_PhongTro.Areas.NguoiChoThue.Controllers
{
    public class DangBaiController : Controller
    {
        PhongTroDataContext db = new PhongTroDataContext();

        // GET: NguoiChoThue/DangBai
        public ActionResult DanhSachDiaChi()
        {
            var viewModel = new DiaChiViewModel
            {
                //TinhThanhList = db.DiaChis.Select(d => d.ThanhPho).Distinct().ToList(),
                //QuanHuyenList = db.DiaChis.Select(d => d.Quan).Distinct().ToList(),
                //PhuongXaList = db.DiaChis.Select(d => d.Phuong).Distinct().ToList()
                TinhThanhList = db.DiaChis.Select(d => d.ThanhPho).Distinct().ToList(),
                QuanHuyenList = db.DiaChis.Select(d => d.Quan).Distinct().ToList(),
                PhuongXaList = new List<string>()
            };

            return View(viewModel);
        }
        public JsonResult GetWardsByDistrict(string district)
        {
            var wards = db.DiaChis
                         .Where(d => d.Quan == district)
                         .Select(d => d.Phuong)
                         .Distinct()
                         .ToList();

            return Json(wards, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDanhSachPhongTro()
        {
            List<long> Idphongs = db.PhongTros.Select(p => p.IdPhongTro).ToList();
            PhongTroViewModel viewModel = new PhongTroViewModel
            {
                IdPhong = Idphongs
            };

            return View(viewModel);
        }
        public ActionResult GetDanhSachDanhMuc()
        {
            var danhMucList = db.DanhMucs.Select(d => new SelectListItem
            {
                Value = d.IdDanhMuc.ToString(),
                Text = d.TenDanhMuc
            }).ToList();

            var model = new DanhMucViewModel
            {
                DanhMucList = danhMucList
            };

            return View(model);
        }
        //public ActionResult ThongTinLienHe()
        //{
        //    var userId = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        //    var nguoiChoThue = (from nd in db.NguoiDungs
        //                        join nct in db.NguoiChoThues on nd.IdNguoiDung equals nct.IdNguoiDung
        //                        where nd.IdNguoiDung == userId
        //                        select new NguoiChoThueViewModel
        //                        {
        //                            TenLienHe = nct.TenNguoiChoThue,
        //                            SoDienThoai = nct.SoDienThoai
        //                        }).FirstOrDefault();

        //    if (nguoiChoThue == null)
        //    {
        //        return View("Error");
        //    }

        //    return View(nguoiChoThue);
        //}
        public ActionResult DangBai()
        {
            return View();
        }


        //public JsonResult ThemBai(BaiDangViewModel model)
        //{
        //    var res = new { success = false, message = "Thêm bài đăng không thành công" };

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
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

        //            // Kiểm tra danh mục
        //            var danhMuc = db.DanhMucs.FirstOrDefault(dm => dm.IdDanhMuc == model.LoaiChuyenMuc);
        //            if (danhMuc == null)
        //            {
        //                return Json(new { success = false, message = "Danh mục không tồn tại." });
        //            }

        //            // Tìm phòng trọ có sẵn, ví dụ dựa vào IdNguoiChoThue và trạng thái phòng
        //            var phongTro = db.PhongTros.FirstOrDefault(pt => pt.IdNguoiChoThue == idNguoiChoThue && pt.TrangThaiPhong == "con_trong");

        //            if (phongTro == null)
        //            {
        //                return Json(new { success = false, message = "Không có phòng trọ có sẵn để đăng." });
        //            }

        //            // Thêm bài đăng liên kết với phòng trọ có sẵn
        //            BaiDang newBd = new BaiDang
        //            {
        //                IdNguoiChoThue = idNguoiChoThue, 
        //                IdPhongTro = phongTro.IdPhongTro,
        //                TieuDe = model.TieuDe,
        //                NoiDung = model.NoiDung,
        //                AnhBaiDang = "def_img.jpg", 
        //                NgayDang = DateTime.Now,
        //                TrangThai = "dang_hoat_dong", 
        //            };
        //            db.BaiDangs.InsertOnSubmit(newBd);
        //            db.SubmitChanges();

        //            res = new { success = true, message = "Đăng bài thành công!" };
        //        }
        //        catch (Exception ex)
        //        {
        //            res = new { success = false, message = "Đã xảy ra lỗi: " + ex.Message };
        //        }
        //    }

        //    return Json(res);
        //}
        public JsonResult ThemBai(BaiDangViewModel model)
        {
            var res = new { success = false, message = "Thêm bài đăng không thành công" };

            if (ModelState.IsValid)
            {
                try
                {
                    string username = User.Identity.Name;

                    // Lấy IdNguoiChoThue từ tài khoản đăng nhập
                    var idNguoiChoThue = (from nguoiDung in db.NguoiDungs
                                          join nct in db.NguoiChoThues on nguoiDung.IdNguoiDung equals nct.IdNguoiDung
                                          where nguoiDung.TenTaiKhoan == username
                                          select nct.IdNguoiChoThue).FirstOrDefault();

                    if (idNguoiChoThue == null)
                    {
                        return Json(new { success = false, message = "Người cho thuê không tồn tại." });
                    }

                    // Kiểm tra danh mục
                    var danhMuc = db.DanhMucs.FirstOrDefault(dm => dm.IdDanhMuc == model.LoaiChuyenMuc);
                    if (danhMuc == null)
                    {
                        return Json(new { success = false, message = "Danh mục không tồn tại." });
                    }

                    // Lấy phòng trọ còn trống
                    var phongTro = db.PhongTros.FirstOrDefault(pt => pt.IdNguoiChoThue == idNguoiChoThue);
                    if (phongTro == null)
                    {
                        return Json(new { success = false, message = "Không có phòng trọ có sẵn để đăng." });
                    }

                    // Tạo bài đăng mới
                    BaiDang newBd = new BaiDang
                    {
                        IdNguoiChoThue = idNguoiChoThue,
                        IdPhongTro = phongTro.IdPhongTro,
                        TieuDe = model.TieuDe,
                        NoiDung = model.NoiDung,
                        AnhBaiDang = "def_img.jpg", // Ảnh mặc định
                        NgayDang = DateTime.Now,
                        TrangThai = "0",
                        SoLuongYeuThich = 0
                    };

                    // Thêm bài đăng vào cơ sở dữ liệu
                    db.BaiDangs.InsertOnSubmit(newBd);
                    db.SubmitChanges();



                    res = new { success = true, message = "Đăng bài thành công!" };
                }
                catch (Exception ex)
                {
                    res = new { success = false, message = "Đã xảy ra lỗi: " + ex.Message };
                }
            }

            return Json(res);
        }


    }
}