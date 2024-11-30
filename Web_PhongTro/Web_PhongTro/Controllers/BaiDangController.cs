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
using System.Collections;

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
        public ActionResult SideBarPosts()
        {
            IEnumerable<BaiDangVM> query = null;
            query = (from danhMuc in db.DanhMucs
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

                         }).Take(1) ;
            return View(query);
        }

        public ActionResult GetDsBaiDangByQuan(string tenDM, int page = 1)
        {
            if (tenDM == null)
            {
                return RedirectToAction("Index", "Home");
            }
                var query = (from phongTro in db.PhongTros
                             join dc in db.DiaChis on phongTro.IdDiaChi equals dc.IdDiaChi
                             join baiDang in db.BaiDangs on phongTro.IdPhongTro equals baiDang.IdPhongTro
                             where baiDang.TrangThai == "1"
                             where dc.Quan == tenDM
                             orderby baiDang.IdBaiDang descending
                             select new BaiDangVM
                             {
                                 noidungPT = phongTro,
                                 BaiDang = baiDang,
                                 diachiPT = dc
                             });
                @ViewBag.TenDC = tenDM;
                int NoOfRecordPerPage = 12;
                int NoOfPages = (int)Math.Ceiling((double)query.Count() / NoOfRecordPerPage);
                int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
                ViewBag.Page = page;
                ViewBag.NoOfPages = NoOfPages;
                query = query.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage);
                return View(query);
            
        }


        //public ActionResult GetDsBaiDangByQuan(string tenQuan, int page = 1)
        //{
        //    if (tenQuan == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    else
        //    {
        //        var query = (from baiDang in db.BaiDangs
        //                     join nd in db.PhongTros on baiDang.IdPhongTro equals nd.IdPhongTro
        //                     join dc in db.DiaChis on nd.IdDiaChi equals dc.IdDiaChi
        //                     join ddA in db.AnhBaiDangs on baiDang.IdBaiDang equals ddA.IdBaiDang
        //                     join thongtinNguoiChoThue in db.NguoiChoThues on baiDang.IdNguoiChoThue equals thongtinNguoiChoThue.IdNguoiChoThue
        //                     where baiDang.TrangThai == "1"
        //                     where dc.Quan == tenQuan

        //                     select new BaiDangVM   
        //                     {
        //                         BaiDang = baiDang,
        //                         noidungPT = nd,
        //                         diachiPT = dc,
        //                         nguoiChoThue = thongtinNguoiChoThue,
        //                         listDdAnh = db.AnhBaiDangs
        //                               .Where(a => a.IdBaiDang == baiDang.IdBaiDang)
        //                               .Select(a => a.DuongDanAnh)
        //                               .ToList()

        //                     });
        //        @ViewBag.TenDC = tenQuan;
        //        int NoOfRecordPerPage = 12;
        //        int NoOfPages = (int)Math.Ceiling((double)query.Count() / NoOfRecordPerPage);
        //        int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
        //        ViewBag.Page = page;
        //        ViewBag.NoOfPages = NoOfPages;
        //        query = query.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage);
        //        return View(query);
        //    }
        //}


        public ActionResult GetDsBaiDangByTenDanhMuc(string tenDM, int page = 1)
        {
            if (tenDM == null)
            {
                return HttpNotFound();
            }
            else
            {
                var query = (from baiDang in db.BaiDangs
                             join nd in db.PhongTros on baiDang.IdPhongTro equals nd.IdPhongTro
                             join dm in db.DanhMucs on nd.IdDanhMuc equals dm.IdDanhMuc
                             join dc in db.DiaChis on nd.IdDiaChi equals dc.IdDiaChi
                             join ddA in db.AnhBaiDangs on baiDang.IdBaiDang equals ddA.IdBaiDang
                             join thongtinNguoiChoThue in db.NguoiChoThues on baiDang.IdNguoiChoThue equals thongtinNguoiChoThue.IdNguoiChoThue
                             where baiDang.TrangThai == "1"
                             where dm.TenDanhMuc == tenDM

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

                             });
                @ViewBag.TenDM = tenDM;
                int NoOfRecordPerPage = 12;
                int NoOfPages = (int)Math.Ceiling((double)query.Count() / NoOfRecordPerPage);
                int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
                ViewBag.Page = page;
                ViewBag.NoOfPages = NoOfPages;
                query = query.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage);
                return View(query);
            }
        }

        public ActionResult GetDsBaiDangByGia(decimal? minGia, decimal? maxGia, int page = 1)
        {
            
            var query = (from phongTro in db.PhongTros
                         join dc in db.DiaChis on phongTro.IdDiaChi equals dc.IdDiaChi
                         join baiDang in db.BaiDangs on phongTro.IdPhongTro equals baiDang.IdPhongTro
                         where baiDang.TrangThai == "1"
                         where (!minGia.HasValue || phongTro.GiaPhong >= minGia)
                         where (!maxGia.HasValue || phongTro.GiaPhong <= maxGia)
                         orderby baiDang.IdBaiDang descending
                         select new BaiDangVM
                         {
                             noidungPT = phongTro,
                             BaiDang = baiDang,
                             diachiPT = dc
                             
                         });
            int NoOfRecordPerPage = 12;
            int NoOfPages = (int)Math.Ceiling((double)query.Count() / NoOfRecordPerPage);
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            query = query.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage);
            return View(query);

        }

        public ActionResult GetDsBaiDangByDienTich(decimal? minDienTich, decimal? maxDienTich, int page = 1)
        {
            // Truy vấn lọc bài đăng theo diện tích
            var query = (from phongTro in db.PhongTros
                         join dc in db.DiaChis on phongTro.IdDiaChi equals dc.IdDiaChi
                         join baiDang in db.BaiDangs on phongTro.IdPhongTro equals baiDang.IdPhongTro
                         where baiDang.TrangThai == "1"
                         where (!minDienTich.HasValue || phongTro.DienTich >= minDienTich)
                         where (!maxDienTich.HasValue || phongTro.DienTich <= maxDienTich)
                         orderby baiDang.IdBaiDang descending
                         select new BaiDangVM
                         {
                             noidungPT = phongTro,
                             BaiDang = baiDang,
                             diachiPT = dc
                         });

            // Tính toán phân trang
            int NoOfRecordPerPage = 12;
            int NoOfPages = (int)Math.Ceiling((double)query.Count() / NoOfRecordPerPage);
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;

            // Gán thông tin phân trang
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;

            // Trả kết quả phân trang
            var paginatedResult = query.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            return View(paginatedResult);
        }


    }
}