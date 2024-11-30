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


        [HttpPost]
        public JsonResult Confirm(kiemduyetbaidang kd)
        {
            // Lấy username từ người dùng đang đăng nhập
            string username = User.Identity.Name.ToString();

            // Lấy IdNguoiDung dựa trên username
            long idKDV = db.NguoiDungs
                          .Where(u => u.TenTaiKhoan == username)
                          .Select(u => u.IdNguoiDung)
                          .FirstOrDefault();

            // Kiểm tra xem bài đăng đã được thích trước đó hay chưa
            

            // Xử lý thêm bài viết yêu thích
            if (ModelState.IsValid)
            {
                try
                {
                    // Tạo đối tượng YeuThich mới
                    kiemduyetbaidang newP = new kiemduyetbaidang
                    {
                        IdBaiDang = kd.IdBaiDang,
                        ngayduyet = DateTime.Now,
                        NoiDung = "Bài đăng của bạn đã được duyệt",
                        IdKDV = idKDV
                    };

                    // Thêm vào cơ sở dữ liệu
                    db.kiemduyetbaidangs.InsertOnSubmit(newP);

                    var bd = db.BaiDangs.Where(x => x.IdBaiDang == kd.IdBaiDang).FirstOrDefault();

                    if (bd != null)
                    {
                        bd.TrangThai = "1";
                        db.SubmitChanges();

                        return Json(new
                        {
                            success = true,
                            message = "Đã duyệt bài đăng!"
                        });
                    }
                    db.SubmitChanges();
                    
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


        [HttpPost]
        public JsonResult Reject(kiemduyetbaidang kd)
        {
            string username = User.Identity.Name.ToString();

            long idKDV = db.NguoiDungs
                          .Where(u => u.TenTaiKhoan == username)
                          .Select(u => u.IdNguoiDung)
                          .FirstOrDefault();
            if (ModelState.IsValid)
            {
                try
                {
                    kiemduyetbaidang newP = new kiemduyetbaidang
                    {
                        IdBaiDang = kd.IdBaiDang,
                        ngayduyet = DateTime.Now,
                        NoiDung = kd.NoiDung,
                        IdKDV = idKDV
                    };

                    db.kiemduyetbaidangs.InsertOnSubmit(newP);

                    db.SubmitChanges();

                    var bd = db.BaiDangs.Where(x => x.IdBaiDang == kd.IdBaiDang).FirstOrDefault();

                    if (bd != null)
                    {
                        bd.TrangThai = "2";
                        db.SubmitChanges();

                        return Json(new
                        {
                            success = true,
                            message = "Đã từ chối bài đăng! "
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


        [HttpPost]
        public JsonResult Delete(long postId)
        {
            
            try
            {
                // Kiểm tra xem bài đăng đã thích tồn tại chưa
                var bd = db.BaiDangs.FirstOrDefault(x => x.IdBaiDang == postId );

                if (bd != null)
                {
                    db.BaiDangs.DeleteOnSubmit(bd); // Xóa bản ghi
                    db.SubmitChanges();

                    return Json(new
                    {
                        success = true,
                        message = "Bài đăng đã được xóa."
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Không tìm thấy bài đăng trong danh sách."
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

        public ActionResult PhanHoi(string PHSearchType, string PHSearchInput, string sortCol, string sortType, int page = 1)
        {
            //IEnumerable<BaiDangVM> query = null;
            var query = (from PhanHoi in db.PhanHois
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
    }
}