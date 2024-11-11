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
                bool existsUsername = db.NguoiDungs.Any(x => x.TenTaiKhoan == lg.Username);

                if (existsUsername)
                {
                    return Json(new { success = false, message = "Tài khoản đã tồn tại" });
                }
                else if (lg.Password == lg.ConfirmPassword)
                {
                    NguoiDung u = new NguoiDung()
                    {
                        TenTaiKhoan = lg.Username,
                        MatKhau = lg.Password,
                        IdVaiTro = 4,
                        TonTai = true
                    };

                    db.NguoiDungs.InsertOnSubmit(u);
                    db.SubmitChanges();

                    return Json(new
                    {
                        JsonRequestBehavior.AllowGet,
                        success = true,
                        message = "Đăng ký thành công"
                    });

                }
                else
                {
                    return Json(new { success = false, message = "Mật khẩu và xác nhận mật khẩu không khớp" });
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
                    return Json(new { success = true });
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
                ViewBag.EmployeeName = (from nguoiDung in db.NguoiDungs
                                        join nhanVien in db.KiemDuyetViens on nguoiDung.IdNguoiDung equals nhanVien.IdNguoiDung
                                        where nguoiDung.TenTaiKhoan == username
                                        select nhanVien.TenKDV).FirstOrDefault();
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
                    redirectUrlKT = Url.Action("Dashboard", "Home"),
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