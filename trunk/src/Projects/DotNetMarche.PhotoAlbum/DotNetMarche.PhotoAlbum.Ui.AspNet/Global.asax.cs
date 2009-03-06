using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
         using (RouteTable.Routes.GetWriteLock())
         {
            RouteTable.Routes.Add(new Route("Photo/{photoid}.axd", new MyPhotoRouter()));
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