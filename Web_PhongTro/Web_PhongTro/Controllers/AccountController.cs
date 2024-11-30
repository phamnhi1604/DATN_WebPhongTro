using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Web_PhongTro.Models;
using Web_PhongTro.ViewModels;

namespace Web_PhongTro.Controllers
{
    public class AccountController : Controller
    {
        PhongTroDataContext db = new PhongTroDataContext();
        // GET: Account
        public ActionResult LoginV()
        {
            return PartialView();
        }

        public ActionResult RegisterV()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Register(UserVM lg)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tài khoản đã tồn tại
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
                    int roleId = lg.AccountType == "Người cho thuê" ? 3 : 4;

                    // Tạo NguoiDung
                    NguoiDung u = new NguoiDung()
                    {
                        TenTaiKhoan = lg.Username,
                        MatKhau = lg.Password,
                        IdVaiTro = roleId,
                        TonTai = true
                    };

                    db.NguoiDungs.InsertOnSubmit(u);
                    db.SubmitChanges();

                    // Lấy IdNguoiDung mới được tạo
                    long newUserId = u.IdNguoiDung;

                    // Thêm bản ghi vào bảng tương ứng
                    if (roleId == 4)
                    {
                        NguoiThue nt = new NguoiThue()
                        {
                            IdNguoiDung = newUserId,
                            TenKhachHang = lg.Username
                        };
                        db.NguoiThues.InsertOnSubmit(nt);
                    }
                    else if (roleId == 3)
                    {
                        NguoiChoThue nct = new NguoiChoThue()
                        {
                            IdNguoiDung = newUserId,
                            TenNguoiChoThue = lg.Username
                        };
                        db.NguoiChoThues.InsertOnSubmit(nct);
                    }

                    db.SubmitChanges();

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
            else
            {
                // Trả về lỗi validation nếu ModelState không hợp lệ
                var validationErrors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                return Json(new { success = false, validationErrors });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Login(string userName, string password)
        {
            if (ModelState.IsValid)
            {
                
                var count = db.NguoiDungs.Count(x => x.TenTaiKhoan == userName && x.MatKhau == password);
                if (count > 0)
                {
                    FormsAuthentication.SetAuthCookie(userName, false);
                    return Json(new { success = true, UN = userName});
                }
                else
                {
                    return Json(new { success = false, message = "Đăng nhập thất bại!" });

                }

            }
            else
            {
                var validationErrors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                return Json(new { success = false, validationErrors });
            }
        }



        public ActionResult RoleAndNamePartial()
        {
            string username = User.Identity.Name.ToString();
            if (!string.IsNullOrEmpty(username))
            {
                ViewBag.Roles = GetRolesForUser(username)[0];
                if (ViewBag.Roles == "Admin")
                {
                    ViewBag.EmployeeName = "Phạm Yến Nhi";
                }
                else if (ViewBag.Roles == "Kiểm duyệt viên") {
                    ViewBag.EmployeeName = (from nguoiDung in db.NguoiDungs
                                            join nhanVien in db.KiemDuyetViens on nguoiDung.IdNguoiDung equals nhanVien.IdNguoiDung
                                            where nguoiDung.TenTaiKhoan == username
                                            select nhanVien.TenKDV).FirstOrDefault();
                }
                else if(ViewBag.Roles == "Người cho thuê") {
                    ViewBag.EmployeeName = (from nguoiDung in db.NguoiDungs
                                            join nct in db.NguoiChoThues on nguoiDung.IdNguoiDung equals nct.IdNguoiDung
                                            where nguoiDung.TenTaiKhoan == username
                                            select nct.TenNguoiChoThue).FirstOrDefault();
                }
                else {
                    ViewBag.EmployeeName = (from nguoiDung in db.NguoiDungs
                                            join nt in db.NguoiThues on nguoiDung.IdNguoiDung equals nt.IdNguoiDung
                                            where nguoiDung.TenTaiKhoan == username
                                            select nt.TenKhachHang).FirstOrDefault();
                }

                
            }
            else
            {
                ViewBag.Roles = string.Empty;
                ViewBag.EmployeeName = string.Empty;
            }
            return PartialView();
        }

        public string[] GetRolesForUser(string userName)
        {
            var role = (from nguoiDung in db.NguoiDungs
                        join vaiTro in db.VaiTros on nguoiDung.IdVaiTro equals vaiTro.IdVaiTro
                        where nguoiDung.TenTaiKhoan == userName
                        select vaiTro.TenVaiTro).FirstOrDefault();
            return string.IsNullOrEmpty(role) ? new string[0] : new string[] { role };
        }
        public JsonResult CheckAuthentication()
        {
            if (User.Identity.IsAuthenticated)
            {
                var roles = GetRolesForUser(User.Identity.Name.ToString());

                return Json(new
                {
                    isAuthenticated = true,
                    //isInRoleCus = roles.Contains("Khách thuê"),
                    isInRoleNCT = roles.Contains("Người cho thuê"),
                    isInRoleKDV = roles.Contains("Kiểm duyệt viên"),
                    isInRoleAdmin = roles.Contains("Admin"),
                    //isInRoleKT = roles.Contains("Khách thuê") ,
                    //isInRoleAdmin = !roles.Contains("Khách thuê") || !roles.Contains("Người cho thuê") || !roles.Contains("Kiểm duyệt viên"),

                    redirectUrlNCT = Url.Action("DashBoard", "NCTHome", new { area = "NguoiChoThue" }),
                    redirectUrlKDV = Url.Action("Dashboard", "KDVHome", new { area = "KiemDuyetVien" }),
                    redirectUrlKT = Url.Action("FavoritePostPartial", "FavoritePost"),
                    redirectUrlAdmin = Url.Action("DashBoard", "AdminHome", new { area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    isAuthenticated = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return Json(new { success = true, redirectUrl = "/" });
        }
    }
}