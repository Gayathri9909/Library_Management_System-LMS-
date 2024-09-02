using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace library
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "Admin",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Admin", action = "Adminhome", id = UrlParameter.Optional }
          );
            routes.MapRoute(
            name: "User",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "User", action = "User", id = UrlParameter.Optional }
        );
           
            routes.MapRoute(
              name: "AddUser",
              url: "Home/Signin/{id}",
              defaults: new { controller = "Home", action = "AddUser", id = UrlParameter.Optional }
          );

            routes.MapRoute(
      name: "Delete",
      url: "Admin/Delete/{email}",
      defaults: new { controller = "Admin", action = "Delete" }
  );

            routes.MapRoute(
             name: "AdminLogout",
             url: "Admin/Logout",
              defaults: new { controller = "Admin", action = "Logout" }
 );



        }

    }
}
