using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_PhongTro.Models;
using Web_PhongTro.Areas.NguoiChoThue.ViewModels;
using Web_PhongTro.ViewModels;
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
        //public ActionResult GetDanhSachPhongTro()
        //{
        //    string username = User.Identity.Name;

        //    var danhSachPhongTro = (from pt in db.PhongTros
        //                            join nct in db.NguoiChoThues on pt.IdNguoiChoThue equals nct.IdNguoiChoThue
        //                            join nd in db.NguoiDungs on nct.IdNguoiDung equals nd.IdNguoiDung
        //                            where nd.TenTaiKhoan == username
        //                            select pt.MoTa).ToList();

        //    PhongTroViewModel viewModel = new PhongTroViewModel
        //    {
        //        MotaPT = danhSachPhongTro
        //    };

        //    return View(viewModel);
        //}


        public ActionResult GetDanhSachPhongTro()
        {
            string username = User.Identity.Name;

            // Lấy danh sách phòng trọ của người cho thuê dựa trên tài khoản người dùng
            var danhSachPhongTro = (from pt in db.PhongTros
                                    join nct in db.NguoiChoThues on pt.IdNguoiChoThue equals nct.IdNguoiChoThue
                                    join nd in db.NguoiDungs on nct.IdNguoiDung equals nd.IdNguoiDung
                                    where nd.TenTaiKhoan == username
                                    select pt).ToList();

            // Trả về view với danh sách phòng trọ
            PhongTroViewModel viewModel = new PhongTroViewModel
            {
                MotaPT = danhSachPhongTro
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
        public PartialViewResult DangBai()
        {
            return PartialView();
        }

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

        //            // Lấy phòng trọ còn trống
        //            //var phongTro = db.PhongTros.FirstOrDefault(pt => pt.MoTa == model.ndPT.MoTa);
        //            //var phongTro = db.PhongTros
        //            //              .Where(pt => pt.MoTa == model.mota)
        //            //              .Select(pt => pt.IdPhongTro)
        //            //              .FirstOrDefault();
        //            var phongtro = new PhongTro
        //            {
        //                MoTa = model.ndPT.MoTa
        //            };
        //            //if (model.ndPT == null)
        //            //{
        //            //    return Json(new { success = false, message = "Không có phòng trọ có sẵn để đăng." });
        //            //}

        //            // Tạo bài đăng mới
        //            BaiDang newBd = new BaiDang
        //            {
        //                IdNguoiChoThue = idNguoiChoThue,
        //                IdPhongTro = model.ndPT.IdPhongTro,
        //                TieuDe = model.TieuDe,
        //                NoiDung = model.NoiDung,
        //                NgayDang = DateTime.Now,
        //                TrangThai = "0",
        //                AnhBaiDang = !string.IsNullOrEmpty(model.AnhBaiDang) ? model.AnhBaiDang : "def_img.jpg"
        //            };

        //            // Thêm bài đăng vào cơ sở dữ liệu
        //            db.BaiDangs.InsertOnSubmit(newBd);
        //            db.SubmitChanges();

        //            // Lấy ID bài đăng mới tạo
        //            long newPostId = newBd.IdBaiDang;

        //            // Thêm danh sách ảnh vào bảng AnhBaiDang
        //            if (model.DanhSachAnh != null && model.DanhSachAnh.Any())
        //            {
        //                foreach (var anh in model.DanhSachAnh)
        //                {
        //                    AnhBaiDang anhBd = new AnhBaiDang
        //                    {
        //                        IdBaiDang = newPostId,
        //                        DuongDanAnh = anh
        //                    };
        //                    db.AnhBaiDangs.InsertOnSubmit(anhBd);
        //                }
        //                db.SubmitChanges();
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

            // Kiểm tra IdPhongTro và lấy phòng trọ từ database
            var phongTro = db.PhongTros.FirstOrDefault(pt => pt.IdPhongTro == model.IdPhongTro);
            if (phongTro == null)
            {
                return Json(new { success = false, message = "Phòng trọ không tồn tại." });
            }

            // Tạo bài đăng mới
            BaiDang newBd = new BaiDang
            {
                IdNguoiChoThue = idNguoiChoThue,
                IdPhongTro = model.IdPhongTro,  // Gắn IdPhongTro vào bài đăng
                TieuDe = model.TieuDe,
                NoiDung = model.NoiDung,
                NgayDang = DateTime.Now,
                TrangThai = "0",
                AnhBaiDang = !string.IsNullOrEmpty(model.AnhBaiDang) ? model.AnhBaiDang : "def_img.jpg"
            };

            // Thêm bài đăng vào cơ sở dữ liệu
            db.BaiDangs.InsertOnSubmit(newBd);
            db.SubmitChanges();

            // Lấy ID bài đăng mới tạo
            long newPostId = newBd.IdBaiDang;

            // Thêm danh sách ảnh vào bảng AnhBaiDang
            if (model.DanhSachAnh != null && model.DanhSachAnh.Any())
            {
                foreach (var anh in model.DanhSachAnh)
                {
                    AnhBaiDang anhBd = new AnhBaiDang
                    {
                        IdBaiDang = newPostId,
                        DuongDanAnh = anh
                    };
                    db.AnhBaiDangs.InsertOnSubmit(anhBd);
                }
                db.SubmitChanges();
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


        [HttpPost]

        public JsonResult UploadListImages(IEnumerable<HttpPostedFileBase> imageFiles)
        {
            var uploadedImageDetails = new List<string>();
            var NF = new List<string>();
            try
            {
                if (imageFiles != null && imageFiles.Any())
                {
                    foreach (var imageFile in imageFiles)
                    {
                        if (imageFile != null && imageFile.ContentLength > 0)
                        {
                            // Tạo tên file duy nhất
                            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                            
                            // Lưu ảnh vào thư mục
                            string path = Path.Combine(Server.MapPath("~/Contents/Images/ImgPost"), uniqueFileName);
                            imageFile.SaveAs(path);

                            // Thêm đường dẫn ảnh vào danh sách kết quả
                            uploadedImageDetails.Add("/Contents/Images/ImgPost/" + uniqueFileName);
                            NF.Add(uniqueFileName);
                        }
                    }
                }

                return Json(new
                {
                    success = true,
                    message = "Tải lên ảnh thành công!",
                    namefile = NF,
                    filePaths = uploadedImageDetails
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Lỗi khi tải lên ảnh: " + ex.Message
                });
            }
        }

        public JsonResult PhongTro(PhongTroViewModel model)
        {
            var res = new { success = false, message = "Phòng Trọ đăng không thành công" };

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

                    var diaChiMoi = new DiaChi
                    {
                        ThanhPho = model.ThanhPho,
                        Quan = model.Quan,
                        Phuong = model.Phuong,
                        Duong = model.Duong,
                        QuocGia = "Việt Nam"
                    };
                    db.DiaChis.InsertOnSubmit(diaChiMoi);
                    db.SubmitChanges();

                    // Kiểm tra danh mục
                    var danhMuc = db.DanhMucs.FirstOrDefault(dm => dm.IdDanhMuc == model.LoaiChuyenMuc);
                    if (danhMuc == null)
                    {
                        return Json(new { success = false, message = "Danh mục không tồn tại." });
                    }

                    var phongTroMoi = new PhongTro
                    {
                        IdNguoiChoThue = idNguoiChoThue,
                        IdDanhMuc = model.LoaiChuyenMuc,
                        DienTich = model.DienTich,
                        GiaPhong = model.GiaPhong,
                        MoTa = model.MoTa,
                        IdDiaChi = diaChiMoi.IdDiaChi,
                        TrangThaiPhong = "Con_trong" // Hoặc trạng thái mặc định
                    };
                    db.PhongTros.InsertOnSubmit(phongTroMoi);
                    db.SubmitChanges();
                    res = new { success = true, message = "Thêm phòng trọ thành công!" };
                }
                catch (Exception ex)
                {
                    res = new { success = false, message = "Đã xảy ra lỗi: " + ex.Message };
                }
            }

            return Json(res);
        }

        [HttpPost]
        public JsonResult UpdatePhongTro(int id, string moTa, decimal dienTich, decimal giaPhong, string diaChi)
        {
            try
            {
                // Tìm phòng theo ID
                var phongTro = db.PhongTros.FirstOrDefault(p => p.IdPhongTro == id);
                if (phongTro == null)
                {
                    return Json(new { success = false, message = "Phòng trọ không tồn tại!" });
                }

                // Cập nhật thông tin phòng
                phongTro.MoTa = moTa;
                phongTro.DienTich = dienTich;
                phongTro.GiaPhong = giaPhong;
                //phongTro.DiaChi = diaChi;

                // Lưu thay đổi
                db.SubmitChanges();

                return Json(new { success = true, message = "Cập nhật phòng trọ thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }


        public JsonResult Delete(int id)
        {
            var response = new { success = false, message = "Xóa phòng trọ không thành công." };

            try
            {
                var phongTro = db.PhongTros.FirstOrDefault(pt => pt.IdPhongTro == id);
                if (phongTro == null)
                {
                    response = new { success = false, message = "Phòng trọ không tồn tại." };
                }
                else
                {
                    db.PhongTros.DeleteOnSubmit(phongTro);
                    db.SubmitChanges();
                    response = new { success = true, message = "Phòng trọ đã được xóa thành công." };
                }
            }
            catch (Exception ex)
            {
                response = new { success = false, message = "Đã xảy ra lỗi: " + ex.Message };
            }

            return Json(response);
        }
    }
}
