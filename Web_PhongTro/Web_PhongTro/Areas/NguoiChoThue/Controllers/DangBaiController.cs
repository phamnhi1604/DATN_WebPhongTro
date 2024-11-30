using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_PhongTro.Models;
using Web_PhongTro.Areas.NguoiChoThue.ViewModels;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IO; 
using Microsoft.AspNetCore.Http;
using System.Web.UI.WebControls;


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
        public PartialViewResult DangBai()
        {
            return PartialView();
        }

        //private string SaveFileAndGetPath(HttpPostedFileBase imageFile)
        //{
        //    // Kiểm tra nếu không có ảnh được chọn


        //    // Đặt đường dẫn thư mục lưu ảnh
        //    string directoryPath = Server.MapPath("~/Contents/Images/ImgPost/");

        //    // Kiểm tra và tạo thư mục nếu không tồn tại
        //    if (!Directory.Exists(directoryPath))
        //    {
        //        Directory.CreateDirectory(directoryPath);
        //    }

        //    // Tạo tên file duy nhất cho ảnh
        //    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
        //    string filePath = Path.Combine(directoryPath, fileName);

        //    imageFile.SaveAs(filePath);

        //    // Trả về thông báo thành công và đường dẫn ảnh
        //    return filePath = "/Contents/Images/ImgPost/" + fileName;

        //    // Trả về đường dẫn tương đối để lưu vào database
        //}

        //private void SaveFileAndInsertToDatabase(HttpPostedFileBase file, long postId)
        //{
        //    string filePath = UploadImage(file);
        //    if (!string.IsNullOrEmpty(filePath))
        //    {
        //        // Thêm thông tin ảnh vào bảng liên quan
        //        AnhBaiDang newImage = new AnhBaiDang
        //        {
        //            IdBaiDang = postId,
        //            DuongDanAnh = filePath
        //        };

        //        db.AnhBaiDangs.InsertOnSubmit(newImage);
        //        db.SubmitChanges();
        //    }
        //}

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

        //            // Kiểm tra phòng trọ có sẵn
        //            var phongTro = db.PhongTros.FirstOrDefault(pt => pt.IdNguoiChoThue == idNguoiChoThue);
        //            if (phongTro == null)
        //            {
        //                return Json(new { success = false, message = "Không có phòng trọ có sẵn để đăng." });
        //            }

        //            // Tạo bài đăng mới
        //            BaiDang newBd = new BaiDang
        //            {
        //                IdNguoiChoThue = idNguoiChoThue,
        //                IdPhongTro = phongTro.IdPhongTro,
        //                TieuDe = model.TieuDe,
        //                NoiDung = model.NoiDung,
        //                NgayDang = DateTime.Now,
        //                TrangThai = "0",
        //                SoLuongYeuThich = 0,
        //                AnhBaiDang = model.DanhSachAnh != null && model.DanhSachAnh.Any() ? UploadImage(model.DanhSachAnh.First()) : "def_img.jpg"
        //            };

        //            // Thêm bài đăng vào cơ sở dữ liệu
        //            db.BaiDangs.InsertOnSubmit(newBd);
        //            db.SubmitChanges();
        //            long newPostId = newBd.IdBaiDang;

        //            // Lưu các ảnh khác (nếu có)
        //            if (model.DanhSachAnh != null && model.DanhSachAnh.Any())
        //            {
        //                foreach (var file in model.DanhSachAnh)
        //                {
        //                    SaveFileAndInsertToDatabase(file, newPostId); // Save ảnh và lưu vào DB
        //                }
        //            }

        //            res = new { success = true, message = "Đăng bài thành công!" };
        //        }
        //        catch (Exception ex)
        //        {
        //            res = new { success = false, message = "Đã xảy ra lỗi: " + ex.Message };
        //        }
        //    }

        //    return Json(res);
        //}
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase imageFile)
        {
            if (imageFile != null && imageFile.ContentLength > 0)
            {
                // Tạo tên file duy nhất
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

                // Lưu ảnh vào thư mục
                string path = Path.Combine(Server.MapPath("~/Contents/Images/ImgPost"), uniqueFileName);
                imageFile.SaveAs(path);

                // Trả về tên file đã lưu
                return Json(new
                {
                    success = true,
                    message = "Ảnh đã được tải lên thành công.",
                    savedFileName = uniqueFileName, // Lưu lại tên file duy nhất
                    filePath = "/Contents/Images/ImgPost/" + uniqueFileName
                });
            }

            return Json(new
            {
                success = false,
                message = "Không có ảnh nào được tải lên."
            });
        }


        [HttpPost]

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

                    // Lấy phòng trọ còn trống
                    var phongTro = db.PhongTros.FirstOrDefault(pt => pt.IdNguoiChoThue == idNguoiChoThue);
                    if (phongTro == null)
                    {
                        return Json(new { success = false, message = "Không có phòng trọ có sẵn để đăng." });
                    }

                    // Upload image and get the unique file name
                   

                    // Tạo bài đăng mới
                    BaiDang newBd = new BaiDang
                    {
                        IdNguoiChoThue = idNguoiChoThue,
                        IdPhongTro = phongTro.IdPhongTro,
                        TieuDe = model.TieuDe,
                        NoiDung = model.NoiDung,
                        NgayDang = DateTime.Now,
                        TrangThai = "0",
                        AnhBaiDang = !string.IsNullOrEmpty(model.AnhBaiDang) ? model.AnhBaiDang : "def_img.jpg"
                    };

                    // Thêm bài đăng vào cơ sở dữ liệu
                    db.BaiDangs.InsertOnSubmit(newBd);
                    db.SubmitChanges();
                    long newPostId = newBd.IdBaiDang;

                    // Lưu các ảnh khác (nếu có)
                    if (model.DanhSachAnh != null && model.DanhSachAnh.Any())
                    {
                        foreach (var file in model.DanhSachAnh)
                        {
                            UploadListImage(file, newPostId); // Save ảnh và lưu vào DB
                        }
                    }

                    res = new { success = true, message = "Đăng bài thành công!" };
                }
                catch (Exception ex)
                {
                    res = new { success = false, message = "Đã xảy ra lỗi: " + ex.Message };
                }
            }

            return Json(res);
        }

        //public string UploadImage(HttpPostedFileBase imageFile)
        //{
        //    if (imageFile != null && imageFile.ContentLength > 0)
        //    {
        //        // Đặt đường dẫn thư mục lưu ảnh
        //        string directoryPath = Server.MapPath("~/Contents/Images/ImgPost/");

        //        // Kiểm tra thư mục, nếu không tồn tại, tạo mới
        //        if (!Directory.Exists(directoryPath))
        //        {
        //            Directory.CreateDirectory(directoryPath);
        //        }

        //        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

        //        string filePath = Path.Combine(directoryPath, fileName);

        //        // Lưu ảnh vào thư mục
        //        imageFile.SaveAs(filePath);

        //        return "~/Contents/Images/ImgPost/" + fileName;
        //    }
        //    return null;
        //}







    }
}