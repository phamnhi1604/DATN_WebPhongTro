using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_PhongTro.Models;
using Web_PhongTro.ViewModels;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace Web_PhongTro.Controllers
{
    public class BaiDangController : Controller
    {
        PhongTroDataContext db = new PhongTroDataContext();
        // GET: BaiDang
        public ActionResult BaiDang()
        {
            return View();
        }

        public ActionResult BaiDangNoiBat(int page = 1)
        {

            return View();
        }

        public ActionResult GetBaiDangById( int? id)
        {
            if(id == 0 || id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var query = (from baiDang in db.BaiDangs
                         join nd in db.PhongTros on baiDang.IdPhongTro equals nd.IdPhongTro
                         join dc in db.DiaChis on nd.IdDiaChi equals dc.IdDiaChi
                         join ddA in db.AnhBaiDangs on baiDang.IdBaiDang equals ddA.IdBaiDang
                         join thongtinNguoiChoThue in db.NguoiChoThues on baiDang.IdNguoiChoThue equals thongtinNguoiChoThue.IdNguoiChoThue
                         where baiDang.IdBaiDang == id
                         select new BaiDangVM
                         {
                             BaiDang = baiDang,
                             noidungPT = nd,
                             diachiPT = dc,
                             nguoiChoThue = thongtinNguoiChoThue,
                             listDdAnh = db.AnhBaiDangs
                                       .Where(a => a.IdBaiDang == baiDang.IdBaiDang)
                                       .Select(a => a.DuongDanAnh)
                                       .ToList()

                         }).Take(1);
            return View(query);
        }

        public async Task<(double latitude, double longitude)> GetCoordinatesFromAddress(string address)
        {
            using (var client = new HttpClient())
            {
                var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json&limit=1";
                var response = await client.GetStringAsync(url);
                var data = JsonConvert.DeserializeObject<List<dynamic>>(response);
                if (data.Count > 0)
                {
                    var latitude = (double)data[0].lat;
                    var longitude = (double)data[0].lon;
                    return (latitude, longitude);
                }
                throw new Exception("Không tìm thấy tọa độ cho địa chỉ này.");
            }
        }
        public ActionResult GetBaiDangByDiaChi(int? id)
        {
            if (id == 0 || id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var query = (from baiDang in db.BaiDangs
                         join nd in db.PhongTros on baiDang.IdPhongTro equals nd.IdPhongTro
                         join dc in db.DiaChis on nd.IdDiaChi equals dc.IdDiaChi
                         join ddA in db.AnhBaiDangs on baiDang.IdBaiDang equals ddA.IdBaiDang
                         join thongtinNguoiChoThue in db.NguoiChoThues on baiDang.IdNguoiChoThue equals thongtinNguoiChoThue.IdNguoiChoThue
                         where baiDang.IdBaiDang == id
                         select new BaiDangVM
                         {
                             BaiDang = baiDang,
                             noidungPT = nd,
                             diachiPT = dc,
                             nguoiChoThue = thongtinNguoiChoThue,
                             listDdAnh = db.AnhBaiDangs
                                       .Where(a => a.IdBaiDang == baiDang.IdBaiDang)
                                       .Select(a => a.DuongDanAnh)
                                       .ToList()

                         }).Take(1);
            return View(query);
        }
    }
}