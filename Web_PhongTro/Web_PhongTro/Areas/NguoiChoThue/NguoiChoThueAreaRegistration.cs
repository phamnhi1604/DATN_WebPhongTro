using System.Web.Mvc;

namespace Web_PhongTro.Areas.NguoiChoThue
{
    public class NguoiChoThueAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "NguoiChoThue";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "NguoiChoThue_default",
                "NguoiChoThue/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}