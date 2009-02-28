using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Handler;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet
{
   public class ApplicationInitialization
   {

      public static void AppInitialize()
      {
         using (RouteTable.Routes.GetWriteLock())
         {
            RouteTable.Routes.Add(new Route("photo/{photoid}.jpg", new MyPhotoRouter()));
         }
      }



   }
}
