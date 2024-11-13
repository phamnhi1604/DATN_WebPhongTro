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
        public ActionResult Dashboard(string productSearchType, string productSearchInput, string sortCol, string sortType, int page = 1)
        {
            IEnumerable<BaiDangVM> query = null;
            if (!string.IsNullOrEmpty(productSearchType) && !string.IsNullOrEmpty(productSearchInput))
            {
                query = db.BaiDangs
                        .Where(p => p.TieuDe.Contains(productSearchInput))
                        .Select(baiDang => new BaiDangVM
                        {
                            BaiDang = baiDang
                            //Gia = db.func_GiaBaiDang(BaiDang.IdBaiDang)
                        });
            }
            else
            {
                query = (from baiDang in db.BaiDangs
                        join nd in db.PhongTros on baiDang.IdPhongTro equals nd.IdPhongTro
                        join dc in db.DiaChis on nd.IdDiaChi equals dc.IdDiaChi
                        orderby baiDang.IdBaiDang descending
                        select new BaiDangVM
                        {
                            BaiDang = baiDang,
                            noidungPT = nd,
                            diachiPT = dc
                            //Gia = db.func_GiaBaiDang(BaiDang.IdBaiDang)
                        });
            }



            // Paging
            int NoOfRecordPerPage = 12;
            int NoOfPages = (int)Math.Ceiling((double)query.Count() / NoOfRecordPerPage);
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.STT = (page - 1) * NoOfRecordPerPage + 1;
            ViewBag.NoOfPages = NoOfPages;
            query = query.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage);
            if (!string.IsNullOrEmpty(sortCol) && !string.IsNullOrEmpty(sortType))
            {
                //switch (sortCol)
                //{
                //    case "GiaDaGiam":
                //        if (sortType == "ASC")
                //            query = query.OrderBy(x => x.Gia);
                //        else if (sortType == "DESC")
                //            query = query.OrderByDescending(x => x.Gia);
                //        else return HttpNotFound();
                //        break;
                //    case "GiaGoc":
                //        if (sortType == "ASC")
                //            query = query.OrderBy(x => x.BaiDang.GiaBan);
                //        else if (sortType == "DESC")
                //            query = query.OrderByDescending(x => x.BaiDang.GiaBan);
                //        else return HttpNotFound();
                //        break;
                //    case "GiamGia":
                //        if (sortType == "ASC")
                //            query = query.OrderBy(x => x.BaiDang.GiamGia);
                //        else if (sortType == "DESC")
                //            query = query.OrderByDescending(x => x.BaiDang.GiamGia);
                //        else return HttpNotFound();
                //        break;
                //    case "SoLuongDanhGia":
                //        if (sortType == "ASC")
                //            query = query.OrderBy(x => x.BaiDang.SoLuongDanhGia);
                //        else if (sortType == "DESC")
                //            query = query.OrderByDescending(x => x.BaiDang.SoLuongDanhGia);
                //        else return HttpNotFound();
                //        break;
                //    default:
                //        return HttpNotFound();
                //}
            }

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
    }
}