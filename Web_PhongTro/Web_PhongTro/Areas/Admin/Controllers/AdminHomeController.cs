using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_PhongTro.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        // GET: Admin/AdminHome
        public ActionResult DashBoard()
        {
            return View();
        }
    }
}