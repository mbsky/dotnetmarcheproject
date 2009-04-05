using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Helpers;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Handler
{
   public class AvatarHandler : IHttpHandler
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
         String fileName = "~/Avatara/" + UserHelper.GetIdOfCurrentUser().ToString() + ".jpg";
         context.Response.TransmitFile(fileName);
      }

      #endregion
   }
}
