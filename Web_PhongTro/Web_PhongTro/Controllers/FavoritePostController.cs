using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_PhongTro.Models;
using Web_PhongTro.ViewModels;

namespace Web_PhongTro.Controllers
{
    public class FavoritePostController : Controller
    {
        // GET: FavoritePost
        PhongTroDataContext db = new PhongTroDataContext();

        public ActionResult Index()
        {
            return View();
        }
        string sessionFavoriteName = "SessionFavoritePost";
        public ActionResult FavoritePostPartial(string postSearchType, string postSearchInput, string sortCol, string sortType, int page = 1)
        {
            string username = User.Identity.Name.ToString();
            IEnumerable<BaiDangVM> query = null;

            query = (from baiDang in db.BaiDangs
                     join pt in db.PhongTros on baiDang.IdPhongTro equals pt.IdPhongTro
                     join dc in db.DiaChis on pt.IdDiaChi equals dc.IdDiaChi
                     join yt in db.YeuThiches on baiDang.IdBaiDang equals yt.IdBaiDang
                     join nt in db.NguoiThues on yt.IdNguoiThue equals nt.IdNguoiThue
                     join nd in db.NguoiDungs on nt.IdNguoiDung equals nd.IdNguoiDung
                     where username == nd.TenTaiKhoan
                     //orderby baiDang.IdBaiDang descending
                     select new BaiDangVM
                     {
                         BaiDang = baiDang,
                         noidungPT = pt,
                         diachiPT = dc
                         //Gia = db.func_GiaBaiDang(BaiDang.IdBaiDang)
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
        public ActionResult TotalPartial()
        {
            ViewBag.Total = Total();
            return PartialView();
        }
        public List<FavoritePostPartialVM> GetFavoritesList()
        {
            List<FavoritePostPartialVM> favoritesList = Session[sessionFavoriteName] as List<FavoritePostPartialVM>;
            if (favoritesList == null)
            {
                favoritesList = new List<FavoritePostPartialVM>();
                Session[sessionFavoriteName] = favoritesList;
            }
            return favoritesList;
        }
        private int Total()
        {
            int total = 0;
            List<FavoritePostPartialVM> favoritesList = GetFavoritesList();
            if (favoritesList != null)
            {
                total = favoritesList.Count;
            }
            return total;
        }

        [HttpPost]
        public JsonResult Add(int idBaiDang)
        {
            List<FavoritePostPartialVM> favoritesList = GetFavoritesList();

            if (favoritesList == null)
            {
                favoritesList = new List<FavoritePostPartialVM>();
            }

            if (favoritesList.Any(x => x.IdBaiDang == idBaiDang))
            {
                return Json(new
                {
                    status = false,
                    message = "Bạn đã thích sản phẩm này!"
                });
            }
            else
            {
                FavoritePostPartialVM newItem = new FavoritePostPartialVM(idBaiDang);
                favoritesList.Add(newItem);
            }

            Session[sessionFavoriteName] = favoritesList;

            return Json(new
            {
                status = true,
                message = "Đã thêm vào danh sách sản phẩm yêu thích!"
            });
        }


        [HttpPost]
        public JsonResult Delete(int idBaiDang)
        {
            List<FavoritePostPartialVM> favoritesList = GetFavoritesList();
            favoritesList.RemoveAll(x => x.IdBaiDang == idBaiDang);
            Session[sessionFavoriteName] = favoritesList;
            return Json(new
            {
                status = true
            });
        }
    }
}