using System.Web.Mvc;

namespace Web_PhongTro.Areas.KiemDuyetVien
{
    public class KiemDuyetVienAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "KiemDuyetVien";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "KiemDuyetVien_default",
                "KiemDuyetVien/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}