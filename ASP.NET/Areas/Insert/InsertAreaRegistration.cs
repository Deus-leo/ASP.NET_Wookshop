using System.Web.Mvc;

namespace ASP.NET.Areas.Insert
{
    public class InsertAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Insert";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Insert_default",
                "Insert/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
