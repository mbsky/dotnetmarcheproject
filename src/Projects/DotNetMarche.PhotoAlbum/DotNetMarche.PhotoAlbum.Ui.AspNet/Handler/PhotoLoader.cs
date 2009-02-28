using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Handler
{
   public class PhotoLoader : IHttpHandler
   {
      public PhotoLoader(string photoId)
      {
         this.PhotoId = photoId;
      }

      public String PhotoId { get; set; }

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
         using (Stream image = Services.PhotoManagerService.GetImage(PhotoId))
         {
            Byte[] buffer = new byte[8192];
            Int32 readCount;
            while ((readCount = image.Read(buffer, 0, buffer.Length)) > 0) 
               context.Response.OutputStream.Write(buffer, 0, readCount);
            context.Response.OutputStream.Flush();
         }
      }

      #endregion
   }
}
