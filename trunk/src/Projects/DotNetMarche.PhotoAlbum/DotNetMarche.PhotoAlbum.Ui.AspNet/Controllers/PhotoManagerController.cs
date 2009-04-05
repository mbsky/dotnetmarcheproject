using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Helpers;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Models;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Controllers
{
   public class PhotoManagerController : MvcMasterController
   {
      private const Int32 defaultPageSize = 5;

      /// <summary>
      /// Manage album need to show the grid of the album with the data of
      /// eventually selected album
      /// </summary>
      /// <param name="id"></param>
      /// <param name="pageid"></param>
      /// <returns></returns>
      public ActionResult ManageAlbum(Guid? id, Int32? pageid, Int32? pagesize)
      {
         AlbumManager model = new AlbumManager();
         model.MainMenu = RootMenu;
         Guid curUserId = UserHelper.GetIdOfCurrentUser();
         if (pageid == null)
         {
            model.Albums = Services.PhotoManagerService.GetAll(curUserId, "Name", Int32.MaxValue, 0);
         } 
         else
         {
            model.Albums = Services.PhotoManagerService.GetAll(curUserId, "Name", pagesize ?? defaultPageSize, pageid.Value);
            model.CurrentPage = 0;
         }
         if (id != null)
         {
            model.PhotoForCurrentAlbum = Services.PhotoManagerService.GetAllPhotoForAlbum(id.Value);
         } else
         {
            model.PhotoForCurrentAlbum = new List<Model.Photo>();
         }

         model.TotalPages = (Int32)Math.Ceiling(Services.PhotoManagerService.GetAlbumCount(curUserId) / 5.0);
         return View(model);
      }

      public ActionResult GenerateEditAlbum(Guid albumId)
      {
         System.Threading.Thread.Sleep(2000);
         AlbumManager model = new AlbumManager();
         model.PhotoForCurrentAlbum = Services.PhotoManagerService.GetAllPhotoForAlbum(albumId);
         return View("AlbumEdit", model);
      }

      [AcceptVerbs("POST")]
      public JsonResult ChangePhotoDescription(Guid photoId, String newPhotoDescription)
      {
         System.Threading.Thread.Sleep(2000);
         return Json(Services.PhotoManagerService
            .ChangePhotoDescription(photoId, newPhotoDescription));
      }

      [AcceptVerbs("POST")]
      public JsonResult SwapPhotoPosition(Guid photoId1, Guid photoId2)
      {
         System.Threading.Thread.Sleep(2000);
         return Json(Services.PhotoManagerService
            .SwapPhotoPosition(photoId1, photoId2));
      }
   }


}
