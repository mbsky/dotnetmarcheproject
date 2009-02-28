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
         String imageId = requestContext.RouteData.Values["photoid"].ToString();
         return new PhotoLoader(imageId);
      }

      #endregion
   }
}
