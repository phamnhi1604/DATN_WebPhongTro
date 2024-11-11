using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_PhongTro.Areas.NguoiChoThue.Controllers
{
    public class NCTHomeController : Controller
    {
        // GET: NguoiChoThue/NCTHome
        public ActionResult DashBoard()
        {
            return View();
        }
    }
}