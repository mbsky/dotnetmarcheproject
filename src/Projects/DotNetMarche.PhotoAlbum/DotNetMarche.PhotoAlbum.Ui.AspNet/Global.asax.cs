﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Handler;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet
{
   public class Global : System.Web.HttpApplication
   {

      protected void Application_Start(object sender, EventArgs e)
      {
        RegisterRoutes();
      }

      public static void RegisterRoutes()
      {
         using (RouteTable.Routes.GetWriteLock())
         {
            RouteTable.Routes.Add(new Route("Photo/{photoid}.axd", new MyPhotoRouter()));
            RouteTable.Routes.Add(new Route("Avatar.axd", new MyAvatarRouter()));
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");
            RouteTable.Routes.MapRoute(
               "Default",
               // Route name
               "{controller}/{action}/{id}",
               // URL with parameters
               new { controller = "Home", action = "Index", id = "" }
               // Parameter defaults
             );
         }
        
      }

      protected void Session_Start(object sender, EventArgs e)
      {

      }

      protected void Application_BeginRequest(object sender, EventArgs e)
      {

      }

      protected void Application_AuthenticateRequest(object sender, EventArgs e)
      {

      }

      protected void Application_Error(object sender, EventArgs e)
      {

      }

      protected void Session_End(object sender, EventArgs e)
      {

      }

      protected void Application_End(object sender, EventArgs e)
      {

      }
   }
}