using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_PhongTro.Areas.KiemDuyetVien.Controllers
{
    public class KDVHomeController : Controller
    {
        // GET: KiemDuyetVien/KDVHome
        public ActionResult DashBoard()
        {
            return View();
        }

    }
}