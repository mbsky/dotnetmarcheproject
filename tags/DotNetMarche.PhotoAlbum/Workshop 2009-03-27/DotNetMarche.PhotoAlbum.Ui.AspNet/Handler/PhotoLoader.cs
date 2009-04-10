using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Services;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Handler
{
   public class PhotoLoader : IHttpHandler
   {


      #region IHttpHandler Members

      public bool IsReusable
      {
         get { return false; }
      }

      /// <summary>
      /// Stream back the image to the requestor.
      /// </summary>
      /// <param name="context"></param>
      public void ProcessRequest(HttpContext context)
      {
         RequestContext routeContext =  (RequestContext) HttpContext.Current.Items["routeContext"];
         String imageId = routeContext.RouteData.Values["photoid"].ToString();
         using (Stream image = PhotoManagerService.GetImage(imageId))
         {
            Byte[] buffer = new byte[8192];
            Int32 readCount;
            while ((readCount = image.Read(buffer, 0, buffer.Length)) > 0) 
               context.Response.OutputStream.Write(buffer, 0, readCount);
            context.Response.OutputStream.Flush();
         }
      }

      #endregion

      public static String GenerateLinkForPhoto(String fileName)
      {
         RouteValueDictionary rv = new RouteValueDictionary { { "photoid", fileName } };
         return RouteTable.Routes.GetVirtualPath(null, rv).VirtualPath;
      }
   }
}
