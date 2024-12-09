using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Web_PhongTro.Models;
using Web_PhongTro.ViewModels;
using System.Net.Mail;
using System.Net;

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
        public ActionResult Register(UserVM lg)
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
                            TenKhachHang = lg.Username,
                            Email = lg.Email
                        };
                        db.NguoiThues.InsertOnSubmit(nt);

                    }
                    else if (roleId == 3)
                    {
                        NguoiChoThue nct = new NguoiChoThue()
                        {
                            IdNguoiDung = newUserId,
                            TenNguoiChoThue = lg.Username,
                            Email = lg.Email
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
            return RedirectToAction("ResetPassword", "Account");
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
        public ActionResult SendResetPasswordOtp(string email)
        {
            var emailLowerCase = email.ToLower();
            // Tìm người dùng trong bảng NguoiChoThue dựa trên email
            var nguoiChoThue = db.NguoiChoThues.FirstOrDefault(nct => nct.Email.ToLower() == emailLowerCase);
            if (nguoiChoThue == null)
            {
                // Nếu không tìm thấy email trong NguoiChoThue, kiểm tra trong NguoiThue
                var nguoiThue = db.NguoiThues.FirstOrDefault(nt => nt.Email.ToLower() == emailLowerCase);
                if (nguoiThue == null)
                {
                    // Nếu không tìm thấy email trong cả hai bảng
                    ModelState.AddModelError("", "Email không tồn tại.");
                    return View();
                }

                // Tìm người dùng trong bảng NguoiDung liên kết với NguoiThue
                var user = db.NguoiDungs.FirstOrDefault(u => u.IdNguoiDung == nguoiThue.IdNguoiDung);
                if (user == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy người dùng liên kết với email này.");
                    return View();
                }
            }
            else
            {
                // Nếu tìm thấy trong bảng NguoiChoThue, kiểm tra người dùng liên kết
                var user = db.NguoiDungs.FirstOrDefault(u => u.IdNguoiDung == nguoiChoThue.IdNguoiDung);
                if (user == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy người dùng liên kết với email này.");
                    return View();
                }
            }

            // Tạo mã OTP ngẫu nhiên
            string otp = new Random().Next(100000, 999999).ToString();
            DateTime otpGeneratedAt = DateTime.Now;

            // Lấy lại người dùng từ bảng NguoiDung theo IdNguoiDung
            var currentUser = db.NguoiDungs.FirstOrDefault(u => u.IdNguoiDung == nguoiChoThue.IdNguoiDung);
            if (currentUser == null)
            {
                ModelState.AddModelError("", "Không tìm thấy người dùng liên kết với email này.");
                return View();
            }

            // Cập nhật OTP vào bảng Người Dùng
            currentUser.ResetPasswordOtp = otp;
            currentUser.OtpGeneratedAt = otpGeneratedAt;
            db.SubmitChanges();

            // Gửi OTP qua email (ví dụ sử dụng phương thức SendEmail)
            SendEmail(emailLowerCase, otp);

            return RedirectToAction("ResetPassword", "Account");
        }



        public void SendEmail(string toEmail, string otp)
        {
            string subject = "Mã OTP để đặt lại mật khẩu";
            string body = $"Mã OTP của bạn là: {otp}. Mã OTP này sẽ hết hạn trong 10 phút.";

            using (var client = new SmtpClient("smtp.example.com"))
            {
                var mailMessage = new MailMessage("no-reply@example.com", toEmail, subject, body);
                client.Send(mailMessage);
            }
        }

        public ActionResult ResetPassword()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string email, string otp, string newPassword, string confirmNewPassword)
        {
            if (newPassword != confirmNewPassword)
            {
                ModelState.AddModelError("", "Mật khẩu xác nhận không khớp.");
                return View();
            }

            // Tìm người dùng trong bảng NguoiChoThue dựa trên email
            var nguoiChoThue = db.NguoiChoThues.FirstOrDefault(nct => nct.Email == email);
            if (nguoiChoThue == null)
            {
                ModelState.AddModelError("", "Email không tồn tại.");
                return View();
            }

            // Tìm người dùng trong bảng NguoiDung liên kết với NguoiChoThue
            var user = db.NguoiDungs.FirstOrDefault(u => u.IdNguoiDung == nguoiChoThue.IdNguoiDung);
            if (user == null)
            {
                ModelState.AddModelError("", "Không tìm thấy người dùng liên kết với email này.");
                return View();
            }

            // Kiểm tra OTP hợp lệ và chưa hết hạn
            if (user.ResetPasswordOtp != otp || user.OtpGeneratedAt == null || user.OtpGeneratedAt.Value.AddMinutes(10) < DateTime.Now)
            {
                ModelState.AddModelError("", "Mã OTP không hợp lệ hoặc đã hết hạn.");
                return View();
            }

            // Cập nhật mật khẩu mới cho người dùng
            user.MatKhau = newPassword; // Mật khẩu mới
            user.ResetPasswordOtp = null; // Xóa OTP sau khi sử dụng
            user.OtpGeneratedAt = null; // Xóa thời gian hết hạn OTP
            db.SubmitChanges();

            return RedirectToAction("LoginV", "Account");
        }
        //[HttpPost]
        public JsonResult SendOtp(string email)
        {
            // Tìm email trong bảng NguoiChoThue
            var userChoThue = db.NguoiChoThues.SingleOrDefault(u => u.Email == email);

            // Nếu không tìm thấy email trong bảng NguoiChoThue, kiểm tra trong bảng NguoiThue
            if (userChoThue == null)
            {
                var userThue = db.NguoiThues.SingleOrDefault(u => u.Email == email);

                if (userThue == null)
                {
                    // Nếu email không tồn tại trong cả hai bảng
                    return Json(new { success = false, message = "Email không tồn tại" });
                }

                // Nếu tìm thấy trong bảng NguoiThue, lấy IdNguoiDung và tiếp tục xử lý
                var nguoiDung = db.NguoiDungs.SingleOrDefault(u => u.IdNguoiDung == userThue.IdNguoiDung);
                if (nguoiDung != null)
                {
                    // Lưu OTP vào bảng NguoiDung
                    string otp = new Random().Next(100000, 999999).ToString();
                    nguoiDung.ResetPasswordOtp = otp;
                    nguoiDung.OtpGeneratedAt = DateTime.Now;
                    db.SubmitChanges();

                    // Gửi mã OTP qua email
                    SendOtpEmail(userThue.Email, otp);
                }

                return Json(new { success = true });
            }

            // Nếu tìm thấy trong bảng NguoiChoThue, lấy IdNguoiDung và tiếp tục xử lý
            var nguoiDungChoThue = db.NguoiDungs.SingleOrDefault(u => u.IdNguoiDung == userChoThue.IdNguoiDung);
            if (nguoiDungChoThue != null)
            {
                // Lưu OTP vào bảng NguoiDung
                string otpChoThue = new Random().Next(100000, 999999).ToString();
                nguoiDungChoThue.ResetPasswordOtp = otpChoThue;
                nguoiDungChoThue.OtpGeneratedAt = DateTime.Now;
                db.SubmitChanges();

                // Gửi mã OTP qua email
                SendOtpEmail(userChoThue.Email, otpChoThue);
            }

            return Json(new { success = true });
        }


        // Gửi email với mã OTP
        public void SendOtpEmail(string toEmail, string otp)
        {
            string subject = "Mã OTP để đặt lại mật khẩu";
            string body = $"Mã OTP của bạn là: {otp}. Mã OTP này sẽ hết hạn trong 10 phút.";

            using (var client = new SmtpClient("smtp.gmail.com"))
            {
                client.Port = 587;  // Cổng cho Gmail
                client.EnableSsl = true;  // Bật SSL
                client.Credentials = new NetworkCredential("your-email@gmail.com", "your-email-password");

                var mailMessage = new MailMessage("your-email@gmail.com", toEmail, subject, body);

                try
                {
                    client.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu gửi mail không thành công
                    Console.WriteLine(ex.Message);
                }
            }
        }



        public JsonResult ForgotPassword(string email)
        {
            try
            {
                // Tìm email trong bảng NguoiChoThue
                var userEmail = db.NguoiChoThues.FirstOrDefault(x => x.Email == email);

                if (userEmail == null)
                {
                    return Json(new { success = false, message = "Email không tồn tại" });
                }

                // Lấy thông tin người dùng từ bảng NguoiDung thông qua IdNguoiDung
                var user = db.NguoiDungs.FirstOrDefault(x => x.IdNguoiDung == userEmail.IdNguoiDung);

                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tài khoản liên kết với email này" });
                }

                // Tạo mã OTP
                string otp = new Random().Next(100000, 999999).ToString();

                // Lưu OTP vào cơ sở dữ liệu (có thể thêm trường ResetPasswordOtp và OtpGeneratedAt nếu chưa có)
                user.ResetPasswordOtp = otp;
                user.OtpGeneratedAt = DateTime.Now;
                db.SubmitChanges();

                // Gửi email OTP
                using (var smtp = new SmtpClient("smtp.your-email-provider.com"))
                {
                    smtp.Credentials = new System.Net.NetworkCredential("your-email@example.com", "your-password");
                    smtp.EnableSsl = true;

                    var message = new MailMessage("your-email@example.com", email)
                    {
                        Subject = "Mã OTP đặt lại mật khẩu",
                        Body = $"Mã OTP của bạn là: {otp}. Vui lòng nhập mã này để đặt lại mật khẩu.",
                        IsBodyHtml = true
                    };
                    smtp.Send(message);
                }

                return Json(new { success = true, message = "Mã OTP đã được gửi đến email" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi gửi mã OTP: " + ex.Message });
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