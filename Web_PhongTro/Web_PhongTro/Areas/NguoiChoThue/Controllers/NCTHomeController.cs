using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_PhongTro.Models;
using Web_PhongTro.ViewModels;

namespace Web_PhongTro.Areas.NguoiChoThue.Controllers
{
    public class NCTHomeController : Controller
    {
        PhongTroDataContext db = new PhongTroDataContext();
        // GET: NguoiChoThue/NCTHome
        public ActionResult Dashboard(string productSearchType, string sortCol, string sortType, int page = 1)
        {
            string username = User.Identity.Name.ToString();
            IEnumerable<BaiDangVM> query = null;
            
                query = (from baiDang in db.BaiDangs
                        join pt in db.PhongTros on baiDang.IdPhongTro equals pt.IdPhongTro
                        join dc in db.DiaChis on pt.IdDiaChi equals dc.IdDiaChi
                        join nct in db.NguoiChoThues on baiDang.IdNguoiChoThue equals nct.IdNguoiChoThue
                        join nd in db.NguoiDungs on nct.IdNguoiDung equals nd.IdNguoiDung
                        orderby baiDang.IdBaiDang descending
                        where username == nd.TenTaiKhoan

                         select new BaiDangVM
                        {
                            BaiDang = baiDang,
                            noidungPT = pt,
                            diachiPT = dc
                            
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

        public ActionResult Info()
        {
            return View();
        }
        public ActionResult AddPartial()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var lsp in db.DanhMucs.ToList())
            {
                items.Add(new SelectListItem
                {
                    Value = lsp.IdDanhMuc.ToString(),
                    Text = lsp.TenDanhMuc
                });
            }

            ViewBag.SPLoaiCha = items;

            return PartialView();
        }
        public ActionResult ChiTietBaiDang(int id)
        {
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
        [HttpPost] 
        public JsonResult Add(BaiDang bd)
        {
            var res = new { success = false, message = "Thêm sản phẩm không thành công" };
            BaiDang temp = bd;
            if (ModelState.IsValid)
            {
                try
                {
                    BaiDang newBd = new BaiDang()
                    {
                        TieuDe = bd.TieuDe
                    };
                    db.BaiDangs.InsertOnSubmit(newBd);
                    db.SubmitChanges();

                    res = new { success = true, message = "Thêm sản phẩm thành công" };
                }
                catch (Exception ex)
                {

                    res = new { success = false, message = "Đã xảy ra lỗi:" + ex.Message };
                }
            }

            return Json(res);
        }

        public ActionResult ThemBai()
        {
            return View();
        }
        public ActionResult EditPartial()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var lsp in db.DanhMucs.ToList())
            {
                items.Add(new SelectListItem
                {
                    Value = lsp.IdDanhMuc.ToString(),
                    Text = lsp.TenDanhMuc
                });
            }

            ViewBag.DanhMuc = items;
            return PartialView();
        }

        //public ActionResult GetDetails(long postId)
        //{
        //    long a = postId;
        //    var query = (from bd in db.BaiDangs
        //                 where bd.IdBaiDang == postId
        //                 select new BaiDangVM
        //                 {
        //                     BaiDang = bd
        //                 }).FirstOrDefault();
        //    long idDanhMuc = (from pt in db.PhongTros
        //                      join loaipt in db.DanhMucs on pt.IdDanhMuc equals loaipt.IdDanhMuc
        //                      where BaiDang. == postId
        //                      select loaiSanPham.IdLoaiSPCha).FirstOrDefault().Value;
        //    BaiDangVM product = new BaiDangVM(query);
        //    if (product != null)
        //    {
        //        return Json(new
        //        {
        //            productData = product,
        //            idLoaiCha = idLoaiCha
        //        }, JsonRequestBehavior.AllowGet);

        //    }

        //    return HttpNotFound();
        //}
    }
}