using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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


        //public ActionResult Info()
        //{
        //    string username = User.Identity.Name.ToString();
        //    var query = (from thongtin in db.NguoiThues
        //             join nd in db.NguoiDungs on thongtin.IdNguoiDung equals nd.IdNguoiDung
        //             where username == nd.TenTaiKhoan
        //             //orderby baiDang.IdBaiDang descending
        //             select new NguoiThueVM
        //             {  
        //                 NguoiThue = thongtin,
        //                 nguoiDung = nd
        //             }).FirstOrDefault();
        //    return View(query);
        //}
        public ActionResult Info1()
        {
            // Lấy tên tài khoản người dùng đang đăng nhập
            string username = User.Identity.Name;

            // Truy vấn thông tin người dùng và người thuê
            var query = (from nd in db.NguoiDungs
                         join nt in db.NguoiThues on nd.IdNguoiDung equals nt.IdNguoiDung into ntGroup
                         from nt in ntGroup.DefaultIfEmpty() // Đảm bảo join không bị null
                         where nd.TenTaiKhoan == username
                         select new NguoiThueVM(nt, nd)).FirstOrDefault();

            // Nếu không tìm thấy dữ liệu, trả về một đối tượng trống
            if (query == null)
            {
                query = new NguoiThueVM
                {
                    TenKhachHang = string.Empty,
                    SoDienThoai = string.Empty,
                    Email = string.Empty,
                    DiaChi = string.Empty,
                    Password = string.Empty
                };
            }

            // Trả về View cùng với ViewModel
            return View(query);
        }

        [HttpPost]
        public JsonResult UpdateInfo(NguoiThueVM nt)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string username = User.Identity.Name;

                    // Log debug
                    System.Diagnostics.Debug.WriteLine("Update for user: " + username);

                    var nguoiDung = db.NguoiDungs.FirstOrDefault(x => x.TenTaiKhoan == username);

                    if (nguoiDung != null)
                    {
                        var nguoiThue = db.NguoiThues.FirstOrDefault(x => x.IdNguoiDung == nguoiDung.IdNguoiDung);

                        if (nguoiThue != null)
                        {
                            if (!string.IsNullOrEmpty(nt.TenKhachHang))
                                nguoiThue.TenKhachHang = nt.TenKhachHang;

                            if (!string.IsNullOrEmpty(nt.SoDienThoai))
                                nguoiThue.SoDienThoai = nt.SoDienThoai;

                            if (!string.IsNullOrEmpty(nt.Email))
                                nguoiThue.Email = nt.Email;

                            if (!string.IsNullOrEmpty(nt.DiaChi))
                                nguoiThue.DiaChi = nt.DiaChi;

                            if (!string.IsNullOrEmpty(nt.Password) && nt.Password == nt.ConfirmPassword)
                            {
                                nguoiDung.MatKhau = nt.Password;
                            }

                            db.SubmitChanges(); // LINQ to SQL
                            return Json(new { success = true, message = "Cập nhật thông tin thành công!" });
                        }

                        return Json(new { success = false, message = "Không tìm thấy thông tin người thuê!" });
                    }

                    return Json(new { success = false, message = "Không tìm thấy thông tin người dùng!" });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                    return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
                }
            }

            return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
        }


    }
}