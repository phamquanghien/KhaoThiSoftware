using System.Web.Mvc;

namespace KhaoThiSoftware.Areas.Lectures
{
    public class LecturesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Lectures";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Lectures_default",
                "Lectures/{controller}/{action}/{id}",
                new { controller = "Home_Le", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}