using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Handler
{
   public class MyPhotoRouter : IRouteHandler
   {
      #region IRouteHandler Members

      public IHttpHandler GetHttpHandler(RequestContext requestContext)
      {
         HttpContext.Current.Items["routeContext"] = requestContext;
         return new PhotoLoader();
      }

      #endregion
   }

   public class MyAvatarRouter : IRouteHandler
   {
      #region IRouteHandler Members

      public IHttpHandler GetHttpHandler(RequestContext requestContext)
      {
         return new AvatarHandler();
      }

      #endregion
   }
}
