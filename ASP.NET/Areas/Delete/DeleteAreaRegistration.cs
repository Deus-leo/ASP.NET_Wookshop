using System.Web.Mvc;

namespace ASP.NET.Areas.Delete
{
    public class DeleteAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Delete";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Delete_default",
                "Delete/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
