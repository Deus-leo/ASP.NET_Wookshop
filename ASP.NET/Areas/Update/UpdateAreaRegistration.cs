using System.Web.Mvc;

namespace ASP.NET.Areas.Update
{
    public class UpdateAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Update";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Update_default",
                "Update/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
