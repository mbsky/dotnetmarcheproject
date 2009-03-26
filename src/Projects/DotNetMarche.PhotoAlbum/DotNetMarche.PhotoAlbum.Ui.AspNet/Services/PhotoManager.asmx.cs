using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Services
{
   /// <summary>
   /// Summary description for PhotoManager
   /// </summary>
   [WebService(Namespace = "http://dotnetmarche.org/photoalbum")]
   [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
   [System.ComponentModel.ToolboxItem(false)]
   [System.Web.Script.Services.ScriptService]
   public class PhotoManager : System.Web.Services.WebService
   {

      [WebMethod]
      public  bool CreateOrUpdatePhotoAlbum(Model.PhotoAlbum album)
      {
         return PhotoManagerService.CreateOrUpdatePhotoAlbum(album);
      }

      [WebMethod]
      public  bool AddPhotoToAlbum(string fileName, String originalFileName, Guid albumId)
      {
         return PhotoManagerService.AddPhotoToAlbum(fileName, originalFileName, albumId);
      }

      [WebMethod]
      public  System.IO.Stream GetImage(string imageFileId)
      {
         return PhotoManagerService.GetImage(imageFileId);
      }

      [WebMethod]
      public  List<Model.PhotoAlbum> GetAll(Guid userId)
      {
         return PhotoManagerService.GetAll(userId).ToList();
      }

      [WebMethod]
      public  List<Model.Photo> GetAllPhotoForAlbum(Guid albumId)
      {
         return PhotoManagerService.GetAllPhotoForAlbum(albumId).ToList();
      }

      [WebMethod]
      public  Model.PhotoAlbum GetPhotoAlbumWithPhoto(Guid albumId)
      {
         return PhotoManagerService.GetPhotoAlbumWithPhoto(albumId);
      }

      [WebMethod]
      public  Boolean MovePhotoBack(Guid photoId)
      {
         return PhotoManagerService.MovePhotoBack(photoId);
      }

      [WebMethod]
      public  Boolean MovePhotoForward(Guid photoId)
      {
         return PhotoManagerService.MovePhotoForward(photoId);
      }

      [WebMethod]
      public  Boolean ChangePhotoDescription(Guid photoId, String newDescription)
      {
         Thread.Sleep(4000);
         return PhotoManagerService.ChangePhotoDescription(photoId, newDescription);
      }

      [WebMethod]
      public  Boolean SwapPhotoPosition(Guid photoId1, Guid photoId2)
      {
         return PhotoManagerService.SwapPhotoPosition(photoId1, photoId2);
      }
   }
}
