using System.Web.Mvc;
using System.Web.Routing;

namespace TaxOverflow2013
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{question_id}",
                defaults: new { controller = "Home", action = "Index", question_id = UrlParameter.Optional }
            );
        }
    }
}
