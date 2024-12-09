using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_PhongTro.Models;
using Web_PhongTro.ViewModels;

namespace Web_PhongTro.Controllers
{
    public class FavoritePostController : Controller
    {
        // GET: FavoritePost
        PhongTroDataContext db = new PhongTroDataContext();

        public ActionResult Index()
        {
            return View();
        }
        string sessionFavoriteName = "SessionFavoritePost";
        public ActionResult FavoritePostPartial(string postSearchType, string postSearchInput, string sortCol, string sortType, int page = 1)
        {
            string username = User.Identity.Name.ToString();
            IEnumerable<BaiDangVM> query = null;

            query = (from baiDang in db.BaiDangs
                     join pt in db.PhongTros on baiDang.IdPhongTro equals pt.IdPhongTro
                     join dc in db.DiaChis on pt.IdDiaChi equals dc.IdDiaChi
                     join yt in db.YeuThiches on baiDang.IdBaiDang equals yt.IdBaiDang
                     join nd in db.NguoiDungs on yt.IdNguoiDung equals nd.IdNguoiDung
                     where username == nd.TenTaiKhoan
                     //orderby baiDang.IdBaiDang descending
                     select new BaiDangVM
                     {
                         BaiDang = baiDang,
                         noidungPT = pt,
                         diachiPT = dc,
                         slyt = db.YeuThiches.Count(yt => yt.IdBaiDang == baiDang.IdBaiDang)
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

        [HttpPost]
        public JsonResult Add(YeuThich yt)
        {
            // Lấy username từ người dùng đang đăng nhập
            string username = User.Identity.Name.ToString();

            // Lấy IdNguoiDung dựa trên username
            long idNT = db.NguoiDungs
                          .Where(u => u.TenTaiKhoan == username)
                          .Select(u => u.IdNguoiDung)
                          .FirstOrDefault();

            // Kiểm tra xem bài đăng đã được thích trước đó hay chưa
            if (db.YeuThiches.Any(x => x.IdBaiDang == yt.IdBaiDang && x.IdNguoiDung == idNT))
            {
                return Json(new
                {
                    success = false,
                    message = "Bạn đã thích bài đăng này!"
                });
            }

            // Xử lý thêm bài viết yêu thích
            else if (ModelState.IsValid)
            {
                try
                {
                    // Tạo đối tượng YeuThich mới
                    YeuThich newP = new YeuThich
                    {
                        IdBaiDang = yt.IdBaiDang,
                        IdNguoiDung = idNT
                    };

                    // Thêm vào cơ sở dữ liệu
                    db.YeuThiches.InsertOnSubmit(newP);
                    db.SubmitChanges();

                    return Json(new
                    {
                        success = true,
                        message = "Thêm vào yêu thích thành công!"
                    });
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Vui lòng đăng nhập!" 
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
            string username = User.Identity.Name.ToString();

            // Lấy IdNguoiDung từ username
            long idNT = db.NguoiDungs
                          .Where(u => u.TenTaiKhoan == username)
                          .Select(u => u.IdNguoiDung)
                          .FirstOrDefault();

            try
            {
                // Kiểm tra xem bài đăng đã thích tồn tại chưa
                var favorite = db.YeuThiches.FirstOrDefault(x => x.IdBaiDang == postId && x.IdNguoiDung == idNT);

                if (favorite != null)
                {
                    db.YeuThiches.DeleteOnSubmit(favorite); // Xóa bản ghi
                    db.SubmitChanges();

                    return Json(new
                    {
                        success = true,
                        message = "Bài đăng đã được xóa khỏi danh sách yêu thích."
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Không tìm thấy bài đăng trong danh sách yêu thích."
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

    }
}