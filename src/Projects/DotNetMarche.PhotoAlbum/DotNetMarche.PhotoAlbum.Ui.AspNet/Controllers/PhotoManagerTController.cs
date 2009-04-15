using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Helpers;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Models;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Controllers
{
   public class PhotoManagerTController : Controller
   {
      private const Int32 defaultPageSize = 5;

      /// <summary>
      /// Create the shell.
      /// </summary>
      /// <param name="id"></param>
      /// <param name="pageid"></param>
      /// <param name="pagesize"></param>
      /// <returns></returns>
      public ActionResult ManageAlbum(Guid? id, Int32? pageid, Int32? pagesize)
      {
         AlbumManager model = new AlbumManager();
         Guid curUserId = UserHelper.GetIdOfCurrentUser();
         return View(model);
      }     
   }


}
